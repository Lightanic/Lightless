using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractableComponent {

    public bool IsEquiped = false;

    /// <summary>
    /// Pick up item by disabling the gameobject
    /// </summary>
    public override void Interact()
    {
        Debug.Log("Picking Up");
        gameObject.SetActive(false);
        //Destroy(gameObject);
        this.IsInteracting = false;
    }

    /// <summary>
    /// Drop the item by enabling the item
    /// NOTE : might need to detach from parent
    /// </summary>
    public void Drop()
    {
        gameObject.SetActive(true);
    }


    /// <summary>
    /// Draw radius on scene
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, InteractDistance);
    }
}
