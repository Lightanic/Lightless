using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[System.Serializable]
public class SwapSprite
{
    [SerializeField]
    public string name;
    [SerializeField]
    public Sprite controllerSprite;
    [SerializeField]
    public Sprite keyboardSprite;
}
public class SpriteManager : MonoBehaviour
{
    static public SpriteManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    List<SwapSprite> SpriteList = new List<SwapSprite>();
    
    public Sprite GetSprite(string name)
    {
        foreach(var item in SpriteList)
        {
            if(item.name == name)
            {
                if(InputManager.Instance.IsGamePadActive)
                {
                    return item.controllerSprite;
                }
                else if(!InputManager.Instance.IsGamePadActive)
                {
                    return item.keyboardSprite;
                }
            }
        }
            
        return null;
    }
}
