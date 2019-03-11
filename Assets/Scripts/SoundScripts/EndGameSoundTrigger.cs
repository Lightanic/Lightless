using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSoundTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            AkSoundEngine.SetState("Game_State", "Endgame");
        }
    }
}
