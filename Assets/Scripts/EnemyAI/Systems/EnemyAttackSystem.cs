using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemyAttackSystem : ComponentSystem {


    private struct AttackData
    {
        public NavAgentComponent AgentComponent;
        public EnemySpeedComponent Speed;
        public PlayerPositionComponent PlayerPosition;
    }

    protected override void OnUpdate()
    {
        foreach (var enemyEntity in GetEntities<AttackData>())
        {
            enemyEntity.AgentComponent.Agent.SetDestination(enemyEntity.PlayerPosition.PlayerPosition.position);
        }
    }


}
