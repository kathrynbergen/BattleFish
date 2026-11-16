using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("Character Information")]
    public string characterName;
    public int damage;
    public int health;

    [Header("Shop Information")] 
    public Sprite characterSprite;
    public int price;
    public string description;
}