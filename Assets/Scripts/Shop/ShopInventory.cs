
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    public static ShopInventory Instance { get; private set; }

    [Header("Player Coins")]
    [SerializeField] private int startingCoins = 1000;

    public int Coins { get; private set; }

    [Header("Current Level")]
    [SerializeField] private int currentLevel = 2;

    public int CurrentLevel => currentLevel;

    [Header("Purchased Monsters")]
    public List<CharacterData> OwnedMonsters =
        new List<CharacterData>();

    [Header("Purchased Abilities")]
    public List<AbilityData> OwnedAbilities =
        new List<AbilityData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Coins = startingCoins;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanAfford(int price)
    {
        return price >= 0 && Coins >= price;
    }

    public bool SpendCoins(int amount)
    {
        if (!CanAfford(amount))
            return false;

        Coins -= amount;
        return true;
    }

    public void AddCoins(int amount)
    {
        if (amount > 0)
            Coins += amount;
    }

    public void AddMonster(CharacterData monster)
    {
        if (monster != null)
            OwnedMonsters.Add(monster);
    }

    public void AddAbility(AbilityData ability)
    {
        if (ability != null && AbilitiesUnlocked())
            OwnedAbilities.Add(ability);
    }

    public bool AbilitiesUnlocked()
    {
        return currentLevel >= 2;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    public bool TryUseAbility(AbilityData ability)
    {
        if (!AbilitiesUnlocked() || ability == null)
            return false;

        return OwnedAbilities.Remove(ability);
    }

    public bool TryUseAbility(string abilityName)
    {
        if (!AbilitiesUnlocked())
            return false;

        for (int i = 0; i < OwnedAbilities.Count; i++)
        {
            if (OwnedAbilities[i] != null &&
                OwnedAbilities[i].abilityName == abilityName)
            {
                OwnedAbilities.RemoveAt(i);
                return true;
            }
        }

        return false;
    }
}
