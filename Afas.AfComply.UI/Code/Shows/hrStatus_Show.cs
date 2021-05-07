using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for hrStatus_Show
/// </summary>
public class hrStatus_Show
{
    private ILog Log = LogManager.GetLogger(typeof(hrStatus_Show));

    /// <summary>
    /// This function is meant to pass data existing HR data into it and validate it. If the HR Status that is passed in doesn't exits
    /// it must be created. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_hrExtStatusID"></param>
    /// <param name="_hrExtStatusName"></param>
    /// <param name="hrTempList"></param>
    /// <returns></returns>
    public int validateHRStatus(int _employerID, string _hrExtStatusID, string _hrExtStatusName, List<hrStatus> hrTempList)
    {
        int _hrStatusID = 0;
        try
        {
            if (_hrExtStatusID.Trim() != "")
            {
                foreach (hrStatus h in hrTempList)
                {
                    if (String.Compare(h.HR_STATUS_EXTERNAL_ID, _hrExtStatusID, true) == 0)
                    {
                        _hrStatusID = h.HR_STATUS_ID;
                        if (String.Compare(h.HR_STATUS_NAME, _hrExtStatusName, true) != 0)
                        {
                            hrStatus_Controller.updateHrStatus(h.HR_STATUS_ID, _hrExtStatusName);
                        }
                        break;
                    }
                }

                if (_hrStatusID == 0)
                {
                    hrStatus h = hrStatus_Controller.manufactureHrStatus(_employerID, _hrExtStatusID, _hrExtStatusName, true);
                    _hrStatusID = h.HR_STATUS_ID;
                    hrTempList.Add(h);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }


        return _hrStatusID;
    }
}