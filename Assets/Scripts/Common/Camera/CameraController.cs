using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;            // The position that that camera will be following.
    public float smoothing = 5f;        // speed

    public Vector3 offset;
    bool isOffsetSet = false;
    private Outline[] playerOutlines;
    void Start()
    {
        // Calculate the initial offset.
        if (!isOffsetSet)
            offset = transform.position - player.position;
        playerOutlines = player.GetComponentsInChildren<Outline>();
    }

    public void SetOffset(Vector3 offsetVector)
    {
        isOffsetSet = true;
        offset = offsetVector;
    }

    void Update()
    {
        DrawPlayerOutline();
        Vector3 targetCamPos = player.position + offset;
        //transform.position = targetCamPos;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    void DrawPlayerOutline()
    {
        var playerDirection = (player.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, playerDirection);
        RaycastHit info;
        if (Physics.Raycast(ray, out info, 100F))
        {

            if (info.collider.name == "Player")
            {
                foreach (var outline in playerOutlines)
                {
                    outline.enabled = false;
                    outline.eraseRenderer = true;
                }
            }
            else
            {
                foreach (var outline in playerOutlines)
                {
                    outline.eraseRenderer = false;
                    outline.enabled = true;
                }
            }
        }
    }
}
