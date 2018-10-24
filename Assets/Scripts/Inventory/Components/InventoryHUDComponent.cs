using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUDComponent : MonoBehaviour {

    public Image SelectedSlot;
    public Image LeftSlot;
    public Image RightSlot;

    [SerializeField]
    Sprite defaultSelected;
    [SerializeField]
    Sprite defaultLeft;
    [SerializeField]
    Sprite defaultRight;

    float alpha;
    // Use this for initialization
    void Start () {
        defaultSelected = SelectedSlot.sprite;
        defaultLeft = LeftSlot.sprite;
        defaultRight = RightSlot.sprite;
        alpha = SelectedSlot.color.a;
	}
	
	public void SetSelectedSlot(Sprite image)
    {
        if(image == null)
        {
            SelectedSlot.sprite = defaultSelected;
        }
        SelectedSlot.sprite = image;
    }

    public void SetLeftSlot(Sprite image)
    {
        if (image == null)
        {
            LeftSlot.sprite = defaultLeft;
        }
        LeftSlot.sprite = image;
    }
    public void SetRightSlot(Sprite image)
    {
        if (image == null)
        {
            RightSlot.sprite = defaultRight;
        }
        RightSlot.sprite = image;
    }
}
