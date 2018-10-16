using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemyAISystem : ComponentSystem
{

    private struct RunnerData
    {
        public NavAgentComponent AgentComponent;
        public Transform EnenmyTransform;
        public EnemyVisionComponent EnemyVision;
        public EnemyDarkVisionComponent NightVision;
        public EnemyAnimator Animator;
       
    }

    private struct LungerData
    {
        public NavAgentComponent AgentComponent;
        public Transform EnenmyTransform;
        public EnemyVisionComponent EnemyVision;
        public EnemyLungeComponent LungeDistance;
        public EnemyAnimator Animator;

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
        PlayerData playerData = new PlayerData();
        LightData lightData = new LightData();
        bool isThereLight = false;


        foreach (var entity in GetEntities<PlayerData>())
        {
            playerData = entity;
            
        }



        foreach (var enemyEntity in GetEntities<RunnerData>())
        {
            float currentDistance = float.MaxValue;
            float lightDistance;
            foreach (var e in GetEntities<LightData>())
            {
                //lightData = e;

                if (e.LightSwitch.LightIsOn)
                {
                    lightDistance = Vector3.Distance(e.LightTransform.position, enemyEntity.EnenmyTransform.position);
                    if (lightDistance < currentDistance)
                    {
                        currentDistance = lightDistance;
                        lightData = e;
                        isThereLight = true;
                    }
                }
            }

            float distanceToLight = currentDistance;
            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, enemyEntity.EnenmyTransform.position);

            if (isThereLight && lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight <= enemyEntity.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    enemyEntity.Animator.isRunning = true;
                    enemyEntity.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light
                    
                }
                else
                {
                    enemyEntity.Animator.isRunning = false;
                    enemyEntity.AgentComponent.Agent.destination = enemyEntity.EnenmyTransform.position; //stay where you are
                }

                if (distanceToPlayer <= enemyEntity.NightVision.Value)
                {
                    enemyEntity.Animator.isRunning = true;
                    enemyEntity.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
                }
            }
            else
            {
                if (distanceToPlayer <= enemyEntity.NightVision.Value)
                {
                    enemyEntity.Animator.isRunning = true;
                    enemyEntity.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
                }
                else
                {
                    enemyEntity.Animator.isRunning = false;
                    enemyEntity.AgentComponent.Agent.destination = enemyEntity.EnenmyTransform.position;   //stay where you are
                }
            }
        }

        foreach (var lunger in GetEntities<LungerData>())
        {
            float currentDistance = float.MaxValue;
            float lightDistance;
            foreach (var e in GetEntities<LightData>())
            {
                //lightData = e;

                if (e.LightSwitch.LightIsOn)
                {
                    lightDistance = Vector3.Distance(e.LightTransform.position, lunger.EnenmyTransform.position);
                    if (lightDistance < currentDistance)
                    {
                        currentDistance = lightDistance;
                        lightData = e;
                        isThereLight = true;
                    }
                }
            }

            float distanceToLight = currentDistance;
            //float distanceToLight = Vector3.Distance(lightData.LightTransform.position, lunger.EnenmyTransform.position);
            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, lunger.EnenmyTransform.position);

            if (lunger.LungeDistance.IsPrelunging)
            {
                lunger.Animator.isWalking = false;
                lunger.Animator.isRunning = false;
                lunger.AgentComponent.Agent.speed = 0.0f;
                if (lunger.LungeDistance.CurrentTimeForPrelunging > lunger.LungeDistance.PrelungeTime)
                {
                    lunger.LungeDistance.IsPrelunging = false;
                    lunger.LungeDistance.IsLunging = true;
                    lunger.LungeDistance.CurrentTimeForPrelunging = 0.0f;
                }
                else
                {
                    
                    lunger.LungeDistance.CurrentTimeForPrelunging += Time.deltaTime;
                }
                continue;

            }

            if (lunger.LungeDistance.IsLunging)
            {
                lunger.Animator.isRunning = true;
                Lunge(lunger, playerData);
                if (lunger.LungeDistance.CurrentTimeForLunging > lunger.LungeDistance.LungeTime)
                {
                    lunger.LungeDistance.IsLunging = false;
                    lunger.LungeDistance.CurrentTimeForLunging = 0.0f;
                    lunger.AgentComponent.Agent.speed = 7;
                }
                else
                {
                    lunger.LungeDistance.CurrentTimeForLunging += Time.deltaTime;
                }
                continue;
            }

            if (isThereLight && lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight <= lunger.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    lunger.Animator.isWalking = true;
                    lunger.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light

                }

                else
                {
                    lunger.Animator.isWalking = false;
                    lunger.Animator.isRunning = false;
                    lunger.AgentComponent.Agent.destination = lunger.EnenmyTransform.position; //stay where you are
                }

                if (distanceToPlayer <= lunger.LungeDistance.LungeValue && !lunger.LungeDistance.IsPrelunging && !lunger.LungeDistance.IsLunging)
                {
                    //Debug.Log("get ready for the lunge");
                    lunger.LungeDistance.IsPrelunging = true;

                }

            }
            else
            {
                if (distanceToPlayer <= lunger.LungeDistance.LungeValue && !lunger.LungeDistance.IsPrelunging && !lunger.LungeDistance.IsLunging)
                {
                    //Debug.Log("get ready for the lunge");
                    lunger.LungeDistance.IsPrelunging = true;
                }
                else
                {
                    lunger.AgentComponent.Agent.destination = lunger.EnenmyTransform.position;   //stay where you are
                }
            }
        }
    }

    void Lunge(LungerData lunger, PlayerData player)
    {
        if (lunger.LungeDistance.IsLunging)
        {
            //Debug.Log("lunging");
            lunger.AgentComponent.Agent.speed = Random.Range(20, 40);
            //Debug.Log(lunger.AgentComponent.Agent.speed);
            lunger.AgentComponent.Agent.SetDestination(player.PlayerTransform.position); //seek player
        }

    }

    void Wander()
    {
        //random rotate
        //move if isWalking is true
    }


}
