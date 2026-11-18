using TMPro;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentWaveText;

    public void UpdateWaveUI(int currentWave)
    {
        if (currentWaveText == null)
        {
            Debug.LogError("CombatUI is missing its Current Wave Text reference.", this);
            return;
        }

        currentWaveText.text = $"Current wave: {currentWave}/5";
    }
}
