using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.ECS_Sample.PureECS
{

    /// <summary>
    /// Pure ECS does not work with game objects. Hence, bootstrapping is required to initialize the objects.
    /// Npte: Since Pure ECS does not play well with animations and such, we should refrain from going this route
    /// until Unity finishes development. 
    /// </summary>
    public class Bootstrap : MonoBehaviour {

        //These values are set in the unity editor
        public float Speed;
        public Mesh Mesh;
        public Material Material;

        void Start()
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            //Player "archetype"
            var playerEntity = entityManager.CreateEntity(
                ComponentType.Create<SpeedComponent>(),
                ComponentType.Create<PlayerInputComponent>(),
                ComponentType.Create<Position>(),
                ComponentType.Create<TransformSystem>(),
                ComponentType.Create<MeshInstanceRenderer>());

            //Initialize player data
            entityManager.SetComponentData(playerEntity, new SpeedComponent { Value = Speed });
            entityManager.SetSharedComponentData(playerEntity, new MeshInstanceRenderer
            {
                mesh = Mesh,
                material = Material
            });
    }

    }
}
