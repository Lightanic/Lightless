using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public string CheckpointName = "Default1";
    public CheckpointManager manager;
    bool HasTriggeredCheckpoint = false;

    private void Start()
    {
        manager = GetComponentInParent<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasTriggeredCheckpoint && other.name == "Player")
        {
            HasTriggeredCheckpoint = true;
            manager.SetLatestCheckpoint(CheckpointName);
        }
    }
}
