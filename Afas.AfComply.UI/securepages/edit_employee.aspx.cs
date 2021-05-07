using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class edit_employee : Afas.AfComply.UI.securepages.SecurePageBase
{
    
    protected override void PageLoadLoggedIn(User user, employer employer)
    {
       HfUserName.Value = user.User_Full_Name;

        HfDistrictID.Value = user.User_District_ID.ToString();
        loadEmployees();
        loadStates();
        loadEmployeeTypes();
        loadHRStatuses();
        loadEmployeeClassifications();
        loadACAstatuses();
    }

    private ILog Log = LogManager.GetLogger(typeof(edit_employee));

    private int EmployerId { get { return int.Parse(HfDistrictID.Value); } }

    private const string errorMsg = "Failed to update Employee";

    private const string successMsg = "Succesfully Saved!";

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void DdlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SelectedEditEmployee"] = null;

        if(DdlEmployee.SelectedIndex > 0){

            setFields();
        }
        else
        {
            clearFields();
        }
        lblMsg.Text = String.Empty;
    }

    protected void BtnFindEmployees_Click(object sender, EventArgs e)
    {
        if (TxtEmployee.Text == String.Empty)
            return;
       
        Session["FilteredEditEmployees"] = null;
        var filteredEmployees = (List<Employee>)EmployeeController.SearchEmployee(EmployerId, TxtEmployee.Text.ToLower(), TxtEmployee.Text.ToLower(), TxtEmployee.Text.ToLower());
     

        Session["FilteredEditEmployees"] = filteredEmployees;
        Session["DdlEmployeeDataSource"] = filteredEmployees;
        formatDdl(DdlEmployee, filteredEmployees, "EMPLOYEE_FULL_NAME", "EMPLOYEE_ID");
        lblMsg.Text = String.Empty;
        if (filteredEmployees.Count > 0)
        {
            DdlEmployee.SelectedIndex = 1;
            setFields();
        }
        else
        {
            clearFields();
        }
    }

    protected void BtnResetEmployees_Click(object sender, EventArgs e)
    {
        clearFields();
        loadEmployees();
        TxtEmployee.Text = String.Empty;
        lblMsg.Text = String.Empty;
    }

    protected void DdlEmployeeState_SelectedIndexChanged(object sender, EventArgs e)
    {
        toggleBackgroundColorDdl(DdlEmployeeState);
    }

    protected void DdlEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        toggleBackgroundColorDdl(DdlEmployeeType);
    }


    protected void DdlHireStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        toggleBackgroundColorDdl(DdlHireStatus);
    }

    protected void DdlClassification_SelectedIndexChanged(object sender, EventArgs e)
    {
        toggleBackgroundColorDdl(DdlClassification);
    }

    protected void DdlAcaStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        toggleBackgroundColorDdl(DdlAcaStatus);
        if (DdlAcaStatus.SelectedItem.Text == "Termed")
        {
            mpAcaStatus.Show();
        }

    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Log.Info("Save button has been clicked");
            var result = updateEmployee();
            showLblMsg(result);
            loadEmployees();
            clearFields();
            
        }
        catch (Exception ex)
        {
            Log.Error(errorMsg, ex);
            showLblMsg(false);
        }
       
    }

    private Boolean updateEmployee()
    {
        this.Log.Info("entered updateEmployee");

        var selectedEmployee = (Employee)Session["SelectedEditEmployee"];

        this.Log.Info(string.Format("The Selected Employee ID is {0}", selectedEmployee.EMPLOYEE_ID));

        int employeeTypeID = 0;
        int hrStatusID = 0;

        if (false == int.TryParse(DdlEmployeeType.SelectedValue, out employeeTypeID) || false == int.TryParse(DdlHireStatus.SelectedValue, out hrStatusID))
        {

            return false;
        }

        string fName = TxtFirstName.Text;
        string mName = TxtMiddleName.Text;
        string lName = TxtLastName.Text;
        string address = TxtEmployerAddress.Text;
        string city = TxtEmployerCity.Text;
        int stateID = int.Parse(DdlEmployeeState.SelectedValue);
        string zip = TxtEmployerZip.Text;
        DateTime? hireDate = TryParseNullableDateTime(TxtHireDate.Text);
        DateTime? currDate = TryParseNullableDateTime(TxtCurrDate.Text);
        string ssn = TxtSSN.Text;
        string extID = TxtExtID.Text;
        DateTime? termDate = TryParseNullableDateTime(TxtTermDate.Text);
        DateTime? dob = TryParseNullableDateTime(TxtDOB.Text);
        DateTime? impEnd = TryParseNullableDateTime(TxtImpEnd.Text);
        int classID = int.Parse(DdlClassification.SelectedValue);
        int acaStatusID = int.Parse(DdlAcaStatus.SelectedValue);

        if (DdlEmployee.SelectedIndex > 0)
        {
            var employee = new Employee(
                selectedEmployee.EMPLOYEE_ID,
                employeeTypeID,
                hrStatusID,
                EmployerId,
                fName,
                mName,
                lName,
                address,
                city,
                stateID,
                zip,
                hireDate,
                currDate,
                ssn,
                extID,
                termDate,
                dob,
                impEnd,
                selectedEmployee.EMPLOYEE_PLAN_YEAR_ID,
                selectedEmployee.EMPLOYEE_PLAN_YEAR_ID_LIMBO,
                selectedEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS,
                selectedEmployee.EMPLOYEE_PY_AVG_MEAS_HOURS,
                selectedEmployee.EMPLOYEE_PY_AVG_ADMIN_HOURS,
                selectedEmployee.EMPLOYEE_PY_AVG_MEAS_HOURS,
                selectedEmployee.EMPLOYEE_PY_AVG_INIT_HOURS,
                classID,
                acaStatusID);
            return EmployeeController.UpdateEmployee(employee, HfUserName.Value);
        }

        return false;
    }

    private void setFields()
    {

        int index = DdlEmployee.SelectedIndex - 1;
        List<Employee> employees = (List<Employee>)Session["DdlEmployeeDataSource"];

        if (employees == null || employees.Count <= 0)
        {
            return;
        }

        Employee employee = employees.ElementAt(index);
        Session["SelectedEditEmployee"] = employee;

        TxtFirstName.Text = employee.EMPLOYEE_FIRST_NAME;
        TxtMiddleName.Text = employee.EMPLOYEE_MIDDLE_NAME;
        TxtLastName.Text = employee.EMPLOYEE_LAST_NAME;
        TxtEmployerAddress.Text = employee.EMPLOYEE_ADDRESS;
        TxtEmployerCity.Text = employee.EMPLOYEE_CITY;
        TxtEmployerZip.Text = employee.EMPLOYEE_ZIP;
        TxtSSN.Text = employee.Employee_SSN_Visible;
        TxtExtID.Text = employee.EMPLOYEE_EXT_ID;

        if (employee.EMPLOYEE_DOB.HasValue)
            TxtDOB.Text = employee.EMPLOYEE_DOB.Value.ToString("MM/dd/yyyy");
        else
            TxtDOB.Text = String.Empty;

        if (employee.EMPLOYEE_HIRE_DATE.HasValue)
            TxtHireDate.Text = employee.EMPLOYEE_HIRE_DATE.Value.ToString("MM/dd/yyyy");
        else
            TxtHireDate.Text = String.Empty;

        if (employee.EMPLOYEE_C_DATE.HasValue)
            TxtCurrDate.Text = employee.EMPLOYEE_C_DATE.Value.ToString("MM/dd/yyyy");
        else
            TxtCurrDate.Text = String.Empty;

        if (employee.EMPLOYEE_TERM_DATE.HasValue)
            TxtTermDate.Text = employee.EMPLOYEE_TERM_DATE.Value.ToString("MM/dd/yyyy");
        else
            TxtTermDate.Text = String.Empty;

        if (employee.EMPLOYEE_IMP_END.HasValue)
            TxtImpEnd.Text = employee.EMPLOYEE_IMP_END.Value.ToString("MM/dd/yyyy");
        else
            TxtImpEnd.Text = String.Empty;


        if (DdlEmployeeState.Items.FindByValue(employee.EMPLOYEE_STATE_ID.ToString()) == null)
            DdlEmployeeState.SelectedIndex = 0;
        else
            DdlEmployeeState.SelectedValue = employee.EMPLOYEE_STATE_ID.ToString();

        if (DdlEmployeeType.Items.FindByValue(employee.EMPLOYEE_TYPE_ID.ToString()) == null)
            DdlEmployeeType.SelectedIndex = 0;
        else
            DdlEmployeeType.SelectedValue = employee.EMPLOYEE_TYPE_ID.ToString();

     
        if (DdlHireStatus.Items.FindByValue(employee.EMPLOYEE_HR_STATUS_ID.ToString()) == null)
            DdlHireStatus.SelectedIndex = 0;
        else
            DdlHireStatus.SelectedValue = employee.EMPLOYEE_HR_STATUS_ID.ToString();

        if (DdlClassification.Items.FindByValue(employee.EMPLOYEE_CLASS_ID.ToString()) == null)
            DdlClassification.SelectedIndex = 0;
        else
            DdlClassification.SelectedValue = employee.EMPLOYEE_CLASS_ID.ToString();

        if (DdlAcaStatus.Items.FindByValue(employee.EMPLOYEE_ACT_STATUS_ID.ToString()) == null)
            DdlAcaStatus.SelectedIndex = 0;
        else
            DdlAcaStatus.SelectedValue = employee.EMPLOYEE_ACT_STATUS_ID.ToString();

        toggleBackgroundColorDdl(DdlEmployeeState);
        toggleBackgroundColorDdl(DdlEmployeeType);
        toggleBackgroundColorDdl(DdlHireStatus);
        toggleBackgroundColorDdl(DdlClassification);
        toggleBackgroundColorDdl(DdlAcaStatus);
    }

    private void clearFields()
    {
        TxtFirstName.Text = String.Empty;
        TxtMiddleName.Text = String.Empty;
        TxtLastName.Text = String.Empty;
        TxtEmployerAddress.Text = String.Empty;
        TxtEmployerCity.Text = String.Empty;
        TxtEmployerZip.Text = String.Empty;
        TxtSSN.Text = String.Empty;
        TxtExtID.Text = String.Empty;

        TxtDOB.Text = String.Empty;
        TxtHireDate.Text = String.Empty;
        TxtCurrDate.Text = String.Empty;
        TxtTermDate.Text = String.Empty;
        TxtImpEnd.Text = String.Empty;

        DdlEmployeeState.SelectedIndex = 0;
        DdlEmployeeType.SelectedIndex = 0;
        DdlHireStatus.SelectedIndex = 0;
        DdlClassification.SelectedIndex = 0;
        DdlAcaStatus.SelectedIndex = 0;
    }

    private void loadEmployees()
    {
        Session["EditEmployees"] = null;
        Session["FilteredEditEmployees"] = null;
        var employees = EmployeeController.manufactureEmployeeList(EmployerId).OrderBy(e => e.EMPLOYEE_FULL_NAME).ToList();
        Session["EditEmployees"] = employees;
        Session["Employees"] = employees;
        Session["DdlEmployeeDataSource"] = employees;
        formatDdl(DdlEmployee, employees, "EMPLOYEE_FULL_NAME", "EMPLOYEE_ID");
    }

    private void loadStates()
    {
        var states = StateController.getStates();
        formatDdl(DdlEmployeeState, states, "State_Name", "State_ID");
    }

    private void loadEmployeeTypes()
    {
        var employeeTypes = EmployeeTypeController.getEmployeeTypes(EmployerId);
        formatDdl(DdlEmployeeType, employeeTypes, "EMPLOYEE_TYPE_NAME", "EMPLOYEE_TYPE_ID");
    }

  
    private void loadHRStatuses()
    {
        var hrStatuses = hrStatus_Controller.manufactureHRStatusList(EmployerId);
        formatDdl(DdlHireStatus, hrStatuses, "HR_STATUS_NAME", "HR_STATUS_ID");
    }

    private void loadEmployeeClassifications()
    {
        var classifications = classificationController.ManufactureEmployerClassificationList(EmployerId, false);
        formatDdl(DdlClassification, classifications, "CLASS_DESC", "CLASS_ID");
    }

    private void loadACAstatuses()
    {
        var acaStatuses = classificationController.getACAstatusList();
        formatDdl(DdlAcaStatus, acaStatuses, "ACA_STATUS_NAME", "ACA_STATUS_ID");
    }

    private void formatDdl(DropDownList Ddl, Object DataSource, String DataTextField, String DataValueField)
    {
        Ddl.DataSource = DataSource;
        Ddl.DataTextField = DataTextField;
        Ddl.DataValueField = DataValueField;
        Ddl.DataBind();
        Ddl.Items.Insert(0, "Select");
        Ddl.SelectedIndex = 0;
    }

    private DateTime? TryParseNullableDateTime(string text)
    {
        DateTime date;
        if (DateTime.TryParse(text, out date))
        {
            return date;
        }
        else
        {
            return null;
        }
    }

    private void showLblMsg(bool result)
    {
        lblMsg.ForeColor = System.Drawing.Color.Black;

        if (result)
        {
            lblMsg.Text = successMsg;
            lblMsg.BackColor = System.Drawing.Color.Green;
        }
        else
        {
            lblMsg.Text = errorMsg;
            lblMsg.BackColor = System.Drawing.Color.Red;
        }
    }

    private void toggleBackgroundColorDdl(DropDownList Ddl)
    {
        if (Ddl.SelectedIndex == 0)
        {
            Ddl.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            Ddl.BackColor = System.Drawing.Color.White;
        }
    }


}