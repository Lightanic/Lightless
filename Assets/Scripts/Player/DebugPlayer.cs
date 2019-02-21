using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayer : MonoBehaviour
{
    [Header("Debug")]
    public bool DebugOn = true;

    [Header("Items list")]
    public List<string> items = new List<string>();

    private void Start()
    {
        if(DebugOn)
        {
            foreach(var item in items)
            {
                InventoryItemComponent inventoryComp = null;
                GameObject obj = GameObject.Find(item);
                inventoryComp = obj.GetComponent<InventoryItemComponent>();
                if(inventoryComp != null)
                {
                    inventoryComp.AddToInventory = true;
                }
            }
        }
    }
}
