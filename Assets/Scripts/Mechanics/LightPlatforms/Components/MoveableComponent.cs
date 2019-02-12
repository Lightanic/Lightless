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
    public bool IsSpotlight = false;

    [Header("Rotation Axis")]
    public bool XAxis;
    public bool YAxis;
    public bool ZAxis;

    [Header("Constraints")]
    public bool UseConstraints = false;
    public Vector3 MaxRotationConstraints = new Vector3(0, 45, 0);
    public Vector3 MinRotationConstraints = new Vector3(-45F, -45F, 0F);

    private void Start()
    {
        PointA = transform.position;
        InitialAngles = transform.rotation.eulerAngles;
        CurrentAngles = InitialAngles;

        if (IsSpotlight)
        {
            XAxis = true;
            YAxis = true;
            CanRotate = true;
        }
    }
}
