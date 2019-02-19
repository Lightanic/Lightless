using UnityEngine;

// Animator Script for Base Enemy class
// Any enemies with unique actions should inherit from this class
public class EnemyAnimator : MonoBehaviour
{

    protected Animator enemyAnimator;
    public bool isWalking;
    public bool isRunning;
    public bool isStunned;

    protected virtual void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        isWalking = false;
        isRunning = false;
        isStunned = false;
    }

    void Update()
    {
        /////////////////// Integrate enemy behaviors control script/controls, right now they're mapped to really arbitrary keys

        //// WALK (W)
        //if (Input.GetKey(KeyCode.W))
        //{
        //    isWalking = true;
            
        //    // RUN (W+LShift)
        //    if (Input.GetKey(KeyCode.LeftShift))
        //    {
        //        isRunning = true;
        //    }
        //    if (Input.GetKeyUp(KeyCode.LeftShift))
        //    {
        //        isRunning = false;
        //    }
        //}

        //// ATTACK (SPACE)
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    enemyAnimator.SetTrigger("Attack");
        //}

        //// WALK/RUN RELEASE
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    isWalking = false;
        //    isRunning = false;
        //}

        enemyAnimator.SetBool("isWalking", isWalking);
        enemyAnimator.SetBool("isRunning", isRunning);
        enemyAnimator.SetBool("isStunned", isStunned);

    }
}
