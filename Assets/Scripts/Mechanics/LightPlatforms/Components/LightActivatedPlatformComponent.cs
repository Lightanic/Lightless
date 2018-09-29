using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LightActivatedPlatformComponent : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 ActivatedPosition;
    public bool IsActivated = false;
    public float ActivationTime = 4F;
    public float CurrentTime = 0F;
    public float MoveSpeed = 3.0F;

    private void Start()
    {
        StartPosition = transform.position; //Set start position as initial position of platform or object. 
    }
}
