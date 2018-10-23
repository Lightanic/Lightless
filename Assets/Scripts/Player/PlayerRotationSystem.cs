using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class PlayerRotationSystem : ComponentSystem {
    
    // Struct specifying all entites that use this move system
    private struct Group
    {
        public Transform Transform;
        public RotationComponentLightless RotationComponent;
        public InputComponent InputComponent;
        public SpeedComponent speedCmp;
    }
    private RaycastHit hit;
    private Quaternion rotation;
    private Vector3 forward;

    protected override void OnUpdate()
    {
        foreach(var entity in GetEntities<Group>())
        {
            if (InputManager.Instance.IsGamePadActive)
            {
                if (entity.InputComponent.Gamepad.GetStick_R().X != 0 || entity.InputComponent.Gamepad.GetStick_R().Y != 0)
                    GamePadRotationRightStick(entity);
                else
                {
                    entity.RotationComponent.RotationSpeed = entity.speedCmp.RotationSpeed;
                    GamePadRotation(entity);
                }
            }
            else if (!InputManager.Instance.IsGamePadActive)
            {
                entity.RotationComponent.RotationSpeed = entity.speedCmp.RotationFineControlSpeed;
                GamePadRotation(entity);
            }
        }
    }

    /// <summary>
    /// Rotate entity to look towards the mouse pointer
    /// </summary>
    /// <param name="entity"></param>
    void MouseRotation( Group entity)
    {
        var mousePosition = Input.mousePosition;                        // Current mouse position
        var cameraRay = Camera.main.ScreenPointToRay(mousePosition);    // Ray from mouse poisiton
        var layerMask = LayerMask.GetMask("Floor");                     // Floor layer mask
        if (Physics.Raycast(cameraRay, out hit, 100, layerMask))        // Raycast to floor - Set layer to floor in editor 
        {
                forward = hit.point - entity.Transform.position;        // Get forward direction
                rotation = Quaternion.LookRotation(forward);            // Rotate to forward direction
                entity.RotationComponent.Rotation = new Quaternion(0, rotation.y, 0, rotation.w).normalized;    // Set rotation vector
        }
    }

    /// <summary>
    /// Rotate entity towards direction of motion
    /// </summary>
    /// <param name="entity"></param>
    void GamePadRotation( Group entity)
    {
            forward = new Vector3(entity.InputComponent.Horizontal, 0, entity.InputComponent.Vertical);    // Get forward direction
            if (forward != Vector3.zero)
                rotation = Quaternion.LookRotation(forward, entity.Transform.up);                          // Rotate to forward direction
            rotation = Quaternion.Slerp(entity.Transform.rotation, rotation, Time.deltaTime * entity.RotationComponent.RotationSpeed);
            entity.RotationComponent.Rotation = new Quaternion(0, rotation.y, 0, rotation.w).normalized;   // Set rotation vector
    }

    /// <summary>
    /// Rotate entity towards direction of motion
    /// </summary>
    /// <param name="entity"></param>
    void GamePadRotationRightStick(Group entity)
    {
        entity.RotationComponent.RotationSpeed = entity.speedCmp.RotationFineControlSpeed;
        forward = new Vector3(entity.InputComponent.Gamepad.GetStick_R().X, 0, entity.InputComponent.Gamepad.GetStick_R().Y);    // Get forward direction
        if (forward != Vector3.zero)
            rotation = Quaternion.LookRotation(forward, entity.Transform.up);                          // Rotate to forward direction
        rotation = Quaternion.Slerp(entity.Transform.rotation, rotation, Time.deltaTime * entity.RotationComponent.RotationSpeed);
        entity.RotationComponent.Rotation = new Quaternion(0, rotation.y, 0, rotation.w).normalized;   // Set rotation vector
    }
}
