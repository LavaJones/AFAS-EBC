using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using AjaxControlToolkit;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;

public partial class securepages_s_setup : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(securepages_s_setup));


    #region Load Functions
    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        loadCurrentUser();

        int _empID = 0;
        int index = 0;

        try
        {
            index = int.Parse(Request.QueryString["code"]);
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            index = 0;
        }

        loadLinks();
        loadStates();
        loadWaitingPeriods();
        _empID = loadEmployerProfile();
        loadEmployeeTypes(_empID);
        GvSetup.SelectedIndex = index;
        PnlInitialMeasurement.Visible = true;
        PnlEquivalency.Visible = false;
        PnlMeasurement.Visible = false;
        PnlUsers.Visible = false;
        PnlDistrictProfile.Visible = false;
        PnlInsurance.Visible = false;
        PnlPlanYear.Visible = false;
        PnlAlerts.Visible = false;

        loadPlanYears();
        employerType(employer.EMPLOYER_ID);
        loadSetup();

        int employerId = int.Parse(HfDistrictID.Value);
        List<alert> alertItem = alert_controller.manufactureEmployerAlertListAll(employerId);
        int Sum = 0;

        for (int i = 0; i < alertItem.Count; ++i)

        {
            Sum += (alertItem[i].ALERT_COUNT);
        }
        if (Sum < 99)
        {
            CountAlert.Text = Sum.ToString();
        }
        else
        {
            CountAlert.Text = "99+";
        }
    }

    private void employerType(int _employerID)
    {
        employer emp = employerController.getEmployer(_employerID);
        if (emp.HasBreaksInService())
        {
            PnlSummerWindow.Visible = true;
        }
        else
        {
            PnlSummerWindow.Visible = false;
        }
    }

    private void loadUnits()
    {
        DdlEquivUnit.DataSource = UnitController.getEmployerUnits();
        DdlEquivUnit.DataTextField = "UNIT_NAME";
        DdlEquivUnit.DataValueField = "UNIT_ID";
        DdlEquivUnit.DataBind();

        DdlEquivUnit.Items.Add("Select");
        DdlEquivUnit.SelectedIndex = DdlEquivUnit.Items.Count - 1;
    }

    private void loadWaitingPeriods()
    {
        Ddl_ec_WaitingPeriod.DataSource = insuranceController.manufactureWaitingPeriod();
        Ddl_ec_WaitingPeriod.DataTextField = "description";
        Ddl_ec_WaitingPeriod.DataValueField = "id";
        Ddl_ec_WaitingPeriod.DataBind();

        Ddl_ec_WaitingPeriod.Items.Add("Select");
        Ddl_ec_WaitingPeriod.SelectedIndex = Ddl_ec_WaitingPeriod.Items.Count - 1;
    }

    private void loadDefaultOfferCodes()
    {
        Ddl_ec_offerCode.DataSource = airController.ManufactureOOCList(true);
        Ddl_ec_offerCode.DataTextField = "OOC_ID";
        Ddl_ec_offerCode.DataValueField = "OOC_ID";
        Ddl_ec_offerCode.DataBind();

        Ddl_ec_offerCode.Items.Add("Select");
        Ddl_ec_offerCode.SelectedIndex = Ddl_ec_offerCode.Items.Count - 1;
    }

    private void loadSetup()
    {
        HiddenField hfID = null;
        int selectedIndex = GvSetup.SelectedIndex;
        GridViewRow row = GvSetup.Rows[selectedIndex];
        int selValue = 0;

        hfID = (HiddenField)row.FindControl("HfReportID");
        selValue = System.Convert.ToInt32(hfID.Value);
        int _employerID = System.Convert.ToInt32(HfDistrictID.Value);
        switch (selValue)
        {
            case 1:   
                PnlInitialMeasurement.Visible = true;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                loadInitialMeasurementPeriods();
                break;
            case 2:   
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = true;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                loadMeasurementTypes();
                loadMeasurements(2);
                HfMeasurementTypeID.Value = "2";
                break;
            case 3:   
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = true;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                loadMeasurementTypes();
                loadMeasurements(1);
                HfMeasurementTypeID.Value = "1";
                break;
            case 4: 
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = true;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                loadGrossPayDescriptions();
                loadEmployerEquivalencies();
                PnlEmployeeClassification.Visible = false;
                loadUnits();
                break;
            case 5: 
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = true;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                loadDistrictUsers();
                break;
            case 6:  
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = true;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                break;
            case 7:  
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = true;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                loadPlanYears();
                loadGvPlanYears();
                break;
            case 8:  
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = true;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                loadInsurance(0);
                break;
            case 9: 
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = true;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                loadAlerts();
                break;
            case 10:    
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = true;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = false;
                loadGrossPayDescriptions();
                loadGPFilter();
                break;
            case 11:  
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = false;
                PnlEmployeeClassification.Visible = true;
                loadClassifications();
                loadDefaultOfferCodes();
                loadAshCodes(Ddl_ec_ASH);
                break;
            case 12:   
                PnlInitialMeasurement.Visible = false;
                PnlMeasurement.Visible = false;
                PnlEquivalency.Visible = false;
                PnlUsers.Visible = false;
                PnlDistrictProfile.Visible = false;
                PnlAlerts.Visible = false;
                PnlInsurance.Visible = false;
                PnlPlanYear.Visible = false;
                PnlGrossPayExclusion.Visible = false;
                PnlInsuranceContribution.Visible = true;
                PnlEmployeeClassification.Visible = false;
                loadInsurance(1);
                loadContributionTypes();
                loadEmployerContributions();
                loadClassificationsDropDown(Ddl_nec_EmployeeClass);
                break;
            default:
                break;
        }
    }


    private List<classification> loadClassifications()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<classification> classList = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        List<classification> activeList = classificationController.getClassificationActiveOnly(classList);
        GvEmployeeClassifications.DataSource = activeList;
        GvEmployeeClassifications.DataBind();

        litClassesShown.Text = GvEmployeeClassifications.Rows.Count.ToString();
        litClassesCount.Text = classList.Count.ToString();

        return classList;
    }

    private void loadClassificationsDropDown(DropDownList ddl)
    {

        int _employerID = int.Parse(HfDistrictID.Value);

        ddl.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        ddl.DataTextField = "CLASS_DESC";
        ddl.DataValueField = "CLASS_ID";
        ddl.DataBind();

        ddl.Items.Add("Select");
        ddl.SelectedIndex = ddl.Items.Count - 1;

    }

    private void loadInsuranceType(DropDownList ddl)
    {
        ddl.DataSource = insuranceController.GetInsuranceTypes(true);
        ddl.DataTextField = "INSURANCE_TYPE_NAME";
        ddl.DataValueField = "INSURANCE_TYPE_ID";
        ddl.DataBind();

        ddl.Items.Add("Select");
        ddl.SelectedIndex = ddl.Items.Count - 1;
    }

    private void loadAshCodes(DropDownList ddl)
    {
        ddl.DataSource = employerController.get4980HReliefCodes();
        ddl.DataTextField = "ASH_ID_DESCRIPTION";
        ddl.DataValueField = "ASH_ID";
        ddl.DataBind();

        ddl.Items.Add("Select");
        ddl.SelectedIndex = ddl.Items.Count - 1;
    }

    private void loadAlerts()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<alert> alertList = alert_controller.manufactureEmployerAlertListAll(_employerID);

        GvAlerts.DataSource = alertList;
        GvAlerts.DataBind();
    }

    private void loadGPFilter()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<gpType> gpTypeList = gpType_Controller.manufactureGrossPayFilter(_employerID);
        GvGrossPayExclusion.DataSource = gpTypeList;
        GvGrossPayExclusion.DataBind();

        LitGpExcShown.Text = GvGrossPayExclusion.Rows.Count.ToString();
        LitGpExcCount.Text = gpTypeList.Count.ToString();

        DdlGrossPayFilter.SelectedIndex = DdlGrossPayFilter.Items.Count - 1;
    }

    private void loadLinks()
    {
        demo d = new demo();
        GvSetup.DataSource = d.getSetupLinks();
        GvSetup.DataBind();
    }

    private void loadInitialMeasurementPeriods()
    {
        DdlInitialLength.DataSource = measurementController.getInitialMeasurements();
        DdlInitialLength.DataTextField = "INITIAL_NAME";
        DdlInitialLength.DataValueField = "INITIAL_ID";
        DdlInitialLength.DataBind();

        employer currEmployer = (employer)Session["CurrentDistrict"];

        int initID = currEmployer.EMPLOYER_INITIAL_MEAS_ID;

        if (initID > 0)
        {
            DdlInitialLength.SelectedIndex = currEmployer.EMPLOYER_INITIAL_MEAS_ID - 1;
        }
        else
        {

        }
    }

    private void loadInsurance(int panelID)
    {
        if (DdlPlanYear == null || DdlPlanYear.SelectedItem == null || DdlPlanYear.SelectedItem.Value == null)
        {
            return;
        }

        int _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
        List<insurance> tempList = insuranceController.getAllActiveInsurancePlansByPlanYear(_planYearID);

        switch (panelID)
        {
            case 0:
                GvInsurance.DataSource = tempList;
                GvInsurance.DataBind();

                LitInsuranceCount.Text = tempList.Count.ToString();
                LitInsuranceDisplay.Text = GvInsurance.Rows.Count.ToString();

                break;
            case 1:
                Ddl_ic_InsurancePlans.DataSource = tempList;
                Ddl_ic_InsurancePlans.DataTextField = "INSURANCE_COMBO";
                Ddl_ic_InsurancePlans.DataValueField = "INSURANCE_ID";
                Ddl_ic_InsurancePlans.DataBind();

                Ddl_nec_InsurancePlans.DataSource = tempList;
                Ddl_nec_InsurancePlans.DataTextField = "INSURANCE_COMBO";
                Ddl_nec_InsurancePlans.DataValueField = "INSURANCE_ID";
                Ddl_nec_InsurancePlans.DataBind();
                Ddl_nec_InsurancePlans.Items.Add("Select");
                Ddl_nec_InsurancePlans.SelectedIndex = Ddl_nec_InsurancePlans.Items.Count - 1;
                break;
            default:
                break;
        }
    }

    private void loadContributionTypes()
    {
        Ddl_nec_ContributionMethod.DataSource = insuranceController.getContributionTypes();
        Ddl_nec_ContributionMethod.DataTextField = "CONT_NAME";
        Ddl_nec_ContributionMethod.DataValueField = "CONT_ID";
        Ddl_nec_ContributionMethod.DataBind();

        Ddl_nec_ContributionMethod.Items.Add("Select");
        Ddl_nec_ContributionMethod.SelectedIndex = Ddl_nec_ContributionMethod.Items.Count - 1;
    }

    private void loadStates()
    {
        DdlEmployerState.DataSource = StateController.getStates();
        DdlEmployerState.DataTextField = "State_Name";
        DdlEmployerState.DataValueField = "State_ID";
        DdlEmployerState.DataBind();
    }

    private void loadMeasurementTypes()
    {
        Ddl_M_MeasurementType.DataSource = measurementController.getMeasurementTypes();
        Ddl_M_MeasurementType.DataTextField = "MEASUREMENT_TYPE_NAME";
        Ddl_M_MeasurementType.DataValueField = "MEASUREMENT_TYPE_ID";
        Ddl_M_MeasurementType.DataBind();
        Ddl_M_MeasurementType.Items.Add("Select");
        Ddl_M_MeasurementType.SelectedIndex = Ddl_M_MeasurementType.Items.Count - 1;
    }

    private void loadEmployeeTypes(int _employeeID)
    {
        List<EmployeeType> employeeType = EmployeeTypeController.getEmployeeTypes(_employeeID);

        DdlEmployeeType.DataSource = employeeType;
        DdlEmployeeType.DataTextField = "EMPLOYEE_TYPE_NAME";
        DdlEmployeeType.DataValueField = "EMPLOYEE_TYPE_ID";
        DdlEmployeeType.DataBind();

        Ddl_M_EmployeeType.DataSource = employeeType;
        Ddl_M_EmployeeType.DataTextField = "EMPLOYEE_TYPE_NAME";
        Ddl_M_EmployeeType.DataValueField = "EMPLOYEE_TYPE_ID";
        Ddl_M_EmployeeType.DataBind();
        Ddl_M_EmployeeType.Items.Add("Select");
        Ddl_M_EmployeeType.SelectedIndex = Ddl_M_EmployeeType.Items.Count - 1;
    }

    private void loadGrossPayDescriptions()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<gpType> tempList = gpType_Controller.getEmployeeTypes(_employerID);
        DdlEquivGrossPayDesc.DataSource = tempList;
        DdlEquivGrossPayDesc.DataTextField = "GROSS_PAY_DESCRIPTION";
        DdlEquivGrossPayDesc.DataValueField = "GROSS_PAY_ID";
        DdlEquivGrossPayDesc.DataBind();

        DdlEquivGrossPayDesc.Items.Add("Select");
        DdlEquivGrossPayDesc.SelectedIndex = DdlEquivGrossPayDesc.Items.Count - 1;

        DdlGrossPayFilter.DataSource = tempList;
        DdlGrossPayFilter.DataTextField = "GROSS_PAY_DESCRIPTION";
        DdlGrossPayFilter.DataValueField = "GROSS_PAY_ID";
        DdlGrossPayFilter.DataBind();

        DdlGrossPayFilter.Items.Add("Select");
        DdlGrossPayFilter.SelectedIndex = DdlGrossPayFilter.Items.Count - 1;
    }

    private void loadPlanYears()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<PlanYear> planYears = PlanYear_Controller.getEmployerPlanYear(_employerID);

        DdlPlanYear.DataSource = planYears;
        DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
        DdlPlanYear.DataBind();

        Ddl_M_PlanYear.DataSource = planYears;
        Ddl_M_PlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        Ddl_M_PlanYear.DataValueField = "PLAN_YEAR_ID";
        Ddl_M_PlanYear.DataBind();
        Ddl_M_PlanYear.Items.Add("Select");
        Ddl_M_PlanYear.SelectedIndex = Ddl_M_PlanYear.Items.Count - 1;

        Ddl_I_PlanYear.DataSource = planYears;
        Ddl_I_PlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
        Ddl_I_PlanYear.DataValueField = "PLAN_YEAR_ID";
        Ddl_I_PlanYear.DataBind();
        Ddl_I_PlanYear.Items.Add("Select");
        Ddl_I_PlanYear.SelectedIndex = Ddl_I_PlanYear.Items.Count - 1;

    }

    private void loadGvPlanYears()
    {
        int _employerID = int.Parse(HfDistrictID.Value);

        List<PlanYear> pyList = PlanYear_Controller.getEmployerPlanYear(_employerID);

        GvPlanYears.DataSource = pyList;
        GvPlanYears.DataBind();

        LitPyShow.Text = GvPlanYears.Rows.Count.ToString();
        LitPyTotal.Text = pyList.Count.ToString();
    }

    private int loadEmployerProfile()
    {
        employer currEmployer = (employer)Session["CurrentDistrict"];

        TxtEmployerIrsName.Text = currEmployer.EMPLOYER_NAME;
        TxtEmployerDbaName.Text = currEmployer.DBAName;
        TxtEmployerAddress.Text = currEmployer.EMPLOYER_ADDRESS;
        TxtEmployerCity.Text = currEmployer.EMPLOYER_CITY;
        DdlEmployerState.Items.FindByValue(currEmployer.EMPLOYER_STATE_ID.ToString()).Selected = true;
        TxtEmployerZip.Text = currEmployer.EMPLOYER_ZIP;
        TxtEmployerEIN.Text = currEmployer.EMPLOYER_EIN;
        HfEmployerTypeID.Value = currEmployer.EMPLOYER_TYPE_ID.ToString();

        return currEmployer.EMPLOYER_ID;
    }

    private void loadMeasurements(int _measType)
    {
        int _employerID = 0;
        int _planYearID = 0;
        int _employeeTypeID = 0;
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlPlanYear, validData);

        if (validData == true)
        {
            _employerID = int.Parse(HfDistrictID.Value);
            _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
            _employeeTypeID = int.Parse(DdlEmployeeType.SelectedItem.Value);

            if (_measType == 1)
            {
                Session["tm"] = measurementController.manufactureMeasurementList(_employerID, _planYearID, _employeeTypeID, _measType);
                RptTransitionMeasurments.DataSource = (List<Measurement>)Session["tm"];
                RptTransitionMeasurments.DataBind();
            }
            else
            {
                Session["om"] = measurementController.manufactureMeasurementList(_employerID, _planYearID, _employeeTypeID, _measType);
                RptTransitionMeasurments.DataSource = (List<Measurement>)Session["om"];
                RptTransitionMeasurments.DataBind();
            }
        }
        else
        {
            UpPlanYear.Update();
            LitMessage.Text = "You must create a Stability Period before measurment periods can be set-up!";
            MpeWebPageMessage.Show();
        }
    }

    private void loadEmployerEquivalencies()
    {
        int _employerID = System.Convert.ToInt32(HfDistrictID.Value);
        List<equivalency> equivList = equivalencyController.manufactureEquivalencyList(_employerID);
        GvEquivlancies.DataSource = equivList;
        GvEquivlancies.DataBind();

        LitEquivShow.Text = GvEquivlancies.Rows.Count.ToString();
        LitEquivTotal.Text = equivList.Count.ToString();
    }

    #endregion


    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void GvSetup_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadSetup();
    }

    private void clearNewUserData()
    {
        TxtNewFName.Text = null;
        TxtNewLName.Text = null;
        TxtNewEmail.Text = null;
        TxtNewUserName.Text = null;
        TxtNewPassword.Text = null;
        TxtNewPassword2.Text = null;
        LblNewUserMessage.Text = null;
        TxtNewPhone.Text = null;
    }


    protected void BtnSaveProfile_Click(object sender, EventArgs e)
    {
        bool validData = true;
        bool updateConfirmed = false;

        validData = validEmployerProfile();

        if (validData == true)
        {
            int _id = System.Convert.ToInt32(HfDistrictID.Value);
            int _empType = System.Convert.ToInt32(HfEmployerTypeID.Value);
            string _name = TxtEmployerIrsName.Text;
            string _dbaName = TxtEmployerDbaName.Text;
            string _address = TxtEmployerAddress.Text;
            string _city = TxtEmployerCity.Text;
            int _stateID = System.Convert.ToInt32(DdlEmployerState.SelectedItem.Value);
            string _zip = TxtEmployerZip.Text;
            string _ein = TxtEmployerEIN.Text;

            updateConfirmed = employerController.updateEmployer(_id, _name, _address, _city, _stateID, _zip, "", _ein, _empType, _dbaName);

            if (updateConfirmed == true)
            {
                employer currDist = (employer)Session["CurrentDistrict"];

                currDist.EMPLOYER_NAME = _name;
                currDist.DBAName = _dbaName;
                currDist.EMPLOYER_ADDRESS = _address;
                currDist.EMPLOYER_CITY = _city;
                currDist.EMPLOYER_STATE_ID = _stateID;
                currDist.EMPLOYER_ZIP = _zip;
                currDist.EMPLOYER_EIN = _ein;

                Session["CurrentDistrict"] = currDist;

                loadEmployerProfile();
            }
            else
            {
            }
        }
        else
        {
        }
    }

    private bool validEmployerProfile()
    {
        bool validData = true;

        validData = errorChecking.validateTextBoxNull(TxtEmployerIrsName, validData);
        validData = errorChecking.validateTextBoxNull(TxtEmployerAddress, validData);
        validData = errorChecking.validateTextBoxNull(TxtEmployerCity, validData);
        validData = errorChecking.validateTextBoxZipCode(TxtEmployerZip, validData);
        validData = errorChecking.validateTextBoxEIN(TxtEmployerEIN, validData);

        if (TxtSummerBreakStart.Text != "" || TxtSummerBreakEnd.Text != "")
        {
            validData = errorChecking.validateTextBoxDate(TxtSummerBreakStart, validData);
            validData = errorChecking.validateTextBoxDate(TxtSummerBreakEnd, validData);
        }

        return validData;
    }

    private bool loadCurrentUser()
    {
        User currUser = (User)Session["CurrentUser"];

        if (currUser != null)
        {
            if (currUser.User_Power != true)
            {
                PnlUsers.Enabled = false;
                PnlDistrictProfile.Enabled = false;
            }

            LitUserName.Text = currUser.User_UserName;
            HfDistrictID.Value = currUser.User_District_ID.ToString();
            HfUserName.Value = currUser.User_UserName;
            employer emp = ((employer)Session["CurrentDistrict"]);
            LitEmployer.Text = emp.EMPLOYER_NAME;
            return true;
        }

        else
        {
            return false;
        }
    }

    protected void BtnSaveNewUser_Click(object sender, EventArgs e)
    {
        string _fname = null;
        string _lname = null;
        string _email = null;
        string _phone = null;
        string _userName = null;
        string _password = null;
        string _modBy = HfUserName.Value;
        DateTime _modDate = System.DateTime.Now;
        int _distID = 0;
        bool _power = false;
        bool _billing = false;
        bool _irsContact = false;
        User tempUser = null;
        List<User> tempList = (List<User>)Session["UserList"];


        if (validateNewUser() == true)
        {
            _fname = TxtNewFName.Text.Trim();
            _lname = TxtNewLName.Text.Trim();
            _email = TxtNewEmail.Text.Trim();
            _phone = TxtNewPhone.Text.Trim();
            _userName = TxtNewUserName.Text.Trim();
            _password = TxtNewPassword.Text.Trim();
            _distID = System.Convert.ToInt32(HfDistrictID.Value);
            _power = false;
            _billing = false;
            _irsContact = false;

            _userName = _userName.ToLower();

            tempUser = UserController.orderUser(_fname, _lname, _email, _phone, _userName, _password, _distID, _power, _modDate, _modBy, true, _billing, _irsContact);

            if (tempUser != null)
            {
                tempList.Add(tempUser);
                clearNewUserData();
                Session["UserList"] = tempList;
                loadDistrictUsers();
            }
            else
            {
                ModalPopupExtender2.Show();
                LblNewUserMessage.Text = "Username is invalid";
            }
        }
        else
        {
            ModalPopupExtender2.Show();
            LblNewUserMessage.Text = "Please correct all red fields.";
        }
    }

    private void loadDistrictUsers()
    {
        int _distID = System.Convert.ToInt32(HfDistrictID.Value);

        if (Session["UserList"] == null)
        {
            Session["UserList"] = UserController.getDistrictUsers(_distID);
        }
        List<User> userList = (List<User>)Session["UserList"];

        GvDistrictUsers.DataSource = userList;
        GvDistrictUsers.DataBind();

        LitUserShow.Text = GvDistrictUsers.Rows.Count.ToString();
        LitUserTotal.Text = userList.Count.ToString();
    }

    protected void GvDistrictUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = GvDistrictUsers.Rows[e.RowIndex];
        HiddenField hfID = (HiddenField)row.FindControl("HfUserID");
        int _userID = System.Convert.ToInt32(hfID.Value);
        bool updateConfirmed = false;

        updateConfirmed = UserController.deleteUser(_userID);

        if (updateConfirmed == true)
        {
            List<User> tempUsers = (List<User>)Session["UserList"];

            foreach (User u in tempUsers)
            {
                if (u.User_ID == _userID)
                {
                    tempUsers.Remove(u);
                    break;
                }
            }

            Session["UserList"] = tempUsers;

            loadDistrictUsers();
        }
        else
        {

        }
    }

    protected void GvDistrictUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = GvDistrictUsers.Rows[e.RowIndex];

        string _fname = null;
        string _lname = null;
        string _email = null;
        string _username = null;
        string _phone = null;
        bool _power = false;
        int _userID = 0;
        string _modBy = HfUserName.Value;
        DateTime _modOn = System.DateTime.Now;
        bool _billing = false;
        bool _irsContact = false;

        HiddenField hfID = (HiddenField)row.FindControl("HfUserID");
        HiddenField hfUserName = (HiddenField)row.FindControl("HfUserName");
        TextBox txtfname = (TextBox)row.FindControl("TxtmpFName");
        TextBox txtlname = (TextBox)row.FindControl("TxtmpLName");
        TextBox txtemail = (TextBox)row.FindControl("TxtmpEmail");
        TextBox txtphone = (TextBox)row.FindControl("TxtmpPhone");
        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("ModalPopupExtender3");
        CheckBox cb = (CheckBox)row.FindControl("CbtmpPowerUser");
        CheckBox cb2 = (CheckBox)row.FindControl("CbtmpBillingUser");
        CheckBox cb3 = (CheckBox)row.FindControl("CbtmpIRSContact");

        bool validData = true;

        validData = errorChecking.validateTextBoxNull(txtfname, validData);
        validData = errorChecking.validateTextBoxNull(txtlname, validData);
        validData = errorChecking.validateTextBoxEmail(txtemail, validData);
        validData = errorChecking.validateTextBoxPhone(txtphone, validData);

        if (validData == true)
        {
            bool updateConfirmed = false;
            _userID = System.Convert.ToInt32(hfID.Value);
            _username = hfUserName.Value;
            _fname = txtfname.Text;
            _lname = txtlname.Text;
            _email = txtemail.Text;
            _power = cb.Checked;
            _phone = txtphone.Text;
            _billing = cb2.Checked;
            _irsContact = cb3.Checked;

            updateConfirmed = UserController.updateUser(_userID, _fname, _lname, _email, _phone, _power, _modBy, _modOn, _billing, _irsContact);

            if (updateConfirmed == true)
            {
                User currUser = (User)Session["CurrentUser"];

                if (currUser.User_ID == _userID)
                {
                    currUser.User_First_Name = _fname;
                    currUser.User_Last_Name = _lname;
                    currUser.User_Email = _email;
                    currUser.User_UserName = _username;
                    currUser.User_Power = _power;
                    currUser.User_Phone = _phone;
                    currUser.LAST_MOD = _modOn;
                    currUser.LAST_MOD_BY = _modBy;
                    currUser.User_Billing = _billing;
                    currUser.User_IRS_CONTACT = _irsContact;

                    Session["CurrentUser"] = currUser;

                    loadCurrentUser();
                }

                List<User> tempUsers = (List<User>)Session["UserList"];
                foreach (User u in tempUsers)
                {
                    if (u.User_ID == _userID)
                    {
                        u.User_First_Name = _fname;
                        u.User_Last_Name = _lname;
                        u.User_Email = _email;
                        u.User_UserName = _username;
                        u.User_Phone = _phone;
                        u.User_Power = _power;
                        u.LAST_MOD = _modOn;
                        u.LAST_MOD_BY = _modBy;
                        u.User_Billing = _billing;
                        u.User_IRS_CONTACT = _irsContact;
                    }
                    UserController.updateUser(u.User_ID, u.User_First_Name, u.User_Last_Name, u.User_Email, u.User_Phone, u.User_Power, u.LAST_MOD_BY, (DateTime)u.LAST_MOD, u.User_Billing, u.User_IRS_CONTACT);
                }
                Session["UserList"] = tempUsers;
                loadDistrictUsers();
            }
        }
        else
        {
            mpe.Show();
        }
    }


    private bool validateNewUser()
    {
        bool validData = true;

        validData = errorChecking.validateTextBoxNull(TxtNewFName, validData);
        validData = errorChecking.validateTextBoxNull(TxtNewLName, validData);
        validData = errorChecking.validateTextBoxEmail(TxtNewEmail, validData);
        validData = errorChecking.validateTextBoxPhone(TxtNewPhone, validData);
        validData = errorChecking.validateTextBox6Length(TxtNewUserName, validData);
        validData = errorChecking.validateTextBoxPassword(TxtNewPassword, TxtNewPassword2, validData);

        return validData;
    }

    protected void RbEquivPayroll_CheckedChanged(object sender, EventArgs e)
    {

    }



    protected void DdlEquivPosition_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void DdlEquivActivity_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void RbEquivPayroll_gv_CheckedChanged(object sender, EventArgs e)
    {
        int index = GvEquivlancies.EditIndex;
        GridViewRow row = GvEquivlancies.Rows[index];

        Panel pnlGrossPay = (Panel)row.FindControl("PnlgpDescription_gv");
        Panel pnlDateRange = (Panel)row.FindControl("PnlgpDate_gv");
        TextBox txtSdate_gv = (TextBox)row.FindControl("TxtEquivStartDate_gv");
        TextBox txtEdate_gv = (TextBox)row.FindControl("TxtEquivEndDate_gv");
        AjaxControlToolkit.ModalPopupExtender mpe = (AjaxControlToolkit.ModalPopupExtender)row.FindControl("MpeEquivEdit");

        txtSdate_gv.Text = null;
        txtEdate_gv.Text = null;

        pnlGrossPay.Visible = true;
        pnlDateRange.Visible = false;
        mpe.Show();
    }

    protected void RbEquivDate_gv_CheckedChanged(object sender, EventArgs e)
    {
        int index = GvEquivlancies.EditIndex;
        GridViewRow row = GvEquivlancies.Rows[index];

        Panel pnlGrossPay = (Panel)row.FindControl("PnlgpDescription_gv");
        Panel pnlDateRange = (Panel)row.FindControl("PnlgpDate_gv");
        DropDownList ddl = (DropDownList)row.FindControl("DdlEquivGrossPayDesc_gv");
        AjaxControlToolkit.ModalPopupExtender mpe = (AjaxControlToolkit.ModalPopupExtender)row.FindControl("MpeEquivEdit");

        pnlGrossPay.Visible = false;
        pnlDateRange.Visible = true;
        mpe.Show();
    }

    protected void ImgBtnEquivEditCancel_Click(object sender, ImageClickEventArgs e)
    {
        loadEmployerEquivalencies();
    }

    protected void ImgBtnClose_Click(object sender, ImageClickEventArgs e)
    {
        resetEquivalencyPanel();
    }

    private void resetEquivalencyPanel()
    {
        DdlEquivGrossPayDesc.SelectedIndex = DdlEquivGrossPayDesc.Items.Count - 1;
        DdlEquivGrossPayDesc.BackColor = System.Drawing.Color.White;
        RbEquivPayroll.Checked = true;
        CbEquivActive.Checked = true;

        TxtEquivName.Text = null;
        TxtEquivName.BackColor = System.Drawing.Color.White;
        TxtEquivNotes.Text = null;
        TxtEquivNotes.BackColor = System.Drawing.Color.White;
        TxtEquivUnits.Text = null;
        TxtEquivUnits.BackColor = System.Drawing.Color.White;
        TxtEquivUnits2.Text = null;
        TxtEquivUnits2.BackColor = System.Drawing.Color.White;

        DdlEquivUnit.SelectedIndex = DdlEquivUnit.Items.Count - 1;
        DdlEquivUnit.BackColor = System.Drawing.Color.White;

        LblEquivError.Text = null;
    }


    private int checkDropDownValue(DropDownList ddl)
    {
        int value = 0;
        try
        {
            value = int.Parse(ddl.SelectedItem.Value);
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            value = 0;

        }

        return value;
    }


    #region EQUIVALENCY FUNCTIONS
    protected void DdlEquivPosition_gv_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = GvEquivlancies.EditIndex;
        GridViewRow row = GvEquivlancies.Rows[index];

        DropDownList ddlPosition = (DropDownList)row.FindControl("DdlEquivPosition_gv");
        DropDownList ddlActivity = (DropDownList)row.FindControl("DdlEquivActivity_gv");

        if (ddlPosition.SelectedItem.Text != "Select")
        {
            int _positionID = int.Parse(ddlPosition.SelectedItem.Value);
            ddlActivity.DataSource = equivalencyController.getEbcActivities(_positionID);
            ddlActivity.DataTextField = "ACTIVITY_NAME";
            ddlActivity.DataValueField = "ACTIVITY_ID";
            ddlActivity.DataBind();
            ddlActivity.Items.Add("Select");
            ddlActivity.SelectedIndex = ddlActivity.Items.Count - 1;
        }

        MpeEquivelancy.Show();
    }

    protected void DdlEquivActivity_gv_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DdlEquivDetail_gv_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void BtnSaveEquiv_Click(object sender, EventArgs e)
    {
        int _empID = 0;
        string _name = null;
        int _gpID = 0;
        decimal _every = 0;
        int _unitID = 0;
        decimal _credit = 0;
        DateTime? _sdate = null;
        DateTime? _edate = null;
        bool validData = true;
        string _modBy = null;
        DateTime _modOn = System.DateTime.Now;
        equivalency tempEquiv = null;
        string _notes = null;
        string _history = null;
        bool _active = false;
        int _typeID = 0;
        string _typeName = null;
        string _unitName = null;
        int _positionID = 0;
        int _activityID = 0;
        int _detailID = 0;

        validData = errorChecking.validateTextBoxNull(TxtEquivName, validData);
        validData = errorChecking.validateTextBoxDecimal(TxtEquivUnits2, validData);
        validData = errorChecking.validateTextBoxDecimal(TxtEquivUnits, validData);
        validData = errorChecking.validateDropDownSelection(DdlEquivUnit, validData);

        if (RbEquivPayroll.Checked == true)
        {
            validData = errorChecking.validateDropDownSelection(DdlEquivGrossPayDesc, validData);
            if (validData == true)
            {
                _gpID = int.Parse(DdlEquivGrossPayDesc.SelectedItem.Value);
                _typeID = 1;
                _typeName = RbEquivPayroll.Text;
            }
        }
        else
        {
        }

        if (validData == true)
        {
            _empID = System.Convert.ToInt32(HfDistrictID.Value);
            _name = TxtEquivName.Text;
            _every = System.Convert.ToDecimal(TxtEquivUnits2.Text);
            _unitID = System.Convert.ToInt32(DdlEquivUnit.SelectedItem.Value);
            _credit = System.Convert.ToDecimal(TxtEquivUnits.Text);
            _modBy = LitUserName.Text;
            _notes = TxtEquivNotes.Text;
            _history = "Created by: " + _modBy + " on " + _modOn;
            _unitName = DdlEquivUnit.SelectedItem.Text;

            _active = CbEquivActive.Checked;

            tempEquiv = equivalencyController.manufactureEquivalency(_empID, _name, _gpID, _every, _unitID, _credit, _sdate, _edate, _notes, _modBy, _modOn, _history, _active, _typeID, _typeName, _unitName, _positionID, _activityID, _detailID);
            loadEmployerEquivalencies();
        }
        else
        {
            MpeEquivelancy.Show();
            LblEquivError.Text = "Please correct all red fields.";
        }

    }
    protected void GvEquivlancies_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GvEquivlancies.EditIndex = e.NewEditIndex;
        loadEmployerEquivalencies();

        int index = GvEquivlancies.EditIndex;
        GridViewRow row = GvEquivlancies.Rows[index];

        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("MpeEquivEdit");
        mpe.Show();
    }

    protected void GvEquivlancies_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = (GridViewRow)GvEquivlancies.Rows[e.RowIndex];

        HiddenField hf = (HiddenField)row.FindControl("HvEquivID");
        int equivID = int.Parse(hf.Value);
        bool validTransaction = true;

        validTransaction = equivalencyController.deleteEquivalency(equivID);
        loadEmployerEquivalencies();

    }

    protected void GvEquivlancies_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvEquivlancies.PageIndex = e.NewPageIndex;
        loadEmployerEquivalencies();
    }

    protected void GvEquivlancies_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int _employerID = int.Parse(HfDistrictID.Value);
            GridViewRow row = e.Row;

            DropDownList ddl = null;
            RadioButton rbGrossPay = null;
            Panel pnlGrossPay = null;
            HiddenField hfGrossPayID = null;
            HiddenField hfUnitID = null;
            HiddenField hfTypeID = null;
            DropDownList ddl2 = null;
            HiddenField hfPosID = null;
            HiddenField hfActID = null;
            HiddenField hfDetID = null;

            int employerID = System.Convert.ToInt32(HfDistrictID.Value);

            ddl = (DropDownList)row.FindControl("DdlEquivGrossPayDesc_gv");
            rbGrossPay = (RadioButton)row.FindControl("RbEquivPayroll_gv");
            pnlGrossPay = (Panel)row.FindControl("PnlgpDescription_gv");
            ddl2 = (DropDownList)row.FindControl("DdlEquivUnit_gv");
            hfGrossPayID = (HiddenField)row.FindControl("HfEquivGpID");
            hfUnitID = (HiddenField)row.FindControl("HfEquivUnitID");
            hfTypeID = (HiddenField)row.FindControl("HfEquivTypeID");
            hfPosID = (HiddenField)row.FindControl("HfEBCPosID");
            hfActID = (HiddenField)row.FindControl("HfEBCActID");
            hfDetID = (HiddenField)row.FindControl("HfEBCDetID");

            int _typeID = int.Parse(hfTypeID.Value);

            ddl2.DataSource = UnitController.getEmployerUnits();
            ddl2.DataTextField = "UNIT_NAME";
            ddl2.DataValueField = "UNIT_ID";
            ddl2.DataBind();

            ddl.DataSource = gpType_Controller.getEmployeeTypes(employerID);
            ddl.DataTextField = "GROSS_PAY_DESCRIPTION";
            ddl.DataValueField = "GROSS_PAY_ID";
            ddl.DataBind();

            ddl.Items.Add("Select");
            ddl.SelectedIndex = ddl.Items.Count - 1;


            ListItem li4 = ddl2.Items.FindByValue(hfUnitID.Value);
            if (li4 != null)
            {
                ddl2.ClearSelection();
                li4.Selected = true;
            }

            if (rbGrossPay.Checked == true)
            {
                pnlGrossPay.Visible = true;
                ListItem li2 = ddl.Items.FindByValue(hfGrossPayID.Value);
                if (li2 != null)
                {
                    ddl.ClearSelection();
                    li2.Selected = true;
                }
            }
            else
            {
                pnlGrossPay.Visible = false;
            }
        }
    }

    protected void GvEquivlancies_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int index = GvEquivlancies.EditIndex;
        GridViewRow row = GvEquivlancies.Rows[index];

        HiddenField hfID = null;
        RadioButton rbDateRange = null;
        RadioButton rbGrossPay = null;
        DropDownList ddlPosition = null;
        DropDownList ddlActivity = null;
        DropDownList ddlDetail = null;
        DropDownList ddlGrossPay = null;
        TextBox txtStartDate = null;
        TextBox txtEndDate = null;
        TextBox txtName = null;
        CheckBox cbActive = null;
        TextBox txtNotes = null;
        TextBox txtEvery = null;
        DropDownList ddlUnit = null;
        TextBox txtCredit = null;
        ModalPopupExtender mpeEdit = null;

        hfID = (HiddenField)row.FindControl("HvEquivID");
        rbDateRange = (RadioButton)row.FindControl("RbEquivDate_gv");
        rbGrossPay = (RadioButton)row.FindControl("RbEquivPayroll_gv");
        ddlPosition = (DropDownList)row.FindControl("DdlEquivPosition_gv");
        ddlActivity = (DropDownList)row.FindControl("DdlEquivActivity_gv");
        ddlDetail = (DropDownList)row.FindControl("DdlEquivDetail_gv");
        ddlGrossPay = (DropDownList)row.FindControl("DdlEquivGrossPayDesc_gv");
        txtStartDate = (TextBox)row.FindControl("TxtEquivStartDate_gv");
        txtEndDate = (TextBox)row.FindControl("TxtEquivEndDate_gv");
        txtName = (TextBox)row.FindControl("TxtEquivName_gv");
        cbActive = (CheckBox)row.FindControl("CbEquivActive_gv");
        txtNotes = (TextBox)row.FindControl("TxtEquivNotes_gv");
        txtEvery = (TextBox)row.FindControl("TxtEquivUnits2_gv");
        ddlUnit = (DropDownList)row.FindControl("DdlEquivUnit_gv");
        txtCredit = (TextBox)row.FindControl("TxtEquivUnits_gv");
        mpeEdit = (ModalPopupExtender)row.FindControl("MpeEquivEdit");

        int _equivID = 0;
        int _empID = 0;
        string _name = null;
        string _extID = null;
        decimal _every = 0;
        int _unitID = 0;
        decimal _credit = 0;
        DateTime? _sdate = null;
        DateTime? _edate = null;
        bool validData = true;
        string _modBy = null;
        DateTime _modOn = System.DateTime.Now;
        equivalency tempEquiv = null;
        string _notes = null;
        string _history = null;
        bool _active = false;
        int _typeID = 0;
        string _typeName = null;
        string _unitName = null;
        int _positionID = 0;
        int _activityID = 0;
        int _detailID = 0;

        validData = errorChecking.validateTextBoxNull(txtName, validData);
        validData = errorChecking.validateTextBoxDecimal(txtEvery, validData);
        validData = errorChecking.validateTextBoxDecimal(txtCredit, validData);
        validData = errorChecking.validateDropDownSelection(ddlUnit, validData);

        if (rbGrossPay.Checked == true)
        {
            _typeID = 1;
            _typeName = rbGrossPay.Text;
            validData = errorChecking.validateDropDownSelection(ddlGrossPay, validData);
            _extID = ddlGrossPay.SelectedItem.Value;
        }
        else
        {
            _typeID = 2;
            _typeName = rbDateRange.Text;
            validData = errorChecking.validateTextBoxDate(txtStartDate, validData);
            validData = errorChecking.validateTextBoxDate(txtEndDate, validData);
            _extID = "0";
        }

        if (validData == true)
        {
            _equivID = int.Parse(hfID.Value);
            _empID = System.Convert.ToInt32(HfDistrictID.Value);
            _name = txtName.Text;
            _every = System.Convert.ToDecimal(txtEvery.Text);
            _unitID = System.Convert.ToInt32(ddlUnit.SelectedItem.Value);
            _credit = System.Convert.ToDecimal(txtCredit.Text);
            _modBy = LitUserName.Text;
            _notes = txtNotes.Text;
            _history = "Created by: " + _modBy + " on " + _modOn;
            _unitName = ddlUnit.SelectedItem.Text;

            _positionID = checkDropDownValue(ddlPosition);
            _activityID = checkDropDownValue(ddlActivity);
            _detailID = checkDropDownValue(ddlDetail);



            try
            {
                _sdate = System.Convert.ToDateTime(txtStartDate.Text);
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                _sdate = null;
            }

            try
            {
                _edate = System.Convert.ToDateTime(txtEndDate.Text);
            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                _edate = null;

            }

            _active = cbActive.Checked;

            tempEquiv = equivalencyController.updateEquivalency(_equivID, _empID, _name, _extID, _every, _unitID, _credit, _sdate, _edate, _notes, _modBy, _modOn, _history, _active, _typeID, _typeName, _unitName, _positionID, _activityID, _detailID);
            loadEmployerEquivalencies();
        }
        else
        {
            mpeEdit.Show();

        }
    }

    #endregion

    #region MEASUREMENT PERIOD FUNCTIONS
    protected void Ddl_M_MeasurementType_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);

        List<PlanYear> tempList = PlanYear_Controller.getEmployerPlanYear(_employerID); ;
        PlanYear tempPY = null;

        if (Ddl_M_PlanYear.SelectedItem.Text != "Select" && Ddl_M_EmployeeType.SelectedItem.Text != "Select" && Ddl_M_MeasurementType.SelectedItem.Text != "Select")
        {
            int planYearID = int.Parse(Ddl_M_PlanYear.SelectedItem.Value);
            employer currEmployer = (employer)Session["CurrentDistrict"];

            foreach (PlanYear py in tempList)
            {
                if (py.PLAN_YEAR_ID == planYearID)
                {
                    tempPY = py;
                    break;
                }
            }
            DateTime plan_start = System.Convert.ToDateTime(tempPY.PLAN_YEAR_START);
            DateTime plan_end = System.Convert.ToDateTime(tempPY.PLAN_YEAR_END);

            LitEndDate.Text = plan_end.ToShortDateString();
            LitStartDate.Text = plan_start.ToShortDateString();

            MpeMeasurementPeriod.Show();
        }
    }

    protected void BtnSaveMeasurementPeriod_Click(object sender, EventArgs e)
    {
        if (false == Feature.SelfMeasurementPeriodsEnabled) { return; }    

        bool validData = true;
        int _employerID = 0;
        int _planID = 0;
        int _employeeTypeID = 0;
        int _measurementTypeID = 0;
        DateTime _meas_start = System.DateTime.Now.AddYears(-50);
        DateTime _meas_end = System.DateTime.Now.AddYears(-50);
        DateTime _admin_start = System.DateTime.Now.AddYears(-50);
        DateTime _admin_end = System.DateTime.Now.AddYears(-50);
        DateTime _open_start = System.DateTime.Now.AddYears(-50);
        DateTime _open_end = System.DateTime.Now.AddYears(-50);
        DateTime _stab_start = System.DateTime.Now.AddYears(-50);
        DateTime _stab_end = System.DateTime.Now.AddYears(-50);
        string _notes = null;
        string _modBy = null;
        DateTime _modOn = System.DateTime.Now;
        string _history = null;
        int measurementID = 0;
        DateTime? _swStart = null;
        DateTime? _swEnd = null;
        DateTime? _swStart2 = null;
        DateTime? _swEnd2 = null;

        validData = errorChecking.validateDropDownSelection(Ddl_M_PlanYear, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_M_EmployeeType, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_M_MeasurementType, validData);
        validData = errorChecking.validateTextBoxDate(Txt_M_Admin_end, validData);
        validData = errorChecking.validateTextBoxDate(Txt_M_Admin_start, validData);
        validData = errorChecking.validateTextBoxDate(Txt_M_Meas_end, validData);
        validData = errorChecking.validateTextBoxDate(Txt_M_Meas_start, validData);
        validData = errorChecking.validateTextBoxDate(Txt_M_Open_end, validData);
        validData = errorChecking.validateTextBoxDate(Txt_M_Open_start, validData);
        validData = errorChecking.validateTextBoxDate(Txt_M_Stab_end, validData);
        validData = errorChecking.validateTextBoxDate(Txt_M_Stab_start, validData);

        _employerID = int.Parse(HfDistrictID.Value);
        employer emp = employerController.getEmployer(_employerID);

        if (validData == true)
        {
            _planID = int.Parse(Ddl_M_PlanYear.SelectedItem.Value);
            _employeeTypeID = int.Parse(Ddl_M_EmployeeType.SelectedItem.Value);
            _measurementTypeID = int.Parse(Ddl_M_MeasurementType.SelectedItem.Value);
            _meas_start = System.Convert.ToDateTime(Txt_M_Meas_start.Text);
            _meas_end = System.Convert.ToDateTime(Txt_M_Meas_end.Text);
            _admin_end = System.Convert.ToDateTime(Txt_M_Admin_end.Text);
            _admin_start = System.Convert.ToDateTime(Txt_M_Admin_start.Text);
            _open_end = System.Convert.ToDateTime(Txt_M_Open_end.Text);
            _open_start = System.Convert.ToDateTime(Txt_M_Open_start.Text);
            _stab_end = System.Convert.ToDateTime(Txt_M_Stab_end.Text);
            _stab_start = System.Convert.ToDateTime(Txt_M_Stab_start.Text);
            _notes = null;
            _history = "Created on " + _modOn.ToString() + " by " + LitUserName.Text;
            _modBy = LitUserName.Text;

            measurementID = measurementController.manufactureNewMeasurement(_employerID, _planID, _employeeTypeID, _measurementTypeID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _modBy, _modOn, _history, _swStart, _swEnd, _swStart2, _swEnd2);

            if (Session["TempBreaks"] != null && measurementID != 0)
            {
                List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];
                foreach (var Break in breaks)
                {
                    BreakInServiceFactory factory = new BreakInServiceFactory();
                    factory.InsertNewBreakInService(measurementID, Break);
                }
            }

            if (measurementID > 0)
            {
                List<Measurement> tempList = new List<Measurement>();
                if (Session["tm"] != null)
                {
                    tempList = (List<Measurement>)Session["tm"];
                }
                tempList.Add(new Measurement(measurementID, _employerID, _planID, _employeeTypeID, _measurementTypeID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _history, _modOn, _modBy, _swStart, _swEnd, _swStart2, _swEnd2, false, false, null, null, null, null));
                Session["tm"] = tempList;
                loadMeasurements(_measurementTypeID);

                resetNewMeasurementPopup();
            }
            else
            {
                Lbl_M_message.Text = "An error occurred, You may already have a measurement period for this PLAN, EMPLOYEE TYPE and MEASUREMENT TYPE.";
                MpeMeasurementPeriod.Show();
            }
        }
        else
        {
            Lbl_M_message.Text = "Please correct all the red fields.";
            MpeMeasurementPeriod.Show();
        }
    }

    private void resetNewMeasurementPopup()
    {
        Ddl_M_PlanYear.SelectedIndex = Ddl_M_PlanYear.Items.Count - 1;
        Ddl_M_EmployeeType.SelectedIndex = Ddl_M_EmployeeType.Items.Count - 1;
        Ddl_M_MeasurementType.SelectedIndex = Ddl_M_MeasurementType.Items.Count - 1;

        Txt_M_Meas_start.Text = null;
        Txt_M_Meas_end.Text = null;
        Txt_M_Admin_end.Text = null;
        Txt_M_Admin_start.Text = null;
        Txt_M_Open_end.Text = null;
        Txt_M_Open_start.Text = null;
        Txt_M_Stab_end.Text = null;
        Txt_M_Stab_start.Text = null;
        TxtSummerBreakEnd.Text = null;
        TxtSummerBreakStart.Text = null;

        TxtSummerBreakEnd.Text = null;
        TxtSummerBreakStart.Text = null;

        Session["TempBreaks"] = new List<BreakInService>();
        Gv_TempBreakOfService.DataSource = null;
        Gv_TempBreakOfService.DataBind();
    }

    protected void DdlPlanYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        HiddenField hfID = null;
        int selectedIndex = GvSetup.SelectedIndex;
        GridViewRow row = GvSetup.Rows[selectedIndex];
        int selValue = 0;

        Session["tm"] = null;
        int _measTypeID = int.Parse(HfMeasurementTypeID.Value);
        loadMeasurements(_measTypeID);
        loadInsurance(0);
        loadInsurance(1);

        hfID = (HiddenField)row.FindControl("HfReportID");
        selValue = System.Convert.ToInt32(hfID.Value);

        switch (selValue)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:   
                loadEmployerContributions();
                break;
            default:
                break;
        }

    }


    protected void RptTransitionMeasurments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Literal litPlanName = (Literal)e.Item.FindControl("LitPlanName");
            Literal litEmployeeType = (Literal)e.Item.FindControl("LitEmployeeType");
            Literal litPlanStart = (Literal)e.Item.FindControl("LitStartDate");
            Literal litPlanEnd = (Literal)e.Item.FindControl("LitEndDate");
            Literal litMeasName = (Literal)e.Item.FindControl("LitMeasurementType");
            HiddenField hfMeasID = (HiddenField)e.Item.FindControl("Hf_M_MeasurementTypeID");
            HiddenField Hf_M_MeasurementID = (HiddenField)e.Item.FindControl("Hf_M_MeasurementID");

            Panel PnlEditSummerWindow = (Panel)e.Item.FindControl("PnlEditSummerWindow");
            Panel PnlSummerWindow3 = (Panel)e.Item.FindControl("PnlSummerWindow3");

            GridView Gv_BreakOfService = (GridView)e.Item.FindControl("Gv_BreakOfService");
            GridView Gv_DisplayBreakOfService = (GridView)e.Item.FindControl("Gv_DisplayBreakOfService");

            litPlanName.Text = DdlPlanYear.SelectedItem.Text;
            litEmployeeType.Text = DdlEmployeeType.SelectedItem.Text;
            litPlanStart.Text = "n/a";
            litPlanEnd.Text = "n/a";

            int measTypeID = int.Parse(hfMeasID.Value);
            int measurementID = int.Parse(Hf_M_MeasurementID.Value);

            if (measTypeID == 1)
            {
                litMeasName.Text = "Transitional Measurement Period";
            }
            else
            {
                litMeasName.Text = "Ongoing Measurement Cycle";
            }

            int _employerID = System.Convert.ToInt32(HfDistrictID.Value);

            employer emp = employerController.getEmployer(_employerID);
            if (emp.HasBreaksInService())
            {
                PnlEditSummerWindow.Visible = true;
                PnlSummerWindow3.Visible = true;
            }
            else
            {
                PnlEditSummerWindow.Visible = false;
                PnlSummerWindow3.Visible = false;
            }

            Gv_DisplayBreakOfService.DataSource = GetBreaks(measurementID).OrderBy(breaks => breaks.StartDate);
            Gv_DisplayBreakOfService.DataBind();

            Gv_BreakOfService.DataSource = GetBreaks(measurementID).OrderBy(breaks => breaks.StartDate);
            Gv_BreakOfService.DataBind();
        }
    }

    protected void RptTransitionMeasurments_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            int measurementID = 0;
            DateTime _meas_start = System.DateTime.Now.AddYears(-50);
            DateTime _meas_end = System.DateTime.Now.AddYears(-50);
            DateTime _admin_start = System.DateTime.Now.AddYears(-50);
            DateTime _admin_end = System.DateTime.Now.AddYears(-50);
            DateTime _open_start = System.DateTime.Now.AddYears(-50);
            DateTime _open_end = System.DateTime.Now.AddYears(-50);
            DateTime _stab_start = System.DateTime.Now.AddYears(-50);
            DateTime _stab_end = System.DateTime.Now.AddYears(-50);
            string _notes = null;
            string _modBy = LitUserName.Text;
            DateTime _modOn = System.DateTime.Now;
            string _history = null;
            bool validData = true;
            DateTime? _swStart = null;
            DateTime? _swEnd = null;
            DateTime? _swStart2 = null;
            DateTime? _swEnd2 = null;
            int _employerID = int.Parse(HfDistrictID.Value);
            employer emp = employerController.getEmployer(_employerID);

            HiddenField hfMeasID = (HiddenField)e.Item.FindControl("Hf_M_MeasurementID");
            TextBox txtMeasStart_new = (TextBox)e.Item.FindControl("Txt_M_Meas_start");
            TextBox txtMeasEnd_new = (TextBox)e.Item.FindControl("Txt_M_Meas_end");
            TextBox txtAdminStart_new = (TextBox)e.Item.FindControl("Txt_M_Admin_start");
            TextBox txtAdminEnd_new = (TextBox)e.Item.FindControl("Txt_M_Admin_end");
            TextBox txtOpenStart_new = (TextBox)e.Item.FindControl("Txt_M_Open_start");
            TextBox txtOpenEnd_new = (TextBox)e.Item.FindControl("Txt_M_Open_end");
            TextBox txtStabStart_new = (TextBox)e.Item.FindControl("Txt_M_Stab_start");
            TextBox txtStabEnd_new = (TextBox)e.Item.FindControl("Txt_M_Stab_end");
            TextBox txtNotes_new = (TextBox)e.Item.FindControl("Txt_M_Notes");
            TextBox txtHistory_new = (TextBox)e.Item.FindControl("TxtTransitionMeasurementHistory");
            Label lblMessage = (Label)e.Item.FindControl("Lbl_M_message");
            ModalPopupExtender mpe = (ModalPopupExtender)e.Item.FindControl("MpeEditTransitionPeriod");

            validData = errorChecking.validateTextBoxDate(txtMeasStart_new, validData);
            validData = errorChecking.validateTextBoxDate(txtMeasEnd_new, validData);
            validData = errorChecking.validateTextBoxDate(txtAdminStart_new, validData);
            validData = errorChecking.validateTextBoxDate(txtAdminEnd_new, validData);
            validData = errorChecking.validateTextBoxDate(txtOpenEnd_new, validData);
            validData = errorChecking.validateTextBoxDate(txtOpenStart_new, validData);
            validData = errorChecking.validateTextBoxDate(txtStabEnd_new, validData);
            validData = errorChecking.validateTextBoxDate(txtStabStart_new, validData);

            if (validData == true)
            {
                measurementID = int.Parse(hfMeasID.Value);
                _meas_start = System.Convert.ToDateTime(txtMeasStart_new.Text);
                _meas_end = System.Convert.ToDateTime(txtMeasEnd_new.Text);
                _admin_start = System.Convert.ToDateTime(txtAdminStart_new.Text);
                _admin_end = System.Convert.ToDateTime(txtAdminEnd_new.Text);
                _open_end = System.Convert.ToDateTime(txtOpenEnd_new.Text);
                _open_start = System.Convert.ToDateTime(txtOpenStart_new.Text);
                _stab_end = System.Convert.ToDateTime(txtStabEnd_new.Text);
                _stab_start = System.Convert.ToDateTime(txtStabStart_new.Text);
                _notes = txtNotes_new.Text;
                _history = txtHistory_new.Text;

                List<Measurement> newMeasList = null;
                int _measTypeID = int.Parse(HfMeasurementTypeID.Value);

                if (_measTypeID == 1)
                {
                    newMeasList = (List<Measurement>)Session["tm"];
                }
                else if (_measTypeID == 2)
                {
                    newMeasList = (List<Measurement>)Session["om"];
                }

                foreach (Measurement m in newMeasList)
                {
                    if (m.MEASUREMENT_ID == measurementID)
                    {
                        _history += Environment.NewLine + Environment.NewLine; ;
                        _history += "Record Altered on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                        _history += "Measerment Period: " + _meas_start.ToString() + " to " + _meas_end.ToString() + Environment.NewLine;
                        _history += "Administrative Period: " + _admin_start.ToString() + " to " + _admin_end.ToString() + Environment.NewLine;
                        _history += "Open Period: " + _open_start.ToString() + " to " + _open_end.ToString() + Environment.NewLine;
                        _history += "Stability Period: " + _stab_start.ToString() + " to " + _stab_end.ToString() + Environment.NewLine;

                        break;
                    }
                }

                validData = measurementController.updateMeasurement(measurementID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _modBy, _modOn, _history, _swStart, _swEnd, _swStart2, _swEnd2);

                if (validData == true)
                {
                    foreach (Measurement m in newMeasList)
                    {
                        if (m.MEASUREMENT_ID == measurementID)
                        {
                            m.MEASUREMENT_START = _meas_start;
                            m.MEASUREMENT_END = _meas_end;
                            m.MEASUREMENT_ADMIN_START = _admin_start;
                            m.MEASUREMENT_ADMIN_END = _admin_end;
                            m.MEASUREMENT_OPEN_START = _open_start;
                            m.MEASUREMENT_OPEN_END = _open_end;
                            m.MEASUREMENT_STAB_START = _stab_start;
                            m.MEASUREMENT_STAB_END = _stab_end;
                            m.MEASUREMENT_NOTES = _notes;
                            m.MEASUREMENT_HISTORY = _history;
                            m.MEASUREMENT_MOD_BY = _modBy;
                            m.MEASUREMENT_MOD_ON = _modOn;

                            break;
                        }
                    }

                    loadMeasurements(_measTypeID);
                }
                else
                {
                    lblMessage.Text = "An error occurred while updating the measurement period.";
                    mpe.Show();
                }
            }
            else
            {
                lblMessage.Text = "Please correct all the red fields.";
                mpe.Show();
            }
        }
    }
    #endregion

    #region PLAN YEAR FUNCTIONS 
    protected void BtnPlanYearUpdate_Click(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        string _name = null;
        DateTime _startDate = System.DateTime.Now.AddYears(-50);
        DateTime _endDate = System.DateTime.Now.AddYears(-50);
        string _notes = null;
        string _history = null;
        DateTime _modOn = System.DateTime.Now;
        string _modBy = HfUserName.Value;
        bool ValidData = true;

        ValidData = errorChecking.validateTextBoxNull(Txt_npy_Name, ValidData);
        ValidData = errorChecking.validateTextBoxDate(Txt_npy_StartDate, ValidData);

        if (ValidData == true)
        {
            _name = Txt_npy_Name.Text;
            _startDate = DateTime.Parse(Txt_npy_StartDate.Text);
            _endDate = _startDate.AddYears(1);
            _endDate = _endDate.AddDays(-1);
            _notes = Txt_npy_Notes.Text;

            _history = "Created on: " + _modOn.ToString() + " by: " + _modBy;

            int GroupId = PlanYearGroupFactory.GetAllPlanYearGroupForEmployerId(_employerID).First().PlanYearGroupId;

            int newPlanID = PlanYear_Controller.manufactureNewPlanYear(_employerID, _name, _startDate, _endDate, _notes, _history, _modOn, _modBy
                , null, null, null, null, null, null, null, null,
                GroupId);

            if (newPlanID > 0)
            {
                PlanYear newPlan = new PlanYear(newPlanID, _employerID, _name, _startDate, _endDate, _notes, _history, _modOn, _modBy);

                loadGvPlanYears();

                loadPlanYears();
                UpPlanYear.Update();
            }
            else
            {
                MpeNewPlanYear.Show();
                Lbl_npy_Message.Text = "An error occurred while adding the new plan year, please try again.";
            }
        }
        else
        {
            MpeNewPlanYear.Show();
            Lbl_npy_Message.Text = "Please correct all red fields.";
        }

    }

    protected void GvPlanYears_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = null;
        int _employerID = int.Parse(HfDistrictID.Value);
        int rowID = GvPlanYears.EditIndex;
        int _planYearID = 0;
        bool validData = true;
        DateTime _modOn = System.DateTime.Now;
        string _modBy = HfUserName.Value;

        row = GvPlanYears.Rows[rowID];

        TextBox txtName = (TextBox)row.FindControl("Txt_epy_Name");
        TextBox txtSdate = (TextBox)row.FindControl("Txt_epy_StartDate");
        TextBox txtEdate = (TextBox)row.FindControl("Txt_epy_EndDate");
        TextBox txtNotes = (TextBox)row.FindControl("Txt_epy_Notes");

        TextBox Txt_M_Meas_start = (TextBox)row.FindControl("Txt_M_Meas_start");
        TextBox Txt_M_Meas_end = (TextBox)row.FindControl("Txt_M_Meas_end");
        TextBox Txt_M_Admin_start = (TextBox)row.FindControl("Txt_M_Admin_start");
        TextBox Txt_M_Admin_end = (TextBox)row.FindControl("Txt_M_Admin_end");
        TextBox Txt_M_Open_start = (TextBox)row.FindControl("Txt_M_Open_start");
        TextBox Txt_M_Open_end = (TextBox)row.FindControl("Txt_M_Open_end");
        TextBox Txt_M_Stab_start = (TextBox)row.FindControl("Txt_M_Stab_start");
        TextBox Txt_M_Stab_end = (TextBox)row.FindControl("Txt_M_Stab_end");


        TextBox txtHistory = (TextBox)row.FindControl("TxtPlanYearHistory");
        HiddenField hfID = (HiddenField)row.FindControl("HfPlanYearID");
        ModalPopupExtender mpePYedit = (ModalPopupExtender)row.FindControl("Mpe_epy");
        Label lblMessage = (Label)row.FindControl("Lbl_npy_Message");

        validData = errorChecking.validateTextBoxNull(txtName, validData);
        validData = errorChecking.validateTextBoxDate(txtSdate, validData);
        validData = errorChecking.validateTextBoxDate(txtEdate, validData);

        if (validData == true)
        {
            string _name = txtName.Text;
            DateTime _sDate = DateTime.Parse(txtSdate.Text);
            DateTime _eDate = DateTime.Parse(txtEdate.Text);

            string _notes = txtNotes.Text;
            string _history = txtHistory.Text;
            _planYearID = int.Parse(hfID.Value);

            _history += Environment.NewLine + Environment.NewLine + "Record modified on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
            _history += "Plan Dates: " + _sDate.ToShortDateString() + " to " + _eDate.ToShortDateString() + Environment.NewLine;

            int GroupId = PlanYearGroupFactory.GetAllPlanYearGroupForEmployerId(_employerID).First().PlanYearGroupId;

            var planYears = PlanYear_Controller.findPlanYear(_planYearID, _employerID);

            validData = PlanYear_Controller.updatePlanYear(_planYearID, _name, _sDate, _eDate, _notes, _history, _modOn, _modBy,
                                                             planYears.Default_Meas_Start, planYears.Default_Meas_End,
                                                             planYears.Default_Admin_End, planYears.Default_Admin_End,
                                                             planYears.Default_Open_Start, planYears.Default_Open_End,
                                                             planYears.Default_Stability_Start, planYears.Default_Stability_End,
                                                             GroupId);

            if (validData == true)
            {

                loadGvPlanYears();
                loadPlanYears();

                Response.Redirect("s_setup.aspx?code=6", false);
            }
            else
            {
                mpePYedit.Show();
                lblMessage.Text = "An error occurred while updating this record, please try again.";
            }
        }
        else
        {
            mpePYedit.Show();
            lblMessage.Text = "Please correct all the red fields.";
        }
    }

    protected void GvPlanYears_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewRow row = null;
        int _employerID = int.Parse(HfDistrictID.Value);
        GvPlanYears.EditIndex = e.NewEditIndex;
        GvPlanYears.SelectedIndex = -1;
        loadGvPlanYears();

        int rowID = GvPlanYears.EditIndex;

        row = GvPlanYears.Rows[rowID];
        ModalPopupExtender mpeHistory = (ModalPopupExtender)row.FindControl("Mpe_epy");
        mpeHistory.Show();
    }

    protected void GvPlanYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        GridViewRow row = GvPlanYears.SelectedRow;
        GvPlanYears.EditIndex = GvPlanYears.SelectedIndex;

        loadGvPlanYears();

        int rowID = GvPlanYears.SelectedIndex;

        row = GvPlanYears.Rows[rowID];
        ModalPopupExtender mpeHistory = (ModalPopupExtender)row.FindControl("MpePlanYearHistory");
        mpeHistory.Show();
    }
    #endregion

    #region INSURANCE FUNCTIONS

    protected void Btn_I_Update_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
        List<insurance> tempList = insuranceController.getAllActiveInsurancePlansByPlanYear(_planYearID);

        insurance _currIns = null;
        validData = errorChecking.validateDropDownSelection(Ddl_I_PlanYear, validData);
        validData = errorChecking.validateTextBoxNull(Txt_I_Name, validData);
        validData = errorChecking.validateTextBoxDecimal(Txt_I_Cost, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_I_MinValue, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_I_Dependant, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_I_Spouse, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_I_Type, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_I_mec, validData);

        if (validData == true)
        {
            string _name = null;
            decimal _cost = 0;
            bool _minValue = false;
            bool _offSpouse = false;
            bool SpouseConditional = false;
            bool _offDependent = false;
            string _modBy = HfUserName.Value;
            DateTime _modOn = System.DateTime.Now;
            string _history = null;
            int _insuranceID = 0;
            int _insuranceTypeID = 0;
            bool _mec = false;
            bool updateConfirmed = false;
            bool fullyPlusSelfInsured = false;

            _name = Txt_I_Name.Text;
            _cost = decimal.Parse(Txt_I_Cost.Text);
            _planYearID = int.Parse(Ddl_I_PlanYear.SelectedItem.Value);
            _mec = bool.Parse(Ddl_I_mec.SelectedItem.Value);
            _minValue = bool.Parse(Ddl_I_MinValue.SelectedItem.Value);
            _offDependent = bool.Parse(Ddl_I_Dependant.SelectedItem.Value);
            _insuranceTypeID = int.Parse(Ddl_I_Type.SelectedItem.Value);

            if (errorChecking.validateDropDownSelectionNoRed(Ddl_I_SelfPlusFullyInsured, true) == true) { bool.TryParse(Ddl_I_SelfPlusFullyInsured.SelectedItem.Value, out fullyPlusSelfInsured); }
            else { fullyPlusSelfInsured = false; }

            if (false == bool.TryParse(Ddl_I_Spouse.SelectedItem.Value, out _offSpouse))
            {
                SpouseConditional = true;
            }

            if (Lit_I_function.Text == "New")
            {
                _history = "Created on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                _history += "Medical plan:" + _name + ", Type:" + Ddl_I_Type.SelectedItem.Text + ", cost:" + _cost.ToString() + ", min:" + _minValue.ToString() + ", os:" + _offSpouse.ToString() + ", od:" + _offDependent.ToString() + ", mec:" + _mec.ToString() + ", fullyAndSelf:" + fullyPlusSelfInsured.ToString();
                insurance temp = null;

                temp = insuranceController.manufactureInsurancePlan(_planYearID, _name, _cost, _minValue, _offSpouse, SpouseConditional, _offDependent, _modBy, _modOn, _history, _insuranceTypeID, _mec, fullyPlusSelfInsured);

                if (temp != null)
                {
                    updateConfirmed = true;
                    loadInsurance(0);
                    Mpe_I_insurance.Hide();

                    LitMessage.Text = "The new medical plan, " + _name + ", has been added. If you do not see the plan, make sure the correct Plan Year is selected in the upper right hand corner.";
                    MpeWebPageMessage.Show();
                }
            }
            else if (Lit_I_function.Text == "Edit")
            {
                _insuranceID = int.Parse(Hf_I_id.Value);
                _currIns = insuranceController.getSingleInsurancePlan(_insuranceID, tempList);

                _history = _currIns.INSURANCE_HISTORY;
                _history += Environment.NewLine + Environment.NewLine;
                _history += "Record modified on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                _history += "Medical plan:" + _name + ", Type:" + Ddl_I_Type.SelectedItem.Text + ", cost:" + _cost + ", min:" + _minValue.ToString() + ", os:" + _offSpouse.ToString() + ", od:" + _offDependent.ToString() + ", mec:" + _mec.ToString() + ", fullyAndSelf:" + fullyPlusSelfInsured.ToString();

                updateConfirmed = insuranceController.updateInsurancePlan(_insuranceID, _planYearID, _name, _cost, _minValue, _offSpouse, SpouseConditional, _offDependent, _modBy, _modOn, _history, _insuranceTypeID, _mec, fullyPlusSelfInsured);

                if (updateConfirmed == true)
                {
                    loadInsurance(0);
                }
            }

            if (updateConfirmed == true)
            {
                Ddl_I_Dependant.SelectedIndex = Ddl_I_Dependant.Items.Count - 1;
                Ddl_I_mec.SelectedIndex = Ddl_I_mec.Items.Count - 1;
                Ddl_I_MinValue.SelectedIndex = Ddl_I_MinValue.Items.Count - 1;
                Ddl_I_PlanYear.SelectedIndex = Ddl_I_PlanYear.Items.Count - 1;
                Ddl_I_Spouse.SelectedIndex = Ddl_I_Spouse.Items.Count - 1;
                Ddl_I_Type.SelectedIndex = Ddl_I_Type.Items.Count - 1;
                Ddl_I_SelfPlusFullyInsured.SelectedIndex = Ddl_I_SelfPlusFullyInsured.Items.Count - 1;
                Txt_I_Cost.Text = String.Empty;
                Txt_I_Name.Text = String.Empty;
                Hf_I_id.Value = String.Empty;
            }
        }
        else
        {
            Lbl_I_Message.Text = "Please correct all the red fields";
            Mpe_I_insurance.Show();
        }
    }

    protected void GvInsurance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)GvInsurance.Rows[e.RowIndex];

            HiddenField hf = (HiddenField)row.FindControl("HfInsuranceID");
            int _insuranceID = int.Parse(hf.Value);
            bool validTransaction = false;

            validTransaction = insuranceController.deleteInsurancePlan(_insuranceID);

            if (validTransaction == true)
            {
                MpeWebPageMessage.Show();
                LitMessage.Text = "The medical plan has been DELETED from the Software.";
                loadInsurance(0);
            }
            else
            {
                MpeWebPageMessage.Show();
                LitMessage.Text = "The medical plan could not be DELETED. It may be linked to an Insurance Contribution. If you have questions about this, please contact " + Branding.CompanyShortName;
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            MpeWebPageMessage.Show();
            LitMessage.Text = "An ERROR occurred while trying to DELETE the Insurance Record. Please contact " + Branding.CompanyShortName;
        }


    }

    protected void GvInsurance_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Lit_I_function.Text = "Edit";
        GvInsurance.EditIndex = e.NewEditIndex;
        loadInsurance(0);

        int index = GvInsurance.EditIndex;
        GridViewRow row = GvInsurance.Rows[index];

        HiddenField hf = (HiddenField)row.FindControl("HfInsuranceID");

        int _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
        List<insurance> tempList = insuranceController.getAllActiveInsurancePlansByPlanYear(_planYearID);

        int _insuranceID = int.Parse(hf.Value);
        insurance _currIns = insuranceController.getSingleInsurancePlan(_insuranceID, tempList);

        loadInsuranceType(Ddl_I_Type);

        Txt_I_Name.Text = _currIns.INSURANCE_NAME;
        Txt_I_Cost.Text = _currIns.INSURANCE_COST.ToString();
        Hf_I_id.Value = _currIns.INSURANCE_ID.ToString();

        errorChecking.setDropDownList(Ddl_I_MinValue, _currIns.INSURANCE_MIN_VALUE);
        errorChecking.setDropDownList(Ddl_I_Spouse, _currIns.INSURANCE_OFF_SPOUSE);
        errorChecking.setDropDownList(Ddl_I_Dependant, _currIns.INSURANCE_OFF_DEPENDENTS);
        errorChecking.setDropDownList(Ddl_I_Type, _currIns.INSURANCE_TYPE_ID);
        errorChecking.setDropDownList(Ddl_I_PlanYear, _currIns.INSURANCE_PLAN_YEAR_ID);
        errorChecking.setDropDownList(Ddl_I_mec, _currIns.INSURANCE_MEC);
        errorChecking.setDropDownList(Ddl_I_SelfPlusFullyInsured, _currIns.INSURANCE_FULLY_AND_SELF);

        if (_currIns.SpouseConditional)
        {
            ListItem li = Ddl_I_Spouse.Items.FindByValue("Conditionally");
            if (li != null)
            {
                Ddl_I_Spouse.ClearSelection();
                li.Selected = true;
            }
        }

        if (_currIns.INSURANCE_TYPE_ID == 1) { Pnl_I_FullyPlusSelfInsured.Visible = true; }
        else { Pnl_I_FullyPlusSelfInsured.Visible = false; }

        Lbl_I_Message.Text = String.Empty;
        Mpe_I_insurance.Show();
    }


    protected void Ddl_I_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validInsuranceType = true;

        validInsuranceType = errorChecking.validateDropDownSelectionNoRed(Ddl_I_Type, validInsuranceType);

        if (validInsuranceType == true)
        {
            int insuranceTypeID = int.Parse(Ddl_I_Type.SelectedItem.Value);
            if (insuranceTypeID == 1) { Pnl_I_FullyPlusSelfInsured.Visible = true; }
            else { Pnl_I_FullyPlusSelfInsured.Visible = false; errorChecking.setDropDownList(Ddl_I_SelfPlusFullyInsured, "false"); }
        }
        else { Pnl_I_FullyPlusSelfInsured.Visible = false; }

        Mpe_I_insurance.Show();
    }

    protected void GvInsurance_SelectedIndexChanged(object sender, EventArgs e)
    {
        GvInsurance.EditIndex = GvInsurance.SelectedIndex;
        loadInsurance(0);

        int index = GvInsurance.EditIndex;
        GridViewRow row = GvInsurance.Rows[index];

        ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("Mpe_h_insurance");


        mpe.Show();
    }

    protected void BtnNewInsurance_Click(object sender, EventArgs e)
    {
        Lit_I_function.Text = "New";
        loadInsuranceType(Ddl_I_Type);
        Mpe_I_insurance.Show();
        Ddl_I_PlanYear.SelectedIndex = Ddl_I_PlanYear.Items.Count - 1;
        Ddl_I_MinValue.SelectedIndex = Ddl_I_MinValue.Items.Count - 1;
        Ddl_I_Dependant.SelectedIndex = Ddl_I_Dependant.Items.Count - 1;
        Ddl_I_Spouse.SelectedIndex = Ddl_I_Spouse.Items.Count - 1;
        Ddl_I_mec.SelectedIndex = Ddl_I_mec.Items.Count - 1;
        Ddl_I_SelfPlusFullyInsured.SelectedIndex = Ddl_I_SelfPlusFullyInsured.Items.Count - 1;
        Txt_I_Cost.Text = null;
        Txt_I_Name.Text = null;
        Hf_I_id.Value = null;
    }

    #endregion

    #region GROSS PAY FILTERS
    protected void BtnSaveGpFilter_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int _employerID = int.Parse(HfDistrictID.Value);

        validData = errorChecking.validateDropDownSelection(DdlGrossPayFilter, validData);

        if (validData == true)
        {
            int _gpID = int.Parse(DdlGrossPayFilter.SelectedItem.Value);
            gpType gp = null;

            gp = gpType_Controller.manufactureGrossPayFilter(_gpID, _employerID);

            if (gp == null)
            {
                LblGpFilterMessage.Text = "This Gross Pay Filter already exists.";
                MpeGPFilter.Show();
            }
            else
            {
                loadGPFilter();
            }
        }
        else
        {
            LblGpFilterMessage.Text = "An error occurred, please correct all of the fields highlighted in RED.";
            MpeGPFilter.Show();
        }
    }

    protected void GvGrossPayExclusion_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)GvGrossPayExclusion.Rows[e.RowIndex];
            HiddenField hf = (HiddenField)row.FindControl("HfFilterID");
            int gpID = int.Parse(hf.Value);
            bool success = false;

            success = gpType_Controller.removeGrossPayFilter(gpID);

            if (success == true)
            {
                loadGPFilter();
            }
            else
            {

            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

        }
    }

    protected void GvGrossPayExclusion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvGrossPayExclusion.PageIndex = e.NewPageIndex;
        loadGPFilter();
    }

    #endregion

    #region EMPLOYEE CLASS
    protected void Btn_ec_Save_Click(object sender, EventArgs e)
    {
        int employerID = 0;
        string description = null;
        string ashCode = null;
        string ooc = null;
        DateTime modOn = DateTime.Now;
        string modBy = LitUserName.Text;
        string history = null;
        bool validData = true;
        bool validTransaction = false;
        int waitingPeriodID = 0;

        validData = errorChecking.validateTextBoxNull(Txt_ec_EmployeeClass, validData);
        validData = errorChecking.validateDropDownSelection(Ddl_ec_WaitingPeriod, validData);

        if (validData == true)
        {
            employerID = int.Parse(HfDistrictID.Value);
            waitingPeriodID = int.Parse(Ddl_ec_WaitingPeriod.SelectedItem.Value);
            description = Txt_ec_EmployeeClass.Text;

            if (errorChecking.validateDropDownSelectionNoRed(Ddl_ec_ASH, true) == false) { ashCode = string.Empty; }
            else { ashCode = Ddl_ec_ASH.SelectedItem.Value.ToLower(); }

            if (errorChecking.validateDropDownSelectionNoRed(Ddl_ec_offerCode, true) == false) { ooc = string.Empty; }
            else { ooc = Ddl_ec_offerCode.SelectedItem.Value.ToLower(); }

            if (Lit_ec_function.Text == "New")
            {
                history = "Created on " + modOn.ToString() + " by " + modBy + System.Environment.NewLine + description;
                history += Environment.NewLine + "Code: " + ashCode + ", WaitingPeriod: " + Ddl_ec_WaitingPeriod.SelectedItem.Text + ", DefaultOfferCode: " + Ddl_ec_offerCode.SelectedItem.Text;
                validTransaction = classificationController.ManufactureEmployeeClassification(employerID, description, ashCode, modOn, modBy, history, waitingPeriodID, ooc);
            }
            else if (Lit_ec_function.Text == "Edit")
            {
                int classID = int.Parse(Hf_ec_id.Value);
                List<classification> tempList = loadClassifications();
                classification myClassification = classificationController.findClassification(classID, tempList);

                if (myClassification != null)
                {
                    history = myClassification.CLASS_HISTORY + System.Environment.NewLine;
                    history += System.Environment.NewLine + "Record modfied on " + modOn.ToString() + " by " + modBy + System.Environment.NewLine + description;
                    history += System.Environment.NewLine + "Code: " + ashCode + ", WaitingPeriod: " + Ddl_ec_WaitingPeriod.SelectedItem.Text + ", DefaultOfferCode: " + Ddl_ec_offerCode.SelectedItem.Text; ;
                    validTransaction = classificationController.UpdateEmployeeClassification(classID, description, ashCode, modOn, modBy, history, waitingPeriodID, myClassification.CLASS_ENTITY_STATUS, ooc);
                }
            }

            if (validTransaction == true)
            {
                LitMessage.Text = description + " has been added to your EMPLOYEE Classifications";
                LblEmpClassMessage.Text = null;
                Txt_ec_EmployeeClass.Text = null;
                Ddl_ec_ASH.SelectedIndex = Ddl_ec_ASH.Items.Count - 1;
                Ddl_ec_WaitingPeriod.SelectedIndex = Ddl_ec_WaitingPeriod.Items.Count - 1;
                Hf_ec_id.Value = null;
                MpeWebPageMessage.Show();
                loadClassifications();
            }
            else
            {
                LblEmpClassMessage.Text = "An error occurred, please try again and if you continue to have issues, please contact " + Branding.CompanyShortName;
                MpeNewEmployeeClass.Show();
            }
        }
        else
        {
            LblEmpClassMessage.Text = "Please correct the fields highlighted in RED.";
            MpeNewEmployeeClass.Show();
        }
    }

    protected void GvEmployeeClassifications_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)GvEmployeeClassifications.Rows[e.RowIndex];
            HiddenField hf = (HiddenField)row.FindControl("HfClassID");
            int _classificationID = int.Parse(hf.Value);
            bool success = false;

            success = classificationController.DeleteEmployeeClassification(_classificationID);

            if (success == true)
            {
                LitMessage.Text = "The EMPLOYEE Classification was DELETED!";
                MpeWebPageMessage.Show();
                loadClassifications();
            }
            else
            {
                LitMessage.Text = "An error occurred while trying to DELETE the EMPLOYEE Classification. This classification may still be linked to an EMPLOYEE. Contact " + Branding.CompanyShortName + " if you questions.";
                MpeWebPageMessage.Show();
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            LitMessage.Text = "An error occurred while trying to DELETE the EMPLOYEE Classification. This classification may still be linked to an EMPLOYEE. Contact " + Branding.CompanyShortName + " if you questions.";
            MpeWebPageMessage.Show();
        }
    }

    protected void GvEmployeeClassifications_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GvEmployeeClassifications.EditIndex = e.NewEditIndex;
        List<classification> tempList = loadClassifications();
        classification currClass = null;
        int classID = 0;
        int index = GvEmployeeClassifications.EditIndex;
        GridViewRow row = GvEmployeeClassifications.Rows[index];
        HiddenField hf = (HiddenField)row.FindControl("HfClassID");

        int.TryParse(hf.Value, out classID);
        currClass = classificationController.findClassification(classID, tempList);

        Lit_ec_function.Text = "Edit";
        Txt_ec_EmployeeClass.Text = currClass.CLASS_DESC;
        Hf_ec_id.Value = currClass.CLASS_ID.ToString();
        errorChecking.setDropDownList(Ddl_ec_ASH, currClass.CLASS_AFFORDABILITY_CODE);
        errorChecking.setDropDownList(Ddl_ec_WaitingPeriod, currClass.CLASS_WAITING_PERIOD_ID);
        errorChecking.setDropDownList(Ddl_ec_offerCode, currClass.CLASS_DEFAULT_OOC);

        MpeNewEmployeeClass.Show();
    }

    protected void BtnNewEmpClassification_Click(object sender, EventArgs e)
    {
        Lit_ec_function.Text = "New";
        Ddl_ec_ASH.SelectedIndex = Ddl_ec_ASH.Items.Count - 1;
        Ddl_ec_WaitingPeriod.SelectedIndex = Ddl_ec_WaitingPeriod.Items.Count - 1;
        Ddl_ec_offerCode.SelectedIndex = Ddl_ec_offerCode.Items.Count - 1;
        Hf_ec_id.Value = null;
        Txt_ec_EmployeeClass.Text = null;
        Txt_ec_EmployeeClass.Focus();
        MpeNewEmployeeClass.Show();
    }

    protected void GvEmployeeClassifications_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvEmployeeClassifications.PageIndex = e.NewPageIndex;
        loadClassifications();
    }

    #endregion

    #region Insurance Contribution

    protected void Ddl_ic_InsurancePlans_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadEmployerContributions();
    }

    protected void Btn_ic_saveContribution_Click(object sender, EventArgs e)
    {
        bool validData = true;
        int _planYearID = 0;
        int _insurancePlanID = 0;
        string _contributionID = null;
        int _classID = 0;
        double _contribution = 0;
        string _modBy = HfUserName.Value;
        DateTime? _modOn = DateTime.Now;
        string _history = null;

        try
        {
            validData = errorChecking.validateDropDownSelection(Ddl_nec_InsurancePlans, validData);
            validData = errorChecking.validateDropDownSelection(Ddl_nec_ContributionMethod, validData);
            validData = errorChecking.validateDropDownSelection(Ddl_nec_EmployeeClass, validData);
            validData = errorChecking.validateTextBoxDecimal(Txt_nec_contribution, validData);

            if (validData == true)
            {
                _insurancePlanID = int.Parse(Ddl_nec_InsurancePlans.SelectedItem.Value);
                _contributionID = Ddl_nec_ContributionMethod.SelectedItem.Value.ToString();
                _classID = int.Parse(Ddl_nec_EmployeeClass.SelectedItem.Value);
                _contribution = double.Parse(Txt_nec_contribution.Text);
                _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
                List<insurance> tempInsList = insuranceController.manufactureInsuranceList(_planYearID);
                insurance selectedInsurance = insuranceController.getSingleInsurancePlan(_insurancePlanID, tempInsList);

                if (_contributionID == "$")
                {
                    if (_contribution > (double)selectedInsurance.INSURANCE_COST)
                    {
                        validData = false;
                        Txt_nec_contribution.BackColor = System.Drawing.Color.Red;
                        Mpe_nec_InsuranceContribution.Show();
                        Lbl_nec_Message.Text = "The contribution amount can not be greater than the plan cost.";
                    }
                }
                else if (_contributionID == "%")
                {
                    if (_contribution > 100)
                    {
                        validData = false;
                        Mpe_nec_InsuranceContribution.Show();
                        Lbl_nec_Message.Text = "The contribution percentage can not be greater than 100 percent.";
                    }
                }
                else
                {
                    validData = false;
                    Mpe_nec_InsuranceContribution.Show();
                    Lbl_nec_Message.Text = "Please select a contribution type.";
                }

                if (validData == true)
                {
                    _history = "Employer Contribution created on " + _modOn.ToString() + " by " + _modBy + System.Environment.NewLine;
                    _history += "Contribution Type: " + _contributionID.ToString() + " | Amount: " + _contribution;

                    insuranceController.manufactureInsuranceContribution(_insurancePlanID, _contributionID, _classID, _contribution, _modBy, _modOn, _history);
                    MpeWebPageMessage.Show();
                    LitMessage.Text = "The insurance contribution for the employee class, (" + _classID + "), has been created.";

                    Lbl_nec_Message.Text = null;
                    Txt_nec_contribution.Text = null;
                    Ddl_nec_EmployeeClass.SelectedIndex = Ddl_nec_EmployeeClass.Items.Count - 1;
                    Ddl_nec_ContributionMethod.SelectedIndex = Ddl_nec_ContributionMethod.Items.Count - 1;
                    Ddl_nec_InsurancePlans.SelectedIndex = Ddl_nec_InsurancePlans.Items.Count - 1;

                    loadEmployerContributions();
                }
            }
            else
            {
                Mpe_nec_InsuranceContribution.Show();
                Lbl_nec_Message.Text = "Please correct any fields highlighted in RED.";
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            Mpe_nec_InsuranceContribution.Show();
            Lbl_nec_Message.Text = "Please correct any fields highlighted in RED.";
        }
    }

    private void loadEmployerContributions()
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_ic_InsurancePlans, validData);

        if (validData == true)
        {
            int _insuranceID = 0;
            _insuranceID = int.Parse(Ddl_ic_InsurancePlans.SelectedItem.Value);
            List<insuranceContribution> icList = insuranceController.manufactureInsuranceContributionList(_insuranceID);

            Gv_employerContributions.DataSource = icList;
            Gv_employerContributions.DataBind();

            LitInsContShowing.Text = Gv_employerContributions.Rows.Count.ToString();
            LitInsContTotal.Text = icList.Count.ToString();
        }
        else
        {
            LitMessage.Text = "Please go to the medical plan Admin page and ADD an medical plan.";
            MpeWebPageMessage.Show();
        }
    }

    protected void Gv_employerContributions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)Gv_employerContributions.Rows[e.RowIndex];
            HiddenField hf = (HiddenField)row.FindControl("HfContributionID");
            int _contributionID = int.Parse(hf.Value);
            bool success = false;

            success = insuranceController.deleteInsuranceContribution(_contributionID);

            if (success == true)
            {
                LitMessage.Text = "The Insurance Contribution was DELETED!";
                MpeWebPageMessage.Show();
                loadInsurance(1);
                loadContributionTypes();
                loadEmployerContributions();
                loadClassificationsDropDown(Ddl_nec_EmployeeClass);
            }
            else
            {
                LitMessage.Text = "An error occurred while trying to DELETE the Insurance Contribution. This contribution may currently be linked to an EMPLOYEE. Contact " + Branding.CompanyShortName + " if you questions.";
                MpeWebPageMessage.Show();
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            LitMessage.Text = "An error occurred while trying to DELETE the Insurance Contribution. Please try again and if the problem continues, contact " + Branding.CompanyShortName;
            MpeWebPageMessage.Show();
        }
    }

    protected void Gv_employerContributions_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Gv_employerContributions.EditIndex = e.NewEditIndex;

        try
        {
            int index = Gv_employerContributions.EditIndex;
            GridViewRow row = Gv_employerContributions.Rows[index];
            int _contributionID = 0;
            int _planYearID = 0;
            int _employerID = 0;
            int _insuranceID = 0;
            insuranceContribution selectedInsCont = null;

            HiddenField hf = (HiddenField)row.FindControl("HfContributionID");
            DropDownList ddlInsPlans = (DropDownList)row.FindControl("Ddl_e_InsurancePlans");
            DropDownList ddlContMeth = (DropDownList)row.FindControl("Ddl_e_ContributionMethod");
            DropDownList ddlEmpClass = (DropDownList)row.FindControl("Ddl_e_EmployeeClass");
            TextBox txtAmount = (TextBox)row.FindControl("Txt_e_contribution");

            ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("Mpe_e_contribution");

            _contributionID = int.Parse(hf.Value);
            _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);
            _employerID = int.Parse(HfDistrictID.Value);
            _insuranceID = int.Parse(Ddl_ic_InsurancePlans.SelectedItem.Value);

            List<insuranceContribution> tempList = insuranceController.manufactureInsuranceContributionList(_insuranceID);
            selectedInsCont = insuranceController.getSingleInsuranceContribution(_contributionID, tempList);

            ddlInsPlans.DataSource = insuranceController.manufactureInsuranceList(_planYearID);
            ddlInsPlans.DataTextField = "INSURANCE_NAME";
            ddlInsPlans.DataValueField = "INSURANCE_ID";
            ddlInsPlans.DataBind();

            ddlContMeth.DataSource = insuranceController.getContributionTypes();
            ddlContMeth.DataTextField = "CONT_NAME";
            ddlContMeth.DataValueField = "CONT_ID";
            ddlContMeth.DataBind();

            ddlEmpClass.DataSource = classificationController.ManufactureEmployerClassificationList(_employerID, true);
            ddlEmpClass.DataTextField = "CLASS_DESC";
            ddlEmpClass.DataValueField = "CLASS_ID";
            ddlEmpClass.DataBind();

            errorChecking.setDropDownList(ddlInsPlans, _insuranceID);
            errorChecking.setDropDownList(ddlEmpClass, selectedInsCont.INS_CONT_CLASSID);
            errorChecking.setDropDownList(ddlContMeth, selectedInsCont.INS_CONT_CONTRIBUTION_ID);
            txtAmount.Text = selectedInsCont.INS_CONT_AMOUNT.ToString("0.00");

            mpe.Show();

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            LitMessage.Text = "An error occurred while loading the Insurance Contribution. Please try again and if the issue continues, please contact " + Branding.CompanyShortName;
            MpeWebPageMessage.Show();
        }
    }

    protected void Gv_employerContributions_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridViewRow row = Gv_employerContributions.Rows[e.RowIndex];

            int _insContID = 0;
            string _insContTypeID = null;
            int _planYearID = 0;
            int _employerID = 0;
            int _insuranceID = 0;
            int _oldInsuranceID = 0;
            int _classID = 0;
            bool validData = true;
            double _amount = 0;
            string _history = null;
            DateTime _modOn = System.DateTime.Now;
            string _modBy = HfUserName.Value;
            bool validTransaction = false;

            HiddenField hf = (HiddenField)row.FindControl("HfContributionID");
            DropDownList ddlInsPlans = (DropDownList)row.FindControl("Ddl_e_InsurancePlans");
            DropDownList ddlContMeth = (DropDownList)row.FindControl("Ddl_e_ContributionMethod");
            DropDownList ddlEmpClass = (DropDownList)row.FindControl("Ddl_e_EmployeeClass");
            TextBox txtAmount = (TextBox)row.FindControl("Txt_e_contribution");
            ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("Mpe_e_contribution");
            Label lblMess = (Label)row.FindControl("Lbl_e_Message");

            validData = errorChecking.validateDropDownSelection(ddlInsPlans, validData);
            validData = errorChecking.validateDropDownSelection(ddlContMeth, validData);
            validData = errorChecking.validateDropDownSelection(ddlEmpClass, validData);
            validData = errorChecking.validateTextBoxDecimal(txtAmount, validData);

            if (validData == true)
            {
                _insContID = int.Parse(hf.Value);                                      
                _insContTypeID = ddlContMeth.SelectedItem.Value.ToString();              
                _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);              
                _employerID = int.Parse(HfDistrictID.Value);                         
                _insuranceID = int.Parse(ddlInsPlans.SelectedItem.Value);              
                _classID = int.Parse(ddlEmpClass.SelectedItem.Value);                  
                _amount = double.Parse(txtAmount.Text);
                _oldInsuranceID = int.Parse(Ddl_ic_InsurancePlans.SelectedItem.Value);

                List<insurance> tempInsList = insuranceController.manufactureInsuranceList(_planYearID);
                insurance selectedInsurance = insuranceController.getSingleInsurancePlan(_insuranceID, tempInsList);

                List<insuranceContribution> tempList = insuranceController.manufactureInsuranceContributionList(_oldInsuranceID);
                insuranceContribution selectedInsCont = insuranceController.getSingleInsuranceContribution(_insContID, tempList);

                _history = selectedInsCont.INS_CONT_HISTORY;

                if (_insContTypeID == "$")
                {
                    if (_amount > (double)selectedInsurance.INSURANCE_COST)
                    {
                        validData = false;
                        txtAmount.BackColor = System.Drawing.Color.Red;
                        mpe.Show();
                        lblMess.Text = "The contribution amount can not be greater than the plan cost.";
                    }
                }
                else if (_insContTypeID == "%")
                {
                    if (_amount > 100)
                    {
                        txtAmount.BackColor = System.Drawing.Color.Red;
                        validData = false;
                        mpe.Show();
                        lblMess.Text = "The contribution percentage can not be greater than 100 percent.";
                    }
                }
                else
                {
                    validData = false;
                    mpe.Show();
                    lblMess.Text = "Please select a contribution type.";
                }

                if (validData == true)
                {
                    _history += System.Environment.NewLine;
                    _history += "Employer Contribution modified on " + _modOn.ToString() + " by " + _modBy + System.Environment.NewLine;
                    _history += "Contribution Type: " + _insContTypeID + " | Amount: " + _amount.ToString();

                    validTransaction = insuranceController.updateInsuranceContribution(_insContID, _insuranceID, _insContTypeID, _classID, _amount, _modBy, _modOn, _history);

                    if (validTransaction == true)
                    {
                        MpeWebPageMessage.Show();
                        LitMessage.Text = "The contribution has been updated.";
                        loadEmployerContributions();
                    }
                    else
                    {
                        mpe.Show();
                        lblMess.Text = "An error occurred while updating the contribution, please try again.";
                    }
                }
            }
            else
            {
                mpe.Show();
                lblMess.Text = "Please correct all fields that are highlighted in Red.";
            }
        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            LitMessage.Text = "An error occurred while UPDATING the Insurance Contribution. Please try again and if the issue continues, please contact " + Branding.CompanyShortName;
            MpeWebPageMessage.Show();
        }
    }

    #endregion



    protected void GvInsurance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvInsurance.PageIndex = e.NewPageIndex;
        loadInsurance(0);
    }

    protected void Gv_employerContributions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gv_employerContributions.PageIndex = e.NewPageIndex;
        loadEmployerContributions();
    }

    protected void BtnNewGPFilter_Click(object sender, EventArgs e)
    {
        MpeGPFilter.Show();
        LblGpFilterMessage.Text = null;
        DdlGrossPayFilter.SelectedIndex = DdlGrossPayFilter.Items.Count - 1;
    }

    protected void GvAlerts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvAlerts.PageIndex = e.NewPageIndex;
        loadAlerts();
    }

    protected void GvPlanYears_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPlanYears.PageIndex = e.NewPageIndex;
        loadGvPlanYears();
    }

    protected void GvDistrictUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvDistrictUsers.PageIndex = e.NewPageIndex;
        loadDistrictUsers();
    }

    protected void DdlEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadMeasurements(2);
    }

    protected void AddNewTempBreak_Click(object sender, EventArgs e)
    {
        if (false == Feature.SelfMeasurementPeriodsEnabled) { return; }    

        if (Session["TempBreaks"] == null)
        {
            Session["TempBreaks"] = new List<BreakInService>();
        }

        List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];

        BreakInService newBreak = new BreakInService();

        User currUser = (User)Session["CurrentUser"];

        newBreak.CreatedBy = currUser.User_UserName;
        DateTime start = new DateTime();
        DateTime end = new DateTime();

        try
        {
            start = System.Convert.ToDateTime(TxtSummerBreakStart.Text);
            end = System.Convert.ToDateTime(TxtSummerBreakEnd.Text);
            newBreak.StartDate = start;
            newBreak.EndDate = end;
        }
        catch (Exception exception)
        {
            Log.Info("User probably put in bad info, casuing an excetpion: ", exception);
        }

        if (end != new DateTime() &&
            start != new DateTime() &&
            end > start &&
            false == CheckForOverlap(breaks, start, end) 
            )
        {
            breaks.Add(newBreak);

            TxtSummerBreakEnd.Text = null;
            TxtSummerBreakStart.Text = null;
        }

        RefreshTempBreaksInService();
    }

    private void RefreshTempBreaksInService()
    {
        List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];
        Gv_TempBreakOfService.DataSource = breaks.OrderBy(breakinservice => breakinservice.StartDate);
        Gv_TempBreakOfService.DataBind();
        MpeMeasurementPeriod.Show();
    }

    protected void Gv_TempBreakOfService_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (false == Feature.SelfMeasurementPeriodsEnabled) { return; }    

        GridViewRow row = Gv_TempBreakOfService.Rows[e.RowIndex];

        TextBox startText = (TextBox)row.FindControl("TxtSummerBreakStart");
        TextBox endText = (TextBox)row.FindControl("TxtSummerBreakEnd");

        DateTime start;
        DateTime end;
        List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];

        if (DateTime.TryParse(startText.Text, out start) &&
            DateTime.TryParse(endText.Text, out end) &&
            breaks != null &&
            false == CheckForOverlap(breaks, start, end)
       
            )
        {
            BreakInService toEdit = breaks[e.RowIndex];
            if (toEdit != null)
            {
                toEdit.StartDate = start;
                toEdit.EndDate = end;
            }
        }

        Gv_TempBreakOfService.EditIndex = -1;

        RefreshTempBreaksInService();
    }

    protected void Gv_TempBreakOfService_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        Gv_TempBreakOfService.EditIndex = -1;

        RefreshTempBreaksInService();
    }

    protected void Gv_TempBreakOfService_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Gv_TempBreakOfService.EditIndex = e.NewEditIndex;

        RefreshTempBreaksInService();
    }

    protected void Gv_TempBreakOfService_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = Gv_TempBreakOfService.Rows[e.RowIndex];

        HiddenField hfId = (HiddenField)row.FindControl("HiddenBreakId");

        List<BreakInService> breaks = (List<BreakInService>)Session["TempBreaks"];

        breaks.RemoveAt(e.RowIndex);

        RefreshTempBreaksInService();
    }

    protected void AddNewEditBreak_Click(object sender, EventArgs e)
    {
        if (false == Feature.SelfMeasurementPeriodsEnabled) { return; }    

        Button btn = (Button)sender;
        RepeaterItem ritem = (RepeaterItem)btn.NamingContainer;
        ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
        GridView Gv_BreakOfService = (GridView)ritem.FindControl("Gv_BreakOfService");
        TextBox TxtEditSummerBreakStart = (TextBox)ritem.FindControl("TxtSummerBreakStart");
        TextBox TxtEditSummerBreakEnd = (TextBox)ritem.FindControl("TxtSummerBreakEnd");
        HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
        int measurementId = int.Parse(Hf_M_MeasurementID.Value);

        BreakInService newBreak = new BreakInService();
        User currUser = (User)Session["CurrentUser"];
        DateTime start = new DateTime();
        DateTime end = new DateTime();

        try
        {
            start = System.Convert.ToDateTime(TxtEditSummerBreakStart.Text);
            end = System.Convert.ToDateTime(TxtEditSummerBreakEnd.Text);
            newBreak.StartDate = start;
            newBreak.EndDate = end;
        }
        catch (Exception exception)
        {
            Log.Info("User probably put in bad info, casuing an excetpion: ", exception);
        }

        if (end != new DateTime() &&
            start != new DateTime() &&
            end > start &&
            false == CheckForOverlap(GetBreaks(measurementId), start, end) 
            )
        {
            newBreak.CreatedBy = currUser.User_UserName;

            BreakInServiceFactory factory = new BreakInServiceFactory();
            factory.InsertNewBreakInService(measurementId, newBreak);

            TxtSummerBreakEnd.Text = null;
            TxtSummerBreakStart.Text = null;
        }

        RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
    }

    private List<BreakInService> GetBreaks(int measurementId)
    {
        BreakInServiceFactory factory = new BreakInServiceFactory();
        return factory.SelectBreaksInService(measurementId);
    }

    private void RefreshBreaksInService(ModalPopupExtender MpeEditTransitionPeriod, GridView Gv_BreakOfService, int measurementId)
    {
        Gv_BreakOfService.DataSource = GetBreaks(measurementId).OrderBy(breaks => breaks.StartDate);
        Gv_BreakOfService.DataBind();
        MpeEditTransitionPeriod.Show();
    }

    protected void Gv_BreakOfService_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (false == Feature.SelfMeasurementPeriodsEnabled) { return; }    

        GridView Gv_BreakOfService = (GridView)sender;
        RepeaterItem ritem = (RepeaterItem)Gv_BreakOfService.NamingContainer;
        ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
        HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
        int measurementId = int.Parse(Hf_M_MeasurementID.Value);

        GridViewRow row = Gv_BreakOfService.Rows[e.RowIndex];

        HiddenField hfId = (HiddenField)row.FindControl("HiddenBreakId");
        TextBox startText = (TextBox)row.FindControl("TxtSummerBreakStart");
        TextBox endText = (TextBox)row.FindControl("TxtSummerBreakEnd");

        List<BreakInService> breaks = GetBreaks(measurementId);

        int Id;
        DateTime start;
        DateTime end;


        if (int.TryParse(hfId.Value, out Id) &&
            DateTime.TryParse(startText.Text, out start) &&
            DateTime.TryParse(endText.Text, out end) &&
            breaks != null 
            )
        {

            BreakInService toEdit = breaks.Where(Break => Break.BreakInServiceId == Id).FirstOrDefault();

            if (toEdit != null)
            {
                User currUser = (User)Session["CurrentUser"];

                toEdit.StartDate = start;
                toEdit.EndDate = end;
                toEdit.ModifiedBy = currUser.User_UserName;
                toEdit.ModifiedDate = DateTime.Now;

                BreakInServiceFactory factory = new BreakInServiceFactory();
                factory.UpdateBreakInService(measurementId, toEdit);
            }
        }

        Gv_BreakOfService.EditIndex = -1;

        RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
    }

    protected void Gv_BreakOfService_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        if (false == Feature.SelfMeasurementPeriodsEnabled) { return; }    

        GridView Gv_BreakOfService = (GridView)sender;
        RepeaterItem ritem = (RepeaterItem)Gv_BreakOfService.NamingContainer;
        ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
        HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
        int measurementId = int.Parse(Hf_M_MeasurementID.Value);

        Gv_BreakOfService.EditIndex = -1;

        RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
    }

    protected void Gv_BreakOfService_RowEditing(object sender, GridViewEditEventArgs e)
    {
        if (false == Feature.SelfMeasurementPeriodsEnabled) { return; }    

        GridView Gv_BreakOfService = (GridView)sender;
        RepeaterItem ritem = (RepeaterItem)Gv_BreakOfService.NamingContainer;
        ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
        HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
        int measurementId = int.Parse(Hf_M_MeasurementID.Value);

        Gv_BreakOfService.EditIndex = e.NewEditIndex;

        RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
    }

    protected void Gv_BreakOfService_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (false == Feature.SelfMeasurementPeriodsEnabled) { return; }    

        GridView Gv_BreakOfService = (GridView)sender;
        RepeaterItem ritem = (RepeaterItem)Gv_BreakOfService.NamingContainer;
        ModalPopupExtender MpeEditTransitionPeriod = (ModalPopupExtender)ritem.FindControl("MpeEditTransitionPeriod");
        HiddenField Hf_M_MeasurementID = (HiddenField)ritem.FindControl("Hf_M_MeasurementID");
        int measurementId = int.Parse(Hf_M_MeasurementID.Value);

        GridViewRow row = Gv_BreakOfService.Rows[e.RowIndex];

        HiddenField hfId = (HiddenField)row.FindControl("HiddenBreakId");

        int Id;

        if (int.TryParse(hfId.Value, out Id))
        {
            User currUser = (User)Session["CurrentUser"];

            BreakInServiceFactory factory = new BreakInServiceFactory();
            factory.DeleteBreakInService(Id, currUser.User_UserName);
        }

        RefreshBreaksInService(MpeEditTransitionPeriod, Gv_BreakOfService, measurementId);
    }

    public static bool CheckForOverlap(List<BreakInService> breaks, DateTime start, DateTime end)
    {
        foreach (var Break in breaks)
        {
            if ((start <= Break.EndDate && start >= Break.StartDate) ||
                (end <= Break.EndDate && end >= Break.StartDate) ||
                (Break.EndDate <= end && Break.EndDate >= start) ||
                (Break.StartDate <= end && Break.StartDate >= start))
            {
                return true;
            }
        }

        return false;
    }

    protected void GvEmployeeClassifications_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        GridViewRow row = GvEmployeeClassifications.SelectedRow;
        GvEmployeeClassifications.EditIndex = -1;
        HiddenField hfID = (HiddenField)row.FindControl("HfClassID");
        int employeeClassificationID = 0;
        int.TryParse(hfID.Value, out employeeClassificationID);

        List<classification> tempList = classificationController.ManufactureEmployerClassificationList(_employerID, true);
        classification myClass = classificationController.findClassification(employeeClassificationID, tempList);
        Txt_h_employeeClass.Text = myClass.CLASS_HISTORY;
        Mpe_h_employeeClass.Show();

    }

    protected void btnYesIRSContactChange_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        CheckBox chkbox = (CheckBox)row.FindControl("CbtmpIRSContact");
        if (chkbox.Checked == false)
        {
            ModalPopupExtender mde = (ModalPopupExtender)row.FindControl("ModalPopupExtenderIRSContactChange");
            mde.Hide();
            chkbox.Checked = true;
            List<User> userList = (List<User>)Session["UserList"];
            var irsContactUserList = from user in userList
                                     where user.User_IRS_CONTACT == true
                                     select user;

            if (irsContactUserList.Count() != 0)
            {
                userList.Select(c => { c.User_IRS_CONTACT = false; return true; }).ToList();
                Session["UserList"] = userList;
            }
        }
        else
        {
            chkbox.Checked = false;
        }
        ModalPopupExtender mdeparent = (ModalPopupExtender)row.FindControl("ModalPopupExtender3");
        mdeparent.Show();
    }


}