using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FireSystem : ComponentSystem
{
    private struct Group
    {
        public FireComponent Fire;
        public OilTrailComponent OilTrail;
        public Transform Transform;
    }

    protected override void OnUpdate()
    {
        var entities = GetEntities<Group>();
        foreach(var entity in entities)
        {
            var firePrefab = entity.Fire.FirePrefab;
            var points = entity.OilTrail.TrailPoints;
            HandleFireInstances(entity);
            
            if (Input.GetKeyDown(KeyCode.F) && points.Count > 0)
            {
                entity.Fire.IsFireStopped = false;
                entity.OilTrail.LineRenderer.positionCount = 0;
                entity.OilTrail.CurrentTrailCount = 0;
                foreach(var point in points)
                {
                    var instance = Object.Instantiate(firePrefab, point, new Quaternion());
                    entity.Fire.Instances.Add(instance);
                }
                
                entity.OilTrail.TrailPoints.Clear();
            }
        }
    }

    private int GetClosestPointIndex(Vector3[] points, Vector3 position)
    {
        int index = -1;
        var minDistance = float.MaxValue;
        for(int i=0;i < points.Length; ++i)
        {
            var distance = Vector3.Distance(points[i], position);
            if (distance < minDistance)
            {
                distance = minDistance;
                index = i;
            }
        }

        return index;
    }

    void HandleFireInstances(Group entity)
    {
        var fireComponent = entity.Fire;
        var fireInstances = entity.Fire.Instances;
        if (fireInstances.Count > 0)
        {
            if (fireComponent.IsFireStopped)
            {
                foreach(var instance in fireInstances)
                {
                    Object.Destroy(instance);
                }

                fireInstances.Clear();
                fireComponent.CurrentFireTime = 0F;
                return;
            }

            if(fireComponent.CurrentFireTime > fireComponent.TotalFireTime)
            {
                foreach(var instance in fireInstances)
                {
                    var particleSystem = instance.GetComponent<ParticleSystem>();
                    var main = particleSystem.main;
                    main.loop = false;
                    if (particleSystem.isStopped)
                        fireComponent.IsFireStopped = true;
                }
            }

            fireComponent.CurrentFireTime += Time.deltaTime;
        }
    }
}
