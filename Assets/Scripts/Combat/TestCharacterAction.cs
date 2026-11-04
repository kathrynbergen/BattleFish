using System.Collections.Generic;

public class TestCharacterAction : CharacterAction
{
    public override void OnTurnAction(List<CharacterStats> allies, List<CharacterStats> enemies)
    {
        print("test character attacks "+ enemies[0]);
        DealDamage(enemies[0], 2);
    }
}
