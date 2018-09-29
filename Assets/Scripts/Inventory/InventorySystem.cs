using UnityEngine;
using Unity.Entities;


class InventorySystem : ComponentSystem
{
    /// <summary>
    /// Inventory item group
    /// </summary>
    struct ItemGroup
    {
        public Pickup PickItem;
        public InventoryItemComponent InventoryItem;
    }

    /// <summary>
    /// Player inventory data
    /// </summary>
    struct InventoryData
    {
        readonly public int Length;
        public ComponentArray<InventoryComponent> Inventory;
    }

    [Inject] private InventoryData data;

    /// <summary>
    /// Add or drop items from the inventory
    /// </summary>
    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<ItemGroup>())
        {
            if (entity.InventoryItem.AddToInventory)                                // Add item to the inventory and disable the item
            {
                data.Inventory[0].PlayerInventory.Add(entity.InventoryItem.item);
                entity.InventoryItem.AddToInventory = false;
                entity.PickItem.Interact();
            }
            else if (entity.InventoryItem.RemoveFromInventory)                      // Remove the item from the inventory and enable the item
            {
                data.Inventory[0].PlayerInventory.Remove(entity.InventoryItem.item);
                entity.InventoryItem.RemoveFromInventory = false;
                entity.PickItem.Drop();
            }
        }
    }
}