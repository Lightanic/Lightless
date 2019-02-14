﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class FootprintSystem : ComponentSystem
{
    private struct FootprintGenGroup
    {
        public FootprintComponent Decal;
        public Transform Transform;
    }

    List<int> indices = new List<int>();

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<FootprintGenGroup>())
        {
            Quaternion quaternion = Quaternion.Euler(90F, entity.Transform.eulerAngles.y, entity.Transform.eulerAngles.z);
            if (entity.Decal.Instances.Count == 0)
            {
                var instance = PrefabPool.Spawn(entity.Decal.DecalPrefab, entity.Transform.position + Vector3.up * 0.01F, quaternion);
                entity.Decal.Instances.Add(instance);
            }

            var previousInstance = entity.Decal.Instances.ToArray()[entity.Decal.Instances.Count - 1];
            if (Vector3.Distance(previousInstance.transform.position, entity.Transform.position) > 1F)
            {
                var instance = PrefabPool.Spawn(entity.Decal.DecalPrefab, entity.Transform.position + Vector3.up * 0.01F, quaternion);
                entity.Decal.Instances.Add(instance);
            }

            indices.Clear();
            int index = 0;
            var instances = entity.Decal.Instances.ToArray();
            foreach(var instance in instances)
            {
                var decal = instance.GetComponent<DecalScript>();
                decal.CurrentTime += Time.deltaTime;
                if(decal.CurrentTime>decal.TotalTimeAlive)
                {
                    indices.Add(index);
                }
            }

            foreach(var i in indices)
            {
                PrefabPool.Despawn(entity.Decal.Instances[i]);
                entity.Decal.Instances.RemoveAt(i);
            }
        }
    }
}

