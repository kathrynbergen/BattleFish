using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData data;

    public string characterName;
    public int Damage { get; private set; }
    public int Health;
    public Type Type;
    private DamageTakenDisplay m_damageTakenDisplay;

    // sets initial values based on their data
    public void Initialize(CharacterData characterData)
    {
        data = characterData;
        characterName = data.characterName;
        Damage = data.damage;
        Health = data.health;
        Type = data.type;
        m_damageTakenDisplay = GetComponentInChildren<DamageTakenDisplay>(true);
    }

    public void TakeDamage(int amount)
    {
        int damageTaken = Mathf.Min(Mathf.Max(Health, 0), Mathf.Max(amount, 0));
        Health -= damageTaken;

        if (damageTaken > 0 && m_damageTakenDisplay != null)
            m_damageTakenDisplay.ShowDamage(damageTaken);
    }
}
