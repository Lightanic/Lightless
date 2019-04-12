using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPusher : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<NavAgentComponent>().Agent.enabled = false;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
