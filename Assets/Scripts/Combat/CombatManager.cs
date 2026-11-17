using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Canvas combatCanvas;
    [SerializeField] private List<CharacterData> enemyCharacters = new List<CharacterData>();

    // holds actual character spawned prefab instances
    private List<CharacterStats> activePlayerCharacters = new List<CharacterStats>();
    private List<CharacterStats> activeEnemyCharacters = new List<CharacterStats>();

    [SerializeField] private float centerOffset = 2f;
    private float characterSpacing = 2f;

    private Coroutine combatTurnTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetupCombat()
    {
        InstantiatePlayerTeam(ShopInventory.Instance.OwnedMonsters);
        InstantiateEnemyTeam(enemyCharacters);
    }

    private void InstantiatePlayerTeam(List<CharacterData> monsters)
    {
        activePlayerCharacters.Clear();
        for (int i = 0; i < monsters.Count; i++)
        {
            CharacterData monster = monsters[i];
            if (monster == null) continue;
            if (monster.prefab == null)
            {
                Debug.LogError($"Character data {monster.name} has no combat prefab assigned.", monster);
                continue;
            }

            Vector3 position = transform.position + Vector3.right * -1f * (centerOffset + i * characterSpacing);
            CharacterStats character = Instantiate(monster.prefab, position, Quaternion.identity);
            character.Initialize(monster);
            FlipCharacterSprites(character);
            activePlayerCharacters.Add(character);
        }
    }

    private void FlipCharacterSprites(CharacterStats character)
    {
        SpriteRenderer[] spriteRenderers = character.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void InstantiateEnemyTeam(List<CharacterData> monsters)
    {
        activeEnemyCharacters.Clear();
        for (int i = 0; i < monsters.Count; i++)
        {
            CharacterData monster = monsters[i];
            if (monster == null) continue;
            if (monster.prefab == null)
            {
                Debug.LogError($"Character data {monster.name} has no combat prefab assigned.", monster);
                continue;
            }

            Vector3 position = transform.position + Vector3.right * (centerOffset + i * characterSpacing);
            CharacterStats character = Instantiate(monster.prefab, position, Quaternion.identity);
            character.Initialize(monster);
            activeEnemyCharacters.Add(character);
        }
    }

    public void StartCombatSequence()
    {
        if (combatTurnTimer != null) return;
        combatTurnTimer = StartCoroutine(CombatCycleTimer());
    }

    IEnumerator CombatCycleTimer()
    {
        while (activePlayerCharacters.Count > 0 && activeEnemyCharacters.Count > 0)
        {
            yield return new WaitForSeconds(1f);
            RunTurnActions(activePlayerCharacters[0], true);
            RunTurnActions(activeEnemyCharacters[0], false);
            
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

        if (isPlayer) activeCharacterAction.OnTurnAction(activePlayerCharacters, activeEnemyCharacters);
        else activeCharacterAction.OnTurnAction(activeEnemyCharacters, activePlayerCharacters);
    }

    private void CheckForDeath()
    {
        List<CharacterStats> dyingCharacters = new List<CharacterStats>();
        // find all characters who are dying this round
        foreach (CharacterStats character in activePlayerCharacters)
        {
            if (character != null && character.Health <= 0)
            {
                dyingCharacters.Add(character);
            }
        }
        foreach (CharacterStats character in activeEnemyCharacters)
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
        // remove from game screen
        Destroy(deadCharacter.gameObject);

        // Remove and re-space only the team this character belonged to.
        if (activePlayerCharacters.Contains(deadCharacter))
        {
            activePlayerCharacters.Remove(deadCharacter);
            RepositionTeam(activePlayerCharacters, -1f);
        }
        else if (activeEnemyCharacters.Contains(deadCharacter))
        {
            activeEnemyCharacters.Remove(deadCharacter);
            RepositionTeam(activeEnemyCharacters, 1f);
        }
    }

    private void RepositionTeam(List<CharacterStats> characters, float side)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] == null) continue;

            characters[i].transform.position = transform.position + Vector3.right * side * (centerOffset + i * characterSpacing);
        }
    }
}
