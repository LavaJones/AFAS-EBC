using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;

public partial class admin_view_employers : Afas.AfComply.UI.admin.AdminPageBase
{

    private ILog Log = LogManager.GetLogger(typeof(admin_view_employers));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {

        if (Feature.FastCalculationEnabled == true)
        {
            Server.ScriptTimeout = 600;
        }

        LitUserName.Text = user.User_UserName;

        DdlFilterEmployers.DataSource = employerController.getAllEmployers();
        DdlFilterEmployers.DataTextField = "EMPLOYER_NAME";
        DdlFilterEmployers.DataValueField = "EMPLOYER_ID";
        DdlFilterEmployers.DataBind();

        DdlFilterEmployers.Items.Add("Select");
        DdlFilterEmployers.SelectedIndex = DdlFilterEmployers.Items.Count - 1;
    
    }

    /*********************************************************************************************
    GROUP 1: All functions that load data into dropdown lists & gridviews. ****************** 
   *********************************************************************************************/
    /// <summary>
    /// 1-1) Load all existing employers into a dropdown list. 
    /// </summary>
    private void loadEmployers(int _employerID)
    {
        List<employer> tempList = new List<employer>();
        tempList.Add(employerController.getEmployer(_employerID));
        GvEmployers.DataSource = tempList;
        GvEmployers.DataBind();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void GvEmployers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litID = (Literal)e.Row.FindControl("LitEmployerID");
            GridView gv = (GridView)e.Row.FindControl("Gv_gv_PlanYears");
            GridView gv2 = (GridView)e.Row.FindControl("Gv_gv_Users");
            GridView gv3 = (GridView)e.Row.FindControl("Gv_gv_Alerts");
            DropDownList ddlVendors = (DropDownList)e.Row.FindControl("DdlPayrollVendors");
            DropDownList ddl = (DropDownList)e.Row.FindControl("DdlAlertType");
            Literal employeeCount = (Literal)e.Row.FindControl("EmployeeCount");
            Literal batchCount = (Literal)e.Row.FindControl("BatchCount");
            Literal hrStatusCount = (Literal)e.Row.FindControl("HrStatusCount");
            Literal payDescCount = (Literal)e.Row.FindControl("PayDescCount");

            int _employerID = System.Convert.ToInt32(litID.Text);
            employer _emp = employerController.getEmployer(_employerID);

            employeeCount.Text = EmployeeController.manufactureEmployeeList(_emp.EMPLOYER_ID).Count.ToString();
            batchCount.Text = employerController.manufactureBatchList(_emp.EMPLOYER_ID).Count.ToString();
            hrStatusCount.Text = hrStatus_Controller.manufactureHRStatusList(_emp.EMPLOYER_ID).Count.ToString();
            payDescCount.Text = gpType_Controller.getEmployeeTypes(_emp.EMPLOYER_ID).Count.ToString();

            gv3.DataSource = alert_controller.manufactureEmployerAlertListAll(_employerID);
            gv3.DataBind();

            gv2.DataSource = UserController.getDistrictUsers(_employerID);
            gv2.DataBind();

            gv.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID).OrderBy(planyear => planyear.PLAN_YEAR_START);
            gv.DataBind();

            ddl.DataSource = alert_controller.manufactureAlertTypeList();
            ddl.DataTextField = "ALERT_NAME";
            ddl.DataValueField = "ALERT_ID";
            ddl.DataBind();

            ddl.Items.Add("Select");
            ddl.SelectedIndex = ddl.Items.Count - 1;


            ddlVendors.DataSource = employerController.manufactureVendors();
            ddlVendors.DataTextField = "VENDOR_NAME";
            ddlVendors.DataValueField = "VENDOR_ID";
            ddlVendors.DataBind();
            ddlVendors.Items.Add("Select");

