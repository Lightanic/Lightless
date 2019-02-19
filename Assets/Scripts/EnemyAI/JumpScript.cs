using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpScript : MonoBehaviour
{
    public GameObject Enemy;
    public NavMeshAgent Agent;

    private void Start()
    {
       
        Agent = Enemy.GetComponentInChildren<NavMeshAgent>();
    }
    private void Update()
    {
        NavMeshHit hit;
        Agent.FindClosestEdge(out hit);
        //Debug.Log(hit.distance);
        if (hit.distance < 0.01)
        {
            //enemy.Transform.LookAt(player.PlayerTransform);
           // Agent.enabled = false;
           Enemy.transform.GetComponentInChildren<Rigidbody>().AddForce(Vector3.Normalize(Enemy.transform.forward + Enemy.transform.up) * 600);
        }
    }
 
}
