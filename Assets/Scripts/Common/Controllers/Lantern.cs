using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour {

    SphereCollider collider;
    public Transform RightHand;

    public void EquipRightHand()
    {
            var itemTransform = transform;
            itemTransform.transform.SetParent(RightHand);
            itemTransform.position = RightHand.position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.gameObject.GetComponent<Pickup>().enabled = false;
            itemTransform.gameObject.GetComponent<Rigidbody>().isKinematic = true;  // disable rigidbody
    }
}
