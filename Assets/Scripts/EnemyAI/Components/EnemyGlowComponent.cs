using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlowComponent : MonoBehaviour
{
    [Header("Glow Configuration")]
    [Range(0F, 2F)]
    public float BaseEmission = 0.1F;

    [Range(0F, 5F)]
    public float MaxEmission = 2F;

    public float AlertSpeed = 1F;
    public float SeekSpeed = 3F;
    public Color BaseColor = Color.red;

    [Header("Instance Data")]
    public Material EnemyMaterial;

    private void Start()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        EnemyMaterial = renderer.material;
    }

}
