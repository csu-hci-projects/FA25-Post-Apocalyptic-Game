using TMPro;
using UnityEngine;

// Attach this script to a UI Canvas or UI management object.
// It will automatically find the PlayerHealth component and update text boxes with current health/armor.
// It can also auto-create TextMeshProUGUI components on child GameObjects named "HP" and "Armor".
public class HealthDisplayUI : MonoBehaviour
{
    [Tooltip("TextMeshPro component that displays total health (Health + Armor).")]
    public TextMeshProUGUI healthText;

    [Tooltip("TextMeshPro component that displays current armor amount.")]
    public TextMeshProUGUI armorText;

    [Tooltip("If true, auto-setup will find/create text components on children named 'HP' and 'Armor'.")]
    public bool autoSetupTextComponents = true;

    [Tooltip("Optional: PlayerHealth component. If not assigned, will search for it in the scene.")]
    public PlayerHealth playerHealth;

    private void Start()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
            Debug.LogError("HealthDisplayUI: PlayerHealth component not found in the scene.");

        // Auto-setup text components if enabled
        if (autoSetupTextComponents)
            AutoSetupTextComponents();

        if (healthText == null)
            Debug.LogWarning("HealthDisplayUI: healthText is not assigned.");

        if (armorText == null)
            Debug.LogWarning("HealthDisplayUI: armorText is not assigned.");
    }

    private void AutoSetupTextComponents()
    {
        // Find or create HP text
        Transform hpTransform = transform.Find("HP");
        if (hpTransform != null)
        {
            healthText = hpTransform.GetComponent<TextMeshProUGUI>();
            if (healthText == null)
            {
                healthText = hpTransform.gameObject.AddComponent<TextMeshProUGUI>();
                ConfigureTextComponent(healthText);
            }
        }

        // Find or create Armor text
        Transform armorTransform = transform.Find("Armor");
        if (armorTransform != null)
        {
            armorText = armorTransform.GetComponent<TextMeshProUGUI>();
            if (armorText == null)
            {
                armorText = armorTransform.gameObject.AddComponent<TextMeshProUGUI>();
                ConfigureTextComponent(armorText);
            }
        }
    }

    private void ConfigureTextComponent(TextMeshProUGUI tmpText)
    {
        tmpText.text = "0";
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.fontSize = 24;
    }

    private void Update()
    {
        if (playerHealth == null)
            return;

        // Update health text with total (Health + Armor)
        if (healthText != null)
        {
            float totalHealth = playerHealth.Health + playerHealth.Armor;
            healthText.text = $"Health: {totalHealth:F0}";
        }

        // Update armor text
        if (armorText != null)
            armorText.text = $"Armor: {playerHealth.Armor:F0}";
    }
}
