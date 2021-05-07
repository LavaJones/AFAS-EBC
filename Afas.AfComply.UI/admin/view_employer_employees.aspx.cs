using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Text;

public partial class admin_view_employer_employees : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_view_employer_employees));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;
        loadEmployers();
    }
    
    /*********************************************************************************************
     GROUP 1: All functions that load data into dropdown lists & gridviews. ****************** 
    *********************************************************************************************/
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

    private void loadEmployees(int _employerID)
    {
        int tempID = 0;

        if (Session["tempID"] == null)
        {
            Session["tempID"] = int.Parse(DdlEmployer.SelectedItem.Value);
            Session["tempEmployee"] = EmployeeController.manufactureImportEmployeeList(_employerID);
        }
        else
        {
            tempID = (int)Session["tempID"];
            if (_employerID != tempID)
            {
                Session["tempID"] = _employerID;
                Session["tempEmployee"] = EmployeeController.manufactureImportEmployeeList(_employerID);
            }
        }

        StringBuilder sb = new StringBuilder();  
        if(Session["tempEmployee"] is List<Employee_I>)   
        {           
            foreach (Employee_I emp in (List<Employee_I>)Session["tempEmployee"]) 
            {                     
                sb.Append("Employee Id:(");
                sb.Append(emp.EMPLOYEE_ID);
                sb.Append("), ");
            }
        }
        else
        {   
            if (null != Session["tempEmployee"])
            {                
                Log.Warn("Session['tempEmployee'] was not of type List<Employee_I>, which was expected. Actual type:" + Session["tempEmployee"].GetType().ToString());
            }
        }

        GvPayrollData.DataSource = Session["tempEmployee"];
        GvPayrollData.DataBind();

        PIILogger.LogPII(String.Format("Showing PII to user ID:[{0}] at IP: [{1}] for employees: [{2}]", ((User)Session["CurrentUser"]).User_ID, Request.UserHostAddress, sb.ToString()));
    }

    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        employer _employer = null;

        GvPayrollData.PageIndex = 0;

        if (DdlEmployer.SelectedItem.Text != "Select")
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            _employer = employerController.getEmployer(_employerID);

            loadEmployees(_employerID);
        }
    }


    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }


    protected void GvPayrollData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (DdlEmployer.SelectedItem.Text != "Select")
        {
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            GvPayrollData.PageIndex = e.NewPageIndex;
            loadEmployees(_employerID);
        }
    }


    protected void GvPayrollData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortBy = e.SortExpression;
        string lastSortExpression = null;
        string lastSortDirection = "ASC";
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

        List<Employee_I> tempList = (List<Employee_I>)Session["tempEmployee"];

        if (Session["sortExp"] != null)
        {
            lastSortExpression = Session["sortExp"].ToString();
        }
        if (Session["sortDir"] != null)
        {
            lastSortDirection = Session["sortDir"].ToString();
        }

        try
        {
            if (e.SortExpression != lastSortExpression)
            {
                lastSortExpression = e.SortExpression;
                lastSortDirection = "ASC";
            }
            else
            {
                if (lastSortDirection == "ASC")
                {
                    lastSortExpression = e.SortExpression;
                    lastSortDirection = "DESC";
                }
                else
                {
                    lastSortExpression = e.SortExpression;
                    lastSortDirection = "ASC";
                }
            }

            switch (lastSortDirection)
            {
                case "ASC":
                    switch (e.SortExpression)
                    {
                        case "EMPLOYEE_EXT_ID":
                            tempList.Sort(delegate(Employee_I x, Employee_I y)
                            {
                                return x.EMPLOYEE_EXT_ID.CompareTo(y.EMPLOYEE_EXT_ID);
                            });
                            break;
                        case "EMPLOYEE_FIRST_NAME":
                            tempList.Sort(delegate(Employee_I x, Employee_I y)
                            {
                                return x.EMPLOYEE_FIRST_NAME.CompareTo(y.EMPLOYEE_FIRST_NAME);
                            });
                            break;
                        case "EMPLOYEE_LAST_NAME":
                            tempList.Sort(delegate(Employee_I x, Employee_I y)
                            {
                                return x.EMPLOYEE_LAST_NAME.CompareTo(y.EMPLOYEE_LAST_NAME);
                            });
                            break;
                        case "EMPLOYEE_I_HIRE_DATE":
                            tempList.Sort(delegate(Employee_I x, Employee_I y)
                            {
                                return x.EMPLOYEE_I_HIRE_DATE.CompareTo(y.EMPLOYEE_I_HIRE_DATE);
                            });
                            break;
                        case "EMPLOYEE_I_TERM_DATE":
                            tempList.Sort(delegate(Employee_I x, Employee_I y)
                            {
                                return x.EMPLOYEE_I_TERM_DATE.CompareTo(y.EMPLOYEE_I_TERM_DATE);
                            });
                            break;
                        case "EMPLOYEE_I_DOB":
                            tempList.Sort(delegate(Employee_I x, Employee_I y)
                            {
                                return x.EMPLOYEE_I_DOB.CompareTo(y.EMPLOYEE_I_DOB);
                            });
                            break;
                        case "Employee_SSN_Hidden":
                            tempList.Sort(delegate(Employee_I x, Employee_I y)
                            {
                                return x.Employee_SSN_Hidden.CompareTo(y.Employee_SSN_Hidden);
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case "DESC":
                    tempList.Reverse();
                    break;
                default:
                    break;
            }

            Session["tempPayroll"] = tempList;
            Session["sortDir"] = lastSortDirection;
            Session["sortExp"] = lastSortExpression;
            loadEmployees(_employerID);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

    }


    protected void BtnDeleteDemAlerts_Click(object sender, EventArgs e)
    {
        int _employerID = 0;
        bool transactionComplete = false;
        bool validData = true;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            transactionComplete = alert_controller.deleteEmployerDemographicAlerts(_employerID, _modBy, _modOn);

            if (transactionComplete == true)
            {
                Session["tempID"] = null;
                loadEmployees(_employerID);
                LitMessage.Text = "All demographic alerts have been deleted for this employer.";
                MpeWebMessage.Show();
            }
            else
            {
                LitMessage.Text = "An error occurred while DELETING the demographic alerts. Please try again.";
                MpeWebMessage.Show();
            }
        }
        else
        {
            LitMessage.Text = "Please select an employer.";
            MpeWebMessage.Show();
        }
    }
}