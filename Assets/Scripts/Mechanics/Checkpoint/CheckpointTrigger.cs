﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public CheckpointManager manager;
    bool HasTriggeredCheckpoint = false;

    private void Start()
    {
        manager = GetComponentInParent<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasTriggeredCheckpoint)
        {
            HasTriggeredCheckpoint = true;
            manager.SetLatestCheckpoint(transform);
        }
    }
}
