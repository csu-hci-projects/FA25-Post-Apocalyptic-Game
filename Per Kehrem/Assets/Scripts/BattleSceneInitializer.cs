using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneInitializer : MonoBehaviour
{
    [Tooltip("Name of the boss being fought (e.g., 'Boss1', 'Boss2')")]
    public string bossName;

    private void Start()
    {
        // Find the PlayerHealth component in the battle scene
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        
        if (playerHealth != null && GameData.Instance != null)
        {
            // Load player stats from the previous scene
            GameData.Instance.LoadPlayerStats(playerHealth);
            Debug.Log("Battle scene initialized with player stats");
        }
        else if (playerHealth == null)
        {
            Debug.LogWarning("BattleSceneInitializer: PlayerHealth not found in battle scene!");
        }
        else if (GameData.Instance == null)
        {
            Debug.LogWarning("BattleSceneInitializer: GameData not found!");
        }
    }

    /// <summary>
    /// Call this when the battle ends (either victory or defeat)
    /// </summary>
    public void ReturnToOverworld()
    {
        if (GameData.Instance == null)
        {
            Debug.LogError("GameData not found!");
            return;
        }

        // Increment battle return counter
        GameData.Instance.IncrementBattleReturnCount();

        // Report boss defeated to BossManager before returning (if it exists)
        if (!string.IsNullOrEmpty(bossName))
        {
            // Search all MonoBehaviours in scene for one with ReportBossDefeatedByName method
            MonoBehaviour[] allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (MonoBehaviour script in allScripts)
            {
                var reportMethod = script.GetType().GetMethod("ReportBossDefeatedByName");
                if (reportMethod != null)
                {
                    reportMethod.Invoke(script, new object[] { bossName });
                    Debug.Log("BattleSceneInitializer: Boss defeat reported for: " + bossName);
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("BattleSceneInitializer: bossName not assigned!");
        }

        string savedScene = GameData.Instance.GetSavedSceneName();
        if (string.IsNullOrEmpty(savedScene))
        {
            Debug.LogWarning("No saved scene found! Loading main menu.");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Load the saved scene
        SceneManager.LoadScene(savedScene);
    }

    /// <summary>
    /// Call this when the overworld scene loads after battle
    /// </summary>
    public void RestorePlayerPosition()
    {
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null && GameData.Instance != null)
        {
            GameData.Instance.LoadLocation(playerHealth.transform);
            Debug.Log("Player position restored");
        }
    }
}

