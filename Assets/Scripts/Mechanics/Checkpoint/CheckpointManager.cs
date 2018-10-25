using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointManager : MonoBehaviour
{

    public Transform latestCheckpoint;
    public Transform Player;

    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;

            Debug.Log("Awake: " + gameObject);
        }
    }

    public void SetLatestCheckpoint(Transform checkpoint)
    {
        latestCheckpoint = checkpoint;
    }

    public void GoToLatestCheckpoint()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player").transform;
        }

        Player.position = latestCheckpoint.position;
    }
}
