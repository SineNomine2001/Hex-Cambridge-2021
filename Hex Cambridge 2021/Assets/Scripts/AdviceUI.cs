using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEditor.Scripting.Python;

public class AdviceUI : MonoBehaviour
{
    protected static AdviceUI s_Instance;
    public static AdviceUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<AdviceUI>();

            return s_Instance;
        }
    }

    public TMP_Text adviceText;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;

        string path = Application.dataPath + "/model.py";
        PythonRunner.RunFile(path);
    }
}
