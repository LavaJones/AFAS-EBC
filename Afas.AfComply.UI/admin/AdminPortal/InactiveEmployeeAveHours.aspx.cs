using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class InactiveEmployeeAveHours : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(RunCalculations));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Response.Redirect("~/default.aspx?error=27", false);
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
            lblMsg.Text = string.Empty;            
        }

        protected void BtnRun_Click(object sender, EventArgs e)
        {
            User User = (User)Session["CurrentUser"];
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

            if(employerController.UpdateEmployeeMeasurementAverageHoursEntityStatus(employerId, User.User_UserName))
            {
                lblMsg.Text = "Updated";
            }
            else
            {
                lblMsg.Text = "Failed to update";
            }
                       
        }
    }
}