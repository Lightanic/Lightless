using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractorComponent : MonoBehaviour
{

    public GameObject ReflectionLightPrefab;
    public GameObject LightInstance = null;
    public LightComponent Switch;
    public bool IsRefracted = false;

    public List<GameObject> LightInstances;

    // Use this for initialization
    void Start()
    {
        LightInstances = new List<GameObject>(10);
    }

    // Update is called once per frame
    void Update()
    {
        if(!Switch.LightIsOn)
        {
            DestroyLightInstances();
        }

        Ray ray = new Ray(transform.position + transform.forward * 1F, transform.forward);
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
                GetComponent<LineRendererComponent>().AddLine(new ReflectionLine(ray.origin, hit.point));
                var splitCount = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().SplitCount;
                var hitPoint = hit.point;
                hitPoint = hitPoint + ray.direction * 1.5F;
                InstantiateLightInstances(splitCount, hitPoint, hit.transform.rotation);

                if (LightInstances.Count > 0)
                {
                    var range = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().SplitAngleRange;
                    var point = hitPoint;
                    var normal = hit.transform.forward;
                    var refractionAngle = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().RefractionAngle;

                    var reflection = ray.direction + 2 * (Vector3.Dot(ray.direction, normal)) * normal;
                    reflection = Quaternion.AngleAxis(refractionAngle, hit.transform.up) * reflection;
                    var lookTowardsPos = point + reflection * 2F;
                  //  LightInstance.transform.LookAt(lookTowardsPos);
                    Debug.DrawRay(point, reflection);
                    var oppAngle = Mathf.Abs(refractionAngle) - range;
                    var total = 2 * Mathf.Abs(refractionAngle);
                    var step = range / (splitCount - 1);

                    int index = 0;
                    for (float angle = refractionAngle; angle >= oppAngle; angle -= step)
                    {
                        reflection = Quaternion.AngleAxis(angle, hit.transform.up) * reflection;
                        var lookTowards = point + reflection * 2F;
                        LightInstances[index].transform.LookAt(lookTowards);
                        index++;
                    }
                }

            }
            else if (LightInstances.Count > 0)
            {
                DestroyLightInstances();
               // Destroy(LightInstance);
                LightInstance = null;
            }
        }
        else if (LightInstances.Count > 0)
        {
            DestroyLightInstances();
           // Destroy(LightInstance);
            LightInstance = null;
        }
    }

    void DrawLine(Vector3 origin, Vector3 point)
    {

    }

    void InstantiateLightInstances(int count, Vector3 point, Quaternion rotation)
    {
        if (LightInstances.Count == 0)
        {
            for (int i = 0; i < count; ++i)
            {
                var instance = Instantiate(ReflectionLightPrefab, point, rotation);
                LightInstances.Add(instance);
                instance.GetComponent<RefractorComponent>().IsRefracted = true;
            }
        }
        else
        {
            for (int i = 0; i < count; ++i)
            {
                LightInstances[i].transform.position = point;
            }
        }
    }

    void DestroyLightInstances()
    {
        foreach (var light in LightInstances)
        {
            Destroy(light);
        }

        LightInstances.Clear();
    }

}
