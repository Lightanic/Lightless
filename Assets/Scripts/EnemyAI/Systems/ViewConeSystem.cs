using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ViewConeSystem : ComponentSystem
{
    private struct Enemy
    {
        public EnemyComponent EnemyComponent;
        public SeekComponent SeekComponent;
        public ViewConeComponent ViewConeComponent;
        public Transform EnemyTransform;

    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Enemy>())
        {
            entity.EnemyComponent.IsTargetInView = false;
            Collider[] targetsInView = Physics.OverlapSphere(entity.EnemyTransform.position, 
                entity.ViewConeComponent.ViewRadius, entity.ViewConeComponent.TargetMask);

            for (int i = 0; i < targetsInView.Length; ++i)
            {
                Transform target = targetsInView[i].transform;
                Vector3 dirToTarget = (target.position - entity.EnemyTransform.position).normalized;
                if (Vector3.Angle(entity.EnemyTransform.forward, dirToTarget) < entity.ViewConeComponent.ViewAngle / 2)
                {
                    bool shouldRaycast = false;
                    float distToTarget = Vector3.Distance(entity.EnemyTransform.position, target.position);
                    if (target.CompareTag("Lantern") || target.CompareTag("Flashlight"))
                    {
                        if (target.GetComponent<LightComponent>().LightIsOn)
                            shouldRaycast = true;
                    }
                    if (target.CompareTag("PlayerBodyMesh"))
                        shouldRaycast = true;
                    RaycastHit hit;
          
                    
                    if (shouldRaycast && Physics.Raycast(entity.EnemyTransform.position, dirToTarget, out hit, distToTarget + 1))
                    {
                        if (hit.collider.CompareTag("Lantern") || hit.collider.CompareTag("Flashlight")
                            || hit.collider.CompareTag("PlayerBodyMesh"))
                        {
                            entity.EnemyComponent.IsTargetInView = true;
                            entity.SeekComponent.Target = target;
                        }
                    
                    }
                }
            }
        }
    }


}
