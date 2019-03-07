using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointComponent : MonoBehaviour
{
    public Transform[] Waypoints;
    public int currentWaypointIndex = 0;
    public bool IsWandering = false;
    public float PatrolSpeed = 3;

    private void Start()
    {
        currentWaypointIndex = 0;
        
    }

}
