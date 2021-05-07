using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployersCurrentPdfStatus : Afas.AfComply.UI.admin.AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(EmployersCurrentPdfStatus));

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
            GridEmployerPdfCount.DataSource = employerController.GetPrintedCountPerEmployer(TaxYearId);
            GridEmployerPdfCount.DataBind();
        }
    }
}