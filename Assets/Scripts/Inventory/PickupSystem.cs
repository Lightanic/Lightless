using Unity.Entities;
using UnityEngine;

public class PickupSystem : ComponentSystem {

    struct Group
    {
        public Transform Transform;
        public Pickup PickItem;
    }

    private struct Player
    {
        readonly public int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<InputComponent> InputComponents;
    }
    [Inject] private Player playerData;
    protected override void OnUpdate()
    {
        Vector3 playerPos = playerData.Transform[0].position;

        foreach (var entity in GetEntities<Group>())
        {
            if(Vector3.Distance(playerPos,entity.Transform.position) <= entity.PickItem.InteractDistance && (playerData.InputComponents[0].Gamepad.GetButtonDown("B") || Input.GetKeyDown(KeyCode.E)))
            {
                entity.PickItem.IsInteracting = true;
                entity.PickItem.Interact();
            }
        }
    }

}
