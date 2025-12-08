using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public float BossMaxHealth = 40f;
    public float BossHealth;
    public float BossWidth = 200f;
    public float BossHeight = 20f;
    [SerializeField] private RectTransform barFill;

    void Start()
    {
        BossHealth = BossMaxHealth;
        SetHealth(BossHealth);
    }

    void onEnable()
    {
        SetHealth(BossHealth);
    }

    public void SetHealth(float health)
    {
        BossHealth = health;
        float newWidth = (BossHealth / BossMaxHealth) * BossWidth;
        barFill.sizeDelta = new Vector2(newWidth, BossHeight);
    }
}

