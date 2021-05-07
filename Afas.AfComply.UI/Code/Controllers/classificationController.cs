using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using log4net;

using Afas.AfComply.UI.Code.Caching;

/// <summary>
/// Summary description for classificationController
/// </summary>
public static class classificationController
{

    private static List<classification_aca> acaStatusList = new List<classification_aca>();

    public static List<classification_aca> getACAstatusList()
    {
        try
        {
            if (acaStatusList.Count < 1)
            {
                acaStatusList = manufactureACAstatusList();
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            acaStatusList = manufactureACAstatusList();
        }

        return acaStatusList;
    }

    public static List<classification_aca> manufactureACAstatusList()
    {
        classificationFactory cf = new classificationFactory();
        return cf.manufactureACAstatusList();
    }

    /// <summary>
    /// Returns the list of employee classifications for the employer.
    /// If useCache is false it retrieves it from the underlying database.
    /// </summary>
    public static List<classification> ManufactureEmployerClassificationList(int _employerID, Boolean useCache)
    {

        if (useCache)
        {
            return CacheManager.EmployeeClassifications(_employerID);
        }

        return new classificationFactory().ManufactureEmployerClassificationList(_employerID);
    
    }

    public static List<EmployeeClassificationInsurance> getEmployeeClassificationByPlanYearAndEmployer(int employerId, int planYearId)
    {
        return new classificationFactory().getEmployeeClassificationByPlanYearAndEmployer(employerId, planYearId);
    }


    /// <summary>
    /// Creates a new classification with the passed values.
    /// Invalidated the employer class cache for this specific employer from the session cache.
    /// </summary>
    public static Boolean ManufactureEmployeeClassification(
            int _employerID, 
            string _desc, 
            string _ashCode, 
            DateTime _modOn, 
            string _modBy, 
            string _history,
            int? _waitingPeriodID,
            string _ooc
        )
    {

        CacheManager.EmployeeClassificationsInvalidate(_employerID);

        return new classificationFactory().ManufactureEmployeeClassification(_employerID, _desc, _ashCode, _modOn, _modBy, _history, _waitingPeriodID, _ooc);
    
    }

    /// <summary>
    /// Deletes the requested classification from the database.
    /// Invalidates all of the currently cached information for any/all employers in session.
    /// </summary>
    public static Boolean DeleteEmployeeClassification(int _classID)
    {

        CacheManager.EmployerInvalidate();

        return new classificationFactory().DeleteEmployeeClassification(_classID);

    }

    /// <summary>
    /// Updates the classification to the values passed.
    /// Invalidates all of the currently cached information for any/all employers in session.
    /// </summary>
    public static Boolean UpdateEmployeeClassification(int _classID, String _desc, String _ashCode, DateTime _modOn,  String _modBy, String _history,int _waitingPeriodID, int _entityStatusID, string _ooc)
    {
        CacheManager.EmployerInvalidate();
        classificationFactory cf = new classificationFactory();
        return cf.UpdateEmployeeClassification(_classID, _desc, _ashCode, _modOn, _modBy, _history, _waitingPeriodID, _entityStatusID, _ooc);
    }

    public static bool validClassification(int _classID, List<classification> tempList)
    {

        classificationShow cs = new classificationShow();
        
        return cs.validClassification(_classID, tempList);
    
    }

    public static classification findClassification(int _classID, List<classification> tempList)
    {
        classificationShow cs = new classificationShow();
        return cs.findClassification(_classID, tempList);
    }
   
    public static List<classification> getClassificationActiveOnly(List<classification> tempList)
    {
        classificationShow cs = new classificationShow();
        return cs.getClassificationActiveOnly(tempList);
    }

    private static ILog Log = LogManager.GetLogger(typeof(classificationController));
}