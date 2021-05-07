using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;
using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.Domain;

public partial class securepages_mass_insurance_offering : Afas.AfComply.UI.securepages.SecurePageBase
{

    private ILog Log = LogManager.GetLogger(typeof(securepages_mass_insurance_offering));

    protected string DisclaimerMessage = "IMPORTANT: the file you are about to export may have been " +
"pre-populated with data from your prior year offer file. " +
"You must review the accuracy and completeness of this information " +
"to ensure accurate tracking and/or reporting results. " +
"BE SURE to make any changes necessary to bring the " +
"file up to date before resubmitting it to your "
+ Branding.ProductName +
@" team.
This file contains Social Security Numbers and I understand it must be handled with care.";

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        DataBind();

        HfUName.Value = user.User_Full_Name;

        HfDistrictID.Value = user.User_District_ID.ToString();

        loadHRStatus();
        loadEmployeeClassifications();
        loadPlanYears();
        loadAlertDetails();
    }

    protected void Cb_f_Classification_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_Classification.Checked == true)
        {
            Ddl_f_Classification.Enabled = true;
            Ddl_f_Classification.SelectedIndex = Ddl_f_Classification.Items.Count - 1;
        }
        else
        {
            Ddl_f_Classification.Enabled = false;
            Ddl_f_Classification.SelectedIndex = Ddl_f_Classification.Items.Count - 1;
        }
    }

    protected void Cb_f_HrStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_HrStatus.Checked == true)
        {
            Ddl_f_HRStatus.Enabled = true;
            Ddl_f_HRStatus.SelectedIndex = Ddl_f_HRStatus.Items.Count - 1;
        }
        else
        {
            Ddl_f_HRStatus.Enabled = false;
            Ddl_f_HRStatus.SelectedIndex = Ddl_f_HRStatus.Items.Count - 1;
        }
    }
    protected void Cb_f_Hours_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_f_Hours.Checked == true)
        {
            Ddl_f_Hours.Enabled = true;
            Ddl_f_Hours.SelectedIndex = Ddl_f_Hours.Items.Count - 1;
        }
        else
        {
            Ddl_f_Hours.Enabled = false;
            Ddl_f_Hours.SelectedIndex = Ddl_f_Hours.Items.Count - 1;
        }
    }

    private void loadPlanYears()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        DdlPlanYear.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
        DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
        DdlPlanYear.DataBind();
        DdlPlanYear.SelectedIndex = DdlPlanYear.Items.Count - 1;
    }

    private void loadInsurancePlans()
    {
        int _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);

        Ddl_io_InsurancePlan.DataSource = insuranceController.manufactureInsuranceList(_planYearID);
        Ddl_io_InsurancePlan.DataTextField = "INSURANCE_COMBO";
        Ddl_io_InsurancePlan.DataValueField = "INSURANCE_ID";
        Ddl_io_InsurancePlan.DataBind();

        Ddl_io_InsurancePlan.Items.Add("Select");
        Ddl_io_InsurancePlan.SelectedIndex = Ddl_io_InsurancePlan.Items.Count - 1;
    }

    private void loadContributionPlans()
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_io_InsurancePlan, validData);

        if (validData == true)
        {
            int _insuranceID = int.Parse(Ddl_io_InsurancePlan.SelectedItem.Value);
            Ddl_io_Contribution.DataSource = insuranceController.manufactureInsuranceContributionList(_insuranceID);
            Ddl_io_Contribution.DataTextField = "INS_DESC";
            Ddl_io_Contribution.DataValueField = "INS_CONT_ID";
            Ddl_io_Contribution.DataBind();

            Ddl_io_Contribution.Items.Add("Select");
            Ddl_io_Contribution.SelectedIndex = Ddl_io_Contribution.Items.Count - 1;
        }
        else
        {

        }
    }

    private void loadHRStatus()
    {
        int _employerID = int.Parse(HfDistrictID.Value);

        if (Session["HRStatus"] == null)
        {
            Session["HRStatus"] = hrStatus_Controller.manufactureHRStatusList(_employerID);
        }
        Ddl_f_HRStatus.DataSource = Session["HRStatus"];
        Ddl_f_HRStatus.DataTextField = "HR_STATUS_NAME";
        Ddl_f_HRStatus.DataValueField = "HR_STATUS_ID";
        Ddl_f_HRStatus.DataBind();

        Ddl_f_HRStatus.Items.Add("Select");
        Ddl_f_HRStatus.SelectedIndex = Ddl_f_HRStatus.Items.Count - 1;
    }

    private void loadEmployeeClassifications()
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        Ddl_f_Classification.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        Ddl_f_Classification.DataTextField = "CLASS_DESC";
        Ddl_f_Classification.DataValueField = "CLASS_ID";
        Ddl_f_Classification.DataBind();

        Ddl_f_Classification.Items.Add("Select");
        Ddl_f_Classification.SelectedIndex = Ddl_f_Classification.Items.Count - 1;

    }

    /// <summary>
    /// 40)
    /// </summary>
    /// <param name="_employerID"></param>
    private void loadAlertDetails()
    {
        int _employerID = int.Parse(HfDistrictID.Value);

        if (Session["insuranceAlertDetails"] == null)
        {
            Session["insuranceAlertDetails"] = alert_controller.manufactureEmployerInsuranceAlertList(_employerID);
        }

        List<alert_insurance> filteredList = new List<alert_insurance>();
        List<alert_insurance> filteredList1 = new List<alert_insurance>();
        List<alert_insurance> filteredList2 = new List<alert_insurance>();
        List<alert_insurance> filteredList3 = new List<alert_insurance>();
        List<alert_insurance> tempList3 = (List<alert_insurance>)Session["insuranceAlertDetails"];

        foreach (alert_insurance ai in tempList3)
        {
            Employee employee = EmployeeController.findSingleEmployee(ai.IALERT_EMPLOYEEID);
            Measurement Meas = measurementController.getPlanYearMeasurement(ai.IALERT_EMPLOYERID, ai.IALERT_PLANYEARID, employee.EMPLOYEE_TYPE_ID);

            if (Meas != null)
            {
                AverageHours average = (from hours in AverageHoursFactory.GetAllAverageHoursForEmployeeId(employee.EMPLOYEE_ID)
                                        where hours.IsNewHire == false && hours.MeasurementId == Meas.MEASUREMENT_ID
                                        select hours).FirstOrDefault();

                if (average != null)
                {
                    ai.EMPLOYEE_AVG_HOURS = average.MonthlyAverageHours;
                }
            }
        }

        if (DdlPlanYear.SelectedItem.Text != "Select")
        {
            int _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
            foreach (alert_insurance ai in tempList3)
            {
                if (ai.IALERT_PLANYEARID == _planYearID)
                {
                    filteredList.Add(ai);
                }
            }
        }

        if (Ddl_f_Classification.SelectedItem.Text != "Select")
        {
            int _classID = int.Parse(Ddl_f_Classification.SelectedItem.Value);
            foreach (alert_insurance ai in filteredList)
            {
                if (ai.IALERT_CLASS_ID == _classID)
                {
                    filteredList1.Add(ai);
                }
            }
        }
        else
        {
            filteredList1 = filteredList;
        }

        if (Ddl_f_HRStatus.SelectedItem.Text != "Select")
        {
            int _hrStatusID = int.Parse(Ddl_f_HRStatus.SelectedItem.Value);
            foreach (alert_insurance ai in filteredList1)
            {
                if (ai.IALERT_HRSTATUS_ID == _hrStatusID)
                {
                    filteredList2.Add(ai);
                }
            }
        }
        else
        {
            filteredList2 = filteredList1;
        }

        if (Ddl_f_Hours.SelectedItem.Text != "Select")
        {
            double _actHours = double.Parse(Ddl_f_Hours.SelectedItem.Value);
            string filterType = Ddl_f_Hours.SelectedItem.Text;

            if (filterType.Contains("Greater"))
            {
                foreach (alert_insurance ai in filteredList2)
                {

                    if (ai.EMPLOYEE_AVG_HOURS >= _actHours)
                    {
                        filteredList3.Add(ai);
                    }

                }
            }

            if (filterType.Contains("Less"))
            {
                foreach (alert_insurance ai in filteredList2)
                {

                    if (ai.EMPLOYEE_AVG_HOURS < _actHours)
                    {
                        filteredList3.Add(ai);
                    }

                }
            }
        }
        else
        {
            filteredList3 = filteredList2;
        }

        if (filteredList.Count > 0)
        {
            Session["FilteredAlerts"] = filteredList3;
            GvOffers.DataSource = filteredList3;
            GvOffers.DataBind();
        }
        else
        {
            Session["FilteredAlerts"] = null;
            GvOffers.DataSource = null;
            GvOffers.DataBind();
        }

        litAlertsShown.Text = GvOffers.Rows.Count.ToString();
        litAlertCount.Text = tempList3.Count.ToString();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void Ddl_io_Offered_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_io_Offered, validData);

        if (validData == true)
        {
            int employerID = 0;
            int planYearID = 0;
            bool offered = false;

            int.TryParse(HfDistrictID.Value, out employerID);
            int.TryParse(DdlPlanYear.SelectedItem.Value, out planYearID);
            bool.TryParse(Ddl_io_Offered.SelectedItem.Value, out offered);

            PlanYear currPlanYear = PlanYear_Controller.findPlanYear(planYearID, employerID);
            if (currPlanYear != null)
            {
                Txt_io_InsuranceEffectiveDate.Text = currPlanYear.Default_Stability_Start.ToShortDate();
            }

            switch (offered)
            {
                case false:   
                    Pnl_io_Accepted.Visible = false;
                    Pnl_io_Plan.Visible = true;
                    Pnl_io_DateOffered.Visible = false;
                    Pnl_io_Effective.Visible = false;
                    Pnl_io_PlanOffered.Visible = false;
                    Lbl_io_Message.Text = null;
                    break;
                case true:  
                    Pnl_io_Accepted.Visible = true;
                    Pnl_io_Plan.Visible = false;
                    Pnl_io_DateOffered.Visible = true;
                    Pnl_io_Effective.Visible = false;
                    Lbl_io_Message.Text = null;
                    loadInsurancePlans();
                    Txt_io_Comments.BackColor = System.Drawing.Color.White;
                    break;
                default:
                    break;
            }
        }
        else
        {
            defaultView();
            MpeWebMessage.Show();
            LitMessage.Text = "Please correct all RED fields.";
        }
    }

    protected void Ddl_io_Accepted2_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_io_Accepted2, validData);

        if (validData == true)
        {
            Pnl_io_Plan.Visible = true;
            Pnl_io_Effective.Visible = true;
            Pnl_io_PlanOffered.Visible = true;
        }
        else
        {
            MpeWebMessage.Show();
        }
    }

    protected void Ddl_io_InsurancePlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_io_InsurancePlan, validData);

        if (validData == true)
        {
            loadContributionPlans();
        }
        else
        {

        }
    }

    protected void DdlPlanYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        defaultView();
        loadAlertDetails();
    }

    private void defaultView()
    {
        Pnl_io_Accepted.Visible = false;
        Pnl_io_Plan.Visible = false;
        Pnl_io_DateOffered.Visible = false;
        Pnl_io_Effective.Visible = false;
        Ddl_io_Offered.SelectedIndex = Ddl_io_Offered.Items.Count - 1;
        Ddl_io_Accepted2.SelectedIndex = Ddl_io_Accepted2.Items.Count - 1;

        Txt_io_AcceptedOffer.Text = null;
        Txt_io_Comments.Text = null;
        Txt_io_DateOffered.Text = null;
        Txt_io_InsuranceEffectiveDate.Text = null;

        Ddl_io_InsurancePlan.DataSource = null;
        Ddl_io_InsurancePlan.DataBind();

        Ddl_io_Contribution.DataSource = null;
        Ddl_io_Contribution.DataBind();

        Lbl_io_Message.Text = null;

        CbCheckAll.Checked = false;
    }

    private bool validateGridviewSelection()
    {
        bool validData = false;

        foreach (GridViewRow row in GvOffers.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");

            if (cb.Checked == true)
            {
                validData = true;
                break;
            }
        }

        return validData;
    }

    protected void Ddl_f_HRStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void Ddl_f_Hours_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void Ddl_f_Classification_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void CbCheckAll_CheckedChanged(object sender, EventArgs e)
    {
        checkAll();
    }

    protected void GvOffers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvOffers.PageIndex = e.NewPageIndex;
        loadAlertDetailsSorted();

        checkAll();
    }

    private void checkAll()
    {
        foreach (GridViewRow row in GvOffers.Rows)
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

    protected void GvOffers_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortBy = e.SortExpression;
        string lastSortExpression = "";
        string lastSortDirection = "ASC";
        int _employerID = int.Parse(HfDistrictID.Value);

        List<alert_insurance> tempList = (List<alert_insurance>)Session["FilteredAlerts"];

        foreach (alert_insurance ai in tempList)
        {
            Employee employee = EmployeeController.findSingleEmployee(ai.IALERT_EMPLOYEEID);
            Measurement Meas = measurementController.getPlanYearMeasurement(ai.IALERT_EMPLOYERID, ai.IALERT_PLANYEARID, employee.EMPLOYEE_TYPE_ID);

            if (Meas != null)
            {
                AverageHours average = (from hours in AverageHoursFactory.GetAllAverageHoursForEmployeeId(employee.EMPLOYEE_ID)
                                        where hours.IsNewHire == false && hours.MeasurementId == Meas.MEASUREMENT_ID
                                        select hours).FirstOrDefault();

                if (average != null)
                {
                    ai.EMPLOYEE_AVG_HOURS = average.MonthlyAverageHours;
                }
            }
        }

        if (null == tempList)
        {
            this.Log.Warn("Sort Temp List of Insurance was null.");
            return;
        }

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
                        case "IALERT_AVG_HOURS":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_AVG_HOURS).ToList();
                            break;
                        case "EMPLOYEE_FULL_NAME":
                            tempList = tempList.OrderBy(o => o.EMPLOYEE_FULL_NAME).ToList();
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

            Session["FilteredAlerts"] = tempList;
            Session["sortDir"] = lastSortDirection;
            Session["sortExp"] = lastSortExpression;
            loadAlertDetailsSorted();

            checkAll();
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }

    }
    private void loadAlertDetailsSorted()
    {
        GvOffers.DataSource = Session["FilteredAlerts"];
        GvOffers.DataBind();
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        int _rowID = 0;
        int _planYearID = 0;
        int _employerID = 0;
        int _employeeID = 0;
        int _insuranceID = 0;
        int _contributionID = 0;
        int? offerID = null;
        double _hraFlex = 0;
        double? _avgHours = null;
        bool offered = false;
        bool? _accepted = null;
        DateTime _modOn = DateTime.Now;
        DateTime? _effectiveDate = null;
        string _modBy = HfUName.Value;
        string _notes = null;
        string _history = null;
        bool validData = true;
        bool gvSelected = false;
        bool validTransaction = false;
        alert_insurance ai = null;
        List<Employee> tempEmpList = null;

        try
        {
            _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
            _employerID = int.Parse(HfDistrictID.Value);
            PlanYear tempPlanYear = PlanYear_Controller.findPlanYear(_planYearID, _employerID);

            validData = errorChecking.validateDropDownSelection(Ddl_io_Offered, validData);

            gvSelected = validateGridviewSelection();

            if (Session["Employees"] == null)
            {
                tempEmpList = EmployeeController.manufactureEmployeeList(_employerID);
                Session["Employees"] = tempEmpList;
            }
            else
            {
                tempEmpList = (List<Employee>)Session["Employees"];
            }

            if (validData == true && gvSelected == true)
            {
                offered = bool.Parse(Ddl_io_Offered.SelectedItem.Value);
                CbCheckAll.BackColor = System.Drawing.Color.Transparent;
                /****************************************************************************************************
                 ************** All functions pertaining to Employee being offered insurance. *********************** 
                 ***************************************************************************************************/
                if (offered == true)
                {
                    validData = errorChecking.validateTextBoxDate(Txt_io_InsuranceEffectiveDate, validData);
                    validData = errorChecking.validateDropDownSelection(Ddl_io_Offered, validData);
                    validData = errorChecking.validateDropDownSelection(Ddl_io_Accepted2, validData);
                    validData = errorChecking.validateDropDownSelection(Ddl_io_InsurancePlan, validData);
                    validData = errorChecking.validateDropDownSelection(Ddl_io_Contribution, validData);
                    validData = errorChecking.validateTextBoxDecimal(Txt_io_HraFlex, validData);

                    if (validData == true)
                    {
                        bool successAll = true;

                        foreach (GridViewRow row in GvOffers.Rows)
                        {
                            HiddenField hf = (HiddenField)row.FindControl("HfRowID");
                            HiddenField hf2 = (HiddenField)row.FindControl("HfEmployeeID");
                            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
                            Literal lit = (Literal)row.FindControl("LitAvgHours");

                            if (cb.Checked == true)
                            {
                                _rowID = int.Parse(hf.Value);
                                _employeeID = int.Parse(hf2.Value);
                                _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
                                Employee tempEmployee = EmployeeController.findEmployee(tempEmpList, _employeeID);
                                DateTime hireDate = (DateTime)tempEmployee.EMPLOYEE_HIRE_DATE;
                                ai = insuranceController.findSingleInsuranceOffer(_planYearID, _employeeID);
                                _avgHours = double.Parse(lit.Text);

                                Measurement tempMeas = measurementController.getPlanYearMeasurement(_employerID, _planYearID, tempEmployee.EMPLOYEE_TYPE_ID);


                                _insuranceID = int.Parse(Ddl_io_InsurancePlan.SelectedItem.Value);
                                _contributionID = int.Parse(Ddl_io_Contribution.SelectedItem.Value);
                                _accepted = bool.Parse(Ddl_io_Accepted2.SelectedItem.Value);
                                _effectiveDate = DateTime.Parse(Txt_io_InsuranceEffectiveDate.Text);
                                _hraFlex = double.Parse(Txt_io_HraFlex.Text);
                                _notes = Txt_io_Comments.Text;
                                _history = ai.IALERT_HISTORY;
                                _history += Environment.NewLine;
                                _history += "Record modified by " + _modBy + " on " + _modOn.ToString() + Environment.NewLine;
                                _history += "Insuranced was NOT offered." + Environment.NewLine;

                                validData = errorChecking.validateInsuranceOfferDates((DateTime)_effectiveDate, tempMeas, (DateTime)_effectiveDate, (DateTime)_effectiveDate, validData, Lbl_io_Message, hireDate, Txt_io_DateOffered, Txt_io_AcceptedOffer, Txt_io_InsuranceEffectiveDate);

                                if (validData == true)
                                {
                                    validTransaction = insuranceController.updateInsuranceOffer(_rowID, _insuranceID, _contributionID, _avgHours, offered, _effectiveDate, _accepted, _effectiveDate, _modOn, _modBy, _notes, _history, _effectiveDate, _hraFlex);

                                    if (validTransaction == true)
                                    {
                                        Session["FilteredAlerts"] = null;
                                        Session["insuranceAlertDetails"] = null;
                                    }
                                }
                                else
                                {
                                    successAll = false;
                                    break;
                                }
                            }
                        }

                        if (successAll == true)
                        {
                            defaultView();
                            loadAlertDetails();
                        }
                        else
                        {
                            MpeWebMessage.Show();
                            LitMessage.Text = "Please correct the dates, they are invalid times.";
                        }

                    }
                    else
                    {
                        MpeWebMessage.Show();
                        LitMessage.Text = "Please correct all of the red highlighted fields.";
                    }
                }

                /****************************************************************************************************
                 ************** All functions pertaining to Employee NOT being offered insurance. *********************** 
                 ***************************************************************************************************/
                else
                {
                    validData = errorChecking.validateTextBoxDate(Txt_io_InsuranceEffectiveDate, validData);
                    validData = errorChecking.validateDropDownSelection(Ddl_io_Offered, validData);

                    if (validData == true)
                    {
                        foreach (GridViewRow row in GvOffers.Rows)
                        {
                            HiddenField hf = (HiddenField)row.FindControl("HfRowID");
                            HiddenField hf2 = (HiddenField)row.FindControl("HfEmployeeID");
                            CheckBox cb = (CheckBox)row.FindControl("Cb_gv_Selected");
                            Literal lit = (Literal)row.FindControl("LitAvgHours");

                            if (cb.Checked == true)
                            {
                                _rowID = int.Parse(hf.Value);
                                _employeeID = int.Parse(hf2.Value);
                                _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
                                ai = insuranceController.findSingleInsuranceOffer(_planYearID, _employeeID);
                                offered = false;
                                _avgHours = double.Parse(lit.Text);

                                _insuranceID = 0;
                                _contributionID = 0;
                                _accepted = null;
                                _hraFlex = 0;

                                _effectiveDate = null;
                                _notes = Txt_io_Comments.Text;
                                _history = ai.IALERT_HISTORY;
                                _history += Environment.NewLine;
                                _history += "Record modified by " + _modBy + " on " + _modOn.ToString() + Environment.NewLine;
                                _history += "Insuranced was NOT offered." + Environment.NewLine;

                                validTransaction = insuranceController.updateInsuranceOffer(_rowID, _insuranceID, _contributionID, _avgHours, offered, _effectiveDate, _accepted, _effectiveDate, _modOn, _modBy, _notes, _history, _effectiveDate, _hraFlex);

                                if (validTransaction == true)
                                {
                                    Session["FilteredAlerts"] = null;
                                    Session["insuranceAlertDetails"] = null;
                                }
                            }
                        }
                        defaultView();
                        loadAlertDetails();
                    }
                    else
                    {
                        MpeWebMessage.Show();
                        LitMessage.Text = "Please correct all red fields.";
                    }
                }   
            }
            else
            {
                if (validData == false)
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "Please Answer Question #1.";
                }
                else if (gvSelected == false)
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "Please select an insurance offer/record to update.";
                    CbCheckAll.BackColor = System.Drawing.Color.Red;
                }
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            MpeWebMessage.Show();
            LitMessage.Text = "An error occurred while updated the insurance offers, please try again.";
        }
    }

    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            employer currEmployer = (employer)Session["CurrentDistrict"];

            int _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
            PlanYear planyear = PlanYear_Controller.findPlanYear(_planYearID, currEmployer.EMPLOYER_ID);

            List<alert_insurance> tempList = insuranceController.getAllInsuranceForEmployerPlanYear(currEmployer.EMPLOYER_ID, _planYearID);
            User currUser = (User)Session["CurrentUser"];

            if (null == tempList)
            {
                this.Log.Warn("Export Temp List of Insurance was null.");
                return;
            }

            string fileName = "InsuranceOffers_" + currEmployer.EMPLOYER_NAME + "_" + DateTime.Now.ToShortDateString() + ".csv";

            string csvString = insuranceController.generateInsuranceAlertFile(tempList, planyear, currEmployer);

            string body = "A " + currEmployer.EMPLOYER_NAME + " insurance alert file has been downloaded by " + HfUName.Value + " at " + DateTime.Now.ToString();

            PIILogger.LogPII(String.Format("All Insurance Offer Alert Download: {0} -- File Path: [{1}], IP:[{2}], User Id:[{3}]", body, fileName, Request.UserHostAddress, currUser.User_ID));
            string appendText = "attachment; filename=" + fileName;
            Response.ClearContent();
            Response.BufferOutput = false;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", appendText);
            Response.Write(csvString);

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


    protected void BtnApplyFilters_Click(object sender, EventArgs e)
    {
        loadAlertDetails();
    }
}