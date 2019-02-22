using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour {

    [Header("WWISE Global")]
    [SerializeField]
    GameObject wwiseGlobal;

    [Space]
	// Use this for initialization
	public int index;
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
        if(Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(fade());
            menuButtons.SetActive(true);
            creditScene.SetActive(false);
        }
		if(Input.GetAxis ("Vertical") != 0){
			if(!keyDown){
				if (Input.GetAxis ("Vertical") < 0) {
					if(index < maxIndex){
						index++;
					}else{
						index = 0;
					}
				} else if(Input.GetAxis ("Vertical") > 0){
					if(index > 0){
						index --; 
					}else{
						index = maxIndex;
					}
				}
				keyDown = true;
			}
		}else{
			keyDown = false;
		}
	}

    public void ButtonActions(int index)
    {
        if (index == 0)         // PlayButton
        {
            //AkSoundEngine.PostEvent("Start_Gameplay", wwiseGlobal);
            AkSoundEngine.PostEvent("Start_Ambiance", wwiseGlobal);
            MenuAnim.SetBool("Exit", true);
        }
        else if (index == 1)    // Credits button
        {
            creditScene.SetActive(true);
            foreach(var g in menuButtons.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                g.color = new Color(255, 255, 255, 0);
            }
        }
        else if (index == 2)    // Quit Button
            Application.Quit();
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
