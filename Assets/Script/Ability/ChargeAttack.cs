using UnityEngine;

/// <summary>
/// 攻击形状类型枚举
/// </summary>
public enum AttackShapeType
{
    Rectangle,  // 矩形
    Circle      // 圆形 AOE
}

/// <summary>
/// 蓄力攻击能力组件
/// 管理攻击的蓄力、指示器显示、伤害判定
/// 支持多种攻击形状（矩形、圆形等）
/// </summary>
public class ChargeAttack : MonoBehaviour
{
    [Header("攻击类型")]
    [SerializeField] private AttackShapeType _shapeType = AttackShapeType.Rectangle;

    [Header("通用配置")]
    [Tooltip("蓄力时间")]
    public float chargeTime = 0.5f;

    [Header("矩形攻击配置")]
    [Tooltip("攻击距离")]
    public float attackRange = 3f;
    [Tooltip("攻击宽度")]
    public float attackWidth = 1f;

    [Header("圆形攻击配置")]
    [Tooltip("AOE 半径")]
    public float aoeRadius = 2f;

    [Header("引用")]
    [SerializeField] private RectIndicator _rectIndicator;
    [SerializeField] private CircleIndicator _circleIndicator;

    // 当前激活的指示器
    private IAttackIndicator _activeIndicator;

    // 状态
    private bool _isCharging;
    private float _chargeProgress;
    private Vector2 _attackDirection;

    public bool IsCharging => _isCharging;
    public float ChargeProgress => _chargeProgress;
    public AttackShapeType ShapeType => _shapeType;

    /// <summary>
    /// 从 UnitData 初始化所有攻击参数（含攻击形状类型）
    /// </summary>
    public void InitializeFromData(UnitData data)
    {
        if (data == null) return;

        _shapeType = data.attackShapeType;
        chargeTime = data.attackChargeTime;
        attackRange = data.attackRange;
        attackWidth = data.attackWidth;
        aoeRadius = data.aoeRadius;
    }

    /// <summary>
    /// 开始蓄力攻击（根据当前配置的攻击形状自动选择矩形/圆形）
    /// </summary>
    /// <param name="direction">攻击方向（归一化，圆形 AOE 时可传入任意值）</param>
    public void StartAttack(Vector2 direction)
    {
        if (_isCharging) return;

        _isCharging = true;
        _chargeProgress = 0f;
        _attackDirection = direction.normalized;

        // 根据攻击类型选择并设置指示器
        SetupIndicator();

        // 显示指示器
        if (_activeIndicator != null)
        {
            _activeIndicator.SetDirection(_attackDirection);
            _activeIndicator.SetFillProgress(0f);
            _activeIndicator.Show();
        }
    }

    /// <summary>
    /// 根据攻击类型设置指示器
    /// </summary>
    private void SetupIndicator()
    {
        switch (_shapeType)
        {
            case AttackShapeType.Rectangle:
                if (_rectIndicator != null)
                {
                    _rectIndicator.Setup(attackRange, attackWidth);
                    _activeIndicator = _rectIndicator;
                }
                break;

            case AttackShapeType.Circle:
                if (_circleIndicator != null)
                {
                    _circleIndicator.Setup(aoeRadius);
                    _activeIndicator = _circleIndicator;
                }
                break;
        }
    }

    /// <summary>
    /// 取消攻击（被打断时调用）
    /// </summary>
    public void CancelAttack()
    {
        if (!_isCharging) return;

        _isCharging = false;
        _chargeProgress = 0f;

        if (_activeIndicator != null)
            _activeIndicator.Hide();
    }

    private void Update()
    {
        if (!_isCharging) return;

        // 更新蓄力进度
        _chargeProgress += Time.deltaTime / chargeTime;

        // 更新指示器填充进度
        if (_activeIndicator != null)
            _activeIndicator.SetFillProgress(_chargeProgress);

        // 蓄力完成
        if (_chargeProgress >= 1f)
        {
            PerformDamage();
            FinishAttack();
        }
    }

    /// <summary>
    /// 执行伤害判定
    /// </summary>
    private void PerformDamage()
    {
        Collider2D[] hits;

        switch (_shapeType)
        {
            case AttackShapeType.Rectangle:
                hits = PerformRectangleDamage();
                break;

            case AttackShapeType.Circle:
                hits = PerformCircleDamage();
                break;

            default:
                hits = new Collider2D[0];
                break;
        }

        ProcessHits(hits);
    }

    private Collider2D[] PerformRectangleDamage()
    {
        Vector2 origin = (Vector2)transform.position;
        Vector2 center = origin + _attackDirection * (attackRange / 2f);
        float angle = Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg;

        return Physics2D.OverlapBoxAll(
            center,
            new Vector2(attackRange, attackWidth),
            angle
        );
    }

    private Collider2D[] PerformCircleDamage()
    {
        return Physics2D.OverlapCircleAll(
            transform.position,
            aoeRadius
        );
    }

    private void ProcessHits(Collider2D[] hits)
    {
        foreach (var hit in hits)
        {
            // 不攻击自己
            if (hit.gameObject == gameObject) continue;

            // 检测是否为敌对单位 (根据 Tag 判断)
            if (hit.CompareTag(gameObject.tag)) continue;

            // 一击必杀
            BearUnit target = hit.GetComponent<BearUnit>();
            if (target != null)
            {
                Debug.Log($"[ChargeAttack] {gameObject.name} 击杀了 {target.name}");
                target.Die();
            }
        }
    }

    /// <summary>
    /// 完成攻击
    /// </summary>
    private void FinishAttack()
    {
        _isCharging = false;
        _chargeProgress = 0f;

        if (_activeIndicator != null)
            _activeIndicator.Hide();
    }

    /// <summary>
    /// 绘制攻击范围 Gizmo（调试用）
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        switch (_shapeType)
        {
            case AttackShapeType.Rectangle:
                DrawRectangleGizmo();
                break;

            case AttackShapeType.Circle:
                DrawCircleGizmo();
                break;
        }
    }

    private void DrawRectangleGizmo()
    {
        Vector2 dir = _isCharging ? _attackDirection : Vector2.right;
        Vector2 center = (Vector2)transform.position + dir * (attackRange / 2f);

        Matrix4x4 oldMatrix = Gizmos.matrix;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(attackRange, attackWidth, 0.1f));
        Gizmos.matrix = oldMatrix;
    }

    private void DrawCircleGizmo()
    {
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}
