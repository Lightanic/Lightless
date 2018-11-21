using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyVisionComponent : MonoBehaviour
{
    public float Value = 3f;
<<<<<<< HEAD
=======
    public float AlertValue = 3f;
    public bool IsAlerted = false;
    public bool IsSeeking = false;
>>>>>>> Develop

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Value);
    }

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.tag == "Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
=======
        if (other.gameObject.tag == "Player" && !GetComponent<EnemyDeathComponent>().EnemyIsDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
>>>>>>> Develop
    }
}
