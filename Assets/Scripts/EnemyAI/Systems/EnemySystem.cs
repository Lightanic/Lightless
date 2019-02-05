using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using static EnemyComponent;

public class EnemySystem : ComponentSystem
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

            if (enemy.EnemyComponent.CanLunge == false)
            {

                enemy.Seek.WaitTime -= Time.deltaTime;

                if (enemy.Seek.WaitTime <= 0)
                {
                    enemy.EnemyComponent.CanLunge = true;
                    enemy.Seek.WaitTime = 2.0f;
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

            if (distanceToLight <= enemy.Seek.VisionRadius + enemy.Seek.AlertRadius)
            {
                if (isThereLight && light.LightSwitch.LightIsOn)
                {
                    enemy.EnemyComponent.State = EnemyState.Alert;
                    if (distanceToLight <= enemy.Seek.VisionRadius) //if distance to light is lesser than enemy vision
                    {
                        enemy.EnemyComponent.State = EnemyState.Seek;
                        enemy.Seek.Target = light.LightTransform;

                    }
                }
            }

            if (distanceToPlayer <= enemy.Seek.NightVisionRadius + enemy.Seek.AlertRadius)
            {
                enemy.EnemyComponent.State = EnemyState.Alert;
                if (distanceToPlayer <= enemy.Seek.NightVisionRadius)
                {
                    enemy.EnemyComponent.State = EnemyState.Seek;
                    enemy.Seek.Target = player.PlayerTransform;
                }
            }

            switch (enemy.EnemyComponent.Type)
            {
                case EnemyType.Stunner:
                    if (enemy.EnemyComponent.GetComponent<EnemyStunComponent>().IsStunned)
                    {
                        enemy.EnemyComponent.State = EnemyState.Stun;
                        enemy.Agent.Agent.speed = 0;
                    }
                    break;
            }
        }
    }
}


