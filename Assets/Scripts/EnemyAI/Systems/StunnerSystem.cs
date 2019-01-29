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
        // public StunnerAnimator AnimatorComponent;
    }

    private struct StunnerAnimatorStruct
    {
        public EnemyStunComponent StunComponent;
        public WayPointComponent PatrolData;
        public StunnerAnimator AnimatorComponent;

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

    private struct StunnerDeath
    {
        public EnemyDeathComponent DeathComponent;
        public Transform EnemyTransform;
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

        foreach (var entity in GetEntities<StunnerDeath>())
        {
            if (entity.DeathComponent.EnemyIsDead)
            {
                entity.EnemyTransform.GetComponent<BoxCollider>().enabled = false;
            }
        }

        foreach (var stunner in GetEntities<StunnerData>())
        {

            if (stunner.EnemyTransform.GetComponent<EnemyDeathComponent>().EnemyIsDead)
            {
                continue;
            }
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
            stunner.EnemyVision.IsAlerted = false;
            stunner.EnemyVision.IsSeeking = false;

            if (isThereLight && lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight > stunner.EnemyVision.Value && distanceToLight <= stunner.EnemyVision.Value + stunner.EnemyVision.AlertValue)
                {

                    stunner.EnemyVision.IsAlerted = true;
                    stunner.PatrolData.IsWandering = true;
                }
                else if (distanceToLight <= stunner.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    //stunner.Animator.isRunning = true;
                    //stunner.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); 
                    //seek the light
                    Seek(stunner, lightData.LightTransform);

                }
                else
                {
                    //patrolling
                    stunner.PatrolData.IsWandering = true;
                }


                if (distanceToPlayer <= stunner.NightVision.Value)
                {
                    //seek player

                    Seek(stunner, playerData.PlayerTransform);
                }
            }
            else
            {
                if (distanceToLight > stunner.NightVision.Value && distanceToLight <= stunner.NightVision.Value + stunner.EnemyVision.AlertValue)
                {

                    stunner.EnemyVision.IsAlerted = true;
                    stunner.PatrolData.IsWandering = true;
                }
                else if (distanceToPlayer <= stunner.NightVision.Value)
                {
                    //seek player
                    Seek(stunner, playerData.PlayerTransform);
                }
                else
                {
                    //patroling
                    stunner.PatrolData.IsWandering = true;
                }
            }
            if (stunner.StunComponent.IsStunned == true)
            {
                stunner.AgentComponent.Agent.speed = 0;
                stunner.StunComponent.WaitTime = 4.0f;
                
                stunner.AgentComponent.Agent.SetDestination(stunner.EnemyTransform.position);
            }
            if (stunner.StunComponent.WaitTime > 0)
            {
                stunner.StunComponent.IsWaiting = true;
                stunner.StunComponent.WaitTime -= 2 * Time.deltaTime;
            }
            if (stunner.StunComponent.IsStunned == false && stunner.PatrolData.IsWandering == false && stunner.StunComponent.WaitTime < 0)
            {
                stunner.StunComponent.IsWaiting = false;
                stunner.AgentComponent.Agent.speed = 9;    
                
            }
            if (stunner.PatrolData.IsWandering == true)
            {
                stunner.AgentComponent.Agent.speed = 3;
            }


        }

        foreach (var e in GetEntities<StunnerAnimatorStruct>())
        {
            UpdateAnimation(e);

        }
    }


    void UpdateAnimation(StunnerAnimatorStruct stunner)
    {
        if (stunner.PatrolData.IsWandering && !stunner.StunComponent.IsStunned)
        {
            stunner.AnimatorComponent.isRunning = false;
            stunner.AnimatorComponent.isStunned = false;
            stunner.AnimatorComponent.isWalking = true;
        }
        else if (stunner.StunComponent.IsStunned || stunner.StunComponent.IsWaiting)
        {
            stunner.AnimatorComponent.isStunned = true;
            stunner.AnimatorComponent.isWalking = false;

        }
        else //if (!stunner.StunComponent.IsStunned && !stunner.PatrolData.IsWandering)
        {
            stunner.AnimatorComponent.isStunned = false;
            stunner.AnimatorComponent.isWalking = false;
            stunner.AnimatorComponent.isRunning = true;
        }

    }


    void Seek(StunnerData stunner, Transform target)
    {
        stunner.EnemyVision.IsSeeking = true;
        stunner.PatrolData.IsWandering = false;
        stunner.AgentComponent.Agent.speed = 9;
        if (target.gameObject.tag == "Flashlight")
        {
            stunner.AgentComponent.Agent.SetDestination(target.position + target.forward * 8);
        }
        if (target.gameObject.CompareTag("Fire"))
        {
            stunner.AgentComponent.Agent.SetDestination(target.position);
            if (Vector3.Distance(stunner.EnemyTransform.position, target.position) < 7)
            {
                stunner.StunComponent.IsStunned = true;
            }
            else
                stunner.StunComponent.IsStunned = false;
        }
        else
            stunner.AgentComponent.Agent.SetDestination(target.position);
    }

}


