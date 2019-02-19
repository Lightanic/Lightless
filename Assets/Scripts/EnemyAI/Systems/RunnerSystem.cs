//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.Entities;
//public class RunnerSystem : ComponentSystem
//{
//    private struct RunnerData
//    {
//        public NavAgentComponent AgentComponent;
//        public Transform EnemyTransform;
//        public EnemyVisionComponent EnemyVision;
//        public EnemyDarkVisionComponent NightVision;
//        public EnemyAnimator Animator;
//        public WayPointComponent PatrolData;

//    }
//    private struct RunnerDeath
//    {
//        public EnemyDeathComponent DeathComponent;
//        public Transform EnemyTransform;
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


//        foreach (var entity in GetEntities<PlayerData>())
//        {
//            playerData = entity;

//        }

//        foreach (var entity in GetEntities<RunnerDeath>())
//        {
//            if (entity.DeathComponent.EnemyIsDead)
//            {
//                entity.EnemyTransform.GetComponent<BoxCollider>().enabled = false;
//            }
//        }


//        foreach (var runner in GetEntities<RunnerData>())
//        {

//            if (runner.EnemyTransform.GetComponent<EnemyDeathComponent>().EnemyIsDead)
//            {
//                continue;
//            }
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
//            runner.EnemyVision.IsAlerted = false;
//            runner.EnemyVision.IsSeeking = false;

//            if (isThereLight && lightData.LightSwitch.LightIsOn)
//            {

//                if (distanceToLight > runner.EnemyVision.Value && distanceToLight <= runner.EnemyVision.Value + runner.EnemyVision.AlertValue)
//                {
//                    runner.Animator.isRunning = false;
//                    runner.Animator.isWalking = true;
//                    runner.EnemyVision.IsAlerted = true;
//                    runner.PatrolData.IsWandering = true;
//                }
//                else if (distanceToLight <= runner.EnemyVision.Value) //if distance to light is lesser than enemy vision
//                {
//                    runner.Animator.isRunning = true;
//                    runner.Animator.isWalking = false;

//                    //seek the light
//                    Seek(runner, lightData.LightTransform);

//                }
//                else
//                {
//                    runner.Animator.isRunning = false;
//                    runner.Animator.isWalking = true;

//                    //patrolling
//                    runner.PatrolData.IsWandering = true;
//                }

//                if (distanceToPlayer <= runner.NightVision.Value)
//                {
//                    runner.Animator.isRunning = true;
//                    //seek player
//                    Seek(runner, playerData.PlayerTransform);
//                }
//            }
//            else
//            {
//                if (distanceToLight > runner.NightVision.Value && distanceToLight <= runner.NightVision.Value + runner.EnemyVision.AlertValue)
//                {
//                    runner.Animator.isRunning = false;
//                    runner.Animator.isWalking = true;
//                    runner.EnemyVision.IsAlerted = true;
//                    runner.PatrolData.IsWandering = true;
//                }
//                else if (distanceToPlayer <= runner.NightVision.Value)
//                {
//                    runner.Animator.isRunning = true;
//                    runner.Animator.isWalking = false;
//                    //seek player
//                    Seek(runner, playerData.PlayerTransform);
//                }
//                else
//                {
//                    runner.Animator.isRunning = false;
//                    //patrolling
//                    runner.PatrolData.IsWandering = true;
//                    runner.Animator.isWalking = true;
//                }
//            }
//        }


//    }

//    void Seek(RunnerData runner, Transform target)
//    {

//        runner.EnemyVision.IsSeeking = true;
//        runner.PatrolData.IsWandering = false;
//        runner.AgentComponent.Agent.speed = 12;
//        if (target.gameObject.tag == "Flashlight")
//        {
//            if (runner.AgentComponent.Agent.enabled)
//                runner.AgentComponent.Agent.SetDestination(target.position + target.forward * 8);
//        }
//        else
//        {
//            if (runner.AgentComponent.Agent.enabled)
//                runner.AgentComponent.Agent.SetDestination(target.position);
//        }
//    }

//}
