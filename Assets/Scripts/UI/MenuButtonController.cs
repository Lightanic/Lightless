using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour {

	// Use this for initialization
	public int index;
	[SerializeField] bool keyDown;
	[SerializeField] int maxIndex;
	public AudioSource audioSource;

    [SerializeField] GameObject creditScene;
    LevelLoader loadingScreen;
    void Start () {
        loadingScreen = GetComponent<LevelLoader>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
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
            loadingScreen.LoadLevelAsync(1);
        else if (index == 1)    // Credits button
            creditScene.SetActive(true);
        else if (index == 2)    // Quit Button
            Application.Quit();
    }

}
