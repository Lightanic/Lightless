using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerComponent : MonoBehaviour
{

    public float MaxReduction = 0.2F;
    public float MaxIncrease = 0.2F;
    public float RateDamping = 0.1F;
    public float Strength = 300;

    public bool StopFlickering = false;
    public float CurrentTime = 0F;

    public float BaseIntensity = 0F;
    public Light LightSource;
    public float BaseRange;
    public Color BaseColor;
    public Color AlertColor;

    void Start()
    {
        LightSource = GetComponent<Light>();
        BaseIntensity = LightSource.intensity;
        BaseRange = LightSource.range;
        BaseColor = LightSource.color;
    }

}
