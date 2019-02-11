using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyComponent;

public class EnemyStunComponent : MonoBehaviour
{
    public bool IsStunned = false;
    public bool IsWaiting = false;
    public bool IsSeekingPlayer = false;
    public float WaitTime;
    public GameObject flashlight;
    bool triggered = false;
    Collider other;

    private void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("Flashlight");
    }
    private void Update()
    {
        //bool StunFlag = false;
        //foreach (var fire in GameObject.FindGameObjectsWithTag("FireStun"))
        //{
        //    StunFlag = true;
        //}
        //if (!StunFlag)
        //{
        //   // IsStunned = false;
        //}
        //if (flashlight.gameObject.activeInHierarchy == false)
        //    IsStunned = false;

        if (triggered && !other || (triggered && !other.gameObject.activeInHierarchy))
        {
            triggered = false;
            IsStunned = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.tag);
        this.other = other;
        triggered = true;
        IsStunned = false;
        if (other.gameObject.tag == "Flashlight")
        {
            if (other.GetComponent<LightComponent>().LightIsOn == true)
            {
                this.GetComponent<EnemyComponent>().State = EnemyState.Stun;
                IsStunned = true;
            }

            //Debug.Log("You look stunning!");
        }
        if (other.gameObject.CompareTag("FireStun"))
        {
            this.GetComponent<EnemyComponent>().State = EnemyState.Stun;
            IsStunned = true;
        }

        //else if (other.gameObject.tag == "Player")
        //{
        //    IsSeekingPlayer = true;
        //}

        //else
        //{
        //    IsStunned = false;
        //}
        
        

    }

    private void OnTriggerExit(Collider other)
    {
        IsStunned = false;
        //if (other.gameObject.tag == "Flashlight")
        //{
            
        //    Debug.Log("I am coming after you!");
        //}
    }

}
