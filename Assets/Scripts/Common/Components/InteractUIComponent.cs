using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUIComponent : MonoBehaviour {

    public GameObject Canvas;

    public float offsetHeight = 2.5f;

    [Header("Sprite Name")]
    public string Name ;

    [Header("Default sprite shown")]
    public Sprite DefaultSprite = null;

    [Header("Show distance")]
    [Range(0, 10)]
    public float ShowDistance = 2.0f;

    Quaternion rotation;

    private void Start()
    {
        rotation = Canvas.transform.rotation;
        //sprite = Canvas.GetComponentInChildren<Image>().sprite;
    }
    
    public void RePosition(Vector3 pos)
    {
        Canvas.transform.rotation = rotation;
        Canvas.transform.position = pos + new Vector3(0, offsetHeight, 0);
    }

    public void ToggleOn(string name)
    {
        var newSprite = SpriteManager.Instance.GetSprite(name);
        if (newSprite != null)
            Canvas.GetComponentInChildren<Image>().sprite = newSprite;
        else
            Canvas.GetComponentInChildren<Image>().sprite = DefaultSprite;
        Canvas.SetActive(true);
    }

    public void ToggleOff()
    {
        Canvas.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ShowDistance);
    }
}
