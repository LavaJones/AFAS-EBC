using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class admin_admin_py_rollover : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_admin_py_rollover));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;
        loadEmployers();
    }

    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        employer _employer = null;

        if (DdlEmployer.SelectedItem.Text != "Select")
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            _employer = employerController.getEmployer(_employerID);

            loadPlanYears(_employerID);
        }
        else
        {
            DdlPlanYearCurrent.Items.Clear();
        }
    }

    private void loadEmployers()
    {
        DdlEmployer.DataSource = employerController.getAllEmployers();
        DdlEmployer.DataTextField = "EMPLOYER_NAME";
        DdlEmployer.DataValueField = "EMPLOYER_ID";
        DdlEmployer.DataBind();

        DdlEmployer.Items.Add("Select");
        DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
    }

    /// <summary>
    /// 1-3) Loads a specific employer's Plan Years into a dropdown list. 
    /// </summary>
    /// <param name="_employerID"></param>
    private void loadPlanYears(int _employerID)
    {

        DdlPlanYearCurrent.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
        DdlPlanYearCurrent.DataTextField = "PLAN_YEAR_DESCRIPTION";
        DdlPlanYearCurrent.DataValueField = "PLAN_YEAR_ID";
        DdlPlanYearCurrent.DataBind();

        DdlPlanYearCurrent.Items.Add("Select");
        DdlPlanYearCurrent.SelectedIndex = DdlPlanYearCurrent.Items.Count - 1;
    }

    protected void BtnProcessFile_Click(object sender, EventArgs e)
    {

        int _employerID = 0;
        int _adminPlanYearID = 0;
        PlanYear py = null;
        Measurement m = null;
        bool validData = true;
        List<alert> al = null;
        int alertCount = 0;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;

        /************************** Validation Steps ****************************************
         * Step 1: Validate all drop down lists. 
         * Step 2: Verify that the employee does not have any Alerts. 
         * Step 3: Verify that the Plan Year's Admin Period has expired.
         * Step 4: Submit data to the database. 
         ***********************************************************************************/

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlPlanYearCurrent, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            _adminPlanYearID = int.Parse(DdlPlanYearCurrent.SelectedItem.Value);

            List<Employee> tempEmpList = EmployeeController.manufactureEmployeeList(_employerID);

            foreach (EmployeeType type in EmployeeTypeController.getEmployeeTypes(_employerID))
            {
                try
                {

                    m = measurementController.getPlanYearMeasurement(_employerID, _adminPlanYearID, type.EMPLOYEE_TYPE_ID);
                    if (null == m)
                    {
                        validData = false;
                        LblRolloverMessage.Text = "Missing Measurement Period for Employee Type: " + type.EMPLOYEE_TYPE_NAME;
                        MpeRolloverMessage.Show();

                        break;
                    }
                    if (m.MEASUREMENT_ADMIN_END > DateTime.Now)
                    {
                        validData = false;
                        LblRolloverMessage.Text = "The measurement period for this plan year will not expire until " + m.MEASUREMENT_ADMIN_END.ToShortDateString() + ". You must wait until the Administrative Period has expired.";
                        MpeRolloverMessage.Show();

                        break;
                    }
                    else
                    {

                        List<Employee> filteredForEmployeeType = (
                                                                  from Employee employee in tempEmpList
                                                                  where employee.EMPLOYEE_TYPE_ID == type.EMPLOYEE_TYPE_ID
                                                                      && employee.EMPLOYEE_PLAN_YEAR_ID_LIMBO == _adminPlanYearID
                                                                  select employee
                                                                 ).ToList();

                        if (filteredForEmployeeType.Count > 0)
                        {

                            this.Log.Info(
                                    String.Format(
                                            "For Employee Type {0} for Employer {1} we have {2} employee(s) assigned to that type.",
                                            type.EMPLOYEE_TYPE_ID,
                                            _employerID,
                                            filteredForEmployeeType.Count
                                        )
                                );

                            validData = EmployeeController.RolloverAdministrativePeriod(_employerID, type.EMPLOYEE_TYPE_ID, _adminPlanYearID, _modOn, _modBy);

                        }

                        if (validData == true)
                        {

                            employerController.insertEmployerCalculation(_employerID);

                            LblRolloverMessage.Text = "The Administrative Period has been rolled over.";
                            MpeRolloverMessage.Show();

                        }
                        else
                        {

                            this.Log.Warn(
                                    String.Format(
                                            "For Employee Type {0}, Admin Plan Year {1} for Employer {2} EmployeeController.rolloverAdministrativePeriod returned false.",
                                            type.EMPLOYEE_TYPE_ID,
                                            _adminPlanYearID,
                                            _employerID
                                        )
                                );

                            LblRolloverMessage.Text = "An error occurred while trying to Rollover the Administrative Period, please try again.";
                            MpeRolloverMessage.Show();

                            break;

                        }

                    }

                }
                catch (Exception exception)
                {

                    Log.Warn("Suppressing errors.", exception);

                    LblRolloverMessage.Text = "An error occurred while calculating the Administrative End Date. Please verify that the Employer has setup the Measurement Start and End dates for this Plan Year.";
                    MpeRolloverMessage.Show();

                    break;

                }
            }
        }
        else
        {
            LblRolloverMessage.Text = "Please correct all the fields highlighted in Red.";
            MpeRolloverMessage.Show();
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
}