using UnityEngine;

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

    public PlayerStats playerStats;

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
}
