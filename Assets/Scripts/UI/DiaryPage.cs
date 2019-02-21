using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryPage : MonoBehaviour
{
    [Header("Current Page")]
    [SerializeField]
    public int CurrentPage = 1;

    [Header("Max Pages")]
    [SerializeField]
    public int MaxPages = 2;

    [Header("Max Notes")]
    [SerializeField]
    public int MaxNotes;

    [Header("Page count text")]
    [SerializeField]
    public Text PageCount;

    
    DiaryPage diary;
    // Start is called before the first frame update
    void Start()
    {
        PageCount.text = CurrentPage.ToString() + " / " + MaxPages;
        diary = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PageTurnBack()
    {
        int prevPage = diary.CurrentPage;
        diary.CurrentPage--;
        diary.CurrentPage = Mathf.Clamp(diary.CurrentPage, 1, diary.MaxPages);
        diary.PageCount.text = diary.CurrentPage.ToString() + " / " + diary.MaxPages;

        if (prevPage != diary.CurrentPage)
        {
            var elementList = transform.GetComponentsInChildren<ButtonScript>();
            foreach (var element in elementList)
            {
                int index = element.thisIndex - 9;
                if (index < MaxNotes)
                {
                    element.notAvailable = false;
                    element.thisIndex -= 9;
                }
                else
                    element.notAvailable = true;
            }
        }
    }

    public void PageTurnNext()
    {
        int prevPage = diary.CurrentPage;
        diary.CurrentPage++;
        diary.CurrentPage = Mathf.Clamp(diary.CurrentPage, 1, diary.MaxPages);
        diary.PageCount.text = diary.CurrentPage.ToString() + " / " + diary.MaxPages;

        if (prevPage != diary.CurrentPage)
        {
            var elementList = transform.GetComponentsInChildren<ButtonScript>();
            foreach (var element in elementList)
            {
                int index = element.thisIndex + 9;
                if (index < MaxNotes)
                {
                    element.notAvailable = false;
                    element.thisIndex += 9;
                }
                else
                    element.notAvailable = true;
            }
        }
    }
}
