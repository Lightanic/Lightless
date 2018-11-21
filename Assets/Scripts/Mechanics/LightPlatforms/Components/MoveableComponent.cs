using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableComponent : MonoBehaviour
{
    public Vector3 PointA;
    public Vector3 PointB;
    public bool IsSelected;
    public float MoveSpeed = 5F;
    public float RotateSpeed = 120F;
    public float MaxRotationAngle = 45F;
    public Transform Activator;
    public bool CanMove = true;
    public bool CanRotate = true;

    public Vector3 InitialAngles;
    public Vector3 CurrentAngles;


    private void Start()
    {
        PointA = transform.position;
        InitialAngles = transform.rotation.eulerAngles;
        CurrentAngles = InitialAngles;
    }
}
