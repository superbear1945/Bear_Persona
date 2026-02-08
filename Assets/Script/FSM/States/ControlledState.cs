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
        PlayerController.Instance.currentUnit = _owner.gameObject;
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
        if (!PlayerController.Instance.AttackAction.WasPressedThisFrame()) return;

        ChargeAttack chargeAttack = _owner.GetComponent<ChargeAttack>();
        if (chargeAttack == null)
        {
            Debug.LogWarning($"[{_owner.name}] 缺少 ChargeAttack 组件");
            return;
        }

        // 如果已经在蓄力，忽略
        if (chargeAttack.IsCharging) return;

        // 计算攻击方向 (从单位指向鼠标)
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        Vector2 direction = (mouseWorldPos - _owner.transform.position).normalized;

        chargeAttack.StartAttack(direction);
        Debug.Log($"[{_owner.name}] 开始蓄力攻击");
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
            Debug.Log($"[{_owner.name}] 切换/附身操作 - 触发子弹时间");
            PlayerController.Instance.TogglePossessionMode();
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
