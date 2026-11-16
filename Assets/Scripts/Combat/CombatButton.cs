using UnityEngine;

public class CombatButton : MonoBehaviour
{
    [SerializeField] private CombatManager combatManager;
    public void OnStartButtonClick()
    {
        combatManager.StartCombatSequence(); 
    }
}
