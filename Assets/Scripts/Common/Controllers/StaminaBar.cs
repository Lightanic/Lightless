using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour {


    public Image stamina;

    SpeedComponent playerComponent = null;
    public GameObject cam;

    Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;
    }


    // Use this for initialization
    void Start () {
        playerComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<SpeedComponent>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = rotation;
        //transform.LookAt(cam.transform);
        if (playerComponent != null)
        {
            float ratio = playerComponent.Stamina / playerComponent.MAX_STAMINA;
            //stamina.rectTransform.localScale = new Vector3(ratio, 1, 1);
            stamina.fillAmount = ratio;
        }
	}
}
