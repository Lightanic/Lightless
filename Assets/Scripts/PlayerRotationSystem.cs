using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class PlayerRotationSystem : ComponentSystem {
    
    // Struct specifying all entites that use this move system
    private struct Group
    {
        public Transform Transform;
        public RotationComponent RotationComponent;
        public InputComponent InputComponent;
    }
    private RaycastHit hit;
    private Quaternion rotation;
    private Vector3 forward;
    private bool gamePadConnected = false;
    protected override void OnUpdate()
    {
        foreach(var entity in GetEntities<Group>())
        {
            if (entity.InputComponent.GamePadState.IsConnected) // Check if a gamepad is connected or not
                gamePadConnected = true;
            else
                gamePadConnected = false;
        }
        if (gamePadConnected)
            GamePadRotation();
        else if (!gamePadConnected)
            MouseRotation();
    }

    void MouseRotation()
    {
        var mousePosition = Input.mousePosition;                        // Current mouse position
        var cameraRay = Camera.main.ScreenPointToRay(mousePosition);    // Ray from mouse poisiton
        var layerMask = LayerMask.GetMask("Floor");                     // Floor layer mask
        if (Physics.Raycast(cameraRay, out hit, 100, layerMask))        // Raycast to floor - Set layer to floor in editor 
        {
            foreach (var entity in GetEntities<Group>())                // Set rotaion component for all entities in with group components
            {
                forward = hit.point - entity.Transform.position;        // Get forward direction
                rotation = Quaternion.LookRotation(forward);            // Rotate to forward direction
                entity.RotationComponent.Rotation = new Quaternion(0, rotation.y, 0, rotation.w).normalized;    // Set rotation vector
            }
        }
    }

    void GamePadRotation()
    {
        foreach (var entity in GetEntities<Group>())                // Set rotaion component for all entities in with group components
        {
            forward = new Vector3(entity.InputComponent.Horizontal, 0, entity.InputComponent.Vertical);    // Get forward direction
            if (forward != Vector3.zero)
                rotation = Quaternion.LookRotation(forward, entity.Transform.up);                          // Rotate to forward direction
            rotation = Quaternion.Slerp(entity.Transform.rotation, rotation, Time.deltaTime * entity.RotationComponent.RotationSpeed);
            entity.RotationComponent.Rotation = new Quaternion(0, rotation.y, 0, rotation.w).normalized;   // Set rotation vector
        }
        
    }
}
