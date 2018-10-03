﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Play()
    {
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

