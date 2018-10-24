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

    private void Start()
    {
        PointA = transform.position;
    }
}
