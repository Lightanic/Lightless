using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class RefractorComponent : MonoBehaviour
{
    private static readonly int RaycastCount = 3;
    public float                LightWidthStep = 0.1F;
    public bool                 IsRefracted = false;
    public float                ActivationDistance = 20F;

    public LightComponent       Switch;
    public GameObject           ReflectionLightPrefab;
    public GameObject           LightInstance = null;
    public List<GameObject>     LightInstances;

    void Start()
    {
        LightInstances = new List<GameObject>(10);
    }

    void Update()
    {
        if (!Switch.LightIsOn)
        {
            DestroyLightInstances();
        }

        var lightDirection = transform.forward;
        var results = new NativeArray<RaycastHit>(RaycastCount, Allocator.Temp);
        var commands = new NativeArray<RaycastCommand>(RaycastCount, Allocator.Temp);
        commands[0] = new RaycastCommand(transform.position + transform.forward * 1F, transform.forward, ActivationDistance);
        commands[1] = new RaycastCommand(transform.position + transform.right * LightWidthStep, transform.forward, ActivationDistance);
        commands[2] = new RaycastCommand(transform.position + transform.right * -LightWidthStep, transform.forward, ActivationDistance);

        var handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        handle.Complete();

        for (int i = 0; i < RaycastCount; ++i)
            Debug.DrawRay(commands[i].from, commands[i].direction);

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
            if (hit.collider.tag == "Refractor" && Switch.LightIsOn)
            {
                GetComponent<LineRendererComponent>().AddLine(new ReflectionLine(transform.position, hit.point));
                var splitCount = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().SplitCount;
                var hitPoint = hit.point;
                hitPoint = hitPoint + lightDirection * 1.5F;
                InstantiateLightInstances(splitCount, hitPoint, hit.transform.rotation);

                if (LightInstances.Count > 0)
                {
                    var range = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().SplitAngleRange;
                    var point = hitPoint;
                    var normal = hit.transform.forward;
                    var refractionAngle = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().RefractionAngle;

                    var reflection = lightDirection + 2 * (Vector3.Dot(lightDirection, normal)) * normal;
                    reflection = Quaternion.AngleAxis(refractionAngle, hit.transform.up) * reflection;
                    var lookTowardsPos = point + reflection * 2F;
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
                LightInstance = null;
            }
        }
        else if (LightInstances.Count > 0)
        {
            DestroyLightInstances();
            LightInstance = null;
        }

        results.Dispose();
        commands.Dispose();
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
