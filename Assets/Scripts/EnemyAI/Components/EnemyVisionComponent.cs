using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyVisionComponent : MonoBehaviour
{
    public float Value = 3f;
    public float AlertValue = 8f;
    public bool IsAlerted = false;
    public bool IsSeeking = false;

    GameObject gameManager;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GetComponent<EnemyDeathComponent>().EnemyIsDead)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Camera.main.gameObject.GetComponent<CameraShake>().enabled = true;
            Camera.main.gameObject.GetComponent<CameraShake>().shakeDuration = 0.5f;

            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameManager.GetComponent<GameManager>().StartDeath();
        }
    }
}
