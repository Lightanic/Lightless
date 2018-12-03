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

        AkSoundEngine.PostEvent("BackgroundStart", gameObject);
    }

    public void SetOffset(Vector3 offsetVector)
    {
        isOffsetSet = true;
        offset = offsetVector;
    }

    void Update()
    {
        DrawPlayerOutline();
        DrawEnemyOutlines();
        Vector3 targetCamPos = player.position + offset;
        //transform.position = targetCamPos;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    void DrawEnemyOutlines()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            var outline = enemy.GetComponentInChildren<Outline>();
            if (outline != null)
            {
                var enemyDirection = (enemy.transform.position - transform.position).normalized;
                Ray ray = new Ray(transform.position, enemyDirection);
                RaycastHit info;
                if (Physics.Raycast(ray, out info, 100F))
                {
                    if (info.collider.tag == "Enemy")
                    {
                        outline.enabled = false;
                        outline.eraseRenderer = true;
                    }
                    else
                    {

                        outline.eraseRenderer = false;
                        outline.enabled = true;
                    }
                }
            }
        }
    }

    void DrawPlayerOutline()
    {
        var playerDirection = (player.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, playerDirection);
        RaycastHit info;
        if (Physics.Raycast(ray, out info, 100F))
        {
            if (info.collider.tag == "PlayerBodyMesh")
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

    private void OnDestroy()
    {
        AkSoundEngine.PostEvent("BackgroundStop", gameObject);
    }
}
