<<<<<<< HEAD
﻿using System.Collections;
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


}
=======
﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.Entities;

//public class EnemyAISystem : ComponentSystem
//{

//    private struct RunnerData
//    {
//        public NavAgentComponent AgentComponent;
//        public Transform EnemyTransform;
//        public EnemyVisionComponent EnemyVision;
//        public EnemyDarkVisionComponent NightVision;
//        public EnemyAnimator Animator;

//    }

//    private struct LungerData
//    {
//        public NavAgentComponent AgentComponent;
//        public Transform EnemyTransform;
//        public EnemyVisionComponent EnemyVision;
//        public EnemyLungeComponent LungeComponent;
//        public EnemyAnimator Animator;

//    }

//    private struct StunnerData
//    {
//        public NavAgentComponent AgentComponent;
//        public Transform EnemyTransform;
//        public EnemyVisionComponent EnemyVision;
//        public EnemyDarkVisionComponent NightVision;
//        public EnemyStunComponent StunComponent;
//    }

//    private struct PlayerData
//    {
//        public InputComponent PlayerInput;
//        public Transform PlayerTransform;
//    }
//    private struct LightData
//    {
//        public LightComponent LightSwitch;
//        public Transform LightTransform;
//    }


//    protected override void OnUpdate()
//    {
//        PlayerData playerData = new PlayerData();
//        LightData lightData = new LightData();
//        bool isThereLight = false;


        //foreach (var entity in GetEntities<PlayerData>())
        //{
        //    playerData = entity;

        //}



//        foreach (var runner in GetEntities<RunnerData>())
//        {
//            float currentDistance = float.MaxValue;
//            float lightDistance;
//            foreach (var e in GetEntities<LightData>())
//            {
//                //lightData = e;

//                if (e.LightSwitch.LightIsOn)
//                {
//                    lightDistance = Vector3.Distance(e.LightTransform.position, runner.EnemyTransform.position);
//                    if (lightDistance < currentDistance)
//                    {
//                        currentDistance = lightDistance;
//                        lightData = e;
//                        isThereLight = true;
//                    }
//                }
//            }

//            float distanceToLight = currentDistance;
//            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, runner.EnemyTransform.position);

//            if (isThereLight && lightData.LightSwitch.LightIsOn)
//            {
//                if (distanceToLight <= runner.EnemyVision.Value) //if distance to light is lesser than enemy vision
//                {
//                    runner.Animator.isRunning = true;
//                    runner.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light

//                }
//                else
//                {
//                    runner.Animator.isRunning = false;
//                    runner.AgentComponent.Agent.destination = runner.EnemyTransform.position; //stay where you are
//                }

//                if (distanceToPlayer <= runner.NightVision.Value)
//                {
//                    runner.Animator.isRunning = true;
//                    runner.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
//                }
//            }
//            else
//            {
//                if (distanceToPlayer <= runner.NightVision.Value)
//                {
//                    runner.Animator.isRunning = true;
//                    runner.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
//                }
//                else
//                {
//                    runner.Animator.isRunning = false;
//                    runner.AgentComponent.Agent.destination = runner.EnemyTransform.position;   //stay where you are
//                }
//            }
//        }

//        foreach (var lunger in GetEntities<LungerData>())
//        {
//            float currentDistance = float.MaxValue;
//            float lightDistance;
//            foreach (var e in GetEntities<LightData>())
//            {
//                //lightData = e;

//                if (e.LightSwitch.LightIsOn)
//                {
//                    lightDistance = Vector3.Distance(e.LightTransform.position, lunger.EnemyTransform.position);
//                    if (lightDistance < currentDistance)
//                    {
//                        currentDistance = lightDistance;
//                        lightData = e;
//                        isThereLight = true;
//                    }
//                }
//            }

//            float distanceToLight = currentDistance;

//            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, lunger.EnemyTransform.position);

//            if (lunger.LungeComponent.IsPrelunging)
//            {
//                lunger.Animator.isWalking = false;
//                lunger.Animator.isRunning = false;
//                lunger.AgentComponent.Agent.speed = 0.0f;
//                if (lunger.LungeComponent.CurrentTimeForPrelunging > lunger.LungeComponent.PrelungeTime)
//                {
//                    lunger.LungeComponent.IsPrelunging = false;
//                    lunger.LungeComponent.IsLunging = true;
//                    lunger.LungeComponent.CurrentTimeForPrelunging = 0.0f;
//                }
//                else
//                {

//                    lunger.LungeComponent.CurrentTimeForPrelunging += Time.deltaTime;
//                }
//                continue;

//            }

