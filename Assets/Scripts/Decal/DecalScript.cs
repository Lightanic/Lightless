using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalScript : MonoBehaviour
{
    public float TotalTimeAlive = 2F;
    public float CurrentTime = 0F;
    private float AliveCounter = 0F;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        CurrentTime = 0F;
        Reposition();
    }

    void Awake()
    {

    }

    void Reposition()
    {
        AliveCounter = 0F;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3F))
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                transform.position = hit.point - transform.forward * 0.1F;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentTime>TotalTimeAlive)
        {
            AliveCounter++;
        }

        if(AliveCounter>10)
        {
            PrefabPool.Despawn(gameObject);
        }
    }
}
