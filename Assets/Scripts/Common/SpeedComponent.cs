using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedComponent : MonoBehaviour {
    public float DEFAULT_SPEED = 2;
    public float SPRINT_SPEED = 3;
    public float MAX_STAMINA = 2;
    public float DODGE_SPEED = 20;
    public float MIN_STAMINA = 1;
    public float DodgeMultiplier = 2;
    public float RotationSpeed = 10;
    public float RotationFineControlSpeed = 2;
    public float StaminaRegenRate = 2;
    public float StaminaConsumptionRate = 2;

    public bool isSprinting = false;
    public bool isDodging = false;
    public bool canDodge = true;
    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public float Stamina = 2;

    public bool isMoving = false;
    public float walkTime = 0.15f;
    public float runTime = 0.2f;
    float curTime = 0;
    private void Start()
    {
        Stamina = MAX_STAMINA;
    }

    private void FixedUpdate()
    {
        Debug.Log(curTime);
        if (isMoving && !isSprinting)
        {
            if(curTime >= walkTime)
            {
                //AkSoundEngine.PostEvent("Step", gameObject);
                curTime = 0;
            }
            curTime += Time.fixedDeltaTime;
        }
        else if(isMoving && isSprinting)
        {
            if (curTime >= walkTime)
            {
                //AkSoundEngine.PostEvent("Step", gameObject);
                curTime = 0;
            }
            curTime += Time.fixedDeltaTime;
        }
        //StartCoroutine(WalkSound());
    }

    IEnumerator WalkSound()
    {
        AkSoundEngine.PostEvent("Step", gameObject);
        yield return new WaitForSeconds(2f); 
    }
}
