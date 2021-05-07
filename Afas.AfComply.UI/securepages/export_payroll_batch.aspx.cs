using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

public partial class securepages_export_payroll_batch : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(securepages_export_payroll_batch));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        HfUserName.Value = user.User_Full_Name;

        HfDistrictID.Value = user.User_District_ID.ToString();

        loadGrossPayDescriptions();
        loadBatches();

        Session["importPayBatchDetails"] = new List<Payroll_E>();
    }


    //******************* START Data Loading Function ********************************
    private void loadEmployees()
    {
        if (Session["Employees"] == null)
        {
            int _employerID = int.Parse(HfDistrictID.Value);
            Session["Employees"] = EmployeeController.manufactureEmployeeList(_employerID);
        }
    }

    private void loadBatches()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        Ddl_f_BatchID.DataSource = employerController.manufactureBatchList(_employerID);
        Ddl_f_BatchID.DataTextField = "BATCH_ID";
        Ddl_f_BatchID.DataValueField = "BATCH_ID";
        Ddl_f_BatchID.DataBind();

        Ddl_f_BatchID.Items.Add("Select");
        Ddl_f_BatchID.SelectedIndex = Ddl_f_BatchID.Items.Count - 1;
    }

    private void loadGrossPayDescriptions()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        Ddl_f_PayDesc.DataSource = gpType_Controller.getEmployeeTypes(_employerID);
        Ddl_f_PayDesc.DataTextField = "GROSS_PAY_DESCRIPTION";
        Ddl_f_PayDesc.DataValueField = "GROSS_PAY_ID";
        Ddl_f_PayDesc.DataBind();

        Ddl_f_PayDesc.Items.Add("Select");
        Ddl_f_PayDesc.SelectedIndex = Ddl_f_PayDesc.Items.Count - 1;
    }


    private void loadPayRollData()
    {

        int _employerID = 0;
        int _batchID = 0;
        int _lastBatchID = 0;

        if (false == int.TryParse(HfDistrictID.Value, out _employerID) || 
            false == int.TryParse(Ddl_f_BatchID.SelectedItem.Value, out _batchID) ||
            false ==  int.TryParse(hfLastBatchID.Value, out _lastBatchID))
        {
            Log.Warn("Failed to parse values: District: ["+ HfDistrictID.Value + "] Selected Batch: ["+ Ddl_f_BatchID.SelectedItem.Value + "] LastBatch: ["+ hfLastBatchID.Value + "]");

            return;
        }

        List<Payroll_E> fullList = new List<Payroll_E>();
        List<Payroll_E> filteredList1 = new List<Payroll_E>();
        List<Payroll_E> filteredList2 = new List<Payroll_E>();
        List<Payroll_E> filteredList3 = new List<Payroll_E>();
        List<Payroll_E> filteredList4 = new List<Payroll_E>();
        List<Payroll_E> filteredList5 = new List<Payroll_E>();
        List<Payroll_E> filteredList6 = new List<Payroll_E>();
        List<Payroll_E> filteredList7 = new List<Payroll_E>();

        if (_batchID != _lastBatchID)
        {
            Session["importPayBatchDetails"] = Payroll_Controller.getPayrollbyBatchID(_batchID, _employerID);
        }


        hfLastBatchID.Value = _batchID.ToString();

        fullList = (List<Payroll_E>)Session["importPayBatchDetails"];

        if (Cb_f_PayrollID.Checked == true)
        {
            string payID = Txt_f_PayrollID.Text.ToLower();

            foreach (Payroll_E pi in fullList)
            {
                if (pi.PAY_EMPLOYEE_EXT_ID.ToLower().Contains(payID))
                {
                    filteredList1.Add(pi);
                }
            }
        }
        else
        {
            filteredList1 = fullList;
        }

        if (Cb_f_LastName.Checked == true)
        {
            string lname = Txt_f_LastName.Text.ToLower();

            foreach (Payroll_E pi in filteredList1)
            {
                if (pi.PAY_L_NAME.ToLower().Contains(lname))
                {
                    filteredList2.Add(pi);
                }
            }
        }
        else
        {
            filteredList2 = filteredList1;
        }

        if (Cb_f_ActHours.Checked == true)
        {
            try
            {
                decimal acahours = decimal.Parse(Txt_f_Hours.Text);

                foreach (Payroll_E pi in filteredList2)
                {
                    if (pi.PAY_HOURS == acahours)
                    {
                        filteredList3.Add(pi);
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

        if (Cb_f_sdate.Checked == true)
        {
            try
            {
                DateTime sdate = DateTime.Parse(Txt_f_Sdate.Text);

                foreach (Payroll_E pi in filteredList3)
                {
                    if (pi.PAY_SDATE == sdate)
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

        if (Cb_f_edate.Checked == true)
        {
            try
            {
                DateTime edate = DateTime.Parse(Txt_f_Edate.Text);

                foreach (Payroll_E pi in filteredList4)
                {
                    if (pi.PAY_EDATE == edate)
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

        if (Cb_f_PayDesc.Checked == true)
        {
            try
            {
                int gpID = int.Parse(Ddl_f_PayDesc.SelectedItem.Value.ToLower());

                foreach (Payroll_E pi in filteredList5)
                {
                    if (pi.PAY_GP_ID == gpID)
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

        if (Cb_f_CheckDate.Checked == true)
        {
            try
            {
                DateTime cdate = DateTime.Parse(Txt_f_Cdate.Text);

                foreach (Payroll_E pi in filteredList6)
                {
                    if (pi.PAY_CDATE == cdate)
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

        GvPayrollData.DataSource = filteredList7;
        GvPayrollData.DataBind();

        litAlertCount.Text = fullList.Count.ToString();
        litAlertsShown.Text = filteredList7.Count.ToString();
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

        List<Payroll_E> tempList = (List<Payroll_E>)Session["importPayBatchDetails"];

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
                        case "PAY_EMPLOYEE_EXT_ID":
                            tempList = tempList.OrderBy(o => o.PAY_EMPLOYEE_EXT_ID).ToList();
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
                        case "PAY_HOURS":
                            tempList = tempList.OrderBy(o => o.PAY_HOURS).ToList();
                            break;
                        case "PAY_SDATE":
                            tempList = tempList.OrderBy(o => o.PAY_SDATE).ToList();
                            break;
                        case "PAY_EDATE":
                            tempList = tempList.OrderBy(o => o.PAY_EDATE).ToList();
                            break;
                        case "PAY_GP_DESC":
                            tempList = tempList.OrderBy(o => o.PAY_GP_NAME).ToList();
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

            Session["importPayBatchDetails"] = tempList;
            Session["sortDir"] = lastSortDirection;
            Session["sortExp"] = lastSortExpression;
            loadPayRollData();
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
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
            List<Payroll_E> tempPayroll = (List<Payroll_E>)Session["importPayBatchDetails"];
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


    protected void BtnSave_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int z = 0;                      
        int i = 0;                    
        List<Payroll_E> tempList = (List<Payroll_E>)Session["importPayBatchDetails"];
        List<Payroll_E> removedAlerts = new List<Payroll_E>();
        string _modBy = HfUserName.Value;
        DateTime _modOn = DateTime.Now;

        if (Cb_u_ACTHours.Checked == true)
        {
            validData = errorChecking.validateTextBoxDecimal(Txt_u_Hours, validData);
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
        if (i > 0)
        {
            CbCheckAll.BackColor = System.Drawing.Color.Transparent;
            if (z > 0)
            {
                Cb_u_ACTHours.BackColor = System.Drawing.Color.Transparent;
                Cb_u_PayrollStart.BackColor = System.Drawing.Color.Transparent;
                Cb_u_PayrollEnd.BackColor = System.Drawing.Color.Transparent;
                if (validData == true)
                {
                    foreach (GridViewRow row in GvPayrollData.Rows)
                    {
                        try
                        {
                            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
                            HiddenField hf = (HiddenField)row.FindControl("Hf_gv_RowID");
                            HiddenField hf2 = (HiddenField)row.FindControl("Hf_gv_EmployeeID");

                            if (cb.Checked == true)
                            {
                                int _rowID = 0;
                                int _employeeID = 0;
                                int _employerID = 0;
                                decimal _hours = 0;
                                DateTime? _sDate = null;
                                DateTime? _eDate = null;
                                bool validTransaction = false;
                                string _history = "Import Values: " + _modBy + " " + _modOn.ToString() + " From: export_payroll_batch.aspx";

                                _employerID = int.Parse(HfDistrictID.Value);
                                _employeeID = int.Parse(hf2.Value);
                                _rowID = int.Parse(hf.Value);

                                Payroll_E currPayroll = Payroll_Controller.getSinglePayroll(_rowID, tempList);

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

                                _history += " | " + currPayroll.PAY_HOURS + " | " + currPayroll.PAY_SDATE + " | " + currPayroll.PAY_EDATE + " | ";

                                validTransaction = Payroll_Controller.updatePayroll(_rowID, _employerID, _employeeID, _hours, _sDate, _eDate, _modBy, _modOn, _history);
                                if (validTransaction == true)
                                {
                                    foreach (Payroll_E pi in tempList)
                                    {
                                        if (pi.ROW_ID == _rowID)
                                        {
                                            pi.PAY_HOURS = _hours;
                                            pi.PAY_SDATE = _sDate;
                                            pi.PAY_EDATE = _eDate;
                                            pi.PAY_MOD_BY = _modBy;
                                            pi.PAY_MOD_ON = _modOn;
                                            pi.PAY_HISTORY = _history;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception exception)
                        {

                            this.Log.Warn("Suppressing errors.", exception);

                        }
                    }
                    Session["importPayBatchDetails"] = tempList;

                    CbCheckAll.Checked = false;
                    loadPayRollData();
                }
                else
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "Please correct any fields that are highlighted in RED. If there are not any RED fields, than make sure that atleast one row is selected to be updated.";
                }
            }
            else
            {
                Cb_u_ACTHours.BackColor = System.Drawing.Color.Red;
                Cb_u_PayrollStart.BackColor = System.Drawing.Color.Red;
                Cb_u_PayrollEnd.BackColor = System.Drawing.Color.Red;
                MpeWebMessage.Show();
                LitMessage.Text = "Please select which field needs to be updated and enter a value.";
            }
        }
        else
        {
            CbCheckAll.BackColor = System.Drawing.Color.Red;
            MpeWebMessage.Show();
            LitMessage.Text = "Please select a record in the Grid View to update.";
        }
    }

    protected void BtnApplyFilters_Click(object sender, EventArgs e)
    {
        loadPayRollData();
    }
    protected void BtnViewBatchData_Click(object sender, EventArgs e)
    {
        loadPayRollData();
    }
}