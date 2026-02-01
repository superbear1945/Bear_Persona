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
