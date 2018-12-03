using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeCanvas : MonoBehaviour {

    GameObject player = null;
    public float defaultOffsetHeight = 2.5f;
    public bool isDisplayed = false;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
        gameObject.transform.Rotate(new Vector3(0, 180, 0));
        RePosition(player.transform.position, defaultOffsetHeight);
    }

    public void RePosition(Vector3 pos, float offsetHeight)
    {
        gameObject.transform.position = pos + new Vector3(0, offsetHeight, 0);
    }

    public void ToggleOn(Sprite newSprite, float heightOffset)
    {
        //Time.timeScale = 0.0f;
        isDisplayed = true;
        defaultOffsetHeight = heightOffset;
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        if (newSprite != null)
            gameObject.GetComponentInChildren<Image>().sprite = newSprite;
        else
            gameObject.GetComponentInChildren<Image>().sprite = null;
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void ToggleOff()
    {
        isDisplayed = false;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        CanvasGroup cGroup = gameObject.GetComponent<CanvasGroup>();

        if (cGroup.alpha <= 0)
        {
            gameObject.SetActive(false);
        }
        while (cGroup.alpha > 0)
        {
            cGroup.alpha -= Time.deltaTime / 2;
            yield return new WaitForSeconds(0.05f);
        }
        cGroup.interactable = false;
        yield return null;
    }

    IEnumerator FadeIn()
    {
        CanvasGroup cGroup = gameObject.GetComponent<CanvasGroup>();
        while (cGroup.alpha < 1)
        {
            cGroup.alpha += Time.deltaTime / 2;
            yield return null;
        }
        cGroup.interactable = false;
        yield return null;
    }
}
