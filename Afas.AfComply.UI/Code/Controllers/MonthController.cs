using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MonthController
/// </summary>
public static class MonthController
{

    /// <summary>
    /// 
    /// </summary>
    private static List<Month> months = new List<Month>();

    public static List<Month> getMonths()
    {
        if (months.Count() < 1)
        {
            generateMonths();
            return months;
        }
        else
        {
            return months;
        }
    }

    public static void addMonth(Month _month)
    {
        months.Add(_month);
    }


    public static bool generateMonths()
    {
        if (months.Count() < 1)
        {
            MonthFactory mf = new MonthFactory();
            bool months_generated = false;

            months_generated = mf.manufactureMonthList();

            return months_generated;
        }
        else
        {
            return true;
        }
    }

   
}