using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class MoveableSystem : ComponentSystem
{

    private struct Group
    {
        public MoveableComponent Platform;
        public Transform Transform;
    }

    public struct PlayerGroup
    {
        public readonly int Length;
        public ComponentArray<InputComponent> Input;
    }

    [Inject]
    private PlayerGroup Player;

    protected override void OnUpdate()
    {
        var input = Player.Input[0];
        var entities = GetEntities<Group>();
        foreach (var entity in entities)
        {
            var target = entity.Transform.position;
            var horizontal = input.Horizontal;
            var vertical = input.Vertical;
            if (input.Horizontal > 0F)
            {
                target = entity.Platform.PointB;
            }
            else if (input.Horizontal < 0F)
            {
                target = entity.Platform.PointA;
            }

            if (entity.Platform.IsSelected)
            {
                entity.Transform.position = Vector3.MoveTowards(
                    entity.Transform.position,
                    target,
                    entity.Platform.MoveSpeed * Mathf.Abs(horizontal) * Time.deltaTime
                    );

                entity.Transform.Rotate(
                    entity.Transform.up, 
                    vertical * Time.deltaTime * 100F
                    );
            }
          
        }
    }

}
