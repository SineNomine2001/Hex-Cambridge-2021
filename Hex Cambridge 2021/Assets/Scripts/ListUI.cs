using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ListUI : MonoBehaviour
{
    protected static ListUI s_Instance;
    public static ListUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<ListUI>();

            return s_Instance;
        }
    }

    public TMP_Dropdown Year;
    public TMP_Dropdown Month;
    public TMP_Text Total;
    public RectTransform PanelTransform;
    public ScrollRect Scroll;

    List<Entry> entryList = new List<Entry>();
    List<string> entryStrings = new List<string>();

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;

        SetYearList();
        ReadFromFile();
    }

    public void SetYearList()
    {
        Year.ClearOptions();
        int y = DateTime.Now.Year;
        for (int i = 0; i <= 20; i++)
            Year.options.Add(new TMP_Dropdown.OptionData((y - i).ToString()));
    }

    public void ResetUI()
    {
        Scroll.verticalNormalizedPosition = 1.0f;
    }

    public void ShowCurrentMonthEntries()
    {
        Scroll.verticalNormalizedPosition = 1.0f;

        foreach (Transform child in PanelTransform)
            Destroy(child.gameObject);

        if(entryList.Count != 0)
        {
            string curMonth = "";
            if (Month.value + 1 < 10)
            {
                curMonth = Year.options[Year.value].text + "-0" + (Month.value + 1).ToString();
            }
            else
            {
                curMonth = Year.options[Year.value].text + (Month.value + 1).ToString();
            }
            List<Entry> displayedEntries = new List<Entry>();
            foreach (Entry entry in entryList)
            {
                if (entry.date.Contains(curMonth))
                    displayedEntries.Add(entry);
            }
            displayedEntries.Sort((s1, s2) => s1.date.CompareTo(s2.date));

            float total = 0;
            PanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 15);
            foreach (Entry entry in displayedEntries)
            {
                GameObject entryPrefab = Resources.Load<GameObject>("Prefabs/Entry");
                GameObject go = Instantiate(entryPrefab, PanelTransform);
                go.GetComponent<EntryUI>().Set(entry);
                total += float.Parse(entry.amount);
                PanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PanelTransform.rect.height + 300);
            }

            Total.text = "Total: £" + Math.Round(total, 2);
        }
    }

    public void ReadFromFile()
    {
        if (File.Exists(GetDirectory()))
        {
            entryStrings = File.ReadAllLines(GetDirectory()).ToList();
            entryList = new List<Entry>();
            foreach (string item in entryStrings)
            {
                string[] values = item.Split(',');
                entryList.Add(new Entry(values[0], values[1], values[2]));
            }
        }

        ShowCurrentMonthEntries();
    }

    public void SaveToFile(string content)
    {
        entryStrings.Add(content);
        string[] values = content.Split(',');
        entryList.Add(new Entry(values[0], values[1], values[2]));

        string filePath = GetDirectory();

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(content);
        }

        ShowCurrentMonthEntries();

        Debug.Log($"CSV file written to /" + filePath);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public string GetDirectory()
    {
        // The target file path e.g.
#if UNITY_EDITOR
        string folder = Application.dataPath;

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
#else
        var folder = Application.persistentDataPath;
#endif

        string filePath = Path.Combine(folder, "export.csv");

        return filePath;
    }
}
