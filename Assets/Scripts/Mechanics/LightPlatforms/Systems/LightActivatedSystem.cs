using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// System to handle light-activated platforms if they are "activated"
/// </summary>
public class LightActivatedSystem : ComponentSystem
{
    /// <summary>
    /// Light activated platform group
    /// </summary>
    private struct Group
    {
        public LightActivatedPlatformComponent Platform;
        public Transform Transform;
    }

    /// <summary>
    /// Player information
    /// </summary>
    private struct PlayerData
    {
        readonly public int Length;
        public ComponentArray<InputComponent> Input;
        public ComponentArray<Transform> Transform;
    }

    [Inject]
    private PlayerData Player; //Inject player data. 

    /// <summary>
    /// Moves the platforms accordingly once activated or activation time limit has reached. 
    /// </summary>
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

    /// <summary>
    /// If inactive, move the platform back to starting position.
    /// </summary>
    /// <param name="platform">Platform Component to move</param>
    /// <param name="transform">Transform of platform</param>
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


    /// <summary>
    /// Move platform towards the activated position i.e. open position. This can be defined in the LightActivatedPlatformComponent variable Vector3 ActivatedPosition.
    /// </summary>
    /// <param name="platform">Platform Component to move</param>
    /// <param name="transform">Transform of platform</param>
    private void HandleActivatedPlatform(LightActivatedPlatformComponent platform, Transform transform)
    {
        var endPosition = platform.ActivatedPosition;
        var currentPosition = transform.position;
        var distance = Vector3.Distance(endPosition, currentPosition);
        if (distance > 0.1F)
        {
            if (platform.ID == "elevator")
            {
                Player.Input[0].AddSmallRumble();
            }
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
