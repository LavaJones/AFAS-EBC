using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Afas.AfComply.Domain;
using log4net;
using System.Text;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.Domain.POCO;



public partial class securepages_alerts : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(securepages_alerts));

    protected override void PageLoadNonPostBack()
    {
        Session["ShowSSN"] = false;
    }

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        HfUserName.Value = user.User_Full_Name;
        HfDistrictID.Value = user.User_District_ID.ToString();

        if (employer.EMPLOYER_ID == 1)
        {
        }

        loadAlertTypes(employer.EMPLOYER_ID, true);
        loadAlertDetails(employer.EMPLOYER_ID);
    }
    /// <summary>
    /// 20)
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="reset"></param>
    private void loadAlertTypes(int _employerID, bool reset)
    {
        int selectedIndex = GvAlerts.SelectedIndex;
        int rows = GvAlerts.Rows.Count;
        int rows2 = 0;
        if (Session["districtAlerts"] == null || reset == true)
        {
            Session["districtAlerts"] = alert_controller.manufactureEmployerAlertList(_employerID);
        }

        GvAlerts.DataSource = (List<alert>)Session["districtAlerts"];
        GvAlerts.DataBind();

        rows2 = GvAlerts.Rows.Count;

        if (selectedIndex < rows2)
        {
            if (selectedIndex == -1)
            {
                GvAlerts.SelectedIndex = 0;
            }
            else
            {
                GvAlerts.SelectedIndex = selectedIndex;
            }
        }
        else
        {
            GvAlerts.SelectedIndex = -1;
        }
    }

    /// <summary>
    /// 50)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvAlerts_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        loadAlertDetails(_employerID);
    }
    /// <summary>
    /// 40)
    /// </summary>
    /// <param name="_employerID"></param>
    private void loadAlertDetails(int _employerID)
    {
        if (Session["importEmployeeList"] == null)
        {
            Session["importEmployeeList"] = EmployeeController.manufactureImportEmployeeList(_employerID);
        }
        if (Session["importPayAlertDetails"] == null)
        {
            Session["importPayAlertDetails"] = Payroll_Controller.manufactureEmployerPayrollImportList(_employerID);
        }
        if (Session["insuranceAlertDetails"] == null)
        {
            Session["insuranceAlertDetails"] = alert_controller.manufactureEmployerInsuranceAlertList(_employerID);
        }
        if (Session["carrierImport"] == null)
        {
            Session["carrierImport"] = insuranceController.manufactureInsuranceCoverageAlerts(_employerID);
        }

        if (GvAlerts.Rows.Count > 0)
        {
            if (GvAlerts.Rows.Count == 1)
            {
                GvAlerts.SelectedIndex = 0;
            }
            HiddenField hfAlertID = null;
            GridViewRow row = GvAlerts.SelectedRow;
            int alertTypeID = 0;

            hfAlertID = (HiddenField)row.FindControl("HfAlertTypeID");
            alertTypeID = int.Parse(hfAlertID.Value);
            List<Payroll_I> tempList = new List<Payroll_I>();
            List<Employee_I> tempList2 = new List<Employee_I>();
            List<alert_insurance> tempList3 = new List<alert_insurance>(); ;
            List<insurance_coverage_I> tempList6 = new List<insurance_coverage_I>();

            List<alert_insurance> tempList7 = new List<alert_insurance>();

            switch (alertTypeID)
            {
                case 1:
                    Pnl_BillWarning.Visible = false;
                    Pnl_InsuranceTypeWarning.Visible = false;
                    Pnl_IRSWarning.Visible = false;
                    Pnl_EmployeeAlertDetails.Visible = true;

                    tempList = (List<Payroll_I>)Session["importPayAlertDetails"];
                    if (tempList.Count > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        foreach (var item in tempList)
                        {
                            builder.Append(item.ROW_ID);
                            builder.Append(',');
                        }
                        PIILogger.LogPII(String.Format("Displaying importPayAlertDetails to User {0} with Ids: " + builder.ToString(), ((User)Session["CurrentUser"]).User_ID));

                        GvAlertDetail.DataSource = tempList;
                        GvAlertDetail.DataBind();
                    }
                    else
                    {
                        GvAlertDetail.DataSource = null;
                        GvAlertDetail.DataBind();
                    }
                    break;
                case 2:
                    Pnl_BillWarning.Visible = false;
                    Pnl_InsuranceTypeWarning.Visible = false;
                    Pnl_IRSWarning.Visible = false;
                    Pnl_EmployeeAlertDetails.Visible = true;

                    tempList2 = (List<Employee_I>)Session["importEmployeeList"];
                    if (tempList2.Count > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        foreach (var item in tempList2)
                        {
                            builder.Append(item.ROW_ID);
                            builder.Append(',');
                        }
                        PIILogger.LogPII(String.Format("Displaying importEmployeeList to User {0} with Ids: " + builder.ToString(), ((User)Session["CurrentUser"]).User_ID));

                        GvAlertDetail.DataSource = tempList2;
                        GvAlertDetail.DataBind();
                    }
                    else
                    {
                        GvAlertDetail.DataSource = null;
                        GvAlertDetail.DataBind();
                    }
                    break;
                case 3:
                    Pnl_BillWarning.Visible = false;
                    Pnl_InsuranceTypeWarning.Visible = false;
                    Pnl_IRSWarning.Visible = false;
                    Pnl_EmployeeAlertDetails.Visible = true;

                    tempList3 = (List<alert_insurance>)Session["insuranceAlertDetails"];

                    foreach (alert_insurance ai in tempList3)
                    {
                        if (ai.IALERT_OFFERED == null)
                        {
                            tempList7.Add(ai);
                        }
                    }


                    if (tempList3.Count > 0)
                    {
                        GvAlertDetail.DataSource = tempList7;  
                        GvAlertDetail.DataBind();
                    }
                    else
                    {
                        GvAlertDetail.DataSource = null;
                        GvAlertDetail.DataBind();
                    }

                    break;
                case 4:
                    break;
                case 5:
                    Pnl_BillWarning.Visible = true;
                    Pnl_InsuranceTypeWarning.Visible = false;
                    Pnl_IRSWarning.Visible = false;
                    Pnl_EmployeeAlertDetails.Visible = false;

                    Ddl_bill_Users.DataSource = UserController.getDistrictUsers(_employerID);
                    Ddl_bill_Users.DataTextField = "User_Full_Name";
                    Ddl_bill_Users.DataValueField = "User_ID";
                    Ddl_bill_Users.DataBind();
                    Ddl_bill_Users.Items.Add("Select");
                    Ddl_bill_Users.SelectedIndex = Ddl_bill_Users.Items.Count - 1;
                    GvAlertDetail.DataSource = null;
                    GvAlertDetail.DataBind();
                    break;
                case 6:
                    Pnl_BillWarning.Visible = false;
                    Pnl_InsuranceTypeWarning.Visible = false;
                    Pnl_IRSWarning.Visible = true;
                    Pnl_EmployeeAlertDetails.Visible = false;

                    Ddl_irs_Users.DataSource = UserController.getDistrictUsers(_employerID);
                    Ddl_irs_Users.DataTextField = "User_Full_Name";
                    Ddl_irs_Users.DataValueField = "User_ID";
                    Ddl_irs_Users.DataBind();
                    Ddl_irs_Users.Items.Add("Select");
                    Ddl_irs_Users.SelectedIndex = Ddl_irs_Users.Items.Count - 1;
                    GvAlertDetail.DataSource = null;
                    GvAlertDetail.DataBind();
                    break;
                case 7:
                    Pnl_BillWarning.Visible = false;
                    Pnl_InsuranceTypeWarning.Visible = true;
                    Pnl_IRSWarning.Visible = false;
                    Pnl_EmployeeAlertDetails.Visible = false;

                    GvAlertDetail.DataSource = null;
                    GvAlertDetail.DataBind();
                    loadErroredInsurancePlans();
                    break;
                case 8:
                    Pnl_BillWarning.Visible = false;
                    Pnl_InsuranceTypeWarning.Visible = false;
                    Pnl_IRSWarning.Visible = false;
                    Pnl_EmployeeAlertDetails.Visible = true;
                    tempList6 = (List<insurance_coverage_I>)Session["carrierImport"];
                    if (tempList6.Count > 0)
                    {
                        GvAlertDetail.DataSource = tempList6;
                        GvAlertDetail.DataBind();
                    }
                    else
                    {
                        GvAlertDetail.DataSource = null;
                        GvAlertDetail.DataBind();
                    }
                    break;
                default:

                    break;
            }

        }
        else
        {
            GvAlertDetail.DataSource = null;
            GvAlertDetail.DataBind();
        }
    }






    /// <summary>
    /// 80) When user selects an alert to view. This is triggered. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvAlertDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        HiddenField hfAlertID = null;
        GridViewRow row = GvAlerts.SelectedRow;
        int alertTypeID = 0;

        hfAlertID = (HiddenField)row.FindControl("HfAlertTypeID");
        alertTypeID = int.Parse(hfAlertID.Value);

        switch (alertTypeID)
        {
            case 1:
                showPayrollAlert();
                break;
            case 2:
                showEmployeeAlert();
                break;
            case 3:
                showInsuranceAlert();
                break;
            case 8:
                showInsuranceCarrierAlert();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 90) 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvAlertDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        GvAlertDetail.PageIndex = e.NewPageIndex;

        PIILogger.LogPII(String.Format("User {0} Change Alert View to Page: " + e.NewPageIndex, ((User)Session["CurrentUser"]).User_ID));

        loadAlertDetails(_employerID);

    }


    /// <summary>
    /// 100) Determine which alert type is being updated and attempt to update the record. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvAlertDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        HiddenField hfAlertID = null;
        GridViewRow row = GvAlerts.SelectedRow;
        int alertTypeID = 0;

        hfAlertID = (HiddenField)row.FindControl("HfAlertTypeID");
        alertTypeID = int.Parse(hfAlertID.Value);

        switch (alertTypeID)
        {
            case 1:
                transferPayroll();
                break;
            case 2:
                transferEmployeeRecord();
                break;
            case 3:
                completeInsuranceOffer();
                break;
            case 8:
                updateInsuranceCarrierAlert();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 110) This function will 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvAlertDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        Log.Warn(String.Format("User {0} requested alert to be deleted.", ((User)Session["CurrentUser"]).User_ID));

        HiddenField hfAlertID = null;
        GridViewRow row = GvAlerts.SelectedRow;
        int alertTypeID = 0;

        hfAlertID = (HiddenField)row.FindControl("HfAlertTypeID");
        alertTypeID = int.Parse(hfAlertID.Value);

        Log.Info(string.Format("Alert delete: Alert Type Id: [{0}] .", alertTypeID));

        switch (alertTypeID)
        {
            case 1:
                deletePayrollAlert();
                break;
            case 2:
                deleteEmployeeAlert();
                break;
            case 3:
                break;
            case 8:
                deleteInsuranceCarrierImport();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 140)
    /// </summary>
    private void showPayrollAlert()
    {
        GridViewRow row = GvAlertDetail.SelectedRow;
        int _employerID = int.Parse(HfDistrictID.Value);
        int _rowID = 0;
        List<Payroll_I> tempPayrollList = null;
        Payroll_I tempPayroll = null;

        if (Session["importPayAlertDetails"] == null)
        {
            Session["importPayAlertDetails"] = Payroll_Controller.manufactureEmployerPayrollImportList(_employerID);
        }

        tempPayrollList = (List<Payroll_I>)Session["importPayAlertDetails"];

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditPayroll");
        Literal litname = (Literal)row.FindControl("Lit_Pay_Name");
        Literal litPayDesc = (Literal)row.FindControl("Lit_Pay_Description");
        Literal litPayrollID = (Literal)row.FindControl("Lit_Pay_EmpExtID");
        Label LblPayrollMess = (Label)row.FindControl("LblPayrollMessage");
        TextBox txtSSN = (TextBox)row.FindControl("Txt_pay_ssn");
        TextBox txtSdate = (TextBox)row.FindControl("Txt_pay_sdate");
        TextBox txtEdate = (TextBox)row.FindControl("Txt_pay_edate");
        TextBox txtCdate = (TextBox)row.FindControl("Txt_pay_cdate");
        DropDownList ddlPayDesc = (DropDownList)row.FindControl("Ddl_pay_description");

        TextBox txtHours = (TextBox)row.FindControl("Txt_pay_hours");
        bool validData = true;

        _rowID = int.Parse(hfRowID.Value);
        ddlPayDesc.DataSource = gpType_Controller.getEmployeeTypes(_employerID);

        ddlPayDesc.DataTextField = "GROSS_PAY_DESCRIPTION_EXTERNAL_ID";
        ddlPayDesc.DataValueField = "GROSS_PAY_EXTERNAL_ID";
        ddlPayDesc.DataBind();
        ddlPayDesc.Items.Add("Select");
        ddlPayDesc.SelectedIndex = ddlPayDesc.Items.Count - 1;

        tempPayroll = Payroll_Controller.getSinglePayroll_I(_rowID, tempPayrollList);
        Session["EditingPayrollRecord"] = tempPayroll;
        if (tempPayroll == null)
        {
            return;
        }
        
        litname.Text = tempPayroll.EMPLOYEE_FULL_NAME;

        errorChecking.setDropDownList(ddlPayDesc, tempPayroll.PAY_GP_EXT_ID);

        try
        {
            DateTime sdate = (DateTime)tempPayroll.PAY_SDATE;
            txtSdate.Text = sdate.ToShortDateString();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            txtSdate.Text = null;
        }
        try
        {
            DateTime edate = (DateTime)tempPayroll.PAY_EDATE;
            txtEdate.Text = edate.ToShortDateString();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            txtEdate.Text = null;
        }
        try
        {
            DateTime cdate = (DateTime)tempPayroll.PAY_CDATE;
            if (cdate > new DateTime(1920, 1, 1))
            {
                txtCdate.Text = cdate.ToShortDateString();
            }
            else
            {
                txtCdate.Text = null;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            txtCdate.Text = null;
        }

        PIILogger.LogPII(string.Format("Showing Payroll alert detailed information to user Id:[{0}] at IP:[{1}]. Employee Id:[{2}], Row Id:[{3}]", ((User)Session["CurrentUser"]).User_ID, Request.UserHostAddress, tempPayroll.PAY_EMPLOYEE_ID, tempPayroll.ROW_ID));

        txtSSN.Text = tempPayroll.PAY_SSN;
        validData = errorChecking.validateTextBoxSSN(txtSSN, validData);

        if (tempPayroll.PAY_SSN != null && tempPayroll.PAY_SSN.IsValidSsn())
        {
            if (Session["showSSN"] != null && (bool)Session["showSSN"])
            {
                txtSSN.Text = tempPayroll.PAY_SSN;
            }
            else
            {
                txtSSN.Text = tempPayroll.PAY_SSN_HIDDEN;
            }
            txtSSN.BackColor = System.Drawing.Color.White;
        }
        else
        {
            txtSSN.Text = tempPayroll.PAY_SSN;
            txtSSN.BackColor = System.Drawing.Color.Red;
        }

        litPayrollID.Text = tempPayroll.EMPLOYEE_EXT_ID;
        txtHours.Text = tempPayroll.PAY_HOURS.ToString();

        validData = errorChecking.validateTextBoxDate(txtSdate, validData);
        validData = errorChecking.validateTextBoxDate(txtEdate, validData);
        validData = errorChecking.validateTextBoxDate(txtCdate, validData);
        validData = errorChecking.validateTextBoxDecimalACAHours(txtHours, validData);
        validData = errorChecking.validateDropDownSelection(ddlPayDesc, validData);

        if (validData == true)
        {
            LblPayrollMess.Text = "This payroll record has valid data. The EMPLOYEE has not been imported into the System.";
            LblPayrollMess.Text += "To correct this please add the EMPLOYEE demographics manually by going to the EMPLOYEE Management Screen.";
        }
        else
        {
            LblPayrollMess.Text = "Please correct all red fields.";
        }

        mpe.Show();

    }


    /// <summary>
    /// 150) This function will attempt to transfer an employee record from the IMPORT_EMPLOYEE table to the actual EMPLOYEE table. 
    /// </summary>
    /// <returns></returns>
    private Payroll transferPayroll()
    {
        Payroll newPayroll = null;
        GridViewRow row = GvAlertDetail.SelectedRow;
        bool validData = true;
        int _employerID = int.Parse(HfDistrictID.Value);


        employer emp = employerController.getEmployer(_employerID);

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditPayroll");
        TextBox txtSSN = (TextBox)row.FindControl("Txt_pay_ssn");
        TextBox txtSdate = (TextBox)row.FindControl("Txt_pay_sdate");
        TextBox txtEdate = (TextBox)row.FindControl("Txt_pay_edate");
        TextBox txtCdate = (TextBox)row.FindControl("Txt_pay_cdate");
        TextBox txtHours = (TextBox)row.FindControl("Txt_pay_hours");
        Label lblMessage = (Label)row.FindControl("LblPayrollMessage");
        DropDownList ddlGP = (DropDownList)row.FindControl("Ddl_pay_description");

        Payroll_I tempPayI = (Payroll_I)Session["EditingPayrollRecord"];

        if (null == tempPayI)
        {
            Log.Error("User atempted to save a payroll edit that we did not have in session.");
            return newPayroll;
        }

        validData = errorChecking.validateTextBoxDate(txtSdate, validData);
        validData = errorChecking.validateTextBoxDate(txtEdate, validData);
        validData = errorChecking.validateTextBoxDate(txtCdate, validData);
        validData = errorChecking.validateTextBoxDecimalACAHours(txtHours, validData);
        validData = errorChecking.validateDropDownSelection(ddlGP, validData);

        if (txtSSN.Text.IsValidSsn() || Session["showSSN"] != null && (bool)Session["showSSN"])
        {
            validData = errorChecking.validateTextBoxSSN(txtSSN, validData);
        }
        else if (validData)
        {
            txtSSN.Text = tempPayI.PAY_SSN;
            validData = errorChecking.validateTextBoxSSN(txtSSN, validData);
        }
        else
        {
            txtSSN.Text = tempPayI.PAY_SSN;
            if (errorChecking.validateTextBoxSSN(txtSSN, true))
            {
                txtSSN.Text = tempPayI.PAY_SSN_HIDDEN;
            }        
        }

        if (validData == true)
        {
            int _rowID = 0;
            int _employeeID = 0;
            string _gpExtID = null;
            string _gpDescription = null;
            DateTime _sdate;
            DateTime _edate;
            DateTime _cdate;
            decimal _hours = 0;
            DateTime _modOn = DateTime.Now;
            string _modBy = HfUserName.Value;
            gpType currGPType = null;
            string _ssn = null;
            string _history = null;

            _gpExtID = ddlGP.SelectedItem.Value;

            List<gpType> tempGPlist = gpType_Controller.getEmployeeTypes(_employerID);

            currGPType = gpType_Controller.validateGpType(_employerID, _gpExtID, null, tempGPlist);

            _rowID = int.Parse(hfRowID.Value);
            _sdate = DateTime.Parse(txtSdate.Text);
            _edate = DateTime.Parse(txtEdate.Text);
            _cdate = DateTime.Parse(txtCdate.Text);
            _hours = txtHours.Text.checkDecimalNull();
            _ssn = txtSSN.Text.checkStringNull();
            _history = "Import Values: " + _modBy + " " + _modOn.ToString();
            _history += " |" + tempPayI.PAY_I_HOURS + "|" + tempPayI.PAY_I_SDATE + "|" + tempPayI.PAY_I_EDATE + "|" + tempPayI.PAY_I_CDATE + " | " + tempPayI.PAY_GP_EXT_ID + " | " + tempPayI.PAY_GP_DESC;

            if (tempPayI.PAY_EMPLOYEE_ID == 0)
            {
                List<Employee> tempList = new List<Employee>();
                Employee tempEmployee = null;
                tempList = EmployeeController.manufactureEmployeeList(_employerID);
                tempEmployee = EmployeeController.validateExistingEmployee(tempList, _ssn);

                if (tempEmployee == null)
                {
                    lblMessage.Text = "No employee found for SSN : " + _ssn;
                    mpe.Show();
                    return null;
                }

                _employeeID = tempEmployee.EMPLOYEE_ID;
            }
            else
            {
                _employeeID = tempPayI.PAY_EMPLOYEE_ID;
            }

            validData = Payroll_Controller.TransferPayroll(_rowID, _employerID, tempPayI.PAY_BATCH_ID, _employeeID, currGPType.GROSS_PAY_ID, _hours, _sdate, _edate, _cdate, _modBy, _modOn, _history);

            if (validData == true)
            {
                mpe.Hide();
                Session["showSSN"] = false;

                List<Payroll_I> tempListI = (List<Payroll_I>)Session["importPayAlertDetails"];
                tempListI.RemoveAll(s => s.ROW_ID == _rowID);

                Session["importPayAlertDetails"] = tempListI;

                loadAlertTypes(_employerID, true);
                loadAlertDetails(_employerID);
                GvAlertDetail.SelectedIndex = -1;
            }
            else
            {
                lblMessage.Text = "An error occurred. Please, check that this employee's demographic information has been imported and their SSN is correct.";
                mpe.Show();
            }

        }
        else
        {
            lblMessage.Text = "Please correct all RED fields.";
            mpe.Show();

        }

        return newPayroll;
    }

    /// <summary>
    /// 120)
    /// </summary>
    private void deletePayrollAlert()
    {
        Log.Info(string.Format("Payroll Alert delete called."));

        GridViewRow row = GvAlertDetail.SelectedRow;
        int _employerID = int.Parse(HfDistrictID.Value);
        int _rowID = 0;
        employer emp = employerController.getEmployer(_employerID);
        bool recordRemoved = false;

        Log.Info(string.Format("Payroll Alert delete: Employer Id: [{0}], Employer is: [{1}].", _employerID, emp));

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");

        _rowID = int.Parse(hfRowID.Value);

        Log.Info(string.Format("Payroll Alert delete: Row Id: [{0}] .", _rowID));

        recordRemoved = Payroll_Controller.deleteImportedPayrollRow(_rowID);

        Log.Info(string.Format("Payroll Alert delete: Row Id: [{0}] with result [{1}].", _rowID, recordRemoved));

        if (recordRemoved == true)
        {
            List<Payroll_I> tempList = (List<Payroll_I>)Session["importPayAlertDetails"];

            foreach (Payroll_I ei in tempList)
            {
                if (ei.ROW_ID == _rowID)
                {
                    tempList.Remove(ei);
                    break;
                }
            }

            Session["importPayAlertDetails"] = tempList;

            loadAlertTypes(_employerID, true);
            loadAlertDetails(_employerID);
        }
    }
    protected void ImgBtnViewSSN_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = GvAlertDetail.SelectedRow;
        ImageButton ImgBtnViewSSN = (ImageButton)row.FindControl("ImgBtnViewSSN");

        if (Session["showSSN"] != null)
        {
            bool value = (bool)Session["showSSN"];
            if (value == true)
            {
                ImgBtnViewSSN.ImageUrl = "~/design/eyeclosed.png";
                Session["showSSN"] = false;
            }
            else
            {
                ImgBtnViewSSN.ImageUrl = "~/design/eyeopen.png";
                Session["showSSN"] = true;
            }
        }
        else
        {
            Session["showSSN"] = false;
            ImgBtnViewSSN.ImageUrl = "~/design/eyeclosed.png";
        }

        showEmployeeAlert();
    }

    protected void ImgBtnViewSSNPay_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = GvAlertDetail.SelectedRow;
        ImageButton ImgBtnViewSSN = (ImageButton)row.FindControl("ImgBtnViewSSNPay");

        if (Session["showSSN"] != null)
        {
            bool value = (bool)Session["showSSN"];
            if (value == true)
            {
                ImgBtnViewSSN.ImageUrl = "~/design/eyeclosed.png";
                Session["showSSN"] = false;
            }
            else
            {
                ImgBtnViewSSN.ImageUrl = "~/design/eyeopen.png";
                Session["showSSN"] = true;
            }
        }
        else
        {
            Session["showSSN"] = false;
            ImgBtnViewSSN.ImageUrl = "~/design/eyeclosed.png";
        }

        showPayrollAlert();
    }

    protected void ImgBtnViewSSNIC_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = GvAlertDetail.SelectedRow;
        ImageButton ImgBtnViewSSN = (ImageButton)row.FindControl("ImgBtnViewSSNIC");

        if (Session["showSSN"] != null)
        {
            bool value = (bool)Session["showSSN"];
            if (value == true)
            {
                ImgBtnViewSSN.ImageUrl = "~/design/eyeclosed.png";
                Session["showSSN"] = false;
            }
            else
            {
                ImgBtnViewSSN.ImageUrl = "~/design/eyeopen.png";
                Session["showSSN"] = true;
            }
        }
        else
        {
            Session["showSSN"] = false;
            ImgBtnViewSSN.ImageUrl = "~/design/eyeclosed.png";
        }

        showInsuranceCarrierAlert();
    }


    /// <summary>
    /// 
    /// </summary>
    private void showEmployeeAlert()
    {
        if (GvAlertDetail == null || HfDistrictID == null)
        {
            Log.Error("Aspx component was null< either HfDistrictID:["+ HfDistrictID + "] or GvAlertDetail:["+ GvAlertDetail + "]." );

            return;
        }

        GridViewRow row = GvAlertDetail.SelectedRow;
        int _employerID = int.Parse(HfDistrictID.Value);
        List<PlanYear> planYearList = null;

        if (row == null)
        {
            Log.Warn("Trying to show employee Alert when selected ro is null.");

            return;
        }
        
        /**********************************************************************************
         ***** Step 1: Check all temp lists needed for this function. ********************* 
         **********************************************************************************/
        if (Session["Employees"] == null)
        {
            Session["Employees"] = EmployeeController.manufactureEmployeeList(_employerID);
        }

        planYearList = PlanYear_Controller.getEmployerPlanYear(_employerID);

        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditEmployee");
        DropDownList ddlPlans = (DropDownList)row.FindControl("Ddl_ei_Plan");
        DropDownList ddlStates = (DropDownList)row.FindControl("Ddl_ei_State");
        DropDownList ddlEmployeeType = (DropDownList)row.FindControl("Ddl_ei_EmpType");
        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        TextBox txtfname = (TextBox)row.FindControl("Txt_ei_FirstName");
        TextBox txtlname = (TextBox)row.FindControl("Txt_ei_LastName");
        TextBox txtaddress = (TextBox)row.FindControl("Txt_ei_Address");
        TextBox txtcity = (TextBox)row.FindControl("Txt_ei_City");
        TextBox txtzip = (TextBox)row.FindControl("Txt_ei_Zip");
        TextBox txtssn = (TextBox)row.FindControl("Txt_ei_SSN");
        TextBox txtdob = (TextBox)row.FindControl("Txt_ei_DOB");
        TextBox txthdate = (TextBox)row.FindControl("Txt_ei_Hdate");
        TextBox txtemployeeID = (TextBox)row.FindControl("Txt_ei_EmployeeID");
        TextBox txtpayID = (TextBox)row.FindControl("Txt_ei_PayrollID");
        Panel pnlIns = (Panel)row.FindControl("PnlNewHireInsurance");
        Literal litMess = (Literal)row.FindControl("LitOfferMessage");

        DropDownList ddl_hrstatus = (DropDownList)row.FindControl("Ddl_ei_HRStatus"); ;
        DropDownList ddl_classification = (DropDownList)row.FindControl("Ddl_ei_Classification"); ;
        DropDownList ddl_acaStatus = (DropDownList)row.FindControl("Ddl_ei_ActStatus"); ;
        DropDownList ddl_planYears = (DropDownList)row.FindControl("Ddl_ei_PlanYear_Coverage");
        DropDownList ddl_insOffer = (DropDownList)row.FindControl("Ddl_ei_Insurance");

        int _rowID = int.Parse(hfRowID.Value);

        ddl_insOffer.SelectedIndex = ddl_insOffer.Items.Count - 1;

        ddl_hrstatus.DataSource = hrStatus_Controller.manufactureHRStatusList(_employerID);
        ddl_hrstatus.DataTextField = "HR_STATUS_NAME";
        ddl_hrstatus.DataValueField = "HR_STATUS_ID";
        ddl_hrstatus.DataBind();
        ddl_hrstatus.Items.Add("Select");
        ddl_hrstatus.SelectedIndex = ddl_hrstatus.Items.Count - 1;

        ddl_classification.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        ddl_classification.DataTextField = "CLASS_DESC";
        ddl_classification.DataValueField = "CLASS_ID";
        ddl_classification.DataBind();

        ddl_classification.Items.Add("Select");
        ddl_classification.SelectedIndex = ddl_classification.Items.Count - 1;

        ddl_acaStatus.DataSource = classificationController.getACAstatusList();
        ddl_acaStatus.DataTextField = "ACA_STATUS_NAME";
        ddl_acaStatus.DataValueField = "ACA_STATUS_ID";
        ddl_acaStatus.DataBind();
        ddl_acaStatus.Items.Add("Select");
        ddl_acaStatus.SelectedIndex = ddl_acaStatus.Items.Count - 1;

        ddlStates.DataSource = StateController.getStates();
        ddlStates.DataTextField = "State_Name";
        ddlStates.DataValueField = "State_ID";
        ddlStates.DataBind();
        ddlStates.Items.Add("Select");
        ddlStates.SelectedIndex = ddlStates.Items.Count - 1;

        ddlPlans.DataSource = planYearList;
        ddlPlans.DataTextField = "PLAN_YEAR_DESCRIPTION";
        ddlPlans.DataValueField = "PLAN_YEAR_ID";
        ddlPlans.DataBind();
        ddlPlans.Items.Add("Select");
        ddlPlans.SelectedIndex = ddlPlans.Items.Count - 1;

        ddl_planYears.DataSource = planYearList;
        ddl_planYears.DataTextField = "PLAN_YEAR_DESCRIPTION";
        ddl_planYears.DataValueField = "PLAN_YEAR_ID";
        ddl_planYears.DataBind();
        ddl_planYears.Items.Add("Select");
        ddl_planYears.SelectedIndex = ddl_planYears.Items.Count - 1;

        ddlEmployeeType.DataSource = EmployeeTypeController.getEmployeeTypes(_employerID);
        ddlEmployeeType.DataTextField = "EMPLOYEE_TYPE_NAME";
        ddlEmployeeType.DataValueField = "EMPLOYEE_TYPE_ID";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Add("Select");
        ddlEmployeeType.SelectedIndex = ddlEmployeeType.Items.Count - 1;

        List<Employee_I> _tempList = (List<Employee_I>)Session["importEmployeeList"];
        List<Employee> _tempList2 = (List<Employee>)Session["Employees"];
        Employee_I currEmployeeI = EmployeeController.findEmployee(_tempList, _rowID);

        Employee currEmployee = EmployeeController.validateExistingEmployee(_tempList2, currEmployeeI.Employee_SSN_Visible);

        int _id = currEmployeeI.EMPLOYEE_STATE_ID;
        errorChecking.setDropDownList(ddlStates, _id);

        if (currEmployee != null)
        {
            txtemployeeID.Text = currEmployee.EMPLOYEE_ID.ToString();
            errorChecking.setDropDownList(ddl_acaStatus, currEmployee.EMPLOYEE_ACT_STATUS_ID);
            errorChecking.setDropDownList(ddl_classification, currEmployee.EMPLOYEE_CLASS_ID);
            errorChecking.setDropDownList(ddlPlans, currEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS);
            errorChecking.setDropDownList(ddlEmployeeType, currEmployee.EMPLOYEE_TYPE_ID);
            errorChecking.setDropDownList(ddl_hrstatus, currEmployeeI.EMPLOYEE_HR_STATUS_ID);

            pnlIns.Visible = false;
        }
        else
        {
            try
            {
                DateTime _hireDate = (DateTime)currEmployeeI.EMPLOYEE_HIRE_DATE;
                txtemployeeID.Text = "n/a";
                bool newHireTest = false;

                List<PlanYear> hiredDuring = PlanYear_Controller.findNewHirePlanYear(planYearList, _hireDate, _employerID);
                litMess.Text = null;

                if (hiredDuring.Count != 0)
                {
                    ddlPlans.Items.Clear();
                    ddlPlans.SelectedIndex = -1;

                    ddlPlans.DataSource = hiredDuring;
                    ddlPlans.DataTextField = "PLAN_YEAR_DESCRIPTION";
                    ddlPlans.DataValueField = "PLAN_YEAR_ID";
                    ddlPlans.DataBind();

                    if (ddlPlans.Items.Count == 1)
                    {
                        ddlPlans.SelectedIndex = ddlPlans.Items.Count - 1;
                        pnlIns.Visible = true;
                        litMess.Text = "The System has identified this Employee as a New Hire and has identified which Plan Year this employee was hired during. Please answer the questions below.";
                    }
                    else
                    {
                        ddlPlans.Items.Add("Select");
                        ddlPlans.SelectedIndex = ddlPlans.Items.Count - 1;
                        pnlIns.Visible = true;
                        litMess.Text = "The System has identified this Employee as a New Hire, However, the System was NOT able to identify which Plan Year this employee was hired during. Please answer the questions below.";
                    }
                }
                else
                {
                    newHireTest = PlanYear_Controller.validateNewHire(planYearList, _hireDate, _employerID);
                    if (newHireTest == false)
                    {
                        ddlPlans.Items.Clear();
                        ddlPlans.SelectedIndex = -1;
                        ddlPlans.Items.Add("Select");
                        ddlPlans.SelectedIndex = ddlPlans.Items.Count - 1;
                        pnlIns.Visible = true;
                        litMess.Text = "This employee looks to be a New Hire. However, the system was not able to identify which Plan Year this employee was hired during. Generally this is due to the Plan Year and Measurement Periods not being set-up. If you have any questions, please contact " + Branding.CompanyShortName;
                    }
                    else
                    {
                        pnlIns.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                pnlIns.Visible = true;
                litMess.Text = "Some data is missing, The System cannot determine if this employee is a New Hire or Not. Please answer the questions below.";
            }


            errorChecking.setDropDownList(ddl_classification, currEmployeeI.EMPLOYEE_CLASS_ID);
            errorChecking.setDropDownList(ddl_acaStatus, currEmployeeI.EMPLOYEE_ACT_STATUS_ID);
            errorChecking.setDropDownList(ddlPlans, currEmployeeI.EMPLOYEE_PLAN_YEAR_ID_MEAS);
            errorChecking.setDropDownList(ddlEmployeeType, currEmployeeI.EMPLOYEE_TYPE_ID);
            errorChecking.setDropDownList(ddl_hrstatus, currEmployeeI.EMPLOYEE_HR_STATUS_ID);
        }

        PIILogger.LogPII(String.Format("Showing EmployeeI Data to User ID:[{0}] at IP: [{1}] for employee Id: [{2}] RowId:[{3}]", ((User)Session["CurrentUser"]).User_ID, Request.UserHostAddress, currEmployeeI.EMPLOYEE_ID, currEmployeeI.ROW_ID));

        txtfname.Text = currEmployeeI.EMPLOYEE_FIRST_NAME;                  
        txtlname.Text = currEmployeeI.EMPLOYEE_LAST_NAME;                   
        txtaddress.Text = currEmployeeI.EMPLOYEE_ADDRESS;                   
        txtcity.Text = currEmployeeI.EMPLOYEE_CITY;                         
        txtzip.Text = currEmployeeI.EMPLOYEE_ZIP;                           
        txtpayID.Text = currEmployeeI.EMPLOYEE_EXT_ID;
        txtssn.Text = currEmployeeI.Employee_SSN_Visible;

        try
        {
            if (currEmployeeI.EMPLOYEE_DOB != null)
            {
                DateTime _dob = System.Convert.ToDateTime(currEmployeeI.EMPLOYEE_DOB);
                txtdob.Text = _dob.ToShortDateString();
            }
            else
            {
                txtdob.Text = null;
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors during call.", exception);

            txtdob.Text = null;
        }
        try
        {
            if (currEmployeeI.EMPLOYEE_HIRE_DATE != null)
            {
                DateTime _hdate = System.Convert.ToDateTime(currEmployeeI.EMPLOYEE_HIRE_DATE);
                txthdate.Text = _hdate.ToShortDateString();
            }
            else
            {
                txthdate.Text = null;
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors during call.", exception);

            txthdate.Text = null;
        }

        errorChecking.validateTextBoxNull(txtpayID, false);
        errorChecking.validateTextBoxNull(txtfname, false);
        errorChecking.validateTextBoxNull(txtlname, false);
        errorChecking.validateTextBoxNull(txtaddress, false);
        errorChecking.validateTextBoxNull(txtcity, false);
        errorChecking.validateTextBoxNull(txtzip, false);
        errorChecking.validateTextBoxDate(txtdob, false);
        errorChecking.validateTextBoxDate(txthdate, false);
        errorChecking.validateDropDownSelection(ddlEmployeeType, false);
        errorChecking.validateDropDownSelection(ddlPlans, false);
        errorChecking.validateDropDownSelection(ddlStates, false);
        errorChecking.validateTextBoxSSN(txtssn, false);
        errorChecking.validateDropDownSelection(ddl_acaStatus, false);
        errorChecking.validateDropDownSelection(ddl_classification, false);
        errorChecking.validateDropDownSelection(ddl_hrstatus, false);

        if (Session["showSSN"] != null && (bool)Session["showSSN"])
        {
            txtssn.Text = currEmployeeI.Employee_SSN_Visible;
        }
        else
        {
            txtssn.Text = currEmployeeI.Employee_SSN_Hidden;
        }

        mpe.Show();
    }

    /// <summary>
    /// 160) The purpose of this function is to transfer the Imported Employee Data from the Import Table to the Actual Employee table. 
    /// </summary>
    /// <returns></returns>
    private void transferEmployeeRecord()
    {

        if (GvAlertDetail == null || HfDistrictID == null)
        {
            Log.Error("Aspx component was null< either HfDistrictID:[" + HfDistrictID + "] or GvAlertDetail:[" + GvAlertDetail + "].");

            return;
        }

        Employee newEmployee = null;

        GridViewRow row = GvAlertDetail.SelectedRow;
        bool validData = true;

        if (row == null)
        {
            Log.Warn("Trying to show employee Alert when selected ro is null.");

            return;
        }

        int _employerID = int.Parse(HfDistrictID.Value);

        employer emp = employerController.getEmployer(_employerID);

        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditEmployee");
        DropDownList ddlPlans = (DropDownList)row.FindControl("Ddl_ei_Plan");
        DropDownList ddlStates = (DropDownList)row.FindControl("Ddl_ei_State");
        DropDownList ddlEmployeeType = (DropDownList)row.FindControl("Ddl_ei_EmpType");
        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        TextBox txtfname = (TextBox)row.FindControl("Txt_ei_FirstName");
        TextBox txtlname = (TextBox)row.FindControl("Txt_ei_LastName");
        TextBox txtaddress = (TextBox)row.FindControl("Txt_ei_Address");
        TextBox txtcity = (TextBox)row.FindControl("Txt_ei_City");
        TextBox txtzip = (TextBox)row.FindControl("Txt_ei_Zip");
        TextBox txtssn = (TextBox)row.FindControl("Txt_ei_SSN");
        TextBox txtdob = (TextBox)row.FindControl("Txt_ei_DOB");
        TextBox txthdate = (TextBox)row.FindControl("Txt_ei_Hdate");
        TextBox txtemployeeID = (TextBox)row.FindControl("Txt_ei_EmployeeID");
        TextBox txtExtID = (TextBox)row.FindControl("Txt_ei_PayrollID");
        DropDownList ddl_class = (DropDownList)row.FindControl("Ddl_ei_Classification");
        DropDownList ddl_acaStatus = (DropDownList)row.FindControl("Ddl_ei_ActStatus");
        DropDownList ddl_hrStatus = (DropDownList)row.FindControl("Ddl_ei_HRStatus");
        DropDownList ddl_io_insuranceOffer = (DropDownList)row.FindControl("Ddl_ei_Insurance");
        DropDownList ddl_io_planYear = (DropDownList)row.FindControl("Ddl_ei_PlanYear_Coverage");
        Panel pnl_newHire = (Panel)row.FindControl("PnlNewHireInsurance");
        Literal litMess2 = (Literal)row.FindControl("lit_ei_message");

        int _rowID = int.Parse(hfRowID.Value);

        List<Employee_I> _tempListI = (List<Employee_I>)Session["importEmployeeList"];
        if ((_tempListI) == null)
        {
            return;
        }

        List<Employee> tempList = (List<Employee>)Session["Employees"];
        if (tempList == null)
        {
            return;
        }


        Employee_I currEmployeeI = EmployeeController.findEmployee(_tempListI, _rowID);

        if (Session["showSSN"] != null && false == (bool)Session["showSSN"] && currEmployeeI != null)
        {
            txtssn.Text = currEmployeeI.Employee_SSN_Visible;
        }

        validData = errorChecking.validateTextBoxNull(txtExtID, validData);
        validData = errorChecking.validateTextBoxNull(txtfname, validData);
        validData = errorChecking.validateTextBoxNull(txtlname, validData);
        validData = errorChecking.validateTextBoxNull(txtaddress, validData);
        validData = errorChecking.validateTextBoxNull(txtcity, validData);
        validData = errorChecking.validateTextBoxNull(txtzip, validData);
        validData = errorChecking.validateTextBoxDate(txtdob, validData);
        validData = errorChecking.validateTextBoxDate(txthdate, validData);
        validData = errorChecking.validateDropDownSelection(ddlEmployeeType, validData);
        validData = errorChecking.validateDropDownSelection(ddlPlans, validData);
        validData = errorChecking.validateTextBoxSSN(txtssn, validData);
        validData = errorChecking.validateDropDownSelection(ddl_class, validData);
        validData = errorChecking.validateDropDownSelection(ddl_acaStatus, validData);
        validData = errorChecking.validateDropDownSelection(ddl_hrStatus, validData);

        if (pnl_newHire.Visible == true)
        {
            validData = errorChecking.validateDropDownSelection(ddl_io_insuranceOffer, validData);
            if (ddl_io_insuranceOffer.SelectedItem.Text == "Yes")
            {
                validData = errorChecking.validateDropDownSelection(ddl_io_planYear, validData);

                if (validData == true)
                {
                    validData = errorChecking.compareIntValueDropDownListNotEqual(ddlPlans, ddl_io_planYear, validData);
                    if (validData == false)
                    {
                        litMess2.Text = "The Meas PLAN YEAR must not be the same as the Insurance Offered Plan Year, the Insurance Offered for NEW HIRES, should be from the previous year.";
                    }
                    else
                    {

                        validData = errorChecking.comparePlanYearSelectionOldNew(ddlPlans, ddl_io_planYear, _employerID, validData);
                        if (validData == false)
                        {
                            litMess2.Text = "The Meas Plan Year must be more Recent than the Insurance Offered Plan Year.";
                        }
                        else
                        {
                            litMess2.Text = null;
                        }
                    }
                }
            }
        }

        if (validData == true)
        {
            int _employeeID = 0;
            int _employeeTypeID = 0;
            int _hrStatusID = 0;
            string _fname = null;
            string _mname = null;
            string _lname = null;
            string _address = null;
            string _city = null;
            int _stateID = 0;
            string _zip = null;
            DateTime _hdate;
            DateTime? _cdate = null;
            string _ssn = null;
            string _extID = null;
            DateTime? _tdate = null;
            DateTime _dob;
            DateTime _impEnd;
            bool _imp = false;
            int _planYearID = 0;
            int _planYearID_limbo = 0;
            int _planYearID_meas = 0;
            bool currentEmployee = false;
            DateTime _modOn = DateTime.Now;
            string _modBy = HfUserName.Value;
            int _classID = 0;
            int _acaStatusID = 0;

            bool _offer = false;
            int _offerPlanYear = 0;

            _extID = txtExtID.Text;
            _employeeTypeID = int.Parse(ddlEmployeeType.SelectedItem.Value);
            _planYearID_meas = int.Parse(ddlPlans.SelectedItem.Value);
            _fname = txtfname.Text;
            _lname = txtlname.Text;
            _address = txtaddress.Text;
            _city = txtcity.Text;
            _stateID = 0;
            int.TryParse(ddlStates.SelectedItem.Value, out _stateID); 
            _zip = txtzip.Text;
            _hdate = DateTime.Parse(txthdate.Text);
            _dob = DateTime.Parse(txtdob.Text);
            _ssn = txtssn.Text;
            _classID = int.Parse(ddl_class.SelectedItem.Value);
            _acaStatusID = int.Parse(ddl_acaStatus.SelectedItem.Value);
            _hrStatusID = int.Parse(ddl_hrStatus.SelectedItem.Value);

            if (pnl_newHire.Visible == true)
            {
                _offer = bool.Parse(ddl_io_insuranceOffer.SelectedItem.Value);
                if (_offer == true)
                {
                    _offerPlanYear = int.Parse(ddl_io_planYear.SelectedItem.Value);
                }
            }

            foreach (Employee emp2 in tempList)
            {
                if (_ssn == emp2.Employee_SSN_Visible)
                {
                    _employeeID = emp2.EMPLOYEE_ID;
                    _planYearID = emp2.EMPLOYEE_PLAN_YEAR_ID;
                    _planYearID_limbo = emp2.EMPLOYEE_PLAN_YEAR_ID_LIMBO;
                    _planYearID_meas = emp2.EMPLOYEE_PLAN_YEAR_ID_MEAS;
                    break;
                }
            }

            int _impID = emp.EMPLOYER_INITIAL_MEAS_ID;

            int _months = measurementController.getInitialMeasurementLength(_impID);

            _mname = currEmployeeI.EMPLOYEE_MIDDLE_NAME;
            _hrStatusID = currEmployeeI.EMPLOYEE_HR_STATUS_ID;
            _cdate = currEmployeeI.EMPLOYEE_C_DATE;
            _tdate = currEmployeeI.EMPLOYEE_TERM_DATE;

            _impEnd = EmployeeController.calculateIMPEndDate(_hdate, _months);

            _imp = EmployeeController.calculateIMP(_employerID, _employeeTypeID, _planYearID_meas, _hdate, 1);

            newEmployee = EmployeeController.TransferImportedEmployee(
                    _employeeID,
                    _rowID,
                    _employeeTypeID,
                    _hrStatusID,
                    _employerID,
                    _fname,
                    _mname,
                    _lname,
                    _address,
                    _city,
                    _stateID,
                    _zip,
                    _hdate,
                    _cdate,
                    _ssn,
                    _extID,
                    _tdate,
                    _dob,
                    _impEnd,
                    _planYearID,
                    _planYearID_limbo,
                    _planYearID_meas,
                    _modOn,
                    _modBy,
                    _offer,
                    _offerPlanYear,
                    _classID,
                    _acaStatusID
                );

            if (newEmployee != null)
            {
                _tempListI.RemoveAll(s => s.ROW_ID == currEmployeeI.ROW_ID);

                Session["importEmployeeList"] = _tempListI;

                if (pnl_newHire.Visible == true)
                {
                    Session["insuranceAlertDetails"] = alert_controller.manufactureEmployerInsuranceAlertList(_employerID);
                }

                foreach (Employee ep in tempList)
                {
                    if (ep.Employee_SSN_Visible == newEmployee.Employee_SSN_Visible)
                    {
                        currentEmployee = true;
                        ep.EMPLOYEE_TYPE_ID = newEmployee.EMPLOYEE_TYPE_ID;
                        ep.EMPLOYEE_PLAN_YEAR_ID = newEmployee.EMPLOYEE_PLAN_YEAR_ID;
                        ep.EMPLOYEE_PLAN_YEAR_ID_LIMBO = newEmployee.EMPLOYEE_PLAN_YEAR_ID_LIMBO;
                        ep.EMPLOYEE_PLAN_YEAR_ID_MEAS = newEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS;
                        ep.EMPLOYEE_FIRST_NAME = newEmployee.EMPLOYEE_FIRST_NAME;
                        ep.EMPLOYEE_MIDDLE_NAME = newEmployee.EMPLOYEE_MIDDLE_NAME;
                        ep.EMPLOYEE_LAST_NAME = newEmployee.EMPLOYEE_LAST_NAME;
                        ep.EMPLOYEE_ADDRESS = newEmployee.EMPLOYEE_ADDRESS;
                        ep.EMPLOYEE_CITY = newEmployee.EMPLOYEE_CITY;
                        ep.EMPLOYEE_STATE_ID = newEmployee.EMPLOYEE_STATE_ID;
                        ep.EMPLOYEE_ZIP = newEmployee.EMPLOYEE_ZIP;
                        ep.EMPLOYEE_HIRE_DATE = newEmployee.EMPLOYEE_HIRE_DATE;
                        ep.EMPLOYEE_DOB = newEmployee.EMPLOYEE_DOB;
                        ep.EMPLOYEE_HR_STATUS_ID = newEmployee.EMPLOYEE_HR_STATUS_ID;
                        ep.EMPLOYEE_C_DATE = newEmployee.EMPLOYEE_C_DATE;
                        ep.EMPLOYEE_EXT_ID = newEmployee.EMPLOYEE_EXT_ID;
                        ep.EMPLOYEE_TERM_DATE = newEmployee.EMPLOYEE_TERM_DATE;
                        ep.EMPLOYEE_ACT_STATUS_ID = newEmployee.EMPLOYEE_ACT_STATUS_ID;
                        ep.EMPLOYEE_CLASS_ID = newEmployee.EMPLOYEE_CLASS_ID;
                        break;
                    }
                }

                if (currentEmployee == false)
                {
                    tempList.Add(newEmployee);
                }

                Session["Employees"] = tempList;

                mpe.Hide();
                Session["showSSN"] = false;

                loadAlertTypes(_employerID, true);
                GvAlertDetail.SelectedIndex = -1;
                loadAlertDetails(_employerID);
            }
        }
        else
        {
            if (Session["showSSN"] != null && false == (bool)Session["showSSN"])
            {
                txtssn.Text = currEmployeeI.Employee_SSN_Hidden;
            }
            mpe.Show();
        }


    }

    /// <summary>
    /// 130)
    /// </summary>
    private void deleteEmployeeAlert()
    {
        Log.Info(string.Format("Employee Alert delete called."));

        GridViewRow row = GvAlertDetail.SelectedRow;
        int _employerID = int.Parse(HfDistrictID.Value);
        int _rowID = 0;
        employer emp = employerController.getEmployer(_employerID);
        bool recordRemoved = false;

        Log.Info(string.Format("Employee Alert delete: Employer Id: [{0}], Employer is: [{1}].", _employerID, emp));

        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditEmployee");
        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");

        _rowID = int.Parse(hfRowID.Value);

        Log.Info(string.Format("Employee Alert delete: Row Id: [{0}].", _rowID));

        recordRemoved = EmployeeController.deleteImportedEmployee(_rowID);

        Log.Info(string.Format("Employee Alert delete: Row Id: [{0}] with result [{1}].", _rowID, recordRemoved));

        if (recordRemoved == true)
        {
            List<Employee_I> tempList = (List<Employee_I>)Session["importEmployeeList"];

            foreach (Employee_I ei in tempList)
            {
                if (ei.ROW_ID == _rowID)
                {
                    tempList.Remove(ei);
                    break;
                }
            }

            Session["importEmployeeList"] = tempList;

            loadAlertTypes(_employerID, true);
            loadAlertDetails(_employerID);
        }
    }
    /// <summary>
    /// 140)
    /// </summary>
    private void showInsuranceAlert()
    {
        try
        {
            GridViewRow row = GvAlertDetail.SelectedRow;
            int _employerID = int.Parse(HfDistrictID.Value);
            int _rowID = 0;
            List<alert_insurance> tempInsuranceList = null;
            List<Employee> tempEmployeeList = null;
            alert_insurance tempInsurance = null;
            PlanYear tempPY = null;
            Employee tempEmployee = null;

            if (Session["insuranceAlertDetails"] == null)
            {
                Session["insuranceAlertDetails"] = alert_controller.manufactureEmployerInsuranceAlertList(_employerID);
            }

            if (Session["Employees"] == null)
            {
                Session["Employees"] = EmployeeController.manufactureEmployeeList(_employerID);
            }

            tempInsuranceList = (List<alert_insurance>)Session["insuranceAlertDetails"];
            tempEmployeeList = (List<Employee>)Session["Employees"];

            HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
            ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditInsurance");
            TextBox txtID = (TextBox)row.FindControl("Txt_ins_ExtID");
            DropDownList ddlInsurancePlans = (DropDownList)row.FindControl("Ddl_io_InsurancePlan");
            Literal lit_12_PlanYear = (Literal)row.FindControl("Lit_io_PlanYear");
            Literal lit_12_EmployeeName = (Literal)row.FindControl("Lit_io_EmployeeName");
            Literal lit_12_payrollID = (Literal)row.FindControl("Lit_io_PayrollID");
            Literal lit_12_MonthlyAvg = (Literal)row.FindControl("Lit_io_MonthlyAvg");
            HiddenField hf_12EmployeeID = (HiddenField)row.FindControl("Hf_io_EmployeeID");
            HiddenField hf_12_PlanYearID = (HiddenField)row.FindControl("Hf_io_PlanYearID");

            _rowID = int.Parse(hfRowID.Value);
            tempInsurance = alert_controller.findSingleInsuranceAlert(tempInsuranceList, _rowID);
            Session["EditingInsuranceRecord"] = tempInsurance;

            tempPY = PlanYear_Controller.findPlanYear(tempInsurance.IALERT_PLANYEARID, _employerID);
            tempEmployee = EmployeeController.findEmployee(tempEmployeeList, tempInsurance.IALERT_EMPLOYEEID);

            lit_12_PlanYear.Text = tempPY.PLAN_YEAR_DESCRIPTION;
            lit_12_EmployeeName.Text = tempEmployee.EMPLOYEE_FIRST_NAME + " " + tempEmployee.EMPLOYEE_LAST_NAME;
            lit_12_payrollID.Text = tempEmployee.EMPLOYEE_EXT_ID;
            lit_12_MonthlyAvg.Text = "0.0";
            hf_12EmployeeID.Value = tempEmployee.EMPLOYEE_ID.ToString();
            hf_12_PlanYearID.Value = tempPY.PLAN_YEAR_ID.ToString();

            Measurement measurement = measurementController.getPlanYearMeasurement(_employerID, tempPY.PLAN_YEAR_ID, tempEmployee.EMPLOYEE_TYPE_ID);
            if (measurement != null)
            {
                AverageHours average = (from hours in AverageHoursFactory.GetAllAverageHoursForEmployeeId(tempEmployee.EMPLOYEE_ID)
                                        where hours.IsNewHire == false && hours.MeasurementId == measurement.MEASUREMENT_ID
                                        select hours).FirstOrDefault();

                if (average != null)
                {
                    lit_12_MonthlyAvg.Text = average.MonthlyAverageHours.ToString("0.##");
                }
            }

            ddlInsurancePlans.DataSource = insuranceController.manufactureInsuranceList(tempPY.PLAN_YEAR_ID);
            ddlInsurancePlans.DataTextField = "INSURANCE_NAME";
            ddlInsurancePlans.DataValueField = "INSURANCE_ID";
            ddlInsurancePlans.DataBind();
            ddlInsurancePlans.Items.Add("Select");
            ddlInsurancePlans.SelectedIndex = ddlInsurancePlans.Items.Count - 1;

            mpe.Show();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

    }
    private void completeInsuranceOffer()
    {
        int offerID = 100;
        int _rowID = 0;
        int _planYearID = 0;
        int _insuranceID = 0;
        int _contributionID = 0;
        int _employeeID = 0;

        double? _avgHours = null;
        bool? _offered = null;
        bool? _accepted = null;
        DateTime _modOn = DateTime.Now;
        DateTime? _effectiveDate = null;
        DateTime? _hireDate = null;
        double _hraFlex = 0;
        string _modBy = HfUserName.Value;
        string _notes = null;
        string _history = null;
        bool validData = true;
        bool validTransaction = false;
        alert_insurance ai = null;

        GridViewRow row = GvAlertDetail.SelectedRow;

        int _employerID = int.Parse(HfDistrictID.Value);

        employer currEmployer = (employer)Session["CurrentDistrict"];
        Employee currEmployee = null;
        PlanYear currPlanYear = null;
        EmployeeType currEmpType = null;
        Measurement currMeas = null;

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditInsurance");
        DropDownList ddlOffer = (DropDownList)row.FindControl("Ddl_io_Offered");
        DropDownList ddlAccepted = (DropDownList)row.FindControl("Ddl_io_Accepted");
        DropDownList ddlInsPlan = (DropDownList)row.FindControl("Ddl_io_InsurancePlan");
        DropDownList ddlContribution = (DropDownList)row.FindControl("Ddl_io_Contribution");
        TextBox txtioComments = (TextBox)row.FindControl("Txt_io_Comments");
        TextBox txtioHraFlex = (TextBox)row.FindControl("Txt_io_HraFlex");
        TextBox txtioEffectiveDate = (TextBox)row.FindControl("Txt_io_InsuranceEffectiveDate");
        Label lblioMessage = (Label)row.FindControl("LblInsuranceMessage");

        validData = errorChecking.validateDropDownSelection(ddlOffer, validData);

        if (validData == true)
        {
            _rowID = int.Parse(hfRowID.Value);
            offerID = int.Parse(ddlOffer.SelectedItem.Value);
            List<alert_insurance> tempList = new List<alert_insurance>();

            if (Session["insuranceAlertDetails"] == null)
            {
                tempList = alert_controller.manufactureEmployerInsuranceAlertList(_employerID);
            }
            else
            {
                tempList = (List<alert_insurance>)Session["insuranceAlertDetails"];
            }

            ai = alert_controller.findSingleInsuranceAlert(tempList, _rowID);

            _employeeID = ai.IALERT_EMPLOYEEID;
            _planYearID = ai.IALERT_PLANYEARID;

            currEmployee = EmployeeController.findSingleEmployee(_employeeID);
            currPlanYear = PlanYear_Controller.findPlanYear(_planYearID, _employeeID);

            currMeas = measurementController.getPlanYearMeasurement(_employerID, _planYearID, currEmployee.EMPLOYEE_TYPE_ID);
            Measurement Meas = measurementController.getPlanYearMeasurement(_employerID, (int)ai.IALERT_ADMIN_PLAN_YEARID, currEmployee.EMPLOYEE_TYPE_ID);

            if (Meas != null)
            {
                AverageHours average = (from hours in AverageHoursFactory.GetAllAverageHoursForEmployeeId(currEmployee.EMPLOYEE_ID)
                                        where hours.IsNewHire == false && hours.MeasurementId == Meas.MEASUREMENT_ID
                                        select hours).FirstOrDefault();

                if (average != null)
                {
                    ai.EMPLOYEE_AVG_HOURS = average.MonthlyAverageHours;
                }
            }

            if (offerID == 1)
            {
                _offered = true;
                _insuranceID = int.Parse(ddlInsPlan.SelectedItem.Value);

                validData = errorChecking.validateTextBoxDate(txtioEffectiveDate, validData);
                validData = errorChecking.validateDropDownSelection(ddlOffer, validData);
                validData = errorChecking.validateDropDownSelection(ddlAccepted, validData);
                validData = errorChecking.validateDropDownSelection(ddlInsPlan, validData);
                validData = errorChecking.validateDropDownSelection(ddlContribution, validData);
                validData = errorChecking.validateTextBoxDecimal(txtioHraFlex, validData);

                if (validData == true)
                {
                    _hireDate = currEmployee.EMPLOYEE_HIRE_DATE;
                    _effectiveDate = DateTime.Parse(txtioEffectiveDate.Text);
                    _accepted = Boolean.Parse(ddlAccepted.SelectedItem.Value);
                    _effectiveDate = DateTime.Parse(txtioEffectiveDate.Text);
                    _hraFlex = double.Parse(txtioHraFlex.Text);

                    validData = errorChecking.validateInsuranceOfferDates((DateTime)_effectiveDate, currMeas, (DateTime)_effectiveDate, (DateTime)_effectiveDate, validData, lblioMessage, (DateTime)_hireDate, txtioEffectiveDate, txtioEffectiveDate, txtioEffectiveDate);

                    if (validData == true)
                    {
                        _contributionID = int.Parse(ddlContribution.SelectedItem.Value);
                        _avgHours = ai.EMPLOYEE_AVG_HOURS;
                        _notes = txtioComments.Text;
                        _history = ai.IALERT_HISTORY;
                        _history += Environment.NewLine;
                        _history += "Record modified by " + _modBy + " on " + _modOn.ToString() + Environment.NewLine;
                        _history += "Insurance ID:" + _insuranceID.ToString() + Environment.NewLine;
                        _history += "Contribution ID:" + _contributionID.ToString() + Environment.NewLine;
                        _history += "Offered:" + _offered.ToString() + Environment.NewLine;
                        _history += "Accepted:" + _accepted.ToString() + Environment.NewLine;
                        _history += "Effective Date:" + _effectiveDate.ToString() + Environment.NewLine;
                        _history += "HRA - Flex Contribution: " + _hraFlex.ToString() + Environment.NewLine;

                        validTransaction = insuranceController.updateInsuranceOffer(_rowID, _insuranceID, _contributionID, _avgHours, _offered, _effectiveDate, _accepted, _effectiveDate, _modOn, _modBy, _notes, _history, _effectiveDate, _hraFlex);

                        if (validTransaction == true)
                        {
                            tempList.Remove(ai);
                            Session["insuranceAlertDetails"] = tempList;

                            loadAlertTypes(currEmployer.EMPLOYER_ID, true);
                            loadAlertDetails(currEmployer.EMPLOYER_ID);
                        }
                        else
                        {
                            lblioMessage.Text = "An error occurred while trying to SAVE the data. Please try again. If problem persists, please contact " + Branding.CompanyShortName;
                            mpe.Show();
                        }

                    }
                    else
                    {
                        mpe.Show();
                    }
                }
                else
                {
                    lblioMessage.Text = "Please correct all RED fields.";
                    mpe.Show();
                }
            }
            else
            {
                _hireDate = currEmployee.EMPLOYEE_HIRE_DATE;
                _effectiveDate = DateTime.Parse(txtioEffectiveDate.Text);
                _effectiveDate = DateTime.Parse(txtioEffectiveDate.Text);

                validData = errorChecking.validateTextBoxDate(txtioEffectiveDate, validData);
                validData = errorChecking.validateInsuranceOfferDates((DateTime)_effectiveDate, currMeas, (DateTime)_effectiveDate, (DateTime)_effectiveDate, validData, lblioMessage, (DateTime)_hireDate, txtioEffectiveDate, txtioEffectiveDate, txtioEffectiveDate);

                if (validData == true)
                {
                    _offered = false;
                    _insuranceID = 0;
                    _contributionID = 0;
                    _avgHours = ai.EMPLOYEE_AVG_HOURS;
                    _accepted = null;
                    _hraFlex = 0;
                    _notes = txtioComments.Text;
                    _history = ai.IALERT_HISTORY;
                    _history += Environment.NewLine;
                    _history += "Record modified by " + _modBy + " on " + _modOn.ToString() + Environment.NewLine;
                    _history += "Insurance was NOT offered." + Environment.NewLine;
                    validTransaction = insuranceController.updateInsuranceOffer(_rowID, _insuranceID, _contributionID, _avgHours, _offered, _effectiveDate, _accepted, _effectiveDate, _modOn, _modBy, _notes, _history, _effectiveDate, _hraFlex);

                    if (validTransaction == true)
                    {
                        tempList.Remove(ai);
                        Session["insuranceAlertDetails"] = tempList;

                        loadAlertTypes(currEmployer.EMPLOYER_ID, true);
                        loadAlertDetails(currEmployer.EMPLOYER_ID);
                    }
                    else
                    {
                        lblioMessage.Text = "An error occurred while trying to SAVE the data. Please try again. If problem persists, please contact " + Branding.CompanyShortName;
                        mpe.Show();
                    }
                }
                else
                {
                    lblioMessage.Text = "Please correct any fields highlighted in red or enter atleast 15 characters into the Explanation/Notes box and enter the effective date. If this employee is NOT a new hire, than the recommended date is: " + currMeas.MEASUREMENT_STAB_START.ToShortDateString();
                    mpe.Show();
                }
            }
        }
        else
        {
            mpe.Show();
            lblioMessage.Text = "Please correct all fields highlighted in RED.";
        }
    }
    private void showInsuranceCarrierAlert()
    {
        GridViewRow row = GvAlertDetail.SelectedRow;
        int _employerID = int.Parse(HfDistrictID.Value);
        List<insurance_coverage_I> iciList = (List<insurance_coverage_I>)Session["carrierImport"];

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("MpeEditCarrier");
        HiddenField HfEmployeeID = (HiddenField)row.FindControl("Hf_IC_EmployeeID");
        TextBox TxtDependentID = (TextBox)row.FindControl("Txt_IC_DependentID");
        TextBox TxtTaxYear = (TextBox)row.FindControl("Txt_IC_TaxYear");
        TextBox TxtFName = (TextBox)row.FindControl("Txt_IC_FirstName");
        TextBox TxtLName = (TextBox)row.FindControl("Txt_IC_LastName");
        TextBox TxtSSN = (TextBox)row.FindControl("Txt_IC_SSN");
        TextBox TxtDOB = (TextBox)row.FindControl("Txt_IC_DOB");

        int _rowID = int.Parse(hfRowID.Value);

        foreach (insurance_coverage_I ici in iciList)
        {
            if (ici.ROW_ID == _rowID)
            {
                HfEmployeeID.Value = ici.IC_EMPLOYEE_ID.ToString();
                TxtDependentID.Text = ici.IC_DEPENDENT_ID.ToString();
                TxtTaxYear.Text = ici.IC_TAX_YEAR.ToString();
                TxtFName.Text = ici.IC_FIRST_NAME;
                TxtLName.Text = ici.IC_LAST_NAME;

                if (Session["showSSN"] != null && (bool)Session["showSSN"])
                {
                    TxtSSN.Text = ici.IC_SSN;
                }
                else
                {
                    TxtSSN.Text = ici.IC_SSN_MASKED;
                }

                if (ici.IC_DOB != null)
                {
                    TxtDOB.Text = ((DateTime)ici.IC_DOB).ToShortDateString();
                }

                break;
            }
        }

        mpe.Show();
    }

    /// <summary>
    /// This will updated an Insurance Carrier Alert and clear it out if possible. 
    /// </summary>
    private void updateInsuranceCarrierAlert()
    {
        int _rowID = 0;
        int _employerID = 0;
        int _employeeID = 0;
        int _dependentID = 0;
        string _fName = null;
        string _lName = null;
        string _ssn = null;
        DateTime? _dob = null;
        bool validData = true;
        bool validEmployeeID = true;
        bool generateEmployeeAlerts = false;
        string _modBy = HfUserName.Value;
        DateTime _modOn = DateTime.Now;
        int rowCount = GvAlertDetail.Rows.Count;
        string modBy = HfUserName.Value;


        GridViewRow row = GvAlertDetail.SelectedRow;
        int selectedIndex = GvAlertDetail.SelectedIndex;
        _employerID = int.Parse(HfDistrictID.Value);

        List<insurance_coverage_I> iciList = (List<insurance_coverage_I>)Session["carrierImport"];

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("MpeEditCarrier");
        DropDownList DdlEmployeeID = (DropDownList)row.FindControl("Ddl_IC_Employees");
        TextBox TxtDependentID = (TextBox)row.FindControl("Txt_IC_DependentID");
        TextBox TxtTaxYear = (TextBox)row.FindControl("Txt_IC_TaxYear");
        TextBox TxtFName = (TextBox)row.FindControl("Txt_IC_FirstName");
        TextBox TxtLName = (TextBox)row.FindControl("Txt_IC_LastName");
        TextBox TxtSSN = (TextBox)row.FindControl("Txt_IC_SSN");
        TextBox TxtDOB = (TextBox)row.FindControl("Txt_IC_DOB");
        RadioButton rbEmp = (RadioButton)row.FindControl("Rb_IC_employee");
        RadioButton rbDep = (RadioButton)row.FindControl("Rb_IC_dependent");
        CheckBox cbGenAlerts = (CheckBox)row.FindControl("CbEmployeeAlerts");
        Literal LitMessage = (Literal)row.FindControl("Lit_IC_Message");

        validData = errorChecking.validateTextBoxNull(TxtFName, validData);
        validData = errorChecking.validateTextBoxNull(TxtLName, validData);
        validData = errorChecking.validateDropDownSelection(DdlEmployeeID, validData);

        if (validData == true)
        {
            _employeeID = int.Parse(DdlEmployeeID.SelectedItem.Value);
        }

        _rowID = int.Parse(hfRowID.Value);

        int.TryParse(TxtDependentID.Text, out _dependentID);

        string ssnMasked = null;

        if (false == TxtSSN.Text.IsValidSsn() || (Session["showSSN"] != null && false == (bool)Session["showSSN"]))
        {
            foreach (insurance_coverage_I ici in iciList)
            {
                if (ici.ROW_ID == _rowID && ici.IC_SSN_MASKED.Equals(TxtSSN.Text))
                {
                    TxtSSN.Text = ici.IC_SSN;
                    ssnMasked = ici.IC_SSN_MASKED;
                    break;
                }
            }
        }

        if (rbEmp.Checked == true)
        {
            validData = errorChecking.validateTextBoxSSN(TxtSSN, validData);
            if (validData == true) { _ssn = TxtSSN.Text.Trim(' '); }
        }

        if (rbDep.Checked == true)
        {
            bool validSSN = true;
            bool validDOB = true;

            validDOB = errorChecking.validateTextBoxDate(TxtDOB, validDOB);
            validSSN = errorChecking.validateTextBoxSSN(TxtSSN, validSSN);

            if (validDOB == false && validSSN == false) { validData = false; }
            else
            {
                if (validDOB == true) { _dob = DateTime.Parse(TxtDOB.Text); }
                if (validSSN == true) { _ssn = TxtSSN.Text.Trim(' '); }
                TxtDOB.BackColor = System.Drawing.Color.White;
                TxtSSN.BackColor = System.Drawing.Color.White;
            }
        }

        if (rbDep.Checked == false && rbEmp.Checked == false)
        {
            validData = false;
            rbEmp.BackColor = System.Drawing.Color.Red;
            rbDep.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            rbEmp.BackColor = System.Drawing.Color.White;
            rbDep.BackColor = System.Drawing.Color.White;
        }


        if (validData == true)
        {
            _fName = TxtFName.Text.Trim(' ');
            _lName = TxtLName.Text.Trim(' ');

            bool validTransaction = false;
            if (rbEmp.Checked == true)
            {
                validTransaction = insuranceController.updateInsuranceCoverageAlert(_rowID, _employeeID, _dependentID, _ssn, _dob, _fName, _lName, rbEmp.Checked);
            }
            else
            {
                Dependent myDependent = EmployeeController.updateEmployeeDependent(0, _employeeID, _fName, string.Empty, _lName, _ssn, _dob, modBy, 1);
                if (myDependent != null)
                {
                    validTransaction = insuranceController.updateInsuranceCoverageAlert(_rowID, _employeeID, myDependent.DEPENDENT_ID, _ssn, _dob, _fName, _lName, rbEmp.Checked);
                }
                else { validTransaction = false; }
            }

            if (validTransaction == true)
            {
                insuranceController.transferInsuranceCarrierImportData(_employerID, _modBy);

                GvAlertDetail.SelectedIndex = -1;
                Session["showSSN"] = false;
                Session["carrierImport"] = null;
                Session["importEmployeeList"] = null;
                loadAlertTypes(_employerID, true);
                loadAlertDetails(_employerID);
                mpe.Hide();
            }
            else
            {
                if (ssnMasked != null)
                {
                    TxtSSN.Text = ssnMasked;
                }

                LitMessage.Text = "The system was not able to update your record. The transaction failed.";
                mpe.Show();
            }
        }
        else
        {
            if (ssnMasked != null)
            {
                TxtSSN.Text = ssnMasked;
            }
            LitMessage.Text = "Please correct any field highlighted in red.";
            mpe.Show();
        }
    }

    /// <summary>
    /// This will allow the User to DELETE an Insurance Carrier Import alert. 
    /// </summary>
    private void deleteInsuranceCarrierImport()
    {
        Log.Info(string.Format("Insurance Carrier Alert deletecalled."));

        int _rowID = 0;
        int _employerID = 0;
        string _modBy = HfUserName.Value;
        DateTime _modOn = DateTime.Now;

        GridViewRow row = GvAlertDetail.SelectedRow;

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("MpeEditCarrier");

        _rowID = int.Parse(hfRowID.Value);

        Log.Info(string.Format("Insurance Carrier Alert delete: Row Id: [{0}].", _rowID));

        bool validTransaction = insuranceController.deleteInsuranceCoverageAlert(_rowID, _modBy, _modOn);

        Log.Info(string.Format("Insurance Carrier Alert delete: Row Id: [{0}] with result [{1}].", _rowID, validTransaction));

        if (validTransaction == true)
        {
            _employerID = int.Parse(HfDistrictID.Value);
            insuranceController.ValidateCurrentEmployee(_employerID);
            insuranceController.validateCurrentEmployeeDependents(_employerID, _modBy);
            insuranceController.transferInsuranceCarrierImportData(_employerID, _modBy);
            Session["carrierImport"] = null;
            loadAlertTypes(_employerID, true);
            loadAlertDetails(_employerID);
        }
        else
        {
            mpe.Show();
        }
    }
    protected void Ddl_io_Offered_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = GvAlertDetail.SelectedRow;

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditInsurance");
        DropDownList ddl = (DropDownList)row.FindControl("Ddl_io_Offered");
        Panel accepted = (Panel)row.FindControl("Pnl_io_Accepted");
        Panel insurance = (Panel)row.FindControl("Pnl_io_Plan");
        Panel insuranceOff = (Panel)row.FindControl("Pnl_io_DateOffered");
        Panel insuranceEff = (Panel)row.FindControl("Pnl_io_Effective");
        Label lblioMessage = (Label)row.FindControl("LblInsuranceMessage");
        Panel insurancePlan = (Panel)row.FindControl("PnlInsurancePlanOffered");
        Literal litPlanYearID = (Literal)row.FindControl("Lit_io_PlanYear");
        TextBox txtEffectiveDate = (TextBox)row.FindControl("Txt_io_InsuranceEffectiveDate");
        HiddenField hfEmployeeID = (HiddenField)row.FindControl("Hf_io_EmployeeID");
        HiddenField hfPlanYearID = (HiddenField)row.FindControl("Hf_io_PlanYearID");

        bool validData = true;
        int planYearID = 0;
        int employerID = 0;
        int offered = 0;
        int employeeID = 0;

        validData = errorChecking.validateDropDownSelection(ddl, validData);

        if (validData == true)
        {
            int.TryParse(hfPlanYearID.Value, out planYearID);
            int.TryParse(HfDistrictID.Value, out employerID);
            int.TryParse(hfEmployeeID.Value, out employeeID);
            int.TryParse(ddl.SelectedItem.Value, out offered);

            if (Session["Employees"] == null)
            {
                Session["Employees"] = EmployeeController.manufactureEmployeeList(employerID);
            }

            List<Employee> currEmployees = (List<Employee>)Session["Employees"];

            PlanYear currPlanYear = PlanYear_Controller.findPlanYear(planYearID, employerID);
            Employee currEmployee = EmployeeController.findEmployee(currEmployees, employeeID);

            if (currPlanYear != null && currEmployee != null)
            {
                if (currPlanYear.Default_Stability_Start > currEmployee.EMPLOYEE_HIRE_DATE) { txtEffectiveDate.Text = currPlanYear.Default_Stability_Start.ToShortDate(); }
                else { txtEffectiveDate.Text = currEmployee.EMPLOYEE_HIRE_DATE.ToShortDate(); }
            }

            switch (offered)
            {
                case 0:    
                    accepted.Visible = false;
                    insurance.Visible = true;
                    insuranceOff.Visible = false;
                    insuranceEff.Visible = false;
                    insurancePlan.Visible = false;
                    lblioMessage.Text = "Please enter why this Employee was NOT made an offer of insurance. (15 character min)";
                    break;
                case 1:   
                    accepted.Visible = true;
                    insurance.Visible = false;
                    insuranceOff.Visible = true;
                    insuranceEff.Visible = false;
                    insurancePlan.Visible = false;
                    lblioMessage.Text = null;
                    break;
                case 2:   
                    accepted.Visible = false;
                    insurance.Visible = false;
                    insuranceOff.Visible = false;
                    insuranceEff.Visible = false;
                    lblioMessage.Text = "Please enter why this Employee was NOT Eligible. (15 character min)";
                    break;
                default:
                    break;
            }


            mpe.Show();
        }
        else
        {
            mpe.Show();
        }
    }


    protected void Ddl_io_Accepted_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = GvAlertDetail.SelectedRow;

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditInsurance");
        DropDownList ddl = (DropDownList)row.FindControl("Ddl_io_Accepted");
        Panel accepted = (Panel)row.FindControl("Pnl_io_Accepted");
        Panel insurance = (Panel)row.FindControl("Pnl_io_Plan");
        Panel insuranceEff = (Panel)row.FindControl("Pnl_io_Effective");
        Panel insurancePlan = (Panel)row.FindControl("PnlInsurancePlanOffered");
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(ddl, validData);

        if (validData == true)
        {
            bool _accepted = Boolean.Parse(ddl.SelectedItem.Value);

            switch (_accepted)
            {
                case false:      
                    insurance.Visible = true;
                    insuranceEff.Visible = true;
                    insurancePlan.Visible = true;
                    break;
                case true:     
                    insurance.Visible = true;
                    insuranceEff.Visible = true;
                    insurancePlan.Visible = true;
                    break;
                default:
                    break;
            }

            mpe.Show();
        }
        else
        {
            mpe.Show();
        }
    }

    protected void Ddl_io_InsurancePlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = GvAlertDetail.SelectedRow;

        HiddenField hfRowID = (HiddenField)row.FindControl("HfRowID");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditInsurance");
        DropDownList ddl = (DropDownList)row.FindControl("Ddl_io_InsurancePlan");
        DropDownList ddl2 = (DropDownList)row.FindControl("Ddl_io_Contribution");
        bool validData = true;
        int _insuranceID = 0;

        validData = errorChecking.validateDropDownSelection(ddl, validData);

        if (validData == true)
        {
            _insuranceID = int.Parse(ddl.SelectedItem.Value);

            ddl2.DataSource = insuranceController.manufactureInsuranceContributionList(_insuranceID);
            ddl2.DataTextField = "INS_DESC";
            ddl2.DataValueField = "INS_CONT_ID";
            ddl2.DataBind();

            ddl2.Items.Add("Select");
            ddl2.SelectedIndex = ddl2.Items.Count - 1;

            mpe.Show();

        }
        else
        {
            mpe.Show();
        }
    }
    protected void BtnUpdateBillContact_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int _userID = 0;
        int _employerID = int.Parse(HfDistrictID.Value);
        string _modBy = HfUserName.Value;
        DateTime _modOn = DateTime.Now;
        User su = null;
        bool validTransaction = false;

        validData = errorChecking.validateDropDownSelection(Ddl_bill_Users, validData);

        if (validData == true)
        {
            _userID = int.Parse(Ddl_bill_Users.SelectedItem.Value);
            su = UserController.findUser(_userID, _employerID);

            validTransaction = UserController.updateUser(_userID, su.User_First_Name, su.User_Last_Name, su.User_Email, su.User_Phone, su.User_Power, _modBy, _modOn, true, su.User_IRS_CONTACT);
            if (validTransaction == true)
            {
                Pnl_BillWarning.Visible = false;
                loadAlertTypes(_employerID, true);
                loadAlertDetails(_employerID);
            }
            else
            {

            }
        }
        else
        {
        }
    }
    protected void BtnUpdateIrsContact_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int _userID = 0;
        int _employerID = int.Parse(HfDistrictID.Value);
        string _modBy = HfUserName.Value;
        DateTime _modOn = DateTime.Now;
        User su = null;
        bool validTransaction = false;

        validData = errorChecking.validateDropDownSelection(Ddl_irs_Users, validData);

        if (validData == true)
        {
            _userID = int.Parse(Ddl_irs_Users.SelectedItem.Value);
            su = UserController.findUser(_userID, _employerID);

            validTransaction = UserController.updateUser(_userID, su.User_First_Name, su.User_Last_Name, su.User_Email, su.User_Phone, su.User_Power, _modBy, _modOn, su.User_Billing, true);
            if (validTransaction == true)
            {
                Pnl_IRSWarning.Visible = false;
                loadAlertTypes(_employerID, true);
                loadAlertDetails(_employerID);
            }
            else
            {

            }
        }
        else
        {
        }
    }
    private void loadErroredInsurancePlans()
    {
        int _employerID = int.Parse(HfDistrictID.Value);

        List<PlanYear> pyList = PlanYear_Controller.getEmployerPlanYear(_employerID);
        List<insurance> insList = insuranceController.getAllActiveInsurancePlans(_employerID, true);

        GvInsuranceAlert.DataSource = insList;
        GvInsuranceAlert.DataBind();
    }

    protected void GvInsuranceAlert_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int rowIndex = GvInsuranceAlert.SelectedIndex;
        GridViewRow row = GvInsuranceAlert.Rows[rowIndex];
        bool validData = true;
        DateTime _modOn = DateTime.Now;
        string _modBy = HfUserName.Value;
        int _insuranceTypeID = 0;
        int _insuranceID = 0;
        int _employerID = 0;
        bool validTransaction = false;
        string history = null;

        DropDownList ddl = (DropDownList)row.FindControl("Ddl_gv_InsuranceType");
        HiddenField hf = (HiddenField)row.FindControl("HfInsuranceID");
        Label lbl = (Label)row.FindControl("LblInsTypeMessage");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditInsType");

        validData = errorChecking.validateDropDownSelection(ddl, validData);

        if (validData == true)
        {
            try
            {

                _employerID = int.Parse(HfDistrictID.Value);
                _insuranceID = int.Parse(hf.Value);
                _insuranceTypeID = int.Parse(ddl.SelectedItem.Value);
                List<insurance> tempList = insuranceController.getAllActiveInsurancePlans(_employerID, true);
                insurance ins = insuranceController.getSingleInsurancePlan(_insuranceID, tempList);
                history = ins.INSURANCE_HISTORY;

                history += "Insurance Type Alert corrected by " + _modBy + " on " + _modOn.ToString() + System.Environment.NewLine;
                history += "Insurance Type: " + ddl.SelectedItem.Text + System.Environment.NewLine;

                validTransaction = insuranceController.updateInsurancePlan(ins.INSURANCE_ID, ins.INSURANCE_PLAN_YEAR_ID, ins.INSURANCE_NAME, ins.INSURANCE_COST, ins.INSURANCE_MIN_VALUE, ins.INSURANCE_OFF_SPOUSE, ins.SpouseConditional, ins.INSURANCE_OFF_DEPENDENTS, _modBy, _modOn, history, _insuranceTypeID, ins.INSURANCE_MEC, ins.INSURANCE_FULLY_AND_SELF);

                if (validTransaction == true)
                {
                    loadAlertTypes(_employerID, true);
                    loadErroredInsurancePlans();
                }
                else
                {
                    mpe.Show();
                    lbl.Text = "An ERROR occurred while updating the medical plan, please contact for assistance.";
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                mpe.Show();
                lbl.Text = "An ERROR occurred while updating the medical plan, please contact " + Branding.CompanyShortName + " if you need assistance.";
            }
        }
        else
        {
            mpe.Show();
            lbl.Text = "An ERROR occurred while updating the medical plan, please contact " + Branding.CompanyShortName + " if you need assistance.";
        }
    }

    protected void GvInsuranceAlert_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        GvInsuranceAlert.PageIndex = e.NewPageIndex;
        loadErroredInsurancePlans();
    }

    /// <summary>
    /// 30)
    /// </summary>
    private void loadDistrictProfile()
    {
        employer currDist = (employer)Session["CurrentDistrict"];

    }

    /// <summary>
    /// 60)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    /// <summary>
    /// 70) This will take users to the Administrative Side of the website.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnAdminPortal_Click(object sender, EventArgs e)
    {
        Response.Redirect("/admin/Default.aspx", false);
    }

    protected void BtnAdminBilling_Click(object sender, EventArgs e)
    {
        Response.Redirect("/efs_module/default.aspx", false);
    }

    protected void GvInsuranceAlert_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        int rowIndex = e.NewSelectedIndex;
        GridViewRow row = GvInsuranceAlert.Rows[rowIndex];

        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("mpeEditInsType");
        DropDownList ddl = (DropDownList)row.FindControl("Ddl_gv_InsuranceType");

        ddl.DataSource = insuranceController.GetInsuranceTypes(true);
        ddl.DataTextField = "INSURANCE_TYPE_NAME";
        ddl.DataValueField = "INSURANCE_TYPE_ID";
        ddl.DataBind();
        ddl.Items.Add("Select");
        ddl.SelectedIndex = ddl.Items.Count - 1;

        mpe.Show();

    }

    protected void ImgBtnEmployeeSearch_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = GvAlertDetail.SelectedRow;
        load_IC_employees(row, true);
    }

    private void load_IC_employees(GridViewRow row, bool search)
    {
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("MpeEditCarrier");
        DropDownList DdlEmployees = (DropDownList)row.FindControl("Ddl_IC_Employees");
        TextBox txtSearchText = (TextBox)row.FindControl("Txt_IC_EmployeeFilterSelection");
        HiddenField hfEmployeeID = (HiddenField)row.FindControl("Hf_IC_EmployeeID");

        List<Employee> tempList = new List<Employee>();
        List<Employee> filteredList = new List<Employee>();
        string searchText = null;
        bool validData = true;
        int employeeID = 0;

        int.TryParse(hfEmployeeID.Value, out employeeID);

        if (Session["Employees"] == null)
        {
            int _employerID = int.Parse(HfDistrictID.Value);
            tempList = EmployeeController.manufactureEmployeeList(_employerID);
            Session["Employees"] = tempList;
        }
        else
        {
            tempList = (List<Employee>)Session["Employees"];
        }


        if (employeeID > 0 && search == false)
        {
            Employee currEmployee = EmployeeController.findSingleEmployee(employeeID);
            filteredList.Add(currEmployee);
        }
        else
        {
            validData = errorChecking.validateTextBoxLength(txtSearchText, validData, 1);

            if (validData == true)
            {
                searchText = txtSearchText.Text;

                foreach (Employee emp in tempList)
                {
                    if (emp.EMPLOYEE_LAST_NAME.ToLower().Contains(searchText.ToLower()))
                    {
                        filteredList.Add(emp);
                    }
                }
            }
        }

        DdlEmployees.DataSource = filteredList;
        DdlEmployees.DataTextField = "EMPLOYEE_FULL_NAME_SSN";
        DdlEmployees.DataValueField = "EMPLOYEE_ID";
        DdlEmployees.DataBind();

        DdlEmployees.Items.Add("Select");
        DdlEmployees.SelectedIndex = DdlEmployees.Items.Count - 1;
        mpe.Show();
    }

    protected void Btn_IC_GenerateIA_Click(object sender, EventArgs e)
    {
        string _modBy = HfUserName.Value;
        DateTime _modOn = DateTime.Now;
        int _employerID = int.Parse(HfDistrictID.Value);

        insuranceController.createAlertsForMissingEmployees(_employerID, _modOn, _modBy);
        Session["showSSN"] = false;
        Session["carrierImport"] = null;
        Session["importEmployeeList"] = null;
        Session["districtAlerts"] = null;

        loadAlertTypes(_employerID, true);
        loadAlertDetails(_employerID);
    }

}
