using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
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

    enum CurrentMenu
    {
        None,
        Pause,
        Diary,
        Options,
        Controller
    };

    CurrentMenu currentMenu;

    Dictionary<CurrentMenu, GameObject> Menus = new Dictionary<CurrentMenu, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        inputComp = GameObject.Find("Player").GetComponent<InputComponent>();
        selectedButton = PauseStartButton;
        currentMenu = CurrentMenu.None;

        Menus.Add(CurrentMenu.None, null);
        Menus.Add(CurrentMenu.Pause, PauseCanvas);
        Menus.Add(CurrentMenu.Diary, DiaryCanvas);
        Menus.Add(CurrentMenu.Options, OptionsCanvas);
        Menus.Add(CurrentMenu.Controller, ControllerCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        if(inputComp.Control("Escape"))
        {
            isPaused = !isPaused;
            if (!isPaused)
            {
                currentMenu = CurrentMenu.None;
                selectedButton.GetComponent<Animator>().SetBool("deselect", true);
                selectedButton.GetComponent<Animator>().SetBool("selected", false);
                selectedButton.GetComponent<Animator>().SetBool("pressed", false);
            }
            else
            {
                currentMenu = CurrentMenu.Pause;
                eventSystem.SetSelectedGameObject(PauseStartButton);
                PauseStartButton.GetComponent<Animator>().SetBool("deselect", false);
                PauseStartButton.GetComponent<Animator>().SetBool("selected", true);
                PauseStartButton.GetComponent<Animator>().SetBool("pressed", false);
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
        switch(currentMenu)
        {
            case CurrentMenu.None:
                {
                    PauseCanvas.SetActive(false);
                    Time.timeScale = 1.0f;
                    isPaused = false;
                    break;
                }
            case CurrentMenu.Pause:
                {
                    PauseCanvas.SetActive(true);
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
                    break;
                }
        }
        CloseInactiveMenus();
    }

    public void ResumeGame()
    {
        currentMenu = CurrentMenu.None;
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
        SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).name);
    }

    void BackButtonPress()
    {
        if(inputComp.Control("Back"))
        {
            OpenPauseMenu();
        }
    }

    void CloseInactiveMenus()
    {
        foreach(var menu in Menus)
        {
            if(menu.Key != currentMenu && menu.Value != null)
            {
                menu.Value.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
