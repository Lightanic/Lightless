using Unity.Entities;
using UnityEngine;

public class EquipperSystem : ComponentSystem {

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

    static int index = 0;
    protected override void OnUpdate()
    {
        
        for (int i=0; i < itemData.Length; i++)
        {
            if (itemData.PickItem[i].IsEquiped)
            {
                if (leftHandData.data[0].isEmpty)
                {
                    leftHandData.EquipComp[0].EquipItem(itemData.InventoryItem[i].gameObject);
                }
            }

            if (playerData.InputComponents[0].Gamepad.GetButtonDown("DPad_Right"))
            {
                if (!leftHandData.data[0].isEmpty)
                {
                    //Add Item to the inventory
                    Debug.Log("Index " + index);
                    leftHandData.EquipComp[0].EquipedItem.GetComponent<Pickup>().IsEquiped = false;
                    leftHandData.EquipComp[0].EquipedItem.GetComponent<InventoryItemComponent>().AddToInventory = true;
                }
                leftHandData.data[0].DropItem();
                // Remove and equip item from inventory
                if (++index > inventoryData.Inventory[0].PlayerInventory.Items.Count - 1)
                    index = 0;
               
                if (inventoryData.Inventory[0].PlayerInventory.Items.Count > 0)
                {
                    leftHandData.EquipComp[0].EquipItem(inventoryData.Inventory[0].PlayerInventory.Items[index].Prefab);
                    inventoryData.Inventory[0].PlayerInventory.Remove(inventoryData.Inventory[0].PlayerInventory.Items[index]);
                }
            }

            if (playerData.InputComponents[0].Gamepad.GetButtonDown("X"))
            {
                leftHandData.EquipComp[0].EquipedItem.GetComponent<Pickup>().IsEquiped = false;
                leftHandData.data[0].DropItem();
            }
        }
    }
}
