using System;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.ECS_Sample
{
    /// <summary>
    /// Movement system takes in the required components required for movement and performs operations on 
    /// the components. These components, being attached to game objects, should be updated accordingly
    /// </summary>
    class MovementSystem : ComponentSystem
    {
        /// <summary>
        /// Group of components associated with game object we want our movement system to work on.
        /// </summary>
        private struct Group
        {
            public Transform Transform;
            public PlayerInput PlayerInput;
            public Speed Speed;
        }

        protected override void OnUpdate()
        {
            foreach(var entity in GetEntities<Group>()) //Gets all entities with the above "Group" of components 
            {
                var position = entity.Transform.position;
                var rotation = entity.Transform.rotation;

                position.x += entity.Speed.Value * entity.PlayerInput.Horizontal * Time.deltaTime;
                rotation.w = Mathf.Clamp(entity.PlayerInput.Horizontal, 0.5f, 0.5f);

                entity.Transform.position = position;
                entity.Transform.rotation = rotation;
            }
        }
    }
}
