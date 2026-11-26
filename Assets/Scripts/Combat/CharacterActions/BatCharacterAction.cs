using System.Collections.Generic;

public class BatCharacterAction : CharacterAction
{
    public override void OnTurnAction(List<CharacterStats> allies, List<CharacterStats> enemies)
    {
        List<CharacterStats> targets = DecideTarget(enemies);

        foreach (CharacterStats targetStats in targets)
        {
            int damage = CalculateDamage(targetStats) / 2; // damage distributed amongst the two
            
            DealDamage(targetStats, damage);
        }
    }
    private List<CharacterStats> DecideTarget(List<CharacterStats> targetableCharacters)
    {
        // Bat attacks first and second character - splits his damage between the two
        
        //special cases - 0 or 1 targets available
        if (targetableCharacters.Count == 0) return new List<CharacterStats>();
        if (targetableCharacters.Count == 1) return new List<CharacterStats> { targetableCharacters[0] };
        
        // else return first two characters as normal
        return new List<CharacterStats> { targetableCharacters[0], targetableCharacters[1] };
    }
}
