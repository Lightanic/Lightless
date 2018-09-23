using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour {

    public bool FPSCounterOn = false;
    public float UpdateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private float fps;
    public GUIStyle style;
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    void OnGUI()
    {
        if(FPSCounterOn)
            GUILayout.Label("" + fps.ToString("f2"),style);
    }

    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + UpdateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
    }
}
