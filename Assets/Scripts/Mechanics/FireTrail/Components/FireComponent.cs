using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireComponent : MonoBehaviour
{
    public GameObject FirePrefab; // Prefab to instantiate for fire
    public List<GameObject> Instances; // Fire instances
    public float TotalFireTime = 6.0F; //Seconds; The amount of time the fire should last. 
    public float CurrentFireTime = 0.0F; 
    public bool IsFireStopped = true; // Is fire stopped. 
    public float OilTrailDistanceThreshold = 2.0F;

    private void Start()
    {
        Instances = new List<GameObject>();
    }
}
