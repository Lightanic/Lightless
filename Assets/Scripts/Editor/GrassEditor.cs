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
}
