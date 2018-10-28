using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivatorComponent : MonoBehaviour
{

    public float ActivationDelay = 1F;
    public float MaxActivationDistance = 15F;
    public GameObject ReflectionLightPrefab;
    public GameObject LightInstance = null;
    public bool ShouldBeDestroyed = false;
    public bool ShouldCreateLightInstance = false;
    public LightComponent Switch;
    public bool IsReflected = false;
    public GameObject PrevInstance = null;

    public GameObject MainInstance = null;
    public float MaximumReflectionChain = 8;
    public float CurrentChainCount = 0;

    private void Update()
    {

        if (!Switch.LightIsOn)
        {
            if (LightInstance != null)
            {
                Destroy(LightInstance);
                LightInstance = null;
            }
            return;
        }

        //if (IsReflected && !Switch.LightIsOn)
        //{
        //    LightInstance.GetComponent<PlatformActivatorComponent>().PrevInstance = null;
        //}

        if (IsReflected && PrevInstance == null)
        {
            Destroy(gameObject);
        }

        Ray ray = new Ray(transform.position + transform.forward * 1F, transform.forward);
        Ray ray1 = new Ray(transform.position + transform.right * 0.1F, transform.forward);
        Ray ray2 = new Ray(transform.position + transform.right * -0.1F, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction);
        Debug.DrawRay(ray1.origin, ray1.direction);
        Debug.DrawRay(ray2.origin, ray2.direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, MaxActivationDistance) && Switch.LightIsOn)
        {
            if (hit.collider.tag == "Reflector")
            {
                GetComponent<LineRendererComponent>().AddLine(new ReflectionLine(ray.origin, hit.point));
                if (LightInstance == null && CurrentChainCount < MaximumReflectionChain)
                {
                    LightInstance = Instantiate(ReflectionLightPrefab, hit.point, hit.transform.rotation);
                    LightInstance.GetComponent<PlatformActivatorComponent>().IsReflected = true;
                    LightInstance.GetComponent<PlatformActivatorComponent>().PrevInstance = gameObject;
                    LightInstance.GetComponent<PlatformActivatorComponent>().MainInstance = MainInstance;
                    LightInstance.GetComponent<PlatformActivatorComponent>().CurrentChainCount = CurrentChainCount + 1;
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
