using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointComponent : MonoBehaviour
{
    public Transform[] Waypoints;
    public int currentWaypointIndex = 0;
    public bool IsWandering = false;


    private void Start()
    {
        currentWaypointIndex = Random.Range(0, Waypoints.Length);
        
    }
    

}
