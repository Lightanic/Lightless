using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class PitScriptForPlayer : MonoBehaviour //this script is for player if they fall off the level
{
    GameObject gameManager;
    public CheckpointManager CheckpointManager;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            gameManager.GetComponent<GameManager>().StartDeath();
        }
    }
}
