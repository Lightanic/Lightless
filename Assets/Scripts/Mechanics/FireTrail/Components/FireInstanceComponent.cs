using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireInstanceComponent : MonoBehaviour
{
    public float TotalFireTime = 6.0F; //Seconds; The amount of time the fire should last. 
    public float CurrentFireTime = 0.0F;
    public bool DestroyNextUpdate = false;

    private void Update()
    {
        if (DestroyNextUpdate)
        {
            Destroy(gameObject);
        }
    }
}
