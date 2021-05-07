using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

public partial class securepages_view_employee_import : Afas.AfComply.UI.securepages.SecurePageBase
{

    private ILog Log = LogManager.GetLogger(typeof(securepages_view_employee_import));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {

        HfDistrictID.Value = user.User_District_ID.ToString();
        
        loadEmployeeData();
        loadACAstatus();
        loadEmployeeClassifications();
        loadHRStatusDescriptions();
        loadPlanYears();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void BtnApplyFilters_Click(object sender, EventArgs e)
    {
        loadEmployeeData();
    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        string recordsRemoved = "Employee Records Removed:<br />";
        string recordsNotRemoved = "Records that could not be DELETED:<br />";
        int i = 0;

        foreach (GridViewRow row in GvEmployeeData.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
            HiddenField hf = (HiddenField)row.FindControl("Hf_gv_RowID");
            bool validTransaction = false;

            if (cb.Checked == true)
            {
                i += 1;
                int _rowID = int.Parse(hf.Value);
                validTransaction = EmployeeController.deleteImportedEmployee(_rowID);

                if (validTransaction == true)
                {
                    CbCheckAll.Checked = false;
                    List<Employee_I> tempList = (List<Employee_I>)Session["importEmployeeAlertDetails"];
                    Employee_I empP = null;

                    foreach (Employee_I pi in tempList)
                    {
                        if (pi.ROW_ID == _rowID)
                        {
                            empP = pi;
                            break;
                        }
                    }

                    tempList.Remove(empP);
                    Session["importEmployeeAlertDetails"] = tempList;

                    recordsRemoved += _rowID.ToString() + "<br />";
                }
                else
                {
                    recordsNotRemoved += _rowID.ToString() + "<br />";
                }
            }
        }





        if (i == 0)
        {
            MpeWebMessage.Show();
            LitMessage.Text = "No records have been selected! Please select which records you would like to delete.";
        }
        else
        {
            loadEmployeeData();

            MpeWebMessage.Show();
            LitMessage.Text = recordsRemoved + "<br />" + recordsNotRemoved;
        }



    
}



    private void loadPlanYears()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        Ddl_u_PlanYear.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
        Ddl_u_PlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        Ddl_u_PlanYear.DataValueField = "PLAN_YEAR_ID";
        Ddl_u_PlanYear.DataBind();

        Ddl_u_PlanYear.Items.Add("Select");
        Ddl_u_PlanYear.SelectedIndex = Ddl_u_PlanYear.Items.Count - 1;
    }

    private void loadACAstatus()
    {
        Ddl_u_AcaStatus.DataSource = classificationController.getACAstatusList();
        Ddl_u_AcaStatus.DataTextField = "ACA_STATUS_NAME";
        Ddl_u_AcaStatus.DataValueField = "ACA_STATUS_ID";
        Ddl_u_AcaStatus.DataBind();

        Ddl_u_AcaStatus.Items.Add("Select");
        Ddl_u_AcaStatus.SelectedIndex = Ddl_u_AcaStatus.Items.Count - 1;
    }

    private void loadEmployeeClassifications()
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        Ddl_u_EmployeeClass.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        Ddl_u_EmployeeClass.DataTextField = "CLASS_DESC";
        Ddl_u_EmployeeClass.DataValueField = "CLASS_ID";
        Ddl_u_EmployeeClass.DataBind();
        
