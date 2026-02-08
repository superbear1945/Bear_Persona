using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("当前被玩家控制的单位")]
    public GameObject currentUnit;

    [Header("输入相关")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _switchAction;
    [SerializeField] private InputActionReference _attackAction;
    [SerializeField] private InputActionReference _specialAttackAction;

    [Header("附身范围指示器")]
    [Tooltip("附身范围半径")]
    public float PossesionRange = 3f;
    [Tooltip("附身范围的圆环预制体 (必须包含 RangeCircle 组件)")]
    [SerializeField] private GameObject rangeCirclePrefab;
    // 当前的RangeCircle实际对象
    private RangeCircle _currentRangeCircle;

    public InputAction MoveAction => _moveAction.action;
    public InputAction SwitchAction => _switchAction.action;
    public InputAction AttackAction => _attackAction.action;
    public InputAction SpecialAttackAction => _specialAttackAction.action;

    // 进入附身模式时触发的事件
    public event Action<BearUnit> OnPossessionChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _switchAction.action.Enable();
        _attackAction.action.Enable();
        _specialAttackAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
        _switchAction.action.Disable();
        _attackAction.action.Disable();
        _specialAttackAction.action.Disable();
    }




    // 切换附身模式（子弹时间 + 视觉效果）
    public void TogglePossessionMode()
    {
        if (TimeManager.Instance == null) return;

        // 切换时间流速
        TimeManager.Instance.TogglePossession();

        // 切换后的状态判断
        if (TimeManager.Instance != null && TimeManager.Instance.IsInPossession)
            ShowRangeIndicator();
        else
            HideRangeIndicator();
    }

    private void ShowRangeIndicator()
    {
        if (rangeCirclePrefab != null && currentUnit != null)
        {
            if (_currentRangeCircle == null)
            {
                GameObject obj = Instantiate(rangeCirclePrefab);
                // DontDestroyonload(obj); // 可选：如果希望跨场景保留，可取消注释
                _currentRangeCircle = obj.GetComponent<RangeCircle>();
            }

            _currentRangeCircle.gameObject.SetActive(true);
            _currentRangeCircle.Setup(PossesionRange);
            _currentRangeCircle.SetTarget(currentUnit.transform);
        }
    }

    private void HideRangeIndicator()
    {
        if (_currentRangeCircle != null)
        {
            _currentRangeCircle.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandlePossessionSwitch();
    }

    private void HandlePossessionSwitch()
    {
        // 1. 检查是否在附身模式 (子弹时间)
        if (TimeManager.Instance == null || !TimeManager.Instance.IsInPossession)
            return;

        // 2. 检测攻击键按下 (确认选择)
        if (_attackAction.action.WasPressedThisFrame())
        {
            PerformRaycastAndSwitch();
        }
    }

    private void PerformRaycastAndSwitch()
    {
        // 获取鼠标位置 (Screen -> World)
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        // 射线检测
        RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero);

        if (hit.collider != null)
        {
            BearUnit targetUnit = hit.collider.GetComponent<BearUnit>();

            // 目标有效，且不是当前控制的单位
            if (targetUnit != null && targetUnit.gameObject != currentUnit)
            {
                // 距离检查 (Skill Range)
                float dist = Vector2.Distance(currentUnit.transform.position, targetUnit.transform.position);
                if (dist <= PossesionRange)
                {
                    SwitchControlTo(targetUnit);
                }
                else
                {
                    Debug.Log("目标超出附身范围");
                }
            }
        }
    }


    private void SwitchControlTo(BearUnit newUnit)
    {
        // 1. 释放旧单位（如果不是死亡的单位）
        if (currentUnit != null && currentUnit != _dyingUnit?.gameObject)
        {
            var pUnit = currentUnit.GetComponent<BearUnit>();
            if (pUnit != null) pUnit.SetControlled(false);
        }

        // 2. 销毁死亡的单位（如果有）
        if (_dyingUnit != null)
        {
            // 取消超时事件订阅
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnDeathPossessionTimeout -= OnGameOver;
            }
            DestroyDyingUnit();
        }

        // 3. 控制新单位
        currentUnit = newUnit.gameObject;
        newUnit.SetControlled(true);

        // 4. 退出附身模式
        TogglePossessionMode();

        // 5. 触发事件
        OnPossessionChanged?.Invoke(newUnit);

        Debug.Log($"[PlayerController] Switched control to {newUnit.name}");
    }

    // 死亡的单位（延迟销毁）
    private BearUnit _dyingUnit;

    /// <summary>
    /// 当前控制的单位死亡时调用
    /// 自动进入死亡附身模式
    /// </summary>
    public void OnControlledUnitDeath(BearUnit dyingUnit)
    {
        Debug.Log("[PlayerController] 控制单位死亡，进入死亡附身模式");

        _dyingUnit = dyingUnit;
        // 保持 currentUnit 引用以便计算距离
        // currentUnit 保持不变，直到附身成功

        // 订阅超时事件
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDeathPossessionTimeout += OnGameOver;
            TimeManager.Instance.StartDeathPossession();
            ShowRangeIndicator();
        }
    }

    /// <summary>
    /// 销毁死亡的单位
    /// </summary>
    private void DestroyDyingUnit()
    {
        if (_dyingUnit != null)
        {
            Destroy(_dyingUnit.gameObject);
            _dyingUnit = null;
        }
    }

    /// <summary>
    /// 死亡附身超时 = 游戏结束
    /// </summary>
    private void OnGameOver()
    {
        // 取消订阅
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDeathPossessionTimeout -= OnGameOver;
        }

        // 销毁死亡的单位
        DestroyDyingUnit();

        Debug.Log("========== 游戏结束 ==========");
        // TODO: 实现真正的游戏结束逻辑
    }
}