using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilCanvasComponent : MonoBehaviour
{

    public float CurrentTime = 0F;
    public float ShowTime = 2F;
    public GameObject TutText;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void Show()
    {
        CurrentTime = 0F;
    }

    public void DisableUI()
    {
        CurrentTime = ShowTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTime < ShowTime)
        {
            TutText.SetActive(true);
            CurrentTime += Time.deltaTime;
        }
        else
        {
            TutText.SetActive(false);
        }
    }
}
