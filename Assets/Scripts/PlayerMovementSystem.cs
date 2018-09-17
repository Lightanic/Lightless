using Unity.Entities;
using UnityEngine;
public class PlayerMovementSystem : ComponentSystem {

    // Struct specifying all entites that use this move system
    private struct Group
    {
        public Rigidbody rigidbody;
        public InputComponent inputComponent;
        public SpeedComponent speedComponent;
    }

	protected override void OnUpdate()
    {
        // Move all entities with Group components
        foreach(var entity in GetEntities<Group>())
        {
            var moveVector = new Vector3(entity.inputComponent.horizontal, 0, entity.inputComponent.vertical);                      // Move direction vector
            var movePosition = entity.rigidbody.position + moveVector.normalized * entity.speedComponent.speed * Time.deltaTime;    // New position

            entity.rigidbody.MovePosition(movePosition);                                                                            // Update entity position to new position
        }
    }
}
