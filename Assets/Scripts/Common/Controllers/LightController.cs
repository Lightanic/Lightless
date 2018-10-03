using UnityEngine;
using Unity.Entities;

public class LightController : ComponentSystem {
    private struct Lights
    {
        public LightComponent light;
    }

    /// <summary>
    /// Player data
    /// </summary>
    private struct Player
    {
        readonly public int Length;
        public ComponentArray<InputComponent> InputComponents;
    }
    [Inject] private Player playerData;

    protected override void OnUpdate()
    {
        foreach(var entities in GetEntities<Lights>())
        {
           if(playerData.InputComponents[0].Control("LeftLightToggle") && "LeftHand" == entities.light.GetParent())
            {
                entities.light.ToggleLightOn();
            }

            if (playerData.InputComponents[0].Control("RightLightToggle") && "RightHand" == entities.light.GetParent())
            {
                entities.light.ToggleLightOn();
            }
        }
    }

}
