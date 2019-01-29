using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


public class SeekSystem : ComponentSystem
{

    private struct Enemy
    {
        public NavAgentComponent Agent;
        public Transform Transform;
        public SeekComponent Seek;
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
            if (enemy.Transform.GetComponent<EnemyDeathComponent>().EnemyIsDead)
            {
                continue;
            }

            float currentDistance = float.MaxValue;
            float lightDistance;
            foreach (var entity in GetEntities<LightData>())
            {
                //lightData = e;

                if (light.LightSwitch.LightIsOn)
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

            if (isThereLight && light.LightSwitch.LightIsOn)
            {
                if (distanceToLight > enemy.Seek.VisionRadius && distanceToLight <= enemy.Seek.VisionRadius + enemy.Seek.AlertRadius)
                {
                    enemy.EnemyComponent.IsRunning = false;
                    enemy.EnemyComponent.IsWalking = true;
                    enemy.EnemyComponent.IsAlerted = true;
                    enemy.EnemyComponent.IsPatrolling = true;
                }
                else if (distanceToLight <= enemy.Seek.VisionRadius) //if distance to light is lesser than enemy vision
                {
                    enemy.EnemyComponent.IsRunning = true;
                    enemy.EnemyComponent.IsWalking = false;

                    //seek the light
                    //Seek(runner, lightData.LightTransform);

                }
                else
                {
                    enemy.EnemyComponent.IsRunning = false;
                    enemy.EnemyComponent.IsWalking = true;

                    //patrolling
                    enemy.EnemyComponent.IsPatrolling = true;
                }

                if (distanceToPlayer <= enemy.Seek.NightVisionRadius)
                {
                    enemy.EnemyComponent.IsRunning = true;
                    //seek player
                    //Seek(runner, playerData.PlayerTransform);
                }
            }
            else
            {
                if (distanceToLight > enemy.Seek.NightVisionRadius && distanceToLight <= enemy.Seek.NightVisionRadius + enemy.Seek.AlertRadius)
                {
                    enemy.EnemyComponent.IsRunning = false;
                    enemy.EnemyComponent.IsWalking = true;
                    enemy.EnemyComponent.IsAlerted = true;
                    enemy.EnemyComponent.IsPatrolling = true;
                }
                else if (distanceToPlayer <= enemy.Seek.NightVisionRadius)
                {
                    enemy.EnemyComponent.IsRunning = true;
                    enemy.EnemyComponent.IsWalking = false;
                    //seek player
                    //Seek(runner, playerData.PlayerTransform);
                }
                else
                {
                    enemy.EnemyComponent.IsRunning = false;
                    //patrolling
                    enemy.EnemyComponent.IsPatrolling = true;
                    enemy.EnemyComponent.IsWalking = true;
                }
            }
        }


    }
}


