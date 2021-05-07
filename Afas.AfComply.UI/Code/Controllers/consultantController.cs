using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Afas.AfComply.UI.Code.Caching;

/// <summary>
/// Summary description for consultantController
/// </summary>
public static class consultantController
{

    public static Boolean InsertUpdateEmployerConsultant(
        string _name,
        string _title,
        int _phoneNumber,
        int _employerId,
        string _crtBy
        )
    {

        // CacheManager.EmployeeClassificationsInvalidate(_employerID);(check with Long about cache manager)
        return new consultantFactory().InsertUpdateEmployerConsultant(_name, _title, _phoneNumber, _employerId, _crtBy);

    }
    public static List<Consultant> getEmployerConsultant()
    {
        consultantFactory cf = new consultantFactory();
        return cf.getEmployerConsultant();
    }
}






























