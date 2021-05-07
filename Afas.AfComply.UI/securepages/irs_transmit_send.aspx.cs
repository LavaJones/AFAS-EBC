using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class irs_transmit_send : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(IRSConfirmation));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        if (null == employer || false == employer.IrsEnabled)
        {
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [IRSVerification] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=62", false);

            return;
        }

                LitUserName.Text = user.User_UserName;

        HfDistrictID.Value = user.User_District_ID.ToString();


    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (chbVerify.Checked)
        {
            User User = ((User)Session["CurrentUser"]);

            int employerId = int.Parse(HfDistrictID.Value);
            employer Employer = employerController.getEmployer(employerId);

            EmployerTaxYearTransmissionStatus currentEployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(Employer.EMPLOYER_ID, 2016);
            employerController.endEmployerTaxYearTransmissionStatus(currentEployerTaxYearTransmissionStatus, User.User_UserName);
            if (currentEployerTaxYearTransmissionStatus != null)
            {
                EmployerTaxYearTransmissionStatus transmissionStatusBeforeHalt = employerController.getCurrentEmployerTaxYearTransmissionStatusBeforeHalt(Employer.EMPLOYER_ID, 2016);

                var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                         currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                         transmissionStatusBeforeHalt.TransmissionStatusId,
                         User.User_UserName,
                         DateTime.Now
                     );

                newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
            }

            Response.Redirect("~/Reporting/Verification", false);

        }
    }


}