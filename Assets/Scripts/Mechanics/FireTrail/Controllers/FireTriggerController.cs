using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerController : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<CapsuleCollider>().enabled = false;
            other.GetComponent<EnemyDeathComponent>().EnemyIsDead = true;
        }
    }
}
