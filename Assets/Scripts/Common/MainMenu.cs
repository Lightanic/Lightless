using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    bool buttonSelected = false;

    void Update()
    {
    
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }

    public void Play()
    {
        var checkpointManager = GameObject.Find("CheckpointManager");
        if(checkpointManager!=null)
        {
            //Destroy(checkpointManager);
            checkpointManager.GetComponent<CheckpointManager>().ResetScene();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PauseMenu.isPaused = false;
        Debug.Log("Play");
    }

    //quit
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

