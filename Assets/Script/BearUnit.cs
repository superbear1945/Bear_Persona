using UnityEngine;

// 基础单位，后续可能会改造成单位基类，但目前思路是通过ScriptableObject实现多兵种
[RequireComponent(typeof(Rigidbody2D))]
public class BearUnit : MonoBehaviour, ISwitchable
{
    [Header("是否被玩家操控")]
    [SerializeField] private bool _isSwitched;

    public bool IsSwitched
    {
        get => _isSwitched;
        set => _isSwitched = value;
    }

    [Header("数据配置")]
    [SerializeField] private UnitData _unitData;

    public float MoveSpeed => _unitData != null ? _unitData.moveSpeed : 5f;

    private StateMachine _stateMachine;
    private IdleState _idleState;
    private ControlledState _controlledState;
    // Keep reference to other states if needed for AI logic later
    private ChaseState _chaseState;
    private AttackState _attackState;
    private PatrolState _patrolState;

    private void Awake()
    {
        _stateMachine = new StateMachine();
        _idleState = new IdleState(this);
        _controlledState = new ControlledState(this);
        _chaseState = new ChaseState(this);
        _attackState = new AttackState(this);
        _patrolState = new PatrolState(this);
    }

    private void Start()
    {
        // Apply Data
        if (_unitData != null)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null && _unitData.unitSprite != null)
            {
                sr.sprite = _unitData.unitSprite;
            }
        }

        // Default verification state
        _stateMachine.ChangeState(_idleState);
    }

    private void Update()
    {
        _stateMachine.Update();
        UpdateDebugSwitchState();
    }

    // 临时用于调试
    private void UpdateDebugSwitchState()
    {
        // 详见 Doc/DebugFeatures.md
        if (_isSwitched && _stateMachine.CurrentState != _controlledState)
        {
            SwitchToControlled();
        }
        else if (!_isSwitched && _stateMachine.CurrentState == _controlledState)
        {
            SwitchToIdle();
        }
    }

    public void SetControlled(bool isControlled)
    {
        _isSwitched = isControlled;
        if (isControlled)
            SwitchToControlled();
        else
            SwitchToIdle();
    }

    private void SwitchToControlled()
    {
        _stateMachine.ChangeState(_controlledState);
    }

    private void SwitchToIdle()
    {
        Debug.Log($"[BearUnit] Debug Switch -> Idle: {gameObject.name}");
        _stateMachine.ChangeState(_idleState);
    }
}
