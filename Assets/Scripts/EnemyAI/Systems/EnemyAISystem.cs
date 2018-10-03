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
       
    }

    private struct LungerData
    {
        public NavAgentComponent AgentComponent;
        public Transform EnenmyTransform;
        public EnemyVisionComponent EnemyVision;
        public EnemyLungeComponent LungeDistance;

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
                    enemyEntity.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light
                }
                else
                {
                    enemyEntity.AgentComponent.Agent.destination = enemyEntity.EnenmyTransform.position; //stay where you are
                }
            }
            else
            {
                if (distanceToPlayer <= enemyEntity.NightVision.Value)
                {
                    enemyEntity.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
                }
                else
                {
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
                

            }

            if (lunger.LungeDistance.IsLunging)
            {
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
            }

            if (isThereLight && lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight <= lunger.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    lunger.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light
                    if (distanceToPlayer <= lunger.LungeDistance.LungeValue && !lunger.LungeDistance.IsPrelunging && !lunger.LungeDistance.IsLunging)
                    {
                        //Debug.Log("get ready for the lunge");
                        lunger.LungeDistance.IsPrelunging = true;
                           
                    }
                }

                else
                {
                    lunger.AgentComponent.Agent.destination = lunger.EnenmyTransform.position; //stay where you are
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
            lunger.AgentComponent.Agent.speed = 20;
            lunger.AgentComponent.Agent.SetDestination(player.PlayerTransform.position); //seek player
        }

    }


}
