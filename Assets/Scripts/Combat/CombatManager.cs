using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public List<CharacterStats> playerCharacters;
    public List<CharacterStats> enemyCharacters;

    private Coroutine combatTurnTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCombatSequence(); 
    }

    private void StartCombatSequence()
    {
        if (combatTurnTimer != null) return;
        
        combatTurnTimer = StartCoroutine(CombatCycleTimer());
    }

    IEnumerator CombatCycleTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            RunTurnActions(playerCharacters);
            RunTurnActions(enemyCharacters);
        }
        
    }

    private void RunTurnActions(List<CharacterStats> characters)
    {
        print(characters[0].data.characterName);
    }
    
}
