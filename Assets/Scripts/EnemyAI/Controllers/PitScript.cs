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
        var terrains = GameObject.FindGameObjectsWithTag("Terrain");
        foreach (var terrain in terrains)
        {
            Physics.IgnoreCollision(collision, terrain.gameObject.GetComponent<Collider>());
        }
       
        if (collision.gameObject.tag == "Enemy")
        {
            
            collision.GetComponent<Rigidbody>().isKinematic = false;
            collision.GetComponent<EnemyDeathComponent>().EnemyIsDead = true;
            collision.GetComponent<NavMeshAgent>().enabled = false;
            collision.GetComponent<NavAgentComponent>().enabled = false;
            collision.GetComponent<WayPointComponent>().enabled = false;

            AkSoundEngine.PostEvent("Play_BlueMonster_Death", collision.gameObject);
        }

    }

}
