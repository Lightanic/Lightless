using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject deathScreen;
    Animator deathAnimator;

    private void Start()
    {
        deathScreen.SetActive(true);
        deathScreen = GameObject.Find("DeathScreen");
        deathAnimator = deathScreen.GetComponent<Animator>();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartDeath()
    {
        deathAnimator.SetBool("DeathStart", true);
    }
}
