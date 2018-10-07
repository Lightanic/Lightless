using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour {

    Light menuLight;
    public float minWaitTime;
    public float maxWaitTime;
	// Use this for initialization
	void Start () {
        menuLight = GetComponent<Light>();
        StartCoroutine(Flicker());
	}
	
    IEnumerator Flicker()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            menuLight.enabled = !menuLight.enabled;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
