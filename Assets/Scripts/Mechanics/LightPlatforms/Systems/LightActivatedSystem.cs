using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class LightActivatedSystem : ComponentSystem
{
    private struct Group
    {
        public LightActivatedPlatformComponent Platform;
        public Transform Transform;
    }

    private struct PlayerData
    {
        readonly public int Length;
        public ComponentArray<InputComponent> Input;
        public ComponentArray<Transform> Transform;
    }

    [Inject]
    private PlayerData Player;

    protected override void OnUpdate()
    {
        var input = Player.Input[0];
        var playerTransform = Player.Transform[0];
        var entities = GetEntities<Group>();
        foreach (var entity in entities)
        {
            var isActivated = entity.Platform.IsActivated;
            var transform = entity.Transform;
            if (isActivated)
            {
                HandleActivatedPlatform(entity.Platform, entity.Transform);
            }
            else
            {
                HandleInactivePlatform(entity.Platform, entity.Transform);
            }
        }
    }

    private void HandleInactivePlatform(LightActivatedPlatformComponent platform, Transform transform)
    {
        var startPosition = platform.StartPosition;
        var currentPosition = transform.position;
        var distance = Vector3.Distance(startPosition, currentPosition);
        if (distance > 0.01F)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, platform.MoveSpeed * Time.deltaTime);
        }
    }

    private void HandleActivatedPlatform(LightActivatedPlatformComponent platform, Transform transform)
    {
        var endPosition = platform.ActivatedPosition;
        var currentPosition = transform.position;
        var distance = Vector3.Distance(endPosition, currentPosition);
        if (distance > 0.1F)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, platform.MoveSpeed * Time.deltaTime);
        }
        else
        {
            if (platform.CurrentTime > platform.ActivationTime)
            {
                platform.IsActivated = false;
                platform.CurrentTime = 0F;
            }
            else
            {
                platform.CurrentTime += Time.deltaTime;
            }
        }
    }
}
