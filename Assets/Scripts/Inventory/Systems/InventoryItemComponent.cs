using UnityEngine;

class InventoryItemComponent : MonoBehaviour
{
    public InventoryItem item;                  // the item data
    public bool AddToInventory = false;
    public bool RemoveFromInventory = false;
    public bool AddToInventoryTop = false;
    public bool Throwable = false;
    private void Start()
    {
        if (item != null)
            item.Prefab = this.gameObject;
    }
}
