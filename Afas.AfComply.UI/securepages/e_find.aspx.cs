using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afas.AfComply.Domain;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.Domain.POCO;

public partial class securepages_e_find : Afas.AfComply.UI.securepages.SecurePageBase
{

    private ILog Log = LogManager.GetLogger(typeof(securepages_e_find));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {

        HfUserName.Value = user.User_Full_Name;
        HfDistrictID.Value = user.User_District_ID.ToString();

        GvEmployees.Columns[3].Visible = Feature.ShowDOB;
        GvDependents.Columns[5].Visible = Feature.ShowDOB;

        int _employerID = employer.EMPLOYER_ID;
        Session["MPSelection"] = 0;

        loadStates();
        loadHRStatus(_employerID);
        loadEmployeeGridview();
        loadEmployeeTypes();
        loadPlanYears();
        loadEmployeeClassifications();
        loadACAstatus();

    }

    private void loadACAstatus()
    {

        List<classification_aca> acaClassifications;

        if (Session["TZ_ACA_CLASSIFICATIONS"] == null)
        {
            Session["TZ_ACA_CLASSIFICATIONS"] = classificationController.getACAstatusList();
        }

        acaClassifications = Session["TZ_ACA_CLASSIFICATIONS"] as List<classification_aca>;

        DdlActStatus.DataSource = acaClassifications;
        DdlActStatus.DataTextField = "ACA_STATUS_NAME";
        DdlActStatus.DataValueField = "ACA_STATUS_ID";
        DdlActStatus.DataBind();

        DdlActStatus.Items.Add("Select");
        DdlActStatus.SelectedIndex = DdlActStatus.Items.Count - 1;

        Ddl_new_ActStatus.DataSource = acaClassifications;
        Ddl_new_ActStatus.DataTextField = "ACA_STATUS_NAME";
        Ddl_new_ActStatus.DataValueField = "ACA_STATUS_ID";
        Ddl_new_ActStatus.DataBind();

        Ddl_new_ActStatus.Items.Add("Select");
        Ddl_new_ActStatus.SelectedIndex = DdlActStatus.Items.Count - 1;

    }

    private void loadStates()
    {

        List<State> states;

        if (Session["TZ_STATES"] == null)
        {
            Session["TZ_STATES"] = StateController.getStates();
        }

        states = Session["TZ_STATES"] as List<State>;

        Ddl_new_State.DataSource = states;
        Ddl_new_State.DataTextField = "State_Name";
        Ddl_new_State.DataValueField = "State_ID";
        Ddl_new_State.DataBind();
        Ddl_new_State.Items.Add("Select");
        Ddl_new_State.SelectedIndex = Ddl_new_State.Items.Count - 1;

    }

    private void loadEmployeeClassifications()
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        DdlClassification.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        DdlClassification.DataTextField = "CLASS_DESC";
        DdlClassification.DataValueField = "CLASS_ID";
        DdlClassification.DataBind();

        DdlClassification.Items.Add("Select");
        DdlClassification.SelectedIndex = DdlClassification.Items.Count - 1;

        Ddl_new_Classification.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        Ddl_new_Classification.DataTextField = "CLASS_DESC";
        Ddl_new_Classification.DataValueField = "CLASS_ID";
        Ddl_new_Classification.DataBind();

