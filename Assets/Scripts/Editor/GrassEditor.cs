using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GrassTest))]
public class GrassEditor : Editor
{
    GrassTest Grass;

    public override void OnInspectorGUI()
    {
        Grass = target as GrassTest;
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            Grass.RemoveChildren();
            Grass.ResetGrass();
            Grass.RenderGrass();
        }
    }

}
