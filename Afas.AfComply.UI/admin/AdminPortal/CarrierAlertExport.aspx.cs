using Afas.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class CarrierAlertExport : AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.NewAdminPanelEnabled)
            {

                this.Log.Info("A user tried to access the CarrierAlertExport page which is disabled in the web config.");

                this.Response.Redirect("~/default.aspx?error=7", false);
            }
            else
            {
                this.loadEmployers();
                this.LoadTaxYearDropDownList();
            }

        }

        private void loadEmployers()
        {

            this.DdlEmployer.DataSource = employerController.getAllEmployers();
            this.DdlEmployer.DataTextField = "EMPLOYER_NAME";
            this.DdlEmployer.DataValueField = "EMPLOYER_ID";
            this.DdlEmployer.DataBind();

            this.DdlEmployer.Items.Add("Select");
            this.DdlEmployer.SelectedIndex = this.DdlEmployer.Items.Count - 1;

        }

        public void LoadTaxYearDropDownList()
        {

            List<int> taxYears = employerController.getTaxYears();
            this.DdlTaxYear.DataSource = taxYears;
            this.DdlTaxYear.DataBind();

            this.DdlTaxYear.Items.Add("Select");
            this.DdlTaxYear.SelectedIndex = this.DdlTaxYear.Items.Count - 1;

        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {

            int employerId = 0;

            //check that data is correct
            if (
                    null == this.DdlEmployer.SelectedItem
                        ||
                    null == this.DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(this.DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                this.lblMsg.Text = "Incorrect parameters";

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            this.cofein.Text = employ.EMPLOYER_EIN;
        }

        protected void BtnClearCarrierAlerts_Click(object sender, EventArgs e)
        {
            if (this.chkConfirm.Checked == false)
            {
                this.lblMsg.Text = "Please confirm the deletion of the Carrier Alerts.";

                return;
            }

            int employerId = 0;
            int taxYear = 0;

            //check that data is correct
            if (
                    null == this.DdlEmployer.SelectedItem
                        ||
                    null == this.DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(this.DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                this.lblMsg.Text = "Please select an employer and confirm the action.";

                return;

            }

            //check that data is correct
            if (
                    null == this.DdlTaxYear.SelectedItem
                        ||
                    null == this.DdlTaxYear.SelectedItem.Value
                        ||
                    false == int.TryParse(this.DdlTaxYear.SelectedItem.Value, out taxYear)
                )
            {

                this.lblMsg.Text = "Please select a Tax Year and confirm the action.";

                return;

            }

            try
            {
                this.lblMsg.Text = "";

                User currUser = (User)this.Session["CurrentUser"];

                // get all the alerts
                List<insurance_coverage_I> alerts = insuranceController.manufactureInsuranceCoverageAlerts(employerId);

                // filter to just one tax year
                alerts = (from insurance_coverage_I coverage in alerts where coverage.IC_TAX_YEAR.Equals(taxYear) select coverage).ToList();

                bool FailedDeletes = false;

                foreach (insurance_coverage_I alert in alerts)
                {
                    if (false == insuranceController.deleteInsuranceCoverageAlert(alert.ROW_ID, currUser.User_UserName, DateTime.Now))
                    {
                        FailedDeletes = true;
                    }

                }

                if (FailedDeletes)
                {
                    this.Log.Warn("Some alerts were not deleted!");

                    this.lblMsg.Text = "Some alerts were not deleted!";
                }
                else
                {
                    this.lblMsg.Text = "All alerts cleared.";
                }

            }
            catch (Exception exception)
            {

                this.Log.Fatal("Errors during export carrier alerts!", exception);

                this.lblMsg.Text = exception.Message;

            }

        }

        protected void BtnRun_Click(object sender, EventArgs e)
        {

            int employerId = 0;
            int taxYear = 0;

            //check that data is correct
            if (
                    null == this.DdlEmployer.SelectedItem
                        ||
                    null == this.DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(this.DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                this.lblMsg.Text = "Please select an employer and confirm the action.";

                return;

            }

            //check that data is correct
            if (
                    null == this.DdlTaxYear.SelectedItem
                        ||
                    null == this.DdlTaxYear.SelectedItem.Value
                        ||
                    false == int.TryParse(this.DdlTaxYear.SelectedItem.Value, out taxYear)
                )
            {

                this.lblMsg.Text = "Please select a Tax Year and confirm the action.";

                return;

            }

            try
            {
                this.lblMsg.Text = "";

                // get the employer by the ID
                employer Employer = employerController.getEmployer(employerId);

                // get all the alerts
                List<insurance_coverage_I> alerts = insuranceController.manufactureInsuranceCoverageAlerts(employerId);

                // filter to just one tax year
                alerts = (from insurance_coverage_I coverage in alerts where coverage.IC_TAX_YEAR.Equals(taxYear) select coverage).ToList();

                // Get the Output table
                DataTable export = insuranceController.GetTableForCarrierExport();

                // Fill the table
                bool error = false;
                foreach (insurance_coverage_I alert in alerts)
                {
                    //build the data table of the export.
                    if (false == insuranceController.AddExportDataToTable(export, alert, Employer.EMPLOYER_EIN))
                    {
                        this.lblMsg.Text = "Failed to create row for Alert ID : " + alert.ROW_ID;
                        error = true;
                        break;
                    }
                }

                if (false == error)
                {
                    this.lblMsg.Text = "Complete!";

                    // Convert to a CSV string and download that as the file
                    // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file 
                    string filename = Employer.EMPLOYER_IMPORT_IC + "_" + Employer.EMPLOYER_EIN + "CarrierAlert";
                    string attachment = "attachment; filename=" + filename.CleanFileName() + ".csv";
                    this.Response.ClearContent();
                    this.Response.BufferOutput = false;
                    this.Response.AddHeader("content-disposition", attachment);
                    this.Response.ContentType = "application/vnd.ms-excel";

                    this.Response.Write(export.GetAsCsv());

                    // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
                    this.Response.Flush(); // Sends all currently buffered output to the client.
                    this.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                    this.Response.End();
                }
            }
            catch (Exception exception)
            {

                this.Log.Fatal("Errors during export carrier alerts!", exception);

                this.lblMsg.Text = exception.Message;

            }

        }

        private ILog Log = LogManager.GetLogger(typeof(CarrierAlertExport));

    }

}