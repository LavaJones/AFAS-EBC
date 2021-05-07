using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;

using log4net;

public partial class complete : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(complete));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        if (null == employer || false == employer.IrsEnabled)
        {
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [complete] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=47", false);

            return;
        }

                HfUserName.Value = user.User_UserName;
        HfDistrictID.Value = user.User_District_ID.ToString();

        loadData();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    private void loadData()
    {
        tax_year_submission tys = null;

        if (Session["irs"] != null)
        {
            tys = (tax_year_submission)Session["irs"];
        }


        if (tys != null)
        {
            errorChecking.setDropDownList(Ddl_step1, tys.IRS_DGE);
            errorChecking.setDropDownList(Ddl_step2, tys.IRS_ALE);
            errorChecking.setDropDownList(Ddl_step5, tys.IRS_UNPAID_LEAVE);
            errorChecking.setDropDownList(Ddl_step6, tys.IRS_ASH);

            errorChecking.validateDropDownSelection(Ddl_step1, false);
            errorChecking.validateDropDownSelection(Ddl_step2, false);
            errorChecking.validateDropDownSelection(Ddl_step5, false);
            errorChecking.validateDropDownSelection(Ddl_step6, false);

            if (tys.IRS_UNPAID_LEAVE != null && tys.IRS_ALE != null&& tys.IRS_TR != null && tys.IRS_TOBACCO != null && tys.IRS_UNPAID_LEAVE != null && tys.IRS_ASH != null)
            {
                Btn_Next.Enabled = true;
                Btn_Next.BackColor = System.Drawing.Color.Red;
            }
            else if (tys.IRS_UNPAID_LEAVE == false)
            {
                Btn_Next.Enabled = true;
                Btn_Next.BackColor = System.Drawing.Color.Red;
            }
        }
        else
        {
            Response.Redirect("step1.aspx", false);
        }
    }


    protected void Btn_Next_Click(object sender, EventArgs e)
    {
         tax_year_submission tys = null;
         string _completedBy = HfUserName.Value;
         DateTime? _completedOn = DateTime.Now;

        if (Session["irs"] != null)
        {
            tys = (tax_year_submission)Session["irs"];
            tys.IRS_COMPLETED_BY = _completedBy;
            tys.IRS_COMPLETED_ON = _completedOn;
            bool validSave =  employerController.updateInsertIrsSubmissionApproval(tys);
            if (validSave == true && initiateEmployerTransaction())
            {
                LblMessage.Text = "The data has been saved. Click <a href=\"/Reporting/Verification\">here</a> to return to the Status & Action Portal";
                MpeWebMessage.Show();
            }
        }
    }

    private Boolean initiateEmployerTransaction()
    {

        User user = ((User)Session["CurrentUser"]);
        int employerId = int.Parse(HfDistrictID.Value);
        int taxYearId = Feature.CurrentReportingYear;

        var currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, taxYearId);

        {

            var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
               currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
               TransmissionStatusEnum.F1094_Collected,
               user.User_UserName
           );

            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

        }

        currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, taxYearId);

        if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.F1094_Collected)
        {

            currentEmployerTaxYearTransmissionStatus = employerController.endEmployerTaxYearTransmissionStatus(currentEmployerTaxYearTransmissionStatus, user.User_UserName);

            var email_to = ConfigurationManager.AppSettings["System.EmailNotificationAddress"].ToString();

            var employer = employerController.getEmployer(employerId);
            Email email = new Email();
            var body = string.Format("Employer \nName: {0}\n EIN: {1}\n Transmission Status: {2}\n <a href=\"/irs_submission/complete.aspx.\"></a>",
                employer.EMPLOYER_NAME, employer.EMPLOYER_EIN, currentEmployerTaxYearTransmissionStatus.TransmissionStatusId);
            email.SendEmail(SystemSettings.IrsProcessingAddress, Feature.IrsStatusEmailSubject, body, false);
        
        }
        else
        {
            Log.Warn(string.Format("EmployerId {0}, TransmissionStatusId {1}, should be {2}", employerId, currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId, TransmissionStatusEnum.F1094_Collected));
        }

        return true;

    }
    protected void Btn_Previous_Click(object sender, EventArgs e)
    {
        Response.Redirect("step6.aspx", false);
    }

}