//            if (lunger.LungeComponent.IsLunging)
//            {
//                lunger.Animator.isRunning = true;
//                Lunge(lunger, playerData);
//                if (lunger.LungeComponent.CurrentTimeForLunging > lunger.LungeComponent.LungeTime)
//                {
//                    lunger.LungeComponent.IsLunging = false;
//                    lunger.LungeComponent.CurrentTimeForLunging = 0.0f;
//                    lunger.AgentComponent.Agent.speed = 7;
//                }
//                else
//                {
//                    lunger.LungeComponent.CurrentTimeForLunging += Time.deltaTime;
//                }
//                continue;
//            }

//            if (isThereLight && lightData.LightSwitch.LightIsOn)
//            {
//                if (distanceToLight <= lunger.EnemyVision.Value) //if distance to light is lesser than enemy vision
//                {
//                    lunger.Animator.isWalking = true;

//                    lunger.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light

//                }

//                else
//                {
//                    lunger.Animator.isWalking = false;
//                    lunger.Animator.isRunning = false;
//                    lunger.AgentComponent.Agent.destination = lunger.EnemyTransform.position; //stay where you are
//                }

//                if (distanceToPlayer <= lunger.LungeComponent.LungeValue && !lunger.LungeComponent.IsPrelunging && !lunger.LungeComponent.IsLunging)
//                {
//                    //Debug.Log("get ready for the lunge");
//                    lunger.LungeComponent.IsPrelunging = true;

//                }

//            }
//            else
//            {
//                if (distanceToPlayer <= lunger.LungeComponent.LungeValue && !lunger.LungeComponent.IsPrelunging && !lunger.LungeComponent.IsLunging)
//                {
//                    //Debug.Log("get ready for the lunge");
//                    lunger.LungeComponent.IsPrelunging = true;
//                }
//                else
//                {
//                    lunger.AgentComponent.Agent.destination = lunger.EnemyTransform.position;   //stay where you are
//                }
//            }
//        }

//        foreach (var stunner in GetEntities<StunnerData>())
//        {
//            float currentDistance = float.MaxValue;
//            float lightDistance;
//            foreach (var e in GetEntities<LightData>())
//            {
//                //lightData = e;

//                if (e.LightSwitch.LightIsOn)
//                {
//                    lightDistance = Vector3.Distance(e.LightTransform.position, stunner.EnemyTransform.position);
//                    if (lightDistance < currentDistance)
//                    {
//                        currentDistance = lightDistance;
//                        lightData = e;
//                        isThereLight = true;
//                    }
//                }
//            }

//            float distanceToLight = currentDistance;
//            float distanceToPlayer = Vector3.Distance(playerData.PlayerTransform.position, stunner.EnemyTransform.position);

//            if (isThereLight && lightData.LightSwitch.LightIsOn)
//            {
//                if (distanceToLight <= stunner.EnemyVision.Value) //if distance to light is lesser than enemy vision
//                {
//                    //stunner.Animator.isRunning = true;
//                    stunner.AgentComponent.Agent.SetDestination(lightData.LightTransform.position); //seek the light

//                }
//                else
//                {
//                    //runner.Animator.isRunning = false;
//                    stunner.AgentComponent.Agent.destination = stunner.EnemyTransform.position; //stay where you are
//                }

//                if (distanceToPlayer <= stunner.NightVision.Value)
//                {
//                    //enemyEntity.Animator.isRunning = true;
//                    stunner.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
//                }
//            }
//            else
//            {
//                if (distanceToPlayer <= stunner.NightVision.Value)
//                {
//                    //stunner.Animator.isRunning = true;
//                    stunner.AgentComponent.Agent.SetDestination(playerData.PlayerTransform.position); //seek player
//                }
//                else
//                {
//                    //stunner.Animator.isRunning = false;
//                    stunner.AgentComponent.Agent.destination = stunner.EnemyTransform.position;   //stay where you are
//                }
//            }
//            if (stunner.StunComponent.IsStunned == true)
//            {
//                stunner.AgentComponent.Agent.speed = 0;
//            }
//            else if (stunner.StunComponent.IsStunned == false)
//            {
//                stunner.AgentComponent.Agent.speed = 7;
//            }
//        }
//    }

//    void Lunge(LungerData lunger, PlayerData player)
//    {
//        if (lunger.LungeComponent.IsLunging)
//        {
//            //Debug.Log("lunging");
//            lunger.AgentComponent.Agent.speed = Random.Range(20, 40);
//            //Debug.Log(lunger.AgentComponent.Agent.speed);
//            lunger.AgentComponent.Agent.SetDestination(player.PlayerTransform.position); //seek player
//        }

//    }


//    void Wander()
//    {
//        //Waypoint navigation
//    }

//    void Stun()
//    { }


//}
>>>>>>> Develop
