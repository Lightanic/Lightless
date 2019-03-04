using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdate : MonoBehaviour
{
    [SerializeField]
    float showTime;
    [SerializeField]
    Transform Player;
    [SerializeField]
    Vector3 offsetFromPlayer;

    Image[] children;

    Animator anim;
    bool disable = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        children = gameObject.GetComponentsInChildren<Image>();
        foreach (var img in children)
        {
            img.color = new Vector4(1, 1, 1, 1);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Player.position + offsetFromPlayer;
    }

    public void Disable()
    {
        anim.SetBool("Show", false);
        anim.SetBool("FadeOut", true);
        //StartCoroutine(timer());
    }

    public void Show()
    {
        anim.SetBool("FadeOut", false);
        anim.SetBool("Show", true);
        Invoke("Disable", showTime);
    }

    public void ShowNoFade()
    {
        anim.SetBool("FadeOut", false);
        anim.SetBool("Show", true);
    }
    
    public void FadeOutFalse()
    {
        anim.SetBool("FadeOut", false);
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(0.5f);
        disable = false;
        anim.SetBool("FadeOut", false);
    }
}
