using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ShopItemUI : MonoBehaviour
{
    [Header("Monster Information")]
    public CharacterData characterData;

    private Button m_button;
    private ShopManager m_shopManager;

    void Start()
    {
        m_button = GetComponent<Button>();
        m_shopManager = FindFirstObjectByType<ShopManager>();

        if (characterData == null || m_shopManager == null)
        {
            Debug.LogError("Missing CharacterData or ShopManager!");
            return;
        }

        if (characterData.characterSprite != null)
        {
            GetComponent<Image>().sprite = characterData.characterSprite;
        }

        m_button.onClick.AddListener(SelectMonster);
    }

    void SelectMonster()
    {
        Debug.Log("Rat was clicked");
        m_shopManager.SelectMonster(characterData);
    }

    public void TestClick()
    {
        Debug.Log("Button is working");
    }
}