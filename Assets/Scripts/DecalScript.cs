using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

   void Awake()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3F))
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                transform.position = hit.point - transform.forward * 0.01F;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
