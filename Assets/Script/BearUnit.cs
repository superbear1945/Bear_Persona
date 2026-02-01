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
    public float AggroRange => _unitData != null ? _unitData.aggroRange : 8f;

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
    }

    /*
    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
    */

    public void SetControlled(bool isControlled)
    {
        _isSwitched = isControlled;

        // 切换 Tag 和 Layer
        if (_isSwitched)
        {
            gameObject.tag = "Player";
            gameObject.layer = LayerMask.NameToLayer("Player");
            SwitchToControlled();
        }
        else
        {
            gameObject.tag = "Enemy";
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            SwitchToIdle();
        }
    }

    private void SwitchToControlled()
    {
        _stateMachine.ChangeState(_controlledState);
    }

    public void SwitchToIdle()
    {
        // Debug.Log($"[BearUnit] Debug Switch -> Idle: {gameObject.name}");
        _stateMachine.ChangeState(_idleState);
    }

    public void SwitchToChase()
    {
        _stateMachine.ChangeState(_chaseState);
    }
}
