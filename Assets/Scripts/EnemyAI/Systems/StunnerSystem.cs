using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class StunnerSystem : ComponentSystem
{

    private struct StunnerData
    {
        public NavAgentComponent AgentComponent;
        public Transform EnemyTransform;
        public EnemyVisionComponent EnemyVision;
        public EnemyDarkVisionComponent NightVision;
        public EnemyStunComponent StunComponent;
        public WayPointComponent PatrolData;
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
        foreach (var stunner in GetEntities<StunnerData>())
        {
            float currentDistance = float.MaxValue;
            float lightDistance;
            foreach (var e in GetEntities<LightData>())
            {
                //lightData = e;

                if (e.LightSwitch.LightIsOn)
                {
                    lightDistance = Vector3.Distance(e.LightTransform.position, stunner.EnemyTransform.position);
                    if (lightDistance < currentDistance)
                    {
                        currentDistance = lightDistance;
                        lightData = e;
                        isThereLight = true;
                    }
                }
            }

            float distanceToLight = currentDistance;
            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, stunner.EnemyTransform.position);

            if (isThereLight && lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight <= stunner.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    //stunner.Animator.isRunning = true;
                    stunner.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light

                }
                else
                {
                    //runner.Animator.isRunning = false;
                    //stunner.AgentComponent.Agent.destination = stunner.EnemyTransform.position; //stay where you are
                    Wander(stunner);
                }

                if (distanceToPlayer <= stunner.NightVision.Value)
                {
                    //enemyEntity.Animator.isRunning = true;
                    //stunner.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
                    Seek(stunner, playerData);
                }
            }
            else
            {
                if (distanceToPlayer <= stunner.NightVision.Value)
                {
                    //stunner.Animator.isRunning = true;
                    //stunner.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
                    Seek(stunner, playerData);
                }
                else
                {
                    //stunner.Animator.isRunning = false;
                    //stunner.AgentComponent.Agent.destination = stunner.EnemyTransform.position;   //stay where you are
                    
                    Wander(stunner);
                }
            }
            if (stunner.StunComponent.IsStunned == true)
            {
                stunner.AgentComponent.Agent.speed = 0;
            }
            else if (stunner.StunComponent.IsStunned == false && stunner.PatrolData.IsWandering == false)
            {
                stunner.AgentComponent.Agent.speed = 7;
            }
            else if (stunner.StunComponent.IsStunned == false && stunner.PatrolData.IsWandering == true)
            {
                stunner.AgentComponent.Agent.speed = 3;
            }
        }
    }

    void Wander(StunnerData stunner)
    {
        //stunner.AgentComponent.Agent.speed = 3;
        stunner.PatrolData.IsWandering = true;
        stunner.AgentComponent.Agent.SetDestination(stunner.PatrolData.Waypoints[stunner.PatrolData.currentWaypointIndex].position);

        if (Vector3.Distance(stunner.EnemyTransform.position, stunner.PatrolData.Waypoints[stunner.PatrolData.currentWaypointIndex].position) <= 2)
        {
            stunner.PatrolData.currentWaypointIndex = Random.Range(0, stunner.PatrolData.Waypoints.Length);
            stunner.AgentComponent.Agent.SetDestination(stunner.PatrolData.Waypoints[stunner.PatrolData.currentWaypointIndex].position);
        }
        
        
    }

    void Seek(StunnerData stunner, PlayerData player)
    {
        stunner.PatrolData.IsWandering = false;
        stunner.AgentComponent.Agent.SetDestination(player.PlayerTransform.position);
    }

}

    
