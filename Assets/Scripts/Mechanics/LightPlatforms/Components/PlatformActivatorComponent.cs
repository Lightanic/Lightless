using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlatformActivatorComponent : MonoBehaviour
{
    private static readonly int RaycastCount = 3;

    [Header("Configuration")]
    public float ActivationDelay = 1F;
    public float MaxActivationDistance = 15F;
    public float LightWidthStep = 0.1F;

    [Header("Instance Data")]
    public bool IsReflected = false;

    public LightComponent Switch;
    public GameObject PrevInstance = null;
    public GameObject ReflectionLightPrefab;
    public GameObject LightInstance = null;
    public Collider PreviousCollider = null;

    public GameObject MainInstance = null;
    public float MaximumReflectionChain = 8;
    public float CurrentChainCount = 0;

    private void Update()
    {
        if (!Switch.LightIsOn)
        {
            if (LightInstance != null)
            {
                DestroyLightInstance();
            }
            return;
        }

        if (IsReflected && PrevInstance == null)
        {
            Destroy(gameObject);
        }

        var direction = transform.forward;
        var results = new NativeArray<RaycastHit>(RaycastCount, Allocator.TempJob);
        var commands = new NativeArray<RaycastCommand>(RaycastCount, Allocator.TempJob);
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
                if (!IsReflected)
                {
                    PreviousCollider = hit.collider;
                }

                GetComponent<LineRendererComponent>().AddLine(new ReflectionLine(transform.position, hit.point));
                if (ShouldInstantiateLight(hit.collider))
                {
                    LightInstance = Instantiate(ReflectionLightPrefab, hit.point, hit.transform.rotation);
                    LightInstance.GetComponent<PlatformActivatorComponent>().IsReflected = true;
                    LightInstance.GetComponent<PlatformActivatorComponent>().PrevInstance = gameObject;
                    LightInstance.GetComponent<PlatformActivatorComponent>().MainInstance = MainInstance;
                    LightInstance.GetComponent<PlatformActivatorComponent>().CurrentChainCount = CurrentChainCount + 1;
                    LightInstance.GetComponent<PlatformActivatorComponent>().PreviousCollider = hit.collider;
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

            }
            else if (LightInstance != null)
            {
                DestroyLightInstance();
            }
        }
        else if (LightInstance != null)
        {
            DestroyLightInstance();
        }

        results.Dispose();
        commands.Dispose();
    }

    void DestroyLightInstance()
    {
        PrefabPool.Despawn(LightInstance);
        LightInstance = null;
        if (IsReflected)
            PrefabPool.Despawn(gameObject);
    }

    bool ShouldInstantiateLight(Collider collider)
    {
        return (LightInstance == null && CurrentChainCount < MaximumReflectionChain) && ((IsReflected && collider.name != PreviousCollider.name) || !IsReflected);
    }

}
