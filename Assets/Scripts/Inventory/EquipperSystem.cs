using Unity.Entities;
using UnityEngine;

public class EquipperSystem : ComponentSystem
{

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


    /// <summary>
    /// Player data
    /// </summary>
    private struct Player
    {
        readonly public int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<InputComponent> InputComponents;
    }
    [Inject] private Player playerData;

    /// <summary>
    /// Left hand data
    /// </summary>
    private struct LeftHandData
    {
        readonly public int Length;
        public ComponentArray<EquipComponent> EquipComp;
        public ComponentArray<LeftHandComponent> data;
    }

    [Inject] private LeftHandData leftHandData;


    /// <summary>
    /// Player inventory data
    /// </summary>
    struct InventoryData
    {
        readonly public int Length;
        public ComponentArray<InventoryComponent> Inventory;
    }

    [Inject] private InventoryData inventoryData;

    int index = 0;       //  used to cycle through the inventory

    /// <summary>
    /// Equip and unequip items from the inventory
    /// Cycle through the inventory items
    /// Drop equiped items
    /// </summary>
    protected override void OnUpdate()
    {
        var lhComponent = leftHandData.data[0];
        Pickup lhDataEquipCompPickup = null;
        InventoryItemComponent lhInventoryItemComp = null;
        if (leftHandData.EquipComp.Length > 0 && leftHandData.EquipComp[0].EquipedItem != null)
        {
            lhDataEquipCompPickup = leftHandData.EquipComp[0].EquipedItem.GetComponent<Pickup>();
            lhInventoryItemComp = leftHandData.EquipComp[0].EquipedItem.GetComponent<InventoryItemComponent>();
        }


        for (int i = 0; i < itemData.Length; i++)
        {
            // Always equip item to an empty hand
            if (itemData.PickItem[i].IsEquiped)
            {
                if (leftHandData.data[0].isEmpty)
                {
                    leftHandData.EquipComp[0].EquipItem(itemData.InventoryItem[i].gameObject);
                }
            }


            // Cycle through inventory items
            if (playerData.InputComponents[0].Control("InventoryNext"))
            {
                lock (leftHandData.data[0])
                {
                    if (!lhComponent.isEmpty && lhDataEquipCompPickup != null && lhInventoryItemComp != null)
                    {
                        //Add Item to the inventory
                        Debug.Log("Index " + index);
                        lhDataEquipCompPickup.IsEquiped = false;
                        lhInventoryItemComp.AddToInventory = true;
                        lhComponent.DropItem();
                    }
                    // Remove and equip item from inventory
                    if (++index > inventoryData.Inventory[0].PlayerInventory.Items.Count - 1)
                        index = 0;

                    if (inventoryData.Inventory[0].PlayerInventory.Items.Count > 0)
                    {
                        leftHandData.EquipComp[0].EquipItem(inventoryData.Inventory[0].PlayerInventory.Items[index].Prefab);
                        inventoryData.Inventory[0].PlayerInventory.Remove(inventoryData.Inventory[0].PlayerInventory.Items[index]);
                    }
                }

            }

            // Drop items from inventory
            if (playerData.InputComponents[0].Control("DropItem"))
            {
                if (!leftHandData.data[0].isEmpty)
                {
                    leftHandData.EquipComp[0].EquipedItem.GetComponent<Pickup>().IsInteractable = true;
                    leftHandData.EquipComp[0].EquipedItem.GetComponent<Pickup>().IsEquiped = false;
                    leftHandData.EquipComp[0].EquipedItem.GetComponent<Rigidbody>().isKinematic = false;    // enable rigidbody
                    leftHandData.data[0].DropItem();
                }
            }
        }
    }
}
