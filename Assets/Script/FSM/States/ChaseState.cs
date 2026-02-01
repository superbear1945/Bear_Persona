using UnityEngine;

public class ChaseState : IState
{
    private BearUnit _owner;

    public ChaseState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        // 默认为Idle状态
        if (PlayerController.Instance == null || PlayerController.Instance.currentUnit == null)
        {
            _owner.SwitchToIdle();
            return;
        }

        // 追踪逻辑
        var target = PlayerController.Instance.currentUnit;
        float distance = Vector2.Distance(_owner.transform.position, target.transform.position);

        // 如果距离过远，放弃追踪 (比如 1.5倍 仇恨范围)
        if (distance > _owner.AggroRange * 1.5f)
        {
            _owner.SwitchToIdle();
            return;
        }

        // 移动
        Vector2 dir = (target.transform.position - _owner.transform.position).normalized;
        var rb = _owner.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * _owner.MoveSpeed;
        }
    }

    public void Exit()
    {
        // Debug.Log($"[{_owner.name}] Exit Chase State");
        var rb = _owner.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
