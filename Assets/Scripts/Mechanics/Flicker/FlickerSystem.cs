using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
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
            bool isEnemyNearby = false;
            var overlapResults = Physics.OverlapSphere(entity.Transform.position, 30F, LayerMask.GetMask("Enemy"));
            for(int i=0;i<overlapResults.Length;++i)
            {
                if (overlapResults[i].CompareTag("Enemy"))
                {
                    isEnemyNearby = true;
                    break;
                }
            }

            FlickerLight(entity, isEnemyNearby);

        }
    }

    void FlickerLight(Group entity, bool isEnemyNearby)
    {
        var light = entity.Flicker.LightSource;
        if (entity.Flicker.CurrentTime > entity.Flicker.RateDamping && !entity.Flicker.StopFlickering)
        {
            light.intensity =
                Mathf.Lerp(light.intensity, Random.Range(
                    entity.Flicker.BaseIntensity - entity.Flicker.MaxReduction,
                    entity.Flicker.BaseIntensity + entity.Flicker.MaxIncrease),
                    entity.Flicker.Strength * Time.deltaTime
                );
            entity.Flicker.CurrentTime = 0F;
        }

        if (isEnemyNearby)
        {
            light.range = Mathf.Lerp(light.range, Random.Range(
                    entity.Flicker.BaseRange - 5F,
                    entity.Flicker.BaseRange - 2F), Time.deltaTime * 2F);
        }
        else
        {
            light.range = Mathf.Lerp(light.range, entity.Flicker.BaseRange, Time.deltaTime * 2F);
        }

        entity.Flicker.CurrentTime += Time.deltaTime;
    }
}
