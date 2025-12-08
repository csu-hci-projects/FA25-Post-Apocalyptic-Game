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
    
    // Reference to the original world instance this UI item was created from.
    // When an item is picked up we disable the world instance and keep a link
    // so we can respawn (reactivate) it later if the item is unequipped or replaced.
    [HideInInspector]
    public GameObject worldInstance;
    [HideInInspector]
    public Vector3 worldPosition;
    [HideInInspector]
    public Quaternion worldRotation;

    // Link this UI item to its world GameObject and record original transform
    public void LinkWorldInstance(GameObject world)
    {
        if (world == null) return;
        worldInstance = world;
        worldPosition = world.transform.position;
        worldRotation = world.transform.rotation;
    }

    // Reactivate and place the original world instance back where it was
    public void RespawnWorldInstance()
    {
        if (worldInstance == null) return;
        worldInstance.transform.position = worldPosition;
        worldInstance.transform.rotation = worldRotation;
        worldInstance.SetActive(true);
        // Clear link so we don't try to respawn again
        worldInstance = null;
    }
}