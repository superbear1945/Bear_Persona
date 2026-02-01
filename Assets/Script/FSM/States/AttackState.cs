using UnityEngine;

public class AttackState : IState
{
    private BearUnit _owner;

    public AttackState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        Debug.Log($"[{_owner.name}] Enter Attack State");
    }

    public void Execute()
    {
        // Attack logic
    }

    public void Exit()
    {
        Debug.Log($"[{_owner.name}] Exit Attack State");
    }
}
