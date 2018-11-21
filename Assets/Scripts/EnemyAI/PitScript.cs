using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class PitScript : MonoBehaviour //this script detects enemy and turns their navmesh off
{
<<<<<<< HEAD
    //public GameObject enemy;
    //bool isDead = false;
=======

>>>>>>> Develop

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("Collided");
            //this.gameObject.SetActive(false);
<<<<<<< HEAD
            collision.GetComponent<NavMeshAgent>().enabled = false;
            collision.GetComponent<GameObjectEntity>().enabled = false;
        }

    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.name == "Player")
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
=======
            collision.GetComponent<Rigidbody>().isKinematic = false;
            collision.GetComponent<EnemyDeathComponent>().EnemyIsDead = true;
            collision.GetComponent<NavMeshAgent>().enabled = false;
            collision.GetComponent<GameObjectEntity>().enabled = false;
            
        }

    }

>>>>>>> Develop
}
