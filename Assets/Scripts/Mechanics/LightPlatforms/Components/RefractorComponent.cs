using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class RefractorComponent : MonoBehaviour
{
    private static readonly int RaycastCount = 3;
    [Header("Configuration")]
    public float LightWidthStep = 0.1F;
    public float ActivationDistance = 20F;

    [Header("Light Instance Data")]
    public bool IsRefracted = false;
    public Collider PreviousCollider = null;
    public LightComponent Switch;
    public GameObject ReflectionLightPrefab;
    public RefractorComponent MainRefractorInstance = null;
    public List<GameObject> LightInstances;

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
        var results = new NativeArray<RaycastHit>(RaycastCount, Allocator.TempJob);
        var commands = new NativeArray<RaycastCommand>(RaycastCount, Allocator.TempJob);
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
                if (!IsRefracted)
                {
                    PreviousCollider = hit.collider;
                }

                GetComponent<LineRendererComponent>().AddLine(new ReflectionLine(transform.position, hit.point));
                var splitCount = hit.transform.gameObject.GetComponent<RefractionAngleComponent>().SplitCount;
                var hitPoint = hit.point;
                hitPoint = hitPoint + lightDirection * 1.5F;

                if (IsRefracted && hit.collider.name != PreviousCollider.name)
                    InstantiateLightInstances(splitCount, hitPoint, hit.transform.rotation, hit.collider);
                else if (!IsRefracted)
                    InstantiateLightInstances(splitCount, hitPoint, hit.transform.rotation, hit.collider);

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
                MainRefractorInstance = null;
            }
        }
        else if (LightInstances.Count > 0)
        {
            DestroyLightInstances();
            MainRefractorInstance = null;
        }

        results.Dispose();
        commands.Dispose();
    }

    void InstantiateLightInstances(int count, Vector3 point, Quaternion rotation, Collider collider)
    {
        if (LightInstances.Count == 0)
        {
            for (int i = 0; i < count; ++i)
            {
                //var instance = PrefabPool.Spawn(ReflectionLightPrefab, point, rotation);
                var instance = Instantiate(ReflectionLightPrefab, point, rotation);
                LightInstances.Add(instance);
                instance.GetComponent<RefractorComponent>().IsRefracted = true;
                instance.GetComponent<RefractorComponent>().MainRefractorInstance = MainRefractorInstance;
                instance.GetComponent<RefractorComponent>().PreviousCollider = collider;

                var reflectionColor = collider.GetComponent<BeamLight>();
                if (reflectionColor != null)
                {
                    instance.GetComponent<Light>().color = reflectionColor.LightColor;
                }
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
            PrefabPool.Despawn(light);
        }

        LightInstances.Clear();
        if (IsRefracted)
        {
            PrefabPool.Despawn(gameObject);
        }
    }

}
