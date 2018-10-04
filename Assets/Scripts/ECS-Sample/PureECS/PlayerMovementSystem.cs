using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts.ECS_Sample.PureECS
{
    /// <summary>
    /// Pure ECS Player movement system. Similar to hybrid ECS Movement system, but the job system requires components which are not "MonoBehavior" objects. 
    /// This obviously takes advantage of multiple cores. But we should still make use of the Job system in cases where the component data is minimal.
    /// </summary>
    public class PlayerMovementSystem : JobComponentSystem
    {
        private struct PlayerMovementJob : IJobProcessComponentData<SpeedComponent, PlayerInputComponent, Position>
        {
            public float DeltaTime;
            public void Execute(ref SpeedComponent speed, ref PlayerInputComponent playerInput, ref Position position)
            {
                position.Value.x += speed.Value * playerInput.Horizontal * DeltaTime;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new PlayerMovementJob
            {
                DeltaTime = Time.deltaTime
            };

            return job.Schedule(this, inputDeps);
        }
    }
}
