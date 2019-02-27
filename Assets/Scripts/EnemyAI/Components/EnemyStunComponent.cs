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
    public float currentTime;
    public GameObject flashlight;
    bool triggered = false;
    Collider other;

    private void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("Flashlight");
    }
    private void Update()
    {
        if (triggered && !other || (triggered && !other.gameObject.activeInHierarchy))
        {
            triggered = false;
            IsStunned = false;
        }
    }

    private void OnTriggerStay(Collider other)
    { 
        this.other = other;
        triggered = true;
        IsStunned = false;
        if (other.gameObject.tag == "Flashlight")
        {
            if (other.GetComponent<LightComponent>().LightIsOn == true)
            {
               
                IsStunned = true;
                transform.LookAt(other.transform);
            }

            //Debug.Log("You look stunning!");
        }
        if (other.gameObject.CompareTag("FireStun"))
        {
           
            IsStunned = true;
            transform.LookAt(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsStunned = false;
      
    }

}
