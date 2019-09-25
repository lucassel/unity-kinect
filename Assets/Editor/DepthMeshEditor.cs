using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DepthMesh))]

public class DepthMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var depth = (DepthMesh)target;


        if (GUILayout.Button("Write depth to txt"))
        {
            depth.WritePointCloud();
        }

        base.DrawDefaultInspector();
    }
}
