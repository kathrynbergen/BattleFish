using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData data;

    public int Damage { get; private set; }
    public int Health;
    public Type Type;

    // sets initial values based on their data
    public void Initialize(CharacterData characterData)
    {
        data = characterData;
        Damage = data.damage;
        Health = data.health;
        Type = data.type;
    }
}
