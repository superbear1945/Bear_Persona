using UnityEngine;

public class ControlledState : IState
{
    private MonoBehaviour _owner;

    public ControlledState(MonoBehaviour owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        Debug.Log($"[{_owner.name}] Enter Controlled State");
    }

    public void Execute()
    {
        // Controlled logic (handled via input events mostly, but here we can do continuous updates if needed)
    }

    public void Exit()
    {
        Debug.Log($"[{_owner.name}] Exit Controlled State");
    }
}
