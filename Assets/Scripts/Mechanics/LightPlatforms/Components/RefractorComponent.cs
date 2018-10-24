using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractorComponent : MonoBehaviour
{

    public GameObject ReflectionLightPrefab;
    public GameObject LightInstance = null;
    public LightComponent Switch;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = new Ray(transform.position, transform.forward);
        Ray ray1 = new Ray(transform.position + transform.right * 0.1F, transform.forward);
        Ray ray2 = new Ray(transform.position + transform.right * -0.1F, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction);
        Debug.DrawRay(ray1.origin, ray1.direction);
        Debug.DrawRay(ray2.origin, ray2.direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20F) && Switch.LightIsOn)
        {
            if (hit.collider.tag == "Refractor" && Switch.LightIsOn)
            {
                var hitPoint = hit.point;
                hitPoint = hitPoint + ray.direction * 1.5F;
                if (LightInstance == null)
                {
                    LightInstance = Instantiate(ReflectionLightPrefab, hitPoint, hit.transform.rotation);
                    LightInstance.GetComponent<PlatformActivatorComponent>().IsReflected = true;
                    LightInstance.GetComponent<PlatformActivatorComponent>().PrevInstance = gameObject;
                }
                else if (LightInstance != null)
                {
                    LightInstance.transform.position = hitPoint;
                }

                if (LightInstance != null)
                {
                    var point = hitPoint;
                    var normal = hit.transform.forward;
                    var refractionAngle = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().RefractionAngle;
                    var splitCount = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().SplitCount;
                    var reflection = ray.direction + 2 * (Vector3.Dot(ray.direction, normal)) * normal;
                    reflection = Quaternion.AngleAxis(refractionAngle, hit.transform.up) * reflection;
                    var lookTowardsPos = point + reflection * 2F;
                    LightInstance.transform.LookAt(lookTowardsPos);
                    Debug.DrawRay(point, reflection);
                    var oppAngle = Mathf.Abs(refractionAngle) - 90F;
                    for (int i = 1; i < splitCount; ++i)
                    {
                        var angle = refractionAngle - i * refractionAngle / splitCount;
                        Debug.Log(i + " " + angle);
                    }
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
