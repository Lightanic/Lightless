using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTarget : MonoBehaviour {

    public Vector3 targetPos;
    public float throwDistance = 8f;

    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(transform.parent.forward);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            targetPos = hit.point;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit " + targetPos);
        }
    }
}
