using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for calculator_DaySpan
/// </summary>
public static class calculator_TimeSpan
{
    /// <summary>
    /// Calculate the difference between two dates in Days. 
    /// If dates are the same, 0 will be returned. 
    /// </summary>
    /// <param name="_sDate"></param>
    /// <param name="_eDate"></param>
    /// <returns></returns>
	public static double calculateDaySpan(DateTime _sDate, DateTime _eDate)
    {
        double days = 0;

        _sDate = DateTime.Parse(_sDate.ToShortDateString());
        _eDate = DateTime.Parse(_eDate.ToShortDateString());

        days = (_eDate - _sDate).TotalDays;

        return days;
    }

    /// <summary>
    /// Calculate the difference between two dates in Days. 
    /// If dates are the same, 1 will be returned. 
    /// </summary>
    /// <param name="_sDate"></param>
    /// <param name="_eDate"></param>
    /// <returns></returns>
    public static double calculateDaySpanNoZero(DateTime _sDate, DateTime _eDate)
    {
        double days = 0;

        _sDate = DateTime.Parse(_sDate.ToShortDateString());
        _eDate = DateTime.Parse(_eDate.ToShortDateString());

        if (_sDate == _eDate)
        {
            days = 1;
        }
        else
        {
            days = (_eDate - _sDate).TotalDays;
        }

        return days;
    }

    public static double calculateMinuteSpan(DateTime _sDate, DateTime _eDate)
    {
        double minutes = 0;

        minutes = (_eDate - _sDate).TotalMinutes;

        return minutes;
    }

    public static double calculateWeekSpan(DateTime _sDate, DateTime _eDate)
    {
        double weeks = 0;

        weeks = ((_eDate - _sDate).TotalDays / 7);

        return weeks;
    }

    public static double calculateMonthSpan(DateTime _sDate, DateTime _eDate)
    {
        double months = 0;

        while (_sDate < _eDate)
        {
            if (_sDate.Month == _eDate.Month && _sDate.Year == _eDate.Year)
            {
                _sDate = _sDate.AddMonths(1);
            }
            else
            {
                months += 1;
                _sDate = _sDate.AddMonths(1);
            }
        }
        
        return months;
    }
}