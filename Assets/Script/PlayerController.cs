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

    [Header("附身范围指示器")]
    [Tooltip("附身范围半径")]
    public float PossesionRange = 3f;
    [Tooltip("附身范围的圆环预制体 (必须包含 RangeCircle 组件)")]
    [SerializeField] private GameObject rangeCirclePrefab;
    private RangeCircle _currentRangeCircle;



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
                // DontDestroyOnLoad(obj); // 可选：如果希望跨场景保留，可取消注释
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
        // 1. 释放旧单位
        if (currentUnit != null)
        {
            var pUnit = currentUnit.GetComponent<BearUnit>();
            if (pUnit != null) pUnit.SetControlled(false);
        }

        // 2. 控制新单位
        currentUnit = newUnit.gameObject;
        newUnit.SetControlled(true);

        // 3. 退出附身模式
        TogglePossessionMode();

        // 4. 触发事件
        OnPossessionChanged?.Invoke(newUnit);

        Debug.Log($"[PlayerController] Switched control to {newUnit.name}");
    }
}