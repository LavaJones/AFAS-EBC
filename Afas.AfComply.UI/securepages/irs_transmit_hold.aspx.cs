using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class irs_transmit_hold : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(IRSConfirmation));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        if (null == employer || false == employer.IrsEnabled)
        {
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [IRSVerification] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=61", false);

            return;
        }

                LitUserName.Text = user.User_UserName;

        HfDistrictID.Value = user.User_District_ID.ToString();

    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (chbVerify.Checked)
        {
            User User = ((User)Session["CurrentUser"]);

            int employerId = int.Parse(HfDistrictID.Value);
            employer Employer = employerController.getEmployer(employerId);

            var message = string.Format("Employer {0} with EIN {1} has the following issue \r\n\r\n{2}\r\n\r\n", Employer.EMPLOYER_NAME, Employer.EMPLOYER_EIN,tbIssues.Text);
            message += string.Format("Emailed by:\r\n{0}\r\n{1}\r\n{2}\r\n", User.User_Full_Name, User.User_Email, User.User_Phone);

            Email em = new Email();
            em.SendEmail(SystemSettings.EmailNotificationAddress, "Issue Reported Prior to IRS Transmission", message, false);

            EmployerTaxYearTransmissionStatus currentEployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(Employer.EMPLOYER_ID, Feature.CurrentReportingYear);
            if (currentEployerTaxYearTransmissionStatus != null)
            {
                var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                         currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                         TransmissionStatusEnum.Halt,
                         User.User_UserName
                     );

                newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
            }

            Response.Redirect("~/Reporting/Verification", false);
        }
      
    }
    
}