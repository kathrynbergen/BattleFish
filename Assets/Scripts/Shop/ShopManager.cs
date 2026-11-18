
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text priceText;
    public TMP_Text messageText;

    public Button purchaseButton;

    private CharacterData m_selectedMonster;
    private AbilityData m_selectedAbility;

    void Start()
    {
        purchaseButton.onClick.AddListener(PurchaseSelectedItem);
        UpdateCoins();
    }

    public void SelectMonster(CharacterData monster)
    {
        if (monster == null)
            return;

        m_selectedMonster = monster;
        m_selectedAbility = null;

        nameText.text = monster.characterName;
        descriptionText.text = monster.description;
        priceText.text = monster.price.ToString();

        messageText.text = "";
    }

    public void SelectAbility(AbilityData ability)
    {
        if (ability == null)
            return;

        m_selectedAbility = ability;
        m_selectedMonster = null;

        nameText.text = ability.abilityName;
        descriptionText.text = ability.description;
        priceText.text = ability.price.ToString();

        messageText.text = "";
    }

    void PurchaseSelectedItem()
    {
        if (m_selectedMonster != null)
        {
            PurchaseMonster();
        }
        else if (m_selectedAbility != null)
        {
            PurchaseAbility();
        }
        else
        {
            messageText.text = "Select an item first!";
        }
    }

    void PurchaseMonster()
    {
        if (!ShopInventory.Instance.SpendCoins(m_selectedMonster.price))
        {
            messageText.text = "Not enough coins!";
            return;
        }

        ShopInventory.Instance.AddMonster(m_selectedMonster);

        messageText.text = "Purchased " + m_selectedMonster.characterName + "!";

        UpdateCoins();
    }

    void PurchaseAbility()
    {
        if (!ShopInventory.Instance.AbilitiesUnlocked())
        {
            messageText.text = "Abilities are locked in Level 1!";
            return;
        }

        if (!ShopInventory.Instance.SpendCoins(m_selectedAbility.price))
        {
            messageText.text = "Not enough coins!";
            return;
        }

        ShopInventory.Instance.AddAbility(m_selectedAbility);

        messageText.text = "Purchased " + m_selectedAbility.abilityName + "!";

        UpdateCoins();
    }

    public void UpdateCoins()
    {
        if (coinText != null && ShopInventory.Instance != null)
        {
            coinText.text = "Coins: " + ShopInventory.Instance.Coins;
        }
    }
}
