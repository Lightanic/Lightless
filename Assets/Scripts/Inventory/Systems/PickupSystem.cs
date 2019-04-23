using Unity.Entities;
using UnityEngine;

public class PickupSystem : ComponentSystem
{

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
        public ComponentArray<EquipComponent> EquipComp;
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

    private struct HUD
    {
        public HUDUpdate props;
    }

    
    /// <summary>
    /// Pick an item up and add it to the inventory
    /// </summary>
    protected override void OnUpdate()
    {
        Vector3 playerPos = playerData.Transform[0].position;
        var animator = playerData.Animators[0];
        var entityHUD = GetEntities<HUD>()[0];

        var lhComponent = leftHandData.data[0];
        Pickup lhDataEquipCompPickup = null;
        InventoryItemComponent lhInventoryItemComp = null;
        if (leftHandData.EquipComp.Length > 0 && leftHandData.EquipComp[0].EquipedItem != null)
        {
            lhDataEquipCompPickup = leftHandData.EquipComp[0].EquipedItem.GetComponent<Pickup>();
            lhInventoryItemComp = leftHandData.EquipComp[0].EquipedItem.GetComponent<InventoryItemComponent>();
        }
        
        foreach (var entity in GetEntities<Group>())
        {
            if (!entity.PickItem.IsEquiped && entity.PickItem.IsInteractable)
            {
                if (Vector3.Distance(playerPos, entity.Transform.position) <= entity.PickItem.InteractDistance && (playerData.InputComponents[0].Control("Interact")))
                {
                    if (!lhComponent.isEmpty && lhDataEquipCompPickup != null && lhInventoryItemComp != null)
                    {
                        //Add Item to the inventory
                        lhDataEquipCompPickup.IsEquiped = false;
                        lhInventoryItemComp.AddToInventory = true;
                        lhComponent.DropItem();
                    }

                    animator.playerAnimator.SetTrigger("interact");
                    entity.PickItem.IsInteracting = true;
                    //if (leftHandData.data[0].isEmpty && entity.PickItem.IsInteractable)
                    //{
                    //    entityHUD.props.Show();
                    //    entity.PickItem.IsEquiped = true;   // equip to left hand
                    //    entity.PickItem.IsInteractable = false;
                    //    break;
                    //}
                    //else 
                    if (entity.PickItem.IsInteractable)
                    {
                        entityHUD.props.Show();
                        entity.PickItem.IsEquiped = true;   // equip to left hand
                        entity.PickItem.IsInteractable = false;
                        break;
                        AkSoundEngine.PostEvent("Play_ItemPickup", entity.PickItem.gameObject);
                        //entity.InventoryItem.AddToInventory = true;
                    }
                }
            }

        }

        foreach (var entity in GetEntities<LanternData>())
        {
            if (Vector3.Distance(playerPos, entity.Transform.position) <= entity.PickItem.InteractDistance && (playerData.InputComponents[0].Control("Interact")))
            {
                entity.PickItem.IsInteracting = true;
                entity.PickItem.IsEquiped = true;   // equip to left hand
                entity.PickItem.IsInteractable = false;
                entity.lantern.EquipRightHand();
            }
        }
    }

}
