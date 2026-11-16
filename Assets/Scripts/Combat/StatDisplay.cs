using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    private CharacterStats character;
    private TMP_Text[] statDisplayText;

    private void Awake()
    {
        character = GetComponentInParent<CharacterStats>();
        statDisplayText = GetComponentsInChildren<TMP_Text>(true);
    }

    private void LateUpdate()
    {
        if (character == null || character.data == null || statDisplayText.Length < 3) return;

        statDisplayText[0].text = $"{character.Health}/{character.data.health}";
        statDisplayText[1].text = $"{character.Damage}";
        statDisplayText[2].text = character.characterName;
    }
}
