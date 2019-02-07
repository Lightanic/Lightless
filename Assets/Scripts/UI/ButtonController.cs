using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Use this for initialization
    public int index = 0;
    int prevIndex = 0;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
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
                        index = 0;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
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
                        index = 0;
                    }
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    if (index > 0)
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
