using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireComponent : MonoBehaviour
{
    public GameObject FirePrefab;
    public List<GameObject> Instances;
    public float TotalFireTime = 6.0F; //Seconds;
    public float CurrentFireTime = 0.0F;
    public bool IsFireStopped = true;

    private void Start()
    {
        Instances = new List<GameObject>();
    }
}
