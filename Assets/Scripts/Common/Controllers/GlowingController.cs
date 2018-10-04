using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingController : MonoBehaviour
{
    public enum GlowColorType
    {
        Orange, Green
    };

    private float emission = 0;
    public float SpeedFactor = 0.3F;
    public GlowColorType GlowColor;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Color baseColor;
        if (GlowColor == GlowColorType.Green)
            baseColor = new Color(0.45588235F, 0.59647059F, 0.25588235F, 1);
        else
            baseColor = new Color(0.55588235F, 0.29647059F, 0.25588235F, 1);

        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;


        emission = Mathf.PingPong(Time.time * SpeedFactor, 1.0f);
        // DynamicGI.SetEmissive(renderer, new Color(0.15588235F, 0.19647059F, 0.15588235F, 1) * emission);

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }
}
