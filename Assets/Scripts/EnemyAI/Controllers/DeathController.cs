using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{

    public bool IsDead = false;
    public Shader DissolveShader;
    public float DeathTime = 2.0F;
    public AnimationCurve fadeIn;

    ParticleSystem particleEffect;
    Renderer enemyRenderer;
    int shaderProperty;
    float timer = 0F;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        DissolveShader = Shader.Find("Custom/Dissolve");
        particleEffect = GetComponentInChildren<ParticleSystem>();
        enemyRenderer = GetComponent<Renderer>();

        var main = particleEffect.main;
        main.duration = DeathTime;
    }

    void Update()
    {
        if(IsDead)
        {
            if (timer > DeathTime + 1)
            {
                Destroy(gameObject);
            }

            if (!particleEffect.isPlaying)
            {
                particleEffect.Play();
            }

            enemyRenderer.material.shader = DissolveShader;
            enemyRenderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, DeathTime, timer)));
            timer += Time.deltaTime;
        }
    }
}
