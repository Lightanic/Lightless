using Unity.Entities;
using UnityEngine;
public class PlayerMovementSystem : ComponentSystem {

    // Struct specifying all entites that use this move system
    private struct Group
    {
        public Rigidbody RigidBody;
        public InputComponent InputComponent;
        public SpeedComponent SpeedComponent;
    }

	protected override void OnUpdate()
    {
        // Move all entities with Group components
        foreach(var entity in GetEntities<Group>())
        {
            var moveVector = new Vector3(entity.InputComponent.Horizontal, 0, entity.InputComponent.Vertical);                      // Move direction vector
            var speed = (Mathf.Abs(entity.InputComponent.Horizontal) + Mathf.Abs(entity.InputComponent.Vertical))*entity.SpeedComponent.Speed;
            var movePosition = entity.RigidBody.position + moveVector.normalized * speed * Time.deltaTime;    // New position

            entity.RigidBody.MovePosition(movePosition);                                                                            // Update entity position to new position
        }
    }
}
