using UnityEngine;

public class IdleState : IState
{
    private BearUnit _owner;

    public IdleState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        Debug.Log($"[{_owner.name}] Enter Idle State");
    }

    public void Execute()
    {
        // Idle logic
    }

    public void Exit()
    {
        Debug.Log($"[{_owner.name}] Exit Idle State");
    }
}
