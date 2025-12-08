using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float Health, MaxHealth;
    public float Armor; // Armor adds directly to total health display
    [SerializeField] private BossHealthBar healthBar;

    void Start()
    {
        Health = MaxHealth;
        healthBar.SetHealth(Health);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);
        Debug.Log("boss health now : " + Health);
    }

    public void AttackBoss(float damage)
    {
        TakeDamage(damage);
    }

}

