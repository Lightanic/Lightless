using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipComponent : MonoBehaviour {

    public GameObject EquipedItem = null;
    private GameObject prevItem = null;

    /// <summary>
    /// Equip item to be a child of the left hand
    /// </summary>
    public void EquipItem(GameObject item)
    {
        EquipedItem = item;
        item.SetActive(true);
        var itemTransform = item.transform;
        itemTransform.transform.SetParent(gameObject.transform);
        itemTransform.position = gameObject.transform.position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.gameObject.GetComponent<Pickup>().enabled = false;
        itemTransform.gameObject.GetComponent<Pickup>().IsEquiped = true;
        itemTransform.gameObject.GetComponent<Rigidbody>().isKinematic = true;  // disable rigidbody
        item.GetComponent<BoxCollider>().enabled = false;
    }

    public void EquipNewItem(GameObject item)
    {
        Instantiate(item);
        EquipedItem = item;
        item.SetActive(true);
        var itemTransform = item.transform;
        itemTransform.transform.SetParent(gameObject.transform);
        itemTransform.position = gameObject.transform.position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.gameObject.GetComponent<Pickup>().enabled = false;
        itemTransform.gameObject.GetComponent<Rigidbody>().isKinematic = true;  // disable rigidbody
    }
}
