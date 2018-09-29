using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class PlatformActivationSystem : ComponentSystem
{

    private struct LightData
    {
        public readonly int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<PlatformActivatorComponent> Activator;
        public ComponentArray<TimedComponent> ActivationTime;
    }

    [Inject]
    private LightData Light;

    protected override void OnUpdate()
    {
        var lightTransform = Light.Transform[0];
        var activator = Light.Activator[0];
        var activationTime = Light.ActivationTime[0];
        var origin = lightTransform.position;
        var direction = lightTransform.forward;

        // Physics ray cast using Job system
        var results = new NativeArray<RaycastHit>(1, Allocator.Temp);
        var commands = new NativeArray<RaycastCommand>(1, Allocator.Temp);
        commands[0] = new RaycastCommand(origin, direction);

        var handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        handle.Complete();
        RaycastHit hit = results[0];

        if (hit.collider != null && hit.collider.tag == "LightActivatedPlatform")
        {
            ActivatePlatform(hit.collider.gameObject, activationTime);
        }
        else
        {
            activationTime.CurrentTime = 0F;
        }

        results.Dispose();
        commands.Dispose();
    }

    void ActivatePlatform(GameObject platformObject, TimedComponent activationTime)
    {
        var platform = platformObject.GetComponent<LightActivatedPlatformComponent>();
        if(!platform.IsActivated)
        {
            if(activationTime.CurrentTime < activationTime.TimeThreshold)
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
