using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;            // The position that that camera will be following.
    public float smoothing = 5f;        // speed

    private Vector3 offset;

    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - player.position;
    }

    void Update()
    {
        Vector3 targetCamPos = player.position + offset;
        //transform.position = targetCamPos;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