        Ddl_u_EmployeeClass.Items.Add("Select");
        Ddl_u_EmployeeClass.SelectedIndex = Ddl_u_EmployeeClass.Items.Count - 1;
    
    }

    private void loadHRStatusDescriptions()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<hrStatus> tempList = hrStatus_Controller.manufactureHRStatusList(_employerID);
        Ddl_f_HrDesc.DataSource = tempList;
        Ddl_f_HrDesc.DataTextField = "HR_STATUS_NAME";
        Ddl_f_HrDesc.DataValueField = "HR_STATUS_ID";
        Ddl_f_HrDesc.DataBind();

        Ddl_f_HrDesc.Items.Add("Select");
        Ddl_f_HrDesc.SelectedIndex = Ddl_f_HrDesc.Items.Count - 1;

        Ddl_u_HrStatus.DataSource = tempList;
        Ddl_u_HrStatus.DataTextField = "HR_STATUS_NAME";
        Ddl_u_HrStatus.DataValueField = "HR_STATUS_ID";
        Ddl_u_HrStatus.DataBind();

        Ddl_u_HrStatus.Items.Add("Select");
        Ddl_u_HrStatus.SelectedIndex = Ddl_u_HrStatus.Items.Count - 1;
    }

    private void loadEmployeeData()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<Employee_I> fullList = new List<Employee_I>();
        List<Employee_I> filteredList1 = new List<Employee_I>();
        List<Employee_I> filteredList2 = new List<Employee_I>();
        List<Employee_I> filteredList3 = new List<Employee_I>();

        if (Session["importEmployeeAlertDetails"] == null)
        {
            Session["importEmployeeAlertDetails"] = EmployeeController.manufactureImportEmployeeList(_employerID);
        }

        fullList = (List<Employee_I>)Session["importEmployeeAlertDetails"];

        if (Cb_f_PayrollID.Checked == true)
        {
            try
            {
                string payrollID = Txt_f_PayrollID.Text;

                foreach (Employee_I emp in fullList)
                {
                    try
                    {
                        if (emp.EMPLOYEE_EXT_ID.ToLower().Contains(payrollID))
                        {
                            filteredList1.Add(emp);
                        }
                    }
                    catch (Exception exception)
                    {
                        this.Log.Warn("Suppressing errors.", exception);
                    }
                }
            }
            catch (Exception exception)
            {
                
                this.Log.Warn("Suppressing errors.", exception);
                
                filteredList1 = fullList;
            
            }
        }
        else
        {
            filteredList1 = fullList;
        }

        if (Cb_f_LastName.Checked == true)
        {
            string lname = Txt_f_LastName.Text.ToLower();

            foreach (Employee_I emp in filteredList1)
            {
                if (emp.EMPLOYEE_LAST_NAME.ToLower().Contains(lname))
                {
                    filteredList2.Add(emp);
                }
            }
        }
        else
        {
            filteredList2 = filteredList1;
        }

        if ( Cb_f_HrDesc.Checked == true)
        {
            try
            {
                string hrdesc = Ddl_f_HrDesc.SelectedItem.Text.ToLower();

                foreach (Employee_I emp in filteredList2)
                {
                    try
                    {
                        if (emp.EMPLOYEE_HR_EXT_DESCRIPTION.ToLower().Contains(hrdesc))
                        {
                            filteredList3.Add(emp);
                        }
                    }
                    catch (Exception exception)
                    {
                        this.Log.Warn("Suppressing errors.", exception);
                    }
                }
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                filteredList3 = filteredList2;
            
            }
        
        }
        else
        {
            filteredList3 = filteredList2;
        }


        GvEmployeeData.DataSource = filteredList3;
        GvEmployeeData.DataBind();

        litAlertCount.Text = fullList.Count.ToString();
        litAlertsShown.Text = GvEmployeeData.Rows.Count.ToString();
    }
    protected void CbCheckAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GvEmployeeData.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");

            if (CbCheckAll.Checked == true)
            {
                cb.Checked = true;
            }
            else
            {
                cb.Checked = false;
            }
        }
    }

    protected void Cb_f_PayrollID_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_PayrollID.Checked == true)
        {
            Txt_f_PayrollID.Text = null;
            Txt_f_PayrollID.Enabled = true;
        }
        else
        {
            Txt_f_PayrollID.Text = "n/a";
            Txt_f_PayrollID.Enabled = false;
        }
    }

    protected void Cb_f_LastName_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_LastName.Checked == true)
        {
            Txt_f_LastName.Text = null;
            Txt_f_LastName.Enabled = true;
        }
        else
        {
            Txt_f_LastName.Text = "n/a";
            Txt_f_LastName.Enabled = false;
        }
    }

    protected void Cb_f_HrDesc_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_HrDesc.Checked == true)
        {
            Ddl_f_HrDesc.SelectedIndex = Ddl_f_HrDesc.Items.Count - 1;
            Ddl_f_HrDesc.Enabled = true;
        }
        else
        {
            Ddl_f_HrDesc.SelectedIndex = Ddl_f_HrDesc.Items.Count - 1;
            Ddl_f_HrDesc.Enabled = false;
        }
    }

    protected void Cb_u_HRstatus_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_HRstatus.Checked == true)
        {
            Ddl_u_HrStatus.SelectedIndex = Ddl_u_HrStatus.Items.Count - 1;
            Ddl_u_HrStatus.Enabled = true;
        }
        else
        {
            Ddl_u_HrStatus.SelectedIndex = Ddl_u_HrStatus.Items.Count - 1;
            Ddl_u_HrStatus.Enabled = false;
        }
    }

    protected void Cb_u_EmployeeClass_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_EmployeeClass.Checked == true)
        {
            Ddl_u_EmployeeClass.SelectedIndex = Ddl_u_EmployeeClass.Items.Count - 1;
            Ddl_u_EmployeeClass.Enabled = true;
        }
        else
        {
            Ddl_u_EmployeeClass.SelectedIndex = Ddl_u_EmployeeClass.Items.Count - 1;
            Ddl_u_EmployeeClass.Enabled = false;
        }
    }

    protected void Cb_u_ACAstatus_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_ACAstatus.Checked == true)
        {
            Ddl_u_AcaStatus.SelectedIndex = Ddl_u_AcaStatus.Items.Count - 1;
            Ddl_u_AcaStatus.Enabled = true;
        }
        else
        {
            Ddl_u_AcaStatus.SelectedIndex = Ddl_u_AcaStatus.Items.Count - 1;
            Ddl_u_AcaStatus.Enabled = false;
        }
    }

    protected void Cb_u_PlanYear_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_PlanYear.Checked == true)
        {
            Ddl_u_PlanYear.SelectedIndex = Ddl_u_PlanYear.Items.Count - 1;
            Ddl_u_PlanYear.Enabled = true;
        }
        else
        {
            Ddl_u_PlanYear.SelectedIndex = Ddl_u_PlanYear.Items.Count - 1;
            Ddl_u_PlanYear.Enabled = false;
        }
    }

    protected void Cb_u_HireDate_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_HireDate.Checked == true)
        {
            Txt_u_HireDate.Text = null;
            Txt_u_HireDate.Enabled = true;
        }
        else
        {
            Txt_u_HireDate.Text = "n/a";
            Txt_u_HireDate.Enabled = false;
        }
    }

    protected void Cb_u_TermDate_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_TermDate.Checked == true)
        {
            Txt_u_TermDate.Text = null;
            Txt_u_TermDate.Enabled = true;
        }
        else
        {
            Txt_u_TermDate.Text = "n/a";
            Txt_u_TermDate.Enabled = false;
        }
    }

    protected void GvEmployeeData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvEmployeeData.PageIndex = e.NewPageIndex;
        loadEmployeeData();
    }

    protected void GvEmployeeData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortBy = e.SortExpression;
        string lastSortExpression = "";
        string lastSortDirection = "ASC";
        int _employerID = int.Parse(HfDistrictID.Value);

        List<Employee_I> tempList = (List<Employee_I>)Session["importEmployeeAlertDetails"];

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
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_EXT_ID).ToList();
                            break;
                        case "EMPLOYEE_FIRST_NAME":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_FIRST_NAME).ToList();
                            break;
                        case "EMPLOYEE_LAST_NAME":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_LAST_NAME).ToList();
                            break;
                        case "EMPLOYEE_I_HIRE_DATE":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_I_HIRE_DATE).ToList();
                            break;
                        case "EMPLOYEE_I_DOB":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_I_DOB).ToList();
                            break;
                        case "Employee_SSN_Hidden":
                            tempList = tempList.OrderBy(o => o.Employee_SSN_Hidden).ToList();
                            break;
                        case "EMPLOYEE_HR_EXT_STATUS_DESCRIPTION":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_HR_EXT_DESCRIPTION).ToList();
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

            Session["importEmployeeAlertDetails"] = tempList;
            Session["sortDir"] = lastSortDirection;
            Session["sortExp"] = lastSortExpression;
            loadEmployeeData();
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }
    }

    

    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
    }

    

   

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int z = 0;                      
        int i = 0;                    
        List<Employee_I> tempList = (List<Employee_I>)Session["importEmployeeAlertDetails"];
        List<Employee_I> removedAlerts = new List<Employee_I>();
        int _employerID = int.Parse(HfDistrictID.Value);

        if (Cb_u_HRstatus.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(Ddl_u_HrStatus, validData);
            z += 1;
        }
        else
        {
            Ddl_u_HrStatus.BackColor = System.Drawing.Color.White;
        }

        if ( Cb_u_EmployeeClass.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(Ddl_u_EmployeeClass, validData);
            z += 1;
        }
        else
        {
            Ddl_u_EmployeeClass.BackColor = System.Drawing.Color.White;
        }


        if (Cb_u_ACAstatus.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(Ddl_u_AcaStatus, validData);
            z += 1;
        }
        else
        {
            Ddl_u_AcaStatus.BackColor = System.Drawing.Color.White;
        }

        if (Cb_u_HireDate.Checked == true)
        {
            validData = errorChecking.validateTextBoxDate(Txt_u_HireDate, validData);
            z += 1;
        }
        else
        {
            Txt_u_HireDate.BackColor = System.Drawing.Color.White;
        }

        if (Cb_u_TermDate.Checked == true)
        {
            validData = errorChecking.validateTextBoxDate(Txt_u_TermDate, validData);
            z += 1;
        }
        else
        {
            Txt_u_TermDate.BackColor = System.Drawing.Color.White;
        }

        if (Cb_u_PlanYear.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(Ddl_u_PlanYear, validData);
            z += 1;
        }
        else
        {
            Ddl_u_PlanYear.BackColor = System.Drawing.Color.White;
        }

        foreach (GridViewRow row in GvEmployeeData.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");

            if (cb.Checked == true)
            {
                i += 1;
            }
        }

        if (i == 0 || z == 0)
        {
            if (i == 0)
            {
                MpeWebMessage.Show();
                CbCheckAll.BackColor = System.Drawing.Color.Red;
                LitMessage.Text = "You must select a record to update. Please select a record by clicking on the check box in the list of alerts.";
            }
            else
            {
                CbCheckAll.BackColor = System.Drawing.Color.White;
            }
            if (z == 0)
            {
                MpeWebMessage.Show();
                LitMessage.Text = "You must select a field to update.";
            }

            validData = false;
        }
        else
        {
            CbCheckAll.BackColor = System.Drawing.Color.White;
        }
        if (validData == true)
        {
            List<hrStatus> hrsList = hrStatus_Controller.manufactureHRStatusList(_employerID);

            foreach (GridViewRow row in GvEmployeeData.Rows)
            {
                try
                {
                    CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
                    HiddenField hf = (HiddenField)row.FindControl("Hf_gv_RowID");

                    if (cb.Checked == true)
                    {
                        int _rowID = int.Parse(hf.Value);
                        int _employeeTypeID = 0;
                        int _hrStatusID = 0;
                        string _hrStatusExt = null;
                        string _hrStatusDesc = null;
                        int _planYearID = 0;
                        int _stateID = 0;
                        int _acaStatusID = 0;
                        int _classID = 0;
                        string _payrollID = null;
                        DateTime? _hDate = null;
                        string _hDateI = null;
                        DateTime? _cDate = null;
                        DateTime? _tDate = null;
                        string _tDateI = null;
                        DateTime? _dob = null;
                        string _dobI = null;
                        DateTime? _impEnd = null;

                        Employee_I tempEmployee = EmployeeController.findEmployee(tempList, _rowID);

                        if (Cb_u_HRstatus.Checked == true) 
                        { 
                            _hrStatusID = int.Parse(Ddl_u_HrStatus.SelectedItem.Value);
                            foreach (hrStatus hrs in hrsList)
                            {
                                if (hrs.HR_STATUS_ID == _hrStatusID)
                                {
                                    _hrStatusExt = hrs.HR_STATUS_EXTERNAL_ID;
                                    _hrStatusDesc = hrs.HR_STATUS_NAME;
                                    break;
                                }
                            }
                        } 
                        else 
                        { 
                            _hrStatusID = tempEmployee.EMPLOYEE_HR_STATUS_ID;
                            _hrStatusExt = tempEmployee.EMPLOYEE_HR_EXT_STATUS_ID;
                            _hrStatusDesc = tempEmployee.EMPLOYEE_HR_EXT_DESCRIPTION;
                        }

                        if (Cb_u_HireDate.Checked == true) 
                        { 
                            _hDate = DateTime.Parse(Txt_u_HireDate.Text);
                            _hDateI = errorChecking.convertShortDate(((DateTime)_hDate).ToShortDateString());
                        } 
                        else 
                        { 
                            _hDate = tempEmployee.EMPLOYEE_HIRE_DATE;
                            _hDateI = tempEmployee.EMPLOYEE_I_HIRE_DATE;
                        }
                        
                        if (Cb_u_TermDate.Checked == true) 
                        { 
                            _tDate = DateTime.Parse(Txt_u_TermDate.Text);
                            _tDateI = errorChecking.convertShortDate(((DateTime)_tDate).ToShortDateString());
                        } 
                        else 
                        { 
                            _tDate = tempEmployee.EMPLOYEE_TERM_DATE;
                            _tDateI = tempEmployee.EMPLOYEE_I_TERM_DATE;
                        }

                        if (Cb_u_EmployeeClass.Checked == true) {_classID=int.Parse(Ddl_u_EmployeeClass.SelectedItem.Value); } else {_classID = tempEmployee.EMPLOYEE_CLASS_ID; }
                        if (Cb_u_ACAstatus.Checked == true) { _acaStatusID = int.Parse(Ddl_u_AcaStatus.SelectedItem.Value); } else { _acaStatusID = tempEmployee.EMPLOYEE_ACT_STATUS_ID; }
                        if (Cb_u_PlanYear.Checked == true) { _planYearID = int.Parse(Ddl_u_PlanYear.SelectedItem.Value); } else { _planYearID = tempEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS; }
                        
                        _employeeTypeID = tempEmployee.EMPLOYEE_TYPE_ID;
                        _stateID = tempEmployee.EMPLOYEE_STATE_ID;
                        _dob = tempEmployee.EMPLOYEE_DOB;
                        _dobI = tempEmployee.EMPLOYEE_I_DOB;
                        _impEnd = tempEmployee.EMPLOYEE_IMP_END;

                        EmployeeController.UpdateImportEmployee(_rowID, _employeeTypeID, _hrStatusID, _hrStatusExt, _hrStatusDesc, _planYearID, _stateID, _hDate, _hDateI, _cDate, _tDate, _tDateI, _dob, _dobI, _impEnd, _acaStatusID, _classID); 
                    }
                }
                catch (Exception exception)
                {
                    this.Log.Warn("Suppressing errors.", exception);
                }
            }
            MpeWebMessage.Show();
            LitMessage.Text = "The records were updated.";
        }

    }


    private void syncPayrollAlertSession(List<Payroll_I> _removedPayrolls)
    {
        List<Payroll_I> tempList = (List<Payroll_I>)Session["importPayAlertDetails"];

        foreach (Payroll_I pi in _removedPayrolls)
        {
            tempList.Remove(pi);
        }

        Session["importPayAlertDetails"] = tempList;
    }

 
}