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
        foreach (CharacterStats character in playerCharacters)
        {
            character.Initialize(character.data);
        }
        foreach (CharacterStats character in enemyCharacters)
        {
            character.Initialize(character.data);
        }
        
        combatTurnTimer = StartCoroutine(CombatCycleTimer());
    }

    IEnumerator CombatCycleTimer()
    {
        while (playerCharacters.Count > 0 && enemyCharacters.Count > 0)
        {
            yield return new WaitForSeconds(1f);
            RunTurnActions(playerCharacters[0], true);
            RunTurnActions(enemyCharacters[0], false);
            
            CheckForDeath();
        }
        print("combat cycle done");
    }

    // runs character's CharacterAction
    private void RunTurnActions(CharacterStats activeCharacter, bool isPlayer)
    {
        if (activeCharacter == null) return;

        CharacterAction activeCharacterAction = activeCharacter.GetComponent<CharacterAction>();
        if (activeCharacterAction == null) return;

        if (isPlayer) activeCharacterAction.OnTurnAction(playerCharacters, enemyCharacters);
        else activeCharacterAction.OnTurnAction(enemyCharacters, playerCharacters);
    }

    private void CheckForDeath()
    {
        List<CharacterStats> dyingCharacters = new List<CharacterStats>();
        // find all characters who are dying this round
        foreach (CharacterStats character in playerCharacters)
        {
            if (character != null && character.Health <= 0)
            {
                dyingCharacters.Add(character);
            }
        }
        foreach (CharacterStats character in enemyCharacters)
        {
            if (character != null && character.Health <= 0)
            {
                dyingCharacters.Add(character);
            }
        }
        // kill them
        foreach (CharacterStats dead in dyingCharacters)
        {
            ResolveDeath(dead);
        }
    }

    private void ResolveDeath(CharacterStats deadCharacter)
    {
        print(deadCharacter.name + " dead");
        playerCharacters.Remove(deadCharacter);
        enemyCharacters.Remove(deadCharacter);
    }
}
