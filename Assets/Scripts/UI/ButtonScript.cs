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
    [SerializeField] int thisIndex;

    NarrativePickup pickup;

    [Header("Default Image")]
    public Sprite sprite;

    Image image;

    [Header("Diary Page object")]
    public DiaryPage diary;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerProperties.narrativePickups.ContainsKey(thisIndex))
        {
            PlayerProperties.narrativePickups.TryGetValue(thisIndex, out pickup);
            image.sprite = pickup.sprite;
        }
        else
        {
            image.sprite = sprite;
        }
        if (menuButtonController.index == thisIndex)
        {
            //Debug.Log(gameObject.name);
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1)
            {
                //Debug.Log(gameObject.name + "submit");
                animator.SetBool("pressed", true);
                //PageTurn();
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

    void PageTurn()
    {
        if(thisIndex == 9) // back button
        {
            diary.CurrentPage--;
        }
        else if(thisIndex == 10)
        {
            diary.CurrentPage++;
        }
        diary.CurrentPage = Mathf.Clamp(diary.CurrentPage, 1, diary.MaxPages);

        diary.PageCount.text = diary.CurrentPage.ToString() + " / " + diary.MaxPages;
    }
}
