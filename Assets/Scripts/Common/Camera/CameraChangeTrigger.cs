using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CameraData
{
    public Vector3 Offset;
    public Vector3 Rotation;
}

public class CameraChangeTrigger : MonoBehaviour
{
    public CameraData CameraOptions;
    private CameraController MainCamera;
    public bool IsChanging;
    private Vector3 currentOffset;
    private Vector3 currentRotation;
    private CameraChangeTrigger[] triggers;

    // Start is called before the first frame update
    void Start()
    {
        triggers = FindObjectsOfType<CameraChangeTrigger>();
        IsChanging = false;
        MainCamera = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsChanging)
        {
            currentOffset = Vector3.Lerp(MainCamera.offset, CameraOptions.Offset, Time.deltaTime);
            currentRotation = Vector3.Lerp(MainCamera.transform.rotation.eulerAngles, CameraOptions.Rotation, Time.deltaTime);
            MainCamera.SetOffset(currentOffset);
            MainCamera.SetRotation(currentRotation);

            if (Vector3.Distance(currentOffset, CameraOptions.Offset) < 0.1F && Vector3.Distance(currentRotation, CameraOptions.Rotation) < 0.1F)
            {
                IsChanging = false;
            }
        }
    }

    void StopOtherTriggers()
    {
        foreach (var trigger in FindObjectsOfType<CameraChangeTrigger>())
        {
            trigger.IsChanging = false;
        }
    }

    void StopOtherTriggers()
    {
        foreach (var trigger in triggers)
        {
            trigger.IsChanging = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StopOtherTriggers();
            IsChanging = true;
        }
    }
}
