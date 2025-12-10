using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    [System.Serializable]
    public class PlayerStats
    {
        public float health;
        public float maxHealth;
        public float armor;
        public float damageBonus;
    }

    [System.Serializable]
    public class LocationData
    {
        public string sceneName;
        public Vector3 playerPosition;
    }

    public PlayerStats playerStats;
    public LocationData locationData;

    // Track how many times player has returned from battle
    public int battleReturnCount = 0;

    private void Awake()
    {
        // Singleton pattern - ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        // Listen for scene loads so we can restore player location when returning
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Save player stats before transitioning to battle scene
    /// </summary>
    public void SavePlayerStats(PlayerHealth playerHealth)
    {
        if (playerHealth == null)
        {
            Debug.LogError("GameData: PlayerHealth is null!");
            return;
        }

        playerStats = new PlayerStats
        {
            health = playerHealth.Health,
            maxHealth = playerHealth.MaxHealth,
            armor = playerHealth.Armor,
            damageBonus = playerHealth.DamageBonus
        };

        Debug.Log($"Player stats saved - Health: {playerStats.health}/{playerStats.maxHealth}, Armor: {playerStats.armor}, Damage: {playerStats.damageBonus}");
    }

    /// <summary>
    /// Load player stats to battle scene
    /// </summary>
    public void LoadPlayerStats(PlayerHealth playerHealth)
    {
        if (playerHealth == null)
        {
            Debug.LogError("GameData: PlayerHealth is null!");
            return;
        }

        if (playerStats == null)
        {
            Debug.LogWarning("GameData: No player stats to load!");
            return;
        }

        playerHealth.Health = playerStats.health;
        playerHealth.MaxHealth = playerStats.maxHealth;
        playerHealth.Armor = playerStats.armor;
        playerHealth.DamageBonus = playerStats.damageBonus;

        Debug.Log($"Player stats loaded - Health: {playerHealth.Health}/{playerHealth.MaxHealth}, Armor: {playerHealth.Armor}, Damage: {playerHealth.DamageBonus}");
    }

    /// <summary>
    /// Clear saved stats
    /// </summary>
    public void ClearPlayerStats()
    {
        playerStats = null;
    }

    /// <summary>
    /// Save the player's current location before battle
    /// </summary>
    public void SaveLocation(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            Debug.LogError("GameData: playerTransform is null!");
            return;
        }

        locationData = new LocationData
        {
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            playerPosition = playerTransform.position
        };

        Debug.Log($"Location saved - Scene: {locationData.sceneName}, Position: {locationData.playerPosition}");
    }

    /// <summary>
    /// Load the player back to their saved location after battle
    /// </summary>
    public void LoadLocation(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            Debug.LogError("GameData: playerTransform is null!");
            return;
        }

        if (locationData == null)
        {
            Debug.LogWarning("GameData: No location data to load!");
            return;
        }

        playerTransform.position = locationData.playerPosition;
        Debug.Log($"Location loaded - Position: {locationData.playerPosition}");
    }

    /// <summary>
    /// Get the saved scene name
    /// </summary>
    public string GetSavedSceneName()
    {
        return locationData?.sceneName;
    }

    /// <summary>
    /// Clear saved location
    /// </summary>
    public void ClearLocation()
    {
        locationData = null;
    }

    /// <summary>
    /// Increment the battle return counter when player returns from a battle
    /// </summary>
    public void IncrementBattleReturnCount()
    {
        battleReturnCount++;
        Debug.Log($"GameData: Battle return count incremented to {battleReturnCount}");
    }

    /// <summary>
    /// Reset the battle return counter
    /// </summary>
    public void ResetBattleReturnCount()
    {
        battleReturnCount = 0;
        Debug.Log("GameData: Battle return count reset to 0");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we have a saved location and the loaded scene matches, restore the player position
        if (locationData == null) return;
        if (scene.name != locationData.sceneName) return;

        // Wait one frame to ensure player objects have been initialized in the scene
        InstanceStartCoroutine(RestorePlayerNextFrame());
    }

    // Helper to start coroutine from a static context on the singleton instance
    private void InstanceStartCoroutine(System.Collections.IEnumerator routine)
    {
        if (Instance != null)
            Instance.StartCoroutine(routine);
    }

    private System.Collections.IEnumerator RestorePlayerNextFrame()
    {
        yield return null; // wait one frame

        // Try to find PlayerHealth component
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null)
        {
            LoadLocation(playerHealth.transform);
            // Clear location so we don't reapply accidentally
            ClearLocation();
        }
        else
        {
            Debug.LogWarning("GameData: Could not find PlayerHealth to restore location after scene load.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
