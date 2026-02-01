using UnityEngine;

public class BearUnit : MonoBehaviour, ISwitchable
{
    [Header("是否被玩家操控")]
    [SerializeField] private bool _isSwitched;

    public bool IsSwitched
    {
        get => _isSwitched;
        set => _isSwitched = value;
    }

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
        _controlledState = new ControlledState(this.gameObject);
        _chaseState = new ChaseState(this);
        _attackState = new AttackState(this);
        _patrolState = new PatrolState(this);
    }

    private void Start()
    {
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

    private void SwitchToControlled()
    {
        Debug.Log($"[BearUnit] Debug Switch -> Controlled: {gameObject.name}");
        _stateMachine.ChangeState(_controlledState);
    }

    private void SwitchToIdle()
    {
        Debug.Log($"[BearUnit] Debug Switch -> Idle: {gameObject.name}");
        _stateMachine.ChangeState(_idleState);
    }
}
