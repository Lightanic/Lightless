using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class PitScript : MonoBehaviour //this script detects enemy and turns their navmesh off
{
    //public GameObject enemy;
    //bool isDead = false;

    public CheckpointManager CheckpointManager;

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
            CheckpointManager.GoToLatestCheckpoint();
           //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
