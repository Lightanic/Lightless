using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class planeScript : MonoBehaviour
{
    //public GameObject enemy;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Collided");
            //this.gameObject.SetActive(false);
            collision.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
