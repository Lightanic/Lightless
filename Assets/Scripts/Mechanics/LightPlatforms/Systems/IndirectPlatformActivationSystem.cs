using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


/// <summary>
/// Activates indirect light-activated platforms if spot light with activator component is shining light at the platform within given max distance.
/// </summary>
public class IndirectPlatformActivationSystem : ComponentSystem
{

    private struct LightData
    {
        public readonly int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<PlatformActivatorComponent> Activator;
        public ComponentArray<RefractorComponent> Refractor;
    }

    [Inject]
    private LightData Light;

    protected override void OnUpdate()
    {
        // Physics ray cast using Job system to check if light is hitting platform. 
        var results = new NativeArray<RaycastHit>(Light.Length, Allocator.Temp);
        var commands = new NativeArray<RaycastCommand>(Light.Length, Allocator.Temp);

        for (int i = 0; i < Light.Length; ++i)
        {
            var lightTransform = Light.Transform[i];
            var activator = Light.Activator[i];

            var origin = lightTransform.position + lightTransform.forward * 1F;
            var direction = lightTransform.forward;

            if (Light.Activator[i].Switch.LightIsOn)
                commands[i] = new RaycastCommand(origin, direction, activator.MaxActivationDistance);
        }

        var handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        handle.Complete();

        for (int i = 0; i < Light.Length; ++i)
        {
            var isRefracted = Light.Refractor[i].IsRefracted;
            var isReflected = Light.Activator[i].IsReflected;
            RaycastHit hit = results[i];

            if (hit.collider != null && hit.collider.tag == "IndirectLightActivatedPlatform")
            {
                var platform = hit.collider.gameObject.GetComponent<IndirectPlatformActivatorComponent>().PlatformToActivate;
                var activationTime = hit.collider.gameObject.GetComponent<TimedComponent>();
                ActivatePlatform(platform, activationTime);
            }
            else
            {
                if (hit.collider != null && hit.collider.tag == "IndirectRefractionActivatedPlatform" && isRefracted)
                {
                    var platform = hit.collider.gameObject.GetComponent<IndirectPlatformActivatorComponent>().PlatformToActivate;
                    var activationTime = hit.collider.gameObject.GetComponent<TimedComponent>();
                    ActivatePlatform(platform, activationTime);
                }

                if (hit.collider != null && hit.collider.tag == "IndirectReflectionActivatedPlatform" && isReflected)
                {
                    var platform = hit.collider.gameObject.GetComponent<IndirectPlatformActivatorComponent>().PlatformToActivate;
                    var activationTime = hit.collider.gameObject.GetComponent<TimedComponent>();
                    ActivatePlatform(platform, activationTime);
                }
            }
        }


        results.Dispose();
        commands.Dispose();
    }

    /// <summary>
    /// Activate platform if light has been shining on indirect object for given time threshold. 
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
