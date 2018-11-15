using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ReflectionLine
{
    public Vector3 PointA;
    public Vector3 PointB;

    public ReflectionLine(Vector3 A, Vector3 B)
    {
        PointA = A;
        PointB = B;
    }
}

public class LineRendererComponent : MonoBehaviour
{

    public LineRenderer LineRenderer;
    public List<ReflectionLine> LineList;

    private void Start()
    {
        LineList = new List<ReflectionLine>();
        if (LineRenderer == null)
        {
            LineRenderer = GetComponent<LineRenderer>();
            LineRenderer.positionCount = 0;
        }
    }

    public void AddLine(ReflectionLine line)
    {
        LineList.Add(line);
    }

    public void AddLine(Vector3 A, Vector3 B)
    {

    }

    private void Update()
    {
        LineRenderer.positionCount = LineList.Count * 2;
        int index = 0;
        foreach (var line in LineList)
        {
            this.LineRenderer.SetPosition(index, line.PointA);
            this.LineRenderer.SetPosition(index + 1, line.PointB);
            index += 2;
        }

        LineList.Clear();
    }
}
