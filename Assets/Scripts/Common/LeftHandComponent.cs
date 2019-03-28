using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandComponent : MonoBehaviour {

    public bool isEmpty = true;

    /// <summary>
    /// Update hand status
    /// </summary>
    private void Update()
    {
        lock(this)
        {
            if (gameObject.transform.childCount != 0)
            {
                isEmpty = false;
            }
            else
                isEmpty = true;
        }
        
    }

    /// <summary>
    /// Unequip all items in the hand
    /// </summary>
    public void UnEquipAll()
    {
        var pickUpItems = GetComponentsInChildren<Pickup>();
        foreach (var item in pickUpItems)
        {
            item.IsEquiped = false;
            item.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    /// <summary>
    /// Drop the item held
    /// </summary>
    public void DropItem()
    {
        UnEquipAll();
        transform.DetachChildren();
    }
}
