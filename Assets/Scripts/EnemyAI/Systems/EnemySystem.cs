using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using static EnemyComponent;
using UnityEngine.AI;

public class EnemySystem : ComponentSystem
{
    private struct Enemy
    {
        public NavAgentComponent AgentComponent;
        public Transform Transform;
        public SeekComponent SeekComponent;
        public EnemyComponent EnemyComponent;
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

    EnemyState prevState;
    protected override void OnUpdate()
    {
        PlayerData player = new PlayerData();
        LightData light = new LightData();
        bool isThereLight = false;

        foreach (var entity in GetEntities<PlayerData>())
        {
            player = entity;

        }

        foreach (var enemy in GetEntities<Enemy>())
        {

            if (enemy.Transform.GetComponent<EnemyDeathComponent>().EnemyIsDead)
            {
                AkSoundEngine.PostEvent("Stop_RedMonster_Breathing", enemy.EnemyComponent.gameObject);
                AkSoundEngine.PostEvent("Stop_RedMonster_Agro", enemy.EnemyComponent.gameObject);
                continue; //run no more code if enemy is dead
            }
            if (!enemy.Transform.GetComponent<NavMeshAgent>().enabled)
            {
                continue;
            }

            if (enemy.EnemyComponent.CanLunge == false)
            {

                enemy.SeekComponent.WaitTime -= Time.deltaTime;

                if (enemy.SeekComponent.WaitTime <= 0)
                {
                    enemy.EnemyComponent.CanLunge = true;
                    enemy.SeekComponent.WaitTime = 2.0f;
                }
            }

            float currentDistance = float.MaxValue;
            float lightDistance;
            foreach (var entity in GetEntities<LightData>())
            {
                if (entity.LightSwitch.LightIsOn)
                {
                    lightDistance = Vector3.Distance(entity.LightTransform.position, enemy.Transform.position);
                    if (lightDistance < currentDistance)
                    {
                        currentDistance = lightDistance;
                        light = entity;
                        isThereLight = true;
                    }
                }
            }

            float distanceToLight = currentDistance;
            float distanceToPlayer = Vector3.Distance(player.PlayerTransform.position, enemy.Transform.position);

            prevState = enemy.EnemyComponent.State;

            enemy.EnemyComponent.State = EvaluateState(enemy.EnemyComponent.State, enemy.EnemyComponent, enemy.SeekComponent,
                distanceToLight, distanceToPlayer, light.LightSwitch, player, enemy.AgentComponent);

            enemy.AgentComponent.Agent.isStopped = false;
            if (enemy.EnemyComponent.State == EnemyState.Stun)
            {
                //enemy.AgentComponent.Agent.acceleration = float.MaxValue;
                enemy.AgentComponent.Agent.velocity = Vector3.zero;
                enemy.AgentComponent.Agent.isStopped = true;
            }
            if (enemy.EnemyComponent.Type == EnemyType.Stunner)
            {
                var stunParticle = enemy.EnemyComponent.gameObject.transform.GetChild(0).gameObject;
                if (enemy.EnemyComponent.State == EnemyState.Stun && prevState != enemy.EnemyComponent.State)
                {
                    if (stunParticle.name == "StunParticle")
                        stunParticle.SetActive(true);
                }
            
                if (enemy.EnemyComponent.State != EnemyState.Stun && enemy.EnemyComponent.State != EnemyState.Wait && prevState == EnemyState.Wait)
                {
                    if (stunParticle.name == "StunParticle")
                        stunParticle.SetActive(false);
                }
            }

            if ((enemy.EnemyComponent.State == EnemyState.Seek) && prevState != enemy.EnemyComponent.State)
            {
                AkSoundEngine.PostEvent("Play_RedMonster_Agro",enemy.EnemyComponent.gameObject);
            }

            if ((enemy.EnemyComponent.State == EnemyState.Patrol) && distanceToPlayer <= enemy.SeekComponent.AlertRadius + 10f && !enemy.SeekComponent.IsBreathing)
            {
                enemy.SeekComponent.IsBreathing = true;
                AkSoundEngine.PostEvent("Play_RedMonster_Breathing", enemy.EnemyComponent.gameObject);
            }
            else if((enemy.EnemyComponent.State == EnemyState.Patrol) && distanceToPlayer >= enemy.SeekComponent.AlertRadius + 10f)
            {
                enemy.SeekComponent.IsBreathing = false;
                AkSoundEngine.PostEvent("Stop_RedMonster_Breathing", enemy.EnemyComponent.gameObject);
            }
            if (prevState == EnemyState.Patrol && enemy.EnemyComponent.State != EnemyState.Patrol)
            {
                enemy.SeekComponent.IsBreathing = false;
                AkSoundEngine.PostEvent("Stop_RedMonster_Breathing", enemy.EnemyComponent.gameObject);
            }

            if (prevState == EnemyState.Seek && enemy.EnemyComponent.State != EnemyState.Seek)
            {
                AkSoundEngine.PostEvent("Stop_RedMonster_Agro", enemy.EnemyComponent.gameObject);
            }

        }
    }
    EnemyState EvaluateState(EnemyState currentState, EnemyComponent enemyComponent, SeekComponent seekComponent,
        float distanceToLight, float distanceToPlayer, LightComponent lightComponent, PlayerData player, NavAgentComponent agent)
    {
        //enemyComponent.GetComponent<Rigidbody>().isKinematic = true;
        switch (currentState)
        {

            case EnemyState.Patrol:
                if (distanceToLight < seekComponent.AlertRadius || distanceToPlayer <= seekComponent.NightVisionRadius)
                    return EnemyState.Alert;
                else
                    return EnemyState.Patrol;

            case EnemyState.Alert:
                if (enemyComponent.IsTargetInView || distanceToPlayer <= seekComponent.NightVisionRadius)
                    return EnemyState.Seek;
                else if ((distanceToLight < seekComponent.AlertRadius && !enemyComponent.IsTargetInView) || distanceToPlayer < seekComponent.NightVisionRadius)
                    return EnemyState.Alert;
                else
                    return EnemyState.Patrol;
            case EnemyState.Seek:
                if (distanceToLight > seekComponent.VisionRadius + 5 && distanceToPlayer >= seekComponent.NightVisionRadius + 5)
                    return EnemyState.Alert;
                else
                    return EvaluateSeek(enemyComponent, seekComponent, distanceToLight, distanceToPlayer, lightComponent, player);

            case EnemyState.Stun:
                if (!enemyComponent.GetComponent<EnemyStunComponent>().IsStunned)
                    return EnemyState.Wait;
                else
                {
                    //enemyComponent.GetComponent<Rigidbody>().isKinematic = true;
                    return EnemyState.Stun;
                }

            case EnemyState.Wait:
                return EvaluateWait(enemyComponent);

            case EnemyState.Lunge:
                if (enemyComponent.CurrentTime < enemyComponent.LungeTime)
                {
                    enemyComponent.CurrentTime += Time.deltaTime;
                    return EnemyState.Lunge;
                }
                else
                {
                    enemyComponent.CurrentTime = 0;
                    return EnemyState.Seek;
                }


        }

        return currentState;
    }


