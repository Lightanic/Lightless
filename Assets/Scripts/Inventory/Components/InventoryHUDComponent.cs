using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUDComponent : MonoBehaviour {

    public Image SelectedSlot;
    public Image LeftSlot;
    public Image RightSlot;

    GameObject selectedSlotMask;

    [SerializeField]
    public Sprite defaultSelected;
    [SerializeField]
    public Sprite defaultLeft;
    [SerializeField]
    public Sprite defaultRight;
    [Space]
    public GameObject cam;
    float alpha;
    // Use this for initialization
    void Start () {
        if (defaultSelected == null)
            defaultSelected = SelectedSlot.sprite;
        if (defaultLeft == null)
            defaultLeft = LeftSlot.sprite;
        if (defaultRight == null)
            defaultRight = RightSlot.sprite;
        alpha = SelectedSlot.color.a;

        selectedSlotMask = GameObject.Find("SelectedSlotMask");
    }
    private void LateUpdate()
    {
        transform.LookAt(cam.transform);
    }
    public void SetSelectedSlot(Sprite image)
    {
        if(image == null)
        {
            SelectedSlot.sprite = defaultSelected;
            return;
        }
        SelectedSlot.sprite = image;
        selectedSlotMask.GetComponent<Image>().sprite = image;
    }

    public void SetLeftSlot(Sprite image)
    {
        if (image == null)
        {
            LeftSlot.sprite = defaultLeft;
            return;
        }
        LeftSlot.sprite = image;
    }
    public void SetRightSlot(Sprite image)
    {
        if (image == null)
        {
            RightSlot.sprite = defaultRight;
            return;
        }
        RightSlot.sprite = image;
    }
}
