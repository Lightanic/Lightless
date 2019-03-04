using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerComponent : MonoBehaviour
{
    [Header("Configuration")]
    public float MaxReduction = 0.2F;
    public float MaxIncrease = 0.2F;
    public float RateDamping = 0.1F;
    public float Strength = 300;
    public bool StopFlickering = false;

    [Header("Alert Configuration")]
    public Color AlertColor;
    public float AlertRadius = 30F;
    public float MinAlertRangeReduction = 5F;
    public float MaxAlertRangeReduction = 2F;
    public float AlertIntensityMultiplier = 1.5F;

    [Header("Instance Auto")]
    public float BaseRange;
    public Color BaseColor;
    public float BaseIntensity = 0F;
    public Light LightSource;
    public float CurrentTime = 0F;

    void Start()
    {
        LightSource = GetComponent<Light>();
        BaseIntensity = LightSource.intensity;
        BaseRange = LightSource.range;
        BaseColor = LightSource.color;
    }

}
