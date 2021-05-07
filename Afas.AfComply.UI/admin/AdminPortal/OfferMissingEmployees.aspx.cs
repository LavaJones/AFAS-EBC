using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using System.Text.RegularExpressions;

using log4net;

using Afas.AfComply.Application;
using Afas.AfComply.Domain;
using Afas.AfComply.Domain.POCO;

using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class OfferMissingEmployees : AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.BulkConverterEnabled)
            {
                Log.Info("A user tried to access the Bulk LegacyConverter page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=37", false);
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

        protected void BtnUploadFile_Click(Object sender, EventArgs eventArgs)
        {

            lblMsg.Text = "";

            int employerId = 0;

            if (
                    null == DdlEmployer.SelectedItem
                        ||
                    null == DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                lblMsg.Text = "Incorrect parameters, select an employer from the list above.";

                return;

            }

            FileArchiverWrapper archive = new FileArchiverWrapper();

            long millis = DateTime.Now.Ticks / (long) TimeSpan.TicksPerMillisecond;

            employer employ = employerController.getEmployer(employerId);

            String ftpPath = Server.MapPath("~\\ftps\\");

            int detectedFiles = 0;

            Boolean coveragePlanYearsSelected = true;
            IList<int> coveragePlanYearIds = new List<int>();

            Boolean offerPlanYearsSelected = true;
            IList<int> offerPlanYearIds = new List<int>();

            if (OfferFile.HasFile)
            {

                foreach (System.Web.UI.WebControls.ListItem listItem in lblPlanYearsOffer.Items)
                {

                    if (listItem.Selected == true)
                    {

                        if (listItem.Value.ToLower().StartsWith("select"))
                        {
                            continue;
                        }

                        offerPlanYearIds.Add(Int32.Parse(listItem.Value));
                    }

                }

                offerPlanYearsSelected = offerPlanYearIds.Count() > 0;

                detectedFiles++;

            }

            if (detectedFiles > 1)
            {

                lblMsg.Text = "Only one file can be uploaded at a time. Files NOT processed.";

                return;

            }

            if (detectedFiles == 0)
            {

                lblMsg.Text = "Please select a file to upload.";

                return;

            }

            if (offerPlanYearsSelected == false)
            {

                lblMsg.Text = "You must select at least one plan year from the list to process an offer file! Files NOT processed.";

                return;

            }

            if ((offerPlanYearsSelected == true) && (offerPlanYearIds.Count() > 4))
            {

                lblMsg.Text = "You selected to many plan years for offer files! Files NOT processed.";

                return;

            }

            if (OfferFile.HasFile)
            {

                String offerPath = ftpPath + "TranslateFrom\\" + employ.EMPLOYER_ID + "_Offer_" + millis + ".csv";

                OfferFile.SaveAs(offerPath);

                String insuranceOfferFtpPath = Server.MapPath("~/ftps/insoffer");

                PIILogger.LogPII(string.Format("User [{0}] Uploaded Offer File [{1}] to [{2}] to find missing employees",
                    ((User)Session["CurrentUser"]).User_UserName, OfferFile.FileName, offerPath));
                String postConvertPathOffer = String.Format("{0}\\{1}_{2}_Offer.csv", insuranceOfferFtpPath, employ.EMPLOYER_IMPORT_IO, millis);

                DataTable dataTable = ConvertOffer(employ, offerPlanYearIds, offerPath, postConvertPathOffer, employ.EMPLOYER_EIN);

                archive.ArchiveFile(offerPath, employ.EMPLOYER_ID, "Offer Missing Employees Processing");

                lblMsg.Text = lblMsg.Text + "Missing Employees:<br/>";
                foreach (DataRow dataRow in dataTable.Rows)
                {

                    lblMsg.Text = lblMsg.Text +
                            String.Format("Name: {0} SSN: {1}<br/>", dataRow["NAME"], dataRow["SSN"]);
                
                }
            
            }

            if (lblMsg.Text.Contains("did not match"))
            {
                lblMsg.Text = "<br/><span style='color: red;font-weight: bold;'>This file does NOT belong to this employer. Contact your manager for assistance!</span><br/>" + lblMsg.Text;
            }

        }

        public DataTable ConvertOffer(
                employer theEmployer,
                IList<int> planYearIds,
                String sourceFileName,
                String destinationFileName,
                String employerFederalIdentificationNumber
            )
        {
            
            DataTable missingEmployees = null;

            try
            {

                String[] source = File.ReadAllLines(sourceFileName);

                ILegacyConverterService legacyConverterService = new LegacyConverterService();
                DataTable dataTable = legacyConverterService.ConvertExtendedOfferVariant1(source, employerFederalIdentificationNumber);

                String planYearOfferFile = String.Empty;

                foreach (int planYearId in planYearIds)
                {

                    planYearOfferFile = String.Format("{0}-{1}.csv", destinationFileName.Replace(".csv", String.Empty), planYearId);

                    missingEmployees = RemoveExistingEmployees(theEmployer, planYearId, dataTable, planYearOfferFile);
                
                }

                lblMsg.Text = lblMsg.Text + " \n" + "CONVERSION SUCCESS! ";

            }
            catch (Exception exception)
            {

                this.Log.Error("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n" + "CONVERSION Error : " + exception.Message;

                return null;

            }

            return missingEmployees;

        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {

            int employerId = 0;

            if (
                    null == DdlEmployer.SelectedItem
                        ||
                    null == DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                lblMsg.Text = "Incorrect parameters";

                lblPlanYearsOffer.Items.Clear();
                lblPlanYearsOffer.Visible = false;

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            loadPlanYears(employ.EMPLOYER_ID);

            cofein.Text = employ.EMPLOYER_EIN;

        }

        private void loadPlanYears(int employerId)
        {

            lblPlanYearsOffer.Items.Clear();

            IList<PlanYear> employerPlanYears = PlanYear_Controller.getEmployerPlanYear(employerId);

            lblPlanYearsOffer.DataSource = PlanYear_Controller.getEmployerPlanYear(employerId);
            lblPlanYearsOffer.DataTextField = "PLAN_YEAR_DESCRIPTION";
            lblPlanYearsOffer.DataValueField = "PLAN_YEAR_ID";
            lblPlanYearsOffer.DataBind();

            lblPlanYearsOffer.Items.Add("Select");
            lblPlanYearsOffer.SelectedIndex = lblPlanYearsOffer.Items.Count - 1;

            lblPlanYearsOffer.Visible = true;

        }

        public DataTable RemoveExistingEmployees(
                employer theEmployer,
                int planYearId,
                DataTable dataTable,
                String destinationFileName
            )
        {

            IList<Employee> employees = EmployeeController.manufactureEmployeeList(theEmployer.EMPLOYER_ID);

            IList<DataRow> inTheDemographicsFile = new List<DataRow>();

            foreach (DataRow dataRow in dataTable.Rows)
            {

                String socialSecurityNumber = String.Empty;
                if (dataTable.Columns.Contains("Subscriber SSN"))
                {
                    socialSecurityNumber = dataRow["Subscriber SSN"].ToString();
                }
                else
                {
                    socialSecurityNumber = dataRow["SSN"].ToString();
                }

                if (employees.FilterForSocialSecurityNumber(socialSecurityNumber).Count() > 1)
                {

                    this.Log.Error(String.Format("Found many employees for employer {0} with the same social!", theEmployer.EMPLOYER_ID));

                    throw new Exception("Found many employees with the same SSN, should not possible.");

                }

                if (employees.FilterForSocialSecurityNumber(socialSecurityNumber).Count() == 1)
                {
                    inTheDemographicsFile.Add(dataRow);
                }

            }

            foreach(DataRow dataRow in inTheDemographicsFile)
            {
                dataTable.Rows.Remove(dataRow);
            }

            dataTable.RenameColumn("OFFER_ROW_ID", "ROW ID");
            dataTable.RenameColumn("OFFER_EMPLOYEE_ID", "EMPLOYEE ID");
            dataTable.RenameColumn("OFFER_EMPLOYER_ID", "EMPLOYER ID");
            dataTable.RenameColumn("OFFER_PLANYEAR_ID", "PLAN YEAR ID");
            dataTable.RenameColumn("OFFER_EMPLOYEE_#", "PAYROLL ID");
            dataTable.RenameColumn("OFFER_NAME", "NAME");
            dataTable.RenameColumn("OFFER_CLASS_ID", "CLASS ID");
            dataTable.RenameColumn("OFFER_AVERAGE_HOURS", "AVG HOURS");
            dataTable.RenameColumn("OFFER_OFFERED", "OFFERED");
            dataTable.RenameColumn("OFFER_OFFERED_ON", "OFFERED ON");
            dataTable.RenameColumn("OFFER_ACCEPTED", "ACCEPTED");
            dataTable.RenameColumn("OFFER_ACCEPTED_ON", "ACCEPTED/DECLINED ON");
            dataTable.RenameColumn("OFFER_INSURANCE_ID", "INSURANCE ID");
            dataTable.RenameColumn("OFFER_CONTRIBUTION_ID", "CONTRIBUTION ID");
            dataTable.RenameColumn("OFFER_EFFECTIVE_DATE", "EFFECTIVE DATE");
            dataTable.RenameColumn("OFFER_HRA_FLEX", "HRA-Flex");

            dataTable.PruneToRequiredColumns(
                "ROW ID",
                "EMPLOYEE ID",
                "EMPLOYER ID",
                "PLAN YEAR ID",
                "PAYROLL ID",
                "NAME",
                "CLASS ID",
                "AVG HOURS",
                "OFFERED",
                "OFFERED ON",
                "ACCEPTED",
                "ACCEPTED/DECLINED ON",
                "INSURANCE ID",
                "CONTRIBUTION ID",
                "EFFECTIVE DATE",
                "HRA-Flex",
                "SSN"
            );

            String missingColumns = String.Empty;
            if (dataTable.VerifyContainsColumns(
                    out missingColumns,
                    "ROW ID",
                    "EMPLOYEE ID",
                    "EMPLOYER ID",
                    "PLAN YEAR ID",
                    "PAYROLL ID",
                    "NAME",
                    "CLASS ID",
                    "AVG HOURS",
                    "OFFERED",
                    "OFFERED ON",
                    "ACCEPTED",
                    "ACCEPTED/DECLINED ON",
                    "INSURANCE ID",
                    "CONTRIBUTION ID",
                    "EFFECTIVE DATE",
                    "HRA-Flex",
                    "SSN"
                ) == false)
            {

                String detailedErrorMesage = String.Format("File is missing required columns for offer: {0}", missingColumns);

                this.Log.Error(detailedErrorMesage);

                throw new Exception(detailedErrorMesage);

            }

            dataTable.ReorderColumns(
                    "ROW ID",
                    "EMPLOYEE ID",
                    "EMPLOYER ID",
                    "PLAN YEAR ID",
                    "PAYROLL ID",
                    "NAME",
                    "CLASS ID",
                    "AVG HOURS",
                    "OFFERED",
                    "OFFERED ON",
                    "ACCEPTED",
                    "ACCEPTED/DECLINED ON",
                    "INSURANCE ID",
                    "CONTRIBUTION ID",
                    "EFFECTIVE DATE",
                    "HRA-Flex",
                    "SSN"
                );

            return dataTable;

        }

        public void WriteoutPayrollFiles(DataTable dataTable, String destinationFileName)
        {

            foreach (DataRow dataRow in dataTable.Rows)
            {

                if (dataRow["Pay Description ID"].ToString().IsNullOrEmpty())
                {
                    dataRow["Pay Description ID"] = "01";
                }

                if (dataRow["Pay Description"].ToString().IsNullOrEmpty())
                {
                    dataRow["Pay Description"] = Branding.ProductName;
                }

                if (dataRow["Check Date"].ToString().IsNullOrEmpty())
                {
                    dataRow["Check Date"] = "19200101";
                }
            
            }

            dataTable.WriteOutCsv(destinationFileName);

        }

        private ILog Log = LogManager.GetLogger(typeof(OfferMissingEmployees));

    }

}