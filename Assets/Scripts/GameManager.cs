using UnityEngine;
using UnityEngine.SceneManagement;

// note: gamemanager is a singleton
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CombatManager combatManager;
    
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Canvas combatCanvas;
    [SerializeField] private Canvas combatResultCanvas;
    [SerializeField] private CombatResultUI combatResultUI;

    private int currentWave = 1;
    private bool lastCombatWon;

    public int CurrentWave => currentWave;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        shopCanvas.enabled = true;
        combatCanvas.enabled = false;
        if (combatResultCanvas != null) combatResultCanvas.enabled = false;
    }

    public void StartCombat()
    {
        print("start combat");
        // turn off shop
        shopCanvas.enabled = false;
        // turn on combat scene
        combatCanvas.enabled = true;
        // tell combatmanager to setup next combat wave
        combatManager.SetupCombat(currentWave);
    }

    public void EndCombat(bool playerWon)
    {
        lastCombatWon = playerWon;
        if (combatCanvas != null) combatCanvas.enabled = false;
        if (combatResultCanvas != null) combatResultCanvas.enabled = true;
        if (combatResultUI != null)
        {
            combatResultUI.ShowResult(playerWon, currentWave);
        }
        else
        {
            Debug.Log("combatResultUI is null");
        }
    }

    public void ContinueAfterCombat()
    {
        if (combatResultCanvas != null) combatResultCanvas.enabled = false;

        if (!lastCombatWon)
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        currentWave++;
        if (shopCanvas != null) shopCanvas.enabled = true;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
