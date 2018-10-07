using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndGame : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;
    bool buttonSelected = false;
    [SerializeField]
    GameObject EndGameScreen;

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 )
        {
            //eventSystem.SetSelectedGameObject(selectedObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {
            EndGameScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
   
}
