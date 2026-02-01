using UnityEngine;

public class ControlledState : IState
{
    private GameObject _owner;

    public ControlledState(GameObject owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        Debug.Log($"[{_owner.name}] Enter Controlled State");
    }

    public void Execute()
    {
        if (PlayerController.Instance == null) return;

        // Move
        Vector2 moveInput = PlayerController.Instance.MoveAction.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            // Debug.Log($"[{_owner.name}] Moving: {moveInput}");
            _owner.transform.Translate(new Vector3(moveInput.x, moveInput.y, 0) * 5f * Time.deltaTime);
        }

        // Attack
        if (PlayerController.Instance.AttackAction.WasPressedThisFrame())
        {
            Debug.Log($"[{_owner.name}] Attack Action Triggered");
        }

        // Special Attack
        if (PlayerController.Instance.SpecialAttackAction.WasPressedThisFrame())
        {
            Debug.Log($"[{_owner.name}] Special Attack Action Triggered");
        }

        // Possess/Switch (Self-trigger, though practically this might be handled by the switcher)
        if (PlayerController.Instance.SwitchAction.WasPressedThisFrame())
        {
            Debug.Log($"[{_owner.name}] Possess Action Triggered");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{_owner.name}] Exit Controlled State");
    }
}
