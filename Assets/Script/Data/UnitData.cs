using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "BearPersona/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("外观")]
    public Sprite unitSprite;

    [Header("属性")]
    public float moveSpeed = 5f;
    public float aggroRange = 8f;
}
