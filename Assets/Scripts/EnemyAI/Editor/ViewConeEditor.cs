using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ViewConeComponent))]
public class ViewConeEditor : Editor
{
    private void OnSceneGUI()
    {
        ViewConeComponent view = (ViewConeComponent)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(view.transform.position, Vector3.up, Vector3.forward, 360, view.ViewRadius);
        Vector3 viewAngleA = view.DirFromAngle(-view.ViewAngle / 2, false);
        Vector3 viewAngleB = view.DirFromAngle(view.ViewAngle / 2, false);

        Handles.DrawLine(view.transform.position, view.transform.position + viewAngleA * view.ViewRadius);
        Handles.DrawLine(view.transform.position, view.transform.position + viewAngleB * view.ViewRadius);
    }
}
