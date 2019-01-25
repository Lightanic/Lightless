using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class InventoryHUDSystem : ComponentSystem
{

    /// <summary>
    /// Player inventory data
    /// </summary>
    struct InventoryData
    {
        readonly public int Length;
        public ComponentArray<InventoryComponent> Inventory;
    }

    [Inject] private InventoryData inventoryData;

    struct InventoryHUD
    {
        readonly public int Length;
        public ComponentArray<InventoryHUDComponent> Slot;
    }

    [Inject] private InventoryHUD slotData;

    /// <summary>
    /// Inventory item group
    /// </summary>
    struct ItemGroup
    {
        readonly public int Length;
        public ComponentArray<Pickup> PickItem;
        public ComponentArray<InventoryItemComponent> InventoryItem;
    }
    [Inject] private ItemGroup itemData;

    static Pickup prevItem;
    InventoryItem selectedItem = null;
    InventoryItem leftItem = null;
    InventoryItem rightItem = null;
    int equiped = 0;                                               // Check if any item is equipped
    protected override void OnUpdate()
    {
        equiped = 0;
        List<InventoryItem> items = new List<InventoryItem>();
        if (inventoryData.Length > 0 && inventoryData.Inventory[0].PlayerInventory != null)
            items = inventoryData.Inventory[0].PlayerInventory.Items;
        for (int i = 0; i < itemData.Length; i++)
        {
            // Always equip item to an empty hand
            if (itemData.PickItem[i].IsEquiped && prevItem != itemData.PickItem[i])
            {
                prevItem = itemData.PickItem[i];
                selectedItem = itemData.InventoryItem[i].item;
                slotData.Slot[0].SetSelectedSlot(itemData.InventoryItem[i].item.InventoryIcon);
                //break;
            }
            if (itemData.PickItem[i].IsEquiped)
            {
                equiped++;
            }
        }

        if (items.Count == 0 && slotData.Length > 0)
        {
            slotData.Slot[0].SetLeftSlot(null);
            slotData.Slot[0].SetRightSlot(null);
        }
        // If Nothing is equipped then show empty slot
        if (equiped == 0 && slotData.Slot.Length != 0)
        {
            prevItem = null;
            selectedItem = null;
            slotData.Slot[0].SetSelectedSlot(null);
        }

        if (items.Count == 1 && slotData.Slot.Length != 0)
        {
            slotData.Slot[0].SetLeftSlot(items[0].InventoryIcon);
            slotData.Slot[0].SetRightSlot(items[0].InventoryIcon);
        }
        if (items.Count > 1 && slotData.Slot.Length != 0)
        {
            slotData.Slot[0].SetLeftSlot(items[items.Count - 1].InventoryIcon);
            slotData.Slot[0].SetRightSlot(items[0].InventoryIcon);
        }

    }
}
