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
        
    }

    public void Execute()
    {
        // Idle logic
    }

    public void Exit()
    {

    }
}
