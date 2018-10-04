using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;

public class NavAgentComponent : MonoBehaviour
{

    public NavMeshAgent Agent;

    private void Start()
    {
        if(Agent == null)
            Agent = GetComponent<NavMeshAgent>();
    }
}
