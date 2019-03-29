using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDeath : MonoBehaviour
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
        if (other.gameObject.name != "Player")
            Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player")
            Destroy(collision.gameObject);
    }
}
