using UnityEngine;

public class IdleState : IState
{
    private BearUnit _owner;
    private float _patrolTimer;
    private float _currentIdleWaitTime;

    public IdleState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        _patrolTimer = 0f;
        float offset = _owner.PatrolIdleTimeRandomOffset;
        _currentIdleWaitTime = _owner.PatrolIdleTime + Random.Range(-offset, offset);
    }

    public void Execute()
    {
        // 简单的仇恨检测
        if (PlayerController.Instance != null && PlayerController.Instance.currentUnit != null)
        {
            var target = PlayerController.Instance.currentUnit;
            // 只有当目标是"别人"时才追
            if (target != _owner.gameObject)
            {
                float distance = Vector2.Distance(_owner.transform.position, target.transform.position);
                if (distance <= _owner.AggroRange)
                {
                    _owner.SwitchToChase();
                    return;
                }
            }
        }

        // 巡逻倒计时：一段时间没发现玩家则进入巡逻
        _patrolTimer += Time.deltaTime;
        if (_patrolTimer >= _currentIdleWaitTime)
        {
            _owner.SwitchToPatrol();
        }
    }

    public void Exit()
    {

    }
}
