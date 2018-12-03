using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativePickup : InteractableComponent {

    public GameObject Canvas;
    public float offsetHeight = 2.5f;

    public Sprite sprite = null;
    GameObject player = null;

    NarrativeCanvas canvasAction;
    //bool tutorialPlaying = false;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        canvasAction = Canvas.GetComponent<NarrativeCanvas>();
    }

    private void Update()
    {
    }

    public void Show()
    {
        Debug.Log("Show Narrative");
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        //isDisplayed = true;
        canvasAction.ToggleOn(sprite, offsetHeight);
        gameObject.SetActive(false);
    }

    public void Close()
    {
        //isDisplayed = false;
        canvasAction.ToggleOff();
    }

    //public void RePosition(Vector3 pos)
    //{
    //    Canvas.transform.position = pos + new Vector3(0, offsetHeight, 0);
    //}

    //public void ToggleOn(Sprite newSprite)
    //{
    //    //Time.timeScale = 0.0f;
    //    isDisplayed = true;
    //    Canvas.GetComponent<CanvasGroup>().alpha = 0;
    //    if (newSprite != null)
    //        Canvas.GetComponentInChildren<Image>().sprite = newSprite;
    //    else
    //        Canvas.GetComponentInChildren<Image>().sprite = sprite;
    //    Canvas.SetActive(true);
    //    StartCoroutine(FadeIn());
    //}

    //public void ToggleOff()
    //{
    //    isDisplayed = false;
    //    StartCoroutine(FadeOut());
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        RePosition(other.transform.position);
    //        ToggleOn(sprite);
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        RePosition(other.transform.position);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        //RePosition();
    //        StartCoroutine(FadeOut());
    //        //ToggleOff();
    //    }
    //}

    //IEnumerator FadeOut()
    //{
    //    CanvasGroup cGroup = Canvas.GetComponent<CanvasGroup>();

    //    if(cGroup.alpha <= 0)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //    while (cGroup.alpha > 0)
    //    {
    //        cGroup.alpha -= Time.deltaTime / 2;
    //        yield return null;
    //    }
    //    cGroup.interactable = false;
    //    yield return null;
    //}

    //IEnumerator FadeIn()
    //{
    //    CanvasGroup cGroup = Canvas.GetComponent<CanvasGroup>();
    //    while (cGroup.alpha < 1)
    //    {
    //        cGroup.alpha += Time.deltaTime / 2;
    //        yield return null;
    //    }
    //    cGroup.interactable = false;
    //    yield return null;
    //}
}
