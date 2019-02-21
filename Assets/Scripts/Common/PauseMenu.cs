using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    bool buttonSelected = false;

    //pause check
    public static bool isPaused = false;
    [Header("Pause Menu Canvas")]
    [SerializeField]
    GameObject PauseCanvas;

    [Header("Pause start button")]
    [SerializeField]
    GameObject PauseStartButton;

    [SerializeField] GameObject Hud;
    [SerializeField] GameObject EndScreen;
    [SerializeField] GameObject Diary;

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetAxisRaw("Horizontal") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }

    public void Pause()
    {
        Hud.SetActive(false);
        EndScreen.SetActive(false);
        // Diary test
        Diary.SetActive(true);
        //pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }
    
    public void Resume()
    {
        
        Hud.SetActive(true);
        EndScreen.SetActive(false);
        //fDiary Test
        Diary.SetActive(false);
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Pressed");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
