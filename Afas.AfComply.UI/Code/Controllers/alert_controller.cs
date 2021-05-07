using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for alert_controller
/// </summary>
public static class alert_controller
{

    public static List<alert> manufactureEmployerAlertList(int _employerID)
    {

        alert_factory af = new alert_factory();
        
        return af.manufactureEmployerAlertListAll(_employerID).Where(alerts => alerts.ALERT_COUNT > 0).ToList();
    
    }

    public static List<alert_type> manufactureAlertTypeList()
    {
    
        alert_factory af = new alert_factory();
        
        return af.manufactureAlertTypeList();
    
    }

    public static List<alert_insurance> manufactureEmployerInsuranceAlertList(int _employerID)
    {

        insuranceFactory insF = new insuranceFactory();
        
        return insF.manufactureEmployerInsuranceAlertList(_employerID);
    
    }

    public static alert_insurance findSingleInsuranceAlert(List<alert_insurance> tempList, int _rowID)
    {

        alert_show als = new alert_show();

        return als.findSingleInsuranceAlert(tempList, _rowID);
    
    }

    public static bool manufactureEmployerAlert(int _employerID, int _alertTypeID)
    {
    
        alert_factory af = new alert_factory();
        
        return af.manufactureEmployerAlert(_employerID, _alertTypeID);
    
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static List<alert> manufactureEmployerAlertListAll(int _employerID)
    {

        alert_factory af = new alert_factory();
        
        return af.manufactureEmployerAlertListAll(_employerID);
    
    }

    public static bool deleteEmployerAlerts(int _employerID, int _alertID)
    {

        alert_factory af = new alert_factory();
        
        return af.deleteAlert(_employerID, _alertID);
    
    }

    public static bool deleteEmployerPayrollAlerts(int _employerID,string _modBy, DateTime _modOn)
    {

        alert_factory af = new alert_factory();
        
        return af.deleteEmployerPayrollAlerts(_employerID, _modBy, _modOn);
    
    }

    public static bool deleteEmployerDemographicAlerts(int _employerID,string _modBy, DateTime _modOn)
    {
    
        alert_factory af = new alert_factory();
        
        return af.deleteEmployerDemographicAlerts(_employerID, _modBy, _modOn);
    
    }

}