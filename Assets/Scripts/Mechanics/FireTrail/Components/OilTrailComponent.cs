using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilTrailComponent : MonoBehaviour
{
    public LineRenderer LineRenderer;
    public int TrailLimit = 5;
    public int CurrentTrailCount = 0;
    public float TrailMinimumDistance = 1.0f;
    public List<Vector3> TrailPoints;
    private void Start()
    {
        TrailPoints = new List<Vector3>();
        LineRenderer = GetComponentInChildren<LineRenderer>();
        LineRenderer.positionCount = 0;
    }
}
