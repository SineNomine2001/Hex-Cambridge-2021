using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEditor.Scripting.Python;

public class ChartUI : MonoBehaviour
{
    protected static ChartUI s_Instance;
    public static ChartUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<ChartUI>();

            return s_Instance;
        }
    }

    public TMP_Dropdown Year;
    public TMP_Dropdown Month;
    public TMP_Dropdown Day;
    public TMP_Dropdown TimeSpan;
    public TMP_Dropdown ChartType;
    public TMP_Text DayLabel;

    public Image Chart1;
    public Image Chart2;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;

        SetYearList();
        SetDayList();
    }

    public void SetYearList()
    {
        Year.ClearOptions();
        int y = DateTime.Now.Year;
        for (int i = 0; i <= 20; i++)
            Year.options.Add(new TMP_Dropdown.OptionData((y - i).ToString()));
    }

    public void SetDayList()
    {
        Day.ClearOptions();
        int d;
        List<int> m30 = new List<int> { 1, 3, 5, 8, 10 };
        if (Month.value == 1 && int.Parse(Year.options[Year.value].text) % 4 != 0)
            d = 28;
        else if (Month.value == 1 && int.Parse(Year.options[Year.value].text) % 4 == 0)
            d = 29;
        else if (m30.Contains(Month.value))
            d = 30;
        else
            d = 31;
        for (int i = 1; i <= d; i++)
            Day.options.Add(new TMP_Dropdown.OptionData(i.ToString()));

        DayLabel.text = Day.options[Day.value].text;
    }

    public void ShowChart()
    {
        switch (ChartType.value)
        {
            case 0:
                Chart1.sprite = Resources.Load<Sprite>("Images/income");
                Chart2.sprite = Resources.Load<Sprite>("Images/expense");
                break;
            case 1:
                Chart1.sprite = Resources.Load<Sprite>("Images/d1inpie");
                Chart2.sprite = Resources.Load<Sprite>("Images/d1expie");
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }
    }

    public void SaveChartInstructions()
    {
        string filePath = GetDirectory();

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("date,type\n" + Year.options[Year.value].text + "-" + (Month.value + 1).ToString() + "-" + Day.options[Day.value].text + "," + TimeSpan.value);
        }

        string path = Application.dataPath + "/hackathon_graph_generator.py";
        PythonRunner.RunFile(path);
        ShowChart();

        Debug.Log($"Chart Instructions written to /" + filePath);

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

        string filePath = Path.Combine(folder, "instructions.csv");

        return filePath;
    }
}
