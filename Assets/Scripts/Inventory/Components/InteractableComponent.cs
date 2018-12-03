using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableComponent : MonoBehaviour {

    public bool IsInteractable = true;
    public bool IsInteracting = false;
    [Range(1,10)]
    public float InteractDistance = 4f;

     
    /// <summary>
    /// Draw radius on scene
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, InteractDistance);
    }
}
