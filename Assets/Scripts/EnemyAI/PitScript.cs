using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;

public class PitScript : MonoBehaviour //this script detects enemy and turns their navmesh off
{
    //public GameObject enemy;
    //bool isDead = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("Collided");
            //this.gameObject.SetActive(false);
            collision.GetComponent<NavMeshAgent>().enabled = false;
            collision.GetComponent<GameObjectEntity>().enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<NavMeshAgent>().enabled == false)
            {
                other.GetComponent<NavMeshAgent>().enabled = true;
                other.GetComponent<GameObjectEntity>().enabled = true;
            }
        }
    }
}
