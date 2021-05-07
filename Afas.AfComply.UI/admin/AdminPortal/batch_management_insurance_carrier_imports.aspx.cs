using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace Afas.AfComply.UI.admin
{
    public partial class batch_management_insurance_carrier_imports : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(admin_batch_management));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.BulkImportEnabled)
            {
                Log.Info("A user tried to access the Bulk EmployerMeasurementPeriod page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=6", false);
            }
            else
            {
                loadEmployers();
            }
        }

        /// <summary>
        /// This will filter and load the employer drop down list. 
        /// </summary>
        private void loadEmployers()
        {
            string searchText = null;
            bool validData = true;
            List<employer> currEmployers = employerController.getAllEmployers();
            List<employer> filteredEmployers = new List<employer>();

            validData = errorChecking.validateTextBoxNull(TxtEmployerSearch, validData);
            TxtEmployerSearch.BackColor = System.Drawing.Color.White;


            //This section can be replaced with the function that is built within AfComply. 
            if (validData == true)
            {
                searchText = TxtEmployerSearch.Text.ToLower();
                filteredEmployers = employerController.FilterEmployerBySearch(searchText, currEmployers);
            }
            else { filteredEmployers = currEmployers; }

            DdlEmployer.DataSource = filteredEmployers;
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            DdlEmployer.Items.Add("Select Employer");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        }

        protected void BtnSearchEmployer_Click(object sender, EventArgs e)
        {
            loadEmployers();
        }

        private void loadBatchData(int _employerID)
        {
            List<batch> batchList = employerController.manufactureBatchListInsuranceCarrierImport(_employerID);
            GvBatchFiles.DataSource = batchList;
            GvBatchFiles.DataBind();
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _employerID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

            if (validData == true)
            {
                _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                loadBatchData(_employerID);
            }
            else
            {


            }
        }

        protected void GvBatchFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GvBatchFiles.Rows[e.RowIndex];
            Label lblBatchID = (Label)row.FindControl("LblBatchID");
            Literal litun = (Literal)this.Master.FindControl("LitUserName");
            string _modBy = litun.Text;
            int _batchID = int.Parse(lblBatchID.Text);
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            //Delete the batch id's. Batch ID's don't have 
            try
            {
                insuranceController.deleteImportedInsuranceCarrierBatch(_batchID, _modBy, _employerID);

                LitMessage.Text = "All files related to Batch ID " + _batchID.ToString() + " have been deleted.";
                MpeWebMessage.Show();
                loadBatchData(_employerID);
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
            }
        }
    }
}