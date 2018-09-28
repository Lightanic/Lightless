using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractableComponent {

    public override void Interact()
    {
        Debug.Log("Picking Up");
        gameObject.SetActive(false);
        this.IsInteracting = false;
    }
}
