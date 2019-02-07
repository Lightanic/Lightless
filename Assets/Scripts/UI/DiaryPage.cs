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

    [Header("Page count text")]
    [SerializeField]
    public Text PageCount;

    // Start is called before the first frame update
    void Start()
    {
        PageCount.text = CurrentPage.ToString() + " / " + MaxPages;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
