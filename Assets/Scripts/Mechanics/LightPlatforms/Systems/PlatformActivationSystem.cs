﻿using Assets.Scripts.Mechanics.LightPlatforms;
using Assets.Scripts.Memory;
using System;
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
        public ComponentArray<RefractorComponent> Refractor;
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
            var lineComponent = Light.LineComponent[i];
            var isReflected = Light.Activator[i].IsReflected;
            var isRefractor = Light.Refractor[i].IsRefracted;
            var activationTime = Light.ActivationTime[i];
            RaycastHit hit = results[i];

            if (hit.collider != null && hit.collider.tag == "LightActivatedPlatform")
            {
                lineComponent.AddLine(new ReflectionLine(origins[i], hit.point));
                ActivatePlatform(hit.collider.gameObject, activationTime, hit);
               
            }
            else
            {
                if (hit.collider != null && hit.collider.tag == "ReflectionActivatedPlatform" && isReflected)
                {
                    lineComponent.AddLine(new ReflectionLine(origins[i], hit.point));
                    ActivatePlatform(hit.collider.gameObject, activationTime, hit);
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
    void ActivatePlatform(GameObject platformObject, TimedComponent activationTime, RaycastHit hit)
    {
        var platform = platformObject.GetComponent<LightActivatedPlatformComponent>();
        if(platform.HasActivated && platform.IsOneTimeActivation)
        {
            return;
        }

        if (!platform.IsActivated)
        {
            ShaderHelper.ApplyHitTexCoord(hit);
            ShaderHelper.SetFillValue(platformObject.GetComponent<Renderer>().material, activationTime.CurrentTime / activationTime.TimeThreshold);
            //Shader.SetGlobalFloat("_FillValue", activationTime.CurrentTime / activationTime.TimeThreshold);
            if (activationTime.CurrentTime < activationTime.TimeThreshold)
            {
                activationTime.CurrentTime += Time.deltaTime;
            }
            else
            {
                platform.IsActivated = true;
                ShaderHelper.SetFillValue(platformObject.GetComponent<Renderer>().material, 1F);
               // Shader.SetGlobalFloat("_FillValue", 1F);
                activationTime.CurrentTime = 0;
                Player.Input[0].Rumble(0.3f, new Vector2(5,5),0);
            }
        }
        else
        {
            ShaderHelper.SetFillValue(platformObject.GetComponent<Renderer>().material, 1F);
            //Shader.SetGlobalFloat("_FillValue", 1F);
            platform.CurrentTime = 0F;
            platform.IsActivated = true;
        }
    }

}
