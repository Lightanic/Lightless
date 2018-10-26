using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointManager : MonoBehaviour
{

    public static string latestCheckpoint;
    public Transform Player;

    private static bool created = false;

    Dictionary<string, Transform> checkpointMap;

    private void Start()
    {
        checkpointMap = new Dictionary<string, Transform>();
        var checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (var checkpoint in checkpoints)
        {
            var name = checkpoint.GetComponent<CheckpointTrigger>().CheckpointName;
            var transform = checkpoint.GetComponent<Transform>();
            if (!checkpointMap.ContainsKey(name))
                checkpointMap.Add(name, transform);
        }

        if(!string.IsNullOrEmpty(latestCheckpoint))
        {
           // GoToLatestCheckpoint();
        }
    }

    void Awake()
    {
        checkpointMap = new Dictionary<string, Transform>();
        var checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (var checkpoint in checkpoints)
        {
            var name = checkpoint.GetComponent<CheckpointTrigger>().CheckpointName;
            var transform = checkpoint.GetComponent<Transform>();
            if (!checkpointMap.ContainsKey(name))
                checkpointMap.Add(name, transform);
        }

        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;

            Debug.Log("Awake: " + gameObject);
        }
    }

    public void SetLatestCheckpoint(string checkpoint)
    {
        latestCheckpoint = checkpoint;
    }

    public void GoToLatestCheckpoint()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player").transform;
        }

        Player.position = checkpointMap[latestCheckpoint].position;
    }
}
