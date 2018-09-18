using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

public class EnemyMovement : ComponentSystem
{

    private struct AgentData
    {
        public NavAgentComponent AgentComponent;
        public EnemySpeedComponent Speed;
        public PlayerPositionComponent PlayerPosition;
    }

    protected override void OnUpdate()
    {
        foreach (var enemyEntity in GetEntities<AgentData>())
        {
            enemyEntity.AgentComponent.Agent.SetDestination(enemyEntity.PlayerPosition.PlayerPosition.position);
        }
    }
}
