using cakeslice;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class MoveableSystem : ComponentSystem
{

    private struct Group
    {
        public Outline Outline;
        public MoveableComponent Platform;
        public Transform Transform;
    }

    public struct PlayerGroup
    {
        public readonly int Length;
        public ComponentArray<InputComponent> Input;
        public ComponentArray<Transform> Transform;
    }

    [Inject]
    private PlayerGroup Player;

    private float CurrentTime = 0;
    private const float KeyDelay = 0.2F;

    protected override void OnUpdate()
    {
        var input = Player.Input[0];
        var player = Player.Transform[0];
        var entities = GetEntities<Group>();
        foreach (var entity in entities)
        {
            var activatorPosition = entity.Transform.position;
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

            if (entity.Platform.Activator != null)
            {
                activatorPosition = entity.Platform.Activator.position;
            }

            var distance = Vector3.Distance(activatorPosition, player.position);
            if (input.Control("Interact") && CurrentTime > KeyDelay)
            {
                if (entity.Platform.IsSelected)
                {
                    CurrentTime = 0F;
                    entity.Platform.IsSelected = false;
                    input.EnablePlayerMovement = true;
                    entity.Outline.eraseRenderer = true;
                }
                else if (distance < 4F)
                {
                    CurrentTime = 0F;
                    entity.Platform.IsSelected = true;
                    input.EnablePlayerMovement = false;
                    entity.Outline.eraseRenderer = false;
                }
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

        CurrentTime += Time.deltaTime;
    }

}
