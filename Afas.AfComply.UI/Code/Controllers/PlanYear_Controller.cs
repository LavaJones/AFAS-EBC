using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PlanYear_Controller
/// </summary>
public class PlanYear_Controller
{
    public static List<PlanYear> getEmployerPlanYear(int _employerID)
    {
        PlanYear_Factory pyf = new PlanYear_Factory();
        return pyf.manufacturePlanYear(_employerID);
    }

    public static int manufactureNewPlanYear(int _employerID, string _name, DateTime _startDate, DateTime _endDate, string _notes, string _history, DateTime _modOn, string _modBy,
        DateTime? default_Meas_Start, DateTime? default_Meas_End, DateTime? default_Admin_Start, DateTime? default_Admin_End,
        DateTime? default_Open_Start, DateTime? default_Open_End, DateTime? default_Stability_Start, DateTime? default_Stability_End, int PlanYearGroupId)
    {
        PlanYear_Factory pyf = new PlanYear_Factory();
        return pyf.manufactureNewPlanYear(_employerID, _name, _startDate, _endDate, _notes, _history, _modOn, _modBy, 
            default_Meas_Start, default_Meas_End, default_Admin_Start, default_Admin_End,
            default_Open_Start, default_Open_End, default_Stability_Start, default_Stability_End, PlanYearGroupId);
    }

    public static bool updatePlanYear(int _planyearID, string _name, DateTime _startDate, DateTime _endDate, string _notes, string _history, DateTime _modOn, string _modBy, 
        DateTime? default_Meas_Start, DateTime? default_Meas_End, DateTime? default_Admin_Start, DateTime? default_Admin_End,
        DateTime? default_Open_Start, DateTime? default_Open_End, DateTime? default_Stability_Start, DateTime? default_Stability_End, int PlanYearGroupId)
    {
        PlanYear_Factory pyf = new PlanYear_Factory();
        return pyf.updatePlanYear(_planyearID, _name, _startDate, _endDate, _notes, _history, _modOn, _modBy,
            default_Meas_Start, default_Meas_End, default_Admin_Start, default_Admin_End,
            default_Open_Start, default_Open_End, default_Stability_Start, default_Stability_End, PlanYearGroupId);
    }

    public static DataTable GetNewImportOfferDataTable()
    {

        DataTable offers = new DataTable();

        offers.Columns.Add("employee_id", typeof(int));
        offers.Columns.Add("plan_year_id", typeof(int));
        offers.Columns.Add("employer_id", typeof(int));
        offers.Columns.Add("insurance_id", typeof(int));
        offers.Columns.Add("ins_cont_id", typeof(int));
        offers.Columns.Add("offered", typeof(bool));
        offers.Columns.Add("offeredOn", typeof(DateTime));
        offers.Columns.Add("accepted", typeof(bool));
        offers.Columns.Add("acceptedOn", typeof(DateTime));
        offers.Columns.Add("modOn", typeof(DateTime));
        offers.Columns.Add("modBy", typeof(String));
        offers.Columns.Add("notes", typeof(String));
        offers.Columns.Add("history", typeof(String));
        offers.Columns.Add("effectiveDate", typeof(DateTime));
        offers.Columns.Add("hra_flex_contribution", typeof(double));

        return offers;
    }

    public static PlanYear findPlanYear(int _planID, int _employerID)
    {
        PlanYear_Show pys = new PlanYear_Show();
        return pys.findPlanYear(_planID, _employerID);
    }

    public static List<PlanYear> filterFuturePlanYears(List<PlanYear> allPY)
    {
        PlanYear_Show pys = new PlanYear_Show();
        return pys.filterFuturePlanYears(allPY);
    }

    public static List<PlanYear> findNewHirePlanYear(List<PlanYear> _currentPlanYears, DateTime _hireDate, int _employerID)
    {
        PlanYear_Show pys = new PlanYear_Show();
        return pys.findNewHirePlanYear(_currentPlanYears, _hireDate, _employerID);
    }

    public static bool validateNewHire(List<PlanYear> _currentPlanYears, DateTime _hireDate, int _employerID)
    {
        PlanYear_Show pys = new PlanYear_Show();
        return pys.validateNewHire(_currentPlanYears, _hireDate, _employerID);
    }

    /// <summary>
    /// The Below function takes Employee as parameter and returns appropriate PlanYearID of that employee. 
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    public static string GetEmployeePlanYearId(Employee employee)
    {
        List<PlanYear> planYears = getEmployerPlanYear(employee.EMPLOYEE_EMPLOYER_ID);

        int planYearGroupId = (from plan in planYears 
                               where plan.PLAN_YEAR_ID == employee.EMPLOYEE_PLAN_YEAR_ID_MEAS
                               select plan.PlanYearGroupId).First();

        string planYearId= (from PlanYear year in planYears
                where   year.PlanYearGroupId == planYearGroupId &&
                        year.PLAN_YEAR_START < employee.EMPLOYEE_HIRE_DATE &&
                        year.PLAN_YEAR_END > employee.EMPLOYEE_HIRE_DATE
                orderby year.PLAN_YEAR_START
                select  year.PLAN_YEAR_ID).FirstOrDefault().ToString();


        return planYearId;
    }
}