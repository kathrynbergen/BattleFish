
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
    public List<string> OwnedMonsters = new List<string>();

    [Header("Purchased Abilities")]
    public List<string> OwnedAbilities = new List<string>();

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

    // Checks whether the player can afford an item
    public bool CanAfford(int price)
    {
        return price >= 0 && Coins >= price;
    }

    // Removes coins when purchasing an item
    public bool SpendCoins(int amount)
    {
        if (!CanAfford(amount))
        {
            return false;
        }

        Coins -= amount;
        return true;
    }

    // Adds coins after winning a battle
    public void AddCoins(int amount)
    {
        if (amount > 0)
        {
            Coins += amount;
        }
    }

    // Adds a purchased monster
    public void AddMonster(string monsterName)
    {
        OwnedMonsters.Add(monsterName);
    }

    // Adds a purchased environmental ability
    public void AddAbility(string abilityName)
    {
        if (currentLevel < 2)
        {
            Debug.Log("Abilities are locked in Level 1!");
            return;
        }

        OwnedAbilities.Add(abilityName);
    }

    // Checks if abilities are unlocked
    public bool AbilitiesUnlocked()
    {
        return currentLevel >= 2;
    }

    // Changes the current level
    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    // Uses one purchased ability
    public bool TryUseAbility(string abilityName)
    {
        if (!AbilitiesUnlocked())
        {
            return false;
        }

        if (OwnedAbilities.Contains(abilityName))
        {
            OwnedAbilities.Remove(abilityName);
            return true;
        }

        return false;
    }
}
