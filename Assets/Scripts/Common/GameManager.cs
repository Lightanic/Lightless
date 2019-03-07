using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject deathScreen;
    [SerializeField]
    GameObject startScreen;

    Animator deathAnimator;

    private void Start()
    {
        deathScreen.SetActive(true);
        deathScreen = GameObject.Find("DeathScreen");
        deathAnimator = deathScreen.GetComponent<Animator>();
        startScreen.SetActive(true);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartDeath()
    {
        Camera.main.gameObject.GetComponent<CameraShake>().enabled = true;
        Camera.main.gameObject.GetComponent<CameraShake>().shakeDuration = 0.5f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        deathAnimator.SetBool("DeathStart", true);
    }
}
