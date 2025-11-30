using UnityEngine;
using UnityEngine.EventSystems;

// Attach this to any GameObject you want to act as a tab button.
// It supports 3 kinds of clicks:
// - UI clicks (implements IPointerClickHandler)
// - Physics clicks on 3D objects with colliders (OnMouseDown)
// - Manual invocation via the public Activate() method
public class TabActivator : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Name of the tab GameObject to activate (case-insensitive).")]
    public string tabName;

    [Tooltip("Optional reference to a MenuComtroler. If empty, one will be found in the scene.")]
    public MenuComtroler menuController;

    void Start()
    {
        if (menuController == null)
            menuController = FindObjectOfType<MenuComtroler>();

        if (string.IsNullOrEmpty(tabName))
            Debug.LogWarning($"TabActivator on '{gameObject.name}' has an empty tabName.");
    }

    // For Unity UI (buttons, graphics) using EventSystem
    public void OnPointerClick(PointerEventData eventData)
    {
        Activate();
    }

    // For 3D GameObjects with Colliders (mouse clicks)
    void OnMouseDown()
    {
        Activate();
    }

    // Public method you can call from other scripts or Animator events
    public void Activate()
    {
        if (menuController == null)
        {
            menuController = FindObjectOfType<MenuComtroler>();
            if (menuController == null)
            {
                Debug.LogError("No MenuComtroler found in scene to activate tabs.");
                return;
            }
        }

        if (string.IsNullOrEmpty(tabName))
        {
            Debug.LogWarning($"TabActivator '{gameObject.name}' has no tabName set.");
            return;
        }

        menuController.ActivateTab(tabName);
    }
}
