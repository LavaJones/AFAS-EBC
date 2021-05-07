using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class admin_admin_py_rollover_prep : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_admin_py_rollover_prep));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {

        LitUserName.Text = user.User_UserName;
        loadEmployers();

    }

    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {

        int _employerID = 0;
        employer _employer = null;

        if (DdlEmployer.SelectedItem.Text != "Select")
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            _employer = employerController.getEmployer(_employerID);


            loadPlanYears(_employerID);

            defaultValidationView();
        }
        else
        {
            defaultValidationView();
            defaultDropDownView();
        }

    }

    private void defaultValidationView()
    {

        CollapsiblePanelExtender1.Collapsed = true;
        CollapsiblePanelExtender2.Collapsed = true;
        CollapsiblePanelExtender3.Collapsed = true;

        ImgStep1.ImageUrl = "~/images/circle_red.png";
        ImgStep2.ImageUrl = "~/images/circle_red.png";
        ImgStep3.ImageUrl = "~/images/circle_red.png";
        ImgStep4.ImageUrl = "~/images/circle_red.png";
        ImgStep5.ImageUrl = "~/images/circle_red.png";

        LitCurrentPlanYear.Text = null;
        LitCurrNewHireCount.Text = null;
        LitCurrOngoingCount.Text = null;
        LitNewNewHireCount.Text = null;
        LitSum.Text = null;
        LitNewPlanYearID.Text = null;
        LitSHCount.Text = null;
        LitStep1End.Text = null;
        LitStep1Start.Text = null;
        LitStep2End.Text = null;
        LitStep2Start.Text = null;
        LitStep4End.Text = null;
        LitStep4Start.Text = null;
        LitTotalCount.Text = null;

        GvCheckDates.DataSource = null;
        GvCheckDates.DataBind();

        GvNhCurrPlan.DataSource = null;
        GvNhCurrPlan.DataBind();

        GvNhNewPlan.DataSource = null;
        GvNhNewPlan.DataBind();

        HfStep1Complete.Value = "false";
        HfStep2Complete.Value = "false";
        HfStep3Complete.Value = "false";
        HfStep4Complete.Value = "false";
        HfStep5Complete.Value = "false";

    }

    private void defaultDropDownView()
    {

        DdlPlanYearCurrent.Items.Clear();
        DdlPlanYearNew.Items.Clear();

    }

    private void loadEmployers()
    {

        DdlEmployer.DataSource = employerController.getAllEmployers();
        DdlEmployer.DataTextField = "EMPLOYER_NAME";
        DdlEmployer.DataValueField = "EMPLOYER_ID";
        DdlEmployer.DataBind();

        DdlEmployer.Items.Add("Select");
        DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;

    }

    /// <summary>
    /// 1-3) Loads a specific employer's Plan Years into a dropdown list. 
    /// </summary>
    /// <param name="_employerID"></param>
    private void loadPlanYears(int _employerID)
    {

        List<PlanYear> planYear = PlanYear_Controller.getEmployerPlanYear(_employerID);

        DdlPlanYearCurrent.DataSource = planYear;
        DdlPlanYearCurrent.DataTextField = "PLAN_YEAR_DESCRIPTION";
        DdlPlanYearCurrent.DataValueField = "PLAN_YEAR_ID";
        DdlPlanYearCurrent.DataBind();

        DdlPlanYearCurrent.Items.Add("Select");
        DdlPlanYearCurrent.SelectedIndex = DdlPlanYearCurrent.Items.Count - 1;

        DdlPlanYearNew.DataSource = planYear;
        DdlPlanYearNew.DataTextField = "PLAN_YEAR_DESCRIPTION";
        DdlPlanYearNew.DataValueField = "PLAN_YEAR_ID";
        DdlPlanYearNew.DataBind();

        DdlPlanYearNew.Items.Add("Select");
        DdlPlanYearNew.SelectedIndex = DdlPlanYearNew.Items.Count - 1;

    }

    protected void BtnProcessFile_Click(object sender, EventArgs e)
    {
        loadData();
    }

    private void loadData()
    {

        Log.Info("Loading Data For Measurement period Rollover");

        int _employerID = 0;
        int _currPlanYearID = 0;
        int _newPlanYearID = 0;
        int _employeeTypeID = 0;
        Measurement currMeas = null;
        Measurement newMeas = null;
        bool validData = true;

        List<Employee> tempEmpList = null;
        List<Employee> ongoingCurrPlanYear = new List<Employee>();
        List<Employee> nhNewPlanYear = new List<Employee>();
        List<Employee> nhCurrPlanYear = new List<Employee>();
        List<Employee> errorList = new List<Employee>();                       

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlPlanYearNew, validData);
        validData = errorChecking.validateDropDownSelection(DdlPlanYearCurrent, validData);

        Log.Info("Data1 Validated: " + validData);
        if (validData == true)
        {

            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            validData = errorChecking.comparePlanYearSelectionOldNew(DdlPlanYearNew, DdlPlanYearCurrent, _employerID, validData);

        }

        Log.Info("Data2 Validated: " + validData);
        if (validData == true)
        {

            _currPlanYearID = int.Parse(DdlPlanYearCurrent.SelectedItem.Value);
            _newPlanYearID = int.Parse(DdlPlanYearNew.SelectedItem.Value);

            PlanYear CurrentPy = PlanYear_Controller.findPlanYear(_currPlanYearID, _employerID);
            PlanYear newPy = PlanYear_Controller.findPlanYear(_newPlanYearID, _employerID);

            if (CurrentPy.PlanYearGroupId != newPy.PlanYearGroupId)
            {
                Log.Info(String.Format("User tried to roll over Plan Year Id:[{0}] to Plan Year Id:[{1}] which does not have the same Plan Year Groups: [{2}] and [{3}]",
                    CurrentPy.PLAN_YEAR_ID, newPy.PLAN_YEAR_ID, CurrentPy.PlanYearGroupId, newPy.PlanYearGroupId));
                MpeRolloverMessage.Show();
                LblRolloverMessage.Text = "Cannot rollover Plan Years from one Group into another Plan Year Group.";

                return;
            }

            List<int> PlanYearIdsInGroup = (from plan in PlanYear_Controller.getEmployerPlanYear(_employerID)
                                            where plan.PlanYearGroupId == CurrentPy.PlanYearGroupId
                                            select plan.PLAN_YEAR_ID).ToList();

            tempEmpList = EmployeeController.manufactureEmployeeList(_employerID);
            tempEmpList = (from emp in tempEmpList where PlanYearIdsInGroup.Contains(emp.EMPLOYEE_PLAN_YEAR_ID_MEAS) select emp).ToList();

            foreach (EmployeeType type in EmployeeTypeController.getEmployeeTypes(_employerID))
            {
                _employeeTypeID = type.EMPLOYEE_TYPE_ID;

                currMeas = measurementController.getPlanYearMeasurement(_employerID, _currPlanYearID, _employeeTypeID);
                newMeas = measurementController.getPlanYearMeasurement(_employerID, _newPlanYearID, _employeeTypeID);

                if (currMeas == null || newMeas == null)
                {
                    Log.Info("Missing Measurement Periods for EmplyeeType: " + type.EMPLOYEE_TYPE_NAME);

                    MpeRolloverMessage.Show();
                    LblRolloverMessage.Text = "Missing Measurement Period for Emplyee Type: " + type.EMPLOYEE_TYPE_NAME;

                    return;
                }

                List<Employee> filteredForEmployeeType = (
                                                          from Employee employee in tempEmpList
                                                          where employee.EMPLOYEE_TYPE_ID == _employeeTypeID
                                                          && employee.EMPLOYEE_HIRE_DATE <= newMeas.MEASUREMENT_END
                                                          select employee
                                                         ).ToList();

                List<Employee> remove = (from Employee employee in tempEmpList
                                         where employee.EMPLOYEE_TYPE_ID == _employeeTypeID
                                         && employee.EMPLOYEE_HIRE_DATE > newMeas.MEASUREMENT_END
                                         select employee).ToList();

                foreach (Employee ep in remove)
                {
                    tempEmpList.Remove(ep);
                }

                if (filteredForEmployeeType.Count <= 0)
                {
                    continue;
                }

                if (CanRollOver(filteredForEmployeeType, currMeas, newMeas, _employerID, _currPlanYearID, _newPlanYearID, _employeeTypeID))
                {

                    nhCurrPlanYear.AddRange(NhCurrPlanYearEmployeeByType(filteredForEmployeeType, currMeas));


                    nhNewPlanYear.AddRange(NhNewPlanYearEmployeeByType(filteredForEmployeeType, newMeas));


                    errorList.AddRange(ErrorListEmployeeByType(filteredForEmployeeType, newMeas, _newPlanYearID, _currPlanYearID));


                    ongoingCurrPlanYear.AddRange(OngoingCurrPlanYearEmployeeByType(filteredForEmployeeType, currMeas, newMeas, _currPlanYearID));
                }

            }

            GvNhCurrPlan.DataSource = nhCurrPlanYear;
            GvNhCurrPlan.DataBind();
            GvNhNewPlan.DataSource = nhNewPlanYear;
            GvNhNewPlan.DataBind();
            GvStep3Errors.DataSource = errorList;
            GvStep3Errors.DataBind();
            GvCheckDates.DataSource = Payroll_Controller.getEmployerCheckDates(_employerID, currMeas.MEASUREMENT_START);
            GvCheckDates.DataBind();

            LitCurrentPlanYear.Text = _currPlanYearID.ToString();
            LitNewPlanYearID.Text = _newPlanYearID.ToString();
            LitStep1Start.Text = currMeas.MEASUREMENT_START.ToShortDateString();
            LitStep1End.Text = currMeas.MEASUREMENT_END.ToShortDateString();
            LitStep2Start.Text = newMeas.MEASUREMENT_START.ToShortDateString();
            LitStep2End.Text = newMeas.MEASUREMENT_END.ToShortDateString();
            LitStep4Start.Text = currMeas.MEASUREMENT_START.ToShortDateString();
            LitStep4End.Text = currMeas.MEASUREMENT_END.ToShortDateString();
            LitTotalCount.Text = tempEmpList.Count.ToString();
            LitCurrNewHireCount.Text = nhCurrPlanYear.Count.ToString();
            LitNewNewHireCount.Text = nhNewPlanYear.Count.ToString();
            LitCurrOngoingCount.Text = ongoingCurrPlanYear.Count.ToString();


            ValidateEmployeeLists(tempEmpList, nhCurrPlanYear, nhNewPlanYear, ongoingCurrPlanYear);


            validateNewHiresNewPlanYear();
            validateNewHireCurrPlanYear();
            validateSummerHours();
            validateEmployeeImportAlerts();
            LitSum.Text = validateEmployeeCounts().ToString();
        }
        else
        {
            Log.Info("Please correct all fields highlighted in RED.");

            MpeRolloverMessage.Show();
            LblRolloverMessage.Text = "Please correct all fields highlighted in RED.";
        }

        Log.Info("Load Data Finished");
    }

    private bool CanRollOver(List<Employee> filteredForEmployeeType, Measurement currMeas, Measurement newMeas, int _employerID, int _currPlanYearID, int _newPlanYearID, int _employeeTypeID)
    {

        if (filteredForEmployeeType.Count <= 0)
        {

            Log.Info("Found No Employees to Roll Over.");

            MpeRolloverMessage.Show();
            LblRolloverMessage.Text = "Found No Employees to Roll Over.";

            return false;

        }

        if (null == currMeas)
        {

            Log.Info("Current Measurment period not Found.");

            MpeRolloverMessage.Show();
            LblRolloverMessage.Text = "Current Measurment period not Found.";

            return false;

        }

        if (null == newMeas)
        {

            Log.Info("New Measurment period not Found.");

            MpeRolloverMessage.Show();
            LblRolloverMessage.Text = "New Measurment period not Found.";

            return false;

        }

        return true;
    }

    private List<Employee> NhCurrPlanYearEmployeeByType(List<Employee> filteredForEmployeeType, Measurement currMeas)        
    {
        List<Employee> nhCurrPlanYear = new List<Employee>();

        foreach (Employee emp in filteredForEmployeeType)
        {
            if (emp.EMPLOYEE_HIRE_DATE >= currMeas.MEASUREMENT_START && emp.EMPLOYEE_HIRE_DATE <= currMeas.MEASUREMENT_END)
            {
                nhCurrPlanYear.Add(emp);
            }
        }
        Log.Info("nhCurrPlanYear Count : " + nhCurrPlanYear.Count);

        return nhCurrPlanYear;
    }

    private List<Employee> NhNewPlanYearEmployeeByType(List<Employee> filteredForEmployeeType, Measurement newMeas)       
    {
        List<Employee> nhNewPlanYear = new List<Employee>();

        foreach (Employee emp in filteredForEmployeeType)
        {
            if (null == emp || null == newMeas || null == nhNewPlanYear)
            {
                Log.Info("Employee is : " + emp + "; New Measurement is : " + newMeas + "; nhNewPlanYear is : " + nhNewPlanYear);
            }

            if (emp.EMPLOYEE_HIRE_DATE >= newMeas.MEASUREMENT_START && emp.EMPLOYEE_HIRE_DATE <= newMeas.MEASUREMENT_END)
            {
                nhNewPlanYear.Add(emp);
            }
        }
        Log.Info("nhNewPlanYear count : " + nhNewPlanYear.Count);

        return nhNewPlanYear;
    }

    private List<Employee> OngoingCurrPlanYearEmployeeByType(List<Employee> filteredForEmployeeType, Measurement currMeas, Measurement newMeas, int _currPlanYearID)
    {
        List<Employee> ongoingCurrPlanYear = new List<Employee>();

        foreach (Employee emp in filteredForEmployeeType)
        {
            if (emp.EMPLOYEE_HIRE_DATE < currMeas.MEASUREMENT_START && emp.EMPLOYEE_PLAN_YEAR_ID_MEAS == _currPlanYearID)
            {
                ongoingCurrPlanYear.Add(emp);
            }
        }
        Log.Info("ongoingCurrPlanYear count : " + ongoingCurrPlanYear.Count);

        return ongoingCurrPlanYear;
    }

    private List<Employee> ErrorListEmployeeByType(List<Employee> filteredForEmployeeType, Measurement newMeas, int _newPlanYearID, int _currPlanYearID)
    {
        List<Employee> errorList = new List<Employee>();                       

        foreach (Employee emp in filteredForEmployeeType)
        {
            if (emp.EMPLOYEE_PLAN_YEAR_ID_MEAS != _newPlanYearID && emp.EMPLOYEE_PLAN_YEAR_ID_MEAS != _currPlanYearID)
            {
                errorList.Add(emp);
            }
            else if (emp.EMPLOYEE_HIRE_DATE > newMeas.MEASUREMENT_END)
            {
                errorList.Add(emp);
            }
            else if (emp.EMPLOYEE_HIRE_DATE < newMeas.MEASUREMENT_START && emp.EMPLOYEE_PLAN_YEAR_ID_MEAS == _newPlanYearID)
            {
                errorList.Add(emp);
            }
        }
        Log.Info("errorList count : " + errorList.Count);

        return errorList;
    }

    private void ValidateEmployeeLists(List<Employee> tempEmpList, List<Employee> nhCurrPlanYear, List<Employee> nhNewPlanYear, List<Employee> ongoingCurrPlanYear)
    {
        List<Employee> notInList = tempEmpList;

        foreach (Employee emp in nhCurrPlanYear)
        {
            if (notInList.Contains(emp))
            {
                notInList.Remove(emp);
            }
        }
        foreach (Employee emp in nhNewPlanYear)
        {
            if (notInList.Contains(emp))
            {
                notInList.Remove(emp);
            }
        }
        foreach (Employee emp in ongoingCurrPlanYear)
        {
            if (notInList.Contains(emp))
            {
                notInList.Remove(emp);
            }
        }

        foreach (Employee emp in notInList)
        {
            Log.Warn(String.Format("An Employee was malformed: [Id:{0}; PlanYearId:{1}; ClassificationId:{2}; ExtId:{3}; HrStatusId:{4}; PlanYearLimboId:{5}; PlanYearMeasuId:{6}; EmpTypeId:{7}]",
                emp.EMPLOYEE_ID,
                emp.EMPLOYEE_PLAN_YEAR_ID,
                emp.EMPLOYEE_CLASS_ID,
                emp.EMPLOYEE_EXT_ID,
                emp.EMPLOYEE_HR_STATUS_ID,
                emp.EMPLOYEE_PLAN_YEAR_ID_LIMBO,
                emp.EMPLOYEE_PLAN_YEAR_ID_MEAS,
                emp.EMPLOYEE_TYPE_ID));
        }
    }


    private int validateEmployeeCounts()
    {
        int currNH = 0;
        int newNH = 0;
        int currOngoing = 0;
        int employeeTotalCount = 0;
        int sum = 0;

        currNH = int.Parse(LitCurrNewHireCount.Text);
        newNH = int.Parse(LitNewNewHireCount.Text);
        currOngoing = int.Parse(LitCurrOngoingCount.Text);
        employeeTotalCount = int.Parse(LitTotalCount.Text);
        sum = currNH + newNH + currOngoing;

        if (sum == employeeTotalCount)
        {
            ImgStep3.ImageUrl = "~/images/circle_green.png";
            HfStep3Complete.Value = "true";
        }
        else
        {
            ImgStep3.ImageUrl = "~/images/circle_red.png";
            HfStep3Complete.Value = "false";
        }

        return sum;
    }

    private void validateNewHiresNewPlanYear()
    {
        int badrecords = 0;

        foreach (GridViewRow row in GvNhNewPlan.Rows)
        {
            int _newPlanYearID = 0;
            int _actualPlanYearID = 0;
            Literal litID = (Literal)row.FindControl("Lit_gv_measID");
            Image imgStep = (Image)row.FindControl("Img_gv_Step2");

            _newPlanYearID = int.Parse(DdlPlanYearNew.SelectedItem.Value);
            _actualPlanYearID = int.Parse(litID.Text);

            if (_newPlanYearID == _actualPlanYearID)
            {
                imgStep.ImageUrl = "~/images/circle_green.png";
            }
            else
            {
                imgStep.ImageUrl = "~/images/circle_red.png";
                badrecords += 1;
            }
        }

        if (badrecords > 0)
        {
            ImgStep2.ImageUrl = "~/images/circle_red.png";
            HfStep2Complete.Value = "false";
        }
        else
        {
            ImgStep2.ImageUrl = "~/images/circle_green.png";
            HfStep2Complete.Value = "true";
        }
    }

    private void validateNewHireCurrPlanYear()
    {
        int badrecords = 0;

        foreach (GridViewRow row in GvNhCurrPlan.Rows)
        {
            int _currPlanYearID = 0;
            int _actualPlanYearID = 0;
            Literal litID = (Literal)row.FindControl("Lit_gv_measID");
            Image imgStep = (Image)row.FindControl("Img_gv_Step1");

            _currPlanYearID = int.Parse(DdlPlanYearCurrent.SelectedItem.Value);
            _actualPlanYearID = int.Parse(litID.Text);

            if (_currPlanYearID == _actualPlanYearID)
            {
                imgStep.ImageUrl = "~/images/circle_green.png";
            }
            else
            {
                imgStep.ImageUrl = "~/images/circle_red.png";
                badrecords += 1;
            }
        }

        if (badrecords > 0)
        {
            ImgStep1.ImageUrl = "~/images/circle_red.png";
            HfStep1Complete.Value = "false";
        }
        else
        {
            ImgStep1.ImageUrl = "~/images/circle_green.png";
            HfStep1Complete.Value = "true";
        }
    }

    private void validateEmployeeImportAlerts()
    {
        int _employerID = 0;
        int count = 0;
        _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        List<Employee_I> alertList = EmployeeController.manufactureImportEmployeeList(_employerID);

        count = alertList.Count;
        LitStep6Count.Text = count.ToString();

        Log.Info("Validating Employees, found " + count + " employee alerts.");

        if (count > 0)
        {
            ImgStep6.ImageUrl = "~/images/circle_red.png";
            HfStep6Complete.Value = "false";
        }
        else
        {
            ImgStep6.ImageUrl = "~/images/circle_green.png";
            HfStep6Complete.Value = "true";
        }

    }

    private void validateSummerHours()
    {
        ImgStep5.ImageUrl = "~/images/circle_green.png";
        HfStep5Complete.Value = "true";
    }

    protected void BtnRolloverMP_Click(object sender, EventArgs e)
    {
        bool step1 = false;
        bool step2 = false;
        bool step3 = false;
        bool step4 = false;
        bool step5 = false;
        bool step6 = false;
        int _employerID = 0;
        int _currPlanYearID = 0;
        int _newPlanYearID = 0;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;

        try
        {
            DateTime measStart = DateTime.Parse(LitStep1Start.Text);
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            _currPlanYearID = int.Parse(DdlPlanYearCurrent.SelectedItem.Value);
            _newPlanYearID = int.Parse(DdlPlanYearNew.SelectedItem.Value);

            step1 = bool.Parse(HfStep1Complete.Value);
            step2 = bool.Parse(HfStep2Complete.Value);
            step3 = bool.Parse(HfStep3Complete.Value);
            step4 = bool.Parse(HfStep4Complete.Value);
            step5 = bool.Parse(HfStep5Complete.Value);
            step6 = bool.Parse(HfStep6Complete.Value);

            if (step1 == true && step2 == true && step3 == true && step4 == true && step5 == true && step6 == true)
            {
                bool validTransaction = false;

                List<Employee> tempEmpList = EmployeeController.manufactureEmployeeList(_employerID);


                foreach (EmployeeType type in EmployeeTypeController.getEmployeeTypes(_employerID))
                {

                    int count = (from emp in tempEmpList
                                 where emp.EMPLOYEE_TYPE_ID == type.EMPLOYEE_TYPE_ID
                                     && emp.EMPLOYEE_PLAN_YEAR_ID_MEAS == _currPlanYearID
                                 select emp).Count();

                    if (count > 0)
                    {
                        validTransaction = EmployeeController.UpdateEmployeePlanYearPeriodMeasurement(_employerID, type.EMPLOYEE_TYPE_ID, _currPlanYearID, _newPlanYearID, _modOn, _modBy);
                        if (validTransaction == true)
                        {
                            MpeRolloverMessage.Show();
                            LblRolloverMessage.Text = "The Measurement Period has been moved to the Administrative Period.";
                        }
                        else
                        {
                            MpeRolloverMessage.Show();
                            LblRolloverMessage.Text = "ERROR, the system could not rollover the Measurement Period due to an ERROR.";

                            return;              
                        }
                    }
                }

                employerController.insertEmployerCalculation(_employerID);
            }
            else
            {
                MpeRolloverMessage.Show();
                LblRolloverMessage.Text = "Please validate all data. They steps with RED circles have invalid data.";
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            MpeRolloverMessage.Show();
            LblRolloverMessage.Text = "Please validate all data. They steps with RED circles have invalid data.";
        }
    }

    protected void BtnApprovePayroll_Click(object sender, EventArgs e)
    {
        ImgStep4.ImageUrl = "~/images/circle_green.png";
        HfStep4Complete.Value = "true";
    }

    protected void BtnOverride_Click(object sender, EventArgs e)
    {
        ImgStep5.ImageUrl = "~/images/circle_green.png";
        HfStep5Complete.Value = "true";
    }

    protected void BtnUpdateStep1_Click(object sender, EventArgs e)
    {
        int _employerID = 0;
        int _planYearID = 0;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;
        bool validTransaction = false;
        _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        _planYearID = int.Parse(LitCurrentPlanYear.Text);

        foreach (GridViewRow row in GvNhCurrPlan.Rows)
        {
            validTransaction = false;
            int _employeeID = 0;
            HiddenField hfID = (HiddenField)row.FindControl("Hf_gv_id");
            _employeeID = int.Parse(hfID.Value);
            validTransaction = EmployeeController.updateEmployeePlanYearMeasId(_employeeID, _planYearID, _modOn, _modBy);
        }

        loadData();
    }

    protected void BtnUpdateStep2_Click(object sender, EventArgs e)
    {
        int _employerID = 0;
        int _planYearID = 0;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;
        bool validTransaction = false;
        _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        _planYearID = int.Parse(LitNewPlanYearID.Text);

        foreach (GridViewRow row in GvNhNewPlan.Rows)
        {
            validTransaction = false;
            int _employeeID = 0;
            HiddenField hfID = (HiddenField)row.FindControl("Hf_gv_id");
            _employeeID = int.Parse(hfID.Value);
            validTransaction = EmployeeController.updateEmployeePlanYearMeasId(_employeeID, _planYearID, _modOn, _modBy);
        }

        loadData();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void GvStep3Errors_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        int _currMeasPlanYearID = 0;

        try
        {
            GvStep3Errors.EditIndex = e.NewEditIndex;
            loadData();

            DropDownList ddl_gv_PlanYear = (DropDownList)GvStep3Errors.Rows[e.NewEditIndex].FindControl("Ddl_gv_PlanYears");
            HiddenField hf_gv_measPYid = (HiddenField)GvStep3Errors.Rows[e.NewEditIndex].FindControl("Hf_gv_measID");
            _currMeasPlanYearID = int.Parse(hf_gv_measPYid.Value);

            ddl_gv_PlanYear.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
            ddl_gv_PlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
            ddl_gv_PlanYear.DataValueField = "PLAN_YEAR_ID";
            ddl_gv_PlanYear.DataBind();
            ddl_gv_PlanYear.Items.Add("Select");

            errorChecking.setDropDownList(ddl_gv_PlanYear, _currMeasPlanYearID);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
    }

    protected void GvStep3Errors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GvStep3Errors.EditIndex = -1;
        loadData();
    }

    protected void GvStep3Errors_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        bool validTransaction = false;
        bool validData = true;
        int _employeeID = 0;
        int _planYearID = 0;
        DateTime _modOn = DateTime.Now;
        string _modBy = LitUserName.Text;

        try
        {
            GridViewRow row = (GridViewRow)GvStep3Errors.Rows[e.RowIndex];
            HiddenField hfID = (HiddenField)row.FindControl("Hf_gv_id");
            DropDownList ddlPlanYear = (DropDownList)row.FindControl("Ddl_gv_PlanYears");

            validData = errorChecking.validateDropDownSelection(ddlPlanYear, validData);

            if (validData == true)
            {
                _planYearID = int.Parse(ddlPlanYear.SelectedItem.Value);
                _employeeID = int.Parse(hfID.Value);

                validTransaction = EmployeeController.updateEmployeePlanYearMeasId(_employeeID, _planYearID, _modOn, _modBy);

                if (validTransaction == true)
                {
                    GvStep3Errors.EditIndex = -1;
                    loadData();
                }
                else
                {
                    MpeRolloverMessage.Show();
                    LblRolloverMessage.Text = "An error occurred while trying to update the employee.";
                }
            }
            else
            {
                MpeRolloverMessage.Show();
                LblRolloverMessage.Text = "Please correct any fields highlighted in RED.";
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            MpeRolloverMessage.Show();
            LblRolloverMessage.Text = "An error occurred while trying to update the employee.";
        }
    }
}