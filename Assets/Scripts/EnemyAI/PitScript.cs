using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class PitScript : MonoBehaviour //this script detects enemy and turns their navmesh off
{


    private void OnTriggerEnter(Collider collision)
    {
        Physics.IgnoreCollision(collision, GameObject.FindWithTag("Terrain").gameObject.GetComponent<Collider>());
        if (collision.gameObject.tag == "Enemy")
        {
            
            collision.GetComponent<Rigidbody>().isKinematic = false;
            collision.GetComponent<EnemyDeathComponent>().EnemyIsDead = true;
            collision.GetComponent<NavMeshAgent>().enabled = false;
            collision.GetComponent<NavAgentComponent>().enabled = false;
            collision.GetComponent<WayPointComponent>().enabled = false;
            
            
        }

    }

}
