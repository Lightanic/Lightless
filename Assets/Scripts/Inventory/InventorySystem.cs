using UnityEngine;
using Unity.Entities;


class InventorySystem : ComponentSystem
{
    /// <summary>
    /// Inventory item group
    /// </summary>
    struct ItemGroup
    {
        readonly public int Length;
        public ComponentArray<Pickup> PickItem;
        public ComponentArray<InventoryItemComponent> InventoryItem;
        public EntityArray entityArray;
    }

    /// <summary>
    /// Player inventory data
    /// </summary>
    struct InventoryData
    {
        readonly public int Length;
        public ComponentArray<InventoryComponent> Inventory;
    }

    [Inject] private ItemGroup itemData;
    [Inject] private InventoryData data;
    [Inject] EndFrameBarrier endFrameBarrier;
    /// <summary>
    /// Add or drop items from the inventory
    /// </summary>
    protected override void OnUpdate()
    {
        EntityCommandBuffer commandBuffer = endFrameBarrier.CreateCommandBuffer();
        Pickup pickedItem = null;
        bool isPicked = false;
     
        for(int i=0; i < itemData.Length; i++)
        {
            if (itemData.InventoryItem[i].AddToInventory)                                // Add item to the inventory and disable the item
            {
                data.Inventory[0].PlayerInventory.Add(itemData.InventoryItem[i].item);
                itemData.InventoryItem[i].AddToInventory = false;
                isPicked = true;                                                        // cannot destroy entitly while job is still running
                pickedItem = itemData.PickItem[i];                                      // once an item is picked break from loop to remove the item
                break;
            }
            else if (itemData.InventoryItem[i].RemoveFromInventory)                      // Remove the item from the inventory and enable the item
            {
                data.Inventory[0].PlayerInventory.Remove(itemData.InventoryItem[i].item);
                itemData.InventoryItem[i].RemoveFromInventory = false;
                itemData.PickItem[i].Drop();
            }
        }
        if(isPicked)
            pickedItem.Interact();                                                      // Remove the picked item from the scene
    }
}