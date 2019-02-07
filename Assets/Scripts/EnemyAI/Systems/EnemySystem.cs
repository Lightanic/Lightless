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

              //  enemy.Transform.GetComponent<Rigidbody>().AddForce(enemy.Transform.forward * 1000);
            }
            NavMeshHit hit;
            enemy.AgentComponent.Agent.FindClosestEdge(out hit);
            Debug.Log(hit.distance);
            if (hit.distance < 0.1 && enemy.AgentComponent.Agent.enabled)
            {
                enemy.Transform.LookAt(player.PlayerTransform);
                enemy.AgentComponent.Agent.enabled = false;
                enemy.Transform.GetComponent<Rigidbody>().AddForce(enemy.Transform.forward * 500);
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

            enemy.EnemyComponent.State = EnemyState.Patrol;

            if (distanceToLight <= enemy.SeekComponent.VisionRadius + enemy.SeekComponent.AlertRadius)
            {
                if (isThereLight && light.LightSwitch.LightIsOn)
                {
                    enemy.EnemyComponent.State = EnemyState.Alert;
                    if (distanceToLight <= enemy.SeekComponent.VisionRadius) //if distance to light is lesser than enemy vision
                    {
                        enemy.EnemyComponent.State = EnemyState.Seek;
                        enemy.SeekComponent.Target = light.LightTransform;

                    }
                }
            }

            if (distanceToPlayer <= enemy.SeekComponent.NightVisionRadius + enemy.SeekComponent.AlertRadius)
            {
                enemy.EnemyComponent.State = EnemyState.Alert;
                if (distanceToPlayer <= enemy.SeekComponent.NightVisionRadius)
                {
                    enemy.EnemyComponent.State = EnemyState.Seek;
                    enemy.SeekComponent.Target = player.PlayerTransform;
                }
            }

            switch (enemy.EnemyComponent.Type)
            {

                case EnemyType.Lunger:
                    if (enemy.SeekComponent.Target != null && enemy.SeekComponent.Target.CompareTag("Player"))
                    {
                        if (enemy.AgentComponent.Agent.remainingDistance < enemy.SeekComponent.LungeDistance)
                        {
                            enemy.EnemyComponent.State = EnemyState.Wait;

                            if (enemy.EnemyComponent.CurrentTime < enemy.EnemyComponent.WaitTime)
                            {
                                enemy.EnemyComponent.CurrentTime += Time.deltaTime;
                            }
                            else
                            {
                                //enemy.EnemyComponent.CurrentTime = 0;
                                enemy.EnemyComponent.State = EnemyState.Lunge;
                                if (enemy.EnemyComponent.AttackTime < enemy.EnemyComponent.LungeTime)
                                {
                                    enemy.EnemyComponent.AttackTime += Time.deltaTime;
                                }
                                else
                                {
                                    enemy.EnemyComponent.AttackTime = 0;
                                    enemy.EnemyComponent.CurrentTime = 0;
                                }

                            }
                        }
                        
                    }
                        
                        break;

                case EnemyType.Stunner:
                    if (enemy.EnemyComponent.GetComponent<EnemyStunComponent>().IsStunned)
                    {
                        enemy.Transform.LookAt(enemy.SeekComponent.Target);
                        enemy.EnemyComponent.State = EnemyState.Stun;
                        enemy.AgentComponent.Agent.speed = 0;
                    }
                    break;
            }
        }
    }
}


