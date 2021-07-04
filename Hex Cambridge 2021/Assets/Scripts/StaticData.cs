using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{

}

public enum IncomeType
{
    Salary,
    Youtube,
    Investment,
    Gift,
    Other
}

public enum ExpenseType
{
    Food,
    Shopping,
    Transport,
    Fitness,
    Entertainment,
    Social,
    Travel
}

public struct Entry
{
    public readonly string date;
    public readonly string amount;
    public readonly string type;

    internal Entry(string date, string amount, string type)
    {
        this.date = date;
        this.amount = amount;
        this.type = type;
    }
}
