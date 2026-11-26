using System.Collections.Generic;
using UnityEngine;

public abstract class TestCharacterAction : CharacterAction
{
    public override void OnTurnAction(List<CharacterStats> allies, List<CharacterStats> enemies)
    {
        CharacterStats target = DecideTarget(enemies);

        if (target == null)
        {
            Debug.Log("no target available");
            return;
        }
        int damage = CalculateDamage(target);
        
        DealDamage(target, damage);
    }
    
    // test char acts like rat
    private CharacterStats DecideTarget(List<CharacterStats> targetableCharacters)
    {
        // check for empty target list
        if (targetableCharacters.Count == 0) return null;
        
        // rat just attacks the first enemy in front of them
        return targetableCharacters[0];
    }
}
