using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableComponent : MonoBehaviour {

    public bool IsInteractable = true;
    public bool IsInteracting = false;
    public float InteractDistance = 4f;

    /// <summary>
    /// Common Interact function for all interactable objects
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("Interacting");
    }

}
