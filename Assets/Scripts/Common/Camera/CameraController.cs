using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;            // The position that that camera will be following.
    public float smoothing = 5f;        // speed

    public Vector3 offset;
    bool isOffsetSet = false;

    void Start()
    {
        // Calculate the initial offset.
        if (!isOffsetSet)
            offset = transform.position - player.position;
    }

    public void SetOffset(Vector3 offsetVector)
    {
        isOffsetSet = true;
        offset = offsetVector;
    }

    void Update()
    {
        Vector3 targetCamPos = player.position + offset;
        //transform.position = targetCamPos;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
