using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{

    public enum EnemyType
    {
        Runner, Lunger, Stunner
    }
    public enum EnemyState
    {
        Alerted, Patrol, Seek
    }

    public EnemyType Type;
    public EnemyState State;

    public bool IsWalking;
    public bool IsRunning;
    public bool IsDead;

    public bool IsSeeking;
    public bool IsPatrolling;
    public bool IsAlerted;

    public bool IsLunging;
    public bool IsPreLunging;

    public bool IsStunned;
}
