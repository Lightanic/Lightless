using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


/// <summary>
/// Activates light-activated platforms if spot light with activator component is shining light at the platform within given max distance.
/// </summary>
public class PlatformActivationSystem : ComponentSystem
{

    private struct LightData
    {
        public readonly int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<PlatformActivatorComponent> Activator;
        public ComponentArray<TimedComponent> ActivationTime;
        public ComponentArray<LineRendererComponent> LineComponent;
    }

    [Inject]
    private LightData Light;

    protected override void OnUpdate()
    {
        Vector3 testStart = Vector3.zero;
        // Physics ray cast using Job system to check if light is hitting platform. 
        var results = new NativeArray<RaycastHit>(Light.Length, Allocator.Temp);
        var commands = new NativeArray<RaycastCommand>(Light.Length, Allocator.Temp);
        var origins = new Vector3[Light.Length];
        for (int i = 0; i < Light.Length; ++i)
        {
            var lightTransform = Light.Transform[i];
            var activator = Light.Activator[i];

            var origin = lightTransform.position + lightTransform.forward * 1F;
            var direction = lightTransform.forward;
            origins[i] = origin;
            if (Light.Activator[i].Switch.LightIsOn)
                commands[i] = new RaycastCommand(origin, direction, activator.MaxActivationDistance);
        }

        var handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        handle.Complete();

        for (int i = 0; i < Light.Length; ++i)
        {
            var lineComponent = Light.LineComponent[i];
            var isReflected = Light.Activator[i].IsReflected;
            var activationTime = Light.ActivationTime[i];
            RaycastHit hit = results[i];

            if (hit.collider != null && hit.collider.tag == "LightActivatedPlatform")
            {
                lineComponent.AddLine(new ReflectionLine(origins[i], hit.point));
                ActivatePlatform(hit.collider.gameObject, activationTime);
            }
            else
            {
                if (hit.collider != null && hit.collider.tag == "ReflectionActivatedPlatform" && isReflected)
                {
                    lineComponent.AddLine(new ReflectionLine(origins[i], hit.point));
                    ActivatePlatform(hit.collider.gameObject, activationTime);
                }
                else
                {
                    activationTime.CurrentTime = 0F; // Reset current time if light is not shining on platform. 
                }
            }
        }


        results.Dispose();
        commands.Dispose();
    }

    /// <summary>
    /// Activate platform if light has been shining on object for given time threshold. 
    /// </summary>
    /// <param name="platformObject"></param>
    /// <param name="activationTime"></param>
    void ActivatePlatform(GameObject platformObject, TimedComponent activationTime)
    {
        var platform = platformObject.GetComponent<LightActivatedPlatformComponent>();
        if (!platform.IsActivated)
        {
            if (activationTime.CurrentTime < activationTime.TimeThreshold)
            {
                activationTime.CurrentTime += Time.deltaTime;
            }
            else
            {
                platform.IsActivated = true;
                activationTime.CurrentTime = 0;
            }
        }
    }
}
