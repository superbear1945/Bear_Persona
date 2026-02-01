using UnityEngine;

public class ControlledState : IState
{
    private BearUnit _owner;
    private Rigidbody2D _rb;

    public ControlledState(BearUnit owner)
    {
        _owner = owner;
        _rb = owner.GetComponent<Rigidbody2D>();
    }

    public void Enter()
    {
        Debug.Log($"[{_owner.name}] Enter Controlled State");
    }

    public void Execute()
    {
        if (PlayerController.Instance == null) return;

        HandleMove();
        HandleAttack();
        HandleSpecialAttack();
        HandleSwitch();
    }

    private void HandleMove()
    {
        if (_rb == null)
            Debug.LogError($"当前单位 [{_owner.name}] 没有 Rigidbody2D！");

        Vector2 moveInput = PlayerController.Instance.MoveAction.ReadValue<Vector2>();

        float speed = _owner.MoveSpeed;

        _rb.velocity = moveInput * speed;
    }

    private void HandleAttack()
    {
        if (PlayerController.Instance.AttackAction.WasPressedThisFrame())
        {
            Debug.Log($"[{_owner.name}] Attack Action Triggered");
        }
    }

    private void HandleSpecialAttack()
    {
        if (PlayerController.Instance.SpecialAttackAction.WasPressedThisFrame())
        {
            Debug.Log($"[{_owner.name}] Special Attack Action Triggered");
        }
    }

    private void HandleSwitch()
    {
        if (PlayerController.Instance.SwitchAction.WasPressedThisFrame())
        {
            Debug.Log($"[{_owner.name}] Possess Action Triggered");
        }
    }

    public void Exit()
    {
        Debug.Log($"[{_owner.name}] Exit Controlled State");
        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }
    }
}
