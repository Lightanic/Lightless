using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ThrowSystem : ComponentSystem {

    /// <summary>
    /// Target to which we throw
    /// </summary>
    struct Target
    {
        readonly public int Length;
        public ComponentArray<Transform> transform;
        public ComponentArray<ThrowTarget> target;
        public ComponentArray<Projector> projector;
        public ComponentArray<Throw> throwBehaviour;
    }

    [Inject] private Target targetData;

    /// <summary>
    /// Player data
    /// </summary>
    private struct Player
    {
        readonly public int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<InputComponent> InputComponents;
        public ComponentArray<RotationComponentLightless> RotationComponent;
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

    private struct Throwables
    {
        public Throw throwable;
    }

    protected override void OnUpdate()
    {
        
        var lhComponent = leftHandData.data[0];
        if (playerData.InputComponents[0].Control("ThrowActive"))
        {
            playerData.RotationComponent[0].EnablePlayerRotation = false;
            targetData.projector[0].enabled = true;

            // Move Throw target
            var forward = new Vector3(playerData.InputComponents[0].Gamepad.GetStick_R().X, 0, playerData.InputComponents[0].Gamepad.GetStick_R().Y);    // Get forward direction
            targetData.transform[0].localPosition += forward;
            Vector3 pos = targetData.transform[0].localPosition; 
            pos.x = Mathf.Clamp(pos.x, -targetData.target[0].throwDistance, targetData.target[0].throwDistance); // clamp position
            pos.z = Mathf.Clamp(pos.z, -targetData.target[0].throwDistance, targetData.target[0].throwDistance);
            targetData.transform[0].localPosition = pos;
            targetData.throwBehaviour[0].DrawPath(leftHandData.EquipComp[0].EquipedItem);
            if (!lhComponent.isEmpty && playerData.InputComponents[0].Control("Throw"))
            {
                if (leftHandData.EquipComp[0].EquipedItem.GetComponent<InventoryItemComponent>().Throwable)
                {
                    var equipped = leftHandData.EquipComp[0].EquipedItem;
                    lhComponent.DropItem();
                    equipped.GetComponent<Pickup>().IsInteractable = true;
                    targetData.throwBehaviour[0].Launch(equipped);
                    leftHandData.EquipComp[0].EquipedItem = null;
                }
            }
        }
        else
        {
            targetData.throwBehaviour[0].ResetLine();
            playerData.RotationComponent[0].EnablePlayerRotation = true;
            targetData.projector[0].enabled = false;
        }
    }

}
