using System.Collections.Generic;
using UnityEngine;

// Animator Script for Base Enemy class
// Any enemies with unique actions should inherit from this class
public class EnemyAnimator : MonoBehaviour
{

    protected Animator enemyAnimator;
    private Dictionary<string, bool> animationStates = new Dictionary<string, bool>();
    private Dictionary<string, bool> animatorStates = new Dictionary<string, bool>();
    private string[] animStates = { "isWalking", "isRunning", "isStunned", "isLunging" };

    protected virtual void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        foreach(var param in enemyAnimator.parameters)
        {
            animatorStates.Add(param.name, false);
        }

        foreach(var state in animStates)
        {
            animationStates.Add(state, false);
        }
    }

    void Update()
    {
        foreach(var param in animatorStates)
        {
            bool status;
            if(!animatorStates.TryGetValue(param.Key, out status))
            {
                status = false;
            }

            enemyAnimator.SetBool(param.Key, status);
        }

    }

    public void DisableAllStates()
    {
        foreach (var param in animatorStates)
        {
            enemyAnimator.SetBool(param.Key, false);
        }
    }

    public void SetState(string state, bool enable)
    {
        animationStates[state] = enable;
    }

}
