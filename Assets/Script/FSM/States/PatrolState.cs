using UnityEngine;

public class PatrolState : IState
{
    private BearUnit _owner;

    public PatrolState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        
    }

    public void Execute()
    {
        // Patrol logic
    }

    public void Exit()
    {

    }
}
