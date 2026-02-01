using UnityEngine;

public class IdleState : IState
{
    private BearUnit _owner;

    public IdleState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        // 简单的仇恨检测
        if (PlayerController.Instance != null && PlayerController.Instance.currentUnit != null)
        {
            var target = PlayerController.Instance.currentUnit;
            // 只有当目标是“别人”时才追
            if (target != _owner.gameObject)
            {
                float distance = Vector2.Distance(_owner.transform.position, target.transform.position);
                if (distance <= _owner.AggroRange)
                {
                    _owner.SwitchToChase();
                }
            }
        }
    }

    public void Exit()
    {

    }
}
