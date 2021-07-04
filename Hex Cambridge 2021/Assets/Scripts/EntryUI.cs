using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class EntryUI : MonoBehaviour
{
    public TMP_Text date;
    public TMP_Text amount;
    public TMP_Text type;

    public void Set(Entry entry)
    {
        date.text = entry.date;
        amount.text = "£" + Math.Round(float.Parse(entry.amount), 2);
        type.text = entry.type;
    }
}
