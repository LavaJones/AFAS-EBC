using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class DisableReview : AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.NewAdminPanelEnabled)
            {

                this.Log.Info("A user tried to access the ConfirmNewHires page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=11", false);
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

                lblMsg.Text = "Please select an employer and confirm the action.";

                return;

            }

            try
            {

                lblMsg.Text = "";

                DateTime modifedOn = DateTime.Now;

                String modifedBy = ((User)Session["CurrentUser"]).User_UserName;

                employer employ = employerController.getEmployer(employerId);

                var currentEmployerTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId
                    (employerId, int.Parse(DdlCalendarYear.SelectedValue));

                if (currentEmployerTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.Review || currentEmployerTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.ETL)
                {
                    currentEmployerTransmissionStatus.EntityStatusId = 2;
                    employerController.insertUpdateEmployerTaxYearTransmissionStatus(currentEmployerTransmissionStatus);


                    currentEmployerTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId
                        (employerId, int.Parse(DdlCalendarYear.SelectedValue));

                    if (currentEmployerTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.ETL)
                    {
                        currentEmployerTransmissionStatus.EntityStatusId = 2;
                        employerController.insertUpdateEmployerTaxYearTransmissionStatus(currentEmployerTransmissionStatus);
                    }
                }                

                lblMsg.Text = "Disable Complete!";

            }
            catch (Exception exception)
            {
                
                this.Log.Fatal("Errors during disable!", exception);
                
                lblMsg.Text = exception.Message;

            }

        }

        private ILog Log = LogManager.GetLogger(typeof(DisableReview));
    
    }

}