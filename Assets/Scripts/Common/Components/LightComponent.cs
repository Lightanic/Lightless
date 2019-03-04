using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightComponent : MonoBehaviour
{
    public bool LightIsOn = true;

    public void ToggleLightOn()
    {
        var lightSource = GetComponentInChildren<Light>();

        PostSound();

        lightSource.enabled = !lightSource.enabled;
        LightIsOn = lightSource.enabled;

        var lc = lightSource.transform.GetComponent<LightComponent>();
        if (lc != null)
        {
            lc.LightIsOn = LightIsOn;
        }
            
    }

    public string GetParent()
    {
        if (transform.parent != null)
            return transform.parent.name.ToString();
        return null;
    }

    public void PostSound()
    {
        if(gameObject.name == "FlashlightPickup")
        {
            if(!LightIsOn)
                AkSoundEngine.PostEvent("Play_Flashlight_On", gameObject);
            else
                AkSoundEngine.PostEvent("Play_Flashlight_Off", gameObject);
        }
        else if (gameObject.name == "lamp")
        {
            if (!LightIsOn)
                AkSoundEngine.PostEvent("Play_Lantern_Off", gameObject);
            else                              
                AkSoundEngine.PostEvent("Play_Lantern_Off", gameObject);
        }
    }
}
