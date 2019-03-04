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

    struct InventoryHUD
    {
        readonly public int Length;
        public ComponentArray<InventoryHUDComponent> Slot;
    }

    [Inject] private InventoryHUD slotData;

    private struct HUD
    {
        public HUDUpdate props;
    }


    InventoryItem leftItem = null;
    InventoryItem rightItem = null;
    /// <summary>
    /// Equip and unequip items from the inventory
    /// Cycle through the inventory items
    /// Drop equiped items
    /// </summary>
    protected override void OnUpdate()
    {
        var entityHUD = GetEntities<HUD>()[0];
        var lhComponent = leftHandData.data[0];
        Pickup lhDataEquipCompPickup = null;
        InventoryItemComponent lhInventoryItemComp = null;
        if (leftHandData.EquipComp.Length > 0 && leftHandData.EquipComp[0].EquipedItem != null)
        {
            lhDataEquipCompPickup = leftHandData.EquipComp[0].EquipedItem.GetComponent<Pickup>();
            lhInventoryItemComp = leftHandData.EquipComp[0].EquipedItem.GetComponent<InventoryItemComponent>();
        }


     


        // Cycle through inventory items
        if (playerData.InputComponents[0].Control("InventoryNext"))
        {
            entityHUD.props.Show();
            LightComponent light = null;
            if (leftHandData.EquipComp[0].EquipedItem != null)
                light = leftHandData.EquipComp[0].EquipedItem.GetComponent<LightComponent>();
            if (light != null)
            {
                light.ToggleLightOn();
            }
            if (!lhComponent.isEmpty && lhDataEquipCompPickup != null && lhInventoryItemComp != null)
            {
                //Add Item to the inventory
                lhDataEquipCompPickup.IsEquiped = false;
                lhInventoryItemComp.AddToInventory = true;
                lhComponent.DropItem();
            }
            if (inventoryData.Inventory[0].PlayerInventory.Items.Count > 0)
            {
                leftHandData.EquipComp[0].EquipItem(inventoryData.Inventory[0].PlayerInventory.Items[0].Prefab);
                inventoryData.Inventory[0].PlayerInventory.Remove(inventoryData.Inventory[0].PlayerInventory.Items[0]);
            }
            

        }

        // Cycle through inventory items
        if (playerData.InputComponents[0].Control("InventoryBack"))
        {
            entityHUD.props.Show();
            LightComponent light = null;
            if(leftHandData.EquipComp[0].EquipedItem !=null)
                light = leftHandData.EquipComp[0].EquipedItem.GetComponent<LightComponent>();

            if (light != null)
            {
                light.ToggleLightOn();
            }
            if (!lhComponent.isEmpty && lhDataEquipCompPickup != null && lhInventoryItemComp != null)
                {
                    //Add Item to the top of the inventory
                    lhDataEquipCompPickup.IsEquiped = false;
                    lhInventoryItemComp.AddToInventory = false; // add to back of inventory
                    lhInventoryItemComp.AddToInventoryTop = true; // add to the top of th einventory
                    lhComponent.DropItem();
                }
                // Remove and equip item at the back of the inventory
                if (inventoryData.Inventory[0].PlayerInventory.Items.Count > 0)
                {
                    leftHandData.EquipComp[0].EquipItem(inventoryData.Inventory[0].PlayerInventory.Items[inventoryData.Inventory[0].PlayerInventory.Items.Count - 1].Prefab);
                    inventoryData.Inventory[0].PlayerInventory.Remove(inventoryData.Inventory[0].PlayerInventory.Items[inventoryData.Inventory[0].PlayerInventory.Items.Count - 1]);
                }
            

        }

        for (int i = 0; i < itemData.Length; i++)
        {
            // Always equip item to an empty hand
            if (itemData.PickItem[i].IsEquiped)
            {
                if (leftHandData.data[0].isEmpty)
                {
                    leftHandData.EquipComp[0].EquipItem(itemData.InventoryItem[i].gameObject);
                    leftHandData.data[0].isEmpty = false;
                }
            }
        }

        // Drop items from inventory
        if (playerData.InputComponents[0].Control("DropItem"))
        {
            if (!leftHandData.data[0].isEmpty)
            {
                if (leftHandData.EquipComp[0].EquipedItem.GetComponent<Pickup>().CanDrop)
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
