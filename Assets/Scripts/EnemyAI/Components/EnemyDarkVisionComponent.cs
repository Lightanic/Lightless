using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDarkVisionComponent : MonoBehaviour
{
    public float Value = 3f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, Value);
    }

}
