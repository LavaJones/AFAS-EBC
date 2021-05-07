using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for gpType_Show
/// </summary>
public class gpType_Show
{
    private ILog Log = LogManager.GetLogger(typeof(gpType_Show));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_gpExtID"></param>
    /// <param name="_gpName"></param>
    /// <param name="gpTempList"></param>
    /// <returns></returns>
    public gpType validateGpType(int _employerID, string _gpExtID, string _gpName, List<gpType> gpTempList)
    {
        gpType tempGP = null;

        try
        {
            if (_gpName != null)
            {
                if (_gpName.Trim() != "")
                {
                    foreach (gpType gp in gpTempList)
                    {
                        if (String.Compare(gp.GROSS_PAY_EXTERNAL_ID, _gpExtID, true) == 0)
                        {
                            tempGP = gp;
                            if (String.Compare(gp.GROSS_PAY_DESCRIPTION, _gpName, true) != 0)
                            {
                                gpType_Controller.updateGpDescription(gp.GROSS_PAY_ID, _gpName);
                                tempGP.GROSS_PAY_DESCRIPTION = _gpName;
                            }
                            break;
                        }
                    }

                    if (tempGP == null)
                    {
                        tempGP = gpType_Controller.manufactureGrossPayType(_employerID, _gpExtID, _gpName, true);
                    }
                }
            }
            else
            {
                foreach (gpType gp in gpTempList)
                {
                    if (String.Compare(gp.GROSS_PAY_EXTERNAL_ID, _gpExtID, true) == 0)
                    {
                        tempGP = gp;
                        if (String.Compare(gp.GROSS_PAY_DESCRIPTION, _gpName, true) != 0)
                        {
                            tempGP.GROSS_PAY_DESCRIPTION = _gpName;
                        }
                        break;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }

        return tempGP;

    }

    public Boolean ValidateGpTypeFilter(String _gpExtID, List<gpType> gpTempList)
    {

        Boolean filter = false;

        try
        {

            foreach (gpType gp in gpTempList)
            {

                if (String.Compare(gp.GROSS_PAY_EXTERNAL_ID, _gpExtID, true) == 0)
                {
                    
                    filter = true;
                    
                    break;

                }

            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);
            
            filter = false;
        
        }

        return filter;

    }
   
}