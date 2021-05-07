using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class ins_alert_generator : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(ins_alert_generator));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;
        loadEmployers();
    }

    /// <summary>
    /// 1-1) Load all existing employers into a dropdown list. 
    /// </summary>
    private void loadEmployers()
    {
        DdlEmployer.DataSource = employerController.getAllEmployers();
        DdlEmployer.DataTextField = "EMPLOYER_NAME";
        DdlEmployer.DataValueField = "EMPLOYER_ID";
        DdlEmployer.DataBind();

        DdlEmployer.Items.Add("Select");
        DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
    }

    private void loadPlanYears()
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            List<PlanYear> tempList = PlanYear_Controller.getEmployerPlanYear(_employerID);

            DdlPlanYear.DataSource = tempList;
            DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYear.DataBind();
            DdlPlanYear.Items.Add("Select");
            DdlPlanYear.SelectedIndex = DdlPlanYear.Items.Count - 1;

            DdlPlanYear2.DataSource = tempList;
            DdlPlanYear2.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYear2.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYear2.DataBind();
            DdlPlanYear2.Items.Add("Select");
            DdlPlanYear2.SelectedIndex = DdlPlanYear2.Items.Count - 1;
        }
        else
        {
            LitMessage.Text = "Please select an Employer.";
            MpeWebMessage.Show();
        }
    }

    /// <summary>
    /// This will refresh the Summer Hour Average for the District that is selected. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int _employerID = 0;
        int _planYearID = 0;
        int _GeneratePlanYearID = 0;
        int _hours = 0;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;
        string _history = null;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlPlanYear, validData);
        validData = errorChecking.validateDropDownSelection(DdlPlanYear2, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            _planYearID = int.Parse(DdlPlanYear2.SelectedItem.Value);
            _GeneratePlanYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
            _history = "Insurance Alert Generated on " + _modBy + " on " + _modOn.ToString() + Environment.NewLine;


            int employeeTypeId = 0;
            if (false == int.TryParse(DdlEmployeeType.SelectedValue, out employeeTypeId))
            {
                MpeWebMessage.Show();
                LitMessage.Text = "Please select an Employee Type.";
                return;
            }

            Measurement mp1 = measurementController.getPlanYearMeasurement(_employerID, _GeneratePlanYearID, employeeTypeId);
            Measurement mp2 = measurementController.getPlanYearMeasurement(_employerID, _planYearID, employeeTypeId);

            int alertsCreated  = employerController.generateMissingInsuranceAlerts(_employerID, _planYearID, _GeneratePlanYearID, mp2.MEASUREMENT_END, _hours, _modBy, _modOn, _history);

            if (alertsCreated > 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = alertsCreated.ToString() + " Insurance Offer Alerts have been created for " + DdlPlanYear.SelectedItem.Text;
            }
            else
            {
                MpeWebMessage.Show();
               LitMessage.Text =  "An error occurred and no Insurance Alerts were created:" +  mp1.MEASUREMENT_PLAN_ID.ToString() + " | " + mp2.MEASUREMENT_PLAN_ID.ToString();
            }
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "Please correct all RED highlighted fields.";
        }
    }

    /// <summary>
    /// Re-load the Plan Years everytime employer changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            loadPlanYears();
            loadEmployeeTypes(int.Parse(DdlEmployer.SelectedValue));
        }
        else
        {
            LitMessage.Text = "Please select an Employer.";
            MpeWebMessage.Show();
        }

    }

    /// <summary>
    /// Loads a specific employer's Employee Types into a dropdown list. 
    /// </summary>
    /// <param name="_employerID"></param>
    private void loadEmployeeTypes(int _employerID)
    {
        DdlEmployeeType.DataSource = EmployeeTypeController.getEmployeeTypes(_employerID);
        DdlEmployeeType.DataTextField = "EMPLOYEE_TYPE_NAME";
        DdlEmployeeType.DataValueField = "EMPLOYEE_TYPE_ID";
        DdlEmployeeType.DataBind();

        DdlEmployeeType.SelectedIndex = DdlEmployeeType.Items.Count - 1;
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
}