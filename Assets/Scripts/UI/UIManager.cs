using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    EventSystem eventSystem;
    InputComponent inputComp;

    bool buttonSelected = false;
    public static bool isPaused = false;

    [Header("Pause Menu Canvas")]
    [SerializeField]
    GameObject PauseCanvas;

    [Header("Pause start button")]
    [SerializeField]
    GameObject PauseStartButton;
    GameObject selectedButton;

    [Header("Controller Menu Canvas")]
    [SerializeField]
    GameObject ControllerCanvas;

    [Header("Diary Menu Canvas")]
    [SerializeField]
    GameObject DiaryCanvas;

    [Header("Options Menu Canvas")]
    [SerializeField]
    GameObject OptionsCanvas;

    [Header("Options start button")]
    [SerializeField]
    GameObject optionsStartButton;

    [Header("HUD Canvas")]
    [SerializeField]
    GameObject HUDCanvas;

    enum CurrentMenu
    {
        None,
        Pause,
        Diary,
        Options,
        Controller,
        HUD
    };

    CurrentMenu currentMenu;
    CurrentMenu prevMenu;

    Dictionary<CurrentMenu, GameObject> Menus = new Dictionary<CurrentMenu, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1F;
        isPaused = false;
        eventSystem = EventSystem.current;
        inputComp = GameObject.Find("Player").GetComponent<InputComponent>();
        selectedButton = PauseStartButton;
        currentMenu = CurrentMenu.None; // previously HUD 
        prevMenu = CurrentMenu.None;

        Menus.Add(CurrentMenu.None, null);
        Menus.Add(CurrentMenu.Pause, PauseCanvas);
        Menus.Add(CurrentMenu.Diary, DiaryCanvas);
        Menus.Add(CurrentMenu.Options, OptionsCanvas);
        Menus.Add(CurrentMenu.Controller, ControllerCanvas);
        Menus.Add(CurrentMenu.HUD, HUDCanvas);
    }

    private void OnEnable()
    {
        Time.timeScale = 1F;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(inputComp.Control("Escape"))
        {
            isPaused = !isPaused;
            if (!isPaused)
            {
                currentMenu = CurrentMenu.None; // previously HUD 
            }
            else
            {
                currentMenu = CurrentMenu.Pause;
            }
            
        }
        if (isPaused)
        {
            Time.timeScale = 0.0f;
            BackButtonPress();
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        DisplayMenu();
        SetActiveButton();
    }
    void SetActiveButton()
    {
        if (PauseCanvas.active)
        {
            if (Input.GetAxisRaw("Vertical") != 0 && !buttonSelected)
            {
                eventSystem.SetSelectedGameObject(selectedButton);
                buttonSelected = true;
            }
        }
    }

    void DisplayMenu()
    {
        if(currentMenu != prevMenu)
        {
            switch (currentMenu)
            {
                case CurrentMenu.None:
                    {
                        PauseCanvas.SetActive(false);
                        Time.timeScale = 1.0f;
                        isPaused = false;
                        break;
                    }
                case CurrentMenu.HUD:
                    {
                        HUDCanvas.SetActive(true);
                        PauseCanvas.SetActive(false);
                        Time.timeScale = 1.0f;
                        isPaused = false;
                        break;
                    }
                case CurrentMenu.Pause:
                    {
                        PauseCanvas.SetActive(true);
                        eventSystem.SetSelectedGameObject(selectedButton);
                        break;
                    }
                case CurrentMenu.Diary:
                    {
                        DiaryCanvas.SetActive(true);
                        break;
                    }
                case CurrentMenu.Controller:
                    {
                        ControllerCanvas.SetActive(true);
                    }
                    break;
                case CurrentMenu.Options:
                    {
                        OptionsCanvas.SetActive(true);
                        eventSystem.SetSelectedGameObject(optionsStartButton);
                        break;
                    }
            }
            CloseInactiveMenus();
            prevMenu = currentMenu;
            eventSystem.UpdateModules();
        }
    }

    public void ResumeGame()
    {
        currentMenu = CurrentMenu.HUD;
    }

    public void OpenPauseMenu()
    {
        currentMenu = CurrentMenu.Pause;
    }

    public void OpenControllerMenu()
    {
        currentMenu = CurrentMenu.Controller;
    }

    public void OpenDiaryMenu()
    {
        currentMenu = CurrentMenu.Diary;
    }

    public void OpenOptionsMenu()
    {
        currentMenu = CurrentMenu.Options;
    }

    public void ExitGame()
    {
        //Application.Quit();
      // PauseCanvas.GetComponent<LevelLoader>().LoadLevelAsync(0);
       SceneManager.LoadScene(0, LoadSceneMode.Single);
       //SceneManager.LoadScene("Menu");
    }

    void BackButtonPress()
    {
        if(inputComp.Control("Back") && currentMenu != CurrentMenu.Pause )
        {
            OpenPauseMenu();
        }
        else if(inputComp.Control("Back") && currentMenu == CurrentMenu.Pause )
        {
            ResumeGame();
        }
    }

    void CloseInactiveMenus()
    {
        foreach(var menu in Menus)
        {
            if(menu.Key != currentMenu && menu.Value != null)
            {
                if (menu.Key == CurrentMenu.HUD)
                {
                    continue;
                }
                menu.Value.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
