using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using static EnemyComponent;

public class PatrolSystem : ComponentSystem
{
    private struct EnemyPatrolData
    {
        public NavAgentComponent AgentComponent;
        public Transform EnemyTransform;
        public WayPointComponent PatrolComponent;
        public EnemyComponent EnemyComponent;
        

    }

    protected override void OnUpdate()
    {
        var entities = GetEntities<EnemyPatrolData>();
        foreach (var enemy in entities)
        {
            if (enemy.EnemyTransform.GetComponent<EnemyDeathComponent>().EnemyIsDead)
            {
                continue;
            }

            if (enemy.EnemyComponent.State == EnemyState.Patrol || enemy.EnemyComponent.State == EnemyState.Alert)
            {
                if (enemy.PatrolComponent.Waypoints.Length == 0)
                {
                    continue; //if waypoints are not setup, enemies can just stand around
                }
                else
                {

                    var index = enemy.PatrolComponent.currentWaypointIndex;
                    if (enemy.PatrolComponent.Waypoints[index] == null) continue;
                    var pos = enemy.PatrolComponent.Waypoints[index].position;
                    if (enemy.AgentComponent.Agent.enabled)
                        enemy.AgentComponent.Agent.SetDestination(pos);
                    enemy.AgentComponent.Agent.speed = enemy.PatrolComponent.PatrolSpeed;
                    if (Vector3.Distance(enemy.EnemyTransform.position, enemy.PatrolComponent.Waypoints[enemy.PatrolComponent.currentWaypointIndex].position) <= 2)
                    {
                        enemy.PatrolComponent.currentWaypointIndex = Random.Range(0, enemy.PatrolComponent.Waypoints.Length);
                        enemy.AgentComponent.Agent.SetDestination(enemy.PatrolComponent.Waypoints[enemy.PatrolComponent.currentWaypointIndex].position);
                    }
                }

            }

        }


    }


}
