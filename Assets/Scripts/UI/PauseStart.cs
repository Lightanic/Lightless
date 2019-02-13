using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseStart : MonoBehaviour
{
    [Header("Pause start button")]
    [SerializeField]
    GameObject PauseStartButton;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(PauseStartButton);
        PauseStartButton.GetComponent<Animator>().SetBool("deselect", false);
        PauseStartButton.GetComponent<Animator>().SetBool("selected", true);
        PauseStartButton.GetComponent<Animator>().SetBool("pressed", false);
    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(PauseStartButton);
        anim = PauseStartButton.GetComponent<Animator>();
        anim.SetBool("deselect", false);
        anim.SetBool("selected", true);
        anim.SetBool("pressed", false);
    }
    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == PauseStartButton && anim.GetBool("selected") != true && anim.GetBool("deselect") == true)
        {
            EventSystem.current.SetSelectedGameObject(PauseStartButton);
            anim.SetBool("deselect", false);
            anim.SetBool("selected", true);
            anim.SetBool("pressed", false);
        }
    }
}
