using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using Afas.Domain;
using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class ToggleIRS : AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.NewAdminPanelEnabled)
            {

                this.Log.Info("A user tried to access the ToggleIRS page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=43", false);
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

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            cofein.Text = employ.EMPLOYER_EIN;

            IrsEnabled.Checked = employ.IrsEnabled;

        }

        protected void BtnRun_Click(object sender, EventArgs e)
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

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            if (employ != null)
            {
                employerController.updateEmployer_IrsEnabled(employerId, IrsEnabled.Checked);
                
            
            }
        }
        protected void BtnStepThree_Click(object sender, EventArgs e)
        {
            int employerId = 0;
            int taxYearId;

            taxYearId = Feature.CurrentReportingYear;
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

                return;

            }

            User user = ((User)Session["CurrentUser"]);

            employer employ = employerController.getEmployer(employerId);

            if (employ != null)
            {

                this.Log.Warn("Start Opening 'Status and Action Portal Step 3' for Employer Id [" + employerId + "] by Admin [" + user.User_UserName + "]");
                
                employerController.updateEmployer_Step(employerId, taxYearId, 22);

                lblMsg.Text = "Step three - View 1095C and 1095 PDF is enable for " + employ.EMPLOYER_NAME;

                //If Step 3 Email is enabled
                if (true == Feature.EnableEmail1095Step3Open)
                {

                    // Send an Email notification to All (active) Users on the Employer.
                    List<User> AllUsers = UserController.getDistrictUsers(employerId);

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
                    this.Log.Warn("Email Sent About Status and Action Portal Step 3 opened for Employer Id [" + employerId + "] by Admin [" + user.User_UserName + "], sent email to [" + (FilteredUsers.Count()) + "] Email Addresses (including the Admin), with addresses: [" + string.Join(", ", FilteredUsers.Select(us => us.User_Email)) + "]");

                    // Done
                }

            }
        }

        protected void BtnStepFive_Click(object sender, EventArgs e)
        {
            int employerId = 0;
            int taxYearId;

            taxYearId = Feature.CurrentReportingYear;
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

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            if (employ != null)
            {
                employerController.updateEmployer_Step(employerId, taxYearId, 23);

                lblMsg.Text = "Step five - View 1094 enable for " + employ.EMPLOYER_NAME;
            }
        }

        protected void BtnTransmit_Click(object sender, EventArgs e)
        {
            int employerId = 0;
            int taxYearId;

            taxYearId = Feature.CurrentReportingYear;
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

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            if (employ != null)
            {
                employerController.updateEmployer_Step(employerId, taxYearId, 11);

                lblMsg.Text = "Step transmit - Transmit is enable for " + employ.EMPLOYER_NAME;

            }
        }
        private ILog Log = LogManager.GetLogger(typeof(ToggleIRS));
    
    }

}