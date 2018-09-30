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
    /// Left hand data
    /// </summary>
    private struct LeftHandData
    {
        readonly public int Length;
        public ComponentArray<EquipComponent> EquipComp;
        public ComponentArray<LeftHandComponent> data;
    }
    [Inject] private LeftHandData leftHandData;

    protected override void OnUpdate()
    {

        for (int i=0; i < itemData.Length; i++)
        {
            if (itemData.PickItem[i].IsEquiped)
            {
                if (!leftHandData.data[0].isEmpty)
                {
                    //leftHandData.data[0].UnEquipAll();
                }
                leftHandData.EquipComp[0].EquipedItem = itemData.InventoryItem[i].gameObject;
            }
        }
    }
}
