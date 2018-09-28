using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemyAISystem : ComponentSystem
{


    private struct AgentData
    {
        public NavAgentComponent AgentComponent;
        public EnemySpeedComponent Speed;
        public Transform EnenmyTransform;
        public EnemyVisionComponent EnemyVision;
        public EnemyDarkVisionComponent NightVision;
       
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


        foreach (var entity in GetEntities<PlayerData>())
        {
            playerData = entity;
            
        }

        foreach (var e in GetEntities<LightData>())
        {
            //for (int i = 1; i <= lightData.LightTransform.Length; i++)
            //{

            //}
            lightData = e;
        }

        foreach (var enemyEntity in GetEntities<AgentData>())
        {
            //enemyEntity.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position);
            //enemyEntity.AgentComponent.Agent.SetDestination(lightData.LightTransform.position);
            float distanceToLight = Vector3.Distance(lightData.LightTransform.position, enemyEntity.EnenmyTransform.position);
            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, enemyEntity.EnenmyTransform.position);

            if (lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight <= enemyEntity.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    enemyEntity.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light
                }
                else
                {
                    enemyEntity.AgentComponent.Agent.destination = enemyEntity.EnenmyTransform.position; //stay where you are
                }
            }
            else
            {
                if (distanceToPlayer <= enemyEntity.NightVision.Value)
                {
                    enemyEntity.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
                }
                else
                {
                    enemyEntity.AgentComponent.Agent.destination = enemyEntity.EnenmyTransform.position;   //stay where you are
                }
            }
        }
    }

}
