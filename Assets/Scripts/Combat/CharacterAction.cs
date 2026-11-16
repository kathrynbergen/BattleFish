using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    protected CharacterStats myStats;

    private void Awake()
    {
        myStats = GetComponent<CharacterStats>();
    }

    public virtual void OnTurnAction(List<CharacterStats> allies, List<CharacterStats> enemies)
    {
        print(myStats.data.characterName + "'s turn - no specific OnTurnAction defined");
    }

    protected void DealDamage(CharacterStats target, int damageAmount)
    {
        target.Health = Mathf.Max(0, target.Health - damageAmount);
    }

    protected int CalculateDamage(CharacterStats target)
    {
        return (int)(myStats.Damage*CalculateTypeAdvantageMult(target));
    }
    protected float CalculateTypeAdvantageMult(CharacterStats target)
    {
        int count = Enum.GetNames(typeof(Type)).Length;
        
        if (myStats.Type == target.Type)
        {
            print("same type");
            return 1f; // same type, no mult
        }
        
        if ((int)myStats.Type == ((int)target.Type + 1) % count)
        {
            print("advantage");
            return 1.5f; // advantage, extra damage
        }
        
        if ((int)myStats.Type == ((int)target.Type - 1) % count)
        {
            print("disadvantage");
            return 0.5f; // disadvantage, less damage
        }

        return 1; // no effect
    }
}
