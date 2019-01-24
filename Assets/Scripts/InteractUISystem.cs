using UnityEngine;
using Unity.Entities;

public class InteractUISystem : ComponentSystem
{
    /// <summary>
    /// Player data
    /// </summary>
    private struct Player
    {
        readonly public int Length;
        public ComponentArray<Transform> Transform;
        public ComponentArray<InputComponent> InputComponents;
        public ComponentArray<CharacterAnimator> Animators;
    }
    [Inject] private Player playerData;

    /// <summary>
    /// Entities taht can be picked up
    /// </summary>
    private struct PickupUI
    {
        public Transform Transform;
        public InteractUIComponent item;
    }

    bool uiEnabled = false;

    protected override void OnUpdate()
    {
        Vector3 playerPos = playerData.Transform[0].position;

        foreach (var entity in GetEntities<PickupUI>())
        {
            GameObject obj = entity.item.gameObject;
            var dist = Vector3.Distance(playerPos, entity.Transform.position);
            if (dist <= entity.item.ShowDistance)
            {
                var pickup = obj.GetComponent<Pickup>();
                if ( pickup != null)
                {
                    if (!pickup.IsEquiped)
                    {
                        entity.item.RePosition(entity.Transform.position);

                        if (entity.item.sprite != null)
                            entity.item.ToggleOn(entity.item.sprite);

                        uiEnabled = true;
                    }
                }
                else
                {
                        entity.item.RePosition(entity.Transform.position);

                        if (entity.item.sprite != null)
                            entity.item.ToggleOn(entity.item.sprite);

                        uiEnabled = true;
                }
            }
            else if (!uiEnabled)
            {
                entity.item.ToggleOff();
            }
        }

        uiEnabled = false;
    }

}
