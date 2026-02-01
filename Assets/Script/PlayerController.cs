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

    private void Update()
    {
        if (currentUnit == null) return;
        InjectInputAction();
    }

    // 向被控制的对象注入输入内容
    void InjectInputAction()
    {
        var switchable = currentUnit.GetComponent<ISwitchable>();
        if (switchable == null) return;

        // 移动
        Vector2 moveInput = _moveAction.action.ReadValue<Vector2>();
        switchable.OnMove(moveInput);

        // 攻击
        if (_attackAction.action.WasPressedThisFrame())
        {
            switchable.OnAttack();
        }

        // 特殊攻击
        if (_specialAttackAction.action.WasPressedThisFrame())
        {
            switchable.OnSpecialAttack();
        }

        // 附身/切换
        if (_switchAction.action.WasPressedThisFrame())
        {
            switchable.OnPossess();
        }
    }
}