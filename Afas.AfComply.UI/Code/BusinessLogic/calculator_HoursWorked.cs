using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for calculator_AverageHoursWorked
/// </summary>
public static class calculator_HoursWorked
{
    private static ILog Log = LogManager.GetLogger(typeof(calculator_HoursWorked));

    /// <summary>
    /// Loop through all payroll records and come up with a sum of hours. 
    /// </summary>
    /// <param name="tempList"></param>
    /// <param name="_sDate"></param>
    /// <param name="_eDate"></param>
    /// <returns></returns>
    public static double sum_Hours(List<Payroll> tempList, DateTime _sDate, DateTime _eDate)
    {
        double totalHours = 0;

        foreach (Payroll p in tempList)
        {
            totalHours += calculateSinglePayrollRecordHours(p, _sDate, _eDate);
        }

        return totalHours;
    }

    /// <summary>
    /// Scan a single Payroll record to see if the hours need to be split up based on overlapping dates. 
    /// </summary>
    /// <param name="p"></param>
    /// <param name="_sDate"></param>
    /// <param name="_eDate"></param>
    /// <returns></returns>
    private static double calculateSinglePayrollRecordHours(Payroll p, DateTime _sDate, DateTime _eDate)
    {
        double daySpan = 0;
        double avgHours = 0;
        double ApplicableDays = 0;
        double ApplicableHours = 0;

        try
        {
            daySpan = calculator_TimeSpan.calculateDaySpan((DateTime)p.PAY_SDATE, (DateTime)p.PAY_EDATE);

            avgHours = (double)p.PAY_HOURS / daySpan;

            if (p.PAY_SDATE < _sDate && p.PAY_EDATE > _sDate)
            {
                if (p.PAY_EDATE > _eDate)
                {
                    ApplicableDays = calculator_TimeSpan.calculateDaySpan(_sDate, _eDate);
                }
                else
                {
                    ApplicableDays = calculator_TimeSpan.calculateDaySpan(_sDate, (DateTime)p.PAY_EDATE);
                }

                ApplicableHours = ApplicableDays * avgHours;
            }
            else if (p.PAY_SDATE >= _sDate && p.PAY_SDATE < _eDate && p.PAY_EDATE >= _eDate)
            {
                ApplicableDays = calculator_TimeSpan.calculateDaySpan((DateTime)p.PAY_SDATE, _eDate);
                ApplicableHours = ApplicableDays * avgHours;
            }
            else if (p.PAY_SDATE >= _sDate && p.PAY_EDATE <= _eDate)
            {
                ApplicableHours = (double)p.PAY_HOURS;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            ApplicableHours = 0;
        }

        return ApplicableHours;
    }
}