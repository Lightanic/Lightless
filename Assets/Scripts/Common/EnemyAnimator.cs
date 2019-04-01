using UnityEngine;

// Animator Script for Base Enemy class
// Any enemies with unique actions should inherit from this class
public class EnemyAnimator : MonoBehaviour
{

    protected Animator enemyAnimator;
    public bool isWalking;
    public bool isRunning;
    public bool isStunned;
    public bool isLunging;
    public bool isWaiting;
    

    protected virtual void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        isWalking = false;
        isRunning = false;
        isStunned = false;
        isLunging = false;
    }

    void Update()
    {
        enemyAnimator.SetBool("isWalking", isWalking);
        enemyAnimator.SetBool("isRunning", isRunning);
        enemyAnimator.SetBool("isStunned", isStunned);
        enemyAnimator.SetBool("isLunging", isLunging);
        enemyAnimator.SetBool("isWaiting", isWaiting);

    }
}
