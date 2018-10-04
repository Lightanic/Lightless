using System;
using Unity.Entities;

namespace Assets.Scripts.ECS_Sample.PureECS
{
    public struct PlayerInputComponent : IComponentData
    {
        public float Horizontal;
    }
}
