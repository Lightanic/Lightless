using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	//[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
    
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (menuButtonController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
			if(Input.GetAxis ("Submit") == 1)
            {
				animator.SetBool ("pressed", true);
                menuButtonController.ButtonActions(thisIndex);
			}
            else if (animator.GetBool ("pressed"))
            {
				animator.SetBool ("pressed", false);
				//animatorFunctions.disableOnce = true;
			}
		}else{
			animator.SetBool ("selected", false);
			animator.SetBool ("pressed", false);
		}
    }

    public void PressedFalse()
    {
        animator.SetBool("selected", false);
        animator.SetBool("pressed", false);
    }
}
