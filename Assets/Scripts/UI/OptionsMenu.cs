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

    [Header("Event System")]
    [SerializeField]
    EventSystem eventSystem;

    [Header(" Options menu first selected")]
    [SerializeField]
    GameObject selectedObject;
    GameObject selectedObjectDefault;

    bool buttonSelected = false;

    private void Start()
    {
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
        //eventSystem.SetSelectedGameObject(selectedObjectDefault);
    }
    private void OnEnable()
    {
        eventSystem.SetSelectedGameObject(selectedObjectDefault);
    }
    private void Update()
    {
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
}
