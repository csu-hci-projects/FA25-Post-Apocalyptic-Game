using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Tooltip("Assign all inventory grid slots here.")]
    public InventorySlot[] inventorySlots;

    [Tooltip("Assign equipment slots here (Head, Chest, Legs, Hands, Feet, Accessory).")]
    public InventorySlot[] equipmentSlots;

    public GameObject itemUIPrefab;

    private PlayerHealth playerHealth;
    private Dictionary<InventorySlot.EquipmentType, Item> equippedItems = new Dictionary<InventorySlot.EquipmentType, Item>();

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
            Debug.LogError("InventoryManager: PlayerHealth not found!");

        // Initialize equipped items dictionary
        foreach (InventorySlot slot in equipmentSlots)
        {
            equippedItems[slot.equipmentType] = null;
        }
    }

    public void EquipItem(Item item, InventorySlot.EquipmentType equipmentType)
    {
        if (item == null) return;

        InventorySlot.EquipmentType slotType = item.equipmentType;
        
        // Remove previous item stats if there was one
        if (equippedItems[equipmentType] != null)
            RemoveItemStats(equippedItems[equipmentType]);

        // Apply new item stats
        equippedItems[equipmentType] = item;
        ApplyItemStats(item);

        Debug.Log($"Equipped: {item.stats.itemName} to {equipmentType}");
    }

    public void UnequipItem(InventorySlot.EquipmentType equipmentType)
    {
        if (equippedItems[equipmentType] != null)
        {
            RemoveItemStats(equippedItems[equipmentType]);
            equippedItems[equipmentType] = null;
            Debug.Log($"Unequipped item from {equipmentType}");
        }
    }

    private void ApplyItemStats(Item item)
    {
        if (playerHealth == null) return;

        if (item.stats.healthBonus > 0)
        {
            playerHealth.MaxHealth += item.stats.healthBonus;
            playerHealth.Health = playerHealth.MaxHealth;
        }

        if (item.stats.armorBonus > 0)
        {
            playerHealth.AddArmor(item.stats.armorBonus);
        }

        // Damage bonus can be applied to player movement or attack system
        if (item.stats.damageBonus > 0)
        {
            Debug.Log($"Damage bonus applied: +{item.stats.damageBonus}");
        }
    }

    private void RemoveItemStats(Item item)
    {
        if (playerHealth == null) return;

        if (item.stats.healthBonus > 0)
        {
            playerHealth.MaxHealth -= item.stats.healthBonus;
            playerHealth.Health = Mathf.Min(playerHealth.Health, playerHealth.MaxHealth);
        }

        if (item.stats.armorBonus > 0)
        {
            playerHealth.AddArmor(-item.stats.armorBonus);
        }
    }

    public Item GetEquippedItem(InventorySlot.EquipmentType equipmentType)
    {
        return equippedItems[equipmentType];
    }
    public void PickupItem(Item worldItem)
{
    if (worldItem == null)
    {
        Debug.LogError("PickupItem: worldItem is null!");
        return;
    }

    if (itemUIPrefab == null)
    {
        Debug.LogError("PickupItem: itemUIPrefab is not assigned!");
        return;
    }

     // Create UI version
    GameObject uiItemObject = Instantiate(itemUIPrefab);
    Item uiItem = uiItemObject.GetComponent<Item>();
    if (uiItem == null)
    {
        Destroy(uiItemObject);
        return;
    }

    // Copy stats
    uiItem.stats = new Item.ItemStats
    {
        itemName = worldItem.stats.itemName,
        description = worldItem.stats.description,
        healthBonus = worldItem.stats.healthBonus,
        armorBonus = worldItem.stats.armorBonus,
        damageBonus = worldItem.stats.damageBonus,
        icon = worldItem.stats.icon
    };

    uiItem.isEquippable = worldItem.isEquippable;
    uiItem.equipmentType = worldItem.equipmentType;

    // If equippable, equip directly
    if (uiItem.isEquippable && uiItem.equipmentType != InventorySlot.EquipmentType.None)
    {
        EquipItem(uiItem, uiItem.equipmentType);

        // Set UI parent to the equipment slot for visuals
        InventorySlot targetSlot = null;
        foreach (var slot in equipmentSlots)
        {
            if (slot.equipmentType == uiItem.equipmentType)
            {
            targetSlot = slot;
            break;
            }
        }
        {
            targetSlot.SetItem(uiItem);
            uiItem.SetParentSlot(targetSlot);
            uiItem.transform.SetParent(targetSlot.transform, false);
        }

        Destroy(worldItem.gameObject);
        Debug.Log($"Equipped: {uiItem.stats.itemName}");
        return; // Done, skip normal inventory
    }

    // Otherwise, add to first empty inventory slot
    bool addedToInventory = false;
    foreach (InventorySlot slot in inventorySlots)
    {
        if (slot != null && slot.GetItem() == null)
        {
            slot.SetItem(uiItem);
            uiItem.SetParentSlot(slot);
            uiItem.transform.SetParent(slot.transform, false);
            addedToInventory = true;
            break;
        }
    }

    if (!addedToInventory)
    {
        Debug.LogWarning("Inventory full!");
        Destroy(uiItemObject);
        return;
    }

    Destroy(worldItem.gameObject);
    Debug.Log($"Picked up: {uiItem.stats.itemName}");
}

}