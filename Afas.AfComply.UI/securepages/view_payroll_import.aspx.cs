using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

public partial class securedpages_view_payroll_import : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(securedpages_view_payroll_import));

    //******************* START Page Load Events ********************************
    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        HfUserName.Value = user.User_Full_Name;

        HfDistrictID.Value = user.User_District_ID.ToString();

        loadPayRollData();
        loadGrossPayDescriptions();
    }

    //******************* END Page Load Events ********************************
    //******************* START Data Loading Function ********************************
    private void loadEmployees()
    {
        if (Session["Employees"] == null)
        {
            int _employerID = int.Parse(HfDistrictID.Value);
            Session["Employees"] = EmployeeController.manufactureEmployeeList(_employerID);
        }
    }

    private void loadGrossPayDescriptions()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        Ddl_f_PayDesc.DataSource = gpType_Controller.getEmployeeTypes(_employerID);
        Ddl_f_PayDesc.DataTextField = "GROSS_PAY_DESCRIPTION";
        Ddl_f_PayDesc.DataValueField = "GROSS_PAY_EXTERNAL_ID";
        Ddl_f_PayDesc.DataBind();

        Ddl_f_PayDesc.Items.Add("Select");
        Ddl_f_PayDesc.SelectedIndex = Ddl_f_PayDesc.Items.Count - 1;

        Ddl_u_PayDesc.DataSource = gpType_Controller.getEmployeeTypes(_employerID);
        Ddl_u_PayDesc.DataTextField = "GROSS_PAY_DESCRIPTION";
        Ddl_u_PayDesc.DataValueField = "GROSS_PAY_ID";
        Ddl_u_PayDesc.DataBind();

        Ddl_u_PayDesc.Items.Add("Select");
        Ddl_u_PayDesc.SelectedIndex = Ddl_u_PayDesc.Items.Count - 1;
    }


    private void loadPayRollData()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<Payroll_I> fullList = new List<Payroll_I>();
        List<Payroll_I> filteredList1 = new List<Payroll_I>();
        List<Payroll_I> filteredList2 = new List<Payroll_I>();
        List<Payroll_I> filteredList3 = new List<Payroll_I>();
        List<Payroll_I> filteredList4 = new List<Payroll_I>();
        List<Payroll_I> filteredList5 = new List<Payroll_I>();
        List<Payroll_I> filteredList6 = new List<Payroll_I>();
        List<Payroll_I> filteredList7 = new List<Payroll_I>();
        List<Payroll_I> filteredList8 = new List<Payroll_I>();

        if (Session["importPayAlertDetails"] == null)
        {
            Session["importPayAlertDetails"] = Payroll_Controller.manufactureEmployerPayrollImportList(_employerID);
        }

        fullList = (List<Payroll_I>)Session["importPayAlertDetails"];

        if (Cb_f_BatchID.Checked == true)
        {
            try
            {
                int batchID = int.Parse(Txt_f_BatchID.Text);

                foreach (Payroll_I pi in fullList)
                {
                    if (pi.PAY_BATCH_ID == batchID)
                    {
                        filteredList1.Add(pi);
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

        if (Cb_f_PayrollID.Checked == true)
        {
            string payrollID = Txt_f_PayrollID.Text.ToLower();

            foreach (Payroll_I pi in filteredList1)
            {
                if (pi.EMPLOYEE_EXT_ID.ToLower().Contains(payrollID))
                {
                    filteredList2.Add(pi);
                }
            }
        }
        else
        {
            filteredList2 = filteredList1;
        }

        if (Cb_f_LastName.Checked == true)
        {
            string lname = Txt_f_LastName.Text.ToLower();

            foreach (Payroll_I pi in filteredList2)
            {
                if (pi.PAY_L_NAME.ToLower().Contains(lname))
                {
                    filteredList3.Add(pi);
                }
            }
        }
        else
        {
            filteredList3 = filteredList2;
        }

        if (Cb_f_ActHours.Checked == true)
        {
            
                try
                {
                    string acahours = Txt_f_Hours.Text;

                    foreach (Payroll_I pi in filteredList3)
                    {
                        if (pi.PAY_I_HOURS.Contains(acahours))
                        {
                            filteredList4.Add(pi);
                        }
                    }
                }
                catch (Exception exception)
                {

                    this.Log.Warn("Suppressing errors.", exception);

                    filteredList4 = filteredList3;

                }

            }
        else
        {
            filteredList4 = filteredList3;
        }

        if (Cb_f_sdate.Checked == true)
        {
            try
            {
                string sdate = Txt_f_Sdate.Text;

                foreach (Payroll_I pi in filteredList4)
                {
                    if (pi.PAY_I_SDATE.Contains(sdate))
                    {
                        filteredList5.Add(pi);
                    }
                }
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                filteredList5 = filteredList4;

            }

        }
        else
        {
            filteredList5 = filteredList4;
        }

        if (Cb_f_edate.Checked == true)
        {
            try
            {
                string edate = Txt_f_Edate.Text;

                foreach (Payroll_I pi in filteredList5)
                {
                    if (pi.PAY_I_EDATE.Contains(edate))
                    {
                        filteredList6.Add(pi);
                    }
                }
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                filteredList6 = filteredList5;

            }

        }
        else
        {
            filteredList6 = filteredList5;
        }

          if (Cb_f_PayDesc.Checked == true)
            {
            try
            {
                string extID = Ddl_f_PayDesc.SelectedItem.Value.ToLower();

                foreach (Payroll_I pi in filteredList6)
                {
                    if (pi.PAY_GP_EXT_ID.ToLower().Contains(extID))
                    {
                        filteredList7.Add(pi);
                    }
                }
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                filteredList7 = filteredList6;

            }

        }
        else
        {
            filteredList7 = filteredList6;
        }

        if (Cb_f_CheckDate.Checked == true)
        {
            try
            {
                string cdate = Txt_f_Cdate.Text;

                foreach (Payroll_I pi in filteredList7)
                {
                    if (pi.PAY_I_CDATE.Contains(cdate))
                    {
                        filteredList8.Add(pi);
                    }
                }
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                filteredList8 = filteredList7;

            }

        }
        else
        {
            filteredList8 = filteredList7;
        }

        GvPayrollData.DataSource = filteredList8;
        GvPayrollData.DataBind();

        litAlertCount.Text = fullList.Count.ToString();
        litAlertsShown.Text = filteredList8.Count.ToString();
    }
    //******************* END Data Loading Function ********************************
    //******************* START Checkbox Changed Events ********************************
    protected void CbCheckAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GvPayrollData.Rows)
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

  protected void Cb_f_BatchID_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_BatchID.Checked == true)
        {
            Txt_f_BatchID.Text = null;
            Txt_f_BatchID.Enabled = true;
        }
        else
        {
            Txt_f_BatchID.Text = "n/a";
            Txt_f_BatchID.Enabled = false;
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
    

    protected void Cb_f_ActHours_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_ActHours.Checked == true)
        {
            Txt_f_Hours.Text = null;
            Txt_f_Hours.Enabled = true;
        }
        else
        {
            Txt_f_Hours.Text = "n/a";
            Txt_f_Hours.Enabled = false;
        }
    }

    protected void Cb_f_sdate_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_sdate.Checked == true)
        {
            Txt_f_Sdate.Text = null;
            Txt_f_Sdate.Enabled = true;
        }
        else
        {
            Txt_f_Sdate.Text = "n/a";
            Txt_f_Sdate.Enabled = false;
        }
    }

    protected void Cb_f_edate_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_edate.Checked == true)
        {
            Txt_f_Edate.Text = null;
            Txt_f_Edate.Enabled = true;
        }
        else
        {
            Txt_f_Edate.Text = "n/a";
            Txt_f_Edate.Enabled = false;
        }
    }

    protected void Cb_f_PayDesc_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_PayDesc.Checked == true)
        {
            Ddl_f_PayDesc.Enabled = true;
            Ddl_f_PayDesc.SelectedIndex = Ddl_f_PayDesc.Items.Count - 1;
        }
        else
        {
            Ddl_f_PayDesc.Enabled = false;
            Ddl_f_PayDesc.SelectedIndex = Ddl_f_PayDesc.Items.Count - 1;
        }
    }

    protected void Cb_f_CheckDate_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_CheckDate.Checked == true)
        {
            Txt_f_Cdate.Text = null;
            Txt_f_Cdate.Enabled = true;
        }
        else
        {
            Txt_f_Cdate.Text = "n/a";
            Txt_f_Cdate.Enabled = false;
        }
    }

   protected void Cb_u_ACTHours_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_ACTHours.Checked == true)
        {
            Txt_u_Hours.Text = null;
            Txt_u_Hours.Enabled = true;
        }
        else
        {
            Txt_u_Hours.Text = "n/a";
            Txt_u_Hours.Enabled = false;
            Txt_u_Hours.BackColor = System.Drawing.Color.White;
        }
    }
  protected void Cb_u_PayrollStart_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_PayrollStart.Checked == true)
        {
            Txt_u_Start.Text = null;
            Txt_u_Start.Enabled = true;
        }
        else
        {
            Txt_u_Start.Text = "n/a";
            Txt_u_Start.Enabled = false;
            Txt_u_Start.BackColor = System.Drawing.Color.White;
        }
    }
    protected void Cb_u_PayrollEnd_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_PayrollEnd.Checked == true)
        {
            Txt_u_End.Text = null;
            Txt_u_End.Enabled = true;
        }
        else
        {
            Txt_u_End.Text = "n/a";
            Txt_u_End.Enabled = false;
            Txt_u_End.BackColor = System.Drawing.Color.White;
        }
    }

  protected void Cb_u_PayDesc_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_PayDesc.Checked == true)
        {
            Ddl_u_PayDesc.SelectedIndex = Ddl_u_PayDesc.Items.Count - 1;
            Ddl_u_PayDesc.Enabled = true;
        }
        else
        {
            Ddl_u_PayDesc.SelectedIndex = Ddl_u_PayDesc.Items.Count - 1;
            Ddl_u_PayDesc.Enabled = false;
            Ddl_u_PayDesc.BackColor = System.Drawing.Color.White;
        }
    }

 protected void Cb_u_CheckDate_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_u_CheckDate.Checked == true)
        {
            Txt_u_Cdate.Text = null;
            Txt_u_Cdate.Enabled = true;
        }
        else
        {
            Txt_u_Cdate.Text = "n/a";
            Txt_u_Cdate.Enabled = false;
            Txt_u_Cdate.BackColor = System.Drawing.Color.White;
        }
    }
    //******************* END Checkbox Changed Events ********************************
    //******************* START Gridview Events ********************************
    protected void GvPayrollData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPayrollData.PageIndex = e.NewPageIndex;
        loadPayRollData();
    }


    protected void GvPayrollData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortBy = e.SortExpression;
        string lastSortExpression = "";
        string lastSortDirection = "ASC";
        int _employerID = int.Parse(HfDistrictID.Value);

        List<Payroll_I> tempList = (List<Payroll_I>)Session["importPayAlertDetails"];

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
                        case "PAY_BATCH_ID":
                            tempList = tempList.OrderBy(o => o.PAY_BATCH_ID).ToList();
                            break;
                        case "PAY_F_NAME":
                            tempList = tempList.OrderBy(o => o.PAY_F_NAME).ToList();
                            break;
                        case "PAY_M_NAME":
                            tempList = tempList.OrderBy(o => o.PAY_M_NAME).ToList();
                            break;
                        case "PAY_L_NAME":
                            tempList = tempList.OrderBy(o => o.PAY_L_NAME).ToList();
                            break;
                        case "PAY_I_HOURS":
                            tempList = tempList.OrderBy(o => o.PAY_I_HOURS).ToList();
                            break;
                        case "PAY_SDATE":
                            tempList = tempList.OrderBy(o => o.PAY_SDATE).ToList();
                            break;
                        case "PAY_EDATE":
                            tempList = tempList.OrderBy(o => o.PAY_EDATE).ToList();
                            break;
                        case "PAY_GP_DESC":
                            tempList = tempList.OrderBy(o => o.PAY_GP_DESC).ToList();
                            break;
                        case "PAY_GP_EXT_ID":
                            tempList = tempList.OrderBy(o => o.PAY_GP_EXT_ID).ToList();
                            break;
                        case "PAY_CDATE":
                            tempList = tempList.OrderBy(o => o.PAY_CDATE).ToList();
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

            Session["importPayAlertDetails"] = tempList;
            Session["sortDir"] = lastSortDirection;
            Session["sortExp"] = lastSortExpression;
            loadPayRollData();
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing exception.", exception);
        }

    }

    //******************* END Gridview Events ********************************

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            employer currDist = (employer)Session["CurrentDistrict"];
            List<Payroll_I> tempPayroll = (List<Payroll_I>)Session["importPayAlertDetails"];
            User currUser = (User)Session["CurrentUser"];

            string fileName = Payroll_Controller.generateTextFile(tempPayroll, currDist);
            string fileFullPath = Server.MapPath("..\\ftps\\export\\") + fileName;
            string body = "A " + currDist.EMPLOYER_NAME + " payroll alert file has been downloaded by " + HfUserName.Value + " at " + DateTime.Now.ToString();

            PIILogger.LogPII(String.Format("Payroll Alert Download: {0} -- File Path: [{1}], IP:[{2}], User Id:[{3}]", body, fileFullPath, Request.UserHostAddress, currUser.User_ID));
            string appendText = "attachment; filename=" + fileName;
            Response.ContentType = "file/text";
            Response.AppendHeader("Content-Disposition", appendText);
            Response.TransmitFile(fileFullPath);
            Response.Flush();         
            Response.SuppressContent = true;                
            HttpContext.Current.ApplicationInstance.CompleteRequest();                      
            Response.End();
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }
    }

    /// <summary>
    /// Delete all checked Payroll Import Errors.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        string recordsRemoved = "Payroll Records Removed:<br />";
        string recordsNotRemoved = "Records that could not be DELETED:<br />";
        int i = 0;

        foreach (GridViewRow row in GvPayrollData.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
            HiddenField hf = (HiddenField)row.FindControl("Hf_gv_RowID");
            bool validTransaction = false;

            if (cb.Checked == true)
            {
                i += 1;
                int _rowID = int.Parse(hf.Value);
                validTransaction = Payroll_Controller.deleteImportedPayrollRow(_rowID);

                if (validTransaction == true)
                {
                    CbCheckAll.Checked = false;
                    List<Payroll_I> tempList = (List<Payroll_I>)Session["importPayAlertDetails"];
                    Payroll_I tempP = null;

                    foreach (Payroll_I pi in tempList)
                    {
                        if (pi.ROW_ID == _rowID)
                        {
                            tempP = pi;
                            break;
                        }
                    }

                    tempList.Remove(tempP);
                    Session["importPayAlertDetails"] = tempList;

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
            loadPayRollData();

            MpeWebMessage.Show();
            LitMessage.Text = recordsRemoved + "<br />" + recordsNotRemoved;
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int z = 0;                      
        int i = 0;                    
        List<Payroll_I> tempList = (List<Payroll_I>)Session["importPayAlertDetails"];
        List<Payroll_I> removedAlerts = new List<Payroll_I>();
        string _modBy = HfUserName.Value;
        DateTime _modOn = DateTime.Now;

        if (Cb_u_ACTHours.Checked == true)
        {
            validData = errorChecking.validateTextBoxDecimal(Txt_u_Hours, validData);
            z += 1;
        }

        if (Cb_u_CheckDate.Checked == true)
        {
            validData = errorChecking.validateTextBoxDate(Txt_u_Cdate, validData);
            z += 1;
        }

        if (Cb_u_PayDesc.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(Ddl_u_PayDesc, validData);
            z += 1;
        }

        if (Cb_u_PayrollStart.Checked == true)
        {
            validData = errorChecking.validateTextBoxDate(Txt_u_Start, validData);
            z += 1;
        }

        if (Cb_u_PayrollEnd.Checked == true)
        {
            validData = errorChecking.validateTextBoxDate(Txt_u_End, validData);
            z += 1;
        }

        foreach (GridViewRow row in GvPayrollData.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");

            if (cb.Checked == true)
            {
                i += 1;
            }
        }
        if (i == 0 || z == 0)
        {
            validData = false;
        }
        if (validData == true)
        {
            foreach (GridViewRow row in GvPayrollData.Rows)
            {
                try
                {
                    CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
                    HiddenField hf = (HiddenField)row.FindControl("Hf_gv_RowID");

                    if (cb.Checked == true)
                    {
                        int _rowID = 0;
                        int _employeeID = 0;
                        decimal _hours = 0;
                        DateTime? _sDate = null;
                        DateTime? _eDate = null;
                        DateTime? _cDate = null;
                        int _grossPayID = 0;
                        bool validTransaction = false;
                        bool validTransfer = false;
                        int _employerID = int.Parse(HfDistrictID.Value);
                        string _history = "Import Values: " + _modBy + " " + _modOn.ToString() + " From: view_payroll_import.aspx";

                        _rowID = int.Parse(hf.Value);
                        Payroll_I currPayroll = Payroll_Controller.getSinglePayroll_I(_rowID, tempList);

                        if (Cb_u_ACTHours.Checked == true)
                        {
                            _hours = decimal.Parse(Txt_u_Hours.Text);
                        }
                        else
                        {
                            _hours = currPayroll.PAY_HOURS;
                        }

                        if (Cb_u_PayrollStart.Checked == true)
                        {
                            _sDate = DateTime.Parse(Txt_u_Start.Text);
                        }
                        else
                        {
                            _sDate = currPayroll.PAY_SDATE;
                        }

                        if (Cb_u_PayrollEnd.Checked == true)
                        {
                            _eDate = DateTime.Parse(Txt_u_End.Text);
                        }
                        else
                        {
                            _eDate = currPayroll.PAY_EDATE;
                        }

                        if (Cb_u_CheckDate.Checked == true)
                        {
                            _cDate = DateTime.Parse(Txt_u_Cdate.Text);
                        }
                        else
                        {
                            _cDate = currPayroll.PAY_CDATE;
                        }

                        if (Cb_u_PayDesc.Checked == true)
                        {
                            _grossPayID = int.Parse(Ddl_u_PayDesc.SelectedItem.Value);
                        }
                        else
                        {
                            List<gpType> tempGpList = gpType_Controller.getEmployeeTypes(_employerID);
                            gpType currGpType = gpType_Controller.validateGpType(_employerID, currPayroll.PAY_GP_EXT_ID, currPayroll.PAY_GP_DESC, tempGpList);

                            _grossPayID = currGpType.GROSS_PAY_ID;
                        }


                        if (currPayroll.PAY_EMPLOYEE_ID == 0)
                        {
                            List<Employee> empList = (List<Employee>)Session["Employees"];
                            Employee tempEmployee = null;
                            tempEmployee = EmployeeController.validateExistingEmployee(empList, currPayroll.PAY_SSN);
                            if (tempEmployee != null)
                            {
                                _employeeID = tempEmployee.EMPLOYEE_ID;
                            }
                            else
                            {
                                _employeeID = 0;
                            }
                        }
                        else
                        {
                            _employeeID = currPayroll.PAY_EMPLOYEE_ID;
                        }

                        _history += " | " + currPayroll.PAY_I_HOURS + " | " + currPayroll.PAY_I_SDATE + " | " + currPayroll.PAY_I_EDATE + " | " + currPayroll.PAY_I_CDATE + " | " + currPayroll.PAY_GP_EXT_ID + " | " + currPayroll.PAY_GP_DESC;

                        validTransaction = Payroll_Controller.UpdateImportPayroll(_rowID, _employeeID, _grossPayID, _hours, _sDate, _eDate, _cDate, _modBy, _modOn);
                        if (validTransaction == true)
                        {
                            foreach (Payroll_I pi in tempList)
                            {
                                if (pi.ROW_ID == _rowID)
                                {
                                    pi.PAY_GP_ID = _grossPayID;
                                    pi.PAY_HOURS = _hours;
                                    pi.PAY_SDATE = _sDate;
                                    pi.PAY_EDATE = _eDate;
                                    pi.PAY_CDATE = _cDate;
                                    pi.PAY_MOD_BY = _modBy;
                                    pi.PAY_MOD_ON = _modOn;
                                    break;
                                }
                            }
                        }

                        try
                        {
                            validTransfer = Payroll_Controller.TransferPayroll(_rowID, _employerID, currPayroll.PAY_BATCH_ID, _employeeID, _grossPayID, _hours, (DateTime)_sDate, (DateTime)_eDate, (DateTime)_cDate, _modBy, _modOn, _history);


                            if (validTransfer == true)
                            {
                                removedAlerts.Add(currPayroll);
                            }
                        }
                        catch (Exception exception)
                        {
                            this.Log.Warn("Suppressing exception.", exception);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.Log.Warn("Suppressing exception.", exception);
                }
            }
            Session["importPayAlertDetails"] = tempList;

            syncPayrollAlertSession(removedAlerts);

            CbCheckAll.Checked = false;
            loadPayRollData();
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "Please correct any fields that are highlighted in RED. If there are not any RED fields, than make sure that atleast one row is selected to be updated.";
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



    protected void BtnApplyFilters_Click(object sender, EventArgs e)
    {
        loadPayRollData();
    }
}