using System;
using UnityEngine;
using Unity.Entities;

namespace Assets.Scripts.ECS_Sample
{
    class PlayerInputSystem : ComponentSystem
    {
        private struct Group
        {
            public PlayerInput PlayerInput;
        }

        protected override void OnUpdate()
        {
            foreach(var entity in GetEntities<Group>())
            {
                entity.PlayerInput.Horizontal = Input.GetAxis("Horizontal");
            }
        }
    }
}
