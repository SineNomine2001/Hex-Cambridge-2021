﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Scripting.Python;

[CustomEditor(typeof(PythonManager))]
public class PythonManagerEditor : Editor
{
    PythonManager targetManager;

    private void OnEnable()
    {
        targetManager = (PythonManager)target;
    }

    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Lauch Python Script", GUILayout.Height(35)))
        {
            string path = Application.dataPath + "/StreamingAssets/log_names.py";
            PythonRunner.RunFile(path);
        }
    }
}
