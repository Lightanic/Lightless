using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class ConeCollider : MonoBehaviour
{
    [SerializeField, Range(0.01f, 88.5f)]
    private float m_angle = 45;
    [SerializeField]
    private float m_distance = 1;
    [SerializeField]
    private bool m_isTrigger;
    private Mesh m_mesh;
    private Vector3 m_localScale;
    [SerializeField]
    private bool m_isFixScale = true;

    void Awake()
    {
        GameObject cone = Resources.Load("Prefab/ConeCollider") as GameObject;

        var initRot = this.transform.rotation;
        this.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        var coneMesh = cone.GetComponent<MeshFilter>().sharedMesh;
        var vertices = coneMesh.vertices;
        var triangles = coneMesh.triangles;
        var forward = this.transform.TransformDirection(Vector3.forward);
        var centerForwardPos = this.transform.position + forward * m_distance;
        var harf = m_distance * Mathf.Tan(m_angle * Mathf.PI / 180f);
        var verticleCount = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            var verticeWorldPos = vertices[i] + this.transform.position;
            if (verticleCount != 2 || i >= 36)
            {

                verticeWorldPos += forward * (m_distance - 1);
                var outVec = (verticeWorldPos - centerForwardPos).normalized;
                var outPos = centerForwardPos + outVec * harf;

                vertices[i] = outPos - this.transform.position;
                verticleCount++;
            }
            else
            {
                verticleCount = 0;
            }
        }


        m_mesh = new Mesh();
        m_mesh.Clear();
        m_mesh.vertices = vertices;
        m_mesh.triangles = triangles;

        MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = m_mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = m_isTrigger;
        meshCollider.sharedMesh.RecalculateBounds();
        meshCollider.sharedMesh.RecalculateNormals();
        meshCollider.hideFlags = HideFlags.HideInInspector;
        this.transform.rotation = initRot;


        if (m_isFixScale)
        {
            var scale = Vector3.one;
            var parent = this.transform.parent;
            while (true)
            {
                if (parent != null)
                {
                    scale.x *= parent.localScale.x;
                    scale.y *= parent.localScale.y;
                    scale.z *= parent.localScale.z;
                    parent = parent.transform.parent;
                }
                else
                {
                    break;
                }
            }
            scale.x = 1.0f / scale.x;
            scale.y = 1.0f / scale.y;
            scale.z = 1.0f / scale.z;
            this.transform.localScale = scale;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    GameObject DebugObject(Vector3 pos, float scale = 1.0f, string name = "Sphere")
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = pos;
        obj.transform.localScale = new Vector3(scale, scale, scale);
        obj.name = name;
        return obj;
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(ConeCollider))]
[CanEditMultipleObjects]
public class ConeColliderEditor : Editor
{
    private SerializedProperty m_angle;
    private SerializedProperty m_distance;
    private SerializedProperty m_isTrigger;
    private SerializedProperty m_isFixScale;
    private ConeCollider m_conecollider;

    void OnEnable()
    {
        SetProperty(ref m_angle, "m_angle");
        SetProperty(ref m_distance, "m_distance");
        SetProperty(ref m_isTrigger, "m_isTrigger");
        SetProperty(ref m_isFixScale, "m_isFixScale");
        m_conecollider = target as ConeCollider;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        {
            DrawPropertyField(m_angle, "Angle");
            DrawPropertyField(m_distance, "Distance");
            DrawPropertyField(m_isTrigger, "isTrigger");
            DrawPropertyField(m_isFixScale, "isFixScale");
        }
        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            m_distance.floatValue = m_distance.floatValue < 1.0f ? 1.0f : m_distance.floatValue;
            var centerForward = m_conecollider.transform.position + m_conecollider.transform.TransformDirection(Vector3.forward) * m_distance.floatValue;
            var harf = m_distance.floatValue * Mathf.Tan(m_angle.floatValue * Mathf.PI / 180f);
            var up = centerForward + m_conecollider.transform.TransformDirection(Vector3.up) * harf;
            var down = centerForward + m_conecollider.transform.TransformDirection(Vector3.down) * harf;
            var right = centerForward + m_conecollider.transform.TransformDirection(Vector3.right) * harf;
            var left = centerForward + m_conecollider.transform.TransformDirection(Vector3.left) * harf;
            Handles.color = new Color(0.53f, 0.82f, 0.5f);
            Handles.DrawLine(m_conecollider.transform.position, up);
            Handles.DrawLine(m_conecollider.transform.position, down);
            Handles.DrawLine(m_conecollider.transform.position, right);
            Handles.DrawLine(m_conecollider.transform.position, left);
            Handles.CircleHandleCap(0, centerForward, m_conecollider.transform.rotation, harf, EventType.Repaint);
            Handles.color = Color.white;
        }
    }

    void SetProperty(ref SerializedProperty property, string name)
    {
        property = serializedObject.FindProperty(name);
    }

    void DrawPropertyField(SerializedProperty property, string name)
    {
        EditorGUILayout.PropertyField(property, new GUIContent(name));
    }
}

#endif