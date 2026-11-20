using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MonsterInventoryItemUI : MonoBehaviour
{
    public Image monsterImage;
    public int InventoryIndex => m_inventoryIndex;

    private CharacterData m_monster;
    private TeamBuilderManager m_manager;
    private int m_inventoryIndex;
    private Button m_button;

    void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(SelectMonster);
    }

    public void SetMonster(
        CharacterData monster,
        TeamBuilderManager manager,
        int inventoryIndex)
    {
        m_monster = monster;
        m_manager = manager;
        m_inventoryIndex = inventoryIndex;

        if (monsterImage != null && monster != null)
        {
            monsterImage.sprite = monster.characterSprite;
            monsterImage.enabled = monster.characterSprite != null;
            monsterImage.color = Color.white;
        }
    }

    void SelectMonster()
    {
        if (m_manager != null)
        {
            m_manager.SelectMonster(m_inventoryIndex);
        }
    }

    public void SetSelected(bool selected)
    {
        // Slightly highlight the selected inventory box
        Image background = GetComponent<Image>();

        if (background != null)
        {
            background.color = selected
                ? new Color(1f, 0.85f, 0.55f)
                : Color.white;
        }
    }
}