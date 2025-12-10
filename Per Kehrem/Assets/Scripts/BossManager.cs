using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance { get; private set; }

    [Tooltip("References to all boss GameObjects in the scene")]
    public GameObject[] bosses;

    [Tooltip("Names of the bosses (must match the names you pass from battle scenes)")]
    public string[] bossNames;

    [Tooltip("Reference to the wall GameObject that blocks progress")]
    public GameObject wallObject;

    [Tooltip("Number of times player must return from battle to unlock door")]
    public int battlesRequiredToUnlock = 2;

    private bool[] bossDefeated;

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
        // Use bossNames if available, otherwise use bosses array
        int bossCount = (bossNames != null && bossNames.Length > 0) ? bossNames.Length : (bosses != null ? bosses.Length : 0);
        
        if (bossCount == 0)
        {
            Debug.LogError("BossManager: No bosses assigned (either bossNames or bosses array)!");
            return;
        }

        // Initialize defeated tracking
        bossDefeated = new bool[bossCount];
        for (int i = 0; i < bossDefeated.Length; i++)
        {
            bossDefeated[i] = false;
        }

        // Ensure wall is active initially
        if (wallObject != null)
            wallObject.SetActive(true);

        Debug.Log($"BossManager initialized with {bossCount} bosses");
    }

    /// <summary>
    /// Call this when a boss is defeated (using GameObject reference)
    /// </summary>
    public void ReportBossDefeated(GameObject boss)
    {
        if (boss == null)
        {
            Debug.LogWarning("BossManager: Boss is null!");
            return;
        }

        ReportBossDefeatedByName(boss.name);
    }

    /// <summary>
    /// Call this when a boss is defeated (using boss name)
    /// </summary>
    public void ReportBossDefeatedByName(string bossName)
    {
        if (string.IsNullOrEmpty(bossName))
        {
            Debug.LogWarning("BossManager: Boss name is null or empty!");
            return;
        }

        // Find the boss index by name
        int bossIndex = -1;
        if (bossNames != null)
        {
            for (int i = 0; i < bossNames.Length; i++)
            {
                if (bossNames[i] == bossName)
                {
                    bossIndex = i;
                    break;
                }
            }
        }

        // Fallback: try to match by GameObject name
        if (bossIndex == -1 && bosses != null)
        {
            for (int i = 0; i < bosses.Length; i++)
            {
                if (bosses[i] != null && bosses[i].name == bossName)
                {
                    bossIndex = i;
                    break;
                }
            }
        }

        if (bossIndex == -1)
        {
            Debug.LogWarning($"BossManager: Boss '{bossName}' not found!");
            return;
        }

        // Mark as defeated and deactivate GameObject if it exists
        bossDefeated[bossIndex] = true;
        if (bosses != null && bosses[bossIndex] != null)
        {
            bosses[bossIndex].SetActive(false);
        }
        Debug.Log($"BossManager: {bossName} defeated (Boss {bossIndex + 1}/{bossDefeated.Length})");

        // Check if door should unlock based on battle returns
        CheckAndUnlockDoor();
    }

    /// <summary>
    /// Check if all bosses have been defeated
    /// </summary>
    public bool AreAllBossesDefeated()
    {
        if (bossDefeated == null) return false;
        
        for (int i = 0; i < bossDefeated.Length; i++)
        {
            if (!bossDefeated[i])
                return false;
        }
        return true;
    }





    /// <summary>
    /// Check if door should unlock based on battle return count
    /// </summary>
    private void CheckAndUnlockDoor()
    {
        if (GameData.Instance == null) return;

        if (GameData.Instance.battleReturnCount >= battlesRequiredToUnlock)
        {
            UnlockDoor();
        }
    }

    /// <summary>
    /// Unlock the door/wall
    /// </summary>
    private void UnlockDoor()
    {
        if (wallObject != null && wallObject.activeSelf)
        {
            wallObject.SetActive(false);
            Debug.Log($"BossManager: Door unlocked after {GameData.Instance.battleReturnCount} battle returns!");
        }
    }
}
