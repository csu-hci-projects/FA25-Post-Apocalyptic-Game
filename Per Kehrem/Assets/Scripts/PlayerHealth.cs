using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Health, MaxHealth;
    public float Armor; // Armor adds directly to total health display
    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        healthBar.SetMaxHealth(MaxHealth);
        Health = MaxHealth;
        healthBar.SetHealth(Health);
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
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);
    }

    public void Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);
    }

    public void AddArmor(float amount)
    {
        Armor += amount;
    }
}

