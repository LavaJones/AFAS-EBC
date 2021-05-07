using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for alert_show
/// </summary>
public class alert_show
{
    public alert_insurance findSingleInsuranceAlert(List<alert_insurance> tempList, int _rowID)
    {
        alert_insurance tempAI = null;

        foreach (alert_insurance ai in tempList)
        {
            if (ai.ROW_ID == _rowID)
            {
                tempAI = ai;
                break;
            }
        }

        return tempAI;
    }
}