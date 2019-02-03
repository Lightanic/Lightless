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
        Alert, Patrol, Seek, Lunge, Stun, Wait, Dead
    }

    public EnemyType Type;
    public EnemyState State;

    public bool IsWalking;
    public bool IsRunning;

    public bool HasLunged;
    public bool CanLunge;

    public bool IsStunned;
}
