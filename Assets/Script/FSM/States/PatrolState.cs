using UnityEngine;

public class PatrolState : IState
{
    private BearUnit _owner;
    private Vector2 _targetPosition;
    private Vector2 _originPosition;
    private readonly float _arrivalThreshold = 0.3f;

    public PatrolState(BearUnit owner)
    {
        _owner = owner;
    }

    public void Enter()
    {
        _originPosition = _owner.transform.position;
        Vector2 randomOffset = Random.insideUnitCircle * _owner.PatrolRadius;
        _targetPosition = _originPosition + randomOffset;
    }

    public void Execute()
    {
        // 仇恨检测：巡逻中发现玩家也要追
        if (PlayerController.Instance != null && PlayerController.Instance.currentUnit != null)
        {
            var target = PlayerController.Instance.currentUnit;
            if (target != _owner.gameObject)
            {
                float distToPlayer = Vector2.Distance(_owner.transform.position, target.transform.position);
                if (distToPlayer <= _owner.AggroRange)
                {
                    _owner.SwitchToChase();
                    return;
                }
            }
        }

        // 到达判定
        float distToTarget = Vector2.Distance(_owner.transform.position, _targetPosition);
        if (distToTarget < _arrivalThreshold)
        {
            _owner.SwitchToIdle();
            return;
        }

        // 移动向目标点
        Vector2 dir = (_targetPosition - (Vector2)_owner.transform.position).normalized;
        var rb = _owner.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * _owner.MoveSpeed;
        }
    }

    public void Exit()
    {
        var rb = _owner.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
