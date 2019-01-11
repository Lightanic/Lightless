using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OilMeter : MonoBehaviour {

    Image OilMeterImg;

    GameObject playerComponent = null;
    OilTrailComponent oilTrail;
    // Use this for initialization
    void Start()
    {
        playerComponent = GameObject.FindGameObjectWithTag("Player");
        oilTrail = playerComponent.GetComponentInChildren<OilTrailComponent>(false);
        OilMeterImg = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        oilTrail = playerComponent.GetComponentInChildren<OilTrailComponent>(false);
        if (oilTrail != null)
        {
            float ratio = (float)oilTrail.usedOil / (float)oilTrail.TrailLimit;
            OilMeterImg.fillAmount = ratio;
        }
        else
        {
            OilMeterImg.sprite = null;
        }
    }
}
