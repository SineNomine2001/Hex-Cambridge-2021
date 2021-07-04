using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputUI : MonoBehaviour
{
    protected static InputUI s_Instance;
    public static InputUI Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;
            s_Instance = FindObjectOfType<InputUI>();

            return s_Instance;
        }
    }

    public Canvas canvas;
    public Image IncomeButtonImage;
    public Image ExpenseButtonImage;
    public TMP_Dropdown Year;
    public TMP_Dropdown Month;
    public TMP_Dropdown Day;
    public TMP_Dropdown Type;
    public TMP_InputField Amount;

    public TMP_Text DayLabel;
    public TMP_Text TypeLabel;

    bool IsInputIncome = true;

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
        SetTypeList();
        Amount.onValidateInput += ValidateInput;
    }

    public void ToggleVisibility()
    {
        canvas.enabled = !canvas.enabled;
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
        List<int> m30 = new List<int>{ 1, 3, 5, 8, 10 };
        if (Month.value == 1 && int.Parse(Year.options[Year.value].text) % 4 != 0)
            d = 28;
        else if (Month.value == 1 && int.Parse(Year.options[Year.value].text) % 4 == 0)
            d = 29;
        else if (m30.Contains(Month.value))
            d = 30;
        else
            d = 31;
        for(int i = 1; i <= d; i++)
            Day.options.Add(new TMP_Dropdown.OptionData(i.ToString()));

        DayLabel.text = Day.options[Day.value].text;
    }

    public void SetTypeList()
    {
        Type.ClearOptions();
        if (IsInputIncome)
        {
            foreach(IncomeType item in Enum.GetValues(typeof(IncomeType)))
                Type.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
        }
        else
        {
            foreach (ExpenseType item in Enum.GetValues(typeof(ExpenseType)))
                Type.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
        }

        TypeLabel.text = Type.options[Type.value].text;
    }

    public void OnIncomeButtonClick()
    {
        IsInputIncome = true;

        Color temp = IncomeButtonImage.color;
        temp.a = 0;
        IncomeButtonImage.color = temp;
        temp = ExpenseButtonImage.color;
        temp.a = 1;
        ExpenseButtonImage.color = temp;
        SetTypeList();
    }

    public void OnExpenseButtonClick()
    {
        IsInputIncome = false;

        Color temp = ExpenseButtonImage.color;
        temp.a = 0;
        ExpenseButtonImage.color = temp;
        temp = IncomeButtonImage.color;
        temp.a = 1;
        IncomeButtonImage.color = temp;
        SetTypeList();
    }
    
    public void OnConfirmButtonClick()
    {
        if (Amount.text != "")
        {
            ListUI.Instance.SaveToFile(InputToString());
            ToggleVisibility();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(OverlayUI.Instance.ShowWarningText());
        }
    }

    public string InputToString()
    {
        string str = "";
        if (Month.value + 1 < 10)
        {
            str = Year.options[Year.value].text + "-0" + (Month.value + 1).ToString() + "-" + Day.options[Day.value].text + ",";
        }
        else
        {
            str = Year.options[Year.value].text + (Month.value + 1).ToString() + "-" + Day.options[Day.value].text + ",";
        }
        if (!IsInputIncome)
            str += "-";
        str += Amount.text + "," + Type.options[Type.value].text;
        Debug.Log(str);
        return str;
    }

    public char ValidateInput(string text, int charIndex, char addedChar)
    {
        char output = addedChar;

        if (addedChar != '1'
            && addedChar != '2'
            && addedChar != '3'
            && addedChar != '4'
            && addedChar != '5'
            && addedChar != '6'
            && addedChar != '7'
            && addedChar != '8'
            && addedChar != '9'
            && addedChar != '0'
            && addedChar != '.'
            || (addedChar == '.' && text.Contains(".")))
        {
            //return a null character
            output = '\0';
        }

        return output;
    }
}
