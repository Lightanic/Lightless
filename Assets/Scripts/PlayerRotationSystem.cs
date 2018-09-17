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
    }
    private RaycastHit hit;
    protected override void OnUpdate()
    {
        var mousePosition = Input.mousePosition;                        // Current mouse position
        var cameraRay = Camera.main.ScreenPointToRay(mousePosition);    // Ray from mouse poisiton
        var layerMask = LayerMask.GetMask("Floor");                     // Floor layer mask
        
        if(Physics.Raycast(cameraRay, out hit, 100, layerMask))         // Raycast to floor - Set layer to floor in editor 
        {
            foreach (var entity in GetEntities<Group>())                // Set rotaion component for all entities in with group components
            {
                var forward = hit.point - entity.Transform.position;    // Get forward direction
                var rotation = Quaternion.LookRotation(forward);        // Rotate to forward direction
                entity.RotationComponent.Rotation = new Quaternion(0, rotation.y, 0, rotation.w).normalized;    // Set rotation vector
            }
        }
    }
}
