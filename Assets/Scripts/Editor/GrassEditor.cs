using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grass))]
public class GrassEditor : Editor
{
    Grass Grass;

    public override void OnInspectorGUI()
    {
        Grass = target as Grass;
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            Grass.RemoveChildren();
            Grass.ResetGrass();
            Grass.RenderGrass();
        }
    }

    private void OnSceneGUI()
    {
        Grass = target as Grass;

        Handles.ScaleHandle(Grass.transform.localScale, Grass.transform.position, Grass.transform.rotation, 5.0f);
        //Handles.PositionHandle(Grass.transform.position, Grass.transform.rotation);
    }
}
