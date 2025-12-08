using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public bool isEquipmentSlot = false;
    public EquipmentType equipmentType = EquipmentType.None;

    private Item currentItem;
    private InventoryManager inventoryManager;
    private Button dragButton;

    public enum EquipmentType
    {
        None,
        Head,
        Chest,
        Legs,
        Hands,
        leftFoot,
        rightFoot,
        Accessory
    }

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        dragButton = GetComponent<Button>();
        
        if (dragButton == null)
            dragButton = gameObject.AddComponent<Button>();

        dragButton.onClick.AddListener(OnSlotClick);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;

        Item draggedItem = droppedObject.GetComponent<Item>();
        if (draggedItem == null) return;

        InventorySlot sourceSlot = draggedItem.parentSlot;
        if (sourceSlot == null) return;

        // If dragging to equipment slot, check compatibility
        if (isEquipmentSlot && !CanEquipItem(draggedItem))
        {
            Debug.LogWarning($"Cannot equip {draggedItem.stats.itemName} to {equipmentType}");
            return;
        }

        // Swap items
        SwapItems(sourceSlot, this);
    }

    private bool CanEquipItem(Item item)
    {
        // You can add logic here to determine which items fit which slots
        return true;
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        
        if (item != null)
        {
            item.SetParentSlot(this);
            if (itemIcon != null)
                itemIcon.sprite = item.stats.icon;
            if (itemNameText != null)
                itemNameText.text = item.stats.itemName;
            itemIcon.enabled = true;
        }
        else
        {
            if (itemIcon != null)
                itemIcon.enabled = false;
            if (itemNameText != null)
                itemNameText.text = "";
        }
    }

    public Item GetItem()
    {
        return currentItem;
    }

    public void ClearSlot()
    {
        currentItem = null;
        if (itemIcon != null)
            itemIcon.enabled = false;
        if (itemNameText != null)
            itemNameText.text = "";
    }

    private void SwapItems(InventorySlot sourceSlot, InventorySlot targetSlot)
    {
        Item sourceItem = sourceSlot.GetItem();
        Item targetItem = targetSlot.GetItem();

        sourceSlot.SetItem(targetItem);
        targetSlot.SetItem(sourceItem);

        // If item was moved to equipment slot, apply stats
        if (targetSlot.isEquipmentSlot && sourceItem != null)
        {
            inventoryManager.EquipItem(sourceItem, targetSlot.equipmentType);
        }
        else if (sourceSlot.isEquipmentSlot && targetItem != null)
        {
            inventoryManager.UnequipItem(sourceSlot.equipmentType);
        }
    }

    private void OnSlotClick()
    {
        // Optional: Add click functionality
    }
}