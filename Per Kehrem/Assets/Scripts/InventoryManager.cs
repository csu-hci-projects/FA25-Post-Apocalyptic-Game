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

        // Instantiate UI item
        GameObject uiItemObject = Instantiate(itemUIPrefab);

        Item uiItem = uiItemObject.GetComponent<Item>();
        if (uiItem == null)
        {
            Debug.LogError("PickupItem: itemUIPrefab is missing the Item component!");
            Destroy(uiItemObject);
            return;
        }

        // Copy stats so UI version does not share reference with world item
        uiItem.stats = new Item.ItemStats
        {
            itemName = worldItem.stats.itemName,
            description = worldItem.stats.description,
            healthBonus = worldItem.stats.healthBonus,
            armorBonus = worldItem.stats.armorBonus,
            damageBonus = worldItem.stats.damageBonus,
            icon = worldItem.stats.icon
        };

        // Find first empty slot
        bool addedToInventory = false;

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot != null && slot.GetItem() == null)
            {
                slot.SetItem(uiItem);
                uiItem.transform.SetParent(slot.transform, false);
                uiItem.SetParentSlot(slot);

                addedToInventory = true;
                break;
            }
        }

        if (!addedToInventory)
        {
            Debug.LogWarning("PickupItem: Inventory is full! Dropping item UI object.");
            Destroy(uiItemObject);
            return;
        }

        // Remove world item from scene
        Destroy(worldItem.gameObject);

        Debug.Log($"Picked up: {uiItem.stats.itemName}");
    }
}