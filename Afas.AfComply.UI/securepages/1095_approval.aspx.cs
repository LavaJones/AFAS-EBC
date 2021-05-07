using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using System.Globalization;

using log4net;

using Afas.AfComply.Domain;
using Afas.Domain;
using Afas.Application.CSV;

public partial class approval_1095 : Afas.AfComply.UI.securepages.SecurePageBase
{

    protected override void PageLoadLoggedIn(User user, employer employer)
    {

        if (null == employer || false == employer.IrsEnabled)
        {

            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [approval_1095] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=57", false);

            return;

        }

        HfDistrictID.Value = user.User_District_ID.ToString();

        int rows = 0;
        int pageIndex = 0;
        int pageSize = Gv1095.PageSize;

        loadEmployeeClasses();
        filterEmployees();
        loadEmployees();

        Page.Form.Attributes.Add("enctype", "multipart/form-data");

        List<Employee> tempList = (List<Employee>)Session["Employees1095Filtered"];
        rows = tempList.Count;
        DataBind_ddlPageNumber(pageIndex, pageSize, rows);

        var EmployerId = int.Parse(HfDistrictID.Value);
        var TaxYearId = DateTime.Now.AddYears(-1).Year;

        EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, TaxYearId);

        if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.ETL)
        {

            var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                TransmissionStatusEnum.Review,
                user.User_UserName
            );

            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

        }

    }

    private void loadEmployeeClasses()
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        Ddl_f_EmployeeClass.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        Ddl_f_EmployeeClass.DataTextField = "CLASS_DESC";
        Ddl_f_EmployeeClass.DataValueField = "CLASS_ID";
        Ddl_f_EmployeeClass.DataBind();

        Ddl_f_EmployeeClass.Items.Add("Select All");
        Ddl_f_EmployeeClass.SelectedIndex = Ddl_f_EmployeeClass.Items.Count - 1;

    }

    private void loadEmployees()
    {

        try
        {

            int employerId = int.Parse(HfDistrictID.Value);
            int taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);
            Session["Approved1095Employees"] = null;                   
            Session["FilteredApproved1095Employees"] = null;           

            List<Employee> tempList = (List<Employee>)Session["Employees1095Filtered"];
            List<Employee> totalList = EmployeeController.EmployeesPending1095Approval(employerId, taxYear, true);

            sortEmployees();

            litAlertCount.Text = totalList.Count.ToString();
            litAlertsShown.Text = tempList.Count.ToString();

        }
        catch (Exception exception)
        {
            Log.Warn("Exception while loading employees.", exception);
        }

    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    private List<monthlyDetail> getMonthlyDetails(int _employeeID)
    {
        return airController.ManufactureEmployeeMonthlyDetailList(_employeeID, true);
    }

    /// <summary>
    /// Update the Monthly Employee Detail in the AIR database. 
    /// </summary>
    private Boolean updateEmployeeDetail(
            int _employeeID,
            int _timeFrameID,
            DropDownList ddlins,
            DropDownList ddlooc,
            DropDownList ddlAsh,
            DropDownList ddlStat,
            CheckBox cbEnrolled,
            CheckBox cbMec,
            TextBox txtLmcp
        )
    {

        Boolean validTransaction = true;
        int _insTypeID = 0;
        int _statusID = 0;
        String _ash = null;
        decimal? _lcmp = null;
        String _ooc = null;
        Boolean _enrolled = false;
        Boolean? _mec = false;
        decimal _hours = 0;

        int _employerID = int.Parse(HfDistrictID.Value);
        String _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;
        Boolean validData = true;

        _enrolled = cbEnrolled.Checked;
        _mec = cbMec.Checked;

        validData = errorChecking.validateDropDownSelection(ddlooc, validData);

        validData = errorChecking.validateDropDownSelectionMonthlyStatus(ddlStat, validData);

        if (false == errorChecking.validateDropDownSelection(ddlooc, true) || false == ddlooc.SelectedValue.Equals("1H"))
        {
            validData = errorChecking.validateDropDownSelection(ddlins, validData);
        }

        validData = errorChecking.validateDropDownSelectionLine15_Value(ddlooc, txtLmcp, validData);

        validData = errorChecking.validateDropDownSelectionLine14_1H_Line15_noValue(ddlooc, txtLmcp, validData);

        if (validData == true)
        {

            _ooc = ddlooc.SelectedItem.Value;

            int.TryParse(ddlStat.SelectedItem.Value, out _statusID);

            _insTypeID = errorChecking.validateDropDownIntNULL(ddlins);

            decimal lcmp = 0;
            if (decimal.TryParse(txtLmcp.Text, out lcmp))
            {
                _lcmp = lcmp;
            }   

            _ash = errorChecking.validateDropDownStringNULL(ddlAsh);

            if (validTransaction == true)
            {

                validTransaction = airController.UpdateEmployeeMonthlyDetailList(
                        _employeeID,
                        _timeFrameID,
                        _employerID,
                        _ooc,
                        _lcmp,
                        _ash,
                        _modBy,
                        _modOn,
                        _hours,
                        _enrolled,
                        _mec,
                        _statusID,
                        _insTypeID,
                        false
                    );

            }

        }
        else
        {
            validTransaction = false;
        }

        return validTransaction;

    }

    protected void RbCheckAll1095C_CheckedChanged(object sender, EventArgs e)
    {
        checkAll();
    }

    protected void RbCheckAllNo1095C_CheckedChanged(object sender, EventArgs e)
    {
        checkAll();
    }

    protected void RbCheckAllClear_CheckedChanged(object sender, EventArgs e)
    {
        checkAll();
    }

    private void checkAll()
    {

        foreach (GridViewRow row in Gv1095.Rows)
        {

            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_1095");

            if (CbCheckAll1095C.Checked == true)
            {
                cb.Checked = true;
                cb.Enabled = true;
            }
            else
            {
                cb.Checked = false;
                cb.Enabled = true;
            }

        }

    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {

        int receive1095 = 0;
        int receive1095No = 0;
        String _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;
        int _employerID = int.Parse(HfDistrictID.Value);
        int _taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);

        Boolean validTransactions = true;

        foreach (GridViewRow row in Gv1095.Rows)
        {

            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_1095");

            if (cb.Checked == true)
            {
                receive1095 += 1;
            }

        }

        if (receive1095 > 0)
        {

            foreach (GridViewRow row in Gv1095.Rows)
            {

                CheckBox cb = (CheckBox)row.FindControl("Cb_gv_1095");

                DropDownList ddl_ooc_Jan = (DropDownList)row.FindControl("Ddl_gv_ooc_jan");
                DropDownList ddl_ash_Jan = (DropDownList)row.FindControl("Ddl_gv_ash_jan");
                TextBox txt_lcmp_Jan = (TextBox)row.FindControl("Txt_gv_lcmp_jan");
                DropDownList ddl_ins_Jan = (DropDownList)row.FindControl("Ddl_gv_ins_type_jan");

                DropDownList ddl_ooc_Feb = (DropDownList)row.FindControl("Ddl_gv_ooc_feb");
                DropDownList ddl_ash_Feb = (DropDownList)row.FindControl("Ddl_gv_ash_feb");
                TextBox txt_lcmp_Feb = (TextBox)row.FindControl("Txt_gv_lcmp_feb");
                DropDownList ddl_ins_Feb = (DropDownList)row.FindControl("Ddl_gv_ins_type_fan");

                DropDownList ddl_ooc_Mar = (DropDownList)row.FindControl("Ddl_gv_ooc_mar");
                DropDownList ddl_ash_Mar = (DropDownList)row.FindControl("Ddl_gv_ash_mar");
                TextBox txt_lcmp_Mar = (TextBox)row.FindControl("Txt_gv_lcmp_mar");
                DropDownList ddl_ins_Mar = (DropDownList)row.FindControl("Ddl_gv_ins_type_mar");

                DropDownList ddl_ooc_Apr = (DropDownList)row.FindControl("Ddl_gv_ooc_apr");
                DropDownList ddl_ash_Apr = (DropDownList)row.FindControl("Ddl_gv_ash_apr");
                TextBox txt_lcmp_Apr = (TextBox)row.FindControl("Txt_gv_lcmp_apr");
                DropDownList ddl_ins_Apr = (DropDownList)row.FindControl("Ddl_gv_ins_type_apr");

                DropDownList ddl_ooc_May = (DropDownList)row.FindControl("Ddl_gv_ooc_may");
                DropDownList ddl_ash_May = (DropDownList)row.FindControl("Ddl_gv_ash_may");
                TextBox txt_lcmp_May = (TextBox)row.FindControl("Txt_gv_lcmp_may");
                DropDownList ddl_ins_May = (DropDownList)row.FindControl("Ddl_gv_ins_type_may");

                DropDownList ddl_ooc_Jun = (DropDownList)row.FindControl("Ddl_gv_ooc_jun");
                DropDownList ddl_ash_Jun = (DropDownList)row.FindControl("Ddl_gv_ash_jun");
                TextBox txt_lcmp_Jun = (TextBox)row.FindControl("Txt_gv_lcmp_jun");
                DropDownList ddl_ins_Jun = (DropDownList)row.FindControl("Ddl_gv_ins_type_jun");

                DropDownList ddl_ooc_Jul = (DropDownList)row.FindControl("Ddl_gv_ooc_jul");
                DropDownList ddl_ash_Jul = (DropDownList)row.FindControl("Ddl_gv_ash_jul");
                TextBox txt_lcmp_Jul = (TextBox)row.FindControl("Txt_gv_lcmp_jul");
                DropDownList ddl_ins_Jul = (DropDownList)row.FindControl("Ddl_gv_ins_type_jul");

                DropDownList ddl_ooc_Aug = (DropDownList)row.FindControl("Ddl_gv_ooc_aug");
                DropDownList ddl_ash_Aug = (DropDownList)row.FindControl("Ddl_gv_ash_aug");
                TextBox txt_lcmp_Aug = (TextBox)row.FindControl("Txt_gv_lcmp_aug");
                DropDownList ddl_ins_Aug = (DropDownList)row.FindControl("Ddl_gv_ins_type_aug");

                DropDownList ddl_ooc_Sep = (DropDownList)row.FindControl("Ddl_gv_ooc_sep");
                DropDownList ddl_ash_Sep = (DropDownList)row.FindControl("Ddl_gv_ash_sep");
                TextBox txt_lcmp_Sep = (TextBox)row.FindControl("Txt_gv_lcmp_sep");
                DropDownList ddl_ins_Sep = (DropDownList)row.FindControl("Ddl_gv_ins_type_sep");

                DropDownList ddl_ooc_Oct = (DropDownList)row.FindControl("Ddl_gv_ooc_oct");
                DropDownList ddl_ash_Oct = (DropDownList)row.FindControl("Ddl_gv_ash_oct");
                TextBox txt_lcmp_Oct = (TextBox)row.FindControl("Txt_gv_lcmp_oct");
                DropDownList ddl_ins_Oct = (DropDownList)row.FindControl("Ddl_gv_ins_type_oct");

                DropDownList ddl_ooc_Nov = (DropDownList)row.FindControl("Ddl_gv_ooc_nov");
                DropDownList ddl_ash_Nov = (DropDownList)row.FindControl("Ddl_gv_ash_nov");
                TextBox txt_lcmp_Nov = (TextBox)row.FindControl("Txt_gv_lcmp_nov");
                DropDownList ddl_ins_Nov = (DropDownList)row.FindControl("Ddl_gv_ins_type_nov");

                DropDownList ddl_ooc_Dec = (DropDownList)row.FindControl("Ddl_gv_ooc_dec");
                DropDownList ddl_ash_Dec = (DropDownList)row.FindControl("Ddl_gv_ash_dec");
                TextBox txt_lcmp_Dec = (TextBox)row.FindControl("Txt_gv_lcmp_dec");
                DropDownList ddl_ins_Dec = (DropDownList)row.FindControl("Ddl_gv_ins_type_dec");

                HiddenField hf_emp_id = (HiddenField)row.FindControl("Hv_gv_EmployeeID");

                int _employeeID = int.Parse(hf_emp_id.Value);
                bool _get1095C = false;
                List<monthlyDetail> mdTempList = getMonthlyDetails(_employeeID);

                if (cb.Checked == true)
                {

                    _get1095C = true;

                    bool validData = true;

                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Jan, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Feb, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Mar, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Apr, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_May, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Jun, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Jul, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Aug, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Sep, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Oct, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Nov, validData);
                    validData = errorChecking.validateDropDownSelection(ddl_ooc_Dec, validData);

                    if (validData == true)
                    {
                        insertTaxYear1095Approval(mdTempList, _taxYear, _employeeID, _employerID, _modBy, _modOn, true);
                    }
                    else
                    {
                        try
                        {

                            Boolean no1095 = Boolean.Parse(Ddl1095Data.SelectedItem.Value);
                            if (no1095 == false)
                            {
                                insertTaxYear1095Approval(mdTempList, _taxYear, _employeeID, _employerID, _modBy, _modOn, false);
                            }

                        }
                        catch (Exception exception)
                        {
                            Log.Warn("Suppressing errors.", exception);
                        }

                    }

                }

            }

            CbCheckAll1095C.Checked = false;

            filterEmployees();
            loadEmployees();

        }

        List<Employee> unapproved = EmployeeController.RemainingEmployeesNeeding1095Approval(_employerID, _taxYear, true);

        if (unapproved.Count == 0)
        {
            updateEmployerTaxYearTransmission();
        }

    }

    private void insertTaxYear1095Approval(
            List<monthlyDetail> mdTempList,
            int _taxYear,
            int _employeeID,
            int _employerID,
            String _modBy,
            DateTime _modOn,
            Boolean _1095C
        )
    {

        Boolean validTransaction = EmployeeController.InsertTaxYear1095Approval(_taxYear, _employeeID, _employerID, _modBy, _modOn, _1095C);

        if (validTransaction == true)
        {

            approveMonthlyDetail(mdTempList, _modBy, _modOn);
            List<Employee> currList = EmployeeController.EmployeesPending1095Approval(_employerID, _taxYear, true);
            List<Employee> currList2 = (List<Employee>)Session["Employees1095Filtered"];

            currList.RemoveAll(Employee => Employee.EMPLOYEE_ID == _employeeID);
            currList2.RemoveAll(Employee => Employee.EMPLOYEE_ID == _employeeID);

            Session["Employees1095Filtered"] = currList2;

            if (currList.Count == 0)
            {
                updateEmployerTaxYearTransmission();
            }

        }

    }

    private void updateEmployerTaxYearTransmission()
    {

        var EmployerId = int.Parse(HfDistrictID.Value);
        var TaxYearId = DateTime.Now.AddYears(-1).Year;

        EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, TaxYearId);
        if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.Review)
        {
            User user = ((User)Session["CurrentUser"]);
            currentEmployerTaxYearTransmissionStatus = employerController.endEmployerTaxYearTransmissionStatus(currentEmployerTaxYearTransmissionStatus, user.User_UserName);

            var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                TransmissionStatusEnum.Certified,
                user.User_UserName,
                DateTime.Now
            );

            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

            if (ValidationHelper.validateNewEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus, Log))
            {
                var employer = employerController.getEmployer(EmployerId);

                Email email = new Email();
                var body = string.Format("Employer \nName: {0}\n EIN: {1}\n Transmission Status: {2}\n <a href=\"/admin/employers_certified.aspx\">Employers Certified</a>",
                    employer.EMPLOYER_NAME, employer.EMPLOYER_EIN, newEmployerTaxYearTransmissionStatus.TransmissionStatusId);
                email.SendEmail(SystemSettings.IrsProcessingAddress, Feature.IrsStatusEmailSubject, body, false);

            }
        }
    }

    /// <summary>
    /// Loop through all monthly detail records and set the Modified On and Modified Date. 
    /// </summary>
    /// <param name="tempList"></param>

    private void approveMonthlyDetail(List<monthlyDetail> tempList, String _modBy, DateTime _modOn)
    {

        foreach (monthlyDetail md in tempList)
        {
            airController.ApproveEmployeeMonthlyDetail(md.MD_EMPLOYEE_ID, md.MD_TIME_FRAME_ID, _modBy, _modOn);
        }

    }

    protected void BtnClear_Click(object sender, EventArgs e)
    {

        CbCheckAll1095C.Checked = false;

        foreach (GridViewRow row in Gv1095.Rows)
        {

            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_1095");

            cb.Checked = false;
            cb.Enabled = true;

        }

    }

    protected void CbCheckAll1095C_CheckedChanged(object sender, EventArgs e)
    {
        checkAll();
    }

    private void loadStates(DropDownList DdlEmployerState)
    {
        DdlEmployerState.DataSource = StateController.getStates();
        DdlEmployerState.DataTextField = "State_Abbr";
        DdlEmployerState.DataValueField = "State_ID";
        DdlEmployerState.DataBind();
    }


    /// <summary>
    /// Events: RowDataBound, SelectedIndexChanging, RowCancelingEdit, PageIndexChanging, RowUpdating
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Gv1095_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);

        bool flaggedFor1095c = bool.Parse(Ddl1095Data.SelectedItem.Value);

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            GridViewRow row = e.Row;

            ImageButton ImgAddRowIII = (ImageButton)e.Row.FindControl("ImgBtnAdd1095");
            ImageButton ImgAddDependent = (ImageButton)e.Row.FindControl("ImgBtnAddDependent");

            Literal lit_hours_jan = (Literal)e.Row.FindControl("Lit_gv_hours_jan");
            Literal lit_hours_feb = (Literal)e.Row.FindControl("Lit_gv_hours_feb");
            Literal lit_hours_mar = (Literal)e.Row.FindControl("Lit_gv_hours_mar");
            Literal lit_hours_apr = (Literal)e.Row.FindControl("Lit_gv_hours_apr");
            Literal lit_hours_may = (Literal)e.Row.FindControl("Lit_gv_hours_may");
            Literal lit_hours_jun = (Literal)e.Row.FindControl("Lit_gv_hours_jun");
            Literal lit_hours_jul = (Literal)e.Row.FindControl("Lit_gv_hours_jul");
            Literal lit_hours_aug = (Literal)e.Row.FindControl("Lit_gv_hours_aug");
            Literal lit_hours_sep = (Literal)e.Row.FindControl("Lit_gv_hours_sep");
            Literal lit_hours_oct = (Literal)e.Row.FindControl("Lit_gv_hours_oct");
            Literal lit_hours_nov = (Literal)e.Row.FindControl("Lit_gv_hours_nov");
            Literal lit_hours_dec = (Literal)e.Row.FindControl("Lit_gv_hours_dec");

            DropDownList ddl_ooc_Jan = (DropDownList)row.FindControl("Ddl_gv_ooc_jan");
            DropDownList ddl_ash_Jan = (DropDownList)row.FindControl("Ddl_gv_ash_jan");
            DropDownList ddl_stat_Jan = (DropDownList)row.FindControl("Ddl_gv_status_jan");
            TextBox txt_lcmp_Jan = (TextBox)row.FindControl("Txt_gv_lcmp_jan");
            DropDownList ddl_ins_Jan = (DropDownList)row.FindControl("Ddl_gv_ins_type_jan");
            CheckBox cb_enrolled_Jan = (CheckBox)row.FindControl("Cb_gv_enrolled_jan");
            CheckBox cb_mec_Jan = (CheckBox)row.FindControl("Cb_gv_mec_jan");

            DropDownList ddl_ooc_Feb = (DropDownList)row.FindControl("Ddl_gv_ooc_feb");
            DropDownList ddl_ash_Feb = (DropDownList)row.FindControl("Ddl_gv_ash_feb");
            DropDownList ddl_stat_Feb = (DropDownList)row.FindControl("Ddl_gv_status_feb");
            TextBox txt_lcmp_Feb = (TextBox)row.FindControl("Txt_gv_lcmp_feb");
            DropDownList ddl_ins_Feb = (DropDownList)row.FindControl("Ddl_gv_ins_type_feb");
            CheckBox cb_enrolled_Feb = (CheckBox)row.FindControl("Cb_gv_enrolled_feb");
            CheckBox cb_mec_Feb = (CheckBox)row.FindControl("Cb_gv_mec_feb");

            DropDownList ddl_ooc_Mar = (DropDownList)row.FindControl("Ddl_gv_ooc_mar");
            DropDownList ddl_ash_Mar = (DropDownList)row.FindControl("Ddl_gv_ash_mar");
            DropDownList ddl_stat_Mar = (DropDownList)row.FindControl("Ddl_gv_status_mar");
            TextBox txt_lcmp_Mar = (TextBox)row.FindControl("Txt_gv_lcmp_mar");
            DropDownList ddl_ins_Mar = (DropDownList)row.FindControl("Ddl_gv_ins_type_mar");
            CheckBox cb_enrolled_Mar = (CheckBox)row.FindControl("Cb_gv_enrolled_mar");
            CheckBox cb_mec_Mar = (CheckBox)row.FindControl("Cb_gv_mec_mar");

            DropDownList ddl_ooc_Apr = (DropDownList)row.FindControl("Ddl_gv_ooc_apr");
            DropDownList ddl_ash_Apr = (DropDownList)row.FindControl("Ddl_gv_ash_apr");
            DropDownList ddl_stat_Apr = (DropDownList)row.FindControl("Ddl_gv_status_apr");
            TextBox txt_lcmp_Apr = (TextBox)row.FindControl("Txt_gv_lcmp_apr");
            DropDownList ddl_ins_Apr = (DropDownList)row.FindControl("Ddl_gv_ins_type_apr");
            CheckBox cb_enrolled_Apr = (CheckBox)row.FindControl("Cb_gv_enrolled_apr");
            CheckBox cb_mec_Apr = (CheckBox)row.FindControl("Cb_gv_mec_apr");

            DropDownList ddl_ooc_May = (DropDownList)row.FindControl("Ddl_gv_ooc_may");
            DropDownList ddl_ash_May = (DropDownList)row.FindControl("Ddl_gv_ash_may");
            DropDownList ddl_stat_May = (DropDownList)row.FindControl("Ddl_gv_status_may");
            TextBox txt_lcmp_May = (TextBox)row.FindControl("Txt_gv_lcmp_may");
            DropDownList ddl_ins_May = (DropDownList)row.FindControl("Ddl_gv_ins_type_may");
            CheckBox cb_enrolled_May = (CheckBox)row.FindControl("Cb_gv_enrolled_may");
            CheckBox cb_mec_May = (CheckBox)row.FindControl("Cb_gv_mec_may");

            DropDownList ddl_ooc_Jun = (DropDownList)row.FindControl("Ddl_gv_ooc_jun");
            DropDownList ddl_ash_Jun = (DropDownList)row.FindControl("Ddl_gv_ash_jun");
            DropDownList ddl_stat_Jun = (DropDownList)row.FindControl("Ddl_gv_status_jun");
            TextBox txt_lcmp_Jun = (TextBox)row.FindControl("Txt_gv_lcmp_jun");
            DropDownList ddl_ins_Jun = (DropDownList)row.FindControl("Ddl_gv_ins_type_jun");
            CheckBox cb_enrolled_Jun = (CheckBox)row.FindControl("Cb_gv_enrolled_jun");
            CheckBox cb_mec_Jun = (CheckBox)row.FindControl("Cb_gv_mec_jun");

            DropDownList ddl_ooc_Jul = (DropDownList)row.FindControl("Ddl_gv_ooc_jul");
            DropDownList ddl_ash_Jul = (DropDownList)row.FindControl("Ddl_gv_ash_jul");
            DropDownList ddl_stat_Jul = (DropDownList)row.FindControl("Ddl_gv_status_jul");
            TextBox txt_lcmp_Jul = (TextBox)row.FindControl("Txt_gv_lcmp_jul");
            DropDownList ddl_ins_Jul = (DropDownList)row.FindControl("Ddl_gv_ins_type_jul");
            CheckBox cb_enrolled_Jul = (CheckBox)row.FindControl("Cb_gv_enrolled_jul");
            CheckBox cb_mec_Jul = (CheckBox)row.FindControl("Cb_gv_mec_jul");

            DropDownList ddl_ooc_Aug = (DropDownList)row.FindControl("Ddl_gv_ooc_aug");
            DropDownList ddl_ash_Aug = (DropDownList)row.FindControl("Ddl_gv_ash_aug");
            DropDownList ddl_stat_Aug = (DropDownList)row.FindControl("Ddl_gv_status_aug");
            TextBox txt_lcmp_Aug = (TextBox)row.FindControl("Txt_gv_lcmp_aug");
            DropDownList ddl_ins_Aug = (DropDownList)row.FindControl("Ddl_gv_ins_type_aug");
            CheckBox cb_enrolled_Aug = (CheckBox)row.FindControl("Cb_gv_enrolled_aug");
            CheckBox cb_mec_Aug = (CheckBox)row.FindControl("Cb_gv_mec_aug");

            DropDownList ddl_ooc_Sep = (DropDownList)row.FindControl("Ddl_gv_ooc_sep");
            DropDownList ddl_ash_Sep = (DropDownList)row.FindControl("Ddl_gv_ash_sep");
            DropDownList ddl_stat_Sep = (DropDownList)row.FindControl("Ddl_gv_status_sep");
            TextBox txt_lcmp_Sep = (TextBox)row.FindControl("Txt_gv_lcmp_sep");
            DropDownList ddl_ins_Sep = (DropDownList)row.FindControl("Ddl_gv_ins_type_sep");
            CheckBox cb_enrolled_Sep = (CheckBox)row.FindControl("Cb_gv_enrolled_sep");
            CheckBox cb_mec_Sep = (CheckBox)row.FindControl("Cb_gv_mec_sep");

            DropDownList ddl_ooc_Oct = (DropDownList)row.FindControl("Ddl_gv_ooc_oct");
            DropDownList ddl_ash_Oct = (DropDownList)row.FindControl("Ddl_gv_ash_oct");
            DropDownList ddl_stat_Oct = (DropDownList)row.FindControl("Ddl_gv_status_oct");
            TextBox txt_lcmp_Oct = (TextBox)row.FindControl("Txt_gv_lcmp_oct");
            DropDownList ddl_ins_Oct = (DropDownList)row.FindControl("Ddl_gv_ins_type_oct");
            CheckBox cb_enrolled_Oct = (CheckBox)row.FindControl("Cb_gv_enrolled_oct");
            CheckBox cb_mec_Oct = (CheckBox)row.FindControl("Cb_gv_mec_oct");

            DropDownList ddl_ooc_Nov = (DropDownList)row.FindControl("Ddl_gv_ooc_nov");
            DropDownList ddl_ash_Nov = (DropDownList)row.FindControl("Ddl_gv_ash_nov");
            DropDownList ddl_stat_Nov = (DropDownList)row.FindControl("Ddl_gv_status_nov");
            TextBox txt_lcmp_Nov = (TextBox)row.FindControl("Txt_gv_lcmp_nov");
            DropDownList ddl_ins_Nov = (DropDownList)row.FindControl("Ddl_gv_ins_type_nov");
            CheckBox cb_enrolled_Nov = (CheckBox)row.FindControl("Cb_gv_enrolled_nov");
            CheckBox cb_mec_Nov = (CheckBox)row.FindControl("Cb_gv_mec_nov");

            DropDownList ddl_ooc_Dec = (DropDownList)row.FindControl("Ddl_gv_ooc_dec");
            DropDownList ddl_ash_Dec = (DropDownList)row.FindControl("Ddl_gv_ash_dec");
            DropDownList ddl_stat_Dec = (DropDownList)row.FindControl("Ddl_gv_status_dec");
            TextBox txt_lcmp_Dec = (TextBox)row.FindControl("Txt_gv_lcmp_dec");
            DropDownList ddl_ins_Dec = (DropDownList)row.FindControl("Ddl_gv_ins_type_dec");
            CheckBox cb_enrolled_Dec = (CheckBox)row.FindControl("Cb_gv_enrolled_dec");
            CheckBox cb_mec_Dec = (CheckBox)row.FindControl("Cb_gv_mec_dec");

            HiddenField hf_state_abrev = (HiddenField)e.Row.FindControl("Hf_state_abrev");
            DropDownList ddl_state = (DropDownList)row.FindControl("Ddl_gv_State");
            loadStates(ddl_state);
            ddl_state.SelectedValue = ddl_state.Items.FindByText(hf_state_abrev.Value).Value;

            HiddenField hf_emp_id = (HiddenField)e.Row.FindControl("Hv_gv_EmployeeID");
            Repeater rpt_dependents = (Repeater)e.Row.FindControl("RptDependents");

            int _employeeID = int.Parse(hf_emp_id.Value);
            List<monthlyDetail> mdList = null;

            if (flaggedFor1095c == true)
            {

                ImgAddDependent.Visible = true;
                ImgAddRowIII.Visible = true;

            }
            else
            {

                ImgAddDependent.Visible = false;
                ImgAddRowIII.Visible = false;

            }

            bindOOCddl(ddl_ooc_Jan);
            bindASHddl(ddl_ash_Jan);
            bindInsTypeddl(ddl_ins_Jan);
            bindStatusddl(ddl_stat_Jan);

            bindOOCddl(ddl_ooc_Feb);
            bindASHddl(ddl_ash_Feb);
            bindInsTypeddl(ddl_ins_Feb);
            bindStatusddl(ddl_stat_Feb);

            bindOOCddl(ddl_ooc_Mar);
            bindASHddl(ddl_ash_Mar);
            bindInsTypeddl(ddl_ins_Mar);
            bindStatusddl(ddl_stat_Mar);

            bindOOCddl(ddl_ooc_Apr);
            bindASHddl(ddl_ash_Apr);
            bindInsTypeddl(ddl_ins_Apr);
            bindStatusddl(ddl_stat_Apr);

            bindOOCddl(ddl_ooc_May);
            bindASHddl(ddl_ash_May);
            bindInsTypeddl(ddl_ins_May);
            bindStatusddl(ddl_stat_May);

            bindOOCddl(ddl_ooc_Jun);
            bindASHddl(ddl_ash_Jun);
            bindInsTypeddl(ddl_ins_Jun);
            bindStatusddl(ddl_stat_Jun);

            bindOOCddl(ddl_ooc_Jul);
            bindASHddl(ddl_ash_Jul);
            bindInsTypeddl(ddl_ins_Jul);
            bindStatusddl(ddl_stat_Jul);

            bindOOCddl(ddl_ooc_Aug);
            bindASHddl(ddl_ash_Aug);
            bindInsTypeddl(ddl_ins_Aug);
            bindStatusddl(ddl_stat_Aug);

            bindOOCddl(ddl_ooc_Sep);
            bindASHddl(ddl_ash_Sep);
            bindInsTypeddl(ddl_ins_Sep);
            bindStatusddl(ddl_stat_Sep);

            bindOOCddl(ddl_ooc_Oct);
            bindASHddl(ddl_ash_Oct);
            bindInsTypeddl(ddl_ins_Oct);
            bindStatusddl(ddl_stat_Oct);

            bindOOCddl(ddl_ooc_Nov);
            bindASHddl(ddl_ash_Nov);
            bindInsTypeddl(ddl_ins_Nov);
            bindStatusddl(ddl_stat_Nov);

            bindOOCddl(ddl_ooc_Dec);
            bindASHddl(ddl_ash_Dec);
            bindInsTypeddl(ddl_ins_Dec);
            bindStatusddl(ddl_stat_Dec);

            mdList = getMonthlyDetails(_employeeID);

            if (mdList.Count > 0)
            {

                foreach (monthlyDetail md in mdList)
                {

                    switch (md.MD_TIME_FRAME_ID - ((md.MD_TIME_FRAME_ID / 12) * 12))
                    {

                        case 1:   

                            txt_lcmp_Jan.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Jan, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Jan, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Jan, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Jan, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_jan.Text = md.MD_HOURS.ToString();
                            cb_mec_Jan.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Jan.Checked = md.MD_ENROLLED;

                            break;

                        case 2:   

                            txt_lcmp_Feb.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Feb, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Feb, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Feb, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Feb, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_feb.Text = md.MD_HOURS.ToString();
                            cb_mec_Feb.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Feb.Checked = md.MD_ENROLLED;

                            break;

                        case 3:   

                            txt_lcmp_Mar.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Mar, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Mar, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Mar, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Mar, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_mar.Text = md.MD_HOURS.ToString();
                            cb_mec_Mar.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Mar.Checked = md.MD_ENROLLED;

                            break;

                        case 4:   

                            txt_lcmp_Apr.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Apr, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Apr, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Apr, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Apr, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_apr.Text = md.MD_HOURS.ToString();
                            cb_mec_Apr.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Apr.Checked = md.MD_ENROLLED;

                            break;

                        case 5:   

                            txt_lcmp_May.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_May, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_May, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_May, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_May, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_may.Text = md.MD_HOURS.ToString();
                            cb_mec_May.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_May.Checked = md.MD_ENROLLED;

                            break;

                        case 6:   

                            txt_lcmp_Jun.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Jun, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Jun, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Jun, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Jun, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_jun.Text = md.MD_HOURS.ToString();
                            cb_mec_Jun.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Jun.Checked = md.MD_ENROLLED;

                            break;

                        case 7:   

                            txt_lcmp_Jul.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Jul, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Jul, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Jul, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Jul, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_jul.Text = md.MD_HOURS.ToString();
                            cb_mec_Jul.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Jul.Checked = md.MD_ENROLLED;

                            break;

                        case 8:   

                            txt_lcmp_Aug.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Aug, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Aug, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Aug, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Aug, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_aug.Text = md.MD_HOURS.ToString();
                            cb_mec_Aug.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Aug.Checked = md.MD_ENROLLED;

                            break;

                        case 9:   

                            txt_lcmp_Sep.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Sep, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Sep, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Sep, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Sep, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_sep.Text = md.MD_HOURS.ToString();
                            cb_mec_Sep.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Sep.Checked = md.MD_ENROLLED;

                            break;

                        case 10:   

                            txt_lcmp_Oct.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Oct, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Oct, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Oct, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Oct, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_oct.Text = md.MD_HOURS.ToString();
                            cb_mec_Oct.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Oct.Checked = md.MD_ENROLLED;

                            break;

                        case 11:   

                            txt_lcmp_Nov.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Nov, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Nov, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Nov, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Nov, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_nov.Text = md.MD_HOURS.ToString();
                            cb_mec_Nov.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Nov.Checked = md.MD_ENROLLED;

                            break;

                        case 0:               

                            txt_lcmp_Dec.Text = md.MD_LCMP.ToString();
                            errorChecking.setDropDownListByText(ddl_ash_Dec, md.MD_ASH);
                            errorChecking.setDropDownListByText(ddl_ooc_Dec, md.MD_OOC);
                            errorChecking.setDropDownList(ddl_ins_Dec, md.MD_INSURANCE_TYPE_ID);
                            errorChecking.setDropDownList(ddl_stat_Dec, md.MD_MONTHLY_STATUS_ID);
                            lit_hours_dec.Text = md.MD_HOURS.ToString();
                            cb_mec_Dec.Checked = md.MD_MEC.checkBoolNull();
                            cb_enrolled_Dec.Checked = md.MD_ENROLLED;

                            break;

                    }

                }

            }

            List<airCoverage> airIcList = airController.GetAirCoverage(_employeeID, true);

            if (airIcList.Count > 0)
            {
                int test = 0;
            }
            else
            {
                airIcList = new List<airCoverage>();
            }

            rpt_dependents.DataSource = airIcList;
            rpt_dependents.DataBind();

        }

    }

    private void EnableDisableEmployeeEdits(GridViewRow row, bool EnabledValue)
    {
        TextBox txt_FirstName = (TextBox)row.FindControl("Txt_gv_FirstName");
        TextBox txt_MiddleName = (TextBox)row.FindControl("Txt_gv_MiddleName");
        TextBox txt_LastName = (TextBox)row.FindControl("Txt_gv_LastName");
        TextBox txt_SSN = (TextBox)row.FindControl("Txt_gv_SSN");
        TextBox txt_Address = (TextBox)row.FindControl("Txt_gv_Address");
        TextBox txt_City = (TextBox)row.FindControl("Txt_gv_City");
        DropDownList ddl_State = (DropDownList)row.FindControl("Ddl_gv_State");
        TextBox txt_Zip = (TextBox)row.FindControl("Txt_gv_Zip");

        HiddenField hf_emp_id = (HiddenField)row.FindControl("Hv_gv_EmployeeID");

        DropDownList ddl_ooc_Jan = (DropDownList)row.FindControl("Ddl_gv_ooc_jan");
        DropDownList ddl_ash_Jan = (DropDownList)row.FindControl("Ddl_gv_ash_jan");
        DropDownList ddl_stat_Jan = (DropDownList)row.FindControl("Ddl_gv_status_jan");
        TextBox txt_lcmp_Jan = (TextBox)row.FindControl("Txt_gv_lcmp_jan");
        DropDownList ddl_ins_Jan = (DropDownList)row.FindControl("Ddl_gv_ins_type_jan");
        CheckBox cb_enrolled_Jan = (CheckBox)row.FindControl("Cb_gv_enrolled_jan");
        CheckBox cb_mec_Jan = (CheckBox)row.FindControl("Cb_gv_mec_jan");

        DropDownList ddl_ooc_Feb = (DropDownList)row.FindControl("Ddl_gv_ooc_feb");
        DropDownList ddl_ash_Feb = (DropDownList)row.FindControl("Ddl_gv_ash_feb");
        DropDownList ddl_stat_Feb = (DropDownList)row.FindControl("Ddl_gv_status_feb");
        TextBox txt_lcmp_Feb = (TextBox)row.FindControl("Txt_gv_lcmp_feb");
        DropDownList ddl_ins_Feb = (DropDownList)row.FindControl("Ddl_gv_ins_type_feb");
        CheckBox cb_enrolled_Feb = (CheckBox)row.FindControl("Cb_gv_enrolled_feb");
        CheckBox cb_mec_Feb = (CheckBox)row.FindControl("Cb_gv_mec_feb");

        DropDownList ddl_ooc_Mar = (DropDownList)row.FindControl("Ddl_gv_ooc_mar");
        DropDownList ddl_ash_Mar = (DropDownList)row.FindControl("Ddl_gv_ash_mar");
        DropDownList ddl_stat_Mar = (DropDownList)row.FindControl("Ddl_gv_status_mar");
        TextBox txt_lcmp_Mar = (TextBox)row.FindControl("Txt_gv_lcmp_mar");
        DropDownList ddl_ins_Mar = (DropDownList)row.FindControl("Ddl_gv_ins_type_mar");
        CheckBox cb_enrolled_Mar = (CheckBox)row.FindControl("Cb_gv_enrolled_mar");
        CheckBox cb_mec_Mar = (CheckBox)row.FindControl("Cb_gv_mec_mar");

        DropDownList ddl_ooc_Apr = (DropDownList)row.FindControl("Ddl_gv_ooc_apr");
        DropDownList ddl_ash_Apr = (DropDownList)row.FindControl("Ddl_gv_ash_apr");
        DropDownList ddl_stat_Apr = (DropDownList)row.FindControl("Ddl_gv_status_apr");
        TextBox txt_lcmp_Apr = (TextBox)row.FindControl("Txt_gv_lcmp_apr");
        DropDownList ddl_ins_Apr = (DropDownList)row.FindControl("Ddl_gv_ins_type_apr");
        CheckBox cb_enrolled_Apr = (CheckBox)row.FindControl("Cb_gv_enrolled_apr");
        CheckBox cb_mec_Apr = (CheckBox)row.FindControl("Cb_gv_mec_apr");

        DropDownList ddl_ooc_May = (DropDownList)row.FindControl("Ddl_gv_ooc_may");
        DropDownList ddl_ash_May = (DropDownList)row.FindControl("Ddl_gv_ash_may");
        DropDownList ddl_stat_May = (DropDownList)row.FindControl("Ddl_gv_status_may");
        TextBox txt_lcmp_May = (TextBox)row.FindControl("Txt_gv_lcmp_may");
        DropDownList ddl_ins_May = (DropDownList)row.FindControl("Ddl_gv_ins_type_may");
        CheckBox cb_enrolled_May = (CheckBox)row.FindControl("Cb_gv_enrolled_may");
        CheckBox cb_mec_May = (CheckBox)row.FindControl("Cb_gv_mec_may");

        DropDownList ddl_ooc_Jun = (DropDownList)row.FindControl("Ddl_gv_ooc_jun");
        DropDownList ddl_ash_Jun = (DropDownList)row.FindControl("Ddl_gv_ash_jun");
        DropDownList ddl_stat_Jun = (DropDownList)row.FindControl("Ddl_gv_status_jun");
        TextBox txt_lcmp_Jun = (TextBox)row.FindControl("Txt_gv_lcmp_jun");
        DropDownList ddl_ins_Jun = (DropDownList)row.FindControl("Ddl_gv_ins_type_jun");
        CheckBox cb_enrolled_Jun = (CheckBox)row.FindControl("Cb_gv_enrolled_jun");
        CheckBox cb_mec_Jun = (CheckBox)row.FindControl("Cb_gv_mec_jun");

        DropDownList ddl_ooc_Jul = (DropDownList)row.FindControl("Ddl_gv_ooc_jul");
        DropDownList ddl_ash_Jul = (DropDownList)row.FindControl("Ddl_gv_ash_jul");
        DropDownList ddl_stat_Jul = (DropDownList)row.FindControl("Ddl_gv_status_jul");
        TextBox txt_lcmp_Jul = (TextBox)row.FindControl("Txt_gv_lcmp_jul");
        DropDownList ddl_ins_Jul = (DropDownList)row.FindControl("Ddl_gv_ins_type_jul");
        CheckBox cb_enrolled_Jul = (CheckBox)row.FindControl("Cb_gv_enrolled_jul");
        CheckBox cb_mec_Jul = (CheckBox)row.FindControl("Cb_gv_mec_jul");

        DropDownList ddl_ooc_Aug = (DropDownList)row.FindControl("Ddl_gv_ooc_aug");
        DropDownList ddl_ash_Aug = (DropDownList)row.FindControl("Ddl_gv_ash_aug");
        DropDownList ddl_stat_Aug = (DropDownList)row.FindControl("Ddl_gv_status_aug");
        TextBox txt_lcmp_Aug = (TextBox)row.FindControl("Txt_gv_lcmp_aug");
        DropDownList ddl_ins_Aug = (DropDownList)row.FindControl("Ddl_gv_ins_type_aug");
        CheckBox cb_enrolled_Aug = (CheckBox)row.FindControl("Cb_gv_enrolled_aug");
        CheckBox cb_mec_Aug = (CheckBox)row.FindControl("Cb_gv_mec_aug");

        DropDownList ddl_ooc_Sep = (DropDownList)row.FindControl("Ddl_gv_ooc_sep");
        DropDownList ddl_ash_Sep = (DropDownList)row.FindControl("Ddl_gv_ash_sep");
        DropDownList ddl_stat_Sep = (DropDownList)row.FindControl("Ddl_gv_status_sep");
        TextBox txt_lcmp_Sep = (TextBox)row.FindControl("Txt_gv_lcmp_sep");
        DropDownList ddl_ins_Sep = (DropDownList)row.FindControl("Ddl_gv_ins_type_sep");
        CheckBox cb_enrolled_Sep = (CheckBox)row.FindControl("Cb_gv_enrolled_sep");
        CheckBox cb_mec_Sep = (CheckBox)row.FindControl("Cb_gv_mec_sep");

        DropDownList ddl_ooc_Oct = (DropDownList)row.FindControl("Ddl_gv_ooc_oct");
        DropDownList ddl_ash_Oct = (DropDownList)row.FindControl("Ddl_gv_ash_oct");
        DropDownList ddl_stat_Oct = (DropDownList)row.FindControl("Ddl_gv_status_oct");
        TextBox txt_lcmp_Oct = (TextBox)row.FindControl("Txt_gv_lcmp_oct");
        DropDownList ddl_ins_Oct = (DropDownList)row.FindControl("Ddl_gv_ins_type_oct");
        CheckBox cb_enrolled_Oct = (CheckBox)row.FindControl("Cb_gv_enrolled_oct");
        CheckBox cb_mec_Oct = (CheckBox)row.FindControl("Cb_gv_mec_oct");

        DropDownList ddl_ooc_Nov = (DropDownList)row.FindControl("Ddl_gv_ooc_nov");
        DropDownList ddl_ash_Nov = (DropDownList)row.FindControl("Ddl_gv_ash_nov");
        DropDownList ddl_stat_Nov = (DropDownList)row.FindControl("Ddl_gv_status_nov");
        TextBox txt_lcmp_Nov = (TextBox)row.FindControl("Txt_gv_lcmp_nov");
        DropDownList ddl_ins_Nov = (DropDownList)row.FindControl("Ddl_gv_ins_type_nov");
        CheckBox cb_enrolled_Nov = (CheckBox)row.FindControl("Cb_gv_enrolled_nov");
        CheckBox cb_mec_Nov = (CheckBox)row.FindControl("Cb_gv_mec_nov");

        DropDownList ddl_ooc_Dec = (DropDownList)row.FindControl("Ddl_gv_ooc_dec");
        DropDownList ddl_ash_Dec = (DropDownList)row.FindControl("Ddl_gv_ash_dec");
        DropDownList ddl_stat_Dec = (DropDownList)row.FindControl("Ddl_gv_status_dec");
        TextBox txt_lcmp_Dec = (TextBox)row.FindControl("Txt_gv_lcmp_dec");
        DropDownList ddl_ins_Dec = (DropDownList)row.FindControl("Ddl_gv_ins_type_dec");
        CheckBox cb_enrolled_Dec = (CheckBox)row.FindControl("Cb_gv_enrolled_dec");
        CheckBox cb_mec_Dec = (CheckBox)row.FindControl("Cb_gv_mec_dec");

        ImageButton ib_edit = (ImageButton)row.FindControl("ImgBtnEdit1095");
        ImageButton ib_save = (ImageButton)row.FindControl("ImgBtnSave1095");
        ImageButton ib_cancel = (ImageButton)row.FindControl("ImgBtnCancel");
        ImageButton ib_all12 = (ImageButton)row.FindControl("ImgBtnAll12");

        txt_FirstName.Enabled = EnabledValue;
        txt_MiddleName.Enabled = EnabledValue;
        txt_LastName.Enabled = EnabledValue;
        txt_SSN.Enabled = EnabledValue;
        txt_Address.Enabled = EnabledValue;
        txt_City.Enabled = EnabledValue;
        ddl_State.Enabled = EnabledValue;
        txt_Zip.Enabled = EnabledValue;

        ddl_ooc_Jan.Enabled = EnabledValue;
        ddl_ooc_Feb.Enabled = EnabledValue;
        ddl_ooc_Mar.Enabled = EnabledValue;
        ddl_ooc_Apr.Enabled = EnabledValue;
        ddl_ooc_May.Enabled = EnabledValue;
        ddl_ooc_Jun.Enabled = EnabledValue;
        ddl_ooc_Jul.Enabled = EnabledValue;
        ddl_ooc_Aug.Enabled = EnabledValue;
        ddl_ooc_Sep.Enabled = EnabledValue;
        ddl_ooc_Oct.Enabled = EnabledValue;
        ddl_ooc_Nov.Enabled = EnabledValue;
        ddl_ooc_Dec.Enabled = EnabledValue;

        ddl_ash_Jan.Enabled = EnabledValue;
        ddl_ash_Feb.Enabled = EnabledValue;
        ddl_ash_Mar.Enabled = EnabledValue;
        ddl_ash_Apr.Enabled = EnabledValue;
        ddl_ash_May.Enabled = EnabledValue;
        ddl_ash_Jun.Enabled = EnabledValue;
        ddl_ash_Jul.Enabled = EnabledValue;
        ddl_ash_Aug.Enabled = EnabledValue;
        ddl_ash_Sep.Enabled = EnabledValue;
        ddl_ash_Oct.Enabled = EnabledValue;
        ddl_ash_Nov.Enabled = EnabledValue;
        ddl_ash_Dec.Enabled = EnabledValue;

        ddl_stat_Jan.Enabled = EnabledValue;
        ddl_stat_Feb.Enabled = EnabledValue;
        ddl_stat_Mar.Enabled = EnabledValue;
        ddl_stat_Apr.Enabled = EnabledValue;
        ddl_stat_May.Enabled = EnabledValue;
        ddl_stat_Jun.Enabled = EnabledValue;
        ddl_stat_Jul.Enabled = EnabledValue;
        ddl_stat_Aug.Enabled = EnabledValue;
        ddl_stat_Sep.Enabled = EnabledValue;
        ddl_stat_Oct.Enabled = EnabledValue;
        ddl_stat_Nov.Enabled = EnabledValue;
        ddl_stat_Dec.Enabled = EnabledValue;

        txt_lcmp_Jan.Enabled = EnabledValue;
        txt_lcmp_Feb.Enabled = EnabledValue;
        txt_lcmp_Mar.Enabled = EnabledValue;
        txt_lcmp_Apr.Enabled = EnabledValue;
        txt_lcmp_May.Enabled = EnabledValue;
        txt_lcmp_Jun.Enabled = EnabledValue;
        txt_lcmp_Jul.Enabled = EnabledValue;
        txt_lcmp_Aug.Enabled = EnabledValue;
        txt_lcmp_Sep.Enabled = EnabledValue;
        txt_lcmp_Oct.Enabled = EnabledValue;
        txt_lcmp_Nov.Enabled = EnabledValue;
        txt_lcmp_Dec.Enabled = EnabledValue;

        ddl_ins_Jan.Enabled = EnabledValue;
        ddl_ins_Feb.Enabled = EnabledValue;
        ddl_ins_Mar.Enabled = EnabledValue;
        ddl_ins_Apr.Enabled = EnabledValue;
        ddl_ins_May.Enabled = EnabledValue;
        ddl_ins_Jun.Enabled = EnabledValue;
        ddl_ins_Jul.Enabled = EnabledValue;
        ddl_ins_Aug.Enabled = EnabledValue;
        ddl_ins_Sep.Enabled = EnabledValue;
        ddl_ins_Oct.Enabled = EnabledValue;
        ddl_ins_Nov.Enabled = EnabledValue;
        ddl_ins_Dec.Enabled = EnabledValue;

        cb_enrolled_Jan.Enabled = EnabledValue;
        cb_enrolled_Feb.Enabled = EnabledValue;
        cb_enrolled_Mar.Enabled = EnabledValue;
        cb_enrolled_Apr.Enabled = EnabledValue;
        cb_enrolled_May.Enabled = EnabledValue;
        cb_enrolled_Jun.Enabled = EnabledValue;
        cb_enrolled_Jul.Enabled = EnabledValue;
        cb_enrolled_Aug.Enabled = EnabledValue;
        cb_enrolled_Sep.Enabled = EnabledValue;
        cb_enrolled_Oct.Enabled = EnabledValue;
        cb_enrolled_Nov.Enabled = EnabledValue;
        cb_enrolled_Dec.Enabled = EnabledValue;

        cb_mec_Jan.Enabled = EnabledValue;
        cb_mec_Feb.Enabled = EnabledValue;
        cb_mec_Mar.Enabled = EnabledValue;
        cb_mec_Apr.Enabled = EnabledValue;
        cb_mec_May.Enabled = EnabledValue;
        cb_mec_Jun.Enabled = EnabledValue;
        cb_mec_Jul.Enabled = EnabledValue;
        cb_mec_Aug.Enabled = EnabledValue;
        cb_mec_Sep.Enabled = EnabledValue;
        cb_mec_Oct.Enabled = EnabledValue;
        cb_mec_Nov.Enabled = EnabledValue;
        cb_mec_Dec.Enabled = EnabledValue;

        cb_enrolled_Jan.Enabled = EnabledValue;
        cb_enrolled_Feb.Enabled = EnabledValue;
        cb_enrolled_Mar.Enabled = EnabledValue;
        cb_enrolled_Apr.Enabled = EnabledValue;
        cb_enrolled_May.Enabled = EnabledValue;
        cb_enrolled_Jun.Enabled = EnabledValue;
        cb_enrolled_Jul.Enabled = EnabledValue;
        cb_enrolled_Aug.Enabled = EnabledValue;
        cb_enrolled_Sep.Enabled = EnabledValue;
        cb_enrolled_Oct.Enabled = EnabledValue;
        cb_enrolled_Nov.Enabled = EnabledValue;
        cb_enrolled_Dec.Enabled = EnabledValue;

        ib_edit.Visible = false == EnabledValue;
        ib_all12.Visible = false == EnabledValue;
        ib_save.Visible = EnabledValue;
        ib_cancel.Visible = EnabledValue;
    }

    protected void Gv1095_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridViewRow row = Gv1095.Rows[e.NewSelectedIndex];

        EnableDisableEmployeeEdits(row, true);
    }

    protected void Gv1095_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

        GridViewRow row = Gv1095.Rows[e.RowIndex];

        EnableDisableEmployeeEdits(row, false);

        Gv1095.SelectedIndex = -1;

        filterEmployees();
        loadEmployees();
    }

    protected void Gv1095_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        GridViewRow row = Gv1095.Rows[Gv1095.SelectedIndex];
        bool validTransaction = true;

        HiddenField hf_emp_id = (HiddenField)row.FindControl("Hv_gv_EmployeeID");
        int _employeeID = int.Parse(hf_emp_id.Value);

        TextBox txt_FirstName = (TextBox)row.FindControl("Txt_gv_FirstName");
        TextBox txt_MiddleName = (TextBox)row.FindControl("Txt_gv_MiddleName");
        TextBox txt_LastName = (TextBox)row.FindControl("Txt_gv_LastName");
        TextBox txt_SSN = (TextBox)row.FindControl("Txt_gv_SSN");
        TextBox txt_Address = (TextBox)row.FindControl("Txt_gv_Address");
        TextBox txt_City = (TextBox)row.FindControl("Txt_gv_City");
        DropDownList ddl_State = (DropDownList)row.FindControl("Ddl_gv_State");
        TextBox txt_Zip = (TextBox)row.FindControl("Txt_gv_Zip");

        if (txt_FirstName.Text.IsNullOrEmpty()
            || txt_LastName.Text.IsNullOrEmpty())
        {
            MpeWebMessage.Show();
            LitMessage.Text = "Employee First and Last Name must be set.";
            return;
        }

        string ssn = null;
        if (txt_SSN.Text.Contains("*"))
        {     
            ssn = null;
        }
        else if (false == txt_SSN.Text.IsValidSsn())
        {
            MpeWebMessage.Show();
            LitMessage.Text = "Must enter a valid SSN.";
            return;
        }
        else
        {
            ssn = txt_SSN.Text.ZeroPadSsn();
        }

        if (ddl_State.SelectedIndex < 0
            || false == txt_Zip.Text.ZeroPadZip().IsValidZipCode()
            || txt_Address.Text.IsNullOrEmpty()
            || txt_City.Text.IsNullOrEmpty())
        {
            MpeWebMessage.Show();
            LitMessage.Text = "You must enter a valid Address, City, State, Zip code.";
            return;
        }

        if (false == EmployeeController.UpdateEmployeeInfo_ACA_AIR(_employeeID, txt_FirstName.Text, txt_MiddleName.Text, txt_LastName.Text, ddl_State.SelectedValue, txt_Address.Text, txt_City.Text, txt_Zip.Text.ZeroPadZip(), ssn, ((User)Session["CurrentUser"]).User_ID))
        {
            MpeWebMessage.Show();
            LitMessage.Text = "Failed to update Employee Personal Information.";
            return;
        }


        DropDownList ddl_ooc_Jan = (DropDownList)row.FindControl("Ddl_gv_ooc_jan");
        DropDownList ddl_ash_Jan = (DropDownList)row.FindControl("Ddl_gv_ash_jan");
        DropDownList ddl_stat_Jan = (DropDownList)row.FindControl("Ddl_gv_status_jan");
        TextBox txt_lcmp_Jan = (TextBox)row.FindControl("Txt_gv_lcmp_jan");
        DropDownList ddl_ins_Jan = (DropDownList)row.FindControl("Ddl_gv_ins_type_jan");
        CheckBox cb_enrolled_Jan = (CheckBox)row.FindControl("Cb_gv_enrolled_jan");
        CheckBox cb_mec_Jan = (CheckBox)row.FindControl("Cb_gv_mec_jan");

        DropDownList ddl_ooc_Feb = (DropDownList)row.FindControl("Ddl_gv_ooc_feb");
        DropDownList ddl_ash_Feb = (DropDownList)row.FindControl("Ddl_gv_ash_feb");
        DropDownList ddl_stat_Feb = (DropDownList)row.FindControl("Ddl_gv_status_feb");
        TextBox txt_lcmp_Feb = (TextBox)row.FindControl("Txt_gv_lcmp_feb");
        DropDownList ddl_ins_Feb = (DropDownList)row.FindControl("Ddl_gv_ins_type_feb");
        CheckBox cb_enrolled_Feb = (CheckBox)row.FindControl("Cb_gv_enrolled_feb");
        CheckBox cb_mec_Feb = (CheckBox)row.FindControl("Cb_gv_mec_feb");

        DropDownList ddl_ooc_Mar = (DropDownList)row.FindControl("Ddl_gv_ooc_mar");
        DropDownList ddl_ash_Mar = (DropDownList)row.FindControl("Ddl_gv_ash_mar");
        DropDownList ddl_stat_Mar = (DropDownList)row.FindControl("Ddl_gv_status_mar");
        TextBox txt_lcmp_Mar = (TextBox)row.FindControl("Txt_gv_lcmp_mar");
        DropDownList ddl_ins_Mar = (DropDownList)row.FindControl("Ddl_gv_ins_type_mar");
        CheckBox cb_enrolled_Mar = (CheckBox)row.FindControl("Cb_gv_enrolled_mar");
        CheckBox cb_mec_Mar = (CheckBox)row.FindControl("Cb_gv_mec_mar");

        DropDownList ddl_ooc_Apr = (DropDownList)row.FindControl("Ddl_gv_ooc_apr");
        DropDownList ddl_ash_Apr = (DropDownList)row.FindControl("Ddl_gv_ash_apr");
        DropDownList ddl_stat_Apr = (DropDownList)row.FindControl("Ddl_gv_status_apr");
        TextBox txt_lcmp_Apr = (TextBox)row.FindControl("Txt_gv_lcmp_apr");
        DropDownList ddl_ins_Apr = (DropDownList)row.FindControl("Ddl_gv_ins_type_apr");
        CheckBox cb_enrolled_Apr = (CheckBox)row.FindControl("Cb_gv_enrolled_apr");
        CheckBox cb_mec_Apr = (CheckBox)row.FindControl("Cb_gv_mec_apr");

        DropDownList ddl_ooc_May = (DropDownList)row.FindControl("Ddl_gv_ooc_may");
        DropDownList ddl_ash_May = (DropDownList)row.FindControl("Ddl_gv_ash_may");
        DropDownList ddl_stat_May = (DropDownList)row.FindControl("Ddl_gv_status_may");
        TextBox txt_lcmp_May = (TextBox)row.FindControl("Txt_gv_lcmp_may");
        DropDownList ddl_ins_May = (DropDownList)row.FindControl("Ddl_gv_ins_type_may");
        CheckBox cb_enrolled_May = (CheckBox)row.FindControl("Cb_gv_enrolled_may");
        CheckBox cb_mec_May = (CheckBox)row.FindControl("Cb_gv_mec_may");

        DropDownList ddl_ooc_Jun = (DropDownList)row.FindControl("Ddl_gv_ooc_jun");
        DropDownList ddl_ash_Jun = (DropDownList)row.FindControl("Ddl_gv_ash_jun");
        DropDownList ddl_stat_Jun = (DropDownList)row.FindControl("Ddl_gv_status_jun");
        TextBox txt_lcmp_Jun = (TextBox)row.FindControl("Txt_gv_lcmp_jun");
        DropDownList ddl_ins_Jun = (DropDownList)row.FindControl("Ddl_gv_ins_type_jun");
        CheckBox cb_enrolled_Jun = (CheckBox)row.FindControl("Cb_gv_enrolled_jun");
        CheckBox cb_mec_Jun = (CheckBox)row.FindControl("Cb_gv_mec_jun");

        DropDownList ddl_ooc_Jul = (DropDownList)row.FindControl("Ddl_gv_ooc_jul");
        DropDownList ddl_ash_Jul = (DropDownList)row.FindControl("Ddl_gv_ash_jul");
        DropDownList ddl_stat_Jul = (DropDownList)row.FindControl("Ddl_gv_status_jul");
        TextBox txt_lcmp_Jul = (TextBox)row.FindControl("Txt_gv_lcmp_jul");
        DropDownList ddl_ins_Jul = (DropDownList)row.FindControl("Ddl_gv_ins_type_jul");
        CheckBox cb_enrolled_Jul = (CheckBox)row.FindControl("Cb_gv_enrolled_jul");
        CheckBox cb_mec_Jul = (CheckBox)row.FindControl("Cb_gv_mec_jul");

        DropDownList ddl_ooc_Aug = (DropDownList)row.FindControl("Ddl_gv_ooc_aug");
        DropDownList ddl_ash_Aug = (DropDownList)row.FindControl("Ddl_gv_ash_aug");
        DropDownList ddl_stat_Aug = (DropDownList)row.FindControl("Ddl_gv_status_aug");
        TextBox txt_lcmp_Aug = (TextBox)row.FindControl("Txt_gv_lcmp_aug");
        DropDownList ddl_ins_Aug = (DropDownList)row.FindControl("Ddl_gv_ins_type_aug");
        CheckBox cb_enrolled_Aug = (CheckBox)row.FindControl("Cb_gv_enrolled_aug");
        CheckBox cb_mec_Aug = (CheckBox)row.FindControl("Cb_gv_mec_aug");

        DropDownList ddl_ooc_Sep = (DropDownList)row.FindControl("Ddl_gv_ooc_sep");
        DropDownList ddl_ash_Sep = (DropDownList)row.FindControl("Ddl_gv_ash_sep");
        DropDownList ddl_stat_Sep = (DropDownList)row.FindControl("Ddl_gv_status_sep");
        TextBox txt_lcmp_Sep = (TextBox)row.FindControl("Txt_gv_lcmp_sep");
        DropDownList ddl_ins_Sep = (DropDownList)row.FindControl("Ddl_gv_ins_type_sep");
        CheckBox cb_enrolled_Sep = (CheckBox)row.FindControl("Cb_gv_enrolled_sep");
        CheckBox cb_mec_Sep = (CheckBox)row.FindControl("Cb_gv_mec_sep");

        DropDownList ddl_ooc_Oct = (DropDownList)row.FindControl("Ddl_gv_ooc_oct");
        DropDownList ddl_ash_Oct = (DropDownList)row.FindControl("Ddl_gv_ash_oct");
        DropDownList ddl_stat_Oct = (DropDownList)row.FindControl("Ddl_gv_status_oct");
        TextBox txt_lcmp_Oct = (TextBox)row.FindControl("Txt_gv_lcmp_oct");
        DropDownList ddl_ins_Oct = (DropDownList)row.FindControl("Ddl_gv_ins_type_oct");
        CheckBox cb_enrolled_Oct = (CheckBox)row.FindControl("Cb_gv_enrolled_oct");
        CheckBox cb_mec_Oct = (CheckBox)row.FindControl("Cb_gv_mec_oct");

        DropDownList ddl_ooc_Nov = (DropDownList)row.FindControl("Ddl_gv_ooc_nov");
        DropDownList ddl_ash_Nov = (DropDownList)row.FindControl("Ddl_gv_ash_nov");
        DropDownList ddl_stat_Nov = (DropDownList)row.FindControl("Ddl_gv_status_nov");
        TextBox txt_lcmp_Nov = (TextBox)row.FindControl("Txt_gv_lcmp_nov");
        DropDownList ddl_ins_Nov = (DropDownList)row.FindControl("Ddl_gv_ins_type_nov");
        CheckBox cb_enrolled_Nov = (CheckBox)row.FindControl("Cb_gv_enrolled_nov");
        CheckBox cb_mec_Nov = (CheckBox)row.FindControl("Cb_gv_mec_nov");

        DropDownList ddl_ooc_Dec = (DropDownList)row.FindControl("Ddl_gv_ooc_dec");
        DropDownList ddl_ash_Dec = (DropDownList)row.FindControl("Ddl_gv_ash_dec");
        DropDownList ddl_stat_Dec = (DropDownList)row.FindControl("Ddl_gv_status_dec");
        TextBox txt_lcmp_Dec = (TextBox)row.FindControl("Txt_gv_lcmp_dec");
        DropDownList ddl_ins_Dec = (DropDownList)row.FindControl("Ddl_gv_ins_type_dec");
        CheckBox cb_enrolled_Dec = (CheckBox)row.FindControl("Cb_gv_enrolled_dec");
        CheckBox cb_mec_Dec = (CheckBox)row.FindControl("Cb_gv_mec_dec");

        ImageButton ib_edit = (ImageButton)row.FindControl("ImgBtnEdit1095");
        ImageButton ib_save = (ImageButton)row.FindControl("ImgBtnSave1095");
        ImageButton ib_all12 = (ImageButton)row.FindControl("ImgBtnAll12");

        ImageButton ib_cancel = (ImageButton)row.FindControl("ImgBtnCancel");

        bool validData = true;
        int _taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);
        int _employerID = int.Parse(HfDistrictID.Value);

        validData = airController.validateEmployeeTaxYearInAIR(_employeeID, _taxYear);

        //************************ New code to handle editing the ALL SELECTS/Employees left out of AIR. 
        validData = airController.validateEmployeeTaxYearInAIR(_employeeID, _taxYear);

        if (validData == false)
        {
            validTransaction = airController.runETL_Build_MissingEmployee(_employerID, _employeeID, _taxYear);

            if (validTransaction == true)
            {
                validData = true;
            }
            else
            {
                validData = false;
            }
        }
        //*********************** End of new code for ALL SELECTS/Employees *****************************

        if (validData == true)
        {
            bool cleanData = true;

            List<int> timeFrames = airController.manufactureTimeFrameList(_taxYear, true);

            foreach (int i in timeFrames)
            {
                switch (i - ((i / 12) * 12))
                {

                    case 1:   

                        Boolean jan = updateEmployeeDetail(_employeeID, i, ddl_ins_Jan, ddl_ooc_Jan, ddl_ash_Jan, ddl_stat_Jan, cb_enrolled_Jan, cb_mec_Jan, txt_lcmp_Jan);
                        if (jan == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 2:   

                        Boolean feb = updateEmployeeDetail(_employeeID, i, ddl_ins_Feb, ddl_ooc_Feb, ddl_ash_Feb, ddl_stat_Feb, cb_enrolled_Feb, cb_mec_Feb, txt_lcmp_Feb);
                        if (feb == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 3:   

                        Boolean mar = updateEmployeeDetail(_employeeID, i, ddl_ins_Mar, ddl_ooc_Mar, ddl_ash_Mar, ddl_stat_Mar, cb_enrolled_Mar, cb_mec_Mar, txt_lcmp_Mar);
                        if (mar == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 4:   

                        Boolean apr = updateEmployeeDetail(_employeeID, i, ddl_ins_Apr, ddl_ooc_Apr, ddl_ash_Apr, ddl_stat_Apr, cb_enrolled_Apr, cb_mec_Apr, txt_lcmp_Apr);
                        if (apr == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 5:   

                        Boolean may = updateEmployeeDetail(_employeeID, i, ddl_ins_May, ddl_ooc_May, ddl_ash_May, ddl_stat_May, cb_enrolled_May, cb_mec_May, txt_lcmp_May);
                        if (may == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 6:   

                        Boolean jun = updateEmployeeDetail(_employeeID, i, ddl_ins_Jun, ddl_ooc_Jun, ddl_ash_Jun, ddl_stat_Jun, cb_enrolled_Jun, cb_mec_Jun, txt_lcmp_Jun);
                        if (jun == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 7:   

                        Boolean jul = updateEmployeeDetail(_employeeID, i, ddl_ins_Jul, ddl_ooc_Jul, ddl_ash_Jul, ddl_stat_Jul, cb_enrolled_Jul, cb_mec_Jul, txt_lcmp_Jul);
                        if (jul == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 8:   

                        Boolean aug = updateEmployeeDetail(_employeeID, i, ddl_ins_Aug, ddl_ooc_Aug, ddl_ash_Aug, ddl_stat_Aug, cb_enrolled_Aug, cb_mec_Aug, txt_lcmp_Aug);
                        if (aug == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 9:   

                        Boolean sep = updateEmployeeDetail(_employeeID, i, ddl_ins_Sep, ddl_ooc_Sep, ddl_ash_Sep, ddl_stat_Sep, cb_enrolled_Sep, cb_mec_Sep, txt_lcmp_Sep);
                        if (sep == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 10:      

                        Boolean oct = updateEmployeeDetail(_employeeID, i, ddl_ins_Oct, ddl_ooc_Oct, ddl_ash_Oct, ddl_stat_Oct, cb_enrolled_Oct, cb_mec_Oct, txt_lcmp_Oct);
                        if (oct == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 11:      

                        Boolean nov = updateEmployeeDetail(_employeeID, i, ddl_ins_Nov, ddl_ooc_Nov, ddl_ash_Nov, ddl_stat_Nov, cb_enrolled_Nov, cb_mec_Nov, txt_lcmp_Nov);
                        if (nov == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    case 0:               

                        Boolean dec = updateEmployeeDetail(_employeeID, i, ddl_ins_Dec, ddl_ooc_Dec, ddl_ash_Dec, ddl_stat_Dec, cb_enrolled_Dec, cb_mec_Dec, txt_lcmp_Dec);
                        if (dec == false)
                        {
                            validTransaction = false;
                        }

                        break;

                    default:

                        validTransaction = false;

                        this.Log.Warn(String.Format("Saving records did not complete, unknown time id: {0}!", i));

                        break;

                }

            }

            if (validTransaction == false)
            {

                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while updating.";

            }
            else
            {
                airController.Recalculate1095Status(_employerID, _employeeID, _taxYear);

                filterEmployees();
                loadEmployees();
                MpeWebMessage.Show();
                LitMessage.Text = "The individual data has been saved.";
            }

        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "This record cannot currently be saved, the appropriate team has been notified.";
        }
    }

    protected void Gv1095_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {

            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            HiddenField hf_emp_id = (HiddenField)row.FindControl("Hv_gv_EmployeeID");
            TextBox txt_emp_Name = (TextBox)row.FindControl("Txt_gv_FirstName");
            int _employeeID = int.Parse(hf_emp_id.Value);

            switch (e.CommandName)
            {

                case "PartIII":

                    Txt_E_EmployeeID.Text = _employeeID.ToString();
                    Txt_E_Name.Text = txt_emp_Name.Text;
                    Txt_E_TaxYear.Text = DdlCalendarYear.SelectedItem.Value.ToString();

                    loadDependents(_employeeID);

                    Cb_E_All12.Checked = false;
                    cb_E_Jan.Checked = false;
                    cb_E_Feb.Checked = false;
                    cb_E_Mar.Checked = false;
                    cb_E_Apr.Checked = false;
                    cb_E_May.Checked = false;
                    cb_E_Jun.Checked = false;
                    cb_E_Jul.Checked = false;
                    cb_E_Aug.Checked = false;
                    cb_E_Sept.Checked = false;
                    cb_E_Oct.Checked = false;
                    cb_E_Nov.Checked = false;
                    cb_E_Dec.Checked = false;

                    MpePartIII.Show();
                    break;

                case "Dependent":

                    Txt_D_EmployeeID.Text = _employeeID.ToString();
                    Txt_D_EmployeeName.Text = txt_emp_Name.Text;

                    Txt_D_FirstName.Text = null;
                    Txt_D_LastName.Text = null;
                    Txt_D_SSN.Text = null;
                    Txt_D_DOB.Text = null;

                    MpeDependent.Show();

                    break;

                case "All12":

                    lit_all12_EmployeeID.Text = _employeeID.ToString();
                    Lit_all12_Name.Text = txt_emp_Name.Text;
                    bindASHddl(Ddl_all12_line16);
                    bindInsTypeddl(Ddl_all12_InsuranceType);
                    bindOOCddl(Ddl_all12_line14);
                    bindStatusddl(Ddl_all12_MonthlyStatus);
                    MpeAll12.Show();

                    break;

                default:
                    break;

            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

    }

    private void loadDependents(int _employeeID)
    {

        List<Dependent> dependents = EmployeeController.manufactureEmployeeDependentList(_employeeID);
        Ddl_E_dependent.DataSource = dependents;
        Ddl_E_dependent.DataTextField = "DEPENDENT_FULL_NAME";
        Ddl_E_dependent.DataValueField = "DEPENDENT_ID";
        Ddl_E_dependent.DataBind();

        string allDependentIds = string.Empty;
        foreach (Dependent depend in dependents)
        {
            allDependentIds += depend.DEPENDENT_ID + ", ";
        }

        PIILogger.LogPII(string.Format("Dependent Data being shown to User Id:[{0}], at IP:[{1}], for employee ID:[{2}]'s Dependents with Ids:[{3}]", ((User)Session["CurrentUser"]).User_ID, Request.UserHostAddress, _employeeID, allDependentIds));

        Ddl_E_dependent.Items.Add("Select");
        Ddl_E_dependent.SelectedIndex = Ddl_E_dependent.Items.Count - 1;

    }

    protected void RptDependents_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        HiddenField hfRowID = (HiddenField)e.Item.FindControl("Hf_dep_rowID");
        HiddenField hfEmployeeID = (HiddenField)e.Item.FindControl("Hf_rpt_EmployeeID");
        TextBox txtFname = (TextBox)e.Item.FindControl("Txt_dep_Fname");
        TextBox txtLname = (TextBox)e.Item.FindControl("Txt_dep_Lname");
        TextBox txtSSN = (TextBox)e.Item.FindControl("Txt_dep_ssn");
        TextBox txtDob = (TextBox)e.Item.FindControl("Txt_dep_dob");
        ImageButton imgBtn = (ImageButton)e.Item.FindControl("ImgBtnEdit1095");
        ImageButton imgBtn2 = (ImageButton)e.Item.FindControl("ImgBtnSave1095");
        ImageButton imgBtn3 = (ImageButton)e.Item.FindControl("ImgBtnCancel");
        ImageButton imgBtn4 = (ImageButton)e.Item.FindControl("ImgBtnDelete1095");
        CheckBox cbJan = (CheckBox)e.Item.FindControl("cb_jan");
        CheckBox cbFeb = (CheckBox)e.Item.FindControl("cb_feb");
        CheckBox cbMar = (CheckBox)e.Item.FindControl("cb_mar");
        CheckBox cbApr = (CheckBox)e.Item.FindControl("cb_apr");
        CheckBox cbMay = (CheckBox)e.Item.FindControl("cb_may");
        CheckBox cbJun = (CheckBox)e.Item.FindControl("cb_jun");
        CheckBox cbJul = (CheckBox)e.Item.FindControl("cb_jul");
        CheckBox cbAug = (CheckBox)e.Item.FindControl("cb_aug");
        CheckBox cbSep = (CheckBox)e.Item.FindControl("cb_sep");
        CheckBox cbOct = (CheckBox)e.Item.FindControl("cb_oct");
        CheckBox cbNov = (CheckBox)e.Item.FindControl("cb_nov");
        CheckBox cbDec = (CheckBox)e.Item.FindControl("cb_dec");

        int _taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);
        int _employeeID = 0;
        int _employerID = int.Parse(HfDistrictID.Value);
        int _dependentID = 0;
        string _fName = null;
        string _mName = null;
        string _lName = null;
        string _ssn = null;
        DateTime? _dob = null;
        bool _all12 = false;
        bool _jan = false;
        bool _feb = false;
        bool _mar = false;
        bool _apr = false;
        bool _may = false;
        bool _jun = false;
        bool _jul = false;
        bool _aug = false;
        bool _sep = false;
        bool _oct = false;
        bool _nov = false;
        bool _dec = false;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;

        switch (e.CommandName)
        {

            case "Edit":

                txtFname.Enabled = true;
                txtLname.Enabled = true;
                txtSSN.Enabled = true;
                txtDob.Enabled = true;

                cbJan.Enabled = true;
                cbFeb.Enabled = true;
                cbMar.Enabled = true;
                cbApr.Enabled = true;
                cbMay.Enabled = true;
                cbJun.Enabled = true;
                cbJul.Enabled = true;
                cbAug.Enabled = true;
                cbSep.Enabled = true;
                cbOct.Enabled = true;
                cbNov.Enabled = true;
                cbDec.Enabled = true;

                imgBtn.Visible = false;
                imgBtn4.Visible = false;
                imgBtn2.Visible = true;
                imgBtn3.Visible = true;

                break;

            case "Cancel":

                txtFname.Enabled = true;
                txtLname.Enabled = true;
                txtSSN.Enabled = false;
                txtDob.Enabled = false;

                cbJan.Enabled = false;
                cbFeb.Enabled = false;
                cbMar.Enabled = false;
                cbApr.Enabled = false;
                cbMay.Enabled = false;
                cbJun.Enabled = false;
                cbJul.Enabled = false;
                cbAug.Enabled = false;
                cbSep.Enabled = false;
                cbOct.Enabled = false;
                cbNov.Enabled = false;
                cbDec.Enabled = false;

                imgBtn.Visible = true;
                imgBtn2.Visible = false;
                imgBtn3.Visible = false;
                imgBtn4.Visible = true;

                filterEmployees();
                loadEmployees();

                break;

            case "Delete":

                int _rowid = int.Parse(hfRowID.Value);

                Boolean secondRemove = airController.RemoveCoveredIndividual(_rowid);

                if (secondRemove)
                {

                    _employeeID = int.Parse(hfEmployeeID.Value);

                    airController.Recalculate1095Status(_employerID, _employeeID, _taxYear);

                    filterEmployees();
                    loadEmployees();

                }
                else
                {

                    this.Log.Error(String.Format("Unable to remove covered individual row {0} using the airController.", _rowid));

                    MpeWebMessage.Show();
                    LitMessage.Text = "An error occured during the removal of the covered individual record. The appropiate team has been notified of the issue.";

                }

                break;

            case "Update":

                bool validTransaction = true;
                bool validData = true;
                string _history = "Mod By: " + LitUserName.Text + " Mod On: " + DateTime.Now.ToString();
                try
                {

                    int _coveredIndividualID = int.Parse(hfRowID.Value);
                    _employeeID = int.Parse(hfEmployeeID.Value);
                    validData = validatePartIIIrow(txtFname, txtLname, txtSSN, txtDob);
                    validTransaction = false;

                    if (validData == true)
                    {

                        _fName = null;
                        _lName = null;
                        _ssn = null;
                        string dob = null;
                        _dob = null;
                        _dependentID = 0;

                        _jan = cbJan.Checked;
                        _feb = cbFeb.Checked;
                        _mar = cbMar.Checked;
                        _apr = cbApr.Checked;
                        _may = cbMay.Checked;
                        _jun = cbJun.Checked;
                        _jul = cbJul.Checked;
                        _aug = cbAug.Checked;
                        _sep = cbSep.Checked;
                        _oct = cbOct.Checked;
                        _nov = cbNov.Checked;
                        _dec = cbDec.Checked;

                        if (_jan == true && _feb == true && _mar == true && _apr == true && _may == true && _jun == true && _jul == true && _aug == true && _sep == true && _oct == true && _nov == true && _dec == true)
                        {
                            _all12 = true;
                        }


                        _fName = txtFname.Text;
                        _lName = txtLname.Text;

                        if (errorChecking.validateTextBoxSSN(txtSSN, true) == true)
                        {
                            _ssn = txtSSN.Text.Trim(' ');
                        }
                        else
                        {
                            txtSSN.BackColor = System.Drawing.Color.White;
                            _ssn = null;
                        }
                        if (errorChecking.validateTextBoxDate(txtDob, true) == true)
                        {
                            
                            _dob = DateTime.Parse(txtDob.Text);
                        }
                        else
                        {
                            txtDob.BackColor = System.Drawing.Color.White;
                            _dob = null;
                        }

                        insurance_coverage tempIC = null;
                        List<insurance_coverage> icTempList = insuranceController.manufactureAllEditableInsuranceCoverage(_employeeID, _taxYear);

                        foreach (insurance_coverage ic in icTempList)
                        {
                            if (ic.ROW_ID == _coveredIndividualID)
                            {
                                tempIC = ic;
                                break;
                            }
                        }

                        if (tempIC != null)
                        {
                            if (tempIC.IC_DEPENDENT_ID == null)
                            {
                                _dependentID = 0;
                            }
                            else
                            {
                                _dependentID = int.Parse(tempIC.IC_DEPENDENT_ID.ToString());
                            }


                            if (_dependentID == 0)
                            {
                                if (_ssn == null && _dob != null)
                                {
                                    validTransaction = EmployeeController.updateEmployeeLineIII_DOB(_employeeID, _modOn, _modBy, (DateTime)_dob, _fName, _lName);
                                    Log.Info("validTransaction set to [" + validTransaction + "]. DOB update, SSN == null");
                                    insuranceController.updateInsuranceCoverageRow(_coveredIndividualID, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, _history);
                                }
                                else if (_dob == null && _ssn != null)
                                {
                                    validTransaction = EmployeeController.updateEmployeeLineIII_SSN(_employeeID, _modOn, _modBy, _ssn, _fName, _lName);
                                    Log.Info("validTransaction set to [" + validTransaction + "]. SSN update, DOB == null");
                                    insuranceController.updateInsuranceCoverageRow(_coveredIndividualID, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, _history);
                                }
                                else
                                {
                                    Log.Info("validTransaction set to false. DOB: [" + _dob + "], SSN.IsNullOrEmpty [" + _ssn.IsNullOrEmpty() + "]");
                                    validTransaction = false;
                                }

                            }
                            else
                            {
                                bool validMonthUpdate = insuranceController.updateInsuranceCoverageRow(_coveredIndividualID, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, _history);
                                Dependent tempD = EmployeeController.updateEmployeeDependent(_dependentID, _employeeID, _fName, _mName, _lName, _ssn, _dob, _modBy, 1);
                                bool isValid = EmployeeController.AddDependentCoverage(_taxYear);
                                if (tempD != null && validMonthUpdate && isValid)
                                {
                                    Log.Info("validTransaction set to true. ");
                                    validTransaction = true;
                                }
                                else
                                {
                                    Log.Info("validTransaction set to false. Update Dependent Info. Valid update month [" + validMonthUpdate + "], isValid [" + isValid + "], tempD [" + tempD + "]");
                                    validTransaction = false;
                                }
                            }

                            if (validTransaction == true)
                            {

                                airController.runETL_ShortBuild(_employerID, _employeeID, _taxYear);

                                airController.Recalculate1095Status(_employerID, _employeeID, _taxYear);

                                loadEmployees();

                            }

                        }

                    }

                }
                catch (Exception exception)
                {

                    Log.Info("validTransaction set to false. Due to exception.", exception);
                    validTransaction = false;

                }

                if (validTransaction == true)
                {

                    loadEmployees();
                    MpeWebMessage.Show();
                    LitMessage.Text = "The records has been updated.";

                }
                else
                {

                    MpeWebMessage.Show();
                    LitMessage.Text = "An error occurred while trying to update the Individual information.";

                }

                break;

            case "New":

                break;
        }

    }

    protected void Btn_E_Save_Click(object sender, EventArgs e)
    {

        int _dependentID = 0;
        MpePartIII.Hide();
        if (Rb_E_employee.Checked == true)
        {
            generateIndividualPartIII(_dependentID);
        }

        if (Rb_E_dependent.Checked == true)
        {

            bool validData = true;

            validData = errorChecking.validateDropDownSelection(Ddl_E_dependent, validData);
            if (validData == true)
            {
                _dependentID = int.Parse(Ddl_E_dependent.SelectedItem.Value);
                generateIndividualPartIII(_dependentID);
            }
            else
            {
                MpePartIII.Show();
            }

        }

    }

    private bool validatePartIIIrow(TextBox txtFname, TextBox txtLname, TextBox txtSSN, TextBox txtDob)
    {

        bool validData = true;
        bool validSSN = true;
        bool validDOB = true;
        bool requireSSN = false;
        bool requireDOB = false;

        validData = errorChecking.validateTextBoxNull(txtFname, validData);
        validData = errorChecking.validateTextBoxNull(txtLname, validData);

        if (txtSSN.Text.Length > 0) { requireSSN = true; }
        if (txtDob.Text.Length > 0) { requireDOB = true; }

        if (requireDOB == true && requireSSN == false)
        {
            validDOB = errorChecking.validateTextBoxDate(txtDob, validDOB);
            if (validDOB == false)
            {
                validData = false;
            }
        }
        else if (requireDOB == false && requireSSN == true)
        {
            validSSN = errorChecking.validateTextBoxSSN(txtSSN, validSSN);
            if (validSSN == false)
            {
                validData = false;
            }
        }
        else
        {
            validDOB = errorChecking.validateTextBoxDate(txtDob, validDOB);
            validSSN = errorChecking.validateTextBoxSSN(txtSSN, validSSN);
            if (validDOB == false || validSSN == false)
            {
                validData = false;
            }
        }

        return validData;
    }

    private bool generateIndividualPartIII(int dependentID)
    {

        int _employerID = 0;
        int _employeeID = 0;
        int _dependentID = dependentID;      
        int _carrierID = 1012;                 
        int _taxYear = 0;
        bool _all12 = false;
        bool _jan = false;
        bool _feb = false;
        bool _mar = false;
        bool _apr = false;
        bool _may = false;
        bool _jun = false;
        bool _jul = false;
        bool _aug = false;
        bool _sept = false;
        bool _oct = false;
        bool _nov = false;
        bool _dec = false;
        string _history = "Created on " + DateTime.Now.ToString() + " by " + LitUserName.Text;

        _employerID = int.Parse(HfDistrictID.Value);
        _employeeID = int.Parse(Txt_E_EmployeeID.Text);
        _taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);
        _all12 = Cb_E_All12.Checked;

        if (_all12 == true)
        {
            _jan = true;
            _feb = true;
            _mar = true;
            _apr = true;
            _may = true;
            _jun = true;
            _jul = true;
            _aug = true;
            _sept = true;
            _oct = true;
            _nov = true;
            _dec = true;
        }
        else
        {
            _jan = cb_E_Jan.Checked;
            _feb = cb_E_Feb.Checked;
            _mar = cb_E_Mar.Checked;
            _apr = cb_E_Apr.Checked;
            _may = cb_E_May.Checked;
            _jun = cb_E_Jun.Checked;
            _jul = cb_E_Jul.Checked;
            _aug = cb_E_Aug.Checked;
            _sept = cb_E_Sept.Checked;
            _oct = cb_E_Oct.Checked;
            _nov = cb_E_Nov.Checked;
            _dec = cb_E_Dec.Checked;
        }

        bool validTransaction = false;
        bool validETLShortBuild = false;
        validTransaction = insuranceController.InsertNewInsuranceCoverageRow(_taxYear, _employeeID, _employerID, _dependentID, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sept, _oct, _nov, _dec);
        validETLShortBuild = airController.runETL_ShortBuild(_employerID, _employeeID, _taxYear);

        airController.Recalculate1095Status(_employerID, _employeeID, _taxYear);

        loadEmployees();

        return validETLShortBuild;

    }

    protected void Rb_E_employee_CheckedChanged(object sender, EventArgs e)
    {

        if (Rb_E_employee.Checked == true)
        {

            Pnl_E_Dependent.Visible = false;
            MpePartIII.Show();
            Ddl_E_dependent.Enabled = false;

        }

    }

    protected void Rb_E_dependent_CheckedChanged(object sender, EventArgs e)
    {

        if (Rb_E_dependent.Checked == true)
        {

            Pnl_E_Dependent.Visible = true;
            MpePartIII.Show();
            Ddl_E_dependent.Enabled = true;

        }

    }

    protected void Btn_D_Save_Click(object sender, EventArgs e)
    {

        bool validData = true;
        int _employeeID = 0;
        int _dependentID = 0;
        string _fName = null;
        string _mName = null;
        string _lName = null;
        string _ssn = null;
        DateTime? _dob = null;
        int _taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);
        string modby = LitUserName.Text;

        validData = validatePartIIIrow(Txt_D_FirstName, Txt_D_LastName, Txt_D_SSN, Txt_D_DOB);

        if (validData == true)
        {
            _fName = Txt_D_FirstName.Text;
            _lName = Txt_D_LastName.Text;
            _employeeID = int.Parse(Txt_D_EmployeeID.Text);
            if (errorChecking.validateTextBoxSSN(Txt_D_SSN, true) == true)
            {
                _ssn = Txt_D_SSN.Text.Trim(' ');
            }
            else
            {
                Txt_D_SSN.BackColor = System.Drawing.Color.White;
                _ssn = null;
            }
            if (errorChecking.validateTextBoxDate(Txt_D_DOB, true) == true)
            {
                _dob = DateTime.Parse(Txt_D_DOB.Text);
            }
            else
            {
                Txt_D_DOB.BackColor = System.Drawing.Color.White;
                _dob = null;
            }

            Dependent dependent = EmployeeController.updateEmployeeDependent(_dependentID, _employeeID, _fName, _mName, _lName, _ssn, _dob, modby, 1);
            bool isValid = EmployeeController.AddDependentCoverage(_taxYear);
            if (isValid)
            {

                int _employerID = int.Parse(HfDistrictID.Value);

                if (_dependentID == 0)
                {

                    insuranceController.InsertNewInsuranceCoverageRow(
                            _taxYear,
                             _employeeID,
                             _employerID,
                             dependent.DEPENDENT_ID,
                             false,
                             false,
                             false,
                             false,
                             false,
                             false,
                             false,
                             false,
                             false,
                             false,
                             false,
                             false
                        );

                    airController.runETL_ShortBuild(_employerID, _employeeID, _taxYear);

                    airController.Recalculate1095Status(_employerID, _employeeID, _taxYear);

                }

                loadEmployees();

                PIILogger.LogPII("Transfering to dependent over to insurance coverage editable for py: " + _taxYear + " dep id: " + dependent.DEPENDENT_ID);

            }
            else
            {
                Log.Warn("Transfering error on trying to put dependent over to insurance coverage editable.");
            }
        }
        else
        {
            MpeDependent.Show();
        }

    }

    private void bindOOCddl(DropDownList ddl)
    {

        List<ooc> oocs = new List<ooc>();

        oocs = airController.ManufactureOOCList(true);

        if (int.Parse(DdlCalendarYear.SelectedItem.Value) == 2016)
        {

            ooc ooc_ = (from _ooc in oocs
                        where _ooc.OOC_ID == "1I"
                        select _ooc
                        ).SingleOrDefault();

            if (ooc_ != null)
            {
                oocs.Remove(ooc_);
            }

        }

        ddl.DataSource = oocs;
        ddl.DataTextField = "OOC_ID";
        ddl.DataValueField = "OOC_ID";
        ddl.DataBind();

        ddl.Items.Add("Select");
        ddl.SelectedIndex = ddl.Items.Count - 1;

    }

    private void bindStatusddl(DropDownList ddl)
    {

        ddl.DataSource = airController.ManufactureStatusList(true);
        ddl.DataTextField = "STATUS_NAME";
        ddl.DataValueField = "STATUS_ID";
        ddl.DataBind();

        ddl.Items.Add("Select");
        ddl.SelectedIndex = ddl.Items.Count - 1;

    }

    private void bindInsTypeddl(DropDownList ddl)
    {

        ddl.DataSource = insuranceController.GetInsuranceTypes(true);
        ddl.DataTextField = "INSURANCE_TYPE_NAME";
        ddl.DataValueField = "INSURANCE_TYPE_ID";
        ddl.DataBind();

        ddl.Items.Add("Select");
        ddl.SelectedIndex = ddl.Items.Count - 1;

    }

    private void bindASHddl(DropDownList ddl)
    {

        List<ash> ashCodes = new List<ash>();

        ashCodes = airController.ManufactureASHList(true);

        if (int.Parse(DdlCalendarYear.SelectedItem.Value) == 2016)
        {

            ash ash_ = (from _ash in ashCodes
                        where _ash.ASH_ID == "2I"
                        select _ash
                        ).SingleOrDefault();

            if (ash_ != null)
            {
                ashCodes.Remove(ash_);
            }

        }

        ddl.DataSource = ashCodes;
        ddl.DataTextField = "ASH_ID";
        ddl.DataValueField = "ASH_ID";
        ddl.DataBind();

        ddl.Items.Add("Select");

        ddl.SelectedIndex = ddl.Items.Count - 1;

    }

    protected void BtnApplyFilters_Click(object sender, EventArgs e)
    {

        int rows = 0;
        int pageIndex = 0;
        int pageSize = int.Parse(DdlPageSize.SelectedItem.Value);
        filterEmployees();
        loadEmployees();
        List<Employee> tempList = (List<Employee>)Session["Employees1095Filtered"];
        rows = tempList.Count;
        DataBind_ddlPageNumber(pageIndex, pageSize, rows);
    }

    /// <summary>
    /// Search by Last Name or Payroll ID. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnFindEmployees_Click(object sender, EventArgs e)
    {

        int taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);
        int employerId = int.Parse(HfDistrictID.Value);

        List<Employee> tempList = EmployeeController.EmployeesPending1095Approval(employerId, taxYear, true);
        List<Employee> filteredList = new List<Employee>();
        String searchText = null;
        Boolean validData = true;
        int rows = 0;
        int pageIndex = 0;
        int pageSize = int.Parse(DdlPageSize.SelectedItem.Value);

        validData = errorChecking.validateTextBoxNull(Txt_f_searchValue, validData);

        if (validData == true)
        {

            searchText = Txt_f_searchValue.Text;

            if (RbLastName.Checked == true)
            {

                foreach (Employee emp in tempList)
                {

                    if (emp.EMPLOYEE_LAST_NAME.ToUpper().Contains(searchText.ToUpper()))
                    {
                        filteredList.Add(emp);
                    }

                }

            }

            if (RbPayrollID.Checked == true)
            {
                foreach (Employee emp in tempList)
                {
                    if (emp.EMPLOYEE_EXT_ID.ToUpper().Contains(searchText.ToUpper()))
                    {
                        filteredList.Add(emp);
                    }
                }
            }
        }

        Session["Employees1095Filtered"] = filteredList;

        string allIds = string.Empty;
        foreach (Employee emp in filteredList)
        {
            allIds += emp.EMPLOYEE_ID + ", ";
        }
        PIILogger.LogPII(string.Format("1095 Form is displaying information to UserId:[{0}], at IP[{1}] for Employees with Ids:[{2}]", ((User)Session["CurrentUser"]).User_ID, Request.UserHostAddress, allIds));

        Gv1095.DataSource = filteredList;
        Gv1095.DataBind();
        rows = filteredList.Count;
        DataBind_ddlPageNumber(pageIndex, pageSize, rows);

    }

    private void filterEmployees()
    {

        List<Employee> tempList = new List<Employee>();
        List<Employee> filteredList = new List<Employee>();
        List<Employee> filteredList2 = new List<Employee>();
        List<Employee> filteredList3 = new List<Employee>();
        List<Employee> filteredList4 = new List<Employee>();
        List<int> employeesInCarrierReport = new List<int>();
        List<int> employeesInYearlyDetail = new List<int>();

        int _employerID = int.Parse(HfDistrictID.Value);
        int _taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);

        employeesInYearlyDetail = airController.GetEmployeesReceiving1095(_employerID, _taxYear, true);

        employeesInCarrierReport = EmployeeController.GetEmployeesInInsuranceCarrierImport(_employerID, _taxYear, true);

        tempList = EmployeeController.EmployeesPending1095Approval(_employerID, _taxYear, true);

        bool filterbyEBCdata = true;
        filterbyEBCdata = errorChecking.validateDropDownSelectionNoRed(Ddl1095Data, filterbyEBCdata);
        if (filterbyEBCdata == true)
        {

            bool value = bool.Parse(Ddl1095Data.SelectedItem.Value);

            if (value == true)
            {

                foreach (Employee emp in tempList)
                {

                    foreach (int i in employeesInYearlyDetail)
                    {

                        if (i == emp.EMPLOYEE_ID)
                        {
                            emp.EMPLOYEE_REC_1095c = true;
                            filteredList.Add(emp);
                            break;
                        }
                    }
                }
            }
            else
            {

                foreach (Employee emp in tempList)
                {

                    bool found = false;

                    foreach (int i in employeesInYearlyDetail)
                    {

                        if (i == emp.EMPLOYEE_ID)
                        {

                            found = true;

                            break;

                        }

                    }

                    if (found == false)
                    {
                        emp.EMPLOYEE_REC_1095c = false;
                        filteredList.Add(emp);
                    }

                }

            }

        }
        else
        {
            filteredList = tempList;
        }

        bool filterByIcReport = true;
        filterByIcReport = errorChecking.validateDropDownSelectionNoRed(DdlFilterList, filterByIcReport);
        if (filterByIcReport == true)
        {
            bool value = bool.Parse(DdlFilterList.SelectedItem.Value);

            if (value == true)
            {
                foreach (Employee emp in filteredList)
                {
                    foreach (int i in employeesInCarrierReport)
                    {
                        if (i == emp.EMPLOYEE_ID)
                        {
                            filteredList2.Add(emp);
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (Employee emp in filteredList)
                {
                    bool found = false;
                    foreach (int i in employeesInCarrierReport)
                    {
                        if (i == emp.EMPLOYEE_ID)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found == false)
                    {
                        filteredList2.Add(emp);
                    }
                }
            }
        }
        else
        {
            filteredList2 = filteredList;
        }

        bool filterByMismatchedRecords = true;
        filterByMismatchedRecords = errorChecking.validateDropDownSelectionNoRed(DdlMismatchedData, filterByMismatchedRecords);
        if (filterByMismatchedRecords == true)
        {

            foreach (Employee emp in filteredList2)
            {

                List<monthlyDetail> tempMonthlyDetail = getMonthlyDetails(emp.EMPLOYEE_ID);
                List<insurance_coverage> icListE = insuranceController.ManufactureEmployeeInsuranceCoverage(emp.EMPLOYEE_ID, _taxYear, true);
                insurance_coverage mergedIC = new insurance_coverage();

                if (icListE.Count > 0)
                {

                    foreach (insurance_coverage ic in icListE)
                    {

                        mergedIC.ROW_ID = ic.ROW_ID;
                        if (ic.IC_JAN == true) { mergedIC.IC_JAN = true; };
                        if (ic.IC_FEB == true) { mergedIC.IC_FEB = true; };
                        if (ic.IC_MAR == true) { mergedIC.IC_MAR = true; };
                        if (ic.IC_APR == true) { mergedIC.IC_APR = true; };
                        if (ic.IC_MAY == true) { mergedIC.IC_MAY = true; };
                        if (ic.IC_JUN == true) { mergedIC.IC_JUN = true; };
                        if (ic.IC_JUL == true) { mergedIC.IC_JUL = true; };
                        if (ic.IC_AUG == true) { mergedIC.IC_AUG = true; };
                        if (ic.IC_SEP == true) { mergedIC.IC_SEP = true; };
                        if (ic.IC_OCT == true) { mergedIC.IC_OCT = true; };
                        if (ic.IC_NOV == true) { mergedIC.IC_NOV = true; };
                        if (ic.IC_DEC == true) { mergedIC.IC_DEC = true; };

                    }

                }

                if (mergedIC.ROW_ID != 0)
                {
                    bool matchingRecords = true;
                    if (tempMonthlyDetail.Count > 0)
                    {
                        foreach (monthlyDetail md in tempMonthlyDetail)
                        {
                            switch (md.MD_TIME_FRAME_ID - ((md.MD_TIME_FRAME_ID / 12) * 12))
                            {
                                case 1:   
                                    if (md.MD_ENROLLED != mergedIC.IC_JAN) { matchingRecords = false; }
                                    break;
                                case 2:   
                                    if (md.MD_ENROLLED != mergedIC.IC_FEB) { matchingRecords = false; }
                                    break;
                                case 3:   
                                    if (md.MD_ENROLLED != mergedIC.IC_MAR) { matchingRecords = false; }
                                    break;
                                case 4:   
                                    if (md.MD_ENROLLED != mergedIC.IC_APR) { matchingRecords = false; }
                                    break;
                                case 5:   
                                    if (md.MD_ENROLLED != mergedIC.IC_MAY) { matchingRecords = false; }
                                    break;
                                case 6:   
                                    if (md.MD_ENROLLED != mergedIC.IC_JUN) { matchingRecords = false; }
                                    break;
                                case 7:   
                                    if (md.MD_ENROLLED != mergedIC.IC_JUL) { matchingRecords = false; }
                                    break;
                                case 8:   
                                    if (md.MD_ENROLLED != mergedIC.IC_AUG) { matchingRecords = false; }
                                    break;
                                case 9:   
                                    if (md.MD_ENROLLED != mergedIC.IC_SEP) { matchingRecords = false; }
                                    break;
                                case 10:   
                                    if (md.MD_ENROLLED != mergedIC.IC_OCT) { matchingRecords = false; }
                                    break;
                                case 11:   
                                    if (md.MD_ENROLLED != mergedIC.IC_NOV) { matchingRecords = false; }
                                    break;
                                case 0:               
                                    if (md.MD_ENROLLED != mergedIC.IC_DEC) { matchingRecords = false; }
                                    break;
                            }
                        }
                        if (matchingRecords == false)
                        {
                            filteredList3.Add(emp);
                        }
                    }
                    else
                    {
                        filteredList3.Add(emp);
                    }
                }
            }
        }
        else
        {
            filteredList3 = filteredList2;
        }

        bool filterByEmployeeClass = true;
        filterByEmployeeClass = errorChecking.validateDropDownSelectionNoRed(Ddl_f_EmployeeClass, filterByEmployeeClass);
        if (filterByEmployeeClass == true)
        {

            int classID = int.Parse(Ddl_f_EmployeeClass.SelectedItem.Value);

            foreach (Employee emp in filteredList3)
            {

                if (emp.EMPLOYEE_CLASS_ID == classID)
                {
                    filteredList4.Add(emp);
                }

            }

        }
        else
        {
            filteredList4 = filteredList3;
        }

        Session["Employees1095Filtered"] = filteredList4;

    }

    private void DataBind_ddlPageNumber(int pageIndex, int pageSize, int totalRows)
    {

        int totalPages = (int)Math.Ceiling((double)totalRows / (double)pageSize);

        if (totalPages > 1)
        {

            DdlPageNumber.Enabled = true;
            DdlPageNumber.Items.Clear();

            for (int i = 1; i <= totalPages; i++)
            {
                DdlPageNumber.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        else
        {
            DdlPageNumber.Enabled = false;
            DdlPageNumber.SelectedIndex = -1;
        }

    }

    protected void DdlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

        int pageSize = int.Parse(DdlPageSize.SelectedValue);
        int pageIndex = 0;
        Gv1095.PageSize = pageSize;
        Gv1095.PageIndex = pageIndex;

        loadEmployees();

        List<Employee> tempList = (List<Employee>)Session["Employees1095Filtered"];

        int rows = tempList.Count;

        DataBind_ddlPageNumber(pageIndex, pageSize, rows);

    }

    protected void DdlPageNumber_SelectedIndexChanged(object sender, EventArgs e)
    {

        int pageIndex = int.Parse(DdlPageNumber.SelectedValue) - 1;

        Gv1095.PageIndex = pageIndex;

        loadEmployees();

    }

    protected void DdlSorting_SelectedIndexChanged(object sender, EventArgs e)
    {
        sortEmployees();
    }

    private void sortEmployees()
    {

        String sortBy = DdlSorting.SelectedItem.Value.ToString();

        List<Employee> tempList = (List<Employee>)Session["Employees1095Filtered"];

        try
        {

            switch (sortBy)
            {

                case "ASC":

                    tempList = tempList.OrderBy(o => o.EMPLOYEE_LAST_NAME).ToList();

                    break;

                case "DESC":

                    tempList.Reverse();

                    break;

                default:

                    break;

            }

            Session["Employees1095Filtered"] = tempList;

            string allIds = string.Empty;
            foreach (Employee emp in tempList)
            {
                allIds += emp.EMPLOYEE_ID + ", ";
            }
            PIILogger.LogPII(string.Format("1095 Form is displaying information to UserId:[{0}], at IP[{1}] for Employees with Ids:[{2}]", ((User)Session["CurrentUser"]).User_ID, Request.UserHostAddress, allIds));

            Gv1095.DataSource = tempList;
            Gv1095.DataBind();

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

    }

    protected void Btn_all12_save_Click(object sender, EventArgs e)
    {

        int failedUpdates = 0;
        int _taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);
        List<int> timeFrames = airController.manufactureTimeFrameList(_taxYear, true);
        bool validTransaction = true;
        bool validUpdate = false;
        int _employeeID = int.Parse(lit_all12_EmployeeID.Text);
        int _employerID = int.Parse(HfDistrictID.Value);

        //************************ New code to handle editing the ALL SELECTS/Employees left out of AIR. 
        bool validData = airController.validateEmployeeTaxYearInAIR(_employeeID, _taxYear);

        if (validData == false)
        {

            validTransaction = airController.runETL_Build_MissingEmployee(_employerID, _employeeID, _taxYear);

            if (validTransaction == true)
            {
                validData = true;
            }
            else
            {
                validData = false;
            }
        }
        //*********************** End of new code for ALL SELECTS/Employees *****************************

        if (validData == true)
        {

            foreach (int i in timeFrames)
            {

                validUpdate = updateEmployeeDetail(
                        _employeeID,
                        i,
                        Ddl_all12_InsuranceType,
                        Ddl_all12_line14,
                        Ddl_all12_line16,
                        Ddl_all12_MonthlyStatus,
                        Cb_all12_Enrolled,
                        Cb_all12_MEC,
                        Txt_all12_lcmp
                    );

                if (validUpdate == false)
                {

                    failedUpdates++;
                    validTransaction = false;
                }
            }

            if (validTransaction == false)
            {

                MpeAll12.Show();
                Lit_all12_message.Text = "An error occurred while updating. " + failedUpdates + " Months Failed to Update.";

            }
            else
            {

                airController.Recalculate1095Status(_employerID, _employeeID, _taxYear);

                filterEmployees();
                loadEmployees();
                LitMessage.Text = "The individual data has been saved.";
                MpeWebMessage.Show();

            }

        }
        else
        {

            LitMessage.Text = "This record cannot currently be saved, the appropriate team has been notified.";
            MpeWebMessage.Show();

        }

    }

    protected void ImgBtnExportCSV_Click(object sender, EventArgs e)
    {
        int _taxYear = int.Parse(DdlCalendarYear.SelectedItem.Value);
        int _employerID = int.Parse(HfDistrictID.Value);

        DataTable export = employerController.ExportCSV(_employerID, _taxYear);

        string filename = "IRSInfo";
        String attachment = "attachment; filename=" + filename.CleanFileName() + ".csv";

        Response.ClearContent();
        Response.BufferOutput = false;
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.ms-excel";

        Response.Write(export.GetAsCsv());

        Response.Flush();         
        Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        Response.End();
    }

    protected void BtnUploadFile_Click(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        String ftpPath = Server.MapPath("~\\ftps\\");
        long millis = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;

        if (IRSFile.HasFile)
        {
            String fileExtension = System.IO.Path.GetExtension(IRSFile.FileName);

            if (String.Compare(fileExtension, ".csv", true) == 0)
            {
                String IRSPath = ftpPath + "Archive\\" + _employerID + "_IRSUploadFiles_" + millis + ".csv";

                IRSFile.SaveAs(IRSPath);

                PIILogger.LogPII(string.Format("User [{0}] Imported IRSFiles [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, IRSFile.FileName, IRSPath));

                String postConvertPath = String.Format("{0}{1}_{2}_IRSUploadFiles.csv", ftpPath, _employerID, millis);

                bool validTransaction = FileImport(IRSPath);

                if (validTransaction == true)
                {
                    PIILogger.LogPII(string.Format("User [{0}] Uploaded IRS File [{1}] to [{2}]",
                        ((User)Session["CurrentUser"]).User_UserName, IRSPath, postConvertPath));

                    filterEmployees();
                    loadEmployees();

                }
                else
                {
                    LitMessage.Text = "An error occured while importing the file. Please try again.";
                    MpeWebMessage.Show();
                }
            }
            else
            {
                LitMessage.Text = "Please upload .csv files only.";
                MpeWebMessage.Show();
            }
        }

    }

    protected bool FileImport(string filePath)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        DataTable dt = new DataTable();
        string[] csvRows = System.IO.File.ReadAllLines(filePath);
        string[] headers = CsvParse.SplitRow(csvRows[0]);
        headers = CsvHelper.SanitizeHeaderValues(headers);
        bool validTransaction = true;

        try
        {
            foreach (String header in headers)
            {
                dt.Columns.Add(header);
            }

            foreach (string row in csvRows.Skip(1))
            {
                DateTimeStyles styles = DateTimeStyles.None;
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                String[] formats = { "yyyy/MM/dd", "MM/dd/yyyy hh:mm" };
                String[] rowValues = CsvParse.SplitRow(row);
                DataRow dataRow = dt.NewRow();

                for (int i = 0; i < rowValues.Length; i++)
                {
                    dataRow[headers[i]] = rowValues[i];
                }

                dt.Rows.Add(dataRow);
            }

            int countColumns = dt.Columns.Count;
            List<string> containName = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                string name;
                string columnName;
                columnName = column.ToString();
                name = column.ToString().ToLower();
                name = RemoveSpecialCharacters(name);
                if (ContainsColumnName(name))
                {
                    containName.Add(columnName);
                }
            }

            foreach (string elements in containName)
            {
                dt.Columns.Remove(elements.ToString());
            }

            dt.Columns.Add("EmployerId");
            foreach (DataRow row in dt.Rows)
            {
                row["EmployerId"] = _employerID;
            }

            List<ooc> oocs = airController.ManufactureOOCList(true);
            List<ash> ashCodes = airController.ManufactureASHList(true);

            foreach (DataRow row in dt.Rows)
            {

                int empId = 0;
                if (int.TryParse(row["EmployeeId"].ToString(), out empId))
                {
                    List<monthlyDetail> monthly = airController.ManufactureEmployeeMonthlyDetailList(empId, false);

                    int monthId = 0;
                    if (int.TryParse(row["Month"].ToString(), out monthId))
                    {

                        int adjustedMonthId = monthId + 24;
                        foreach (monthlyDetail month in monthly)
                        {

                            if (month.MD_TIME_FRAME_ID != adjustedMonthId)
                            { continue; }        


                            string line14 = null;
                            if (false == row["Line14OfferOfCoverageCode"].ToString().IsNullOrEmpty())
                            {
                                line14 = row["Line14OfferOfCoverageCode"].ToString();
                                if (oocs.Where(oo => oo.OOC_ID.ToUpper() == line14.ToUpper()).Count() <= 0)
                                {
                                    line14 = null;
                                }
                            }

                            decimal? line15 = null;
                            if (false == row["Line15Premium"].ToString().IsNullOrEmpty())
                            {
                                decimal line15parse = 0;
                                if (decimal.TryParse(row["Line15Premium"].ToString(), out line15parse))
                                {
                                    line15 = line15parse;
                                }
                            }

                            string line16 = null;
                            if (false == row["Line16SafeHarborCode"].ToString().IsNullOrEmpty())
                            {
                                line16 = row["Line16SafeHarborCode"].ToString();
                                if (ashCodes.Where(ash => ash.ASH_ID.ToUpper() == line16.ToUpper()).Count() <= 0)
                                {
                                    line16 = null;
                                }
                            }

                            if (false == airController.UpdateEmployeeMonthlyDetailList(
                                month.MD_EMPLOYEE_ID,
                                month.MD_TIME_FRAME_ID,
                                month.MD_EMPLOYER_ID,
                                line14,
                                line15,
                                line16,
                                LitUserName.Text,
                                DateTime.Now,
                                month.MD_HOURS,
                                month.MD_ENROLLED,
                                month.MD_MEC,
                                month.MD_MONTHLY_STATUS_ID,
                                month.MD_INSURANCE_TYPE_ID,
                                false))
                            {

                                Log.Error(String.Format("Failed to update MOnthly Details from IRS Import. EMPLOYEE_ID: [{0}], TIME_FRAME_ID: [{1}], EMPLOYER_ID: [{2}]",
                                            month.MD_EMPLOYEE_ID,
                                            month.MD_TIME_FRAME_ID,
                                            month.MD_EMPLOYER_ID));
                            }


                        }


                    }


                }


            }


        }
        catch (Exception exception)
        {

            this.Log.Warn("Supressing errors.", exception);

            validTransaction = false;

        }

        return validTransaction;

    }

    protected static bool ContainsColumnName(string name)
    {
        List<string> list = new List<string>();
        list.Add("employername");
        list.Add("ein");
        list.Add("employeraddress");
        list.Add("employercity");
        list.Add("employercode");
        list.Add("employerzip");
        list.Add("employertelephone");
        foreach (string element in list)
        {
            if (element.ToString() == name)
                return true;
        }
        return false;
    }

    protected static string RemoveSpecialCharacters(string input)
    {
        Regex r = new Regex("(?:[^a-z]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        return r.Replace(input, String.Empty);
    }

    private ILog Log = LogManager.GetLogger(typeof(approval_1095));

}