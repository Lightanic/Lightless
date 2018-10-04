using UnityEngine;
using Unity.Entities;
public class RotationSystem : ComponentSystem {
    // Struct specifying all entites that use this move system
    private struct Group
    {
        readonly public int Length;
        public ComponentArray<RotationComponent> RotationComponents;
        public ComponentArray<Rigidbody> Rigidbody;
    }

    [Inject] private Group data;    // Inject entities with "Group" components

    protected override void OnUpdate()
    {
        for(int i = 0; i < data.Length; i++)                    // Go through all entities with Group components
        {
            var rotation = data.RotationComponents[i].Rotation; // Set rotation value 
            data.Rigidbody[i].MoveRotation(rotation.normalized);// Set rigidbody rotation
        }
    }
}
