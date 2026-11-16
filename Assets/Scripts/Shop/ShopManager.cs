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

    void Start()
    {
        purchaseButton.onClick.AddListener(PurchaseMonster);
        UpdateCoins();
    }

    public void SelectMonster(CharacterData monster)
    {
        m_selectedMonster = monster;

        nameText.text = monster.characterName;
        descriptionText.text = monster.description;
        priceText.text = monster.price.ToString();

        messageText.text = "";
        
        Debug.Log("Selected: " + monster.characterName);
    }

    void PurchaseMonster()
    {
        if (m_selectedMonster == null)
            return;

        if (ShopInventory.Instance.Coins < m_selectedMonster.price)
        {
            messageText.text = "Not enough coins!";
            return;
        }

        ShopInventory.Instance.SpendCoins(m_selectedMonster.price);

        ShopInventory.Instance.AddMonster(m_selectedMonster.characterName);

        messageText.text = "Purchased " + m_selectedMonster.characterName + "!";

        UpdateCoins();
    }

    void UpdateCoins()
    {
        coinText.text = "Coins: " + ShopInventory.Instance.Coins;
    }
}