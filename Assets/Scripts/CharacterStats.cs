using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData data;

    public int Damage { get; private set; }
    public int Health { get; private set; }

    public void Initialize(CharacterData characterData)
    {
        data = characterData;
        Damage = data.damage;
        Health = data.health;
    }

    public void TakeDamage(int amount)
    {
        Health = Mathf.Max(0, Health - amount);
    }

    public void AddStats(int damage, int health)
    {
        Damage += damage;
        Health += health;
    }
}
