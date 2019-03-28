using UnityEngine;

// Animator Script for Player Character
public class CharacterAnimator : MonoBehaviour {

    public Animator playerAnimator;
    public bool isWalking;
    public bool isSlowWalking;
    public bool isRunning;
    public bool equipped;
    LeftHandComponent lhComp;
	void Start () {
        //playerAnimator = GetComponent<Animator>();
        isWalking = false;
        isSlowWalking = false;
        isRunning = false;
        lhComp = gameObject.GetComponentInChildren<LeftHandComponent>();
    }
	
	void Update () {
        /////////////////// Integrate regular player control script/controls, right now they're mapped to really arbitrary keys

        //// WALK (W)
        //if (Input.GetKey(KeyCode.W))  
        //{
        //    isWalking = true;

        //    // SLOW WALK (W+Q)
        //    if (Input.GetKey(KeyCode.Q))
        //    {
        //        isSlowWalking = true;
        //    }
        //    if (Input.GetKeyUp(KeyCode.Q))
        //    {
        //        isSlowWalking = false;
        //    }

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

        //// INTERACT (for now: pick up & drop & throw) (SPACE)
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    playerAnimator.SetTrigger("Interact");
        //}
        //else if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    playerAnimator.SetTrigger("Dash");
        //}

        //// WALK/RUN RELEASE
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    isWalking = false;
        //    isSlowWalking = false;
        //    isRunning = false;
        //}
        equipped = !(lhComp.isEmpty);
        playerAnimator.SetBool("walk", isWalking);
        //playerAnimator.SetBool("isSlowWalking", isSlowWalking);
        playerAnimator.SetBool("run", isRunning);
        playerAnimator.SetBool("equipped", equipped);

    }
}
