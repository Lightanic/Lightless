using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using Unity.Collections;
>>>>>>> Develop
using UnityEngine;

public class PlatformActivatorComponent : MonoBehaviour
{
<<<<<<< HEAD

    public float ActivationDelay = 1F;
    public float MaxActivationDistance = 15F;
    public GameObject ReflectionLightPrefab;
    public GameObject LightInstance = null;
    public bool ShouldBeDestroyed = false;
    public bool ShouldCreateLightInstance = false;
    public LightComponent Switch;
    public bool IsReflected = false;
    public GameObject PrevInstance = null;

    private void Update()
    {

=======
    private static readonly int RaycastCount = 3;

    [Header("Configuration")]
    public float                ActivationDelay = 1F;
    public float                MaxActivationDistance = 15F;
    public float                LightWidthStep = 0.1F;

    [Header("Instance Data")]
    public bool                 IsReflected = false;

    public LightComponent       Switch;
    public GameObject           PrevInstance = null;
    public GameObject           ReflectionLightPrefab;
    public GameObject           LightInstance = null;


    public GameObject   MainInstance = null;
    public float        MaximumReflectionChain = 8;
    public float        CurrentChainCount = 0;

    private void Update()
    {
>>>>>>> Develop
        if (!Switch.LightIsOn)
        {
            if (LightInstance != null)
            {
                Destroy(LightInstance);
                LightInstance = null;
            }
            return;
        }

<<<<<<< HEAD
        if (IsReflected && !Switch.LightIsOn)
        {
            LightInstance.GetComponent<PlatformActivatorComponent>().PrevInstance = null;
        }

        if(IsReflected && PrevInstance == null)
        {
            Destroy(gameObject);
        }
        //if(ShouldCreateLightInstance && LightInstance == null)
        //{
        //    LightInstance = Instantiate(ReflectionLightPrefab);
        //    ShouldCreateLightInstance = false;
        //}

        //if (ShouldBeDestroyed && LightInstance!=null)
        //{
        //    Destroy(LightInstance);
        //    ShouldBeDestroyed = false;
        //}
        Ray ray = new Ray(transform.position, transform.forward);
        Ray ray1 = new Ray(transform.position + transform.right * 0.1F, transform.forward);
        Ray ray2 = new Ray(transform.position + transform.right * -0.1F, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction);
        Debug.DrawRay(ray1.origin, ray1.direction);
        Debug.DrawRay(ray2.origin, ray2.direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20F))
        {
            if (hit.collider.tag == "Reflector")
            {
                if (LightInstance == null)
=======
        if (IsReflected && PrevInstance == null)
        {
            Destroy(gameObject);
        }

        var direction = transform.forward;
        var results = new NativeArray<RaycastHit>(RaycastCount, Allocator.Temp);
        var commands = new NativeArray<RaycastCommand>(RaycastCount, Allocator.Temp);
        commands[0] = new RaycastCommand(transform.position + transform.forward * 1F, transform.forward, MaxActivationDistance);
        commands[1] = new RaycastCommand(transform.position + transform.right * LightWidthStep, transform.forward, MaxActivationDistance);
        commands[2] = new RaycastCommand(transform.position + transform.right * -LightWidthStep, transform.forward, MaxActivationDistance);

        var handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        handle.Complete();

        bool hasHit = false;
        RaycastHit hit = new RaycastHit();
        for (var i = 0; i < RaycastCount; ++i)
        {
            if (results[i].collider != null)
            {
                hit = results[i];
                hasHit = true;
                break;
            }
        }

        if (hasHit && Switch.LightIsOn)
        {
            if (hit.collider.tag == "Reflector")
            {
                GetComponent<LineRendererComponent>().AddLine(new ReflectionLine(transform.position, hit.point));
                if (LightInstance == null && CurrentChainCount < MaximumReflectionChain)
>>>>>>> Develop
                {
                    LightInstance = Instantiate(ReflectionLightPrefab, hit.point, hit.transform.rotation);
                    LightInstance.GetComponent<PlatformActivatorComponent>().IsReflected = true;
                    LightInstance.GetComponent<PlatformActivatorComponent>().PrevInstance = gameObject;
<<<<<<< HEAD
                    //LightInstance.GetComponent<PlatformActivatorComponent>().MainInstance = MainInstance;
                }
                else
                {
                    LightInstance.transform.position = hit.point;
                }
                var point = hit.point;
                var normal = hit.transform.forward;
                var reflection = ray.direction - 2 * (Vector3.Dot(ray.direction, normal)) * normal;
                var lookTowardsPos = point + reflection * 2F;
                LightInstance.transform.LookAt(lookTowardsPos);
                Debug.DrawRay(point, reflection);
=======
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
                    var reflection = direction - 2 * (Vector3.Dot(direction, normal)) * normal;
                    var lookTowardsPos = point + reflection * 2F;
                    LightInstance.transform.LookAt(lookTowardsPos);
                    Debug.DrawRay(point, reflection);
                }

>>>>>>> Develop
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
<<<<<<< HEAD
=======

        results.Dispose();
        commands.Dispose();
>>>>>>> Develop
    }

}
