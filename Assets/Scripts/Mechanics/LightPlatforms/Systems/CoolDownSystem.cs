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

    protected override void OnUpdate()
    {
        foreach(var entity in GetEntities<Group>())
        {
            var toActivate = entity.IndirectActivator.PlatformToActivate;
            if(toActivate.GetComponent<LightActivatedPlatformComponent>().IsRetracting)
            {
                ShaderHelper.SetFillValue(entity.Transform.GetComponent<Renderer>().material, 0F);
            }
        }
    }
}
