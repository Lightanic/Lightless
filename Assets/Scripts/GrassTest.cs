using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTest : MonoBehaviour
{
    public GameObject grass;
    //public GameObject plane;
    Mesh mesh;

    [Header("Quad Size")]
    public Vector2 size;

    [Header("Noise scale")]
    public int scale = 1;

    [Header("Density")]
    public int density = 20;

    [Header("Threashold")]
    [Range(0.2f,1f)]
    [SerializeField]
    float threshold = 0.5f;

    List<Vector3> vertextPts = new List<Vector3>();
    List<Vector3> points = new List<Vector3>();
    List<GameObject> objs = new List<GameObject>();

    float large = 0;
    float width;
    float length;
    float xIncrement;
    float yIncrement;
    float yPos;
    float xPos;
    int count = 0;

    [Space(10)]
    public bool reset = false;

    Vector3 topLeft;
    Vector3 topRight;
    Vector3 bottomRight;
    Vector3 bottomLeft;
    // Start is called before the first frame update
    /* Quad ( rot x 90 )
     * vert 0 = bottom left
     * vert 2 = bottom right
     * vert 1 = top right
     * vert 3 = top left
     */
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        ResetGrass();
    }

    private void OnDrawGizmos()
    {
        CalculateQuadVertex();

        Gizmos.color = Color.green;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
    }

    bool Noise(Vector3 pos)
    {
        var Noise = Mathf.PerlinNoise(pos.x / scale, pos.z / scale);
        if (Noise > large)
            large = Noise;
        if (Noise > threshold)
            return true;
        return false;
    }

    void DensityFilter()
    {
        var tempCount = points.Count - density;
        if (tempCount <= 0)
            tempCount = points.Count/2;
        while(tempCount!=0)
        {
            int rand = Random.Range(0, points.Count);
            points.RemoveAt(rand);
            tempCount--;
        }
    }

    void CalculateQuadVertex()
    {
        topLeft = transform.position + new Vector3(-0.5f * Mathf.Abs(size.x), 0f, 0.5f * Mathf.Abs(size.y));
        topRight = topLeft + new Vector3(1.0f * Mathf.Abs(size.x), 0f, 0f);
        bottomRight = topRight + new Vector3(0f, 0f, -1.0f * Mathf.Abs(size.y));
        bottomLeft = topLeft + new Vector3(0f, 0f, -1.0f * Mathf.Abs(size.y));
    }

    public void RenderGrass()
    {
        foreach (var obj in objs)
        {
            if (Application.isEditor)
                DestroyImmediate(obj);
            else
                Destroy(obj);
        }
        objs.Clear();
        vertextPts.Clear();
        points.Clear();
        count = 0;

        CalculateQuadVertex();
        vertextPts.Add(topLeft);
        vertextPts.Add(topRight);
        vertextPts.Add(bottomLeft);
        vertextPts.Add(bottomLeft);

        width = Vector3.Distance(topLeft, topRight);
        length = Vector3.Distance(topLeft, bottomLeft);

        xIncrement = 1;
        yIncrement = 1;

        yPos = vertextPts[0].z;
        xPos = vertextPts[0].x;

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var pos = new Vector3(xPos + (xIncrement * j), vertextPts[2].y, yPos);
                if (Noise(pos))
                {
                    points.Add(pos);
                }
            }
            yPos = vertextPts[2].z + (yIncrement * i);
        }
        DensityFilter();
        foreach (var pt in points)
        {
            objs.Add(Instantiate(grass, pt, Quaternion.identity, transform) as GameObject);
        }
    }

    public void ResetGrass()
    {
        if (reset)
        {
            foreach (var obj in objs)
            {
                if (Application.isEditor)
                    DestroyImmediate(obj);
                else
                    Destroy(obj);
            }
            objs.Clear();
            vertextPts.Clear();
            count = 0;
            reset = false;
        }
    }

    public void RemoveChildren()
    {
        if(transform.childCount > 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}
