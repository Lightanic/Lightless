using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightComponent : MonoBehaviour
{
    public bool LightIsOn = true;

    public void ToggleLightOn()
    {
        var lightSource = GetComponentInChildren<Light>();
        lightSource.enabled = !lightSource.enabled;
        LightIsOn = lightSource.enabled;
<<<<<<< HEAD
=======

        var lc = lightSource.transform.GetComponent<LightComponent>();
        if (lc != null)
        {
            lc.LightIsOn = LightIsOn;
        }
            
>>>>>>> Develop
    }

    public string GetParent()
    {
<<<<<<< HEAD
        if(transform.parent != null)
=======
        if (transform.parent != null)
>>>>>>> Develop
            return transform.parent.name.ToString();
        return null;
    }

}
