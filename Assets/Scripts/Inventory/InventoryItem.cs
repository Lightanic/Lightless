using UnityEngine;

[CreateAssetMenu]
public class InventoryItem : ScriptableObject{

    public string GUID = "";        // unique ID
    public GameObject Prefab;           // the item prefab
    public Sprite InventoryIcon;        // The icon in the inventory GUI
    public Sprite PopupIcon;            // The icon that shows up as a tool tip
    public bool IsHolding = false;      // If the player is holding the item
}
