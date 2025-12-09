using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Tooltip("Assign all inventory grid slots here.")]
    public InventorySlot[] inventorySlots;

    [Tooltip("Assign equipment slots here (Head, Chest, Legs, Hands, Feet, Accessory).")]
    public InventorySlot[] equipmentSlots;

    public GameObject itemUIPrefab;
    
    [Tooltip("Assign the damage display text (optional).")]
    public TextMeshProUGUI damageDisplayText;

    private PlayerHealth playerHealth;
    private Dictionary<InventorySlot.EquipmentType, Item> equippedItems = new Dictionary<InventorySlot.EquipmentType, Item>();

    private void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth == null)
            Debug.LogError("InventoryManager: PlayerHealth not found!");

        // Initialize equipped items dictionary
        foreach (InventorySlot slot in equipmentSlots)
        {
            equippedItems[slot.equipmentType] = null;
        }
        
        UpdateDamageDisplay();
    }

    public void EquipItem(Item item, InventorySlot.EquipmentType equipmentType)
    {
        if (item == null) return;

        InventorySlot.EquipmentType slotType = item.equipmentType;
        // If there's an item already equipped in this slot, remove its stats,
        // respawn its original world instance and remove the UI representation.
        if (equippedItems[equipmentType] != null)
        {
            Item prev = equippedItems[equipmentType];
            // remove previous item stats
            RemoveItemStats(prev);

            // respawn the world object where it was (if linked)
            prev.RespawnWorldInstance();

            // Remove it from its UI slot (if any) and destroy the UI object
            if (prev.parentSlot != null)
            {
                prev.parentSlot.SetItem(null);
            }
            Destroy(prev.gameObject);
        }

        // Apply new item stats and record it as equipped
        equippedItems[equipmentType] = item;
        ApplyItemStats(item);

        Debug.Log($"Equipped: {item.stats.itemName} to {equipmentType}");
        UpdateDamageDisplay();
    }

    public void UnequipItem(InventorySlot.EquipmentType equipmentType)
    {
        if (equippedItems[equipmentType] != null)
        {
            RemoveItemStats(equippedItems[equipmentType]);
            equippedItems[equipmentType] = null;
            Debug.Log($"Unequipped item from {equipmentType}");
            UpdateDamageDisplay();
        }
    }

    private void ApplyItemStats(Item item)
    {
        if (playerHealth == null) return;

        if (item.stats.healthBonus > 0)
        {
            if (item.stats.healthBonus >= 100)
            {
                playerHealth.DoubleMaxHealth();
            }
            else
            {
                playerHealth.MaxHealth += item.stats.healthBonus;
                // Only add the bonus to current health, don't heal to full
                playerHealth.Health = Mathf.Min(playerHealth.Health + item.stats.healthBonus, playerHealth.MaxHealth);
            }
        }

        if (item.stats.armorBonus > 0)
        {
            playerHealth.AddArmor(item.stats.armorBonus);
        }

        if (item.stats.damageBonus > 0)
        {
            playerHealth.AddDamageBonus(item.stats.damageBonus);
            Debug.Log($"Damage bonus applied: +{item.stats.damageBonus}");
        }
    }

    private void RemoveItemStats(Item item)
    {
        if (playerHealth == null) return;

        if (item.stats.healthBonus > 0)
        {
            // If healthBonus is 100 or more, restore the original max health
            if (item.stats.healthBonus >= 100)
            {
                playerHealth.RestoreMaxHealth();
            }
            else
            {
                playerHealth.MaxHealth -= item.stats.healthBonus;
                playerHealth.Health = Mathf.Min(playerHealth.Health, playerHealth.MaxHealth);
            }
        }

        if (item.stats.armorBonus > 0)
        {
            playerHealth.AddArmor(-item.stats.armorBonus);
        }

        if (item.stats.damageBonus > 0)
        {
            playerHealth.AddDamageBonus(-item.stats.damageBonus);
        }
    }

    public Item GetEquippedItem(InventorySlot.EquipmentType equipmentType)
    {
        return equippedItems[equipmentType];
    }

    private void UpdateDamageDisplay()
    {
        if (damageDisplayText != null && playerHealth != null)
        {
            damageDisplayText.text = $"Damage: {playerHealth.DamageBonus:F1}";
        }
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

    // Link the UI item back to the original world instance and disable the world object
    uiItem.LinkWorldInstance(worldItem.gameObject);

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

        // disable the world instance instead of destroying it so it can be respawned later
        worldItem.gameObject.SetActive(false);
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
            
            // Apply item stats when picked up
            ApplyItemStats(uiItem);
            UpdateDamageDisplay();
            
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

    // disable the world instance instead of destroying it so it can be respawned later
    worldItem.gameObject.SetActive(false);
    Debug.Log($"Picked up: {uiItem.stats.itemName}");
}

}