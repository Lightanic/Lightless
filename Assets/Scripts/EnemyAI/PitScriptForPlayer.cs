using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class PitScriptForPlayer : MonoBehaviour //this script detects enemy and turns their navmesh off
{
    //public GameObject enemy;
    //bool isDead = false;

    public CheckpointManager CheckpointManager;

 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            //CheckpointManager.GoToLatestCheckpoint();
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
