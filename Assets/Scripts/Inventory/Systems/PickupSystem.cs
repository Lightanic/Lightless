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
        public ComponentArray<CharacterAnimator> Animators;
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
    /// Entities taht can be picked up
    /// </summary>
    private struct LanternData
    {
        public Transform Transform;
        public Pickup PickItem;
        public Lantern lantern;
    }


    /// <summary>
    /// Entities taht can be picked up
    /// </summary>
    private struct PickupUI
    {
        public Transform Transform;
        public Pickup PickItem;
        public InteractUIComponent tooltips;
        public InventoryItemComponent InventoryItem;
    }

    bool uiEnabled = false;

    /// <summary>
    /// Pick an item up and add it to the inventory
    /// </summary>
    protected override void OnUpdate()
    {
        Vector3 playerPos = playerData.Transform[0].position;
        var animator = playerData.Animators[0];

        foreach (var entity in GetEntities<Group>())
        {
            if(Vector3.Distance(playerPos,entity.Transform.position) <= entity.PickItem.InteractDistance && (playerData.InputComponents[0].Control("Interact")))
            {
                animator.playerAnimator.SetTrigger("Interact");
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

        }

        foreach(var entity in GetEntities<LanternData>())
        {
            if (Vector3.Distance(playerPos, entity.Transform.position) <= entity.PickItem.InteractDistance && entity.PickItem.IsEquiped != true)
            {
                entity.lantern.ShowtoolTip = true;
                uiEnabled = true;
            }
            else
            {
                entity.lantern.ShowtoolTip = false;
            }
                if (Vector3.Distance(playerPos, entity.Transform.position) <= entity.PickItem.InteractDistance && (playerData.InputComponents[0].Control("Interact")))
            {
                entity.PickItem.IsInteracting = true;
                entity.PickItem.IsEquiped = true;   // equip to left hand
                entity.PickItem.IsInteractable = false;
                entity.lantern.EquipRightHand();
            }
            entity.lantern.ToggleToolTip();
        }

        foreach(var entity in GetEntities<PickupUI>())
        {
            if ((Vector3.Distance(playerPos, entity.Transform.position) <= entity.PickItem.InteractDistance) && entity.PickItem.IsEquiped != true)
            {
                entity.tooltips.RePosition(entity.Transform.position);
                entity.tooltips.ToggleOn(entity.InventoryItem.item.PopupIcon);
                uiEnabled = true;
            }
            else if(!uiEnabled)
            {
                entity.tooltips.ToggleOff();
            }
        }

        uiEnabled = false;
    }

}
