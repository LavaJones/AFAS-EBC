using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for hrStatus_Controller
/// </summary>
public static class hrStatus_Controller
{
    public static List<hrStatus> manufactureHRStatusList(int _employerID)
    {
        hrStatus_Factory hsf = new hrStatus_Factory();
        return hsf.manufactureHRStatusList(_employerID);
    }

    public static hrStatus manufactureHrStatus(int _employerID, string _extID, string _name, bool _active)
    {
        hrStatus_Factory hsf = new hrStatus_Factory();
        return hsf.manufactureHrStatus(_employerID, _extID, _name, _active);
    }

    public static int validateHRStatus(int _employerID, string _hrExtStatusID, string _hrExtStatusName, List<hrStatus> _hrTempList)
    {
        hrStatus_Show hss = new hrStatus_Show();
        return hss.validateHRStatus(_employerID, _hrExtStatusID, _hrExtStatusName, _hrTempList);
    }

    public static bool updateHrStatus(int _hrStatusID, string _name)
    {
        hrStatus_Factory hsf = new hrStatus_Factory();
        return hsf.updateHrStatus(_hrStatusID, _name);
    }
}