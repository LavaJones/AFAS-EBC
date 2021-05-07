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

    public partial class LegacyConverter : AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.BulkConverterEnabled)
            {
                Log.Info("A user tried to access the Bulk LegacyConverter page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=33", false);
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

            long millis = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;

            employer employ = employerController.getEmployer(employerId);

            String ftpPath = Server.MapPath("~\\ftps\\");

            int detectedFiles = 0;

            Boolean coveragePlanYearsSelected = true;
            IList<int> coveragePlanYearIds = new List<int>();

            Boolean offerPlanYearsSelected = true;
            IList<int> offerPlanYearIds = new List<int>();

            if (DemographicsFile.HasFile)
            {
                detectedFiles++;
            }

            if (CoverageFile.HasFile)
            {

                foreach (System.Web.UI.WebControls.ListItem listItem in lblPlanYearsCoverage.Items)
                {

                    if (listItem.Selected == true)
                    {

                        if (listItem.Value.ToLower().StartsWith("select"))
                        {
                            continue;
                        }

                        coveragePlanYearIds.Add(Int32.Parse(listItem.Value));
                    }

                }

                coveragePlanYearsSelected = coveragePlanYearIds.Count() > 0;

                detectedFiles++;

            }

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

            if (PayrollFile.HasFile)
            {
                detectedFiles++;
            }

            if (OhioAffordPayrollFile.HasFile)
            {
                detectedFiles++;
            }

            if (OhioAffordAlternatePayrollFile.HasFile)
            {
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

            if (coveragePlanYearsSelected == false)
            {

                lblMsg.Text = "You must select at least one plan year from the list to process a coverage file! Files NOT processed.";

                return;

            }

            if ((coveragePlanYearsSelected == true) && (coveragePlanYearIds.Count() > 4))
            {

                lblMsg.Text = "You selected to many plan years for coverage files! Files NOT processed.";

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

            if (DemographicsFile.HasFile)
            {

                String demographicsPath = ftpPath + "TranslateFrom\\" + employ.EMPLOYER_ID + "_Demographics_" + millis + ".csv";

                DemographicsFile.SaveAs(demographicsPath);

                PIILogger.LogPII(string.Format("User [{0}] Uploaded Demographics File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, DemographicsFile.FileName, demographicsPath));

                String postConvertPath = String.Format("{0}{1}_{2}_Demographics.csv", ftpPath, employ.EMPLOYER_IMPORT_EMPLOYEE, millis);

                ConvertDemographics(demographicsPath, postConvertPath, employ.EMPLOYER_EIN);

                PIILogger.LogPII(string.Format("User [{0}] Converted Demographics File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, demographicsPath, postConvertPath));

                archive.ArchiveFile(demographicsPath, employ.EMPLOYER_ID, "Legacy Converter Demographic Processing");

            }

            if (CoverageFile.HasFile)
            {

                String coveragePath = ftpPath + "TranslateFrom\\" + employ.EMPLOYER_ID + "_Coverage_" + millis + ".csv";

                CoverageFile.SaveAs(coveragePath);

                String insuranceCarrierFtpPath = Server.MapPath("~/ftps/inscarrier");
                String insuranceOfferFtpPath = Server.MapPath("~/ftps/insoffer");

                PIILogger.LogPII(string.Format("User [{0}] Uploaded Coverage File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, CoverageFile.FileName, coveragePath));

                String postConvertPathOffer = String.Format("{0}\\{1}_{2}_Coverage_Offer.csv", insuranceOfferFtpPath, employ.EMPLOYER_IMPORT_IO, millis);
                String postConvertPathCarrier = String.Format("{0}\\{1}_{2}_Coverage_Carrier.csv", insuranceCarrierFtpPath, employ.EMPLOYER_IMPORT_IC, millis);

                ConvertCoverage(employ, coveragePlanYearIds, coveragePath, postConvertPathOffer, postConvertPathCarrier, employ.EMPLOYER_EIN);

                PIILogger.LogPII(
                        String.Format("User [{0}] Converted Coverage File [{1}] to offer file [{2}]", ((User)Session["CurrentUser"]).User_UserName, coveragePath, postConvertPathOffer)
                    );


                archive.ArchiveFile(coveragePath, employ.EMPLOYER_ID, "Legacy Converter Coverage Processing");

            }

            if (OfferFile.HasFile)
            {

                String offerPath = ftpPath + "TranslateFrom\\" + employ.EMPLOYER_ID + "_Offer_" + millis + ".csv";

                OfferFile.SaveAs(offerPath);

                String insuranceOfferFtpPath = Server.MapPath("~/ftps/insoffer");

                PIILogger.LogPII(string.Format("User [{0}] Uploaded Offer File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, OfferFile.FileName, offerPath));
                String postConvertPathOffer = String.Format("{0}\\{1}_{2}_Offer.csv", insuranceOfferFtpPath, employ.EMPLOYER_IMPORT_IO, millis);

                ConvertOffer(employ, offerPlanYearIds, offerPath, postConvertPathOffer, employ.EMPLOYER_EIN);

                PIILogger.LogPII(
                        String.Format("User [{0}] Converted Offer File [{1}] to offer file [{2}]", ((User)Session["CurrentUser"]).User_UserName, offerPath, postConvertPathOffer)
                    );


                archive.ArchiveFile(offerPath, employ.EMPLOYER_ID, "Legacy Converter Offer Processing");

            }

            if (OhioAffordAlternatePayrollFile.HasFile)
            {

                String selectedDays = OhioAffordAlternatePayrollFileDays.SelectedItem.Value;

                if (selectedDays.IsNullOrEmpty())
                {

                    lblMsg.Text = lblMsg.Text + " \n" + "Select a pay period length. File not processed!";

                    return;

                }

                int daysInPayPeriod = Int32.Parse(selectedDays);

                String payrollPath = ftpPath + "TranslateFrom\\" + employ.EMPLOYER_ID + "_Payroll_Hours_" + millis + ".csv";

                OhioAffordAlternatePayrollFile.SaveAs(payrollPath);

                PIILogger.LogPII(String.Format("User [{0}] Uploaded PayrollFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, PayrollFile.FileName, payrollPath));

                String postConvertPath = String.Format("{0}{1}_{2}_Payroll.csv", ftpPath, employ.EMPLOYER_IMPORT_PAYROLL, millis);

                ConvertOhioPayrollMissingStartDate(payrollPath, postConvertPath, employ.EMPLOYER_EIN, daysInPayPeriod);

                PIILogger.LogPII(String.Format("User [{0}] Converted Payroll File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, payrollPath, postConvertPath));

                archive.ArchiveFile(payrollPath, employ.EMPLOYER_ID, "Legacy Converter Payroll Processing");

            }

            if (OhioAffordPayrollFile.HasFile)
            {

                String payrollPath = ftpPath + "TranslateFrom\\" + employ.EMPLOYER_ID + "_Payroll_Hours_" + millis + ".csv";

                OhioAffordPayrollFile.SaveAs(payrollPath);

                PIILogger.LogPII(String.Format("User [{0}] Uploaded Payroll File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, PayrollFile.FileName, payrollPath));

                String postConvertPath = String.Format("{0}{1}_{2}_Payroll.csv", ftpPath, employ.EMPLOYER_IMPORT_PAYROLL, millis);

                ConvertOhioPayroll(payrollPath, postConvertPath, employ.EMPLOYER_EIN);

                PIILogger.LogPII(String.Format("User [{0}] Converted Payroll File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, payrollPath, postConvertPath));

                archive.ArchiveFile(payrollPath, employ.EMPLOYER_ID, "Legacy Converter Payroll Processing");

            }

            if (PayrollFile.HasFile)
            {

                String selectedDays = PayrollFileDays.SelectedItem.Value;

                if (selectedDays.IsNullOrEmpty())
                {

                    lblMsg.Text = lblMsg.Text + " \n" + "Select a pay period length. Payroll file not processed!";

                    return;

                }

                int daysInPayPeriod = Int32.Parse(selectedDays);

                String payrollPath = ftpPath + "TranslateFrom\\" + employ.EMPLOYER_ID + "_Payroll_Hours_" + millis + ".csv";

                PayrollFile.SaveAs(payrollPath);

                PIILogger.LogPII(string.Format("User [{0}] Uploaded Payroll File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, PayrollFile.FileName, payrollPath));

                String postConvertPath = String.Format("{0}{1}_{2}_Payroll.csv", ftpPath, employ.EMPLOYER_IMPORT_PAYROLL, millis);

                ConvertPayroll(payrollPath, postConvertPath, employ.EMPLOYER_EIN, daysInPayPeriod);

                PIILogger.LogPII(string.Format("User [{0}] Converted Payroll File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, payrollPath, postConvertPath));

                archive.ArchiveFile(payrollPath, employ.EMPLOYER_ID, "Legacy Import Converter Automatic Payroll Processing");

            }

            if (lblMsg.Text.Contains("did not match"))
            {
                lblMsg.Text = "<br/><span style='color: red;font-weight: bold;'>This file does NOT belong to this employer. Contact your manager for assistance!</span><br/>" + lblMsg.Text;
            }

        }

        public void ConvertCoverage(
                employer theEmployer,
                IList<int> planYearIds,
                String sourceFileName,
                String destinationFileNameOffer,
                String destinationFileNameCarrier,
                String employerFederalIdentificationNumber
            )
        {

            try
            {

                String[] source = File.ReadAllLines(sourceFileName);

                ILegacyConverterService legacyConverterService = new LegacyConverterService();
                DataTable dataTable = legacyConverterService.ConvertCoverageVariant1(source, employerFederalIdentificationNumber);

                var dependentRecords = (
                                        from DataRow dataRow in dataTable.Rows
                                        where dataRow["InsuredMember"].ToString().Trim().ToUpper() == "D"
                                        select dataRow["InsuredMember"]
                                       ).ToList();

                Boolean needsCarrierFile = dependentRecords.Count() > 0;
                WriteoutCoverageFiles(theEmployer, planYearIds, dataTable, destinationFileNameOffer, destinationFileNameCarrier, needsCarrierFile);

                if (needsCarrierFile)
                {

                    PIILogger.LogPII(
                            String.Format("User [{0}] Converted Coverage File [{1}] to carrier file [{2}]", ((User)Session["CurrentUser"]).User_UserName, sourceFileName, destinationFileNameCarrier)
                        );

                }

                lblMsg.Text = lblMsg.Text + " \n" + "COVERAGE SUCCESS! ";

            }
            catch (Exception exception)
            {



                this.Log.Warn("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n" + "COVERAGE Error : " + exception.Message;

            }

        }

        public void ConvertDemographics(String sourceFileName, String destinationFileName, String employerFederalIdentificationNumber)
        {

            try
            {

                String[] source = File.ReadAllLines(sourceFileName);

                ILegacyConverterService legacyConverterService = new LegacyConverterService();
                DataTable dataTable = legacyConverterService.ConvertDemographicsVariant1(source, employerFederalIdentificationNumber);

                var distinctEmployeeTypes = (from DataRow distinctDataRow in dataTable.Rows
                                             select distinctDataRow["EEType"]
                                            ).Distinct().ToList();

                if (distinctEmployeeTypes.Count() == 1 && distinctEmployeeTypes.First().ToString().Trim().Length == 0)
                {

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        dataRow["EEType"] = "All Employees";
                    }

                    distinctEmployeeTypes = (from DataRow distinctDataRow in dataTable.Rows
                                             select distinctDataRow["EEType"]
                                            ).Distinct().ToList();

                }

                foreach (String employeeType in distinctEmployeeTypes)
                {

                    String employeeTypeFileName = String.Format(
                            "{0}-{1}.csv",
                            destinationFileName.Replace(".csv", String.Empty),
                            ConverterHelper.BuildSafeFilename(employeeType)
                        );

                    DataTable filteredDataTable = (from r in dataTable.AsEnumerable() where r.Field<String>("EEType") == employeeType select r).CopyToDataTable();

                    WriteoutDemographicFiles(filteredDataTable, employeeTypeFileName);

                }

                lblMsg.Text = lblMsg.Text + " \n" + "DEMOGRAPHICS SUCCESS! ";

            }
            catch (Exception exception)
            {

                this.Log.Error("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n" + "DEMOGRAPHICS Error : " + exception.Message;

            }

        }

        public void ConvertOffer(
                employer theEmployer,
                IList<int> planYearIds,
                String sourceFileName,
                String destinationFileName,
                String employerFederalIdentificationNumber
            )
        {

            try
            {

                String[] source = File.ReadAllLines(sourceFileName);

                ILegacyConverterService legacyConverterService = new LegacyConverterService();
                DataTable dataTable = legacyConverterService.ConvertExtendedOfferVariant1(source, employerFederalIdentificationNumber);

                String planYearOfferFile = String.Empty;

                foreach (int planYearId in planYearIds)
                {

                    planYearOfferFile = String.Format("{0}-{1}.csv", destinationFileName.Replace(".csv", String.Empty), planYearId);

                    DataTable filledInOffer = FillinOfferFile(theEmployer, planYearId, dataTable);

                    WriteoutOfferFile(filledInOffer, planYearOfferFile);

                }

                lblMsg.Text = lblMsg.Text + " \n" + "OFFER SUCCESS! ";

            }
            catch (Exception exception)
            {

                this.Log.Error("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n" + "OFFER Error : " + exception.Message;

            }

        }

        public void ConvertOhioPayroll(String sourceFileName, String destinationFileName, String employerFederalIdentificationNumber)
        {

            try
            {

                String[] source = File.ReadAllLines(sourceFileName);

          
                if (source[0].Contains("START_DT") == false)
                {

                    lblMsg.Text = lblMsg.Text + " \n" + "This Ohio variant is missing the Start Date, use the Alternate Ohio format, Files not processed! ";

                    return;

                }

                ILegacyConverterService legacyConverterService = new LegacyConverterService();
                DataTable dataTable = legacyConverterService.ConvertPayrollOhioVariant1(source, employerFederalIdentificationNumber);

                WriteoutPayrollFiles(dataTable, destinationFileName);

                lblMsg.Text = lblMsg.Text + " \n" + "PAYROLL SUCCESS! ";

            }
            catch (Exception exception)
            {

                this.Log.Error("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n" + "PAYROLL Error : " + exception.Message;

            }

        }

        public void ConvertOhioPayrollMissingStartDate(
                String sourceFileName,
                String destinationFileName,
                String employerFederalIdentificationNumber,
                int daysInThePast
            )
        {
            
            try
            {

                String[] source = File.ReadAllLines(sourceFileName);

                
                if (source[0].Contains("START_DT") == true)
                {

                    lblMsg.Text = lblMsg.Text + " \n" + "This Ohio variant has the Start Date, use the Ohio format, Files not processed! ";

                    return;

                }

                ILegacyConverterService legacyConverterService = new LegacyConverterService();
                DataTable dataTable = legacyConverterService.ConvertPayrollOhioVariant2(source, employerFederalIdentificationNumber, daysInThePast);

                WriteoutPayrollFiles(dataTable, destinationFileName);

                lblMsg.Text = lblMsg.Text + " \n" + "PAYROLL SUCCESS! ";

            }
            catch (Exception exception)
            {

                this.Log.Error("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n" + "PAYROLL Error : " + exception.Message;

            }

        }

        public void ConvertPayroll(String sourceFileName, String destinationFileName, String employerFederalIdentificationNumber, int daysInThePast)
        {
           
            try
            {

                String[] source = File.ReadAllLines(sourceFileName);

                ILegacyConverterService legacyConverterService = new LegacyConverterService();
                DataTable dataTable = legacyConverterService.ConvertPayrollVariant1(source, employerFederalIdentificationNumber, daysInThePast);

                WriteoutPayrollFiles(dataTable, destinationFileName);

                lblMsg.Text = lblMsg.Text + " \n" + "PAYROLL SUCCESS! ";

            }
            catch (Exception exception)
            {

                this.Log.Error("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n" + "PAYROLL Error : " + exception.Message;

            }

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

                lblPlanYearsCoverage.Items.Clear();
                lblPlanYearsCoverage.Visible = false;

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

            lblPlanYearsCoverage.Items.Clear();
            lblPlanYearsOffer.Items.Clear();

            IList<PlanYear> employerPlanYears = PlanYear_Controller.getEmployerPlanYear(employerId);

            lblPlanYearsCoverage.DataSource = PlanYear_Controller.getEmployerPlanYear(employerId);
            lblPlanYearsCoverage.DataTextField = "PLAN_YEAR_DESCRIPTION";
            lblPlanYearsCoverage.DataValueField = "PLAN_YEAR_ID";
            lblPlanYearsCoverage.DataBind();

            lblPlanYearsCoverage.Items.Add("Select");
            lblPlanYearsCoverage.SelectedIndex = lblPlanYearsCoverage.Items.Count - 1;

            lblPlanYearsCoverage.Visible = true;

            lblPlanYearsOffer.DataSource = PlanYear_Controller.getEmployerPlanYear(employerId);
            lblPlanYearsOffer.DataTextField = "PLAN_YEAR_DESCRIPTION";
            lblPlanYearsOffer.DataValueField = "PLAN_YEAR_ID";
            lblPlanYearsOffer.DataBind();

            lblPlanYearsOffer.Items.Add("Select");
            lblPlanYearsOffer.SelectedIndex = lblPlanYearsOffer.Items.Count - 1;

            lblPlanYearsOffer.Visible = true;

        }

        public DataTable FillinOfferFile(
                employer theEmployer,
                int planYearId,
                DataTable dataTable
            )
        {

            IList<Employee> employees = EmployeeController.manufactureEmployeeList(theEmployer.EMPLOYER_ID);
            IList<PlanYear> planYears = PlanYear_Controller.getEmployerPlanYear(theEmployer.EMPLOYER_ID);
            PlanYear planYear = planYears.FilterForPlanYearId(planYearId).Single();
            IList<insurance> insurances = insuranceController.getAllActiveInsurancePlans(theEmployer.EMPLOYER_ID, false);
            IList<alert_insurance> offerAlerts = alert_controller.manufactureEmployerInsuranceAlertList(theEmployer.EMPLOYER_ID);

            insurance defaultInsurance = null;
            insurance insurance_ = null;
            Boolean mustLookupInsurance = false;
            if (insurances.FilterForPlanYearByPlanYearId(planYearId).Count() == 0)
            {

                this.Log.Error(String.Format("Found 0 insurance plans for employer {0}!", theEmployer.EMPLOYER_ID));

                throw new Exception("Found no active insurance plans for employer.");

            }

            if (insurances.FilterForPlanYearByPlanYearId(planYearId).Count() == 1)
            {

                defaultInsurance = insurances.FilterForPlanYearByPlanYearId(planYearId).Single();

                mustLookupInsurance = false;

            }
            else if (insurances.FilterForPlanYearByPlanYearId(planYearId).Count() > 1)
            {
                mustLookupInsurance = true;
            }

            if (offerAlerts.FilterForPlanYearByPlanYearId(planYearId).Count() == 0)
            {
                this.Log.Warn(String.Format("For employer {0} plan year {1} does not exist in the offer alerts, continuing.", theEmployer.EMPLOYER_ID, planYearId));
            }

            IList<DataRow> notInThisPlanYear = new List<DataRow>();

            dataTable.Columns.AddColumnIfMissing("REJECTION-REASON");

            DataTable sortedDataTable = dataTable.AsEnumerable()
                                            .OrderBy(r => r.Field<String>("SSN"))
                                            .ThenBy(r => r.Field<String>("Coverage Date Start"))
                                            .CopyToDataTable();

            sortedDataTable.Columns.Add("EVENT_ROW_ID", typeof(long));
            sortedDataTable.Columns.Add("TYPE_OF_EVENT", typeof(String));
            sortedDataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(DateTime));

            int eventRowId = 0;

            IList<DataRow> rowsToBeAdded = new List<DataRow>();

            IList<String> processedSocials = new List<String>();
            IList<String> changeEventSocials = new List<String>();
            IList<String> offerRejectionSocials = new List<String>();

            new LegacyConverterService().SetBlankCoverageEndDateToPlanYearEnd(sortedDataTable, planYear.PLAN_YEAR_END.Value);

            foreach (DataRow dataRow in sortedDataTable.Rows)
            {

                insurance_ = null;

                eventRowId++;

                dataRow["EVENT_ROW_ID"] = eventRowId;

                dataRow["TYPE_OF_EVENT"] = OfferChangeEvents.SimpleOffer;
                dataRow["OFFER_EMPLOYER_ID"] = theEmployer.EMPLOYER_ID;
                dataRow["OFFER_PLANYEAR_ID"] = planYearId;

                String socialSecurityNumber = String.Empty;
                if (sortedDataTable.Columns.Contains("Subscriber SSN"))
                {
                    socialSecurityNumber = dataRow["Subscriber SSN"].ToString().ZeroPadSsn();
                }
                else
                {
                    socialSecurityNumber = dataRow["SSN"].ToString().ZeroPadSsn();
                }

                if (employees.FilterForSocialSecurityNumber(socialSecurityNumber).Count() == 0)
                {

                    this.Log.Error(String.Format("Employee for employer {0} does not exist in the demographics!", theEmployer.EMPLOYER_ID));

                    throw new Exception(String.Format("Employee for employer {0} does not exist in the demographics, can not provide any information! Use the offer missing employees page. Employee Last Name: [{1}]", theEmployer.EMPLOYER_ID, dataRow["OFFER_NAME"]));

                }

                if (employees.FilterForSocialSecurityNumber(socialSecurityNumber).Count() > 1)
                {

                    this.Log.Error(String.Format("Found many employees for employer {0} with the same social!", theEmployer.EMPLOYER_ID));

                    throw new Exception("Found many employees with the same SSN, should not possible.");

                }
                Employee employee = employees.FilterForSocialSecurityNumber(socialSecurityNumber).Single();

                dataRow["OFFER_EMPLOYEE_ID"] = employee.EMPLOYEE_ID;
                dataRow["OFFER_EETYPE_ID"] = employee.EMPLOYEE_TYPE_ID;
                dataRow["OFFER_CLASS_ID"] = employee.EMPLOYEE_CLASS_ID;
                dataRow["EMPLOYEE_HIRE_DATE"] = employee.EMPLOYEE_HIRE_DATE.Value;

                Boolean offeredFlag = new LegacyConverterService().ParseRequiredBoolean(sortedDataTable, dataRow, "OFFER_OFFERED");
                Boolean acceptedFlag = new LegacyConverterService().ParseRequiredBoolean(sortedDataTable, dataRow, "OFFER_ACCEPTED");

                if (acceptedFlag == true)
                {

                    DateTime coverageStartDate = new LegacyConverterService().ParseRequiredDate(sortedDataTable, dataRow, "Coverage Date Start");
                    DateTime coverageEndDate = new LegacyConverterService().ParseRequiredDate(sortedDataTable, dataRow, "Coverage Date End");

                    if (new LegacyConverterService().IsCoverageDatesWithinThisPlanYear(dataRow, planYear.PLAN_YEAR_START.Value, planYear.PLAN_YEAR_END.Value) == false)
                    {

                        notInThisPlanYear.Add(dataRow);

                        continue;

                    }

                }
                else
                {

                    DateTime declinedDate = new LegacyConverterService().ParseRequiredDate(sortedDataTable, dataRow, "OFFER_ACCEPTED_ON");
                    DateTime coverageEndDate = new LegacyConverterService().ParseRequiredDate(sortedDataTable, dataRow, "Coverage Date End");

                    Boolean overWroteStartDate = false;

                    if (dataRow["Coverage Date Start"].ToString().IsNullOrEmpty() == true)
                    {

                        dataRow["Coverage Date Start"] = declinedDate.ToShortDateString();

                        overWroteStartDate = true;

                    }

                    if (new LegacyConverterService().IsCoverageDatesWithinThisPlanYear(dataRow, planYear.PLAN_YEAR_START.Value, planYear.PLAN_YEAR_END.Value) == false)
                    {

                        notInThisPlanYear.Add(dataRow);

                        if (overWroteStartDate)
                        {
                            dataRow["Coverage Date Start"] = String.Empty;
                        }

                        continue;

                    }

                    if (overWroteStartDate)
                    {
                        dataRow["Coverage Date Start"] = String.Empty;
                    }


                }

                if (mustLookupInsurance)
                {

                    String medicalPlanName = dataRow["Medical Plan Name"].ToString();

                    if (String.IsNullOrEmpty(medicalPlanName))
                    {

                        this.Log.Error(String.Format("Found more than 1 active insurance plan for employer {0} and the offer file line has no medical plan name!", theEmployer.EMPLOYER_ID));

                        throw new Exception(String.Format("Found more than once active insurance plan for employer and the offer file line has no medical plan name for {0}.", dataRow["OFFER_NAME"]));

                    }

                    if (insurances.FilterForPlanYearByPlanYearId(planYearId).ToList().FilterForInsuranceName(medicalPlanName).Count() == 0)
                    {

                        this.Log.Error(String.Format("Found more than 1 active insurance plan for employer {0} and no medical plan name!", theEmployer.EMPLOYER_ID));

                        throw new Exception(String.Format("Found no active insurance plan for employer {0} and medical plan name '{1}'.", theEmployer.EMPLOYER_ID, medicalPlanName));

                    }
                    else if (insurances.FilterForPlanYearByPlanYearId(planYearId).ToList().FilterForInsuranceName(medicalPlanName).Count() > 1)
                    {

                        this.Log.Error(String.Format("Found more than 1 active insurance plan for employer {0} with medical plan name '{1}'!", theEmployer.EMPLOYER_ID, medicalPlanName));

                        throw new Exception(String.Format("Found more than once active insurance plan for employer {0} for medical plan name '{1}'.", theEmployer.EMPLOYER_ID, medicalPlanName));

                    }

                    insurance_ = insurances.FilterForPlanYearByPlanYearId(planYearId).ToList().FilterForInsuranceName(medicalPlanName).Single();

                }
                else
                {
                    insurance_ = defaultInsurance;
                }

                IList<insuranceContribution> insuranceContributions = insuranceController.manufactureInsuranceContributionList(insurance_.INSURANCE_ID);

                if (insuranceContributions.FilterForEmployeeClassByEmployeeClassId(employee.EMPLOYEE_CLASS_ID).Count() == 0)
                {

                    dataRow["REJECTION-REASON"] = "MISSING CLASS CONTRIBUTION!";

                    if (offerRejectionSocials.Contains(socialSecurityNumber) == false)
                    {
                        offerRejectionSocials.Add(socialSecurityNumber);
                    }

                    continue;

                }

                if (insuranceContributions.FilterForEmployeeClassByEmployeeClassId(employee.EMPLOYEE_CLASS_ID).Count() > 1)
                {

                    this.Log.Error(String.Format("Found more than one insurance contribution for employer {0} for plan year {1} for class {2}!", theEmployer.EMPLOYER_ID, planYearId, employee.EMPLOYEE_CLASS_ID));

                    throw new Exception(String.Format("Found more than one insurance contribution for employer {0} for plan year {1} for class {2}!", theEmployer.EMPLOYER_ID, planYearId, employee.EMPLOYEE_CLASS_ID));

                }
                insuranceContribution insuranceContribution_ = insuranceContributions.FilterForEmployeeClassByEmployeeClassId(employee.EMPLOYEE_CLASS_ID).Single();

                dataRow["OFFER_INSURANCE_ID"] = insurance_.INSURANCE_ID;
                dataRow["OFFER_CONTRIBUTION_ID"] = insuranceContribution_.INS_CONT_ID;

                DateTime effectiveDateTime = new LegacyConverterService().ParseRequiredDate(sortedDataTable, dataRow, "OFFER_EFFECTIVE_DATE");
                new LegacyConverterService().CorrectEffectiveDatesBeforePlanYearStartDates(dataRow, effectiveDateTime, planYear.PLAN_YEAR_START.Value);

                if (offerAlerts.FilterForEmployeeByEmployeeId(employee.EMPLOYEE_ID).ToList().FilterForPlanYearByPlanYearId(planYearId).Count() == 0)
                {

                    alert_insurance alertInsurance = insuranceController.findSingleInsuranceOffer(planYearId, employee.EMPLOYEE_ID);

                    if (alertInsurance == null)
                    {

                        dataRow["TYPE_OF_EVENT"] = OfferChangeEvents.SimpleOffer;
                        dataRow["OFFER_ROW_ID"] = "NEW-HIRE-COVERAGE";

                        dataRow["OFFER_AVERAGE_HOURS"] = "0.00";

                        this.Log.Info(String.Format("Employee {0} for employer {1} does not exist in the offer alerts for plan year {2}.", employee.EMPLOYEE_ID, theEmployer.EMPLOYER_ID, planYearId));

                    }
                    else
                    {

                        dataRow["OFFER_ROW_ID"] = alertInsurance.ROW_ID;
                        dataRow["OFFER_AVERAGE_HOURS"] = alertInsurance.EMPLOYEE_AVG_HOURS;

                    }

                }
                else
                {

                    alert_insurance alertInsurance = offerAlerts.FilterForEmployeeByEmployeeId(employee.EMPLOYEE_ID).ToList().FilterForPlanYearByPlanYearId(planYearId).Single();

                    dataRow["OFFER_ROW_ID"] = alertInsurance.ROW_ID;
                    dataRow["OFFER_AVERAGE_HOURS"] = alertInsurance.EMPLOYEE_AVG_HOURS;

                }

                if (processedSocials.Contains(socialSecurityNumber))
                {

                    if (changeEventSocials.Contains(socialSecurityNumber) == false)
                    {
                        changeEventSocials.Add(socialSecurityNumber);
                    }

                }
                else
                {
                    processedSocials.Add(socialSecurityNumber);
                }


            }

            sortedDataTable.Rows.RemoveRows(notInThisPlanYear);

            foreach (DataRow dataRow in rowsToBeAdded)
            {
                sortedDataTable.Rows.Add(dataRow);
            }

            foreach (DataRow dataRow in sortedDataTable.Rows)
            {

                String socialSecurityNumber = String.Empty;
                if (sortedDataTable.Columns.Contains("Subscriber SSN"))
                {
                    socialSecurityNumber = dataRow["Subscriber SSN"].ToString();
                }
                else
                {
                    socialSecurityNumber = dataRow["SSN"].ToString();
                }

                if (new LegacyConverterService().ShouldOfferBeRejected(dataRow, offerRejectionSocials))
                {

                    if (offerRejectionSocials.Contains(socialSecurityNumber) == false)
                    {
                        offerRejectionSocials.Add(socialSecurityNumber);
                    }

                }

            }

            new LegacyConverterService().ResetCoverageEndDateBasedUponPlanYearEnd(dataTable, planYear.PLAN_YEAR_END.Value);

            foreach (DataRow dataRow in sortedDataTable.Rows)
            {

                String coverageEnd = dataRow["Coverage Date End"].ToString();
                if (String.IsNullOrEmpty(coverageEnd) == false)
                {

                    DateTime coverageEndDate;

                    if (DateTime.TryParse(coverageEnd, out coverageEndDate) == false)
                    {
                        throw new Exception(String.Format("Unable to determine coverage end date {0} - insurance change event detected.", dataRow["OFFER_NAME"].ToString()));
                    }

                    if (coverageEndDate < planYear.PLAN_YEAR_END.Value)
                    {

                        DateTime originalEffectiveDate = new LegacyConverterService().ParseRequiredDate(sortedDataTable, dataRow, "OFFER_EFFECTIVE_DATE");
                        DateTime originalAcceptedDate = new LegacyConverterService().ParseRequiredDate(sortedDataTable, dataRow, "OFFER_ACCEPTED_ON");
                        DateTime originalOfferedDate = new LegacyConverterService().ParseRequiredDate(sortedDataTable, dataRow, "OFFER_OFFERED_ON");

                        if (coverageEndDate < originalEffectiveDate)
                        {
                            throw new Exception(String.Format("Original effective date '{0}' is after the coverage ends '{1}' - insurance change event detected for {2}.", originalEffectiveDate.ToShortDateString(), coverageEndDate.ToShortDateString(), dataRow["OFFER_NAME"].ToString()));
                        }

                        if (coverageEndDate < originalAcceptedDate)
                        {
                            throw new Exception(String.Format("Original accepted date '{0}' is after the coverage ends '{1}' - insurance change event detected for {2}.", originalAcceptedDate.ToShortDateString(), coverageEndDate.ToShortDateString(), dataRow["OFFER_NAME"].ToString()));
                        }

                        if (coverageEndDate < originalOfferedDate)
                        {
                            throw new Exception(String.Format("Original offered date '{0}' is after the coverage ends '{1}' - insurance change event detected for {2}.", originalOfferedDate.ToShortDateString(), coverageEndDate.ToShortDateString(), dataRow["OFFER_NAME"].ToString()));
                        }

                        DataRow changeEvent = sortedDataTable.NewRow();

                        foreach (DataColumn dataColumn in sortedDataTable.Columns)
                        {
                            changeEvent[dataColumn.ColumnName] = dataRow[dataColumn.ColumnName];
                        }

                        eventRowId++;
                        changeEvent["EVENT_ROW_ID"] = eventRowId;
                        changeEvent["TYPE_OF_EVENT"] = OfferChangeEvents.InsuranceChange;
                        changeEvent["OFFER_OFFERED"] = false;
                        changeEvent["OFFER_OFFERED_ON"] = coverageEndDate.AddDays(1).ToShortDateString();
                        changeEvent["OFFER_ACCEPTED"] = false;
                        changeEvent["OFFER_ACCEPTED_ON"] = coverageEndDate.AddDays(1).ToShortDateString();
                        changeEvent["OFFER_EFFECTIVE_DATE"] = coverageEndDate.AddDays(1).ToShortDateString();

                        if (new LegacyConverterService().ShouldOfferBeRejected(changeEvent, offerRejectionSocials))
                        {

                            String socialSecurityNumber = String.Empty;
                            if (sortedDataTable.Columns.Contains("Subscriber SSN"))
                            {
                                socialSecurityNumber = dataRow["Subscriber SSN"].ToString();
                            }
                            else
                            {
                                socialSecurityNumber = dataRow["SSN"].ToString();
                            }

                            if (offerRejectionSocials.Contains(socialSecurityNumber) == false)
                            {
                                offerRejectionSocials.Add(socialSecurityNumber);
                            }

                        }

                        rowsToBeAdded.Add(changeEvent);

                    }

                }

            }

            foreach (DataRow dataRow in rowsToBeAdded)
            {
                sortedDataTable.Rows.Add(dataRow);
            }

            DataTable rejectedOffersTagged = new LegacyConverterService().ClassifyInsuranceRejections(sortedDataTable, offerRejectionSocials);

            DataTable classifedOfferers = new LegacyConverterService().ClassifyInsuranceChangeEvents(rejectedOffersTagged, changeEventSocials);

            return classifedOfferers.AsEnumerable()
                                    .OrderBy(r => r.Field<long>("EVENT_ROW_ID"))
                                    .CopyToDataTable();

        }



        public void WriteoutCsvFile(DataTable dataTable, String destinationFileName)
        {

            dataTable.WriteOutCsv(destinationFileName);

        }

        public void WriteoutDemographicFiles(DataTable dataTable, String destinationFileName)
        {

            dataTable.Columns.RemoveColumnIfExists("EEType");

            var distinctHrStatusIds = (from DataRow distinctDataRow in dataTable.Rows
                                       select distinctDataRow["HR Status Code"]
                                      ).Distinct().ToList();

            foreach (String hrStatusId in distinctHrStatusIds)
            {

                if (hrStatusId.Equals("-E3B0C44298FC1C149AF", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                DataTable filteredDataTable = (from r in dataTable.AsEnumerable() where r.Field<String>("HR Status Code") == hrStatusId select r).CopyToDataTable();

                String safeFilename = ConverterHelper.BuildSafeFilename(filteredDataTable.Rows[0]["HR Status Description"].ToString());

                String filteredFilename = String.Format("{0}-{1}.csv", destinationFileName.Replace(".csv", String.Empty), safeFilename);

                foreach (DataRow dataRow in filteredDataTable.Rows)
                {

                    dataRow["HR Status Code"] = "01";

                    dataRow["HR Status Description"] = Branding.ProductName;

                    if (dataRow["DOB"].ToString().IsNullOrEmpty())
                    {
                        dataRow["DOB"] = "19200101";
                    }

                    if (dataRow["SSN"].ToString().Contains("-"))
                    {
                        dataRow["SSN"] = dataRow["SSN"].ToString().Replace("-", String.Empty);
                    }

                    if (dataRow["Employee #"].ToString().IsNullOrEmpty())
                    {

                        dataRow["Employee #"] = ConverterHelper.BuildDefaultEmployeeNumber(
                                Branding.ProductName,
                                dataRow["First_Name"].ToString(),
                                dataRow["Last_Name"].ToString(),
                                dataRow["SSN"].ToString()
                            );

                    }

                }

                filteredDataTable.WriteOutCsv(filteredFilename);

            }

        }

        public void WriteoutCoverageFiles(
                employer theEmployer,
                IList<int> planYearIds,
                DataTable dataTable,
                String destinationFileNameOffer,
                String destinationFileNameCarrier,
                Boolean writeCarrierFile
            )
        {

            DataTable offerDataTable = dataTable.Copy();
            DataTable carrierDataTable = dataTable.Copy();

            Boolean writeOfferFiles = planYearIds.Count() > 0;

            int planYearId = planYearIds.Single();

            if (writeCarrierFile)
            {

                IList<DataRow> waivedCoveragesToBeDeleted = new List<DataRow>();
                foreach (DataRow dataRow in carrierDataTable.Rows)
                {

                    if (dataRow["EnrollStatus"].ToString().ToUpper() == "W")
                    {
                        waivedCoveragesToBeDeleted.Add(dataRow);
                    }

                }

                carrierDataTable.Rows.RemoveRows(waivedCoveragesToBeDeleted);

                carrierDataTable.RenameColumn("Subid", "OldSUBID");
                carrierDataTable.RenameColumn("OldSUBID", "SUBID");

                carrierDataTable.RenameColumn("Last Name", "OldLastName");
                carrierDataTable.RenameColumn("OldLastName", "LAST NAME");

                carrierDataTable.RenameColumn("First Name", "OldFirstName");
                carrierDataTable.RenameColumn("OldFirstName", "FIRST NAME");

                carrierDataTable.PruneToRequiredColumns(
                        "SUBID",
                        "MEMBER",
                        "SSN",
                        "LAST NAME",
                        "FIRST NAME",
                        "DOB",
                        "JAN",
                        "FEB",
                        "MAR",
                        "APR",
                        "MAY",
                        "JUN",
                        "JUL",
                        "AUG",
                        "SEP",
                        "OCT",
                        "NOV",
                        "DEC",
                        "Total"
                    );

                String missingColumns = String.Empty;
                if (carrierDataTable.VerifyContainsColumns(
                        out missingColumns,
                        "SUBID",
                        "MEMBER",
                        "SSN",
                        "LAST NAME",
                        "FIRST NAME",
                        "DOB",
                        "JAN",
                        "FEB",
                        "MAR",
                        "APR",
                        "MAY",
                        "JUN",
                        "JUL",
                        "AUG",
                        "SEP",
                        "OCT",
                        "NOV",
                        "DEC",
                        "TOTAL"
                    ) == false)
                {

                    String detailedErrorMesage = String.Format("Legacy Coverage file is missing required columns for carrier: {0}", missingColumns);

                    this.Log.Error(detailedErrorMesage);

                    throw new Exception(detailedErrorMesage);

                }

                carrierDataTable.ReorderColumns(
                        "SUBID",
                        "MEMBER",
                        "SSN",
                        "LAST NAME",
                        "FIRST NAME",
                        "DOB",
                        "JAN",
                        "FEB",
                        "MAR",
                        "APR",
                        "MAY",
                        "JUN",
                        "JUL",
                        "AUG",
                        "SEP",
                        "OCT",
                        "NOV",
                        "DEC",
                        "Total"
                    );

            }

            if (writeOfferFiles)
            {

                IList<DataRow> dependentsToBeDeleted = new List<DataRow>();
                foreach (DataRow dataRow in offerDataTable.Rows)
                {

                    if (dataRow["InsuredMember"].ToString().Trim().ToUpper() == "D")
                    {
                        dependentsToBeDeleted.Add(dataRow);
                    }

                }

                offerDataTable.Rows.RemoveRows(dependentsToBeDeleted);

            }

            Boolean wroteCarrierFile = false;
            Boolean wroteOfferFile = false;

            try
            {
                if (writeCarrierFile)
                {

                    WriteoutCsvFile(carrierDataTable, destinationFileNameCarrier);

                    wroteCarrierFile = true;

                }

                if (writeOfferFiles)
                {

                    DataTable filledInOffer = FillinOfferFile(theEmployer, planYearId, offerDataTable);

                    WriteoutOfferFile(filledInOffer, destinationFileNameOffer);

                    wroteOfferFile = true;

                }
            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during converting coverage to carrier and/or offer files, removing files.", exception);

                try
                {

                    if (wroteCarrierFile) { File.Delete(destinationFileNameCarrier); }

                    if (wroteOfferFile) { File.Delete(destinationFileNameOffer); }

                }
                catch (Exception fileDeleteException)
                {

                    this.Log.Error(String.Format("Errors removing files, You must manually delete {0} and {1}.", destinationFileNameCarrier, destinationFileNameOffer), fileDeleteException);

                    throw;

                }

                throw;

            }

        }

        public DataTable FilterOfferFile(
                DataTable dataTable,
                String typeOfEvent
            )
        {

            DataTable filteredVersion = dataTable.Copy();

            IList<DataRow> doesNotMatchFilter = new List<DataRow>();
            foreach (DataRow dataRow in filteredVersion.Rows)
            {

                if (dataRow["TYPE_OF_EVENT"].ToString().Trim().ToUpper().Equals(typeOfEvent) == false)
                {
                    doesNotMatchFilter.Add(dataRow);
                }

            }

            filteredVersion.Rows.RemoveRows(doesNotMatchFilter);

            return filteredVersion;

        }


        public void WriteoutOfferFile(
                DataTable dataTable,
                String destinationFileName
            )
        {

            DataTable simpleOffers = FilterOfferFile(dataTable, "SIMPLE-OFFER");
            DataTable changeEvents = FilterOfferFile(dataTable, "INSURANCE-CHANGE");
            DataTable rejectedOffers = FilterOfferFile(dataTable, "OFFER-FILE-REJECTION");

            Boolean writeOutSimpleOfferFile = simpleOffers.Rows.Count > 0;
            Boolean writeOutChangeEventsFile = changeEvents.Rows.Count > 0;
            Boolean writeOutRefectedOffersFile = rejectedOffers.Rows.Count > 0;

            String simpleOfferFilename = destinationFileName;
            String insuranceChangeEventsFilename = destinationFileName.Replace("insoffer", "InsuranceChangeEvent");
            String insuranceDiscrepancyFilename = destinationFileName.Replace("insoffer", "InsuranceDiscrepancy");

            Boolean wroteOutSimpleOfferFile = false;
            Boolean wroteOutChangeEventsFile = false;
            Boolean wrouteOutRejectedOffers = false;

            try
            {

                if (writeOutSimpleOfferFile)
                {

                    simpleOffers.RenameColumn("OFFER_ROW_ID", "ROW ID");
                    simpleOffers.RenameColumn("OFFER_EMPLOYEE_ID", "EMPLOYEE ID");
                    simpleOffers.RenameColumn("OFFER_EMPLOYER_ID", "EMPLOYER ID");
                    simpleOffers.RenameColumn("OFFER_PLANYEAR_ID", "PLAN YEAR ID");
                    simpleOffers.RenameColumn("OFFER_EMPLOYEE_#", "PAYROLL ID");
                    simpleOffers.RenameColumn("OFFER_NAME", "NAME");
                    simpleOffers.RenameColumn("OFFER_CLASS_ID", "CLASS ID");
                    simpleOffers.RenameColumn("OFFER_AVERAGE_HOURS", "AVG HOURS");
                    simpleOffers.RenameColumn("OFFER_OFFERED", "OFFERED");
                    simpleOffers.RenameColumn("OFFER_OFFERED_ON", "OFFERED ON");
                    simpleOffers.RenameColumn("OFFER_ACCEPTED", "ACCEPTED");
                    simpleOffers.RenameColumn("OFFER_ACCEPTED_ON", "ACCEPTED/DECLINED ON");
                    simpleOffers.RenameColumn("OFFER_INSURANCE_ID", "INSURANCE ID");
                    simpleOffers.RenameColumn("OFFER_CONTRIBUTION_ID", "CONTRIBUTION ID");
                    simpleOffers.RenameColumn("OFFER_EFFECTIVE_DATE", "EFFECTIVE DATE");
                    simpleOffers.RenameColumn("OFFER_HRA_FLEX", "HRA-Flex");

                    simpleOffers.PruneToRequiredColumns(
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
                        "HRA-Flex"
                    );

                    String missingColumns = String.Empty;
                    if (simpleOffers.VerifyContainsColumns(
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
                            "HRA-Flex"
                        ) == false)
                    {

                        String detailedErrorMesage = String.Format("File is missing required columns for offer: {0}", missingColumns);

                        this.Log.Error(detailedErrorMesage);

                        throw new Exception(detailedErrorMesage);

                    }

                    simpleOffers.ReorderColumns(
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
                            "HRA-Flex"
                        );

                    WriteoutCsvFile(simpleOffers, simpleOfferFilename);

                    wroteOutSimpleOfferFile = true;

                }

                if (writeOutChangeEventsFile)
                {

                    changeEvents.RenameColumn("OFFER_ROW_ID", "ROW ID");
                    changeEvents.RenameColumn("OFFER_EMPLOYEE_ID", "EMPLOYEE ID");
                    changeEvents.RenameColumn("OFFER_EMPLOYER_ID", "EMPLOYER ID");
                    changeEvents.RenameColumn("OFFER_PLANYEAR_ID", "PLAN YEAR ID");
                    changeEvents.RenameColumn("OFFER_EMPLOYEE_#", "PAYROLL ID");
                    changeEvents.RenameColumn("OFFER_NAME", "NAME");
                    changeEvents.RenameColumn("OFFER_CLASS_ID", "CLASS ID");
                    changeEvents.RenameColumn("OFFER_AVERAGE_HOURS", "AVG HOURS");
                    changeEvents.RenameColumn("OFFER_OFFERED", "OFFERED");
                    changeEvents.RenameColumn("OFFER_OFFERED_ON", "OFFERED ON");
                    changeEvents.RenameColumn("OFFER_ACCEPTED", "ACCEPTED");
                    changeEvents.RenameColumn("OFFER_ACCEPTED_ON", "ACCEPTED/DECLINED ON");
                    changeEvents.RenameColumn("OFFER_INSURANCE_ID", "INSURANCE ID");
                    changeEvents.RenameColumn("OFFER_CONTRIBUTION_ID", "CONTRIBUTION ID");
                    changeEvents.RenameColumn("OFFER_EFFECTIVE_DATE", "EFFECTIVE DATE");
                    changeEvents.RenameColumn("OFFER_HRA_FLEX", "HRA-Flex");

                    changeEvents.PruneToRequiredColumns(
                        "EVENT_ROW_ID",
                        "TYPE_OF_EVENT",
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
                        "HRA-Flex"
                    );

                    String missingColumns = String.Empty;
                    if (changeEvents.VerifyContainsColumns(
                            out missingColumns,
                            "EVENT_ROW_ID",
                            "TYPE_OF_EVENT",
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
                            "HRA-Flex"
                        ) == false)
                    {

                        String detailedErrorMesage = String.Format("File is missing required columns for offer: {0}", missingColumns);

                        this.Log.Error(detailedErrorMesage);

                        throw new Exception(detailedErrorMesage);

                    }

                    changeEvents.ReorderColumns(
                            "EVENT_ROW_ID",
                            "TYPE_OF_EVENT",
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
                            "HRA-Flex"
                        );

                    WriteoutCsvFile(changeEvents, insuranceChangeEventsFilename);

                    wroteOutChangeEventsFile = true;

                }

                if (writeOutRefectedOffersFile)
                {

                    WriteoutCsvFile(rejectedOffers, insuranceDiscrepancyFilename);

                    wrouteOutRejectedOffers = true;

                }
            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during converting offer to offer, change events and/or rejected offer files, removing files.", exception);

                try
                {

                    if (wrouteOutRejectedOffers) { File.Delete(insuranceDiscrepancyFilename); }

                    if (wroteOutChangeEventsFile) { File.Delete(insuranceChangeEventsFilename); }

                    if (wroteOutSimpleOfferFile) { File.Delete(simpleOfferFilename); }

                }
                catch (Exception fileDeleteException)
                {

                    this.Log.Error(String.Format("Errors removing files, You must manually delete {0} and {1} and {2}.", insuranceDiscrepancyFilename, insuranceChangeEventsFilename, simpleOfferFilename), fileDeleteException);

                    throw;

                }

                throw;

            }

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

        private ILog Log = LogManager.GetLogger(typeof(LegacyConverter));

    }

}