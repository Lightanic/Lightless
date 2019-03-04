using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointLogo : MonoBehaviour
{
    public bool EnableCheckpointLoader;
    public float CheckpointShowTime = 1.5F;
    public Image Logo;
    private float CurrentTime = 0F;

    public void TriggerCheckpointLogo()
    {
        EnableCheckpointLoader = true;
    }

    void Start()
    {
        if (!Logo)
        {
            Logo = GetComponentInChildren<Image>();
        }
    }

    void Update()
    {
        if (EnableCheckpointLoader)
        {
            EnableCheckpointLoader = false;
            StartCoroutine(FadeTo(1, CheckpointShowTime));
        }
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        Color color = Logo.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(color.r, color.g, color.b, (t / 1F) * (t / 1F));
            Logo.color = newColor;
            yield return null;
        }

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float alpha = 1 - (t / 1F);
            Color newColor = new Color(color.r, color.g, color.b, alpha * alpha);
            Logo.color = newColor;
            yield return null;
        }
    }
}
