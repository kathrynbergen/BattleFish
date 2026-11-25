using UnityEngine;
using UnityEngine.SceneManagement;

// note: gamemanager is a singleton
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CombatManager combatManager;
    
    [Header("Canvases")]
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Canvas teamBuilderCanvas;
    [SerializeField] private Canvas combatCanvas;
    [SerializeField] private Canvas combatResultCanvas;
    
    [Header("UI Manager")]
    [SerializeField] private CombatResultUI combatResultUI;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private TeamBuilderManager teamBuilderManager;

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
        teamBuilderCanvas.enabled = false;
        combatCanvas.enabled = false;
        
        if (combatResultCanvas != null) combatResultCanvas.enabled = false;
        combatManager.AddWaveCharacters(currentWave);
    }

    // Shop's next button -> goes to teambuilderCanvas
    public void GoToTeamBuilder()
    {
        Debug.Log("opening team builder");
        shopCanvas.enabled = false;
        teamBuilderCanvas.enabled = true;

        if (teamBuilderManager != null)
        {
            Debug.Log("loading monster inventory");
            teamBuilderManager.LoadOwnedMonsters();
        }
        else
        {
            Debug.LogError("Team Builder Manager is not assigned");
        }
    }
    
    // teambuilderCanvas's back button
    public void BackToShop()
    {
        teamBuilderCanvas.enabled = false;
        shopCanvas.enabled = true;
    }
    
    public void StartCombat() // starts from teambuilderCanvas now
    {
        print("start combat");
        // turn off teambuilder
        teamBuilderCanvas.enabled = false;
        // turn off shop
        shopCanvas.enabled = false;
        // turn on combat scene
        combatCanvas.enabled = true;
        // tell combatmanager to setup next combat wave
        combatManager.SetupCombat(currentWave, teamBuilderManager.GetLineup());
    }

    public void EndCombat(bool playerWon)
    {
        lastCombatWon = playerWon;
        if (playerWon && ShopInventory.Instance != null)
        {
            ShopInventory.Instance.AddCoins(500);
        }
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

        if (currentWave >= 5)
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        currentWave++;
        combatManager.AddWaveCharacters(currentWave);
        
        teamBuilderCanvas.enabled = false;
        combatCanvas.enabled = false;
        
        if (shopCanvas != null) shopCanvas.enabled = true;
        if (shopManager != null) shopManager.UpdateCoins();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
