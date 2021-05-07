using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployersCurrentTransmissionTaxYearStatus : Afas.AfComply.UI.admin.AdminPageBase
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

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            var taxYears = employerController.getTaxYears();
            CASSPrintFileGenerator.PopulateTaxYearDropDownList(DdlTaxYear, taxYears);
        }

        protected void DdlTaxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<EmployersCurrentTaxYearTransmissionStatus> employerTransmissionStatuses = employerController.getEmployersCurrentTaxYearTransmissionStatus(TaxYearId);
            List<employer> employers = employerController.getAllEmployers();
            //Let it be noted that this is not how I wanted to fix this, but the code made a more elegant solution infeaseible

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("name");
            dataTable.Columns.Add("Ein");
            dataTable.Columns.Add("TransmissionStatusId");
            dataTable.Columns.Add("StartDate", typeof(DateTime));
            dataTable.Columns.Add("EndDate", typeof(DateTime));

            foreach (EmployersCurrentTaxYearTransmissionStatus ectyts in employerTransmissionStatuses)
            {
                DataRow row = dataTable.NewRow();
                row["name"] = ectyts.name;
                row["TransmissionStatusId"] = ectyts.TransmissionStatusId.ToString();
                if (ectyts.StartDate != null)
                    row["StartDate"] = ectyts.StartDate;
                if (ectyts.EndDate != null)
                    row["EndDate"] = ectyts.EndDate;

                employer employer = (from employer emp in employers where emp.EMPLOYER_ID == ectyts.employer_id select emp).FirstOrDefault();
                if(employer != null)
                {
                    row["Ein"] = employer.EMPLOYER_EIN;
                }

                dataTable.Rows.Add(row);
            }
            
            gEmployerTransmissionStatuses.DataSource = dataTable;
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
    }
}