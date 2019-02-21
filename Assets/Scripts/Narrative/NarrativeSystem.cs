using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class NarrativeSystem : ComponentSystem {

    /// <summary>
    /// Entities taht can be picked up
    /// </summary>
    struct Group
    {
        public Transform Transform;
        public NarrativePickup PickItem;
    }


    /// <summary>
    /// Player data
    /// </summary>
    private struct Player
    {
        readonly public int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<InputComponent> InputComponents;
        public ComponentArray<CharacterAnimator> Animators;
        public ComponentArray<PlayerProperties> props;
    }
    [Inject] private Player playerData;

    struct CanvasGroup
    {
        public NarrativeCanvas canvas;
    }

    NarrativePickup currentPickup = null;
    NarrativeCanvas canvas = null;
    bool canvasOn = false;
    protected override void OnUpdate()
    {
        Vector3 playerPos = playerData.Transform[0].position;
        var animator = playerData.Animators[0];

        foreach (var entity in GetEntities<CanvasGroup>())
        {
            if (entity.canvas.isDisplayed && playerData.InputComponents[0].Control("Back"))
            {
                canvasOn = true;
                canvas = entity.canvas;
            }
        }

        foreach (var entity in GetEntities<Group>())
        {
            if (Vector3.Distance(playerPos, entity.Transform.position) <= entity.PickItem.InteractDistance && (playerData.InputComponents[0].Control("Interact")))
            {
                currentPickup = entity.PickItem;
                PlayerProperties.narrativePickups.Add(entity.PickItem.index,entity.PickItem);
                entity.PickItem.obtained = true;
            }

        }

        if (currentPickup != null)
        {
            currentPickup.Show();
            currentPickup = null;
        }
        if (canvasOn)
        {
            canvas.ToggleOff();
            canvasOn = false;
        }
    }

}
