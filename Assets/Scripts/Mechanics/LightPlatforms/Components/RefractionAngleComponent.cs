using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionAngleComponent : MonoBehaviour
{
    [Range(-89.9F, 89.9F)]
    public float RefractionAngle = 45F;

    [Range(1, 10)]
    public int SplitCount = 2;
}
