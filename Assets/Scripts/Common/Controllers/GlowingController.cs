using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingController : MonoBehaviour
{
    private float emission = 0;
    public float SpeedFactor = 0.3F;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;


        emission = Mathf.PingPong(Time.time * SpeedFactor, 1.0f);
       // DynamicGI.SetEmissive(renderer, new Color(0.15588235F, 0.19647059F, 0.15588235F, 1) * emission);
        Color baseColor = new Color(0.15588235F, 0.19647059F, 0.15588235F, 1);
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

         mat.SetColor("_EmissionColor", finalColor);
    }
}
