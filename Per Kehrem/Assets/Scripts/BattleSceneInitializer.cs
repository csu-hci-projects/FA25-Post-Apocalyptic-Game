using UnityEngine;

public class BattleSceneInitializer : MonoBehaviour
{
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
}
