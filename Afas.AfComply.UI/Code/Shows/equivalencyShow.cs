using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for equivalencyShow
/// </summary>
public class equivalencyShow
{
    public List<activity> getEbcActivities(int _positionID)
    {
        List<activity> tempList = new List<activity>();

        foreach (activity a in equivalencyController.getActivities())
        {
            if (a.ACTIVITY_POSITION_ID == _positionID)
            {
                tempList.Add(a);
            }
        }

        return tempList;
    }

    public List<detail> getEbcDetails(int _activityID)
    {
        List<detail> tempList = new List<detail>();

        foreach (detail a in equivalencyController.getDetails())
        {
            if (a.DETAIL_ACTIVITY_ID == _activityID)
            {
                tempList.Add(a);
            }
        }

        return tempList;
    }

    public detail getEbcSingleDetail(int _detailID)
    {
        detail tempDetail = null;

        foreach (detail d in equivalencyController.getDetails())
        {
            if (d.DETAIL_ID == _detailID)
            {
                tempDetail = d;
                break;
            }
        }

        return tempDetail;
    }

    public equivalency getSingleEquivalency(int _gpID, List<equivalency> _tempList)
    {
        equivalency tempEQ = null;

        foreach (equivalency eq in _tempList)
        {
            if (eq.EQUIV_GROSS_PAY_ID == _gpID)
            {
                tempEQ = eq;
                break;
            }
        }

        return tempEQ;
    }
}