using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Tooltip("Assign all inventory grid slots here.")]
    public InventorySlot[] inventorySlots;

    [Tooltip("Assign equipment slots here (Head, Chest, Legs, Hands, Feet, Accessory).")]
    public InventorySlot[] equipmentSlots;

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
}