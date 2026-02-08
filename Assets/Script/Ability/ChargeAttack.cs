using UnityEngine;

/// <summary>
/// 蓄力攻击能力组件
/// 管理攻击的蓄力、指示器显示、伤害判定
/// </summary>
public class ChargeAttack : MonoBehaviour
{
    [Header("配置")]
    [Tooltip("攻击距离")]
    public float attackRange = 3f;
    [Tooltip("攻击宽度")]
    public float attackWidth = 1f;
    [Tooltip("蓄力时间")]
    public float chargeTime = 0.5f;

    [Header("引用")]
    [SerializeField] private RectIndicator _indicator;
    [SerializeField] private ChargeBar _chargeBar;

    // 状态
    private bool _isCharging;
    private float _chargeProgress;
    private Vector2 _attackDirection;

    public bool IsCharging => _isCharging;
    public float ChargeProgress => _chargeProgress;

    /// <summary>
    /// 从 UnitData 初始化攻击参数
    /// </summary>
    public void Initialize(float range, float width, float time)
    {
        attackRange = range;
        attackWidth = width;
        chargeTime = time;
    }

    /// <summary>
    /// 开始蓄力攻击
    /// </summary>
    /// <param name="direction">攻击方向（归一化）</param>
    public void StartAttack(Vector2 direction)
    {
        if (_isCharging) return;

        _isCharging = true;
        _chargeProgress = 0f;
        _attackDirection = direction.normalized;

        // 显示指示器
        if (_indicator != null)
        {
            _indicator.Setup(attackRange, attackWidth);
            _indicator.SetDirection(_attackDirection);
            _indicator.Show();
        }

        // 显示进度条
        if (_chargeBar != null)
        {
            _chargeBar.SetProgress(0f);
            _chargeBar.Show();
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

        if (_indicator != null) _indicator.Hide();
        if (_chargeBar != null) _chargeBar.Hide();
    }

    private void Update()
    {
        if (!_isCharging) return;

        // 更新蓄力进度
        _chargeProgress += Time.deltaTime / chargeTime;

        // 更新矩形填充进度
        if (_indicator != null)
            _indicator.SetFillProgress(_chargeProgress);

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
        // 计算矩形区域
        Vector2 origin = (Vector2)transform.position;
        Vector2 center = origin + _attackDirection * (attackRange / 2f);

        // 使用 OverlapBox 检测敌人
        float angle = Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg;
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            center,
            new Vector2(attackRange, attackWidth),
            angle
        );

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
                Destroy(target.gameObject);
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

        if (_indicator != null) _indicator.Hide();
        if (_chargeBar != null) _chargeBar.Hide();
    }

    /// <summary>
    /// 绘制攻击范围 Gizmo（调试用）
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 dir = _isCharging ? _attackDirection : Vector2.right;
        Vector2 center = (Vector2)transform.position + dir * (attackRange / 2f);

        // 绘制矩形轮廓
        Matrix4x4 oldMatrix = Gizmos.matrix;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(attackRange, attackWidth, 0.1f));
        Gizmos.matrix = oldMatrix;
    }
}
