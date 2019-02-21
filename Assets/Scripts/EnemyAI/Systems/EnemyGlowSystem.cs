using Unity.Entities;
using UnityEngine;
using static EnemyComponent;

class EnemyGlowSystem : ComponentSystem
{
    struct EnemyGroup
    {
        public EnemyGlowComponent Glow;
        //public EnemyVisionComponent Vision;
        public EnemyComponent EnemyComponent;
    }

    protected override void OnUpdate()
    {
        var entities = GetEntities<EnemyGroup>();
        foreach(var entity in entities)
        {
            var material = entity.Glow.EnemyMaterial;
            var color = entity.Glow.BaseColor;
            var speed = 0F;
            var maxEmission = entity.Glow.MaxEmission;
            var noPingPong = false;

            if (entity.EnemyComponent.State == EnemyState.Alert)
                speed = entity.Glow.AlertSpeed;
            else if (entity.EnemyComponent.State == EnemyState.Seek)
                speed = entity.Glow.SeekSpeed;
            else
                noPingPong = true;

            if (noPingPong)
                SetEmission(entity.Glow.BaseEmission, color, material);
            else
                EmissionPingPong(speed, maxEmission, color, material);
        }
    }

    void EmissionPingPong(float Speed, float MaxEmission, Color BaseColor, Material EnemyMaterial)
    {
        float emission = Mathf.PingPong(Time.time * Speed, MaxEmission);
        SetEmission(emission, BaseColor, EnemyMaterial);
    }

    void SetEmission(float Emission, Color BaseColor, Material EnemyMaterial)
    {
        Color finalColor = BaseColor * Mathf.LinearToGammaSpace(Emission);
        EnemyMaterial.SetColor("_EmissionColor", finalColor);
    }
}

