using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class RunnerSystem : ComponentSystem
{
    private struct RunnerData
    {
        public NavAgentComponent AgentComponent;
        public Transform EnemyTransform;
        public EnemyVisionComponent EnemyVision;
        public EnemyDarkVisionComponent NightVision;
        public EnemyAnimator Animator;

    }


    private struct PlayerData
    {
        public InputComponent PlayerInput;
        public Transform PlayerTransform;
    }
    private struct LightData
    {
        public LightComponent LightSwitch;
        public Transform LightTransform;
    }

    protected override void OnUpdate()
    {
        PlayerData playerData = new PlayerData();
        LightData lightData = new LightData();
        bool isThereLight = false;


        foreach (var entity in GetEntities<PlayerData>())
        {
            playerData = entity;

        }



        foreach (var runner in GetEntities<RunnerData>())
        {
            float currentDistance = float.MaxValue;
            float lightDistance;
            foreach (var e in GetEntities<LightData>())
            {
                //lightData = e;

                if (e.LightSwitch.LightIsOn)
                {
                    lightDistance = Vector3.Distance(e.LightTransform.position, runner.EnemyTransform.position);
                    if (lightDistance < currentDistance)
                    {
                        currentDistance = lightDistance;
                        lightData = e;
                        isThereLight = true;
                    }
                }
            }

            float distanceToLight = currentDistance;
            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, runner.EnemyTransform.position);

            if (isThereLight && lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight <= runner.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    runner.Animator.isRunning = true;
                    runner.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light

                }
                else
                {
                    runner.Animator.isRunning = false;
                    runner.AgentComponent.Agent.destination = runner.EnemyTransform.position; //stay where you are
                }

                if (distanceToPlayer <= runner.NightVision.Value)
                {
                    runner.Animator.isRunning = true;
                    runner.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
                }
            }
            else
            {
                if (distanceToPlayer <= runner.NightVision.Value)
                {
                    runner.Animator.isRunning = true;
                    runner.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
                }
                else
                {
                    runner.Animator.isRunning = false;
                    runner.AgentComponent.Agent.destination = runner.EnemyTransform.position;   //stay where you are
                }
            }
        }
    }

}
