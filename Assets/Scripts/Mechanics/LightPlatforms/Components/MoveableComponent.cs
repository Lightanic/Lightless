using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableComponent : MonoBehaviour
{
    [Header("Instance Data")]
    public Vector3 PointA;
    public Vector3 InitialAngles;
    public Vector3 CurrentAngles;
    public bool IsSelected;

    [Header("Configuration")]
    public Vector3 PointB;
    public float MoveSpeed = 5F;
    public float RotateSpeed = 120F;
    public float MaxRotationAngle = 45F;
    public Transform Activator;
    public bool CanMove = true;
    public bool CanRotate = true;

    [Header("Rotation Axis")]
    public bool XAxis;
    public bool YAxis;
    public bool ZAxis;

    private void Start()
    {
        PointA = transform.position;
        InitialAngles = transform.rotation.eulerAngles;
        CurrentAngles = InitialAngles;
    }
}
