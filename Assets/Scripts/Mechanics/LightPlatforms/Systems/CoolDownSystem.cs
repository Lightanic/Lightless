using Assets.Scripts.Mechanics.LightPlatforms;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CoolDownSystem : ComponentSystem
{
    struct Group
    {
        public IndirectPlatformActivatorComponent IndirectActivator;
        public Transform Transform;
    }

    struct PlatformGroup
    {
        public LightActivatedPlatformComponent Platform;
    }

    struct FlashlightGroup
    {
        public TimedComponent Timer;
        public PlatformActivatorComponent Activator;
    }

    FlashlightGroup flashlight = new FlashlightGroup();

    protected override void OnUpdate()
    {

        foreach (var light in GetEntities<FlashlightGroup>())
        {
            flashlight = light;
        }

        foreach (var entity in GetEntities<Group>())
        {
            CoolDownIndirectActivator(entity);
        }

        foreach (var platform in GetEntities<PlatformGroup>())
        {
            if (!platform.Platform.IsActivated)
            {
                platform.Platform.FillValue = Mathf.Lerp(platform.Platform.FillValue, 0F, Time.deltaTime);
                if (platform.Platform.FillValue != 0F)
                    ShaderHelper.SetFillValue(platform.Platform.GetComponent<Renderer>().material, platform.Platform.FillValue);
            }
        }
    }

    void CoolDownIndirectActivator(Group entity)
    {
        var toActivate = entity.IndirectActivator.PlatformToActivate;
        if (toActivate.GetComponent<LightActivatedPlatformComponent>().IsRetracting)
        {
            ShaderHelper.SetFillValue(entity.Transform.GetComponent<Renderer>().material, 0F);
        }
    }
}
