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
            if (!enemy.AgentComponent.Agent.enabled)
            {
                return;
            }
            NavMeshHit hit;
            enemy.AgentComponent.Agent.FindClosestEdge(out hit);
            //Debug.Log(hit.distance);
            if (hit.distance < 0.01 && enemy.AgentComponent.Agent.enabled)
            {
                //enemy.Transform.LookAt(player.PlayerTransform);
                enemy.AgentComponent.Agent.enabled = false;
                enemy.Transform.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(enemy.Transform.forward + enemy.Transform.up) * 600);
            }



            if (enemy.Transform.GetComponent<EnemyDeathComponent>().EnemyIsDead)
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

            enemy.EnemyComponent.State = EvaluateState(enemy.EnemyComponent.State, enemy.EnemyComponent, enemy.SeekComponent,
                distanceToLight, distanceToPlayer, light.LightSwitch, player, enemy.AgentComponent);

            if (enemy.EnemyComponent.State == EnemyState.Stun)
            {
                enemy.AgentComponent.Agent.speed = 0;
            }

            //    if (distanceToLight <= enemy.SeekComponent.VisionRadius + enemy.SeekComponent.AlertRadius)
            //    {
            //        if (isThereLight && light.LightSwitch.LightIsOn)
            //        {
            //            enemy.EnemyComponent.State = EnemyState.Alert;
            //            if (distanceToLight <= enemy.SeekComponent.VisionRadius) //if distance to light is lesser than enemy vision
            //            {
            //                enemy.EnemyComponent.State = EnemyState.Seek;
            //                enemy.SeekComponent.Target = light.LightTransform;

            //            }
            //        }
            //    }

            //    if (distanceToPlayer <= enemy.SeekComponent.NightVisionRadius + enemy.SeekComponent.AlertRadius)
            //    {
            //        enemy.EnemyComponent.State = EnemyState.Alert;
            //        if (distanceToPlayer <= enemy.SeekComponent.NightVisionRadius)
            //        {
            //            enemy.EnemyComponent.State = EnemyState.Seek;
            //            enemy.SeekComponent.Target = player.PlayerTransform;
            //        }
            //    }

            //    switch (enemy.EnemyComponent.Type)
            //    {

            //        case EnemyType.Lunger:
            //            if (enemy.SeekComponent.Target != null && enemy.SeekComponent.Target.CompareTag("Player"))
            //            {
            //                if (enemy.AgentComponent.Agent.remainingDistance < enemy.SeekComponent.LungeDistance)
            //                {
            //                    enemy.EnemyComponent.State = EnemyState.Wait;

            //                    if (enemy.EnemyComponent.CurrentTime < enemy.EnemyComponent.WaitTime)
            //                    {
            //                        enemy.EnemyComponent.CurrentTime += Time.deltaTime;
            //                    }
            //                    else
            //                    {
            //                        //enemy.EnemyComponent.CurrentTime = 0;
            //                        enemy.EnemyComponent.State = EnemyState.Lunge;
            //                        if (enemy.EnemyComponent.AttackTime < enemy.EnemyComponent.LungeTime)
            //                        {
            //                            enemy.EnemyComponent.AttackTime += Time.deltaTime;
            //                        }
            //                        else
            //                        {
            //                            enemy.EnemyComponent.AttackTime = 0;
            //                            enemy.EnemyComponent.CurrentTime = 0;
            //                        }

            //                    }
            //                }

            //            }

            //            break;

            //        case EnemyType.Stunner:
            //            if (enemy.EnemyComponent.GetComponent<EnemyStunComponent>().IsStunned)
            //            {
            //                enemy.Transform.LookAt(enemy.SeekComponent.Target);
            //                enemy.EnemyComponent.State = EnemyState.Stun;
            //                enemy.AgentComponent.Agent.speed = 0;
            //            }
            //            break;
            //    }
            //}
        }
    }
    EnemyState EvaluateState(EnemyState currentState, EnemyComponent enemyComponent, SeekComponent seekComponent,
        float distanceToLight, float distanceToPlayer, LightComponent lightComponent, PlayerData player, NavAgentComponent agent)
    {
        enemyComponent.GetComponent<Rigidbody>().isKinematic = false;
        switch (currentState)
        {

            case EnemyState.Patrol:
                if (distanceToLight < seekComponent.AlertRadius || distanceToPlayer < seekComponent.VisionRadius)
                    return EnemyState.Alert;
                else
                    return EnemyState.Patrol;

            case EnemyState.Alert:
                Vector3 targetDir = player.PlayerTransform.position - enemyComponent.transform.position;
                targetDir.y = enemyComponent.transform.position.y;
                enemyComponent.transform.rotation = Quaternion.Slerp(enemyComponent.transform.rotation,Quaternion.LookRotation(targetDir), Time.deltaTime); //Quaternion.RotateTowards(enemyComponent.transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime);
                //enemyComponent.transform.LookAt(player.PlayerTransform);
                agent.Agent.speed = 0;
                enemyComponent.GetComponent<Rigidbody>().isKinematic = true;
                //agent.Agent.SetDestination(enemyComponent.transform.position);
                if (distanceToLight > seekComponent.AlertRadius && distanceToPlayer > seekComponent.VisionRadius)
                    return EnemyState.Patrol;
                else if ((distanceToLight < seekComponent.VisionRadius && lightComponent.LightIsOn) || distanceToPlayer < seekComponent.NightVisionRadius)
                    return EnemyState.Seek;
                else
                    return EnemyState.Alert;

            case EnemyState.Seek:
                if (distanceToLight > seekComponent.VisionRadius && distanceToPlayer > seekComponent.NightVisionRadius)
                    return EnemyState.Alert;
                else
                    return EvaluateSeek(enemyComponent, seekComponent, distanceToLight, distanceToPlayer, lightComponent, player);

            case EnemyState.Stun:
                if (!enemyComponent.GetComponent<EnemyStunComponent>().IsStunned)
                    return EnemyState.Wait;
                else
                    return EnemyState.Stun;

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
       
        if (distanceToLight < seekComponent.VisionRadius)
        {
            seekComponent.Target = lightComponent.transform;
        }
        if (distanceToPlayer < seekComponent.NightVisionRadius)
        {
            seekComponent.Target = player.PlayerTransform;
        }

        

        switch (enemyComponent.Type)
        {
            case EnemyType.Lunger:
                if (distanceToPlayer < seekComponent.LungeDistance)
                    return EnemyState.Wait;
                else
                    return EnemyState.Seek;

            case EnemyType.Stunner:
                if (enemyComponent.GetComponent<EnemyStunComponent>().IsStunned 
                    && GameObject.FindGameObjectWithTag("Flashlight").GetComponent<LightComponent>().LightIsOn)
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


