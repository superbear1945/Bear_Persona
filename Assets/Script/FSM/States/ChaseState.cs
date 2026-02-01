using UnityEngine;

public class ChaseState : IState
{
    private MonoBehaviour _owner;

    public ChaseState(MonoBehaviour owner)
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
