using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;


public partial class AdminAuto_py_rollover : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(AdminAuto_py_rollover));
    

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {

        LitUserName.Text = user.User_UserName;
        loadGrid();
    }

    private void loadGrid()
    {
        List<EmployersMeasurementPeriodDetails> Employers = employerController.getAllEmployersMeasurementPeriodDetails();
        if (Employers.Count>0)
        {
            GvEmployerAdminDetails.DataSource = Employers.Select(empr => new {  empr.EmployerId,
                                                                                empr.EmployerName,
                                                                                empr.PlanYear,
                                                                                empr.PlanYearID,
                                                                                empr.AdminEnd,
                                                                                empr.AdminStart,
                                                                                empr.StabilityStart,
                                                                                empr.StabilityEnd
                                                                            }).Distinct().ToList();
            GvEmployerAdminDetails.DataBind();
        }
        else
        {
            btnRollover.Visible = false;
            GvEmployerAdminDetails.DataSource = null;
            GvEmployerAdminDetails.DataBind();
        }
        
    }


    protected void GvEmployerAdminDetails_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        List<EmployersMeasurementPeriodDetails> Employers = employerController.getAllEmployersMeasurementPeriodDetails();
        GvEmployerAdminDetails.PageIndex =e.NewPageIndex;
        GvEmployerAdminDetails.DataSource = Employers.Select(empr => new {  empr.EmployerId,
                                                                            empr.EmployerName,
                                                                            empr.PlanYear,
                                                                            empr.PlanYearID,
                                                                            empr.AdminEnd,
                                                                            empr.AdminStart,
                                                                            empr.StabilityStart,
                                                                            empr.StabilityEnd
                                                                        }).Distinct().ToList(); GvEmployerAdminDetails.DataBind();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
    protected void BtnRollover_Click(object sender, EventArgs e)
    {
        List<EmployersMeasurementPeriodDetails> Employers = employerController.getAllEmployersMeasurementPeriodDetails();
        Dictionary<int, string> logDictionary =  new Dictionary<int, string>();
        bool validData = true;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;
        try
        {
            foreach (EmployersMeasurementPeriodDetails employer in Employers)
            {
                    Measurement m = measurementController.getPlanYearMeasurement(employer.EmployerId, employer.PlanYearID, employer.EmployeeTypeID);
                    List<Employee> tempEmpList = EmployeeController.manufactureEmployeeList(employer.EmployerId);
                    List<Employee> filteredForEmployeeType = (
                                                       from Employee employee in tempEmpList
                                                       where employee.EMPLOYEE_TYPE_ID == employer.EmployeeTypeID
                                                       && employee.EMPLOYEE_PLAN_YEAR_ID_LIMBO == employer.PlanYearID
                                                       select employee
                                                      ).ToList();

                    if (filteredForEmployeeType.Count > 0)
                    {
                        logDictionary.Add(employer.EmployerId, String.Format("For Employee Type {0} for Employer {1} we have {2} employee(s) assigned to that type.",
                                                  employer.EmployeeTypeID,
                                                  employer.EmployerId,
                                                  filteredForEmployeeType.Count));
                        validData = EmployeeController.RolloverAdministrativePeriod(employer.EmployerId, employer.EmployeeTypeID, employer.PlanYearID, _modOn, _modBy);

                        if (validData == true)
                        {

                            employerController.insertEmployerCalculation(employer.EmployerId);

                            LblRolloverMessage.Text = "The Administrative Period has been rolled over.";
                        LblRolloverMessage.BorderColor = System.Drawing.Color.Green;
                        }
                        else
                        {

                            this.Log.Warn(String.Format("For Employee Type {0}, Admin Plan Year {1} for Employer {2} EmployeeController.rolloverAdministrativePeriod returned false.",
                                            employer.EmployeeTypeID, employer.PlanYearID, employer.EmployerId));

                            LblRolloverMessage.Text = "An error occurred while trying to Rollover the Administrative Period, please try again.";
                            LblRolloverMessage.BorderColor = System.Drawing.Color.Red;
                            break;

                        }
                    }
                   
                 }
            LogInfo(logDictionary);
            loadGrid();
        }
                catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            LblRolloverMessage.Text = "An error occurred while calculating the Administrative End Date. Please verify that the Employer has setup the Measurement Start and End dates for this Plan Year.";
         
        }
    }

    private void LogInfo(Dictionary<int, string> logDictionary)
    {
        foreach (KeyValuePair<int, string> entry in logDictionary)
        {
            this.Log.Info(entry.Value);
        }
    }
}
