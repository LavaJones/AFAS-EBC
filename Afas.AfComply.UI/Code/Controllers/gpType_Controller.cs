using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for gpType_Controller
/// </summary>
public class gpType_Controller
{
    public static List<gpType> getEmployeeTypes(int _employerID)
    {
        gpType_Factory gpTF = new gpType_Factory();
        return gpTF.manufactureGrossPayType(_employerID);
    }

    public static gpType manufactureGrossPayType(int _employerID, string _extID, string _name, bool _active)
    {
        gpType_Factory gpTF = new gpType_Factory();
        return gpTF.manufactureGrossPayType(_employerID, _extID, _name, _active);
    }

    public static gpType validateGpType(int _employerID, string _gpExtID, string _gpName, List<gpType> gpTempList)
    {
        gpType_Show gps = new gpType_Show();
        return gps.validateGpType(_employerID, _gpExtID, _gpName, gpTempList);
    }

    public static List<gpType> manufactureGrossPayFilter(int _employerID)
    {
        gpType_Factory gpTF = new gpType_Factory();
        return gpTF.manufactureGrossPayFilter(_employerID);
    }

    public static gpType manufactureGrossPayFilter(int _gpID, int _employerID)
    {
        gpType_Factory gpTF = new gpType_Factory();
        return gpTF.manufactureGrossPayFilter(_gpID, _employerID);
    }

    public static Boolean ValidateGpTypeFilter(String _gpExtID, List<gpType> gpTempList)
    {

        gpType_Show gpS = new gpType_Show();
        
        return gpS.ValidateGpTypeFilter(_gpExtID, gpTempList);
    
    }

    public static bool removeGrossPayFilter(int _gpID)
    {
        gpType_Factory gpTF = new gpType_Factory();
        return gpTF.removeGrossPayFilter(_gpID);
    }

    public static bool updateGpDescription(int _gpID, string _name)
    {
        gpType_Factory gpTF = new gpType_Factory();
        return gpTF.updateGpDescription(_gpID, _name);
    }


    /// <summary>
    /// This will take two different Gross Pay ID's and merget them into one. 
    /// We needed to add this as there are many instances where the leading zeros were missing in 
    /// reworked files and we needed a way to correct this in the software as it will continue to 
    /// happen when the users are manually manipulating the payroll data. 
    /// </summary>
    /// <returns></returns>
    public static bool mergeGrossPayType(int _gpID, int _gpID2)
    {
        gpType_Factory gpTF = new gpType_Factory();
        return gpTF.mergeGrossPayType(_gpID, _gpID2);
    }
}