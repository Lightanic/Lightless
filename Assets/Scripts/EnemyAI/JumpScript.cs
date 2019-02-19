using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.GetComponent<NavAgentComponent>().enabled = false;
            other.GetComponent<WayPointComponent>().enabled = false;
            //transform.gameObject.SetActive(false);
        }
    }
}
