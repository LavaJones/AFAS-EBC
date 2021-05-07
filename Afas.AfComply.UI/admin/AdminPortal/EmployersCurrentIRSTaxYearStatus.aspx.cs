using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployersCurrentIRSTaxYearStatus : Afas.AfComply.UI.admin.AdminPageBase
    {

        private int EmployerId
        {
            get
            {
                int employerId = 0;
                int.TryParse(DdlFilterEmployers.SelectedItem.Value, out employerId);
                return employerId;
            }
        }

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            lblMsg.Text = String.Empty;
            var employers = employerController.getAllEmployers();
            CASSPrintFileGenerator.PopulateEmployersDropDownList(DdlFilterEmployers, employers);
        }

        protected void DdlFilterEmployers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var employerIrsStatuses = employerController.getIRSStatus(EmployerId, DateTime.Now.Year);
            gEmployerIRSStatuses.DataSource = employerIrsStatuses;
            gEmployerIRSStatuses.DataBind();

            if (employerIrsStatuses.Count > 0)
            {
                lblMsg.Text = String.Empty;
            }
            else
            {
                lblMsg.Text = CASSPrintFileGenerator.NoRecordsFoundErrorMessage;
            }

            
        }

    }
}