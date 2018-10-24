using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedComponent : MonoBehaviour {
    public float DEFAULT_SPEED = 2;
    public float SPRINT_SPEED = 3;
    public float MAX_STAMINA = 2;
    public float DODGE_SPEED = 20;
    public float DodgeMultiplier = 100;
    public float RotationSpeed = 10;
    public float RotationFineControlSpeed = 2;

    public bool isSprinting = false;
    public bool isDodging = false;
    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public float Stamina = 1;
}
