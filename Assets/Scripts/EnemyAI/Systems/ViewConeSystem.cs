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
                entity.ViewConeComponent.ViewRadius, entity.ViewConeComponent.TargetMask, QueryTriggerInteraction.Collide);

            
            Vector3 enemyPosWithOffset = entity.EnemyTransform.position + entity.ViewConeComponent.EyeOffset;
            for (int i = 0; i < targetsInView.Length; ++i)
            {
                Transform target = targetsInView[i].transform;
                Vector3 targetPos = target.position;
                targetPos.y = enemyPosWithOffset.y;
                Vector3 dirToTarget = (targetPos - enemyPosWithOffset).normalized;
                Vector3 actualDirToTarget = (target.position - enemyPosWithOffset).normalized;
                if (Vector3.Angle(entity.EnemyTransform.forward, dirToTarget) < entity.ViewConeComponent.ViewAngle / 2)
                {
                    bool shouldRaycast = false;
                    float distToTarget = Vector3.Distance(enemyPosWithOffset, target.position);
                    if (target.CompareTag("Lantern") || target.CompareTag("Flashlight"))
                    {
                        if (target.GetComponent<LightComponent>().LightIsOn)
                            shouldRaycast = true;
                    }
                    if (target.CompareTag("PlayerBodyMesh") || target.CompareTag("FireStun") || target.CompareTag("Player"))
                        shouldRaycast = true;
                    RaycastHit hit;

                    Debug.DrawRay(enemyPosWithOffset, actualDirToTarget, Color.red);
                    Debug.DrawLine(enemyPosWithOffset, target.position, Color.blue);

                    if (shouldRaycast && Physics.Raycast(enemyPosWithOffset, actualDirToTarget, out hit, distToTarget + 1.0f, entity.ViewConeComponent.TargetMask | entity.ViewConeComponent.ObstacleMask, QueryTriggerInteraction.Collide))
                    {
                        if (hit.collider.CompareTag("Lantern") || hit.collider.CompareTag("Flashlight") || hit.collider.CompareTag("FireStun")
                            || hit.collider.CompareTag("PlayerBodyMesh") || hit.collider.CompareTag("Player"))
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
