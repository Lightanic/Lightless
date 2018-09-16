using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Assets.Scripts.ECS_Sample.PureECS
{
    public class InputSystem : JobComponentSystem
    {
        private struct PlayerInputJob : IJobProcessComponentData<PlayerInputComponent>
        {
            public float Horizontal;

            public void Execute(ref PlayerInputComponent input)
            {
                input.Horizontal = Horizontal;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new PlayerInputJob
            {
                Horizontal = Input.GetAxis("Horizontal")
            };
            
            return job.Schedule(this, inputDeps);
        }
    }
}
