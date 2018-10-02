using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLungeComponent : MonoBehaviour
{

    public float LungeValue;
    public float PrelungeValue;

    public bool IsLunging;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, LungeValue);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, PrelungeValue);
    }

}
