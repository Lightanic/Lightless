using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionAngleComponent : MonoBehaviour
{
    [Range(-89.9F, 89.9F)]
    public float RefractionAngle = 45F;

    [Range(1F, 10F)]
    public int SplitCount = 2;

    [Range(0F,180F)]
    public float SplitAngleRange = 90F;
}
