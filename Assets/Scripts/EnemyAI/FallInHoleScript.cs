using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallInHoleScript : MonoBehaviour
{
    private GameObject g;
	// Use this for initialization
	void Start ()
    {
        g = this.gameObject;
	}

    private void Awake()
    {
        //Physics.IgnoreLayerCollision(0, 9, false);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Pit")
        {
            g.gameObject.GetComponent<BoxCollider>().enabled = false;
            if (g.gameObject.GetComponent<BoxCollider>() == null)
            {
                g.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
            }
            
            
        }
    }

}
