using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverlayUI : MonoBehaviour
{
    protected static OverlayUI s_Instance;
    public static OverlayUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<OverlayUI>();

            return s_Instance;
        }
    }

    float fadeTime = 1f;

    public TMP_Text WarningText;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
    }
    public IEnumerator ShowWarningText()
    {
        WarningText.text = "Please enter all data correctly!";

        Color temp = WarningText.color;
        temp.a = 0f;
        Color transparent = temp;
        temp.a = 1f;
        Color opaque = temp;

        for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
        {
            WarningText.color = Color.Lerp(opaque, transparent, t / fadeTime);
            yield return null;
        }
    }
}
