using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonScript : MonoBehaviour
{
    [SerializeField] ButtonController menuButtonController;
    [SerializeField] Animator animator;
    //[SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] public int thisIndex;

    NarrativePickup pickup;

    [Header("Default Image")]
    public Sprite sprite;

    [Header("Image to represent no notes")]
    public Sprite transparent;

    Image image;

    [Header("Diary Left Page object")]
    public GameObject LeftHandView;

    [Header("Diary Left Page Sprite")]
    Sprite noteSprite;

    InputComponent inputComp;

    public bool notAvailable = false;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        image = gameObject.GetComponent<Image>();
        inputComp = GameObject.Find("Player").GetComponent<InputComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (notAvailable)
        {
            image.sprite = transparent;
            noteSprite = transparent;
            gameObject.GetComponent<Button>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Button>().enabled = true;
            if (PlayerProperties.narrativePickups.ContainsKey(thisIndex))
            {
                bool notePresent = PlayerProperties.narrativePickups.TryGetValue(thisIndex, out pickup);
                if (notePresent)
                {
                    image.sprite = pickup.sprite;
                    noteSprite = pickup.fullNote;
                }
                else
                {
                    image.sprite = sprite;
                    noteSprite = sprite;
                }
            }
            else
            {
                image.sprite = sprite;
                noteSprite = sprite;
            }
            if (menuButtonController.index == thisIndex)
            {
                animator.SetBool("selected", true);
                if (inputComp.Control("Accept"))
                {
                    animator.SetBool("pressed", true);
                    DiaryLeftViewUpdate();
                }
                else if (animator.GetBool("pressed"))
                {
                    animator.SetBool("pressed", false);
                    //animatorFunctions.disableOnce = true;
                }
            }
            else
            {
                animator.SetBool("selected", false);
            }
        }
    }
    public void DiaryLeftViewUpdate()
    {
        LeftHandView.GetComponent<Image>().sprite = noteSprite;
    }
}