        Ddl_new_Classification.Items.Add("Select");
        Ddl_new_Classification.SelectedIndex = DdlClassification.Items.Count - 1;

    }

    private void loadEmployeeTypes()
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        List<EmployeeType> employeeTypes;

        if (Session["TZ_EMPLOYEE_TYPES"] == null)
        {
            Session["TZ_EMPLOYEE_TYPES"] = EmployeeTypeController.getEmployeeTypes(_employerID);
        }

        employeeTypes = Session["TZ_EMPLOYEE_TYPES"] as List<EmployeeType>;

        Ddl_new_Type.DataSource = employeeTypes;
        Ddl_new_Type.DataTextField = "EMPLOYEE_TYPE_NAME";
        Ddl_new_Type.DataValueField = "EMPLOYEE_TYPE_ID";
        Ddl_new_Type.DataBind();

        Ddl_new_Type.Items.Add("Select");
        Ddl_new_Type.SelectedIndex = Ddl_new_Type.Items.Count - 1;

    }

    private void loadPlanYears()
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        List<PlanYear> planYears;

        if (Session["TZ_EMPLOYER_PLAN_YEAR"] == null)
        {
            Session["TZ_EMPLOYER_PLAN_YEAR"] = PlanYear_Controller.getEmployerPlanYear(_employerID);
        }

        planYears = Session["TZ_EMPLOYER_PLAN_YEAR"] as List<PlanYear>;

        Ddl_new_PlanYear.DataSource = planYears;
        Ddl_new_PlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        Ddl_new_PlanYear.DataValueField = "PLAN_YEAR_ID";
        Ddl_new_PlanYear.DataBind();

        Ddl_new_PlanYear.Items.Add("Select");
        Ddl_new_PlanYear.SelectedIndex = Ddl_new_PlanYear.Items.Count - 1;

    }

    private void loadHRStatus(int _employerID)
    {

        if (Session["HRStatus"] == null)
        {
            Session["HRStatus"] = hrStatus_Controller.manufactureHRStatusList(_employerID);
        }

        DdlHrStatus.DataSource = Session["HRStatus"];
        DdlHrStatus.DataTextField = "HR_STATUS_NAME";
        DdlHrStatus.DataValueField = "HR_STATUS_ID";
        DdlHrStatus.DataBind();

        Ddl_new_HRStatus.DataSource = Session["HRStatus"];
        Ddl_new_HRStatus.DataTextField = "HR_STATUS_NAME";
        Ddl_new_HRStatus.DataValueField = "HR_STATUS_ID";
        Ddl_new_HRStatus.DataBind();

        DdlHrStatus.Items.Add("Select");
        DdlHrStatus.SelectedIndex = DdlHrStatus.Items.Count - 1;

        Ddl_new_HRStatus.Items.Add("Select");
        Ddl_new_HRStatus.SelectedIndex = Ddl_new_HRStatus.Items.Count - 1;

    }

    /// <summary>
    /// Loads the Contribution Drop Down List. Pass in 0 if you want to pull Insurance ID value from the dropdown list. 
    /// </summary>
    /// <param name="_insuranceID"></param>
    private void loadContributionPlans(int _insuranceID)
    {

        bool validData = true;
        int insuranceID = 0;

        if (_insuranceID == 0)
        {
            validData = errorChecking.validateDropDownSelection(Ddl_io_InsurancePlan, validData);
            insuranceID = int.Parse(Ddl_io_InsurancePlan.SelectedItem.Value);
        }
        else
        {
            insuranceID = _insuranceID;
        }

        if (validData == true)
        {

            Ddl_io_Contribution.DataSource = insuranceController.manufactureInsuranceContributionList(insuranceID);
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


    protected void GvEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {

        int _index = 0;
        HiddenField hf = null;
        GridViewRow row = GvEmployees.SelectedRow;
        ImgBtnViewSSN.ImageUrl = "~/design/eyeclosed.png";
        Session["showSSN"] = false;
        hf = (HiddenField)row.FindControl("HfEmployeeID");
        int _employeeID = int.Parse(hf.Value);
        loadEmployeeData(_employeeID, _index);

    }



    private void loadEmployeeGridview()
    {

        if (Session["Employees"] == null)
        {

            int _employerID = int.Parse(HfDistrictID.Value);

            Session["Employees"] = EmployeeController.manufactureEmployeeList(_employerID);

        }

        if (null == Session["MPSelection"])
        {
            return;
        }

        int _index = (int)Session["MPSelection"];

        string searchText = txtSearch.Text;
        string searchText2 = TxtSearch2.Text;

        List<Employee> tempEmp = (List<Employee>)Session["Employees"];
        List<Employee> filteredList = new List<Employee>();
        List<Employee> filteredList2 = new List<Employee>();
        litEmployeeCount.Text = tempEmp.Count().ToString();

        try
        {

            if (searchText != "")
            {
                foreach (Employee emp in tempEmp)
                {
                    if (filterEmployees(emp, searchText) == true)
                    {
                        filteredList.Add(emp);
                    }
                }
                litEmployeeShown.Text = filteredList.Count().ToString();
            }

            if (searchText2 == "")
            {
                filteredList2 = filteredList;
            }
            else if (searchText2 != "" && searchText == "")
            {
                foreach (Employee emp in tempEmp)
                {
                    if (emp.EMPLOYEE_EXT_ID.ToUpper().Contains(searchText2.ToUpper()))
                    {
                        filteredList2.Add(emp);
                    }
                }
            }
            else
            {

                foreach (Employee emp in filteredList)
                {

                    if (emp.EMPLOYEE_EXT_ID.ToUpper().Contains(searchText2.ToUpper()))
                    {
                        filteredList2.Add(emp);
                    }

                }

            }

            GvEmployees.DataSource = filteredList2;
            GvEmployees.DataBind();

            if (GvEmployees.Rows.Count > 0)
            {

                if (GvEmployees.SelectedIndex == -1)
                {
                    GvEmployees.SelectedIndex = 0;
                }

                GridViewRow row = GvEmployees.SelectedRow;
                HiddenField hf = null;
                hf = (HiddenField)row.FindControl("HfEmployeeID");
                int _employeeID = int.Parse(hf.Value);
                loadEmployeeData(_employeeID, _index);

                EnableOrDisableConrtols(true);

            }
            else
            {

                clearEmployeeData();
                EnableOrDisableConrtols(false);
                GvEmployees.DataSource = null;
                GvEmployees.DataBind();

            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);
            EnableOrDisableConrtols(false);
            clearEmployeeData();
            GvEmployees.DataSource = null;
            GvEmployees.DataBind();

        }

        if (GvEmployees.Rows.Count > 0)
        {

            GvEmployees.SelectedIndex = 0;
            HiddenField hf = null;
            GridViewRow row = GvEmployees.SelectedRow;
            hf = (HiddenField)row.FindControl("HfEmployeeID");
            int _employeeID = int.Parse(hf.Value);
            loadEmployeeData(_employeeID, _index);

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_empID"></param>
    private void loadEmployeeData(int _empID, int _index)
    {

        if (0 == _empID)
        {
            Log.Warn("Tried to load employee Data for emp Id 0, with Index: " + _index);
            return;
        }

        List<Employee> tempEmployeeList = (List<Employee>)Session["Employees"];

        Session["EmployeeCurrPayroll"] = null;
        Session["Employee2ndPayroll"] = null;

        Measurement measCurrMeasPeriod = null;
        Measurement measCurrStabPeriod = null;
        Measurement measCurrAdminPeriod = null;

        PlanYear initPY = null;
        PlanYear stabilityPY = null;
        PlanYear measurementPY = null;
        PlanYear adminPY = null;

        State currState = null;

        Employee currEmployee = null;

        currEmployee = EmployeeController.findEmployee(tempEmployeeList, _empID);
        if (null == currEmployee)
        {
            Log.Warn(String.Format("Did not Find Employee by Id: [{0}], in List of size: [{1}], Index was: [{2}];", _empID, tempEmployeeList.Count, _index));
            return;
        }

        currState = StateController.findState(currEmployee.EMPLOYEE_STATE_ID);

        if (Session["showSSN"] == null)
        {
            Session["showSSN"] = false;
        }
        bool showSSN = (bool)Session["showSSN"];

        try
        {
            initPY = new PlanYear(0, currEmployee.EMPLOYEE_EMPLOYER_ID, "Initial Plan Year", EmployeeController.calculateIMPStartDate((DateTime)currEmployee.EMPLOYEE_HIRE_DATE), currEmployee.EMPLOYEE_IMP_END, "", "", null, null);
            stabilityPY = PlanYear_Controller.findPlanYear(currEmployee.EMPLOYEE_PLAN_YEAR_ID, currEmployee.EMPLOYEE_EMPLOYER_ID);
            measurementPY = PlanYear_Controller.findPlanYear(currEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS, currEmployee.EMPLOYEE_EMPLOYER_ID);
            adminPY = PlanYear_Controller.findPlanYear(currEmployee.EMPLOYEE_PLAN_YEAR_ID_LIMBO, currEmployee.EMPLOYEE_EMPLOYER_ID);

            measCurrStabPeriod = measurementController.getPlanYearMeasurement(currEmployee.EMPLOYEE_EMPLOYER_ID, currEmployee.EMPLOYEE_PLAN_YEAR_ID, currEmployee.EMPLOYEE_TYPE_ID);
            measCurrMeasPeriod = measurementController.getPlanYearMeasurement(currEmployee.EMPLOYEE_EMPLOYER_ID, currEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS, currEmployee.EMPLOYEE_TYPE_ID);
            measCurrAdminPeriod = measurementController.getPlanYearMeasurement(currEmployee.EMPLOYEE_EMPLOYER_ID, currEmployee.EMPLOYEE_PLAN_YEAR_ID_LIMBO, currEmployee.EMPLOYEE_TYPE_ID);

            bool imp = false;
            string status = null;
            string planName = null;
            string planYearID = null;

            if (stabilityPY != null)
            {
                if (currEmployee.EMPLOYEE_HIRE_DATE >= measCurrStabPeriod.MEASUREMENT_START && currEmployee.EMPLOYEE_HIRE_DATE <= measCurrStabPeriod.MEASUREMENT_END)
                {
                    imp = true;
                    status = "New Hire";
                    LbInitialStatus.Enabled = true;
                    planName = "Employee was hired during the " + stabilityPY.PLAN_YEAR_DESCRIPTION + " measurement period.";
                    planYearID = stabilityPY.PLAN_YEAR_ID.ToString();
                    initPY.PLAN_YEAR_ID = stabilityPY.PLAN_YEAR_ID;
                    LbCurrentStatus.Text = " ";
                    LbCurrentStatus.Enabled = false;
                }
                else
                {
                    LbCurrentStatus.Text = "Current SP";
                    LbCurrentStatus.Enabled = true;
                }
            }
            else
            {
                LbCurrentStatus.Text = " ";
                LbCurrentStatus.Enabled = false;
            }

            if (measurementPY != null)
            {
                if (currEmployee.EMPLOYEE_HIRE_DATE >= measCurrMeasPeriod.MEASUREMENT_START && currEmployee.EMPLOYEE_HIRE_DATE < measCurrMeasPeriod.MEASUREMENT_END)
                {
                    imp = true;
                    status = "New Hire";
                    LbInitialStatus.Enabled = true;
                    planName = "Employee was hired during the " + measurementPY.PLAN_YEAR_DESCRIPTION + " measurement period.";
                    planYearID = measurementPY.PLAN_YEAR_ID.ToString();
                    initPY.PLAN_YEAR_ID = measurementPY.PLAN_YEAR_ID;
                    LbUpcomingStatus.Text = " ";
                    LbUpcomingStatus.Enabled = false;
                }
                else
                {
                    LbUpcomingStatus.Text = "Current MP";
                    LbUpcomingStatus.Enabled = true;
                }
            }
            else
            {
                LbUpcomingStatus.Text = " ";
                LbUpcomingStatus.Enabled = false;
            }

            if (adminPY != null)
            {
                if (currEmployee.EMPLOYEE_HIRE_DATE >= measCurrAdminPeriod.MEASUREMENT_START && currEmployee.EMPLOYEE_HIRE_DATE < measCurrAdminPeriod.MEASUREMENT_END)
                {
                    imp = true;
                    status = "New Hire";
                    LbInitialStatus.Enabled = true;
                    planName = "Employee was hired during the " + adminPY.PLAN_YEAR_DESCRIPTION + " measurement period.";
                    planYearID = adminPY.PLAN_YEAR_ID.ToString();
                    initPY.PLAN_YEAR_ID = adminPY.PLAN_YEAR_ID;
                    LbLimboStatus.Text = " ";
                    LbLimboStatus.Enabled = false;
                }
                else
                {
                    LbLimboStatus.Text = "Current AP";
                    LbLimboStatus.Enabled = true;
                }
            }
            else
            {
                LbLimboStatus.Text = " ";
                LbLimboStatus.Enabled = false;
            }

            LbInitialStatus.Text = status;
            LbInitialStatus.Enabled = imp;
            HfPlanYearID.Value = planYearID;
            LitPlanName.Text = planName;


            if (LbInitialStatus.Text == "" && _index == 0)
            {
                _index = 3;
            }

            LitActID.Text = currEmployee.EMPLOYEE_ID.ToString();
            LitPayrollID.Text = currEmployee.EMPLOYEE_EXT_ID;
            LitFirstname.Text = currEmployee.EMPLOYEE_FIRST_NAME;
            LitMiddleName.Text = currEmployee.EMPLOYEE_MIDDLE_NAME;
            LitLastName.Text = currEmployee.EMPLOYEE_LAST_NAME;

            PIILogger.LogPII(string.Format("Find is displaying information to UserId:[{0}], at IP[{1}] for Employee with Id:[{2}]", ((User)Session["CurrentUser"]).User_ID, Request.UserHostAddress, currEmployee.EMPLOYEE_ID));

            LitAddress.Text = currEmployee.EMPLOYEE_ADDRESS;
            LitCity.Text = currEmployee.EMPLOYEE_CITY;
            LitState.Text = currState.State_Abbr;
            LitZip.Text = currEmployee.EMPLOYEE_ZIP;

            LitDOB.Text = System.Convert.ToDateTime(currEmployee.EMPLOYEE_DOB).ToShortDateString();
            LitHireDate.Text = System.Convert.ToDateTime(currEmployee.EMPLOYEE_HIRE_DATE).ToShortDateString();
            if (currEmployee.EMPLOYEE_TERM_DATE != null)
            {
                LitTermDate.Text = System.Convert.ToDateTime(currEmployee.EMPLOYEE_TERM_DATE).ToShortDateString();
            }
            else
            {
                LitTermDate.Text = String.Empty;
            }

            Lit_pr_Name.Text = currEmployee.EMPLOYEE_FULL_NAME;
            Lit_pr_ExtID.Text = currEmployee.EMPLOYEE_EXT_ID;

            if (showSSN == true)
            {
                TxtSSN.Text = currEmployee.Employee_SSN_Visible;
            }
            else
            {
                TxtSSN.Text = currEmployee.Employee_SSN_Hidden;
            }

            if (currEmployee.EMPLOYEE_C_DATE != null)
            {
                LitStatusChange.Text = System.Convert.ToDateTime(currEmployee.EMPLOYEE_C_DATE).ToShortDateString();
            }
            else
            {
                LitStatusChange.Text = null;
            }

            errorChecking.setDropDownList(DdlHrStatus, currEmployee.EMPLOYEE_HR_STATUS_ID);
            errorChecking.setDropDownList(DdlClassification, currEmployee.EMPLOYEE_CLASS_ID);
            errorChecking.setDropDownList(DdlActStatus, currEmployee.EMPLOYEE_ACT_STATUS_ID);

            if (_index == 0)
            {
                LbInitialStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF");
                LbCurrentStatus.BackColor = System.Drawing.Color.Transparent;
                LbUpcomingStatus.BackColor = System.Drawing.Color.Transparent;
                LbLimboStatus.BackColor = System.Drawing.Color.Transparent;
                loadCurrentMP(currEmployee, initPY, _index);
                LnBtnInsuranceDetails.Visible = true;

                HfPlanYearID.Value = PlanYear_Controller.GetEmployeePlanYearId(currEmployee);

            }
            else if (_index == 1)
            {
                LbInitialStatus.BackColor = System.Drawing.Color.Transparent;
                LbCurrentStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF");
                LbUpcomingStatus.BackColor = System.Drawing.Color.Transparent;
                LbLimboStatus.BackColor = System.Drawing.Color.Transparent;
                loadCurrentMP(currEmployee, stabilityPY, _index);
                LitPlanName.Text = stabilityPY.PLAN_YEAR_DESCRIPTION;
                HfPlanYearID.Value = stabilityPY.PLAN_YEAR_ID.ToString();
                LnBtnInsuranceDetails.Visible = true;
            }
            else if (_index == 2)
            {
                LbInitialStatus.BackColor = System.Drawing.Color.Transparent;
                LbCurrentStatus.BackColor = System.Drawing.Color.Transparent;
                LbUpcomingStatus.BackColor = System.Drawing.Color.Transparent;
                LbLimboStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF");
                loadCurrentMP(currEmployee, adminPY, _index);
                LitPlanName.Text = adminPY.PLAN_YEAR_DESCRIPTION;
                HfPlanYearID.Value = adminPY.PLAN_YEAR_ID.ToString();
                LnBtnInsuranceDetails.Visible = true;
            }
            else if (_index == 3)
            {
                LbInitialStatus.BackColor = System.Drawing.Color.Transparent;
                LbCurrentStatus.BackColor = System.Drawing.Color.Transparent;
                LbUpcomingStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#33CCFF");
                LbLimboStatus.BackColor = System.Drawing.Color.Transparent;
                loadCurrentMP(currEmployee, measurementPY, _index);
                LitPlanName.Text = measurementPY.PLAN_YEAR_DESCRIPTION;
                HfPlanYearID.Value = measurementPY.PLAN_YEAR_ID.ToString();
                LnBtnInsuranceDetails.Visible = false;
            }

            GvPayrollDetails.PageIndex = 0;
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            LitMessage.Text = "An ERROR occurred while calculating the data for " + currEmployee.EMPLOYEE_FULL_NAME + ", please try again and if this same error occures, please contact " + Branding.CompanyShortName;
            clearEmployeeData();
            MpeWebMessage.Show();
        }
    }

    private void EnableOrDisableConrtols(bool visible)
    {
        BtnSaveEmployeeInfo.Visible = visible;
        LbViewDependents.Visible = visible;
        LbInitialStatus.Visible = visible;
        LbCurrentStatus.Visible = visible;
        LbLimboStatus.Visible = visible;
        LbUpcomingStatus.Visible = visible;
        DdlClassification.Enabled = visible;
        TxtSSN.Enabled = visible;
        DdlHrStatus.Enabled = visible;
        DdlActStatus.Enabled = visible;
    }

    private void clearEmployeeData()
    {
        LitFirstname.Text = null;
        LitMiddleName.Text = null;
        LitLastName.Text = null;

        LitAddress.Text = null;
        LitCity.Text = null;
        LitState.Text = null;
        LitZip.Text = null;

        LitDOB.Text = null;
        LitHireDate.Text = null;
        LitTermDate.Text = null;

        TxtSSN.Text = null;
        LitStatusChange.Text = null;
        DdlHrStatus.SelectedIndex = DdlHrStatus.Items.Count - 1;
        TxtStartDate.Text = null;

        LbCurrentStatus.Text = "";
        LbLimboStatus.Text = "";
        LbUpcomingStatus.Text = "";

        LitAdminEndCurr.Text = "n/a";
        LitAdminStartCurr.Text = "n/a";
        LitAdminPercentCurr.Text = "ERROR";

        LitMeasStartCurr.Text = "n/a";
        LitMeasEndCurr.Text = "n/a";
        LitMeasPercentCurr.Text = "ERROR";

        LitStabStartCurr.Text = "n/a";
        LitStabEndCurr.Text = "n/a";
        LitStabPercentCurr.Text = "ERROR";

        LitMeasHoursCurr.Text = "ERROR";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_emp"></param>
    /// <param name="_py"></param>
    private void loadCurrentMP(Employee _emp, PlanYear _py, int _index)
    {
        if (null == _emp)
        {
            Log.Warn("Employee Passed to loadCurrentMP was Null. Index: " + _index);
            return;
        }

        if (null == _py)
        {
            Log.Warn("PlanYear Passed to loadCurrentMP was Null. Index: " + _index);
            return;
        }

        DateTime _payStart = DateTime.Now;
        DateTime _payEnd = DateTime.Now;
        DateTime _adminStart = DateTime.Now;
        DateTime _adminEnd = DateTime.Now;
        DateTime _stabStart = DateTime.Now;
        DateTime _stabEnd = DateTime.Now;

        DateTime currentDate = DateTime.Now;
        List<Payroll> currPayroll = null;
        List<Payroll> currPayrollSupplemental = null;

        Measurement tempMeas = null;
        double averageHours = 0;
        double percentMeas = 0;
        double percentHours = 0;
        double percentAdmin = 0;
        double percentStab = 0;
        string measurementStyle = null;
        string hoursStyle = null;
        string adminStyle = null;
        string stabStyle = null;
        string measColor = null;
        string hoursColor = null;
        string adminColor = null;
        string stabColor = null;

        if (_index == 0)    
        {   
            averageHours = _emp.EMPLOYEE_PY_AVG_INIT_HOURS;

            PnlStabCurr.Visible = true;

            _payStart = EmployeeController.calculateIMPStartDate((DateTime)_emp.EMPLOYEE_HIRE_DATE);
            _payEnd = (DateTime)_emp.EMPLOYEE_IMP_END;

            employer currEmployer = (employer)Session["CurrentDistrict"];

            int impLength = measurementController.getInitialMeasurementLength(currEmployer.EMPLOYER_INITIAL_MEAS_ID);

            int adminLength = 2;
            if (impLength > 11)
            {
                adminLength = 1;
            }

            _adminStart = _payEnd.AddDays(1);
            _adminEnd = _adminStart.AddMonths(adminLength).AddDays(-1);

            _stabStart = _adminEnd.AddDays(1);
            _stabEnd = _stabStart.AddMonths(impLength).AddDays(-1);

        }
        else
        {
            //*Note: There can only be a single measurement period for each EMPLOYER, PLAN YEAR and EMPLOYEE TYPE.
            if (_index == 1)
            {  
                averageHours = _emp.EMPLOYEE_PY_AVG_STABILITY_HOURS;
                tempMeas = measurementController.getPlanYearMeasurement(_emp.EMPLOYEE_EMPLOYER_ID, _emp.EMPLOYEE_PLAN_YEAR_ID, _emp.EMPLOYEE_TYPE_ID);
            }
            else if (_index == 2)
            {  
                averageHours = _emp.EMPLOYEE_PY_AVG_ADMIN_HOURS;
                tempMeas = measurementController.getPlanYearMeasurement(_emp.EMPLOYEE_EMPLOYER_ID, _emp.EMPLOYEE_PLAN_YEAR_ID_LIMBO, _emp.EMPLOYEE_TYPE_ID);
            }
            else if (_index == 3)
            {  
                averageHours = _emp.EMPLOYEE_PY_AVG_MEAS_HOURS;
                tempMeas = measurementController.getPlanYearMeasurement(_emp.EMPLOYEE_EMPLOYER_ID, _emp.EMPLOYEE_PLAN_YEAR_ID_MEAS, _emp.EMPLOYEE_TYPE_ID);
            }
            else
            {
                Log.Warn("Index was not Valid. Index: " + _index);
                return;
            }

            _payStart = tempMeas.MEASUREMENT_START;
            _payEnd = tempMeas.MEASUREMENT_END;
            Lit_pr_MP.Text = _payStart.ToShortDateString() + " - " + _payEnd.ToShortDateString();

            _adminStart = tempMeas.MEASUREMENT_ADMIN_START;
            _adminEnd = tempMeas.MEASUREMENT_ADMIN_END;

            _stabStart = tempMeas.MEASUREMENT_STAB_START;
            _stabEnd = tempMeas.MEASUREMENT_STAB_END;
        }

        currPayroll = Payroll_Controller.getEmployeePayroll(_emp.EMPLOYEE_ID, _payStart, _payEnd);
        currPayrollSupplemental = Payroll_Controller.getEmployeePayrollSummerAverages(_emp.EMPLOYEE_ID, _py.PLAN_YEAR_ID);

        foreach (Payroll p in currPayrollSupplemental)
        {
            currPayroll.Add(p);
        }

        currPayroll = currPayroll.OrderBy(o => o.PAY_SDATE).ToList();

        Session["EmployeeCurrPayroll"] = currPayroll;

        LitStabStartCurr.Text = _stabStart.ToShortDateString();
        LitStabEndCurr.Text = _stabEnd.ToShortDateString();
        LitAdminStartCurr.Text = _adminStart.ToShortDateString();
        LitAdminEndCurr.Text = _adminEnd.ToShortDateString();
        LitMeasStartCurr.Text = _payStart.ToShortDateString();
        LitMeasEndCurr.Text = _payEnd.ToShortDateString();

        if (currentDate >= _payStart)
        {
            percentMeas = EmployeeController.getDateTimeComplete_Percent(_payStart, _payEnd);
            measColor = "bg_nav.png";
            LitMeasPercentCurr.Text = percentMeas.ToString() + " % complete.";

            percentHours = (averageHours / 130) * 100;
            hoursColor = EmployeeController.calculateBarColor(averageHours);
            LitMeasHoursCurr.Text = "Averaging " + Math.Round(averageHours, 2).ToString() + " per month";
            LitMeasHourCurrMax.Text = "130";
            LitMeasHourCurrMin.Text = "0";
        }
        else
        {
            LitMeasHoursCurr.Text = "This will not start calculating average hours until " + _payStart.ToShortDateString() + ".";
            LitMeasPercentCurr.Text = "This Measurement Period will not start until " + _payStart.ToShortDateString() + ".";
        }

        if (currentDate >= _adminStart)
        {
            percentAdmin = EmployeeController.getDateTimeComplete_Percent(_adminStart, _adminEnd);
            adminColor = "bg_nav.png";
            LitAdminPercentCurr.Text = percentAdmin.ToString() + " % complete.";
        }
        else
        {
            LitAdminPercentCurr.Text = "This Administrative period will not start until " + _adminStart.ToShortDateString() + ".";
        }

        if (currentDate >= _stabStart)
        {
            percentStab = EmployeeController.getDateTimeComplete_Percent(_stabStart, _stabEnd);
            stabColor = "bg_nav.png";
            LitStabPercentCurr.Text = percentStab.ToString() + "% complete.";
        }
        else
        {
            LitStabPercentCurr.Text = "This stability period will not be active until " + _stabStart.ToShortDateString() + ".";
        }

        measurementStyle = "height: 15px; width: 500px; border-radius: 5px; background-color: lightgray; color: white; background-image: url(/images/" + measColor + "); background-size: " + percentMeas.ToString() + "% 100%; background-repeat:no-repeat;";
        hoursStyle = "height: 15px; width: 500px; border-radius: 5px; background-color: lightgray; color: white; background-image: url(/images/" + hoursColor + "); background-size: " + Math.Round(percentHours, 0).ToString() + "% 100%; background-repeat:no-repeat;";
        adminStyle = "height: 15px; width: 500px; border-radius: 5px; background-color: lightgray; color: white; background-image: url(/images/" + adminColor + "); background-size: " + Math.Round(percentAdmin, 0).ToString() + "% 100%; background-repeat:no-repeat;";
        stabStyle = "height: 15px; width: 500px; border-radius: 5px; background-color: lightgray; color: white; background-image: url(/images/" + stabColor + "); background-size: " + Math.Round(percentStab, 0).ToString() + "% 100%; background-repeat:no-repeat;";

        measCurr.Attributes["style"] = measurementStyle;
        measHours.Attributes["style"] = hoursStyle;
        adminCurr.Attributes["style"] = adminStyle;
        stabCurr.Attributes["style"] = stabStyle;
    }

    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        GvEmployees.SelectedIndex = -1;
        loadEmployeeGridview();
    }

    protected void ImgBtnSearch2_Click(object sender, ImageClickEventArgs e)
    {
        GvEmployees.SelectedIndex = -1;
        loadEmployeeGridview();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvEmployees.SelectedIndex = 0;
        int _employerID = int.Parse(HfDistrictID.Value);
        GvEmployees.PageIndex = e.NewPageIndex;
        loadEmployeeGridview();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="emp"></param>
    /// <param name="_searchText"></param>
    /// <returns></returns>
    private bool filterEmployees(Employee emp, string _searchText)
    {
        if (emp == null || emp.EMPLOYEE_LAST_NAME == null || _searchText == null)
        {
            return false;
        }

        bool _match = false;

        try
        {
            if (emp.EMPLOYEE_LAST_NAME.ToUpper().Contains(_searchText.ToUpper()))
            {
                _match = true;
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            _match = false;

        }

        return _match;
    }



    protected void ImgBtnViewSSN_Click(object sender, ImageClickEventArgs e)
    {
        int _index = (int)Session["MPSelection"];

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
            Session["showSSN"] = true;
            ImgBtnViewSSN.ImageUrl = "~/design/eyeopen.png";
        }

        if (GvEmployees.Rows.Count > 0)
        {
            int _employeeID = int.Parse(LitActID.Text);
            loadEmployeeData(_employeeID, _index);
        }
    }

    protected void BtnSaveEmployeeInfo_Click(object sender, EventArgs e)
    {
        int _employeeID = int.Parse(LitActID.Text);
        string _ssn = null;
        DateTime _modOn = DateTime.Now;
        string _modBy = HfUserName.Value;
        int _hrStatusID = 0;
        int _classID = 0;
        int _acaStatusID = 0;

        List<Employee> tempEmp = (List<Employee>)Session["Employees"];
        Employee emp = EmployeeController.findEmployee(tempEmp, _employeeID);
        int _index = (int)Session["MPSelection"];

        bool validData = true;

        if (Session["showSSN"] != null)
        {
            bool showSSN = (bool)Session["showSSN"];
            if (showSSN == true)
            {
                validData = errorChecking.validateTextBoxSSN(TxtSSN, validData);
                if (validData == true)
                {
                    _ssn = TxtSSN.Text.Trim(new char[] { ' ' });
                }
            }
            else
            {
                _ssn = emp.Employee_SSN_Visible;
            }
        }
        else
        {
            _ssn = emp.Employee_SSN_Visible;
        }

        validData = errorChecking.validateDropDownSelection(DdlActStatus, validData);
        validData = errorChecking.validateDropDownSelection(DdlClassification, validData);
        validData = errorChecking.validateDropDownSelection(DdlHrStatus, validData);

        if (validData == true)
        {
            _hrStatusID = int.Parse(DdlHrStatus.SelectedItem.Value);
            _classID = int.Parse(DdlClassification.SelectedItem.Value);
            _acaStatusID = int.Parse(DdlActStatus.SelectedItem.Value);

            validData = EmployeeController.UpdateEmployeeSNN(_employeeID, _modOn, _modBy, _ssn, _hrStatusID, _classID, _acaStatusID);

            if (validData == true)
            {

                emp.Employee_SSN_Visible = _ssn;
                emp.EMPLOYEE_HR_STATUS_ID = _hrStatusID;
                emp.EMPLOYEE_CLASS_ID = _classID;
                emp.EMPLOYEE_ACT_STATUS_ID = _acaStatusID;

                ImgBtnViewSSN.ImageUrl = "~/design/eyeclosed.png";
                Session["showSSN"] = false;

                loadEmployeeData(_employeeID, _index);

                LitMessage.Text = "Employee Data has been updated.";
                MpeWebMessage.Show();
            }
            else
            {
                LitMessage.Text = "An ERROR occurred while updating this Employee's Data. Please try again.";
                MpeWebMessage.Show();
            }

        }
        else
        {
            LitMessage.Text = "An ERROR occurred while updating this Employee's Data. Please correct any RED highlighted fields.";
            MpeWebMessage.Show();
        }

    }


    /**********************************************************************************
     **********************************************************************************
     ******** Index Change Events for Employee Measurement Periods. *******************
     **********************************************************************************
     *********************************************************************************/
    protected void LbInitialStatus_Click(object sender, EventArgs e)
    {
        int _employeeID = int.Parse(LitActID.Text);
        Session["MPSelection"] = 0;
        loadEmployeeData(_employeeID, 0);
    }

    protected void LbCurrentStatus_Click(object sender, EventArgs e)
    {
        int _employeeID = int.Parse(LitActID.Text);
        Session["MPSelection"] = 1;
        loadEmployeeData(_employeeID, 1);
    }

    protected void LbLimboStatus_Click(object sender, EventArgs e)
    {
        int _employeeID = int.Parse(LitActID.Text);
        Session["MPSelection"] = 2;
        loadEmployeeData(_employeeID, 2);
    }

    protected void LbUpcomingStatus_Click(object sender, EventArgs e)
    {
        int _employeeID = int.Parse(LitActID.Text);
        Session["MPSelection"] = 3;
        loadEmployeeData(_employeeID, 3);
    }


    protected void BtnNewEmployee_Click(object sender, EventArgs e)
    {
        MpeNewEmployee.Show();
    }

    /// <summary>
    /// This will allow users you manually add NEW employees to the ACT software. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSaveNewEmployee_Click(object sender, EventArgs e)
    {
        int _planYearID = 0;
        int _employeeTypeID = 0;
        int _employerID = int.Parse(HfDistrictID.Value);
        int _hrStatusID = 0;
        string _fname = null;
        string _lname = null;
        string _address = null;
        string _zip = null;
        int _stateID = 0;
        string _city = null;
        DateTime? _dob = null;
        DateTime _hdate;
        DateTime _imEnd;
        string _ssn = null;
        string _extEmployeeID = null;
        DateTime _modOn = DateTime.Now;
        string _modBy = HfUserName.Value;
        int _classID = 0;
        int _actStatusID = 0;

        int _impMonths = 0;
        int _impID = 0;
        bool _imp = false;

        bool validData = true;

        validData = errorChecking.validateTextBoxNull(Txt_new_FirstName, validData);
        validData = errorChecking.validateTextBoxNull(Txt_new_LastName, validData);
        validData = errorChecking.validateTextBoxNull(Txt_new_Address, validData);
        validData = errorChecking.validateTextBoxNull(Txt_new_City, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_new_PlanYear, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_new_Type, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_new_HRStatus, validData);
        validData = errorChecking.validateTextBoxZipCode(Txt_new_Zip, validData);
        validData = errorChecking.validateTextBoxSSN(Txt_new_SSN, validData);

        if (Feature.EmployeeDemographicEmployeeNumberRequired)
        {
            validData = errorChecking.validateTextBoxNull(Txt_new_PayrollID, validData);
        }

        validData = errorChecking.validateTextBoxDate(Txt_new_DOB, validData);
        validData = errorChecking.validateTextBoxDate(Txt_new_Hdate, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_new_ActStatus, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_new_Classification, validData);

        if (validData == true)
        {

            _planYearID = int.Parse(Ddl_new_PlanYear.SelectedItem.Value);
            _employeeTypeID = int.Parse(Ddl_new_Type.SelectedItem.Value);
            _hrStatusID = int.Parse(Ddl_new_HRStatus.SelectedItem.Value);
            _fname = Txt_new_FirstName.Text;
            _lname = Txt_new_LastName.Text;
            _address = Txt_new_Address.Text;
            _stateID = 0;
            int.TryParse(Ddl_new_State.SelectedItem.Value, out _stateID);    
            _city = Txt_new_City.Text;
            _zip = Txt_new_Zip.Text;
            _dob = DateTime.Parse(Txt_new_DOB.Text);
            _hdate = DateTime.Parse(Txt_new_Hdate.Text);
            _ssn = Txt_new_SSN.Text;
            _extEmployeeID = Txt_new_PayrollID.Text;
            _classID = int.Parse(Ddl_new_Classification.SelectedItem.Value);
            _actStatusID = int.Parse(Ddl_new_ActStatus.SelectedItem.Value);

            employer currEmployer = (employer)Session["CurrentDistrict"];

            _impID = currEmployer.EMPLOYER_INITIAL_MEAS_ID;

            _impMonths = measurementController.getInitialMeasurementLength(_impID);
            _imEnd = EmployeeController.calculateIMPEndDate(_hdate, _impMonths);
            _imp = EmployeeController.calculateIMP(_employerID, _employeeTypeID, _planYearID, _hdate, 1);

            Employee newEmployee = null;

            newEmployee = EmployeeController.ManufactureEmployee(
                    _employeeTypeID,
                    _hrStatusID,
                    _employerID,
                    _fname,
                    null,
                    _lname,
                    _address,
                    _city,
                    _stateID,
                    _zip,
                    _hdate,
                    null,
                    _ssn,
                    _extEmployeeID,
                    null,
                    _dob,
                    _imEnd,
                    _planYearID,
                    _modOn,
                    _modBy,
                    _classID,
                    _actStatusID
                );

            if (newEmployee != null)
            {
                List<Employee> tempEmp = (List<Employee>)Session["Employees"];
                tempEmp.Add(newEmployee);
                Session["Employees"] = tempEmp;
                loadEmployeeGridview();
            }
            else
            {
                clearEmployeeData();
                LitMessage.Text = "This social is already in use. Please update the social and try again.";
                MpeWebMessage.Show();
            }
        }
        else
        {
            MpeNewEmployee.Show();
        }

    }

    /// <summary>
    /// This will open up a new popup window and allow users to view a specific employees payroll records. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LbHourDetails_Click(object sender, EventArgs e)
    {
        try
        {
            int _employeeID = int.Parse(LitActID.Text);
            GvPayrollDetails.DataSource = Session["EmployeeCurrPayroll"];
            GvPayrollDetails.DataBind();
            MpePayrollDetails.Show();
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            clearEmployeeData();
            LitMessage.Text = "An error occurred loading the employee data, please make sure an employee is selected.";
            MpeWebMessage.Show();
        }
    }

    /// <summary>
    /// This will open up a new popup window and allow users to enter a new payroll record. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LbHourAdd_Click(object sender, EventArgs e)
    {
        try
        {
            int _employeeID = int.Parse(LitActID.Text);
            int _employerID = int.Parse(HfDistrictID.Value);
            LitName.Text = LitFirstname.Text + " " + LitLastName.Text;

            DdlGrossPayDesc.DataSource = gpType_Controller.getEmployeeTypes(_employerID);
            DdlGrossPayDesc.DataTextField = "GROSS_PAY_DESCRIPTION";
            DdlGrossPayDesc.DataValueField = "GROSS_PAY_ID";
            DdlGrossPayDesc.DataBind();

            DdlGrossPayDesc.Items.Add("Select");
            DdlGrossPayDesc.SelectedIndex = DdlGrossPayDesc.Items.Count - 1;

            MpeNewPayroll.Show();
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            clearEmployeeData();
            LitMessage.Text = "An error occurred loading the employee data, please make sure an employee is selected.";
            MpeWebMessage.Show();
        }
    }


    protected void BtnSavePayroll_Click(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateTextBoxDate(TxtStartDate, validData);
        validData = errorChecking.validateTextBoxDate(TxtEndDate, validData);
        validData = errorChecking.validateTextBoxDate(TxtCheckDate, validData);
        validData = errorChecking.validateTextBoxDecimal(TxtACAhours, validData);
        validData = errorChecking.validateDropDownSelection(DdlGrossPayDesc, validData);
        validData = errorChecking.validateTextBoxDateCompare(TxtStartDate, TxtEndDate, validData);

        if (validData == true)
        {
            int _employeeID = int.Parse(LitActID.Text);
            int _employerID = int.Parse(HfDistrictID.Value);
            int _gpID = int.Parse(DdlGrossPayDesc.SelectedItem.Value);
            DateTime _sdate = DateTime.Parse(TxtStartDate.Text);
            DateTime _edate = DateTime.Parse(TxtEndDate.Text);
            DateTime _cdate = DateTime.Parse(TxtCheckDate.Text);
            decimal _acaHours = decimal.Parse(TxtACAhours.Text);
            DateTime _modOn = DateTime.Now;
            string _modBy = HfUserName.Value;
            Payroll newPR = null;
            string _history = "Manually Created on " + _modOn + " by " + _modBy;
            _history += _acaHours.ToString() + "|" + _sdate.ToString() + "|" + _edate.ToString() + "|" + _cdate.ToString();

            int batchID = EmployeeController.manufactureBatchID(_employerID, _modOn, _modBy);

            newPR = Payroll_Controller.manufactureSinglePayroll(batchID, _employeeID, _employerID, _gpID, _acaHours, _sdate, _edate, _cdate, _modBy, _modOn, _history);

            employerController.insertEmployerCalculation(_employerID);
            if (newPR != null)
            {
                int index = (int)Session["MPSelection"];
                loadEmployeeData(_employeeID, 0);
            }
            else
            {

            }
        }
        else
        {
            LitPayrollMessage.Text = "Please correct all errors.";
            MpeNewPayroll.Show();
        }
    }

    protected void GvPayrollDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GvPayrollDetails.EditIndex = e.NewEditIndex;
        GvPayrollDetails.DataSource = Session["EmployeeCurrPayroll"];
        GvPayrollDetails.DataBind();
        MpePayrollDetails.Show();
    }

    protected void GvPayrollDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string _modBy = HfUserName.Value;
            DateTime _modOn = DateTime.Now;
            GridViewRow row = (GridViewRow)GvPayrollDetails.Rows[e.RowIndex];
            HiddenField hf = (HiddenField)row.FindControl("Hf_pr_RowID");
            Label lbl = (Label)row.FindControl("Lbl_pr_PDesc");
            int rowID = int.Parse(hf.Value);
            int employeeID = int.Parse(LitActID.Text);
            bool validData = true;
            string gpType = lbl.Text;

            if (gpType.Contains("AVG SUMMER HOURS"))
            {
                validData = Payroll_Controller.deleteSummerAveragePayroll(rowID, _modBy, _modOn);
            }
            else
            {
                validData = Payroll_Controller.deletePayroll(rowID, _modBy, _modOn);
            }

            int index = (int)Session["MPSelection"];

            if (validData == true)
            {
                LitPayrollMessage.Text = "The record has been DELETED.";
                loadEmployeeData(employeeID, index);
                GvPayrollDetails.EditIndex = -1;
                GvPayrollDetails.DataSource = Session["EmployeeCurrPayroll"];
                GvPayrollDetails.DataBind();
                MpePayrollDetails.Show();
            }
            else
            {
                LitPayrollMessage.Text = "An error occurred while trying to DELETE this record. Contact " + Branding.CompanyShortName + " if this issue continues.";
                MpePayrollDetails.Show();
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

        }
    }

    protected void GvPayrollDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GvPayrollDetails.EditIndex = -1;
        GvPayrollDetails.DataSource = Session["EmployeeCurrPayroll"];
        GvPayrollDetails.DataBind();
        MpePayrollDetails.Show();
    }

    protected void GvPayrollDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)GvPayrollDetails.Rows[e.RowIndex];
            HiddenField hf = (HiddenField)row.FindControl("Hf_pr_RowID2");
            TextBox txtHours = (TextBox)row.FindControl("Txt_pr_Hours");
            TextBox txtSdate = (TextBox)row.FindControl("Txt_pr_PStart");
            TextBox txtEdate = (TextBox)row.FindControl("Txt_pr_PEnd");
            bool validData = true;
            List<Payroll> tempList = (List<Payroll>)Session["EmployeeCurrPayroll"];
            int index = (int)Session["MPSelection"];
            int employeeID = int.Parse(LitActID.Text);
            int _employerID = int.Parse(HfDistrictID.Value);

            validData = errorChecking.validateTextBoxDecimalACAHours(txtHours, validData);
            validData = errorChecking.validateTextBoxDate(txtSdate, validData);
            validData = errorChecking.validateTextBoxDate(txtEdate, validData);

            if (validData == true)
            {
                int _rowID = 0;
                decimal _actHours = 0;
                DateTime? _sDate;
                DateTime? _eDate;
                string _history = null;
                Payroll currPayroll = null;
                string _modBy = HfUserName.Value;
                DateTime _modOn = DateTime.Now;
                bool successUpdate = false;

                _rowID = int.Parse(hf.Value);
                _actHours = decimal.Parse(txtHours.Text);
                _sDate = DateTime.Parse(txtSdate.Text);
                _eDate = DateTime.Parse(txtEdate.Text);

                currPayroll = Payroll_Controller.getSinglePayroll(_rowID, tempList);
                _history = currPayroll.PAY_HISTORY + Environment.NewLine;
                _history += "Record Altered on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                _history += "Old Values: " + Environment.NewLine;
                _history += "Hours: " + currPayroll.PAY_HOURS.ToString() + Environment.NewLine;
                _history += "Payroll Start Date: " + currPayroll.PAY_SDATE.ToString() + Environment.NewLine;
                _history += "Payroll End Date: " + currPayroll.PAY_EDATE.ToString() + Environment.NewLine;

                successUpdate = Payroll_Controller.updatePayroll(_rowID, _employerID, employeeID, _actHours, _sDate, _eDate, _modBy, _modOn, _history);

                if (successUpdate == true)
                {
                    loadEmployeeData(employeeID, index);
                    GvPayrollDetails.EditIndex = -1;
                    GvPayrollDetails.DataSource = Session["EmployeeCurrPayroll"];
                    GvPayrollDetails.DataBind();
                    LitPayrollMessage.Text = "The record has been updated.";
                    MpePayrollDetails.Show();
                }
                else
                {
                    LitPayrollMessage.Text = "An error occurred while trying to update this record. Contact " + Branding.CompanyShortName + " if this issue continues.";
                    MpePayrollDetails.Show();
                }
            }
            else
            {
                LitPayrollMessage.Text = "Please correct all the fields highlighted in red.";
                MpePayrollDetails.Show();
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            LitPayrollMessage.Text = "An error occurred while trying to update this record. Contact " + Branding.CompanyShortName + " if this issue continues.";
            MpePayrollDetails.Show();
        }
    }

    protected void GvPayrollDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPayrollDetails.PageIndex = e.NewPageIndex;
        GvPayrollDetails.DataSource = Session["EmployeeCurrPayroll"];
        GvPayrollDetails.DataBind();
        MpePayrollDetails.Show();
    }

    protected void GvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = e.Row;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hf = (HiddenField)row.FindControl("HfEmployeeID");
            Image img = (Image)row.FindControl("Image1");
            int _employeeID = int.Parse(hf.Value);

            List<Employee> tempList = (List<Employee>)Session["Employees"];
            Employee emp = EmployeeController.findEmployee(tempList, _employeeID);

            if (emp.EMPLOYEE_PLAN_YEAR_ID != 0)
            {
                alert_insurance ai = insuranceController.findSingleInsuranceOffer(emp.EMPLOYEE_PLAN_YEAR_ID, _employeeID);

                if (ai != null)
                {
                    if (ai.IALERT_ACCEPTED == true && ai.IALERT_OFFERED == true)
                    {
                        img.ImageUrl = "~/images/circle_green.png";
                        img.ToolTip = emp.EMPLOYEE_FULL_NAME + " currently is enrolled in an medical plan";
                    }
                    else
                    {
                        img.ImageUrl = "~/images/circle_red.png";
                        img.ToolTip = emp.EMPLOYEE_FULL_NAME + " is NOT enrolled in an medical plan";
                    }
                }
                else
                {
                    img.ImageUrl = "~/images/circle_red.png";
                    img.ToolTip = emp.EMPLOYEE_FULL_NAME + " is NOT enrolled in an medical plan";
                }
            }
            else
            {
                img.ImageUrl = "~/images/circle_red.png";
                img.ToolTip = emp.EMPLOYEE_FULL_NAME + " is NOT enrolled in an medical plan";
            }
        }
    }


    protected void LbViewDependents_Click(object sender, EventArgs e)
    {
        MpeDependents.Show();
        int _employeeID = int.Parse(LitActID.Text);
        List<Dependent> tempList = EmployeeController.manufactureEmployeeDependentList(_employeeID);
        GvDependents.DataSource = tempList;
        GvDependents.DataBind();
        Lit_dep_message.Text = "";
    }

    protected void GvDependents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        int _employeeID = int.Parse(LitActID.Text);
        GvDependents.EditIndex = -1;
        GvDependents.DataSource = EmployeeController.manufactureEmployeeDependentList(_employeeID);
        GvDependents.DataBind();
        MpeDependents.Show();
    }

    protected void GvDependents_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int _employeeID = int.Parse(LitActID.Text);
        GvDependents.EditIndex = e.NewEditIndex;
        GvDependents.DataSource = EmployeeController.manufactureEmployeeDependentList(_employeeID);
        GvDependents.DataBind();
        MpeDependents.Show();
    }

    protected void GvDependents_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)GvDependents.Rows[e.RowIndex];
            HiddenField hf = (HiddenField)row.FindControl("Hf_dep_id");
            int _dependentID = int.Parse(hf.Value);
            int _employeeID = int.Parse(LitActID.Text);
            bool validData = true;

            validData = EmployeeController.DeleteEmployeeDependent(_dependentID, _employeeID);

            if (validData == true)
            {
                Lit_dep_message.Text = "The record has been DELETED.";
                GvDependents.EditIndex = -1;
                GvDependents.DataSource = EmployeeController.manufactureEmployeeDependentList(_employeeID);
                GvDependents.DataBind();
                MpeDependents.Show();
            }
            else
            {
                Lit_dep_message.Text = "This Dependent is not able to be DELETED. It is tied to other records in the System.";
                MpeDependents.Show();
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            Lit_dep_message.Text = "An error occurred while trying to DELETE this record. Please contact " + Branding.CompanyShortName + " if this problem continues.";
            MpeDependents.Show();
        }
    }

    protected void GvDependents_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)GvDependents.Rows[e.RowIndex];
            HiddenField hf = (HiddenField)row.FindControl("Hf_dep_id2");
            TextBox txtFname = (TextBox)row.FindControl("Txt_dep_Fname");
            TextBox txtMname = (TextBox)row.FindControl("Txt_dep_Mname");
            TextBox txtLname = (TextBox)row.FindControl("Txt_dep_Lname");
            TextBox txtSSN = (TextBox)row.FindControl("Txt_dep_SSN");
            TextBox txtDOB = (TextBox)row.FindControl("Txt_dep_DOB");
            int _dependentID = int.Parse(hf.Value);
            int _employeeID = int.Parse(LitActID.Text);
            bool validData = true;
            string modby = HfUserName.Value;


            string _fname = null;
            string _mname = null;
            string _lname = null;
            string _ssn = null;
            string dob = null;
            DateTime? _dob = null;

            validData = errorChecking.validateTextBoxNull(txtFname, validData);
            validData = errorChecking.validateTextBoxNull(txtLname, validData);

            if (txtSSN.Text.Length > 0)
            {
                validData = errorChecking.validateTextBoxSSN(txtSSN, validData);
                _ssn = txtSSN.Text.Trim(new char[] { ' ', '"' });
            }
            else
            {
                txtSSN.BackColor = System.Drawing.Color.White;
            }

            if (txtDOB.Text.Length > 0)
            {
                validData = errorChecking.validateTextBoxDate(txtDOB, validData);
            }
            else
            {
                txtDOB.BackColor = System.Drawing.Color.White;
            }

            if (validData == true)
            {
                _fname = txtFname.Text;
                _mname = txtMname.Text;
                _lname = txtLname.Text;

                dob = txtDOB.Text;
                try
                {
                    _dob = DateTime.Parse(dob);
                }
                catch (Exception exception)
                {

                    this.Log.Warn("Suppressing errors.", exception);

                    _dob = null;

                }
                Dependent currDependent = EmployeeController.updateEmployeeDependent(_dependentID, _employeeID, _fname, _mname, _lname, _ssn, _dob, modby, 1);

                if (currDependent != null)
                {
                    Lit_dep_message.Text = "The record has been UPDATED.";
                    GvDependents.EditIndex = -1;
                    GvDependents.DataSource = EmployeeController.manufactureEmployeeDependentList(_employeeID);
                    GvDependents.DataBind();
                    MpeDependents.Show();
                }
                else
                {
                    Lit_dep_message.Text = "An error occurred while trying to UPDATE this record. Please try again and if the problem continues, contact " + Branding.CompanyShortName;
                    MpeDependents.Show();
                }
            }
            else
            {
                Lit_dep_message.Text = "Please correct any fields highlighted in RED.";
                MpeDependents.Show();
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            Lit_dep_message.Text = "An error occurred while validating data for this record. Please try again and if the problem continues, contact " + Branding.CompanyShortName;
            MpeDependents.Show();
        }
    }


    //*********************************************** Insurance Change Event **************************************
    /// <summary>
    /// This will display the employees current insurance offer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LnBtnInsuranceDetails_Click(object sender, EventArgs e)
    {
        GridViewRow row = GvEmployees.SelectedRow;
        HiddenField hf = null;
        alert_insurance ai = null;
        int _employeeID = 0;
        int _planYearID = 0;

        try
        {
            hf = (HiddenField)row.FindControl("HfEmployeeID");
            _employeeID = int.Parse(hf.Value);
            _planYearID = int.Parse(HfPlanYearID.Value);

            reset_io_view();

            ai = insuranceController.findSingleInsuranceOffer(_planYearID, _employeeID);

            if (ai != null)
            {
                errorChecking.setDropDownList(Ddl_io_Offered, ai.IALERT_OFFERED);
                Hf_io_rowID.Value = ai.ROW_ID.ToString();
                Hf_io_planYearID.Value = ai.IALERT_PLANYEARID.ToString();

                if (ai.IALERT_OFFERED == false)
                {
                    Pnl_io_Accepted.Visible = false;
                    Pnl_io_DateOffered.Visible = false;
                    Pnl_io_Effective.Visible = false;
                    Pnl_io_Plan.Visible = false;
                }
                else
                {
                    Pnl_io_Accepted.Visible = true;
                    Pnl_io_DateOffered.Visible = true;
                    Pnl_io_Effective.Visible = true;
                    Pnl_io_Plan.Visible = true;

                    errorChecking.setDropDownList(Ddl_io_Accepted, ai.IALERT_ACCEPTED);


                    Ddl_io_InsurancePlan.DataSource = insuranceController.manufactureInsuranceList(_planYearID);
                    Ddl_io_InsurancePlan.DataTextField = "INSURANCE_COMBO";
                    Ddl_io_InsurancePlan.DataValueField = "INSURANCE_ID";
                    Ddl_io_InsurancePlan.DataBind();
                    Ddl_io_InsurancePlan.Items.Add("Select");

                    errorChecking.setDropDownList(Ddl_io_InsurancePlan, (int)ai.IALERT_INSURANCE_ID);

                    Txt_io_AcceptedOffer.Text = ((DateTime)ai.IALERT_ACCEPTEDDATE).ToShortDateString();
                    Txt_io_DateOffered.Text = ((DateTime)ai.IALERT_OFFERED_ON).ToShortDateString();
                    Txt_io_InsuranceEffectiveDate.Text = ((DateTime)ai.IALERT_EFFECTIVE_DATE).ToShortDateString();
                    Txt_io_HraFlex.Text = ai.IALERT_FLEX_HRA.ToString();

                    loadContributionPlans((int)ai.IALERT_INSURANCE_ID);
                    errorChecking.setDropDownList(Ddl_io_Contribution, ai.IALERT_CONTRIBUTION_ID);
                }

                Lit_io_EmployeeName.Text = ai.EMPLOYEE_FULL_NAME;
                Lit_io_PayrollID.Text = ai.EMPLOYEE_EXT_ID;
                Lit_io_PlanYear.Text = LitPlanName.Text;

                Lit_io_MonthlyAvg.Text = "0.0";

                Employee employee = EmployeeController.findSingleEmployee(_employeeID);
                Measurement Meas = measurementController.getPlanYearMeasurement(ai.IALERT_EMPLOYERID, ai.IALERT_PLANYEARID, employee.EMPLOYEE_TYPE_ID);

                if (Meas != null)
                {
                    AverageHours average = (from hours in AverageHoursFactory.GetAllAverageHoursForEmployeeId(employee.EMPLOYEE_ID)
                                            where hours.IsNewHire == false && hours.MeasurementId == Meas.MEASUREMENT_ID
                                            select hours).FirstOrDefault();

                    if (average != null)
                    {
                        Lit_io_MonthlyAvg.Text = average.MonthlyAverageHours.ToString("0.##");
                    }
                }

                Txt_io_Comments.Text = ai.IALERT_NOTES;
                mpeEditInsurance.Show();
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "No Insurance Offer or Data exists for this time period.";
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            MpeWebMessage.Show();
            LitMessage.Text = "No Insurance Offer or Data exists for this time period.";
        }
    }

    /// <summary>
    /// This will set the flag of where the user is creating a change event or just editing a record due to a mistake.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Ddl_io_ChangeEvent_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;
        int eventType = 0;
        bool offer = false;

        validData = errorChecking.validateDropDownSelection(Ddl_io_ChangeEvent, validData);
        Txt_io_ChangeDate.Text = null;
        Txt_io_ChangeDate.BackColor = System.Drawing.Color.White;

        if (validData == true)
        {
            eventType = int.Parse(Ddl_io_ChangeEvent.SelectedItem.Value);
            offer = bool.Parse(Ddl_io_Offered.SelectedItem.Value);

            if (offer == true)
            {
                show_io_all();
            }
            else
            {
                show_io_not_offered();
            }

            switch (eventType)
            {

                case 1:
                    mpeEditInsurance.Show();
                    Txt_io_ChangeDate.Visible = false;
                    Txt_io_ChangeDate.Enabled = false;
                    Txt_io_InsuranceEffectiveDate.Enabled = true;
                    Lbl_io_ChangeEventDate.Visible = false;
                    LblInsuranceMessage.Text = "You are correcting an insurance offer.";
                    break;

                case 2:
                    mpeEditInsurance.Show();
                    Txt_io_ChangeDate.Visible = true;
                    Txt_io_ChangeDate.Enabled = true;
                    Txt_io_InsuranceEffectiveDate.Enabled = false;
                    Lbl_io_ChangeEventDate.Visible = true;
                    LblInsuranceMessage.Text = "This employee has encountered a situation where a change event has occurred.";
                    break;

                default:
                    mpeEditInsurance.Show();
                    LblInsuranceMessage.Text = "Please correct all red highlighted fields.";
                    break;

            }

        }
        else
        {

            loadEmployeeGridview();

            show_io_default();

            mpeEditInsurance.Show();

            LblInsuranceMessage.Text = "Please select Change Event or Correction.";

        }

    }

    /// <summary>
    /// If employee was offered insurance, display the accepted offer panel. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Ddl_io_Offered_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_io_Offered, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_io_ChangeEvent, validData);

        if (validData == true)
        {
            bool offer = bool.Parse(Ddl_io_Offered.SelectedItem.Value);

            if (offer == true)
            {
                int _planYearID = int.Parse(Hf_io_planYearID.Value);
                Ddl_io_InsurancePlan.DataSource = insuranceController.manufactureInsuranceList(_planYearID);
                Ddl_io_InsurancePlan.DataTextField = "INSURANCE_COMBO";
                Ddl_io_InsurancePlan.DataValueField = "INSURANCE_ID";
                Ddl_io_InsurancePlan.DataBind();
                Ddl_io_InsurancePlan.Items.Add("Select");
                Ddl_io_InsurancePlan.SelectedIndex = Ddl_io_InsurancePlan.Items.Count - 1;
                show_io_all();
            }
            else
            {
                show_io_not_offered();
            }
        }
        else
        {
            mpeEditInsurance.Show();
            LblInsuranceMessage.Text = "Please correct all red highlighted fields.";
        }
    }

    /// <summary>
    /// This will auto populate the Insurance Contribution Drop Down list if changed. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Ddl_io_InsurancePlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_io_InsurancePlan, validData);

        if (validData == true)
        {
            mpeEditInsurance.Show();
            loadContributionPlans(0);
        }
        else
        {
            mpeEditInsurance.Show();
            LblInsuranceMessage.Text = "Please correct all red highlighted fields.";
        }
    }

    private void show_io_all()
    {

        mpeEditInsurance.Show();

        Ddl_io_Offered.Enabled = true;
        Ddl_io_Accepted.Enabled = true;
        Ddl_io_InsurancePlan.Enabled = true;
        Ddl_io_Contribution.Enabled = true;

        Txt_io_Comments.Enabled = true;
        Txt_io_ChangeDate.Enabled = true;
        Txt_io_HraFlex.Enabled = true;

        Pnl_io_Accepted.Visible = true;
        Pnl_io_DateOffered.Visible = true;
        Pnl_io_Effective.Visible = true;
        Pnl_io_Plan.Visible = true;
        PnlInsurancePlanOffered.Visible = true;

        Btn_io_update.Enabled = true;
        Btn_io_update.Visible = true;

    }

    private void show_io_not_offered()
    {

        mpeEditInsurance.Show();

        Ddl_io_Offered.Enabled = true;
        Ddl_io_Accepted.Enabled = false;
        Ddl_io_InsurancePlan.Enabled = false;
        Ddl_io_Contribution.Enabled = false;

        Txt_io_Comments.Enabled = true;
        Txt_io_HraFlex.Enabled = false;

        Pnl_io_Accepted.Visible = false;
        Pnl_io_DateOffered.Visible = false;
        Pnl_io_Effective.Visible = false;

        Pnl_io_Plan.Visible = true;
        PnlInsurancePlanOffered.Visible = false;

        Btn_io_update.Enabled = true;
        Btn_io_update.Visible = true;

    }

    private void show_io_default()
    {
        Ddl_io_Offered.Enabled = false;
        Ddl_io_Accepted.Enabled = false;
        Ddl_io_InsurancePlan.Enabled = false;
        Ddl_io_Contribution.Enabled = false;

        Txt_io_Comments.Enabled = false;
        Txt_io_ChangeDate.Enabled = false;
        Txt_io_HraFlex.Enabled = false;

        Btn_io_update.Enabled = false;
        Btn_io_update.Visible = false;
        Txt_io_ChangeDate.Visible = false;
        Lbl_io_ChangeEventDate.Visible = false;
    }

    private void reset_io_view()
    {
        Ddl_io_ChangeEvent.SelectedIndex = Ddl_io_ChangeEvent.Items.Count - 1;
        Ddl_io_ChangeEvent.BackColor = System.Drawing.Color.White;
        LblInsuranceMessage.Text = null;
        Txt_io_ChangeDate.Text = null;
        Txt_io_ChangeDate.BackColor = System.Drawing.Color.White;
    }

    /// <summary>
    /// User cancels edit insurance offer screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        loadEmployeeGridview();
        show_io_default();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void DdlActStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlActStatus.SelectedItem.Text == "Termed")
        {
            mpAcaStatus.Show();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Btn_io_update_Click(object sender, EventArgs e)
    {
        int changeEvent = 0;
        int _rowID = 0;
        int _planYearID = 0;
        int _insuranceID = 0;
        int _contributionID = 0;
        int _employeeID = 0;
        int _employerID = 0;
        double _hraFlex = 0;

        double? _avgHours = null;
        bool? _offered = null;
        DateTime? _offeredOn = null;
        bool? _accepted = null;
        DateTime? _acceptedOn = null;
        DateTime _modOn = DateTime.Now;
        DateTime? _effectiveDate = null;
        DateTime? _newEffectiveDate = null;
        DateTime? _hireDate = null;
        string _modBy = HfUserName.Value;
        string _notes = null;
        string _history = null;
        bool validData = true;
        bool validTransaction = false;
        alert_insurance currInsOffer = null;
        Measurement currMeas = null;
        Employee currEmployee = null;
        PlanYear currPlanYear = null;

        validData = errorChecking.validateDropDownSelection(Ddl_io_ChangeEvent, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_io_Offered, validData);

        if (validData == true)
        {
            _employerID = int.Parse(HfDistrictID.Value);
            _employeeID = int.Parse(LitActID.Text);
            _rowID = int.Parse(Hf_io_rowID.Value);
            _planYearID = int.Parse(Hf_io_planYearID.Value);
            changeEvent = int.Parse(Ddl_io_ChangeEvent.SelectedItem.Value);
            _offered = bool.Parse(Ddl_io_Offered.SelectedItem.Value);

            currInsOffer = insuranceController.findSingleInsuranceOffer(_planYearID, _employeeID);

            if (changeEvent == 1)
            {
                validData = errorChecking.validateTextBoxDate(Txt_io_InsuranceEffectiveDate, validData);
            }

            if (changeEvent == 2)
            {
                validData = errorChecking.validateTextBoxDate(Txt_io_ChangeDate, validData);
                if (validData == true)
                {
                    _newEffectiveDate = DateTime.Parse(Txt_io_ChangeDate.Text);
                    TextBox txt = new TextBox();
                    txt.Text = currInsOffer.IALERT_EFFECTIVE_DATE.ToString();
                    if (txt.Text != null)
                    {
                        validData = errorChecking.validateTextBoxDateCompare(txt, Txt_io_ChangeDate, validData);
                    }
                }
            }

            if (_offered == true && validData == true)
            {
                currEmployee = EmployeeController.findSingleEmployee(_employeeID);
                currPlanYear = PlanYear_Controller.findPlanYear(_planYearID, _employerID);

                currMeas = measurementController.getPlanYearMeasurement(_employerID, _planYearID, currEmployee.EMPLOYEE_TYPE_ID);

                validData = errorChecking.validateDropDownSelection(Ddl_io_Contribution, validData);
                validData = errorChecking.validateDropDownSelection(Ddl_io_InsurancePlan, validData);
                validData = errorChecking.validateDropDownSelection(Ddl_io_Accepted, validData);
                validData = errorChecking.validateTextBoxDate(Txt_io_InsuranceEffectiveDate, validData);
                validData = errorChecking.validateTextBoxDecimal(Txt_io_HraFlex, validData);

                if (validData == true)
                {
                    _accepted = bool.Parse(Ddl_io_Accepted.SelectedItem.Value);
                    _effectiveDate = DateTime.Parse(Txt_io_InsuranceEffectiveDate.Text);
                    _offeredOn = _effectiveDate;
                    _acceptedOn = _effectiveDate;
                    _hireDate = currEmployee.EMPLOYEE_HIRE_DATE;
                    _insuranceID = int.Parse(Ddl_io_InsurancePlan.SelectedItem.Value);
                    _contributionID = int.Parse(Ddl_io_Contribution.SelectedItem.Value);
                    _hraFlex = double.Parse(Txt_io_HraFlex.Text);
                    _avgHours = currInsOffer.EMPLOYEE_AVG_HOURS;
                    _notes = Txt_io_Comments.Text;
                    _history = currInsOffer.IALERT_HISTORY + Environment.NewLine;
                    _history += "Record Edited on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                    _history += "Old Values: Offered: " + currInsOffer.IALERT_OFFERED.ToString() + Environment.NewLine;
                    _history += "Accepted: " + currInsOffer.IALERT_ACCEPTED.ToString() + Environment.NewLine;
                    _history += "Effective Date: " + currInsOffer.IALERT_EFFECTIVE_DATE.ToString() + Environment.NewLine;
                    _history += "Insurance ID: " + currInsOffer.IALERT_INSURANCE_ID.ToString() + Environment.NewLine;
                    _history += "Contribution ID: " + currInsOffer.IALERT_CONTRIBUTION_ID.ToString() + Environment.NewLine;
                    _history += "HRA-Flex Contribution: " + currInsOffer.IALERT_FLEX_HRA.ToString() + Environment.NewLine;

                    validData = errorChecking.validateInsuranceOfferDates((DateTime)_offeredOn, currMeas, (DateTime)_acceptedOn, (DateTime)_effectiveDate, validData, LblInsuranceMessage, (DateTime)_hireDate, Txt_io_DateOffered, Txt_io_AcceptedOffer, Txt_io_InsuranceEffectiveDate);

                    if (validData == true)
                    {
                        switch (changeEvent)
                        {
                            case 1:
                                validTransaction = insuranceController.updateInsuranceOffer(_rowID, _insuranceID, _contributionID, _avgHours, _offered, _offeredOn, _accepted, _acceptedOn, _modOn, _modBy, _notes, _history, _effectiveDate, _hraFlex);
                                break;
                            case 2:
                                _history = "Insurance Change Event created on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                                validTransaction = insuranceController.TransferInsuranceChangeEvent(_rowID, _insuranceID, _contributionID, _avgHours, _offered, _offeredOn, _accepted, _acceptedOn, _modOn, _modBy, _notes, _history, _newEffectiveDate, _hraFlex);
                                break;
                            default:
                                validTransaction = false;
                                break;
                        }

                        if (validTransaction == true)
                        {
                            loadEmployeeGridview();
                            show_io_default();
                            mpeEditInsurance.Hide();
                            LblInsuranceMessage.Text = "Insurance Offer Date is updated.";
                        }
                        else
                        {
                            mpeEditInsurance.Show();
                            LblInsuranceMessage.Text = "An ERROR occurred while trying to update this record.";
                        }
                    }
                    else
                    {
                        mpeEditInsurance.Show();
                    }
                }
                else
                {
                    mpeEditInsurance.Show();
                }
            }
            else if (_offered == false && validData == true)
            {
                _offered = false;
                _offeredOn = null;
                _accepted = null;
                _acceptedOn = null;
                _effectiveDate = null;
                _insuranceID = 0;
                _contributionID = 0;
                _hraFlex = 0;
                _avgHours = currInsOffer.EMPLOYEE_AVG_HOURS;
                _notes = Txt_io_Comments.Text;
                _history = currInsOffer.IALERT_HISTORY + Environment.NewLine;
                _history += "Record Edited on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                _history += "Insurance was NOT offered.";

                switch (changeEvent)
                {
                    case 1:
                        validTransaction = insuranceController.updateInsuranceOffer(_rowID, _insuranceID, _contributionID, _avgHours, _offered, _offeredOn, _accepted, _acceptedOn, _modOn, _modBy, _notes, _history, _effectiveDate, _hraFlex);
                        break;
                    case 2:
                        _history = "Insurance Change Event created on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                        validTransaction = insuranceController.TransferInsuranceChangeEvent(_rowID, _insuranceID, _contributionID, _avgHours, _offered, _offeredOn, _accepted, _acceptedOn, _modOn, _modBy, _notes, _history, _newEffectiveDate, _hraFlex);
                        break;
                    default:
                        validTransaction = false;
                        break;
                }

                if (validTransaction == true)
                {
                    loadEmployeeGridview();
                    show_io_default();
                    mpeEditInsurance.Hide();
                    LblInsuranceMessage.Text = null;
                }
                else
                {
                    mpeEditInsurance.Show();
                    LblInsuranceMessage.Text = "An ERROR occurred while trying to update this record.";
                }
            }
            else
            {
                mpeEditInsurance.Show();
                LblInsuranceMessage.Text = "Please correct all red highlighted fields.";
            }
        }
        else
        {
            mpeEditInsurance.Show();
            LblInsuranceMessage.Text = "Please correct all red highlighted fields.";
        }
    }

}