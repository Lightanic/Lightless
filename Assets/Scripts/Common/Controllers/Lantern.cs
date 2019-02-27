using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Lantern : MonoBehaviour
{

    SphereCollider collider;
    public Transform RightHand;

    public float TooltipOffset = 1.5f;
    public bool ShowtoolTip;
    [SerializeField] Sprite toolTipSprite;
    [SerializeField] GameObject Canvas;

    [Header("Equip lantern on start")]
    public bool equipLantern = false;

    [Header("Lantern Features")]
    public bool EnableDarkMode = false;
    [Range(-1, 0)]
    public float DarknessGammaValue = -0.4F;

    public PostProcessVolume postProcessVolume;
    public LightComponent lightComponent;

    void Start()
    {
        if (postProcessVolume == null)
        {
            postProcessVolume = GameObject.Find("Post-process Volume").GetComponent<PostProcessVolume>();
        }

        lightComponent = GetComponent<LightComponent>();
        GameObject lantern = GameObject.Find("lamp");
        Pickup lanternPickup = lantern.GetComponent<Pickup>();
        if (equipLantern)
        {
            lanternPickup.IsInteracting = true;
            lanternPickup.IsEquiped = true;   // 
            lanternPickup.IsInteractable = false;
            lanternPickup.GetComponent<Lantern>().EquipRightHand();
        }


    }

    public void Update()
    {
        if (EnableDarkMode)
        {
            ColorGrading colorGrading;
            postProcessVolume.profile.TryGetSettings(out colorGrading);
            if (!GetComponent<LightComponent>().LightIsOn)
            {
                colorGrading.enabled.value = true;
                colorGrading.gamma.value.w = Mathf.Lerp(colorGrading.gamma.value.w, DarknessGammaValue, 2 * Time.deltaTime);
            }
            else
            {
                colorGrading.gamma.value.w = Mathf.Lerp(colorGrading.gamma.value.w, 0F, 2 * Time.deltaTime);
            }
        }
    }

    public void EquipRightHand()
    {
        var itemTransform = transform;
        itemTransform.transform.SetParent(RightHand);
        itemTransform.position = RightHand.position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.gameObject.GetComponent<Pickup>().enabled = false;
        itemTransform.gameObject.GetComponent<Rigidbody>().isKinematic = true;  // disable rigidbody
    }

    public void ToggleToolTip()
    {
        if (ShowtoolTip && Canvas != null)
        {
            Canvas.transform.position = transform.position + new Vector3(0, TooltipOffset, 0);
            if (toolTipSprite != null)
                Canvas.GetComponentInChildren<Image>().sprite = toolTipSprite;
            Canvas.SetActive(true);
        }
        else
            Canvas.SetActive(false);

    }
}
