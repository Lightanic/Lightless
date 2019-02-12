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
    private Vector3 RotationEulers;

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
                if (entity.Platform.IsSelected) //Unselect if already selected
                {
                    CurrentTime = 0F;
                    entity.Platform.IsSelected = false;
                    input.EnablePlayerMovement = true;
                    entity.Outline.eraseRenderer = true;
                    if (entity.Platform.Activator != null)
                    {
                        var outline = entity.Platform.Activator.GetComponent<Outline>();
                        if (outline != null)
                        {
                            outline.eraseRenderer = true;
                        }
                    }
                }
                else if (distance < 4F) //Select if within distance
                {
                    CurrentTime = 0F;
                    entity.Platform.IsSelected = true;
                    input.EnablePlayerMovement = false;
                    entity.Outline.eraseRenderer = false;
                    if (entity.Platform.Activator != null)
                    {
                        var outline = entity.Platform.Activator.GetComponent<Outline>();
                        if (outline != null)
                        {
                            outline.eraseRenderer = false;
                        }
                    }
                }
            }

            if(entity.Platform.IsSelected && distance > 4F) //Enable player movement if the player is far enough from the platform
            {
                CurrentTime = 0F;
                entity.Platform.IsSelected = false;
                input.EnablePlayerMovement = true;
                entity.Outline.eraseRenderer = true;
                if (entity.Platform.Activator != null)
                {
                    var outline = entity.Platform.Activator.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.eraseRenderer = true;
                    }
                }
            }

            if (entity.Platform.IsSelected)
            {
                float x = 0F;
                float y = 0F;
                float z = 0F;
                if (InputManager.Instance.IsGamePadActive)
                {
                    float rotation = Player.Input[0].Gamepad.GetStick_R().X * Time.deltaTime * 100F;
                    if (entity.Platform.XAxis) x = -Player.Input[0].Gamepad.GetStick_R().Y * Time.deltaTime * 100F;
                    if (entity.Platform.YAxis) y = Player.Input[0].Gamepad.GetStick_R().X * Time.deltaTime * 100F;
                    if (entity.Platform.ZAxis) z = rotation;
                    RotationEulers.Set(x, y, z);

                    if (entity.Platform.CanMove)
                    {
                        entity.Transform.position = Vector3.MoveTowards(
                            entity.Transform.position,
                            target,
                            entity.Platform.MoveSpeed * Mathf.Abs(Player.Input[0].Gamepad.GetStick_L().X) * Time.deltaTime
                        );
                    }
                    if (entity.Platform.CanRotate)
                    {
                        entity.Transform.Rotate(RotationEulers);
                    }
                }
                else if (!InputManager.Instance.IsGamePadActive)
                {
                    float rotation = vertical * Time.deltaTime * 100F;
                    if (entity.Platform.XAxis) x = horizontal * Time.deltaTime * 100F;
                    if (entity.Platform.YAxis) y = vertical * Time.deltaTime * 100F;
                    if (entity.Platform.ZAxis) z = rotation;
                    RotationEulers.Set(x, y, z);

                    if (entity.Platform.CanMove)
                    {
                        entity.Transform.position = Vector3.MoveTowards(
                            entity.Transform.position,
                            target,
                            entity.Platform.MoveSpeed * Mathf.Abs(horizontal) * Time.deltaTime
                        );
                    }
                    if (entity.Platform.CanRotate)
                    {
                        entity.Transform.Rotate(RotationEulers);
                    }
                }
            }

        }

        CurrentTime += Time.deltaTime;
    }

    Vector3 ClampEulerRotation(Vector3 Rotation, Vector3 Max, Vector3 Min)
    {
        Rotation.x = Mathf.Clamp(Rotation.x, Min.x, Max.x);
        Rotation.y = Mathf.Clamp(Rotation.y, Min.y, Max.y);
        Rotation.z = Mathf.Clamp(Rotation.z, Min.z, Max.z);

        return Rotation;
    }

}
