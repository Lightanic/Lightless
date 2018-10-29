using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject Canvas;
    public float offsetHeight = 2.5f;
    Quaternion rotation;

    public Sprite sprite = null;
    bool tutorialPlaying = false;
    InputComponent inputComp = null;

    // Use this for initialization
    void Start () {
        rotation = Canvas.transform.rotation;
        var player = GameObject.Find("Player");
        inputComp = player.GetComponent<InputComponent>();
    }
	
	// Update is called once per frame
	void Update () {
        Canvas.transform.LookAt(Camera.main.transform);
		if(Input.GetKeyDown(KeyCode.Return) || inputComp.Gamepad.GetButtonDown("A"))
        {
            Time.timeScale = 1;
            ToggleOff();
        }
	}

    public void RePosition(Vector3 pos)
    {
        Canvas.transform.rotation = rotation;
        Canvas.transform.position = pos + new Vector3(0, offsetHeight, 0);
    }

    public void ToggleOn(Sprite newSprite)
    {
        Time.timeScale = 0.0f;

        if (newSprite != null)
            Canvas.GetComponentInChildren<Image>().sprite = newSprite;
        else
            Canvas.GetComponentInChildren<Image>().sprite = sprite;
        Canvas.SetActive(true);
    }

    public void ToggleOff()
    {
        Canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            RePosition(transform.position);
            ToggleOn(sprite);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //RePosition();
            ToggleOff();
        }
    }
}
