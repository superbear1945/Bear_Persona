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
        Debug.Log($"[{_owner.name}] Enter Patrol State");
    }

    public void Execute()
    {
        // Patrol logic
    }

    public void Exit()
    {
        Debug.Log($"[{_owner.name}] Exit Patrol State");
    }
}
