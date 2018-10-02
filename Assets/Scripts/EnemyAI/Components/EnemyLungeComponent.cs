using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLungeComponent : MonoBehaviour
{

    public float LungeValue = 5.0f;
    //public float PrelungeValue = 5.0f;
    public float PrelungeTime = 2.0f;
    public float CurrentTime = 0.0f;

    public bool IsLunging;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, LungeValue);
        //Gizmos.color = Color.grey;
        //Gizmos.DrawWireSphere(transform.position, PrelungeValue);
    }

}
