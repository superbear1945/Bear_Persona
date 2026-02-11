using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "BearPersona/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("外观")]
    public Sprite unitSprite;

    [Header("属性")]
    public float moveSpeed = 5f;
    public float aggroRange = 8f;

    [Header("攻击")]
    [Tooltip("攻击形状类型：矩形 / 圆形 AOE")]
    public AttackShapeType attackShapeType = AttackShapeType.Rectangle;
    [Tooltip("蓄力时间")]
    public float attackChargeTime = 0.5f;

    [Header("矩形攻击参数")]
    public float attackRange = 3f;
    public float attackWidth = 1f;

    [Header("圆形 AOE 参数")]
    [Tooltip("AOE 半径")]
    public float aoeRadius = 2f;

    [Header("巡逻")]
    [Tooltip("Idle 状态下多久切换到巡逻（秒）")]
    public float patrolIdleTime = 3f;
    [Tooltip("Idle 等待时间的正负随机偏差（秒）")]
    public float patrolIdleTimeRandomOffset = 1f;
    [Tooltip("巡逻随机选点半径")]
    public float patrolRadius = 5f;
}
