using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Afas.AfComply.Domain;
using log4net;
using System.Drawing;
using Afas.Domain;

public partial class IRSConfirmation : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(IRSConfirmation));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {

        if (null == employer || false == employer.IrsEnabled)
        {

            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [IRSVerification] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=63", false);

            return;

        }

                LitUserName.Text = user.User_UserName;

        HfDistrictID.Value = user.User_District_ID.ToString();

        LabelLegalName.Text = employer.EMPLOYER_NAME;

        List<User> users = UserController.getDistrictUsers(employer.EMPLOYER_ID);
        List<User> IrsContacts = (from User contact in users where contact.User_IRS_CONTACT == true select contact).ToList();

        gvContacts.DataSource = IrsContacts;
        gvContacts.DataBind();

        btnConfirm.Enabled = (IrsContacts.Count == 1);
        if (btnConfirm.Enabled == false)
        {
            PopupMessage.Text = "You can't proceed until you have ONLY one IRS Contact!";
            MpeWebMessage.Show();
        }

        List<classification> classifications = classificationController.ManufactureEmployerClassificationList(employer.EMPLOYER_ID, true);

        Gv_SafeHarbor.DataSource = classifications;
        Gv_SafeHarbor.DataBind();

        string Message2g = "";
        List<classification> TwoG = classifications.Where(c => c.CLASS_AFFORDABILITY_CODE.ToLower() == "2g").ToList();

