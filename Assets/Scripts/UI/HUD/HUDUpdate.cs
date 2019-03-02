using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdate : MonoBehaviour
{
    [SerializeField]
    Transform Player;
    [SerializeField]
    Vector3 offsetFromPlayer;

    Image[] children;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        children = gameObject.GetComponentsInChildren<Image>();
        foreach (var img in children)
        {
            img.color = new Vector4(0, 0, 0, 0);
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
    }

    public void Show()
    {
        foreach (var img in children)
        {
            img.color = new Vector4(1, 1, 1, 1);
        }
        anim.SetBool("Show", true);
        Invoke("Disable", 3);
    }
    
    public void FadeOutFalse()
    {
        anim.SetBool("FadeOut", false);
    }
}
