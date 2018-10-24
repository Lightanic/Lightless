using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUIComponent : MonoBehaviour {

    public GameObject Canvas;
    public float offsetHeight = 2.5f;
    public Sprite sprite = null;


    Quaternion rotation;

    private void Start()
    {
        rotation = Canvas.transform.rotation;
       // sprite = Canvas.GetComponentInChildren<Image>().sprite;
    }

    public void RePosition(Vector3 pos)
    {
        Canvas.transform.rotation = rotation;
        Canvas.transform.position = pos + new Vector3(0, offsetHeight, 0);
    }

    public void ToggleOn(Sprite newSprite)
    {
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

}
