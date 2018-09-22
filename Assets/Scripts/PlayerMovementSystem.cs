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
            Sprint(entity);
            var speed = (Mathf.Abs(entity.InputComponent.Horizontal) + Mathf.Abs(entity.InputComponent.Vertical))*entity.SpeedComponent.Speed;
            speed = Mathf.Clamp(speed, 0, entity.SpeedComponent.Speed);
            var movePosition = entity.RigidBody.position + moveVector.normalized * speed * Time.deltaTime;    // New position
            entity.RigidBody.MovePosition(movePosition);                                                                            // Update entity position to new position
        }
    }

    /// <summary>
    /// Increase the speed of the entity for a given amount of time
    /// </summary>
    /// <param name="entity"></param>
    void Sprint(Group entity)
    {
        if(entity.InputComponent.GamePadState.Triggers.Right != 0 || Input.GetKey(KeyCode.LeftShift))
        {
            entity.SpeedComponent.isSprinting = true;
            entity.SpeedComponent.Speed = entity.SpeedComponent.SPRINT_SPEED;
        }
        else if(entity.InputComponent.GamePadState.Triggers.Right == 0 || Input.GetKeyUp(KeyCode.LeftShift))
        {
            entity.SpeedComponent.isSprinting = false;
            entity.SpeedComponent.Speed = entity.SpeedComponent.DEFAULT_SPEED;
        }
        StaminaControl(entity);
    }

    /// <summary>
    /// Control time duration for which speed is increased
    /// </summary>
    /// <param name="entity"></param>
    void StaminaControl(Group entity)
    {
        if (entity.SpeedComponent.isSprinting)
        {
            entity.SpeedComponent.Stamina -= Time.deltaTime;
            if(entity.SpeedComponent.Stamina <= (0 + Mathf.Epsilon))
            {
                entity.SpeedComponent.Stamina = 0;
                entity.SpeedComponent.isSprinting = false;
                entity.SpeedComponent.Speed = entity.SpeedComponent.DEFAULT_SPEED;
            }
        }
        else if(entity.SpeedComponent.Stamina < entity.SpeedComponent.MAX_STAMINA)
        {
            entity.SpeedComponent.Stamina += Time.deltaTime;
        }
    }
}
