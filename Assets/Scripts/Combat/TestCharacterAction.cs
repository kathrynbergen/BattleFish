using System.Collections.Generic;

public class TestCharacterAction : CharacterAction
{
    public override void OnTurnAction(List<CharacterStats> allies, List<CharacterStats> enemies)
    {
        CharacterStats target = enemies[0]; // whoever they target, test character targets enemy in the front only, but other characters could target differently
        
        int damage = CalculateDamage(target); 
        
        print("test character attacks "+ target + "for "+ damage + " damage");
        DealDamage(target, damage);
    }
}
