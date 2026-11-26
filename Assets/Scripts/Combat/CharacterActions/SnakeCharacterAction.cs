using System.Collections.Generic;

public class SnakeCharacterAction : CharacterAction
{
    public override void OnTurnAction(List<CharacterStats> allies, List<CharacterStats> enemies)
    {
        CharacterStats target = DecideTarget(enemies);
        
        int damage = CalculateDamage(target);
        
        DealDamage(target, damage);

        if (SnakeIsLucky())
        {
            print("snake got lucky!");

            if (enemies.Count == 0)
            {
                print("no target for lucky snake");
                return;
            }

            CharacterStats luckyTarget = enemies[^1]; // last enemy in list
            int luckyDamage = CalculateDamage(luckyTarget);
            DealDamage(luckyTarget, luckyDamage);
        }
    }
    
    // attacks character directly in front of them for 100% damage, 10% chance to deal 50% damage to the enemy in the back
    private CharacterStats DecideTarget(List<CharacterStats> targetableCharacters)
    {
        // check for empty target list
        if (targetableCharacters.Count == 0) return null;
        
        // first enemy in front of them
        return targetableCharacters[0];
    }

    private bool SnakeIsLucky()
    {
        // 10% chance to attack enemy in the back
        return (UnityEngine.Random.value <= 0.1f);
    }
}
