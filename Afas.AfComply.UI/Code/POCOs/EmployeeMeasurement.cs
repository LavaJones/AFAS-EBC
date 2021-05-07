using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmployeeMeasurement
/// </summary>
public static class EmployeeMeasurement
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <param name="_hdate"></param>
    /// <param name="_imEnd"></param>
    /// <param name="_initialStart"></param>
    /// <param name="_initialEnd"></param>
    /// <returns></returns>
    public static bool transitionVerification(DateTime _hdate, DateTime _imEnd, Measurement _m)
    {
        bool transition = false;

        if (_hdate <= _m.MEASUREMENT_START && _m.MEASUREMENT_TYPE_ID == 1)
        {
            transition = true;
        }

        return transition;
    }

    public static bool initialVerification(DateTime _hdate, DateTime _imEnd, Measurement _m)
    {
        bool initial = false;

        if (_hdate > _m.MEASUREMENT_START && _hdate < _m.MEASUREMENT_END)
        {
            initial = true;
        }

        return initial;
    }

    public static bool ongoingVerification(Measurement _m)
    {
        bool ongoing = false;
        if (_m.MEASUREMENT_TYPE_ID == 2)
        {
            ongoing = true;
        }

        return ongoing;
    }
}