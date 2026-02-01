using UnityEngine;

public class PatrolState : IState
{
    private MonoBehaviour _owner;

    public PatrolState(MonoBehaviour owner)
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
