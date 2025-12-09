using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Health, MaxHealth;
    public float Armor; // Armor adds directly to total health display
    public float DamageBonus = 0f; // Damage boost from items
    [SerializeField] private HealthBar healthBar;
    private bool isDefending = false;
    [SerializeField] private GameObject GameOverContainer;
    
    private float baseMaxHealth; // Store original max health for items that modify it

    void Start()
    {
        baseMaxHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
        Health = MaxHealth;
        healthBar.SetHealth(Health);
        GameOverContainer.SetActive(false);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HayBale"))
        {
            TakeDamage(20f);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = isDefending ? damage * 0.5f : damage;
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            FindObjectOfType<PhaseManager>().EndPlayerDefeat();
        }
    }

    public void Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);
    }

    public void AddArmor(float amount)
    {
        // Update armor value and reflect it in MaxHealth so the UI shows the increase.
        Armor += amount;

        // Adjust MaxHealth to include armor change and clamp Health appropriately.
        MaxHealth += amount;
        Health = Mathf.Min(Health, MaxHealth);

        // Update health bar to reflect new max and current health.
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
            healthBar.SetHealth(Health);
        }
    }

    // Double the player's max health (for items)
    public void DoubleMaxHealth()
    {
        MaxHealth *= 2f;
        Health = MaxHealth;
        
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
            healthBar.SetHealth(Health);
        }
        Debug.Log("Max health doubled to: " + MaxHealth);
    }

    // Restore max health to base value (when unequipping item)
    public void RestoreMaxHealth()
    {
        MaxHealth = baseMaxHealth;
        Health = Mathf.Min(Health, MaxHealth);
        
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);
            healthBar.SetHealth(Health);
        }
        Debug.Log("Max health restored to: " + MaxHealth);
    }

    public void StartDefend()
    {
        isDefending = true;
        Invoke(nameof(StopDefend), 3f);
    }

    public void StopDefend()
    {
        isDefending = false;
    }

    // Add damage bonus from items
    public void AddDamageBonus(float amount)
    {
        DamageBonus += amount;
        Debug.Log("Damage bonus increased to: " + DamageBonus);
    }

    // Remove damage bonus from items
    public void RemoveDamageBonus(float amount)
    {
        DamageBonus -= amount;
        DamageBonus = Mathf.Max(0, DamageBonus);
        Debug.Log("Damage bonus reduced to: " + DamageBonus);
    }

    // Reset damage bonus to zero
    public void ResetDamageBonus()
    {
        DamageBonus = 0f;
        Debug.Log("Damage bonus reset to 0");
    }
}

