using UnityEngine;

class InventoryItemComponent : MonoBehaviour
{
    public InventoryItem item;                  // the item data
    public bool AddToInventory = false;         // adds the item to the inventory
    public bool RemoveFromInventory = false;    // removes the item from the inventory
}
