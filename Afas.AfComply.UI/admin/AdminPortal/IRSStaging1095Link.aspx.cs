using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class IRSStaging1095Link : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(IRSStaging1095Link));

        private int EmployerId
        {
            get
            {
                int employerId = 0;

                //check that data is correct
                if (
                        null == DdlEmployer.SelectedItem
                            ||
                        null == DdlEmployer.SelectedItem.Value
                            ||
                        false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                    )
                {

                    lblMsg.Text = "Incorrect parameters";

                }

                return employerId;
            }
        }

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the IRSTransmit page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=32", false);
            }
            else
            {
                loadEmployers();
            }

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

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (EmployerId == 0) return;

            employer employ = employerController.getEmployer(EmployerId);

            cofein.Text = employ.EMPLOYER_EIN;

            chkConfirm.Checked = false;

        }

        protected void BtnEnableIRS_Click(object sender, EventArgs e)
        {

            if (EmployerId == 0) return;

            if (chkConfirm.Checked)
            {

                PIILogger.LogPII("Enabling the IRS 1095 review links for EmployerId:" + EmployerId);
                EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, Feature.CurrentReportingYear);

                if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.F1094_Collected)
                {

                    User user = ((User)Session["CurrentUser"]);

                    var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                        currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                        TransmissionStatusEnum.ETL,
                        user.User_UserName,
                        DateTime.Now
                    );

                    employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

                }
                else
                {
                    Log.Warn(String.Format("EmployerId {0}, TransmissionStatusId {1}, should be {2}", EmployerId, currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId, TransmissionStatusEnum.F1094_Collected));
                }

            }
            else
            {
                lblMsg.Text = "You must certify that you are authorized to transmit this employer.";
            }

        }

        protected void BtnEnable1095_Click(object sender, EventArgs e)
        {
            if (EmployerId == 0) return;

            if (chkConfirm.Checked)
            {

                PIILogger.LogPII("Enabling the IRS 1095 review links for EmployerId:" + EmployerId);
                EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, 2016);

                if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.ETL)
                {

                    User user = ((User)Session["CurrentUser"]);

                    this.Log.Warn("Start Opening 'Status and Action Portal Step 3' for Employer Id [" + EmployerId + "] by Admin [" + user.User_UserName + "]");

                    var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                        currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                        TransmissionStatusEnum.StepThree,
                        user.User_UserName,
                        DateTime.Now
                    );

                    employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);


                    //If Step 3 Email is enabled
                    if (true == Feature.EnableEmail1095Step3Open)
                    {

                        // Send an Email notification to All (active) Users on the Employer.
                        List<User> AllUsers = UserController.getDistrictUsers(EmployerId);

                        // Filter out Users that (probably) shouldn't get the email
                        List<User> FilteredUsers = AllUsers.Where((emailTo) =>
                                                                    emailTo.USER_ACTIVE == true
                                                                    && emailTo.User_Floater == false
                                                                    && emailTo.User_PWD_RESET == false
                                                                    && emailTo.User_Email.IsValidEmail() == true).ToList();

                        // Including the User/Admin clicking the button
                        FilteredUsers.Add(user);

                        // Build the Subject line
                        String SubjectLine = "1095C Review Opened in Status and Action Portal";

                        // Octo Variable Message over-rides (because, the message will probably change)
                        if (false == Feature.EmailTitle1095Step3Open.IsNullOrEmpty())
                        {
                            SubjectLine = Feature.EmailTitle1095Step3Open;
                        }

                        // Build the Email Body
                        String EmailMessage = "Thank you for completing Steps 1 and 2 in your " +
                            Branding.ProductName +
                            " Status & Action Portal.Your data has been loaded, and it’s time for you to certify the information that will be printed on your 1095 forms for the " +
                            Feature.CurrentReportingYear + " tax year.";

                        // Octo Variable Message over-rides (because, the message will probably change)
                        if (false == Feature.EmailBody1095Step3Open.IsNullOrEmpty())
                        {
                            EmailMessage = Feature.EmailBody1095Step3Open;
                        }

                        // actually send the email
                        Email email = new Email();
                        email.sendEmail(FilteredUsers, SubjectLine, EmailMessage, true);

                        // Log that we sent the email.
                        this.Log.Warn("Email Sent About Status and Action Portal Step 3 opened for Employer Id [" + EmployerId + "] by Admin [" + user.User_UserName + "], sent email to [" + (FilteredUsers.Count()) + "] Email Addresses (including the Admin), with addresses: [" + string.Join(", ", FilteredUsers.Select(us => us.User_Email)) + "]");

                        // Done
                    }

                }
                else
                {
                    Log.Warn(String.Format("EmployerId {0}, TransmissionStatusId {1}, should be {2}", EmployerId, currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId, TransmissionStatusEnum.F1094_Collected));
                }

            }
            else
            {
                lblMsg.Text = "You must certify that you are authorized to transmit this employer.";
            }
        }
        protected void BtnEnable1094_Click(object sender, EventArgs e)
        {
            if (EmployerId == 0) return;

            if (chkConfirm.Checked)
            {

                PIILogger.LogPII("Enabling the IRS 1095 review links for EmployerId:" + EmployerId);
                EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, 2016);

                if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.StepThree)
                {

                    User user = ((User)Session["CurrentUser"]);

                    var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                        currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                        TransmissionStatusEnum.StepFive,
                        user.User_UserName,
                        DateTime.Now
                    );

                    employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

                }
                else
                {
                    Log.Warn(String.Format("EmployerId {0}, TransmissionStatusId {1}, should be {2}", EmployerId, currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId, TransmissionStatusEnum.F1094_Collected));
                }

            }
            else
            {
                lblMsg.Text = "You must certify that you are authorized to transmit this employer.";
            }
        }
        protected void BtnEnableTransmit_Click(object sender, EventArgs e)
        {
            if (EmployerId == 0) return;

            if (chkConfirm.Checked)
            {

                PIILogger.LogPII("Enabling the IRS 1095 review links for EmployerId:" + EmployerId);
                EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, 2016);

                if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.StepFive)
                {

                    User user = ((User)Session["CurrentUser"]);

                    var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                        currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                        TransmissionStatusEnum.Transmit,
                        user.User_UserName,
                        DateTime.Now
                    );

                    employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

                }
                else
                {
                    Log.Warn(String.Format("EmployerId {0}, TransmissionStatusId {1}, should be {2}", EmployerId, currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId, TransmissionStatusEnum.F1094_Collected));
                }

            }
            else
            {
                lblMsg.Text = "You must certify that you are authorized to transmit this employer.";
            }
        }

        protected void BtnTransmit_Click(object sender, EventArgs e)
        {

            if (EmployerId == 0) return;

            if (chkConfirm.Checked)
            {

                PIILogger.LogPII("Enabling the IRS 1095 review links for EmployerId:" + EmployerId);
                EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, 2016);

                if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.F1094_Collected)
                {

                    User user = ( (User) Session["CurrentUser"]);

                    var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                        currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                        TransmissionStatusEnum.ETL,
                        user.User_UserName,
                        DateTime.Now
                    );

                    employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

                }
                else
                {
                    Log.Warn(String.Format("EmployerId {0}, TransmissionStatusId {1}, should be {2}", EmployerId, currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId, TransmissionStatusEnum.F1094_Collected));
                }

            }
            else
            {
                lblMsg.Text = "You must certify that you are authorized to transmit this employer.";
            }

        }

    }

}