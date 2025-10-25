using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float Health, MaxHealth, Width, Height;
    [SerializeField] private RectTransform healthBar;

    public void SetmaxHealth(float maxHealth){
        MaxHealth = maxHealth;
    }

    public void SetHealth(float health){
        Health = health;
        float newWidth = (Health / MaxHealth) * Width;

        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }
}
