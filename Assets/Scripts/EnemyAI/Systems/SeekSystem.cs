using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using static EnemyComponent;

public class SeekSystem : ComponentSystem
{
    private struct Seek
    {
        public SeekComponent SeekComponent;
        public EnemyComponent Enemy;
        public NavAgentComponent AgentComponent;
    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Seek>())
        {
            if (entity.Enemy.State == EnemyState.Seek)
            {
                switch (entity.Enemy.Type)
                {
                    case EnemyType.Runner:
                        RunnerSeek(entity, entity.SeekComponent.Target);
                        break;
                }
               

            }
        }
    }

    void RunnerSeek(Seek entity, Transform target)
    {
        entity.AgentComponent.Agent.speed = 12;
        if (entity.SeekComponent.Target.gameObject.tag == "Flashlight")
        {
            if (entity.AgentComponent.Agent.enabled)
                entity.AgentComponent.Agent.SetDestination(target.position + target.forward * 8);
        }
        else
        {
            if (entity.AgentComponent.Agent.enabled)
                entity.AgentComponent.Agent.SetDestination(target.position);
        }
    }
}


