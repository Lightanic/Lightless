using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LightActivatedPlatformComponent : MonoBehaviour
{
    [Header("Instance Data")]
    public Vector3  StartPosition;
    public bool     IsActivated = false;
    public bool     IsRetracting = false;
    public float    CurrentTime = 0F;
    public bool     HasActivated = false;
    public float    FillValue = 0F;

    [Header("Options")]
    [Tooltip("If 'UseOffset' is enabled, this value is auto populated")]
    public Vector3  ActivatedPosition;
    [Tooltip("If 'UseOffset' is enabled, this value is used.")]
    public Vector3  OffsetPosition;
    public bool     UseOffset = false;
    public bool     IsOneTimeActivation = false;
    public float    ActivationTime = 4F;
    public float    MoveSpeed = 3.0F;
    public string   ID = "";


    private void Start()
    {
        StartPosition = transform.position; //Set start position as initial position of platform or object. 
        if(UseOffset)
        {
            ActivatedPosition = StartPosition + OffsetPosition;
        }
    }
}
