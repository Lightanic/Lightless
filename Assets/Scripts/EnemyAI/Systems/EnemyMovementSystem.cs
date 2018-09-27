using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemyMovementSystem : ComponentSystem
{


    private struct AgentData
    {
        public NavAgentComponent AgentComponent;
        public EnemySpeedComponent Speed;
        public PlayerPositionComponent PlayerPosition;
        public EnemyPositionComponent EnemyPosition;
        public LightSourceComponent LightSources;
        public EnemyVisionComponent EnemyVision;
        public EnemyDarkVisionComponent NightVision;
    }

    protected override void OnUpdate()
    {

        bool lightIsOn = true;



        foreach (var enemyEntity in GetEntities<AgentData>())
        {
            //enemyEntity.AgentComponent.Agent.SetDestination(enemyEntity.PlayerPosition.PlayerPosition.position);
            float distanceToLight = Vector3.Distance(enemyEntity.LightSources.Lights[1].position, enemyEntity.EnemyPosition.EnemyPosition.position);
            float distanceToPlayer = Vector3.Distance(enemyEntity.PlayerPosition.PlayerPosition.position, enemyEntity.EnemyPosition.EnemyPosition.position);

            if (lightIsOn)
            {
                if (distanceToLight <= enemyEntity.EnemyVision.Value)
                {
                    enemyEntity.AgentComponent.Agent.SetDestination(enemyEntity.LightSources.Lights[1].position);
                }
                else
                {
                    enemyEntity.AgentComponent.Agent.destination = enemyEntity.EnemyPosition.EnemyPosition.position;
                }
            }
            else
            {
                if (distanceToPlayer <= enemyEntity.NightVision.Value)
                {
                    enemyEntity.AgentComponent.Agent.SetDestination(enemyEntity.PlayerPosition.PlayerPosition.position);
                }
                else
                {
                    enemyEntity.AgentComponent.Agent.destination = enemyEntity.EnemyPosition.EnemyPosition.position;
                }
            }
        }
    }

}
