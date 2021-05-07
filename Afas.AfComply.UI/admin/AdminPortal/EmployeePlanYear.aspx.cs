using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployeePlanYear : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployeePlanYear));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            loadEmployers();
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
            List<PlanYear> planYear = PlanYear_Controller.getEmployerPlanYear(_employerID);

            DdlPlanYearCurrent.DataSource = planYear;
            DdlPlanYearCurrent.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYearCurrent.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYearCurrent.DataBind();

            DdlPlanYearCurrent.Items.Add("Select");
            DdlPlanYearCurrent.SelectedIndex = DdlPlanYearCurrent.Items.Count - 1;

            DdlPlanYearNew.DataSource = planYear;
            DdlPlanYearNew.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYearNew.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYearNew.DataBind();

            DdlPlanYearNew.Items.Add("Select");
            DdlPlanYearNew.SelectedIndex = DdlPlanYearNew.Items.Count - 1;

        }

        protected void BtnProcessFile_Click(object sender, EventArgs e)
        {
            bool validTransaction = false;
            try
            {
                //Step 1: Identify needed variables.
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                int _currPlanYearID = int.Parse(DdlPlanYearCurrent.SelectedItem.Value);
                int _newPlanYearID = int.Parse(DdlPlanYearNew.SelectedItem.Value);
                string _modBy = ((User)Session["CurrentUser"]).User_UserName;

                List<Employee> tempEmpList = EmployeeController.manufactureEmployeeList(_employerID);

                foreach (var type in EmployeeTypeController.getEmployeeTypes(_employerID))
                {
                    
                    int count = (from emp in tempEmpList
                                 where emp.EMPLOYEE_TYPE_ID == type.EMPLOYEE_TYPE_ID
                                     && emp.EMPLOYEE_PLAN_YEAR_ID_MEAS == _currPlanYearID
                                 select emp).Count();

                    if (count > 0)
                    {

                        validTransaction = EmployeeController.RollBackEmployeePlanYearPeriod_Measurement(
                            _employerID,
                            type.EMPLOYEE_TYPE_ID,
                            _currPlanYearID,
                            _newPlanYearID,
                            DateTime.Now,
                            _modBy);

                        if (validTransaction == true)
                        {
                            lblMsg.Text = "The Measurement Period has been RolledBack.";
                        }
                        else
                        {
                            lblMsg.Text = "ERROR, the system could not rollover the Measurement Period due to an ERROR.";

                            EditTable();
                            return; // if there is an error we want to stop and tell the user
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
            }

            EditTable();
        }

        protected void BtnFix_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            int _planYearID = 0;
            int _employeeID = 0;
            DateTime _modOn = DateTime.Now;
            string _modBy = ((User)Session["CurrentUser"]).User_UserName;
            try
            {
                foreach (GridViewRow row in GvPlanYearErrors.Rows)
                {
                    //Load the Plan Year selected. 
                    DropDownList ddl_gv_PlanYear = (DropDownList)row.FindControl("Ddl_gv_PlanYears");

                    if (int.TryParse(ddl_gv_PlanYear.SelectedValue, out _planYearID) && _planYearID != 0)
                    {
                        //load the employee id
                        HiddenField gv_employee_Id = (HiddenField)row.FindControl("Hf_gv_id");
                        _employeeID = int.Parse(gv_employee_Id.Value);

                        //save the new Plan year to the employee
                        if (false == EmployeeController.updateEmployeePlanYearMeasId(_employeeID, _planYearID, _modOn, _modBy))
                        {
                            lblMsg.Text += "Update Failed: Id [" + _employeeID + "] ";
                        }
                    }
                }

                // Queue up the calculation
                employerController.insertEmployerCalculation(int.Parse(DdlEmployer.SelectedItem.Value));
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);

                lblMsg.Text += "Save Failed with error : " + exception.Message;
            }

            //refresh the grid
            EditTable();
        }

        protected void PlanYears_All_SelectedIndexChanged(object sender, EventArgs e)
        {
            int planYearId = 0;

            //check that data is correct
            if (
                    null == Ddl_gv_PlanYears_All.SelectedItem
                        ||
                    null == Ddl_gv_PlanYears_All.SelectedItem.Value
                        ||
                    false == int.TryParse(Ddl_gv_PlanYears_All.SelectedItem.Value, out planYearId)
                )
            {

                lblMsg.Text = "Incorrect parameters";

                return;
            }

            EditTable(planYearId);
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditTable();
        }

        private void EditTable(int? AllPlanYearID = null)
        {
            int employerId = 0;

            //check that data is correct
            if (
                    null == DdlEmployer.SelectedItem
                        ||
                    null == DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                lblMsg.Text = "Incorrect parameters";

                return;

            }

            bool showAllEmployees = ShowAllEmployees.Checked;

            employer employ = employerController.getEmployer(employerId);

            // Load Plan Years
            loadPlanYears(employerId);

            List<Employee> employees = EmployeeController.manufactureEmployeeList(employerId);

            // Only show employees that don't have a plan year
            if (false == showAllEmployees)
            {
                employees = (from Employee emp in employees where emp.EMPLOYEE_PLAN_YEAR_ID_MEAS <= 0 select emp).ToList();
            }

            GvPlanYearErrors.DataSource = employees;
            GvPlanYearErrors.DataBind();

            cofein.Text = employ.EMPLOYER_EIN;

            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            int _currMeasPlanYearID = 0;
            var planyear = PlanYear_Controller.getEmployerPlanYear(_employerID);

            //Load the Employer Plan Years. 
            Ddl_gv_PlanYears_All.DataSource = planyear;
            Ddl_gv_PlanYears_All.DataTextField = "PLAN_YEAR_DESCRIPTION";
            Ddl_gv_PlanYears_All.DataValueField = "PLAN_YEAR_ID";
            Ddl_gv_PlanYears_All.DataBind();
            Ddl_gv_PlanYears_All.Items.Add("Select");
            Ddl_gv_PlanYears_All.SelectedIndex = Ddl_gv_PlanYears_All.Items.Count - 1;

            try
            {
                foreach (GridViewRow row in GvPlanYearErrors.Rows)
                {
                    //Load the Plan Years available. 
                    DropDownList ddl_gv_PlanYear = (DropDownList)row.FindControl("Ddl_gv_PlanYears");
                    HiddenField hf_gv_measPYid = (HiddenField)row.FindControl("Hf_gv_measID");
                    _currMeasPlanYearID = int.Parse(hf_gv_measPYid.Value);

                    //Load the Employer Plan Years. 
                    ddl_gv_PlanYear.DataSource = planyear;
                    ddl_gv_PlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
                    ddl_gv_PlanYear.DataValueField = "PLAN_YEAR_ID";
                    ddl_gv_PlanYear.DataBind();
                    ddl_gv_PlanYear.Items.Add("Select");

                    //Set the dropdown value to it's current value.
                    errorChecking.setDropDownList(ddl_gv_PlanYear, AllPlanYearID ?? _currMeasPlanYearID);
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
            }
        }
    }
}