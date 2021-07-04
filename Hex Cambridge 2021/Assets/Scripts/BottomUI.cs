using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomUI : MonoBehaviour
{
    protected static BottomUI s_Instance;
    public static BottomUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<BottomUI>();

            return s_Instance;
        }
    }

    public Transform orange;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
    }

    public void MoveOrange(Transform target)
    {
        orange.position = target.position;
    }
}
