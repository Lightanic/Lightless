using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunComponent : MonoBehaviour
{
    public bool IsStunned = false;
    public bool IsWaiting = false;
    public bool IsSeekingPlayer = false;
    public float WaitTime;
    public GameObject flashlight;

    private void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("Flashlight");
    }
    private void Update()
    {
        if (flashlight.gameObject.activeInHierarchy == false)
            IsStunned = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Flashlight")
        {
                if (other.GetComponent<LightComponent>().LightIsOn == true)
                {
                    IsStunned = true;
                }

            //Debug.Log("You look stunning!");
        }

        else if (other.gameObject.tag == "Player")
        {
            IsSeekingPlayer = true;
        }

        //else
        //{
        //    IsStunned = false;
        //}
        
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Flashlight")
        {
            IsStunned = false;
            Debug.Log("I am coming after you!");
        }
    }

}
