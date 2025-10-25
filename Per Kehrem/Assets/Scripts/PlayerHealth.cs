using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Health, MaxHealth;
    [SerializeField] private HealthBar healthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("d")){
            SetHealth(-20f);
        }
        if(Input.GetKeyDown("g")){
            SetHealth(20f);
        }
    }

    public void SetHealth(float healthChange){
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
    }
}
