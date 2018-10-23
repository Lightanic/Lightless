using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{

    public bool IsDead = false;
    public bool StopEffect = false;
    public Shader DissolveShader;
    public float DeathTime = 2.0F;
    public AnimationCurve fadeIn;
    public GameObject DissolvePrefab;
    ParticleSystem particleEffect;
    Renderer enemyRenderer;
    int shaderProperty;
    float timer = 0F;
    Material dissolveMatProperties;
    bool isMaterialSet = false;
    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        DissolveShader = Shader.Find("Custom/Dissolve");
        particleEffect = GetComponentInChildren<ParticleSystem>();
        enemyRenderer = GetComponent<Renderer>();

        var main = particleEffect.main;
        main.duration = DeathTime;
        var dissolveObject = Instantiate(DissolvePrefab);
        dissolveMatProperties = dissolveObject.GetComponent<Renderer>().material;
        Destroy(dissolveObject);
    }

    void Update()
    {
        if(IsDead && !StopEffect)
        {
            if(!isMaterialSet)
            {
                GetComponent<Renderer>().material.CopyPropertiesFromMaterial(dissolveMatProperties);
            }

            if (timer > DeathTime + 1)
            {
                StopEffect = true;
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
