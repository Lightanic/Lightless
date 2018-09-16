using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.ECS_Sample.PureECS
{
    public struct SpeedComponent : IComponentData
    {
        public float Value;
    }
}