        List<PlanYear> planYears = PlanYear_Controller.getEmployerPlanYear(employer.EMPLOYER_ID);
        foreach (PlanYear year in planYears)
        {
            if (year.PLAN_YEAR_START < new DateTime(2016, 12, 31) && year.PLAN_YEAR_END > new DateTime(2016, 1, 1))
            {
                foreach (insurance ins in insuranceController.manufactureInsuranceList(year.PLAN_YEAR_ID))
                {
                    foreach (insuranceContribution contribution in insuranceController.manufactureInsuranceContributionList(ins.INSURANCE_ID))
                    {
                        foreach (classification classy in TwoG)
                        {
                            if (contribution.INS_CONT_CLASSID == classy.CLASS_ID)
                            {
                                decimal price = 0;
                                if (contribution.INS_CONT_CONTRIBUTION_ID.Equals("%"))
                                {
                                    price = (decimal)(1.0 - (contribution.INS_CONT_AMOUNT / 100.0)) * ins.INSURANCE_COST;
                                }
                                else
                                {
                                    price = ins.INSURANCE_COST - (decimal)contribution.INS_CONT_AMOUNT;
                                }

                                if (price > (decimal)95.63)
                                {
                                    foreach (GridViewRow row in Gv_SafeHarbor.Rows)
                                    {
                                        try
                                        {
                                            HiddenField hf = (HiddenField)row.FindControl("HiddenTypeId");
                                            if (int.Parse(hf.Value) == classy.CLASS_ID)
                                            {
                                                row.BackColor = Color.Red;
                                            }
                                        }
                                        catch
                                        {        
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (false == Message2g.IsNullOrEmpty())
        {
            LitMessage.Text = String.Format("<p>Note that you have selected the 2G Federal Poverty Level safe harbor code for one or more plans with an employee-only contribution that exceeds the $95.63 per month contribution limit to qualify under this safe harbor.  Please correct either the contribution amount or the safe harbor code; start <a href=\"/securepages/s_setup.aspx\">here</a> and follow the instructions in the <a href=\"{0}\">instruction guide</a>.</p>", Feature.IrsInstructionsLink);
        }
        else
        {
            LitMessage.Text = "";
        }

        int employerId = int.Parse(HfDistrictID.Value);

        var alerts = alert_controller.manufactureEmployerAlertList(employerId);
        if (alerts.Count > 0)
        {
            txtAlert.Text = "You have alerts that need to be cleared; click <a href=\"/securepages/alerts.aspx\">here</a> to review and process.";
           
        }
        else
        {
            txtAlert.Text = "You do not have any alerts that need to be cleared; you may proceed to the next step.";
        }


        int taxYearId = Feature.PreviousReportingYear;

        var currentEmployerTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, taxYearId);
        if (currentEmployerTransmissionStatus == null)
        {
            initiateEmployerTransaction();
        }

        EditTable(employer.EMPLOYER_ID);
    }


    protected void BtnNewHire_Click(object sender, EventArgs e)
    {
        int EmployeeId = 0;
        int ClassId = 0;
        int AcaStatusId = 0;
        DateTime ModOn = DateTime.Now;
        try
        {
            string ModBy = ((User)Session["CurrentUser"]).User_UserName;
            int EmployerId = int.Parse(HfDistrictID.Value);

            foreach (GridViewRow row in GridView_NewHires.Rows)
            {
                DropDownList ddl_gv_AcaType = (DropDownList)row.FindControl("Ddl_gv_AcaType");

                if (int.TryParse(ddl_gv_AcaType.SelectedValue, out AcaStatusId) && AcaStatusId != 0)
                {
                    HiddenField gv_employee_Id = (HiddenField)row.FindControl("Hf_gv_id");
                    EmployeeId = int.Parse(gv_employee_Id.Value);

                    HiddenField gv_class_Id = (HiddenField)row.FindControl("Hf_gv_class_id");
                    ClassId = int.Parse(gv_class_Id.Value);

                    if (false == EmployeeController.UpdateEmployeeClassAcaStatus(EmployerId, EmployeeId, ClassId, AcaStatusId, ModBy, ModOn))
                    {
                        LitMessage.Text += "Update Failed for: Id [" + EmployeeId + "] ";
                    }
                }
            }

            EditTable(EmployerId);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);

            LitMessage.Text = "Save Failed with error : " + exception.Message;
        }
    }

    private void EditTable(int employerId)
    {

        List<Employee> employees = EmployeeController.manufactureEmployeeList(employerId);

        DateTime ReportingEnd = new DateTime(2016, 12, 31);

        const int COBRA = 4;
        const int FULLTIME = 5;
        const int RETIRED = 8;

        IList<Employee> nonRetiredNonCobraNonFulltimeEmployees = (
                                                                  from Employee emp in employees
                                                                  where
                                                                      emp.EMPLOYEE_IMP_END >= ReportingEnd
                                                                        &&
                                                                      emp.EMPLOYEE_ACT_STATUS_ID != COBRA
                                                                        &&
                                                                      emp.EMPLOYEE_ACT_STATUS_ID != RETIRED
                                                                        &&
                                                                      emp.EMPLOYEE_ACT_STATUS_ID != FULLTIME
                                                                  select emp
                                                                 ).ToList();

        GridView_NewHires.DataSource = nonRetiredNonCobraNonFulltimeEmployees;
        GridView_NewHires.DataBind();

        List<classification_aca> AcaStatus = classificationController.getACAstatusList();

        try
        {
            foreach (GridViewRow row in GridView_NewHires.Rows)
            {
                DropDownList ddl_gv_AcaType = (DropDownList)row.FindControl("Ddl_gv_AcaType");
                HiddenField hf_gv_AcaTypeId = (HiddenField)row.FindControl("Hf_gv_AcaTypeId");
                int AcaTypeId = int.Parse(hf_gv_AcaTypeId.Value);

                ddl_gv_AcaType.DataSource = AcaStatus;
                ddl_gv_AcaType.DataTextField = "ACA_STATUS_NAME";
                ddl_gv_AcaType.DataValueField = "ACA_STATUS_ID";
                ddl_gv_AcaType.DataBind();
                ddl_gv_AcaType.Items.Add("Select");

                errorChecking.setDropDownList(ddl_gv_AcaType, AcaTypeId);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
    }

    private void initiateEmployerTransaction()
    {

        User user = ((User)Session["CurrentUser"]);
        int employerId = int.Parse(HfDistrictID.Value);
        var newEmployerTaxYearTransmission = new EmployerTaxYearTransmission();
        newEmployerTaxYearTransmission.EmployerId = employerId;
        newEmployerTaxYearTransmission.TaxYearId = Feature.PreviousReportingYear;
        newEmployerTaxYearTransmission.EntityStatusId = 1;
        newEmployerTaxYearTransmission.CreatedBy = user.User_Full_Name;
        newEmployerTaxYearTransmission.ModifiedBy = user.User_Full_Name;
        newEmployerTaxYearTransmission = employerController.insertUpdateEmployerTaxYearTransmission(newEmployerTaxYearTransmission);

        if (ValidationHelper.validateNewEmployerTaxYearTransmission(newEmployerTaxYearTransmission, Log))
        {

            var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                newEmployerTaxYearTransmission.EmployerTaxYearTransmissionId,
                TransmissionStatusEnum.Initiated,
                user.User_UserName
            );

            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
            ValidationHelper.validateNewEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus, Log);

        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        int EmployerId = int.Parse(HfDistrictID.Value);
        var TaxYearId = Feature.PreviousReportingYear;
        EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, TaxYearId);
        if (currentEmployerTaxYearTransmissionStatus != null && currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.Initiated
            && currentEmployerTaxYearTransmissionStatus.EndDate == null)
        {
            User user = ((User)Session["CurrentUser"]);
            currentEmployerTaxYearTransmissionStatus = employerController.endEmployerTaxYearTransmissionStatus(currentEmployerTaxYearTransmissionStatus, user.User_UserName);
        }

        Response.Redirect("/Reporting/Verification", false);

    }

}