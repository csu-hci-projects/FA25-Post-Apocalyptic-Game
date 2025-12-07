using UnityEngine;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public class ItemStats
    {
        public string itemName = "Item";
        public string description = "";
        public int healthBonus = 0;
        public int armorBonus = 0;
        public int damageBonus = 0;
        public Sprite icon;
    }

     public ItemStats stats = new ItemStats();

    // Add this if it's equippable
    public bool isEquippable = false;
    public InventorySlot.EquipmentType equipmentType = InventorySlot.EquipmentType.None;

    public InventorySlot parentSlot;
    public void SetParentSlot(InventorySlot slot) => parentSlot = slot;
}