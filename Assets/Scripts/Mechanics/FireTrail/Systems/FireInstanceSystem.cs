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
        foreach(var entity in entities)
        {
            Debug.Log("Test");
        }
    }
}

