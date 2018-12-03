using Unity.Entities;
using UnityEngine;
public class PlayerMovementSystem : ComponentSystem
{

    // Struct specifying all entites that use this move system
    private struct Group
    {
        public Transform transform;
        public Rigidbody RigidBody;
        public InputComponent InputComponent;
        public SpeedComponent SpeedComponent;
        public CharacterAnimator Animator;
    }

    private struct Staminabar
    {
        public StaminaBar staminaBar;
    }

    GameObject staminaObj = null;

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Staminabar>())
        {
            staminaObj = entity.staminaBar.gameObject;
        }
        // Move all entities with Group components
        foreach (var entity in GetEntities<Group>())
        {
            if (entity.InputComponent.EnablePlayerMovement)
            {
                var moveVector = new Vector3(entity.InputComponent.Horizontal, 0, entity.InputComponent.Vertical);                      // Move direction vector
                if (moveVector != Vector3.zero)
                    entity.SpeedComponent.isMoving = true;
                else
                    entity.SpeedComponent.isMoving = false;
                Sprint(entity, moveVector);
                Dodge(entity);
                StaminaControl(entity);
                var speed = (Mathf.Abs(entity.InputComponent.Horizontal) + Mathf.Abs(entity.InputComponent.Vertical)) * entity.SpeedComponent.Speed;
                speed = Mathf.Clamp(speed, 0, entity.SpeedComponent.Speed);
                var movePosition = entity.RigidBody.position + moveVector.normalized * speed * Time.deltaTime;                          // New position
                entity.RigidBody.MovePosition(movePosition);                                                                            // Update entity position to new position
                UpdateAnimation(entity, moveVector);
            }
        }
    }

    void UpdateAnimation(Group entity, Vector3 moveVector)
    {
        if (!entity.SpeedComponent.isSprinting && moveVector != Vector3.zero)
        {
            entity.Animator.isRunning = false;
            entity.Animator.isWalking = true;
        }
        else if (entity.SpeedComponent.isSprinting)
        {
            entity.Animator.isWalking = false;
            entity.Animator.isRunning = true;
        }
        else if (entity.SpeedComponent.isDodging)
        {
            entity.Animator.playerAnimator.SetTrigger("Dash");
        }
        else
        {
            entity.Animator.isWalking = false;
            entity.Animator.isRunning = false;
        }
    }

    /// <summary>
    /// Increase the speed of the entity for a given amount of time
    /// </summary>
    /// <param name="entity"></param>
    void Sprint(Group entity, Vector3 moveVector)
    {
        if (entity.InputComponent.Control("Sprint") && moveVector != Vector3.zero)
        {
            entity.SpeedComponent.isSprinting = true;
            entity.SpeedComponent.Speed = entity.SpeedComponent.SPRINT_SPEED;

            if (staminaObj != null)
                staminaObj.SetActive(true);
        }
        else if (!entity.InputComponent.Control("Sprint"))
        {
            entity.SpeedComponent.isSprinting = false;
            entity.SpeedComponent.Speed = entity.SpeedComponent.DEFAULT_SPEED;

            if (staminaObj != null)
                staminaObj.SetActive(false);
        }
    }

    /// <summary>
    /// Control time duration for which speed is increased
    /// </summary>
    /// <param name="entity"></param>
    void StaminaControl(Group entity)
    {
        if (entity.SpeedComponent.isSprinting || entity.SpeedComponent.isDodging)
        {
            if (entity.SpeedComponent.isSprinting)
            {
                entity.SpeedComponent.Stamina -= Time.deltaTime * entity.SpeedComponent.StaminaConsumptionRate;
            }
            if (entity.SpeedComponent.isDodging)
            {
                var currStamina = entity.SpeedComponent.Stamina;
                var stAmtLost = entity.SpeedComponent.DodgeMultiplier;
                currStamina /= stAmtLost;
                var diff = entity.SpeedComponent.Stamina - currStamina;
                entity.SpeedComponent.Stamina = diff;
            }
            if (entity.SpeedComponent.Stamina <= (0 + Mathf.Epsilon))
            {
                entity.SpeedComponent.Stamina = 0;
                entity.SpeedComponent.isSprinting = false;
                entity.SpeedComponent.isDodging = false;
                entity.SpeedComponent.Speed = entity.SpeedComponent.DEFAULT_SPEED;
            }
        }
        else if (entity.SpeedComponent.Stamina < entity.SpeedComponent.MAX_STAMINA)
        {
            if (staminaObj != null)
                staminaObj.SetActive(true);
            entity.SpeedComponent.Stamina += Time.deltaTime * entity.SpeedComponent.StaminaRegenRate;
        }
    }

    /// <summary>
    /// Sudden burst of speed to dodge
    /// </summary>
    /// <param name="entity"></param>
    void Dodge(Group entity)
    {
        if (CalculateDodge(entity) >= entity.SpeedComponent.MIN_STAMINA)
            entity.SpeedComponent.canDodge = true;
        else
            entity.SpeedComponent.canDodge = false;

        if (entity.InputComponent.Control("Dodge") && entity.SpeedComponent.canDodge)
        {
            entity.SpeedComponent.isDodging = true;
            entity.SpeedComponent.Speed = entity.SpeedComponent.DODGE_SPEED;
        }
        else if (!entity.InputComponent.Control("Dodge"))
        {
            entity.SpeedComponent.isDodging = false;
        }
    }

    float CalculateDodge(Group entity)
    {
        var currStamina = entity.SpeedComponent.Stamina;
        var stAmtLost = entity.SpeedComponent.DodgeMultiplier;
        currStamina /= stAmtLost;
        var diff = entity.SpeedComponent.Stamina - currStamina;
        return diff;
    }
}
