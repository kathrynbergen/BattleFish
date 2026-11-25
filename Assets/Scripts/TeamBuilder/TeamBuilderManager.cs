
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeamBuilderManager : MonoBehaviour
{
    [Header("Inventory")]
    public Transform inventoryGrid;
    public GameObject monsterInventoryItemPrefab;

    [Header("Team")]
    public CharacterData[] teamSlots = new CharacterData[5];
    public TeamSlotUI[] slotUI = new TeamSlotUI[5];

    [Header("Monster Information")]
    public TMP_Text nameText;
    public TMP_Text damageText;
    public TMP_Text healthText;
    public TMP_Text descriptionText;
    public TMP_Text messageText;

    private int m_selectedIndex = -1;

    // Stores which purchased copy belongs to each team slot
    private int[] m_teamIndices = { -1, -1, -1, -1, -1 };

    private List<MonsterInventoryItemUI> m_inventoryItems =
        new List<MonsterInventoryItemUI>();

    public void LoadOwnedMonsters()
    {
        if (ShopInventory.Instance == null ||
            inventoryGrid == null ||
            monsterInventoryItemPrefab == null)
        {
            Debug.LogError("Missing inventory references!");
            return;
        }

        // Remove old inventory UI
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        m_inventoryItems.Clear();
        m_selectedIndex = -1;

        List<CharacterData> monsters =
            ShopInventory.Instance.OwnedMonsters;

        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == null)
                continue;

            GameObject item = Instantiate(
                monsterInventoryItemPrefab,
                inventoryGrid
            );

            MonsterInventoryItemUI itemUI =
                item.GetComponent<MonsterInventoryItemUI>();

            if (itemUI == null)
            {
                Debug.LogError("Missing MonsterInventoryItemUI!");
                continue;
            }

            itemUI.SetMonster(monsters[i], this, i);
            m_inventoryItems.Add(itemUI);
        }

        ClearMonsterInfo();
        RefreshSlots();

        Debug.Log("Loaded " + monsters.Count + " owned monsters.");
    }

    public void SelectMonster(int inventoryIndex)
    {
        List<CharacterData> monsters =
            ShopInventory.Instance.OwnedMonsters;

        if (inventoryIndex < 0 ||
            inventoryIndex >= monsters.Count)
            return;

        CharacterData monster = monsters[inventoryIndex];

        if (monster == null)
            return;

        m_selectedIndex = inventoryIndex;

        if (nameText != null)
            nameText.text = monster.characterName;

        if (damageText != null)
            damageText.text = "Damage: " + monster.damage;

        if (healthText != null)
            healthText.text = "Health: " + monster.health;

        if (descriptionText != null)
            descriptionText.text = monster.description;

        if (messageText != null)
            messageText.text = "Choose a team slot.";

        foreach (MonsterInventoryItemUI item in m_inventoryItems)
        {
            if (item != null)
            {
                item.SetSelected(item.InventoryIndex == m_selectedIndex);
            }
        }

        Debug.Log("Selected: " + monster.characterName);
    }

    public void AssignMonster(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= teamSlots.Length)
            return;

        if (m_selectedIndex == -1)
        {
            ShowMessage("Select a monster first!");
            return;
        }

        // Prevent using the same purchased copy twice
        for (int i = 0; i < m_teamIndices.Length; i++)
        {
            if (i != slotIndex &&
                m_teamIndices[i] == m_selectedIndex)
            {
                ShowMessage("This monster is already on your team!");
                return;
            }
        }

        List<CharacterData> monsters =
            ShopInventory.Instance.OwnedMonsters;

        if (m_selectedIndex >= monsters.Count)
            return;

        m_teamIndices[slotIndex] = m_selectedIndex;
        teamSlots[slotIndex] = monsters[m_selectedIndex];

        ShowMessage("Monster added to team!");
        RefreshSlots();
    }

    public void RemoveMonster(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= teamSlots.Length)
            return;

        teamSlots[slotIndex] = null;
        m_teamIndices[slotIndex] = -1;

        RefreshSlots();
    }

    public void ClearTeam()
    {
        for (int i = 0; i < teamSlots.Length; i++)
        {
            teamSlots[i] = null;
            m_teamIndices[i] = -1;
        }

        RefreshSlots();
        ShowMessage("Team cleared!");
    }

    public List<CharacterData> GetLineup()
    {
        List<CharacterData> lineup = new List<CharacterData>();
        for (int i = teamSlots.Length - 1; i >= 0; i--) // starts at "right" (end) since right is the front of the lineup and is spawned in first
        {
            CharacterData monster = teamSlots[i];
            if (monster != null)
                lineup.Add(monster);
        }

        return lineup;
    }

    public void RefreshSlots()
    {
        for (int i = 0; i < slotUI.Length; i++)
        {
            if (slotUI[i] != null)
                slotUI[i].UpdateSlot();
        }
    }

    void ClearMonsterInfo()
    {
        if (nameText != null)
            nameText.text = "Select a Monster";

        if (damageText != null)
            damageText.text = "";

        if (healthText != null)
            healthText.text = "";

        if (descriptionText != null)
            descriptionText.text = "";

        if (messageText != null)
            messageText.text = "";
    }

    void ShowMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;

        Debug.Log(message);
    }
}
