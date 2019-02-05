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

                    case EnemyType.Lunger:
                        LungerSeek(entity, entity.SeekComponent.Target);
                        break;

                    case EnemyType.Stunner:
                        StunnerSeek(entity, entity.SeekComponent.Target);
                        break;
                }
               

            }
        }
    }

    void RunnerSeek(Seek entity, Transform target)
    {
        entity.AgentComponent.Agent.speed = 12;
        if (entity.SeekComponent.Target.gameObject.CompareTag("Flashlight"))
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

    void LungerSeek(Seek entity, Transform target)
    {
        entity.AgentComponent.Agent.speed = 12;

        if (entity.SeekComponent.Target.CompareTag("Player"))
        {
           entity.AgentComponent.Agent.SetDestination(target.position);
            if (entity.AgentComponent.Agent.remainingDistance < entity.SeekComponent.LungeDistance)
            {
                entity.Enemy.State = EnemyState.Lunge;
                entity.AgentComponent.Agent.Move(entity.Enemy.transform.forward * 0.5f);
                
            }
            
        }
        else
        {
            if (entity.SeekComponent.Target.gameObject.CompareTag("Flashlight"))
            {
                if (entity.AgentComponent.Agent.enabled)
                    entity.AgentComponent.Agent.SetDestination(target.position + target.forward * 8);
            }
            else 
                entity.AgentComponent.Agent.SetDestination(target.position);
        }
    }

    void StunnerSeek(Seek entity, Transform target)
    {
        entity.AgentComponent.Agent.speed = 12;
        entity.AgentComponent.Agent.SetDestination(target.position);
        //if (entity.Enemy.GetComponent<EnemyStunComponent>().IsStunned)
        //{
        //    entity.Enemy.State = EnemyState.Stun;
        //    entity.AgentComponent.Agent.speed = 0;
        //}

        if (entity.SeekComponent.Target.gameObject.CompareTag("Flashlight"))
        {
            if (entity.AgentComponent.Agent.enabled)
                entity.AgentComponent.Agent.SetDestination(target.position + target.forward * 8);
            //if (entity.Enemy.GetComponent<EnemyStunComponent>().IsStunned)
            //{
            //    entity.Enemy.State = EnemyState.Stun;
            //    entity.AgentComponent.Agent.speed = 0;
            //}
        }

        //if (entity.SeekComponent.Target.CompareTag("Fire"))
        //{
        //    entity.AgentComponent.Agent.SetDestination(target.position);
        //    if (entity.AgentComponent.Agent.remainingDistance < 9)
        //    {
                
        //        entity.Enemy.State = EnemyState.Stun;
        //        entity.AgentComponent.Agent.speed = 0;
        //    }
            
        //}
        //else
        //{
        //    if (entity.AgentComponent.Agent.enabled)
        //    {
        //        entity.AgentComponent.Agent.SetDestination(target.position);
        //        if (entity.Enemy.GetComponent<EnemyStunComponent>().IsStunned)
        //        {
        //            entity.Enemy.State = EnemyState.Stun;
        //            entity.AgentComponent.Agent.speed = 0;
        //        }

        //    }

        //}
        entity.Enemy.GetComponent<EnemyStunComponent>().IsStunned = false;
    }
}


