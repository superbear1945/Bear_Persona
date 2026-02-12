using UnityEngine;

public class ChaseState : IState
{
    private BearUnit _owner;
    private readonly Collider2D[] _nearbyColliders = new Collider2D[12];
    private const float SeparationRadius = 0.9f;
    private const float SeparationWeight = 1.35f;

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

        // 如果在攻击范围内，切换到攻击状态
        if (distance <= _owner.AttackRange)
        {
            _owner.SwitchToAttack();
            return;
        }

        // 移动
        Vector2 chaseDir = ((Vector2)target.transform.position - (Vector2)_owner.transform.position).normalized;
        Vector2 separationDir = CalculateSeparationDirection();
        Vector2 finalDir = chaseDir + separationDir * SeparationWeight;
        if (finalDir.sqrMagnitude <= 0.0001f)
        {
            finalDir = chaseDir;
        }
        else
        {
            finalDir.Normalize();
        }
        var rb = _owner.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = finalDir * _owner.MoveSpeed;
        }
    }

    private Vector2 CalculateSeparationDirection()
    {
        Vector2 ownerPos = _owner.transform.position;
        int count = Physics2D.OverlapCircleNonAlloc(ownerPos, SeparationRadius, _nearbyColliders);
        if (count <= 0)
        {
            return Vector2.zero;
        }

        Vector2 separation = Vector2.zero;
        for (int i = 0; i < count; i++)
        {
            var hit = _nearbyColliders[i];
            if (hit == null)
            {
                continue;
            }

            BearUnit other = hit.GetComponentInParent<BearUnit>();
            if (other == null || other == _owner)
                continue;

            // 仅怪物之间做分离，避免影响玩家控制单位
            if (!other.CompareTag("Enemy"))
                continue;

            Vector2 away = ownerPos - (Vector2)other.transform.position;
            float distance = away.magnitude;
            if (distance <= 0.0001f || distance > SeparationRadius)
            {
                continue;
            }

            float ratio = (SeparationRadius - distance) / SeparationRadius;
            separation += away / distance * ratio;
        }

        return separation;
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
