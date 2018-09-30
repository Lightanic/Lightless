using Unity.Entities;
using UnityEngine;

public class PickupSystem : ComponentSystem {

    /// <summary>
    /// Entities taht can be picked up
    /// </summary>
    struct Group
    {
        public Transform Transform;
        public Pickup PickItem;
        public InventoryItemComponent InventoryItem;
    }

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
        public ComponentArray<LeftHandComponent> data;
    }
    [Inject] private LeftHandData leftHandData;

    /// <summary>
    /// Pick an item up and add it to the inventory
    /// </summary>
    protected override void OnUpdate()
    {
        Vector3 playerPos = playerData.Transform[0].position;

        foreach (var entity in GetEntities<Group>())
        {
            if(Vector3.Distance(playerPos,entity.Transform.position) <= entity.PickItem.InteractDistance && (playerData.InputComponents[0].Gamepad.GetButtonDown("B") || Input.GetKeyDown(KeyCode.E)))
            {
                entity.PickItem.IsInteracting = true;
                if (leftHandData.data[0].isEmpty)
                {
                    entity.PickItem.IsEquiped = true;   // equip to left hand
                    entity.PickItem.IsInteractable = false;
                }
                else if (entity.PickItem.IsInteractable)
                {
                    entity.InventoryItem.AddToInventory = true;
                }
            }

            if((playerData.InputComponents[0].Gamepad.GetButtonDown("Y") || Input.GetKeyDown(KeyCode.G)))
            {
                if(!leftHandData.data[0].isEmpty)
                {
                    leftHandData.data[0].DropItem();
                }
            }
        }
    }

}
