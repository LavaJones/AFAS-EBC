using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for measurement_Show
/// </summary>
public class measurementShow
{
    public int getInitialMeasurementLength(int _id)
    {
        int months = 0;

        foreach (measurementInitial mi in measurementController.getInitialMeasurements())
        {
            if (mi.INITIAL_ID == _id)
            {
                months = mi.INITIAL_MONTHS;
                break;
            }
        }

        return months;
    }


}