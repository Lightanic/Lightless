using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipComponent : MonoBehaviour {

    public GameObject EquipedItem = null;
    private GameObject prevItem = null;

    /// <summary>
    /// Equip item only if it changes from the previous item
    /// </summary>
    private void Update()
    {
        if (EquipedItem != null && prevItem != EquipedItem)
            EquipItem();
        prevItem = EquipedItem;
    }

    void EquipItem()
    {
        var itemTransform = EquipedItem.transform;
        itemTransform.transform.SetParent(gameObject.transform);
        itemTransform.position = gameObject.transform.position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.gameObject.GetComponent<Pickup>().enabled = false;
    }
}
