using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewConeComponent : MonoBehaviour
{
    [Range(0, 40)]
    public float ViewRadius;
    [Range(0, 360)]
    public float ViewAngle;
    public LayerMask TargetMask;
    public LayerMask ObstacleMask;
    public Vector3 EyeOffset;

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
