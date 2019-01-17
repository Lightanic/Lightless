using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Checkpoint
{
    [Serializable]
    class GameSaveData
    {
        public string CurrentCheckpoint;
        public List<string> InventoryItems;
        public int OilTankAmount;
        public Vector3 CameraOffset; 
    }
}
