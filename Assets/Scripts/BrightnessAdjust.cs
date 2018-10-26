using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class BrightnessAdjust : MonoBehaviour {

    [Range(0,1)]
    float rbgValue = 0.0f;
    Slider brightnessSlider;

    public float MaxValue = 0.1f;
    public float MinValue = 0.0f;

    private void Start()
    {
        brightnessSlider = GetComponent<Slider>();
        brightnessSlider.minValue = MinValue;
        brightnessSlider.maxValue = MaxValue;
        rbgValue = brightnessSlider.value;
    }

    void Update()
    {
        rbgValue = brightnessSlider.value;
        Mathf.Clamp(rbgValue, MinValue, MaxValue);
        RenderSettings.ambientLight = new Color(rbgValue, rbgValue, rbgValue, 1);
    }

}
