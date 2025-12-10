using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [Tooltip("The win UI container to activate")]
    public GameObject winUI;

    private void Start()
    {
        // Ensure this collider is set as a trigger
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowWinUI();
        }
    }

    private void ShowWinUI()
    {
        if (winUI != null)
        {
            winUI.SetActive(true);
            Time.timeScale = 0f; // Pause game
            Debug.Log("WinTrigger: Win UI activated!");
        }
        else
        {
            Debug.LogWarning("WinTrigger: winUI not assigned!");
        }
    }
}
