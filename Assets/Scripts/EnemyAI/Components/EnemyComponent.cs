using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyComponent : MonoBehaviour
{
    [Header("Stun particle prefab")]
    public GameObject stunParticle;
    GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }
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

    public bool IsTargetInView;

    public float CurrentTime = 0;
    public float WaitTime = 2;
    public float LungeTime = 1;
    public float AttackTime = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GetComponent<EnemyDeathComponent>().EnemyIsDead)
        {
            gameManager.GetComponent<GameManager>().StartDeath();
        }
    }

}
