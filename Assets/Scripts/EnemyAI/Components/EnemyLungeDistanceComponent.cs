using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLungeDistanceComponent : MonoBehaviour
{

    public float Value;

    public bool IsLunging;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Value);
    }

}
