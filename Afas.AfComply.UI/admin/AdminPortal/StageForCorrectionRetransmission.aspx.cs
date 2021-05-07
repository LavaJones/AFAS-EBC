using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class StageForCorrectionRetransmission : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(StageForCorrectionRetransmission));

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

        private int TaxYearId
        {
            get
            {
                return int.Parse(DdlCalendarYear.SelectedItem.Value);
            }
        }

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            Server.ScriptTimeout = 1800;

            if (false == Feature.NewAdminPanelEnabled)
            {

                Log.Info("A user tried to access the IRSTransmit page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=42", false);

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

        //protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        protected void BtnReTransmit_Click(object sender, EventArgs e)
        {
            User User = (User)Session["CurrentUser"];
            EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EmployerId, TaxYearId);
            if (currentEmployerTaxYearTransmissionStatus != null)
            {
                if (employerController.stageTaxYear1095cCorrection(EmployerId, TaxYearId, User.User_UserName))
                {
                    employerController.endEmployerTaxYearTransmissionStatus(currentEmployerTaxYearTransmissionStatus, User.User_UserName);
                    var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                           currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                           TransmissionStatusEnum.ReTransmit,
                           User.User_UserName,
                           DateTime.Now
                       );

                    lblMsg.Text = "Sucessful";
                    lblMsg.BackColor = System.Drawing.Color.Green;
                    lblMsg.ForeColor = System.Drawing.Color.White;
                    return;
                }
            }
            lblMsg.Text = "Failed";
            lblMsg.BackColor = System.Drawing.Color.Red;
            lblMsg.ForeColor = System.Drawing.Color.Black;
        }

    }
}