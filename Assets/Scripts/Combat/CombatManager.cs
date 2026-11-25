using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Canvas combatCanvas;
    private List<CharacterData> enemyCharacters = new List<CharacterData>();
    
    [SerializeField] private List<CharacterData> enemyWave1Characters = new List<CharacterData>();
    [SerializeField] private List<CharacterData> enemyWave2Characters = new List<CharacterData>();
    [SerializeField] private List<CharacterData> enemyWave3Characters = new List<CharacterData>();
    [SerializeField] private List<CharacterData> enemyWave4Characters = new List<CharacterData>();
    [SerializeField] private List<CharacterData> enemyWave5Characters = new List<CharacterData>();

    // holds actual character spawned prefab instances
    private List<CharacterStats> activePlayerCharacters = new List<CharacterStats>();
    private List<CharacterStats> activeEnemyCharacters = new List<CharacterStats>();

    private float centerOffset = 2f;
    private float characterSpacing = 2f;

    private Coroutine combatTurnTimer;

    private CombatUI combatUI = null;

    private void Awake()
    {
        if (combatCanvas != null)
        {
            combatUI = combatCanvas.GetComponentInChildren<CombatUI>(true);
        }
    }

    public void SetupCombat(int currentWave, List<CharacterData> playerLineup)
    {
        ClearCharacters(activePlayerCharacters);
        ClearCharacters(activeEnemyCharacters);
        InstantiatePlayerTeam(playerLineup);
        InstantiateEnemyTeam(enemyCharacters);
        
        if (combatCanvas != null && combatUI == null) combatUI = combatCanvas.GetComponent<CombatUI>();
        if (combatUI != null)
        {
            combatUI.UpdateWaveUI(currentWave);
        }
        else Debug.LogError("CombatManager could not find a CombatUI component under its combat canvas.", this);
    }

    public void AddWaveCharacters(int wave)
    {
        switch (wave)
        {
            case 1:
                enemyCharacters.AddRange(enemyWave1Characters);
                break;
            case 2:
                enemyCharacters.AddRange(enemyWave2Characters);
                break;
            case 3:
                enemyCharacters.AddRange(enemyWave3Characters);
                break;
            case 4:
                enemyCharacters.AddRange(enemyWave4Characters);
                break;
            case 5:
                enemyCharacters.AddRange(enemyWave5Characters);
                break;
            default:
                Debug.Log("wave count greater than available characters to add");
                break;
        }
    }

    private static void ClearCharacters(List<CharacterStats> characters)
    {
        foreach (CharacterStats character in characters)
        {
            if (character != null) Destroy(character.gameObject);
        }

        characters.Clear();
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

        yield return new WaitForSeconds(1f); // wait for 1 second before ending the round so players can see outcome
        bool playerWon = activePlayerCharacters.Count > 0 && activeEnemyCharacters.Count == 0; // check if player won
        combatTurnTimer = null;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndCombat(playerWon);
        }
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
