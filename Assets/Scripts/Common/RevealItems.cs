using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealItems : MonoBehaviour {

    [Header("Rate of fade when approaching")]
    [SerializeField]
    [Range(1,10)]
    float fadeRate = 6;
    GameObject lampLight;

    [Header("Distance at which item is revealed")]
    [SerializeField]
    [Range(1,100)]
    float range;

    [Header("Toggle for range gizmo")]
    [SerializeField]
    bool showRange = true;

    SpriteRenderer sprite;
    Color white = Color.white;
    LightComponent lc;
	// Use this for initialization
	void Start () {
        lampLight = GameObject.Find("lamp");
        lc = lampLight.GetComponent<LightComponent>();
        if(range == 0)
            range = lampLight.GetComponent<Pickup>().InteractDistance;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        white.a = 0;
        sprite.color = white;
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(transform.position, lampLight.transform.position);
        if ((distance <= range) && lc.LightIsOn)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(showRange)
            Gizmos.DrawWireSphere(transform.position, range);
    }

    IEnumerator FadeOut()
    {
        while (sprite.color.a > 0)
        {
            white.a -= Time.deltaTime / fadeRate;
            sprite.color = white;
            yield return null;
        }
        yield return null;
    }

    IEnumerator FadeIn()
    {
        while (sprite.color.a < 1)
        {
            white.a += Time.deltaTime / fadeRate;
            sprite.color = white;
            yield return null;
        }
        yield return null;
    }
}