            if (_emp.EMPLOYER_VENDOR_ID > 0)
            {
                errorChecking.setDropDownList(ddlVendors, _emp.EMPLOYER_VENDOR_ID);
            }
            else
            {
                ddlVendors.SelectedIndex = ddlVendors.Items.Count - 1;
            }
        }
    }

    protected void Gv_gv_PlanYears_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblRenewal = (Label)e.Row.FindControl("Lbl_gv_Renewal");
            Image imgValid = (Image)e.Row.FindControl("img_gv_MP");
            Literal litMPstart = (Literal)e.Row.FindControl("Lit_gv_MP_start");
            Literal litMPend = (Literal)e.Row.FindControl("Lit_gv_MP_end");
            Literal litAPstart = (Literal)e.Row.FindControl("Lit_gv_AP_start");
            Literal litAPend = (Literal)e.Row.FindControl("Lit_gv_AP_end");
            Literal litOEstart = (Literal)e.Row.FindControl("Lit_gv_OE_start");
            Literal litOEend = (Literal)e.Row.FindControl("Lit_gv_OE_end");
            Literal litSPstart = (Literal)e.Row.FindControl("Lit_gv_SP_start");
            Literal litSPend = (Literal)e.Row.FindControl("Lit_gv_SP_end");
            Literal litSWstart = (Literal)e.Row.FindControl("Lit_gv_SW_start");
            Literal litSWend = (Literal)e.Row.FindControl("Lit_gv_SW_end");
            Literal litSW2start = (Literal)e.Row.FindControl("Lit_gv_SW2_start");
            Literal litSW2end = (Literal)e.Row.FindControl("Lit_gv_SW2_end");
            HiddenField hfEmployerID = (HiddenField)e.Row.FindControl("Hf_gv_employerID");
            HiddenField hfPlanID = (HiddenField)e.Row.FindControl("Hf_gv_planID");

            Measurement currMeas = null;
            DateTime renewal;
            DateTime redDate = DateTime.Parse("1/1/2015");
            DateTime orangeDate = DateTime.Parse("7/1/2015");

            int _planID = int.Parse(hfPlanID.Value);
            int _employerID = int.Parse(hfEmployerID.Value);

            List<Measurement> tempList = measurementController.manufactureMeasurementList(_employerID);

            foreach (Measurement m in tempList)
            {
                if (m.MEASUREMENT_PLAN_ID == _planID)
                {
                    currMeas = m;
                    break;
                }
            }

            if (currMeas != null)
            {
                litMPstart.Text = currMeas.MEASUREMENT_START.ToShortDateString();
                litMPend.Text = currMeas.MEASUREMENT_END.ToShortDateString();
                litOEstart.Text = currMeas.MEASUREMENT_OPEN_START.ToShortDateString();
                litOEend.Text = currMeas.MEASUREMENT_OPEN_END.ToShortDateString();
                litAPstart.Text = currMeas.MEASUREMENT_ADMIN_START.ToShortDateString();
                litAPend.Text = currMeas.MEASUREMENT_ADMIN_END.ToShortDateString();
                litSPstart.Text = currMeas.MEASUREMENT_STAB_START.ToShortDateString();
                litSPend.Text = currMeas.MEASUREMENT_STAB_END.ToShortDateString();
            }
            else
            { 
            
            }
            
            try
            {
                renewal = DateTime.Parse(lblRenewal.Text);

                if (renewal < redDate)
                {
                    imgValid.ImageUrl = "~/images/circle_red.png";
                }
                else if (litMPstart.Text == "")
                {
                    imgValid.ImageUrl = "~/images/circle_red.png";
                }
                else
                {
                    imgValid.ImageUrl = "~/images/circle_green.png";
                }

                if (renewal == redDate)
                {
                    lblRenewal.BackColor = System.Drawing.Color.Yellow;
                }

                ////Validate that the Summer Window has been entered. 
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                imgValid.ImageUrl = "~/images/circle_red.png";
            }

        }
    }

    protected void GvEmployers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = (GridViewRow)GvEmployers.Rows[e.RowIndex];

        Literal litEmployerID = null;
        TextBox txtInitEmployeeImport = null;
        TextBox txtInitEmployeeCleanup = null;
        TextBox txtFTPEmployeeImport = null;
        TextBox txtFTPEmployeeCleanup = null;
        TextBox txtInitPayrollImport = null;
        TextBox txtInitPayrollCleanup = null;
        TextBox txtFTPPayrollImport = null;
        TextBox txtFTPPayrollCleanup = null;
        TextBox txtProcessCompleted = null;
        CheckBox cbBilling = null;
        CheckBox cbFileUpload = null;

        TextBox txtPaySU = null;
        TextBox txtDemSU = null;
        TextBox txtGpSU = null;
        TextBox txtHrSU = null;
        TextBox txtEcSU = null;
        TextBox txtIoSU = null;
        TextBox txtIcSU = null;
        TextBox txtPayModSU = null;
        DropDownList ddlPayrollVendor = null;

        litEmployerID = (Literal)row.FindControl("LitEmployerID");
        txtInitEmployeeImport = (TextBox)row.FindControl("TxtEmpInit_I");
        txtInitEmployeeCleanup = (TextBox)row.FindControl("TxtEmpInit_C");
        txtFTPEmployeeImport = (TextBox)row.FindControl("TxtEmpFTP_I");
        txtFTPEmployeeCleanup = (TextBox)row.FindControl("TxtEmpFPT_C");
        txtInitPayrollImport = (TextBox)row.FindControl("TxtPayInit_I");
        txtInitPayrollCleanup = (TextBox)row.FindControl("TxtPayInit_C");
        txtFTPPayrollImport = (TextBox)row.FindControl("TxtPayFTP_I");
        txtFTPPayrollCleanup = (TextBox)row.FindControl("TxtPayFTP_C");
        txtProcessCompleted = (TextBox)row.FindControl("TxtComplete");
        cbBilling = (CheckBox)row.FindControl("CbMonthlyBillingOn");
        cbFileUpload = (CheckBox)row.FindControl("CbAutoFileUploadOn");

        txtPaySU = (TextBox)row.FindControl("TxtPaySU");
        txtDemSU = (TextBox)row.FindControl("TxtDemSU");
        txtGpSU = (TextBox)row.FindControl("TxtPcSU");
        txtHrSU = (TextBox)row.FindControl("TxtHrSU");
        txtEcSU = (TextBox)row.FindControl("TxtEcSU");
        txtIoSU = (TextBox)row.FindControl("TxtIoSU");
        txtIcSU = (TextBox)row.FindControl("TxtIcSu");
        txtPayModSU = (TextBox)row.FindControl("TxtPayModSu");
        ddlPayrollVendor = (DropDownList)row.FindControl("DdlPayrollVendors");

        int _employerID = int.Parse(litEmployerID.Text);
        string _iei = txtInitEmployeeImport.Text;
        string _iec = txtInitEmployeeCleanup.Text;
        string _ftpei = txtFTPEmployeeImport.Text;
        string _ftpec = txtFTPEmployeeCleanup.Text;
        string _ipi = txtInitPayrollImport.Text;
        string _ipc = txtInitPayrollCleanup.Text;
        string _ftppi = txtFTPPayrollImport.Text;
        string _ftppc = txtFTPPayrollCleanup.Text;
        string _pc = txtProcessCompleted.Text;
        bool _billing = cbBilling.Checked;
        bool _fileUpload = cbFileUpload.Checked;

        string _paySU = txtPaySU.Text;
        string _demSU = txtDemSU.Text;
        string _gpSU = txtGpSU.Text;
        string _hrSU = txtHrSU.Text;
        string _ecSU = txtEcSU.Text;
        string _ioSU = txtIoSU.Text;
        string _icSU = txtIcSU.Text;
        string _icPayMod = txtPayModSU.Text;
        int _vendorID = 0;
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(ddlPayrollVendor, validData);

        if (validData == true)
        {
            _vendorID = int.Parse(ddlPayrollVendor.SelectedItem.Value);
        }

        bool succesfull = employerController.updateEmployerSetup(_employerID, _iei, _iec, _ftpei, _ftpec, _ipi, _ipc, _ftppi, _ftppc, _pc, _billing, _fileUpload, _paySU, _demSU, _gpSU, _hrSU, _vendorID, _ecSU, _ioSU, _icSU, _icPayMod);

        if (succesfull == true)
        {
            if (DdlFilterEmployers.SelectedItem.Text != "View All")
            {
                _employerID = int.Parse(DdlFilterEmployers.SelectedItem.Value);
            }
            else
            {
                _employerID = 0;
            }

            loadEmployers(_employerID);
            LitMessage.Text = "Employer " + litEmployerID.Text + " has been updated.";
            MpeWebMessage.Show();
        }
        else
        {
            LitMessage.Text = "An error occurred while saving the updates. Please try again.";
            MpeWebMessage.Show();
        }
    }

    protected void DdlFilterEmployers_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;

        if (DdlFilterEmployers.SelectedItem.Text != "Select")
        {
            _employerID = int.Parse(DdlFilterEmployers.SelectedItem.Value);
            loadEmployers(_employerID);
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "Please select an EMPLOYER to view.";
        }
    }


    protected void GvEmployers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = (GridViewRow)GvEmployers.Rows[e.RowIndex];

        Literal litEmployerID = null;
        DropDownList ddlAlertTypeID = null;
        bool validData = true;

        litEmployerID = (Literal)row.FindControl("LitEmployerID");
        ddlAlertTypeID = (DropDownList)row.FindControl("DdlAlertType");

        validData = errorChecking.validateDropDownSelection(ddlAlertTypeID, validData);

        if (validData == true)
        {
            int _employerID = int.Parse(litEmployerID.Text);
            int _alertTypeID = int.Parse(ddlAlertTypeID.SelectedItem.Value);
            bool validTransaction = false;

            validTransaction = alert_controller.manufactureEmployerAlert(_employerID, _alertTypeID);

            if (validTransaction == true)
            {
                loadEmployers(_employerID);
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "An error occurred while creating the new Alert.";
            }
        }
    }

    protected void BtnUpdateAverages_Click(object sender, EventArgs e)
    {

        bool validData = true;
        int _employerID = 0;

        validData = errorChecking.validateDropDownSelection(DdlFilterEmployers, validData);

        if (validData == true)
        {
            try
            {
                _employerID = int.Parse(DdlFilterEmployers.SelectedItem.Value);
                
                AverageHoursCalculator calc = new AverageHoursCalculator();
                validData = calc.CalculateAveragesForEmployer(_employerID);                

                if (validData == true)
                {

                    NightlyCalculationFactory.updateNightlyCalculation(_employerID);
                    MpeWebMessage.Show();
                    LitMessage.Text = "The Batch Averages have finished re-calculating.";
                
                }
                else
                {

                    NightlyCalculationFactory.updateFailNightlyCalculation(_employerID);
                    MpeWebMessage.Show();
                    LitMessage.Text = "An ERROR occurred while re-calculating the batch averages, please contact IT if this issue continues.";
                
                }

            }
            catch(Exception exception)
            {

                Log.Warn("Exception During Calculate for employer Id: [" + _employerID + "]", exception);
                
                MpeWebMessage.Show();
                
                LitMessage.Text = "An ERROR occurred while re-calculating the batch averages, please contact IT if this issue continues.";
            }

        }
        else
        {

            MpeWebMessage.Show();
            
            LitMessage.Text = "Please SELECT an EMPLOYER";
        
        }

    }

    protected void Gv_gv_Alerts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView Gv_gv_Alerts = (GridView)GvEmployers.Rows[0].FindControl("Gv_gv_Alerts");

        GridViewRow row = Gv_gv_Alerts.Rows[e.RowIndex];
        string alertid = ((HiddenField)row.FindControl("HiddenAlertId")).Value;
        int _employerID = int.Parse(DdlFilterEmployers.SelectedItem.Value);
        int _alertID = int.Parse(alertid);
        try
        {
            alert_controller.deleteEmployerAlerts(_employerID, _alertID);

            LitMessage.Text = "Employer Alert has been deleted.";
            MpeWebMessage.Show();

            loadEmployers(_employerID);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
    }
}