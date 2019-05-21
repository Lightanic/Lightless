using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitScriptEnemyDeath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyDeathComponent>().EnemyIsDead = true;
        }
    }
}
