using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractorComponent : MonoBehaviour {

    public GameObject ReflectionLightPrefab;
    public GameObject LightInstance = null;
    public LightComponent Switch;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        Ray ray = new Ray(transform.position, transform.forward);
        Ray ray1 = new Ray(transform.position + transform.right * 0.1F, transform.forward);
        Ray ray2 = new Ray(transform.position + transform.right * -0.1F, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction);
        Debug.DrawRay(ray1.origin, ray1.direction);
        Debug.DrawRay(ray2.origin, ray2.direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20F) && Switch.LightIsOn)
        {
            if (hit.collider.tag == "Refractor")
            {
                if (LightInstance == null )
                {
                    LightInstance = Instantiate(ReflectionLightPrefab, hit.point, hit.transform.rotation);
                    LightInstance.GetComponent<PlatformActivatorComponent>().IsReflected = true;
                    LightInstance.GetComponent<PlatformActivatorComponent>().PrevInstance = gameObject;
                }
                else if (LightInstance != null)
                {
                    LightInstance.transform.position = hit.point;
                }

                if (LightInstance != null)
                {
                    var point = hit.point;
                    var normal = hit.transform.forward;
                    var reflection = ray.direction - 2 * (Vector3.Dot(ray.direction, normal)) * normal;
                    var lookTowardsPos = point + reflection * 2F;
                    LightInstance.transform.LookAt(lookTowardsPos);
                    Debug.DrawRay(point, reflection);
                }

            }
            else if (LightInstance != null)
            {
                Destroy(LightInstance);
                LightInstance = null;
            }
        }
        else if (LightInstance != null)
        {
            Destroy(LightInstance);
            LightInstance = null;
        }
    }
}
