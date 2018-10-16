using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotator : MonoBehaviour {

    public float speed = 100.0f;
    Vector3 angle;
    float rotation = 0f;
    public enum Axis
    {
        X,
        Y,
        Z
    }
    public Axis axis = Axis.X;
    public bool direction = true;

    void Start()
    {
        angle = transform.localEulerAngles;
    }

    void Update()
    {
        switch (axis)
        {
            case Axis.X:
                transform.localEulerAngles = new Vector3(Rotation(), angle.y, angle.z);
                break;
            case Axis.Y:
                transform.localEulerAngles = new Vector3(angle.x, Rotation(), angle.z);
                break;
            case Axis.Z:
                transform.localEulerAngles = new Vector3(angle.x, angle.y, Rotation());
                break;
        }
    }

    float Rotation()
    {
        rotation += speed * Time.deltaTime;
        if (rotation >= 360f)
            rotation -= 360f; // this will keep it to a value of 0 to 359.99...
        return direction ? rotation : -rotation;
    }
}