using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ShopItemUI : MonoBehaviour
{
    [Header("Monster Information")]
    public CharacterData characterData;

    [Header("Ability Information")]
    public AbilityData abilityData;

    private Button m_button;
    private ShopManager m_shopManager;

    void Start()
    {
        m_button = GetComponent<Button>();
        m_shopManager = FindFirstObjectByType<ShopManager>();

        if (m_shopManager == null)
        {
            Debug.LogError("Missing ShopManager!");
            return;
        }

        if (characterData != null)
        {
            if (characterData.characterSprite != null)
            {
                GetComponent<Image>().sprite =
                    characterData.characterSprite;
            }

            m_button.onClick.AddListener(SelectMonster);
        }
        else if (abilityData != null)
        {
            if (abilityData.abilitySprite != null)
            {
                GetComponent<Image>().sprite =
                    abilityData.abilitySprite;
            }

            m_button.onClick.AddListener(SelectAbility);
        }
        else
        {
            Debug.LogError("Missing CharacterData or AbilityData!");
        }
    }

    void SelectMonster()
    {
        m_shopManager.SelectMonster(characterData);
    }

    void SelectAbility()
    {
        m_shopManager.SelectAbility(abilityData);
    }

    public void TestClick()
    {
        Debug.Log("Button is working");
    }

    public void OnStartCombatClicked()
    {
        GameManager.Instance.StartCombat();
    }
}