using System;
using UnityEngine;

public class MenuComtroler : MonoBehaviour
{
    public GameObject menuCanvas;
    [Tooltip("List of tab GameObjects (assign in Inspector).")]
    public GameObject[] tabs;
    [Tooltip("Name of the default tab to show when the menu opens (case-insensitive).")]
    public string defaultTabName = "Player";

    void Start()
    {
        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        DeactivateAllTabs();
        ActivateDefaultTab();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab pressed");
            ToggleMenu();
        }
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
