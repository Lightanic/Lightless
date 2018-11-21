using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerController : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
<<<<<<< HEAD
            Destroy(other.gameObject);
=======
            other.GetComponent<EnemyDeathComponent>().EnemyIsDead = true;
>>>>>>> Develop
        }
    }
}