    EnemyState EvaluateSeek(EnemyComponent enemyComponent, SeekComponent seekComponent,
        float distanceToLight, float distanceToPlayer, LightComponent lightComponent, PlayerData player)
    {

        switch (enemyComponent.Type)
        {
            case EnemyType.Lunger:
                if (distanceToPlayer < seekComponent.LungeDistance)
                    return EnemyState.Wait;
                else
                    return EnemyState.Seek;

            case EnemyType.Stunner:
                if (enemyComponent.GetComponent<EnemyStunComponent>().IsStunned)
                    return EnemyState.Stun;
                else
                    return EnemyState.Seek;


        }
        return EnemyState.Seek;
    }

    EnemyState EvaluateWait(EnemyComponent enemyComponent)
    {
        if (enemyComponent.CurrentTime < enemyComponent.WaitTime)
        {
            enemyComponent.CurrentTime += Time.deltaTime;
        }
        else
        {
            enemyComponent.CurrentTime = 0;
            switch (enemyComponent.Type)
            {
                case EnemyType.Stunner:
                    return EnemyState.Seek;

                case EnemyType.Lunger:
                    return EnemyState.Lunge;

                default:
                    return EnemyState.Seek;
            }
        }

        enemyComponent.GetComponent<NavAgentComponent>().Agent.speed = 0;
       
        return EnemyState.Wait;
    }
}


