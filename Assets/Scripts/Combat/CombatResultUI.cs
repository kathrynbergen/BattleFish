using TMPro;
using UnityEngine;

public class CombatResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;

    private void Awake()
    {
        if (resultText == null)
        {
            TMP_Text[] childTexts = GetComponentsInChildren<TMP_Text>(true);
            foreach (TMP_Text childText in childTexts)
            {
                if (childText.name == "WonLost-Text")
                {
                    resultText = childText;
                    break;
                }
            }
        }
    }

    public void ShowResult(bool playerWon, int wave)
    {
        if (resultText == null)
        {
            Debug.LogError("CombatResultUI is missing its result text reference.", this);
            return;
        }

        resultText.text = playerWon
            ? (wave >= 5 ? "You win level 1!" : $"You won Wave {wave}!")
            : $"You lost Wave {wave}.";
    }
}
