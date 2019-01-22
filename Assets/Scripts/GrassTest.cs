using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTest : MonoBehaviour
{
    public GameObject grass;
    public GameObject plane;
    Mesh mesh;

    [Header("Quad Size")]
    public Vector3 size;

    [Header("Noise scale")]
    public int scale = 1;

    [Header("Density")]
    public int density = 20;

    [Header("Threashold")]
    [Range(0.2f,1f)]
    [SerializeField]
    float threshold = 0.5f;

    List<Vector3> vertextPts;
    List<Vector3> points;
    List<GameObject> objs;

    float large = 0;
    float width;
    float length;
    float xIncrement;
    float yIncrement;
    float yPos;
    float xPos;
    int count = 0;

    public bool reset = false;
    public bool clear = false;
    // Start is called before the first frame update
        /* Quad ( rot x 90 )
         * vert 0 = bottom left
         * vert 2 = bottom right
         * vert 1 = top right
         * vert 3 = top left
         */
    void Start()
    {
        vertextPts = new List<Vector3>();
        points = new List<Vector3>();
        objs = new List<GameObject>();
        plane.GetComponent<MeshRenderer>().enabled = false;
        size = plane.transform.localScale;
        mesh = plane.GetComponent<MeshFilter>().sharedMesh;
        for (int j = 0; j < mesh.vertexCount; j++)
        {
            vertextPts.Add(plane.transform.TransformPoint(mesh.vertices[j]));
        }

        width = Vector3.Distance(vertextPts[0], vertextPts[2]);
        length = Vector3.Distance(vertextPts[1], vertextPts[3]);

        xIncrement = width / size.x;
        yIncrement = length / size.y;

        yPos = vertextPts[0].z;
        xPos = vertextPts[0].x;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var pos = new Vector3(xPos + (xIncrement * j),vertextPts[0].y,yPos);
                if (noise(pos))
                {
                    points.Add(pos);
                }
            }
            yPos = vertextPts[0].z + (yIncrement * i);
        }
        DensityFilter();
        foreach (var pt in points)
        {
            objs.Add(Instantiate(grass, pt, Quaternion.identity, transform) as GameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(clear)
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
            clear = !clear;
        }
        if(reset)
        {
            plane.transform.localScale = size;
            foreach( var obj in objs)
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
            mesh = plane.GetComponent<MeshFilter>().sharedMesh;
            for (int j = 0; j < mesh.vertexCount; j++)
            {
                vertextPts.Add(plane.transform.TransformPoint(mesh.vertices[j]));
            }

            width = Vector3.Distance(vertextPts[0], vertextPts[2]);
            length = Vector3.Distance(vertextPts[1], vertextPts[3]);

            xIncrement = width / size.x;
            yIncrement = length / size.y;

            yPos = vertextPts[0].z;
            xPos = vertextPts[0].x;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var pos = new Vector3(xPos + (xIncrement * j), vertextPts[0].y, yPos);
                    if (noise(pos))
                    {
                        points.Add(pos);
                    }
                }
                yPos = vertextPts[0].z + (yIncrement * i);
            }
            DensityFilter();
            foreach(var pt in points)
            {
                objs.Add(Instantiate(grass, pt, Quaternion.identity, transform) as GameObject);
            }
            reset = !reset;
        }
    }

    bool noise(Vector3 pos)
    {
        var noise = Mathf.PerlinNoise(pos.x / scale, pos.z / scale);
        if (noise > large)
            large = noise;
        if (noise > threshold)
            return true;
        return false;
    }

    void DensityFilter()
    {
        var tempCount = Mathf.Abs(points.Count - density);
        while(tempCount!=0)
        {
            int rand = Random.Range(0, points.Count);
            points.RemoveAt(rand);
            tempCount--;
        }
    }
}
