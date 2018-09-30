using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandComponent : MonoBehaviour {

    public bool isEmpty = true;

    private void Update()
    {
        if(gameObject.transform.childCount != 0)
        {
            isEmpty = false;
        }
        else
            isEmpty = true;
    }

    public void UnEquipAll()
    {
        var pickUpItems = GetComponentsInChildren<Pickup>();
        foreach (var item in pickUpItems)
        {
            item.IsEquiped = false;
        }
    }

    public void DropItem()
    {
        UnEquipAll();
        transform.DetachChildren();
    }
}
