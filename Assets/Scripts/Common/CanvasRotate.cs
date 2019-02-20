using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotate : MonoBehaviour
{
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.LookAt(cam.transform);
    }
}
