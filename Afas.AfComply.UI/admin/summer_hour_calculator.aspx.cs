using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class admin_summer_hour_calculator : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_summer_hour_calculator));

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

    /// <summary>
    /// This will refresh the Summer Hour Average for the District that is selected. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCalculateSummerHours_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int _employerID = 0;
        int _planYearID = 0;
        List<Employee_I> tempIList = new List<Employee_I>();
        List<Payroll_I> tempPList = new List<Payroll_I>();

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlPlanYear, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);

            tempIList = EmployeeController.manufactureImportEmployeeList(_employerID);
            tempPList = Payroll_Controller.manufactureEmployerPayrollImportList(_employerID);
        }

        if (validData == true && tempIList.Count == 0 && tempPList.Count == 0)
        {
            string _extGpID = "SUMMER";
            gpType _gpType = null;
            employer currentEmployer = null;
            List<Employee> tempList = new List<Employee>();
            List<gpType> tempGPlist = new List<gpType>();
            DateTime _modOn = DateTime.Now;
            string _modBy = LitUserName.Text;

            tempGPlist = gpType_Controller.getEmployeeTypes(_employerID);

            _gpType = gpType_Controller.validateGpType(_employerID, _extGpID, "AVG SUMMER HOURS", tempGPlist);

            tempList = EmployeeController.manufactureEmployeeList(_employerID);

            currentEmployer = employerController.getEmployer(_employerID);

            int batchID = EmployeeController.manufactureBatchID(_employerID, _modOn, _modBy);

            foreach (Employee emp in tempList)
            {
                calculator_summerAverage.calculateEmployeeSummerAverages(batchID, _planYearID, emp, currentEmployer, _gpType, _modBy, _modOn);
            }
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "An error occurred. <br /> Step 1: Correct all fields highlighted in RED.<br />";
            LitMessage.Text += "Step 2: You have " + tempIList.Count.ToString() + " Employee Alerts that need to be corrected.<br />";
            LitMessage.Text += "Step 3: You have " + tempPList.Count.ToString() + " Payroll Alerts that need to be corrected.";
        }
    }

    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            DdlPlanYear.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
            DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYear.DataBind();

            DdlPlanYear.Items.Add("Select");
            DdlPlanYear.SelectedIndex = DdlPlanYear.Items.Count - 1;
        }
        else
        { 
        
        }

    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
}