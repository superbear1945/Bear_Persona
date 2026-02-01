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
        _controlledState = new ControlledState(this);
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
    }

    public void OnEnterSwitch()
    {
        Debug.Log($"[BearUnit] Entered Switch State: {gameObject.name}");
        IsSwitched = true;
        _stateMachine.ChangeState(_controlledState);
    }

    public void OnExitSwitch()
    {
        Debug.Log($"[BearUnit] Exited Switch State: {gameObject.name}");
        IsSwitched = false;
        _stateMachine.ChangeState(_idleState);
    }

    public void OnMove(Vector2 direction)
    {
        // Only move if in ControlledState? Or just let input drive it given PlayerController safeguards.
        // For now, keep logic here, but we could delegate to state.
        if (direction != Vector2.zero)
        {
            // Debug.Log($"[BearUnit] Moving: {direction}");
            transform.Translate(new Vector3(direction.x, 0, direction.y) * 5f * Time.deltaTime);
        }
    }

    public void OnAttack()
    {
        Debug.Log($"[BearUnit] Attack Action Triggered on {gameObject.name}");
        // If we wanted, we could switch to AttackState here temporarily via animation events etc.
        // _stateMachine.ChangeState(_attackState);
    }

    public void OnSpecialAttack()
    {
        Debug.Log($"[BearUnit] Special Attack Action Triggered on {gameObject.name}");
    }

    public void OnPossess()
    {
        Debug.Log($"[BearUnit] Possess Action Triggered on {gameObject.name}");
    }
}
