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
    public float attackRange = 3f;
    public float attackWidth = 1f;
    public float attackChargeTime = 0.5f;
}
