using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionComponent : MonoBehaviour
{
    public float Value = 3f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Value);
    }
}
