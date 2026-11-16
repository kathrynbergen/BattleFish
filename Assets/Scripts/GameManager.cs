using Unity.VisualScripting;
using UnityEngine;

// note: gamemanager is a singleton
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CombatManager combatManager;
    
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Canvas combatCanvas;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        shopCanvas.enabled = true;
        combatCanvas.enabled = false;
    }

    public void StartCombat()
    {
        print("start combat");
        // turn off shop
        shopCanvas.enabled = false;
        // turn on combat scene
        combatCanvas.enabled = true;
        // tell combatmanager to setup next combat wave
        combatManager.SetupCombat();
    }
}
