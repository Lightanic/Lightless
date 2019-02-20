using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    [Header("Resolution Dropdown object")]
    [SerializeField]
    TMPro.TMP_Dropdown resolutionDropdown;

    [Header("Quality Dropdown object")]
    [SerializeField]
    TMPro.TMP_Dropdown qualityDropdown;

    EventSystem eventSystem;

    [Header(" Options menu first selected")]
    [SerializeField]
    GameObject selectedObject;
    GameObject selectedObjectDefault;

    bool buttonSelected = false;
    InputComponent inputComp;


    private void Start()
    {
        eventSystem = EventSystem.current;
        inputComp = GameObject.Find("Player").GetComponent<InputComponent>();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> res = new List<string>();
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string r = resolutions[i].width + "x" + resolutions[i].height;
            res.Add(r);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResIndex = i;
        }
        resolutionDropdown.AddOptions(res);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
        Screen.SetResolution(resolutions[currentResIndex].width, resolutions[currentResIndex].height, Screen.fullScreen);

        qualityDropdown.value =(int) QualitySettings.GetQualityLevel();

        selectedObjectDefault = selectedObject;
        EventSystem.current.SetSelectedGameObject(selectedObjectDefault);
        //eventSystem.SetSelectedGameObject(selectedObjectDefault);
    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(selectedObjectDefault);
        Destroy(GameObject.Find("Blocker"));
    }
    private void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(selectedObjectDefault);
        }
        if (Input.GetAxisRaw("Vertical") != 0 && !buttonSelected)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }
    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width,resolutions[index].height, Screen.fullScreen);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetVolume(float value)
    {
        // Set master volume
        //SoundEngine.SetRTPCValue("MasterVolume", value);
    }

    private void OnDisable()
    {
        qualityDropdown.onValueChanged.RemoveAllListeners();
        eventSystem.UpdateModules();
        if (resolutionDropdown.transform.childCount > 3)
        {
            var a = resolutionDropdown.transform.GetChild(3);
            if (a != null)
                Destroy(a.gameObject);
        }
        if (qualityDropdown.transform.childCount > 3)
        {
            var a = qualityDropdown.transform.GetChild(3);
            if (a != null)
                Destroy(a.gameObject);
        }
        //Destroy(GameObject.Find("Blocker"));
        buttonSelected = false;
    }
}
