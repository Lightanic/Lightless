using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

public class FireInstanceSystem : ComponentSystem
{
    struct Group
    {
        public FireInstanceComponent FireInstance;
        public Transform Transform;
    }

    protected override void OnUpdate()
    {
        var entities = GetEntities<Group>();
        foreach (var entity in entities)
        {
            HandleFireInstance(entity);
        }
        
    }

    /// <summary>
    /// Handles fire particle instances. Lets the fire particle systems run until time threshold. 
    /// </summary>
    /// <param name="entity"></param>
    private void HandleFireInstance(Group entity)
    {
        var particleSystem = entity.Transform.gameObject.GetComponentInChildren<ParticleSystem>();
        var fireInstance = entity.FireInstance;
        if (fireInstance.CurrentFireTime > fireInstance.TotalFireTime)
        {
            var main = particleSystem.main;
            main.loop = false;
        }

        if (particleSystem.isStopped)
        {
            fireInstance.DestroyNextUpdate = true;
        }

        fireInstance.CurrentFireTime += Time.deltaTime;
    }
}

