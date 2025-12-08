using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Health, MaxHealth;
    public float Armor; // Armor adds directly to total health display
    [SerializeField] private HealthBar healthBar;
    private bool isDefending = false;
    [SerializeField] private GameObject GameOverContainer;

    void Start()
    {
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

    public void StartDefend()
    {
        isDefending = true;
        Invoke(nameof(StopDefend), 3f);
    }

    public void StopDefend()
    {
        isDefending = false;
    }

}

