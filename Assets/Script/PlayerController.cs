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

    // Update removed as Logic is moved to FSM
}