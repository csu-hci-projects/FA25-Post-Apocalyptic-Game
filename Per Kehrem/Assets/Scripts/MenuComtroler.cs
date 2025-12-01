using System;
using UnityEngine;

public class MenuComtroler : MonoBehaviour
{
    public GameObject menuCanvas;
    [Tooltip("List of tab GameObjects (assign in Inspector).")]
    public GameObject[] tabs;
    [Tooltip("Name of the default tab to show when the menu opens (case-insensitive).")]
    public string defaultTabName = "Player";
    
    [Header("Audio")]
    [Tooltip("Master volume slider (0-1).")]
    public UnityEngine.UI.Slider volumeSlider;

    void Start()
    {
        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        DeactivateAllTabs();
        ActivateDefaultTab();
        InitializeVolumeSlider();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab pressed");
            ToggleMenu();
        }
    }

    // Initialize volume slider
    private void InitializeVolumeSlider()
    {
        if (volumeSlider == null) return;
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Set master volume
    public void SetVolume(float value)
    {
        AudioListener.volume = Mathf.Clamp01(value);
    }

    // Unstuck the player by resetting position to spawn point or safe location
    public void UnstuckPlayer()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogWarning("Player not found. Make sure the player GameObject has the 'Player' tag.");
            return;
        }

        // Reset to spawn point or safe location (adjust coordinates as needed)
        playerTransform.position = new Vector3(0, 1, 0);
        playerTransform.rotation = Quaternion.identity;
        Debug.Log("Player unstuck and reset to safe position.");
    }

    // Quit to main menu
    public void QuitToMenu()
    {
        Time.timeScale = 1f; // Ensure game time is running
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameMenu");
    }

    // Quit the application
    public void QuitGame()
    {
        Time.timeScale = 1f; // Ensure game time is running
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Toggle the menu open/closed. When opening, activate the default tab.
    public void ToggleMenu()
    {
        if (menuCanvas == null) return;
        bool opening = !menuCanvas.activeSelf;
        menuCanvas.SetActive(opening);
        if (opening)
            ActivateDefaultTab();
    }

    // Deactivate all tabs
    private void DeactivateAllTabs()
    {
        if (tabs == null) return;
        foreach (var t in tabs)
            if (t != null)
                t.SetActive(false);
    }

    // Activate a tab by its GameObject name (useful for Button onClick wiring)
    public void ActivateTab(string tabName)
    {
        if (tabs == null) return;
        foreach (var t in tabs)
        {
            if (t == null) continue;
            if (string.Equals(t.name, tabName, StringComparison.OrdinalIgnoreCase))
            {
                DeactivateAllTabs();
                t.SetActive(true);
                return;
            }
        }
        Debug.LogWarning($"Tab '{tabName}' not found in MenuComtroler.tabs");
    }

    // Activate a tab by its index in the `tabs` array
    public void ActivateTabByIndex(int index)
    {
        if (tabs == null) return;
        if (index < 0 || index >= tabs.Length)
        {
            Debug.LogWarning($"Tab index {index} is out of range (0..{Math.Max(0, tabs.Length - 1)})");
            return;
        }
        DeactivateAllTabs();
        if (tabs[index] != null)
            tabs[index].SetActive(true);
    }

    // Try to activate the named default tab; fall back to first tab if not found
    private void ActivateDefaultTab()
    {
        if (tabs == null || tabs.Length == 0) return;

        foreach (var t in tabs)
        {
            if (t == null) continue;
            if (string.Equals(t.name, defaultTabName, StringComparison.OrdinalIgnoreCase))
            {
                DeactivateAllTabs();
                t.SetActive(true);
                return;
            }
        }

        // fallback to first tab
        DeactivateAllTabs();
        if (tabs[0] != null)
            tabs[0].SetActive(true);
    }
}
