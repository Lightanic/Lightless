using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// System to instantiate fire at oil trail points. 
/// </summary>
public class FireSystem : ComponentSystem
{
    private struct Group
    {
        public FireComponent Fire;
        public Transform Transform;
    }

    private struct OilCanInstanceGroup
    {
        public ComponentArray<OilTrailComponent> OilTrail;
    }
    [Inject] private OilCanInstanceGroup OilCanInstance;

    /// <summary>
    /// Player data
    /// </summary>
    private struct Player
    {
        readonly public int Length;
        public ComponentArray<InputComponent> InputComponents;
    }
    [Inject] private Player playerData;

    protected override void OnUpdate()
    {
        var entities = GetEntities<Group>();
        if (OilCanInstance.OilTrail.Length > 0)
        {
            var oilTrail = OilCanInstance.OilTrail[0];
            foreach (var entity in entities)
            {
                var firePrefab = entity.Fire.FirePrefab;
                var points = oilTrail.TrailPoints;

                bool isPlayerClose = IsPlayerClose(points.ToArray(), entity.Transform.position, entity.Fire.OilTrailDistanceThreshold);

                // Allow burning of oil on ground only if player there is oil to burn and player is close to oil trail
                if (playerData.InputComponents[0].Control("LightFire") && points.Count > 0 && isPlayerClose)
                {
                    entity.Fire.IsFireStopped = false;
                    oilTrail.LineRenderer.positionCount = 0;
                    oilTrail.CurrentTrailCount = 0;
                    foreach (var point in points)
                    {
                        var pos = point;
                        pos.x += 0.2F;
                        var instance = Object.Instantiate(firePrefab, pos, new Quaternion());
                        entity.Fire.Instances.Add(instance);
                    }

                    oilTrail.TrailPoints.Clear(); //Clear out Oil Trail Component once fire has been instantiated. 
                }
            }
        }

    }

    /// <summary>
    /// Returns true if player is close to given position. 
    /// </summary>
    /// <param name="points"></param>
    /// <param name="position"></param>
    /// <param name="distanceThreshold"></param>
    /// <returns></returns>
    private bool IsPlayerClose(Vector3[] points, Vector3 position, float distanceThreshold)
    {
        int index = -1;
        var minDistance = float.MaxValue;
        for (int i = 0; i < points.Length; ++i)
        {
            var distance = Vector3.Distance(points[i], position);
            if (distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }

        return (minDistance <= distanceThreshold);
    }

}
