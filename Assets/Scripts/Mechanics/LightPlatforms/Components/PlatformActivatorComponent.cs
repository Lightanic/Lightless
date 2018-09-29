using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivatorComponent : MonoBehaviour {

    public float ActivationDelay = 1F;
    public float MaxActivationDistance = 15F;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward);
        RaycastHit info;
        if(Physics.Raycast(ray, out info, 10F))
        {
            Debug.Log(info.collider.name);
        }
	}
}
