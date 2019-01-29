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
        public ComponentArray<LineRendererComponent> LineComponent;
    }

    [Inject]
    private LightData Light;

    /// <summary>
    /// Player information
    /// </summary>
    private struct PlayerData
    {
        readonly public int Length;
        public ComponentArray<InputComponent> Input;
    }

    [Inject]
    private PlayerData Player; //Inject player data. 

    protected override void OnUpdate()
    {
        // Physics ray cast using Job system to check if light is hitting platform. 
        var results = new NativeArray<RaycastHit>(Light.Length, Allocator.TempJob);
        var commands = new NativeArray<RaycastCommand>(Light.Length, Allocator.TempJob);
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
            var isRefracted = Light.Refractor[i].IsRefracted;
            var isReflected = Light.Activator[i].IsReflected;
            var lineComponent = Light.LineComponent[i];
            RaycastHit hit = results[i];
            if (hit.collider != null)
            {
                var indirectActivator = hit.collider.gameObject.GetComponent<IndirectPlatformActivatorComponent>();
                if (hit.collider.tag == "IndirectLightActivatedPlatform" && !isReflected && !isRefracted)
                {
                    lineComponent.AddLine(new ReflectionLine(origins[i], hit.point));
                    var platform = indirectActivator.PlatformToActivate;
                    var activationTime = hit.collider.gameObject.GetComponent<TimedComponent>();
                    ActivatePlatform(platform, activationTime);
                }
                else
                {
                    if (hit.collider.tag == "IndirectRefractionActivatedPlatform" && isRefracted)
                    {
                        lineComponent.AddLine(new ReflectionLine(origins[i], hit.point));
                        var platform = indirectActivator.PlatformToActivate;
                        var activationTime = hit.collider.gameObject.GetComponent<TimedComponent>();
                        ActivatePlatform(platform, activationTime);
                    }

                    if (hit.collider.tag == "IndirectReflectionActivatedPlatform" && isReflected)
                    {
                        lineComponent.AddLine(new ReflectionLine(origins[i], hit.point));
                        var platform = indirectActivator.PlatformToActivate;
                        var activationTime = hit.collider.gameObject.GetComponent<TimedComponent>();
                        ActivatePlatform(platform, activationTime);
                    }
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
        if (platform.HasActivated && platform.IsOneTimeActivation)
        {
            return;
        }

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
                Player.Input[0].Rumble(0.3f, new Vector2(5, 5), 0);
            }
        }
        else
        {
            platform.CurrentTime = 0F;
            platform.IsActivated = true;
        }
    }
}
