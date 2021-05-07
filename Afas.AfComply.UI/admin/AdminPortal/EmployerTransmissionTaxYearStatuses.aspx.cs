using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployerTransmissionTaxYearStatuses : Afas.AfComply.UI.admin.AdminPageBase
    {

        private int TaxYearId
        {
            get
            {
                int taxYearId = 0;
                int.TryParse(DdlTaxYear.SelectedValue, out taxYearId);
                return taxYearId;
            }

        }

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
            var employers = employerController.getAllEmployers();
            CASSPrintFileGenerator.PopulateEmployersDropDownList(DdlFilterEmployers, employers);
        }

        protected void DdlFilterEmployers_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (CASSPrintFileGenerator.ValidateDdlFilterEmployersSelectedItem(DdlFilterEmployers, DdlTaxYear, MpeWebMessage, LitMessage))
            {
                loadTaxYears();
            }
        }

        private void loadTaxYears()
        {
            var taxYears = employerController.getTaxYears();
            CASSPrintFileGenerator.PopulateTaxYearDropDownList(DdlTaxYear, taxYears);
        }

        protected void DdlTaxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            var employerTransmissionStatuses = employerController.getEmployerTaxYearTransmissionStatusesByEmployerIdAndTaxYearId(EmployerId, TaxYearId);
            gEmployerTransmissionStatuses.DataSource = employerTransmissionStatuses;
            gEmployerTransmissionStatuses.DataBind();

            if (employerTransmissionStatuses.Count > 0)
            {
                lblMsg.Text = "";
            }
            else
            {
                lblMsg.Text = CASSPrintFileGenerator.NoRecordsFoundErrorMessage;
            }
            
        }

        private ILog Log = LogManager.GetLogger(typeof(EmployerTransmissionTaxYearStatuses));

    }
}