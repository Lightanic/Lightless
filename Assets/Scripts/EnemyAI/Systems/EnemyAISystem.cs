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



        foreach (var entity in GetEntities<PlayerData>())
        {
            playerData = entity;
            
        }

        foreach (var e in GetEntities<LightData>())
        {
            //for (int i = 1; i <= lightData.LightTransform.Length; i++)
            //{

            //}
            lightData = e;
        }

        foreach (var enemyEntity in GetEntities<RunnerData>())
        {
            //enemyEntity.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position);
            //enemyEntity.AgentComponent.Agent.SetDestination(lightData.LightTransform.position);
            float distanceToLight = Vector3.Distance(lightData.LightTransform.position, enemyEntity.EnenmyTransform.position);
            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, enemyEntity.EnenmyTransform.position);

            if (lightData.LightSwitch.LightIsOn)
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
            float distanceToLight = Vector3.Distance(lightData.LightTransform.position, lunger.EnenmyTransform.position);
            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, lunger.EnenmyTransform.position);

            if (lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight <= lunger.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    lunger.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light
                    if (distanceToPlayer <= lunger.LungeDistance.LungeValue)
                    {
                        Debug.Log("get ready for the lunge");
                        lunger.AgentComponent.Agent.speed = 0f;
                        if (lunger.LungeDistance.CurrentTime > lunger.LungeDistance.PrelungeTime)
                        {
                            lunger.LungeDistance.IsLunging = true;
                            Lunge(lunger, playerData);
                            lunger.LungeDistance.CurrentTime = 0.0f;
                        }
                        else
                        {
                            lunger.LungeDistance.CurrentTime += Time.deltaTime;
                        }
                           
                    }
                }

                else
                {
                    lunger.AgentComponent.Agent.destination = lunger.EnenmyTransform.position; //stay where you are
                }

            }
            else
            {
                if (distanceToPlayer <= lunger.LungeDistance.LungeValue)
                {
                    Debug.Log("get ready for the lunge");
                    lunger.AgentComponent.Agent.speed = 2f;
                    if (lunger.LungeDistance.CurrentTime > lunger.LungeDistance.PrelungeTime)
                    {
                        lunger.LungeDistance.IsLunging = true;
                        Lunge(lunger, playerData);
                    }
                    else
                        lunger.LungeDistance.CurrentTime += Time.deltaTime;
                    //if (distanceToPlayer <= lunger.LungeDistance.LungeValue)
                    //{
                    //    lunger.LungeDistance.IsLunging = true;
                    //    Lunge(lunger, playerData);
                    //}
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
            Debug.Log("lunging");
            if(lunger.LungeDistance.IsLunging)
            lunger.AgentComponent.Agent.speed = 20;
            lunger.AgentComponent.Agent.SetDestination(player.PlayerTransform.position); //seek player
            
        }
        else
        {
            lunger.AgentComponent.Agent.speed = 0;
        }

    }


}
