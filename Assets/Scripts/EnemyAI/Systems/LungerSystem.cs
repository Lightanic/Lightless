using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class LungerSystem : ComponentSystem
{

    private struct LungerData
    {
        public NavAgentComponent AgentComponent;
        public Transform EnemyTransform;
        public EnemyVisionComponent EnemyVision;
        public EnemyLungeComponent LungeComponent;
        public EnemyAnimator Animator;
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
        foreach (var lunger in GetEntities<LungerData>())
        {

            float currentDistance = float.MaxValue;
            float lightDistance;
            foreach (var e in GetEntities<LightData>())
            {
                //lightData = e;

                if (e.LightSwitch.LightIsOn)
                {
                    lightDistance = Vector3.Distance(e.LightTransform.position, lunger.EnemyTransform.position);
                    if (lightDistance < currentDistance)
                    {
                        currentDistance = lightDistance;
                        lightData = e;
                        isThereLight = true;
                    }
                }
            }

            float distanceToLight = currentDistance;

            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, lunger.EnemyTransform.position);

            if (lunger.LungeComponent.IsPrelunging)
            {
                lunger.Animator.isWalking = false;
                lunger.Animator.isRunning = false;
                lunger.AgentComponent.Agent.speed = 0.0f;
                if (lunger.LungeComponent.CurrentTimeForPrelunging > lunger.LungeComponent.PrelungeTime)
                {
                    lunger.LungeComponent.IsPrelunging = false;
                    lunger.LungeComponent.IsLunging = true;
                    lunger.LungeComponent.CurrentTimeForPrelunging = 0.0f;
                }
                else
                {

                    lunger.LungeComponent.CurrentTimeForPrelunging += Time.deltaTime;
                }
                continue;

            }

            if (lunger.LungeComponent.IsLunging)
            {
                lunger.Animator.isRunning = true;
                Lunge(lunger, playerData);
                if (lunger.LungeComponent.CurrentTimeForLunging > lunger.LungeComponent.LungeTime)
                {
                    lunger.LungeComponent.IsLunging = false;
                    lunger.LungeComponent.CurrentTimeForLunging = 0.0f;
                    lunger.AgentComponent.Agent.speed = 7;
                }
                else
                {
                    lunger.LungeComponent.CurrentTimeForLunging += Time.deltaTime;
                }
                continue;
            }

            if (isThereLight && lightData.LightSwitch.LightIsOn)
            {
                if (distanceToLight <= lunger.EnemyVision.Value) //if distance to light is lesser than enemy vision
                {
                    lunger.Animator.isWalking = true;

                     //seek the light
                    Seek(lunger, lightData.LightTransform.position);

                }

                else
                {
                    lunger.Animator.isWalking = true;
                    lunger.Animator.isRunning = false;
                
                    //patrolling
                    lunger.PatrolData.IsWandering = true;
                }

                if (distanceToPlayer <= lunger.LungeComponent.LungeValue && !lunger.LungeComponent.IsPrelunging && !lunger.LungeComponent.IsLunging)
                {
                    //Debug.Log("get ready for the lunge");
                    lunger.LungeComponent.IsPrelunging = true;

                }

            }
            else
            {
                if (distanceToPlayer <= lunger.LungeComponent.LungeValue && !lunger.LungeComponent.IsPrelunging && !lunger.LungeComponent.IsLunging)
                {
                    //Debug.Log("get ready for the lunge");
                    lunger.LungeComponent.IsPrelunging = true;
                }
                else
                {
                    //patrolling
                    //lunger.AgentComponent.Agent.destination = lunger.EnemyTransform.position;   //stay where you are
                    lunger.PatrolData.IsWandering = true;
                }
            }
        }
    }

    void Lunge(LungerData lunger, PlayerData player)
    {
        if (lunger.LungeComponent.IsLunging)
        {
            //Debug.Log("lunging");
            lunger.AgentComponent.Agent.speed = Random.Range(20, 40);
            //seek player
            Seek(lunger, player.PlayerTransform.position);
        }

    }

    void Seek(LungerData lunger, Vector3 target)
    {
        lunger.PatrolData.IsWandering = false;
        lunger.AgentComponent.Agent.speed = 7;
        lunger.AgentComponent.Agent.SetDestination(target);
    }

}
