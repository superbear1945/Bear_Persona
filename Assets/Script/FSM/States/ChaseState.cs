using UnityEngine;

public class ChaseState : IState
{
    private BearUnit _owner;

    public ChaseState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        Debug.Log($"[{_owner.name}] Enter Chase State");
    }

    public void Execute()
    {
        // Chase logic
    }

    public void Exit()
    {
        Debug.Log($"[{_owner.name}] Exit Chase State");
    }
}
