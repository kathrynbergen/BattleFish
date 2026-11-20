using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TeamSlotUI : MonoBehaviour
{
    public int slotIndex;

    public Image pedestalImage;
    public Image monsterImage;

    public Sprite normalSprite;
    public Sprite selectedSprite;

    private TeamBuilderManager m_manager;
    private Button m_button;

    void Start()
    {
        m_manager = FindFirstObjectByType<TeamBuilderManager>();

        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnSlotClicked);

        UpdateSlot();
    }

    void OnSlotClicked()
    {
        if (m_manager == null)
            return;

        m_manager.AssignMonster(slotIndex);
    }

    public void UpdateSlot()
    {
        if (m_manager == null)
            m_manager = FindFirstObjectByType<TeamBuilderManager>();

        if (m_manager == null)
            return;

        CharacterData monster = m_manager.teamSlots[slotIndex];

        if (monster != null)
        {
            monsterImage.sprite = monster.characterSprite;
            monsterImage.enabled = monster.characterSprite != null;

            pedestalImage.sprite = selectedSprite;
        }
        else
        {
            monsterImage.sprite = null;
            monsterImage.enabled = false;

            pedestalImage.sprite = normalSprite;
        }
    }
}