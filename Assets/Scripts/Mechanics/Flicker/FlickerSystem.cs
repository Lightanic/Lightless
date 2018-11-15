using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FlickerSystem : ComponentSystem
{
    private struct Group
    {
        public FlickerComponent Flicker;
        public Transform Transform;
    }

    protected override void OnUpdate()
    {
        var entities = GetEntities<Group>();
        foreach (var entity in entities)
        {
            FlickerLight(entity);
        }
    }

    void FlickerLight(Group entity)
    {
        if (entity.Flicker.CurrentTime > entity.Flicker.RateDamping && !entity.Flicker.StopFlickering)
        {
            var light = entity.Flicker.LightSource;
            light.intensity =
                Mathf.Lerp(light.intensity, Random.Range(
                    entity.Flicker.BaseIntensity - entity.Flicker.MaxReduction,
                    entity.Flicker.BaseIntensity + entity.Flicker.MaxIncrease),
                    entity.Flicker.Strength * Time.deltaTime
                );
            entity.Flicker.CurrentTime = 0F;
        }

        entity.Flicker.CurrentTime += Time.deltaTime;
    }
}
