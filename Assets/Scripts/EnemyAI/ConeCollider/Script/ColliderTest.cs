using UnityEngine;
using System.Collections;

public class ColliderTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("CollisionEnter \"" + other.gameObject.name + " \"Object");
    }

    void OnCollisionStay(Collision other)
    {
        Debug.Log("CollisionStay \"" + other.gameObject.name + " \"Object");
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("CollisionExit \"" + other.gameObject.name + " \"Object");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter \"" + other.gameObject.name + " \"Object");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("TriggerStay \"" + other.gameObject.name + " \"Object");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("TriggerExit \"" + other.gameObject.name + " \"Object");
    }
}
