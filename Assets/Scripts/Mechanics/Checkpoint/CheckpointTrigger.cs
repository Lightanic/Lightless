using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public string CheckpointName = "Default1";
    public CheckpointManager manager;
    bool HasTriggeredCheckpoint = false;
    public CheckpointLogo CheckpointLogoLoader;
    private GameObject LoaderObject;

    private void Start()
    {
        if (!CheckpointLogoLoader)
        {
            LoaderObject = GameObject.Find("CheckpointLoader");
            if (LoaderObject)
                CheckpointLogoLoader = LoaderObject.GetComponent<CheckpointLogo>();
        }
        manager = GetComponentInParent<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasTriggeredCheckpoint && other.name == "Player")
        {
            HasTriggeredCheckpoint = true;
            manager.SetLatestCheckpoint(CheckpointName);
            if (CheckpointLogoLoader)
                CheckpointLogoLoader.TriggerCheckpointLogo();
        }
    }
}
