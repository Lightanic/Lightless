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
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("Collided");
            //this.gameObject.SetActive(false);
            collision.GetComponent<Rigidbody>().isKinematic = false;
            collision.GetComponent<EnemyDeathComponent>().EnemyIsDead = true;
            collision.GetComponent<NavMeshAgent>().enabled = false;
            //collision.GetComponent<GameObjectEntity>().enabled = false;
            
        }

    }

}
