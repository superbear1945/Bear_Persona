using UnityEngine;

/// <summary>
/// AI 攻击状态
/// 进入时开始蓄力攻击，蓄力完成后返回追踪或待机
/// </summary>
public class AttackState : IState
{
    private BearUnit _owner;
    private ChargeAttack _chargeAttack;

    public AttackState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        Debug.Log($"[{_owner.name}] Enter Attack State");

        _chargeAttack = _owner.GetComponent<ChargeAttack>();
        if (_chargeAttack == null)
        {
            Debug.LogWarning($"[{_owner.name}] 缺少 ChargeAttack 组件，无法攻击");
            _owner.SwitchToIdle();
            return;
        }

        // 计算攻击方向 (朝向玩家)
        if (PlayerController.Instance == null || PlayerController.Instance.currentUnit == null)
        {
            _owner.SwitchToIdle();
            return;
        }

        Vector2 direction = (PlayerController.Instance.currentUnit.transform.position - _owner.transform.position).normalized;
        _chargeAttack.StartAttack(direction);
    }

    public void Execute()
    {
        // 等待蓄力完成
        if (_chargeAttack == null || !_chargeAttack.IsCharging)
        {
            // 攻击完成，返回追踪
            _owner.SwitchToChase();
        }
    }

    public void Exit()
    {
        Debug.Log($"[{_owner.name}] Exit Attack State");

        // 如果被打断，取消攻击
        if (_chargeAttack != null && _chargeAttack.IsCharging)
        {
            _chargeAttack.CancelAttack();
        }
    }
}
