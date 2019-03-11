using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour {

    [Header("WWISE Global")]
    [SerializeField]
    GameObject wwiseGlobal;

    [Space]
	// Use this for initialization
	public int index;
	public int prevIndex;
	[SerializeField] bool keyDown;
	[SerializeField] int maxIndex;
	public AudioSource audioSource;

    [SerializeField] GameObject creditScene;
    [SerializeField] GameObject menuButtons;
    [SerializeField] Animator MenuAnim;

    [SerializeField] LevelLoader loadingScreen;
    void Start () {
        //loadingScreen = GetComponent<LevelLoader>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        prevIndex = index;
        if(Input.GetButtonDown("Cancel") && creditScene.active)
        {
            AkSoundEngine.PostEvent("Play_MenuSelect", gameObject);
            StartCoroutine(fade());
            menuButtons.SetActive(true);
            creditScene.SetActive(false);
        }
        if (!creditScene.active)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                if (!keyDown)
                {
                    if (Input.GetAxis("Vertical") < 0)
                    {
                        if (index < maxIndex)
                        {
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }
                    }
                    else if (Input.GetAxis("Vertical") > 0)
                    {
                        if (index > 0)
                        {
                            index--;
                        }
                        else
                        {
                            index = maxIndex;
                        }
                    }
                    keyDown = true;
                }
            }
            else
            {
                keyDown = false;
            }
        }
	}

    public void ButtonActions(int index)
    {
        if (index == 0)         // PlayButton
        {
            AkSoundEngine.PostEvent("Play_MenuSelect", gameObject);
            var checkpointManager = GameObject.Find("CheckpointManager");
            if(checkpointManager)
            {
                var comp = checkpointManager.GetComponent<CheckpointManager>();
                comp.SetLatestCheckpoint(null);
            }
            MenuAnim.SetBool("Exit", true);
        }
        else if (index == 1)    // Credits button
        {
            AkSoundEngine.PostEvent("Play_MenuSelect", gameObject);
            creditScene.SetActive(true);
            foreach (var g in menuButtons.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                g.color = new Color(255, 255, 255, 0);
            }
        }
        else if (index == 2)    // Quit Button
        {
            AkSoundEngine.PostEvent("Play_MenuSelect", gameObject);
            Application.Quit();
        }
    }

    IEnumerator fade()
    {
        foreach (var g in menuButtons.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            while (g.color.a <= 1)
            {
                g.color = new Color(255, 255, 255, g.color.a + Time.deltaTime);
                yield return null;
            }
        }
    }
}
