﻿using Assets.Scripts.Mechanics.Checkpoint;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{

    public static string latestCheckpoint;
    public static Vector3 cameraOffset;
    public GameObject Player;
    public CameraController CamController;
    private Vector3 defaultCamOffset = new Vector3(5.4F, 20.5F, -17.1F);
    private static bool created = false;
    public static Inventory latestPlayerInventory;
    public static string leftHandComponent;
    public static List<string> inventoryList;
    public string CheckpointLocal;
    public bool DebugModeQuickMove = false;

    Dictionary<string, Transform> checkpointMap;

    private void Start()
    {
        if (CamController == null)
        {
            var cam = GameObject.Find("Main Camera");
            if (cam != null)
            {
                CamController = cam.GetComponent<CameraController>();
            }
        }
        checkpointMap = new Dictionary<string, Transform>();
        var checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (var checkpoint in checkpoints)
        {
            var name = checkpoint.GetComponent<CheckpointTrigger>().CheckpointName;
            var transform = checkpoint.GetComponent<Transform>();
            if (!checkpointMap.ContainsKey(name))
                checkpointMap.Add(name, transform);
        }

        if (!string.IsNullOrEmpty(latestCheckpoint))
        {
            GoToLatestCheckpoint();
        }
    }

    private void Update()
    {
        if(DebugModeQuickMove)
        {
            if (Player == null)
            {
                Player = GameObject.Find("Player");
            }
            if (CamController == null)
            {
                var cam = GameObject.Find("Main Camera");
                if (cam != null)
                {
                    CamController = cam.GetComponent<CameraController>();
                }
            }

            DebugModeQuickMove = false;
            SetLatestCheckpoint(CheckpointLocal);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Awake()
    {
        checkpointMap = new Dictionary<string, Transform>();
        var checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (var checkpoint in checkpoints)
        {
            var name = checkpoint.GetComponent<CheckpointTrigger>().CheckpointName;
            var transform = checkpoint.GetComponent<Transform>();
            if (!checkpointMap.ContainsKey(name))
                checkpointMap.Add(name, transform);
        }

        if (!created)
        {
            inventoryList = new List<string>();
            DontDestroyOnLoad(gameObject);
            //cameraOffset = defaultCamOffset;
            created = true;

            Debug.Log("Awake: " + gameObject);
        }
    }

    public void SetLatestCheckpoint(string checkpoint)
    {
        if (checkpoint == latestCheckpoint) return;
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }

        GameSaveData saveData = new GameSaveData();
        saveData.CurrentCheckpoint = checkpoint;
        latestCheckpoint = checkpoint;
        cameraOffset = CamController.offset;
        latestPlayerInventory = Player.GetComponent<InventoryComponent>().PlayerInventory;
        var equippedItem = Player.GetComponentInChildren<EquipComponent>().EquipedItem;
        if (equippedItem != null)
            leftHandComponent = equippedItem.name;

        inventoryList.Clear();
        foreach(var item in Player.GetComponent<InventoryComponent>().PlayerInventory.Items)
        {
            Debug.Log(item.Prefab.name);
            inventoryList.Add(item.Prefab.name);
        }
        inventoryList.Add(leftHandComponent);

        saveData.InventoryItems = inventoryList;
    }

    public void GoToLatestCheckpoint()
    {
        Player.GetComponent<InventoryComponent>().PlayerInventory.Items.Clear();
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }
        Player.transform.GetChild(1).transform.DetachChildren();
        Player.GetComponent<InventoryComponent>().PlayerInventory = latestPlayerInventory;
        CamController.SetOffset(cameraOffset);
        Player.transform.position = checkpointMap[latestCheckpoint].position + transform.forward * 1.5F;

        var lamp = GameObject.Find("lamp");
        var pickup = lamp.GetComponent<Pickup>();
        var lantern = lamp.GetComponent<Lantern>();
        pickup.IsInteracting = true;
        pickup.IsEquiped = true;   // equip to left hand
        pickup.IsInteractable = false;
        lantern.EquipRightHand();

        lamp.GetComponent<LightComponent>().ToggleLightOn();
        foreach(var pickupItem in inventoryList)
        {
            PickupItem(pickupItem);
        }
    }

    public void ResetScene()
    {
        created = false;
        latestCheckpoint = "section1";
    }

    void PickupItem(string pickupName)
    {
        var item = GameObject.Find(pickupName);
        if(leftHandComponent == pickupName)
        {
            var pickup = item.GetComponent<Pickup>();
            pickup.IsInteracting = true;
            pickup.IsEquiped = true;   // equip to left hand
            pickup.IsInteractable = false;
            Player.GetComponentInChildren<EquipComponent>().EquipItem(item);
        }
        else if(item!=null)
        {
            item.GetComponent<InventoryItemComponent>().AddToInventory = true;
        }
    }

    static void SaveGame(GameSaveData data)
    {
    }



}
