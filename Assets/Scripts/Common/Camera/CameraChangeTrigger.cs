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
            var Rot = Quaternion.Euler(CameraOptions.Rotation);
            MainCamera.transform.rotation = Quaternion.Slerp(MainCamera.transform.rotation, Rot, Time.deltaTime);
            currentRotation = MainCamera.transform.rotation.eulerAngles;
            MainCamera.SetOffset(currentOffset);

            var toRotation = ChangeRotationForCheck(CameraOptions.Rotation);
            if (Vector3.Distance(currentOffset, CameraOptions.Offset) < 0.1F && Vector3.Distance(currentRotation, toRotation) < 0.1F)
            {
                IsChanging = false;
            }
        }
    }

    Vector3 ChangeRotationForCheck(Vector3 Rotation)
    {
        if (Rotation.x < 0) Rotation.x = 360F + Rotation.x;
        if (Rotation.y < 0) Rotation.y = 360F + Rotation.y;
        if (Rotation.z < 0) Rotation.z = 360F + Rotation.z;

        return Rotation;
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
