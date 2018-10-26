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

        var lc = lightSource.transform.GetComponent<LightComponent>();
        if (lc != null)
            lc.LightIsOn = LightIsOn;
    }

    public string GetParent()
    {
        if (transform.parent != null)
            return transform.parent.name.ToString();
        return null;
    }

}
