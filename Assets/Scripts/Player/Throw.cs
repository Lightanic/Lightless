using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Throw : MonoBehaviour {


    public LineRenderer lr;

    Rigidbody throwObject;
    Transform target;
    LeftHandComponent player;
    ThrowTarget targetComp;

    public float h = 4;
    public float gravity = -18;

    public bool debugPath;
    List<Vector3> points = new List<Vector3>();
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        targetComp = GameObject.Find("ThrowTarget").GetComponent<ThrowTarget>();
        target = GameObject.Find("ThrowTarget").transform;
    }

    void Update()
    {
        if (debugPath)
        {
            //DrawPath();
        }
    }

    public void Launch(GameObject equipped)
    {
        throwObject = equipped.GetComponent<Rigidbody>();
        throwObject.isKinematic = false;
        Physics.gravity = Vector3.up * gravity;
        throwObject.useGravity = true;
        throwObject.velocity = CalculateLaunchData().initialVelocity;
    }

    LaunchData CalculateLaunchData()
    {
        float displacementY = target.position.y - throwObject.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - throwObject.position.x, 0, target.position.z - throwObject.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    LaunchData CalculateLaunchData(GameObject equipped)
    {
        throwObject = equipped.GetComponent<Rigidbody>();
        float displacementY = target.position.y - throwObject.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - throwObject.position.x, 0, target.position.z - throwObject.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    public void DrawPath(GameObject equipped)
    {
        if (equipped)
        {
            ResetLine();
            LaunchData launchData = CalculateLaunchData(equipped);
            Vector3 previousDrawPoint = throwObject.position;

            int resolution = 30;
            for (int i = 1; i <= resolution; i++)
            {
                points.Add(previousDrawPoint);
                float simulationTime = i / (float)resolution * launchData.timeToTarget;
                Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
                Vector3 drawPoint = throwObject.position + displacement;
                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
                previousDrawPoint = drawPoint;
            }
            lr.positionCount = resolution;
            lr.SetPositions(points.ToArray());
        }
    }

    public void ResetLine()
    {
        lr.positionCount = 0;
        points.Clear();
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
}
