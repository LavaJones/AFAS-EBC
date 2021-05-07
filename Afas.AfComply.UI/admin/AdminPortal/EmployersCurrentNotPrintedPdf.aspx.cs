using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployersCurrentNotPrintedPdf : Afas.AfComply.UI.admin.AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(EmployersCurrentNotPrintedPdf));

        private int TaxYearId
        {
            get
            {
                int taxYearId = 0;
                int.TryParse(DdlTaxYear.SelectedValue, out taxYearId);
                return taxYearId;
            }

        }

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            var taxYears = employerController.getTaxYears();
            CASSPrintFileGenerator.PopulateTaxYearDropDownList(DdlTaxYear, taxYears);
        }

        protected void DdlTaxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridEmployerPdfCount.DataSource = employerController.GetNotPrintedCountPerEmployer(TaxYearId);
            GridEmployerPdfCount.DataBind();
        }
    }
}