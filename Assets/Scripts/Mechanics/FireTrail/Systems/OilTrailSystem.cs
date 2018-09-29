using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class OilTrailSystem : ComponentSystem
{
    private struct Group
    {
        public OilTrailComponent OilTrail;
        public InputComponent Input;
        public Transform Transform;
    }

    protected override void OnUpdate()
    {
        var entities = GetEntities<Group>();
        foreach (var entity in entities)
        {
            if (entity.OilTrail.IsEquipped)
            {
                var position = entity.Transform.position;
                var lineRenderer = entity.OilTrail.LineRenderer;
                var minDistance = entity.OilTrail.TrailMinimumDistance;
                if (Input.GetMouseButton(0))
                {
                    if (entity.OilTrail.CurrentTrailCount == 0)
                    {
                        entity.OilTrail.TrailPoints.Add(position);
                        lineRenderer.positionCount = 2;
                        position.y = 0.85f;
                        lineRenderer.SetPosition(0, position);
                      
                        position.x += 0.1f;
                        lineRenderer.SetPosition(1, position);
                        entity.OilTrail.CurrentTrailCount = 2;
                    }
                    else
                    {
                        var distance = float.MaxValue;
                        Vector3 closestPoint = new Vector3();
                        for (int i = 0; i < lineRenderer.positionCount; ++i)
                        {
                            var trailDistance = Vector3.Distance(lineRenderer.GetPosition(i), position);
                            if (trailDistance < distance)
                            {
                                distance = trailDistance;
                                closestPoint = lineRenderer.GetPosition(i);
                            }
                        }

                        if (entity.OilTrail.CurrentTrailCount < entity.OilTrail.TrailLimit && distance >= minDistance)
                        {
                            entity.OilTrail.TrailPoints.Add(position);
                            lineRenderer.positionCount = entity.OilTrail.CurrentTrailCount;
                            position.y = 0.85f;
                            lineRenderer.SetPosition(entity.OilTrail.CurrentTrailCount - 1, position);
                           
                            entity.OilTrail.CurrentTrailCount++;
                        }
                    }
                }
            }
        }
    }

}
