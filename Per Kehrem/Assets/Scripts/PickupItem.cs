using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item item;   // The Item component on the world object

    private InventoryManager inventory;

    private void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
    }

    private void OnMouseDown()
    {
        if (inventory == null || item == null) return;

        Debug.Log($"Picking up: {item.stats.itemName}");
        inventory.PickupItem(item);
    }
}