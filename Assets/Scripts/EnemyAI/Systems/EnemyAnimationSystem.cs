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
            entity.EnemyAnimator.DisableAllStates();
            switch (entity.EnemyComponent.State)
            {
                case EnemyState.Patrol:
                    entity.EnemyAnimator.SetState("isWalking", true);
                    break;

                case EnemyState.Seek:
                    entity.EnemyAnimator.SetState("isRunning", true);
                    break;

                case EnemyState.Lunge:
                    entity.EnemyAnimator.SetState("isLunging", true);
                    break;

                case EnemyState.Stun:
                case EnemyState.Wait:
                    entity.EnemyAnimator.SetState("isStunned", true);
                    entity.EnemyAnimator.SetState("isWaiting", true);
                    break;

                default:
                    entity.EnemyAnimator.SetState("isWalking", true);
                    break;
            }
        }
    }


}
