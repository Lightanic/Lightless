using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivatorComponent : MonoBehaviour
{

    public float ActivationDelay = 1F;
    public float MaxActivationDistance = 15F;
    public GameObject ReflectionLightPrefab;

    private GameObject lightInstance = null;
    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Ray ray1 = new Ray(transform.position + transform.right * 0.1F, transform.forward);
        Ray ray2 = new Ray(transform.position + transform.right * -0.1F, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction);
        Debug.DrawRay(ray1.origin, ray1.direction);
        Debug.DrawRay(ray2.origin, ray2.direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20F))
        {
            if(hit.collider.name == "Reflector")
            {
                if (lightInstance == null)
                {
                    lightInstance = Instantiate(ReflectionLightPrefab, hit.point, hit.transform.rotation);
                }
                else
                {
                    lightInstance.transform.position = hit.point;
                }
                var point = hit.point;
                var normal = hit.transform.forward;
                var reflection = ray.direction - 2 * (Vector3.Dot(ray.direction, normal)) * normal;
                var lookTowardsPos = point + reflection * 2F;
                lightInstance.transform.LookAt(lookTowardsPos);
                Debug.DrawRay(point, reflection);
            }
            else if (lightInstance != null)
            {
                Destroy(lightInstance);
                lightInstance = null;
            }
        }
        else if (lightInstance != null)
        {
            Destroy(lightInstance);
            lightInstance = null;
        }
    }

}
