using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "ScriptableObjects/AbilityData", order = 1)]
public class AbilityData : ScriptableObject
{
    [Header("Ability Information")]
    public string abilityName;
    public int damage;

    [Header("Shop Information")]
    [FormerlySerializedAs("characterSprite")]
    public Sprite abilitySprite;
    public int price;
    public string description;
}