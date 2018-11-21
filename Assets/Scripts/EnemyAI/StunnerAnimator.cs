using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnerAnimator : MonoBehaviour
{
    protected Animator enemyAnimator;
    public bool isWalking;
    public bool isRunning;

    // for STUNNER ONLY
    //public bool stunner = false;       // debug bool to determine if this is a stunner
    public bool isStunned;

    protected virtual void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        isWalking = false;
        isRunning = false;
        isStunned = false;
    }


    // Update is called once per frame
    void Update ()
    {

        enemyAnimator.SetBool("isWalking", isWalking);
        enemyAnimator.SetBool("isRunning", isRunning);

        // stunner
        enemyAnimator.SetBool("isStunned", isStunned);
    }
}
