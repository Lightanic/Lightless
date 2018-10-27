using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject {

    public List<InventoryItem> Items = new List<InventoryItem>();
    public List<uint> ItemNumber = new List<uint>(); 

    public Dictionary<string, InventoryItem> ItemsDictonary = new Dictionary<string, InventoryItem>();

    /// <summary>
    /// Populate the items dictonary for unique reference
    /// </summary>
    private void OnEnable()
    {
        ItemsDictonary.Clear();
        for (int i = 0; i < Items.Count; i++)
        {
            //ItemsDictonary.Add(Items[i].GUID, Items[i]);
        }
        //Items.Clear();
        //ItemNumber.Clear();
    }

    /// <summary>
    /// Add an instance of an item to the inventory
    /// </summary>
    /// <param name="item"></param>
    public void Add(InventoryItem item)
    {
        if(!Items.Contains(item))
        {
            //Add item
            Items.Add(item);
            ItemNumber.Add(1);
            //ItemsDictonary.Add(Items[Items.Count - 1].GUID, Items[Items.Count - 1]);
        }
        else
        {
            int index = Items.IndexOf(item);
            ++ItemNumber[index];
        }
    }

    /// <summary>
    /// Add an instance of an item to the top inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddTop(InventoryItem item)
    {
        if (!Items.Contains(item))
        {
            //Add item
            Items.Insert(0,item);
            ItemNumber.Insert(0,1);
            //ItemsDictonary.Add(Items[Items.Count - 1].GUID, Items[Items.Count - 1]);
        }
        else
        {
            int index = Items.IndexOf(item);
            ++ItemNumber[index];
        }
    }

    /// <summary>
    /// Removes an instance of an item in the inventory
    /// If the same item is present multiple times then remove one item
    /// </summary>
    /// <param name="item"></param>
    public void Remove(InventoryItem item)
    {
        if(Items.Contains(item))
        {
            int index = Items.IndexOf(item);
            --ItemNumber[index];
            if (ItemNumber[index] <= 0)
            {
                ItemNumber.RemoveAt(index);
                Items.Remove(item);
            }
        }
    }

    /// <summary>
    /// Context menu option to populate all the inventory items to the inventory
    /// </summary>
    [ContextMenu("Populate Items")]
    private void FindItems()
    {
        //string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(InventoryItem).Name);
        
        //for (int i = 0; i < guids.Length; i++)
        //{
        //    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
        //    InventoryItem item = UnityEditor.AssetDatabase.LoadAssetAtPath<InventoryItem>(path);
        //    Items.Add(item);
        //    ItemNumber.Add(1);
        //    item.GUID = guids[i];
        //    UnityEditor.EditorUtility.SetDirty(this);
        //}
    }

    private void OnDestroy()
    {
        Items.Clear();
        ItemNumber.Clear();
    }

    private void OnDisable()
    {
        Items.Clear();
        ItemNumber.Clear();
    }

}
