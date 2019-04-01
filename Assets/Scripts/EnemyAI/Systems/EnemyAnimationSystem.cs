using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static EnemyComponent;

public class EnemyAnimationSystem : ComponentSystem
{
    private struct EnemyData
    {
        public EnemyComponent EnemyComponent;
        public EnemyAnimator EnemyAnimator;
    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<EnemyData>())
        {
            entity.EnemyAnimator.isWalking = false;
            entity.EnemyAnimator.isRunning = false;
            entity.EnemyAnimator.isStunned = false;
            entity.EnemyAnimator.isLunging = false;
            entity.EnemyAnimator.isWaiting = false;

            switch (entity.EnemyComponent.State)
            {
                case EnemyState.Patrol:
                    entity.EnemyAnimator.isWalking = true;
                    break;

                case EnemyState.Seek:
                    entity.EnemyAnimator.isRunning = true;
                    break;

                case EnemyState.Lunge:
                    entity.EnemyAnimator.isLunging = true;
                    break;

                case EnemyState.Stun:
                case EnemyState.Wait:
                    entity.EnemyAnimator.isStunned = true;
                    entity.EnemyAnimator.isWaiting = true;
                    break;

                default:
                    entity.EnemyAnimator.isWalking = true;
                    break;
            }
        }
    }


}
