using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Use this for initialization
    public int index = 0;
    int prevIndex = 0;
    [SerializeField] bool keyDown;
    [Header("Max index in a page")]
    [SerializeField] int indexPerPage;
    int maxIndex;
    public AudioSource audioSource;

    InputComponent inputComp;

    [Header("Diary Page object")]
    [SerializeField] DiaryPage diary;
    void Start()
    {
        inputComp = GameObject.Find("Player").GetComponent<InputComponent>();
        maxIndex = diary.CurrentPage * indexPerPage;
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputComp.Gamepad.GetButtonDown("LB"))
        {
            diary.PageTurnBack();
            index = (diary.CurrentPage - 1) * 9;
            maxIndex = diary.CurrentPage * indexPerPage;
        }
        else if (inputComp.Gamepad.GetButtonDown("RB"))
        {
            diary.PageTurnNext();
            index = (diary.CurrentPage - 1) * 9;
            maxIndex = diary.CurrentPage * indexPerPage;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (index < maxIndex)
                    {
                        index+=3;
                        if (index > maxIndex)
                            index = prevIndex;
                    }
                    else
                    {
                        index = (diary.CurrentPage-1) * 9;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > (diary.CurrentPage - 1) * 9)
                    {
                        index-=3;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
            prevIndex = index;
        }
        else if (Input.GetAxis("Horizontal") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    if (index < maxIndex)
                    {
                        index ++;
                    }
                    else
                    {
                        index = (diary.CurrentPage - 1) * 9;
                    }
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    if (index > (diary.CurrentPage - 1) * 9)
                    {
                        index --;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
            prevIndex = index;
        }
        else
        {
            keyDown = false;
        }
    }
}
