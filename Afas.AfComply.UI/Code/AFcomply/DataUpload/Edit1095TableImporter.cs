using Afas.AfComply.Domain;
using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Application.Services.LegacyServices;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Domain.LegacyData;
using Afas.AfComply.Reporting.Domain.MonthlyDetails;
using Afas.AfComply.UI.App_Start;
using Afas.Domain;
using Afc.Core.Application;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Afas.AfComply.UI.Code.AFcomply.DataUpload
{
    public class Edit1095TableImporter
    {
        private static ILog Log = LogManager.GetLogger(typeof(OfferDataTableImporter));

        private static readonly string[] RequiredColumns = { "FEIN", "SSN", "Tax Year", "Month", "Line 14", "Line 15", "Line 16" };

        private static readonly string[] OptionalColumns = { "Receiving 1095", "Measured FT", "Reviewed", "Name", "Hire Date", "Term Date", "Insurance Type", "Status", "Offered", "Enrolled", "Monthly Average Hours" };

        public IList<ValidationFailure> DataValidationMessages { get; protected set; }

        private string UserId;

        private int EmployerId;

        private int TaxYearId;

        private int CriticalIssueCount;


        public DataTable ImportProcessedData(DataTable importData, int employerId, int taxYearId, string userId, ITransactionContext transactionContext)
        {

            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(Edit1095TableImporter), "ImportProcessedData", SystemSettings.UsePerformanceLog))
            {

                methodTimer.StartTimer("Setup and Startup");

                Log.Info("Begin Importing data with total count: " + importData.Rows.Count);

                this.DataValidationMessages = new List<ValidationFailure>();
                this.EmployerId = employerId;
                this.UserId = userId;
                this.TaxYearId = taxYearId;
                this.CriticalIssueCount = 0;

                if (false == this.HasRequiredColumnNames(importData))
                {
                    Log.Error("Attempted to process Data Table that was lacking required Columns.");
                    return importData;
                }


                methodTimer.StartTimer("Setup Directly from DI");

                IUserEditPart2Service UserEditPart2Service = ContainerActivator._container.Resolve<IUserEditPart2Service>();
                UserEditPart2Service.Context = transactionContext;
                IUserReviewedService UserReviewedService = ContainerActivator._container.Resolve<IUserReviewedService>();
                UserReviewedService.Context = transactionContext;
                IFinalize1095Service Finalize1095Service = ContainerActivator._container.Resolve<IFinalize1095Service>();
                Finalize1095Service.Context = transactionContext;
                ILegacyEmployeeService legacy = ContainerActivator._container.Resolve<ILegacyEmployeeService>();

                methodTimer.LogTimeAndDispose("Setup Directly from DI");

                methodTimer.StartTimer("Create SSN Lookup");
                methodTimer.StartTimer("Employee SSN Add to Lookup");

                DataTable FailedImportData = new DataTable();
                this.CopyColumns(importData, FailedImportData);

                Dictionary<string, int> SsnToEmployeeIdLookUp = new Dictionary<string, int>();
                foreach (employee Employee in legacy.GetEmployeesForEmployer(this.EmployerId))
                {
                    if (SsnToEmployeeIdLookUp.ContainsKey(Employee.SSN.RemoveDashes().ZeroPadSsn()))
                    {
                        Log.Warn(string.Format("Import of 1095 Edits Encountered Duplicate SSN for Employee Id: [{0}] and Employee Id: [{1}] ", SsnToEmployeeIdLookUp[Employee.SSN.RemoveDashes().ZeroPadSsn()], Employee.employee_id));

                        this.CriticalIssueCount++;

                        ValidationFailure failure = new Afas.Domain.ValidationFailure
                        {
                            Identifier = string.Format("EE_Id: [{0}], [{1}]", SsnToEmployeeIdLookUp[Employee.SSN.RemoveDashes().ZeroPadSsn()], Employee.employee_id),
                            ValidationSeverity = Afas.Domain.ValidationSeverity.Value,
                            ValidationType = "Duplicate SSN In DataBase",
                            ValidationMessage = "Data Error: Multiple Employees have the same SSN In the Database."
                        };
                        this.DataValidationMessages.Add(failure);


                        SsnToEmployeeIdLookUp.Remove(Employee.SSN.RemoveDashes().ZeroPadSsn());

                        continue;
                    }
                    else
                    {
                        SsnToEmployeeIdLookUp.Add(Employee.SSN.RemoveDashes().ZeroPadSsn(), Employee.employee_id);
                    }

                    methodTimer.Lap("Employee SSN Add to Lookup");

                }

                methodTimer.LogTimeAndDispose("Create SSN Lookup");
                methodTimer.LogAllLapsAndDispose("Employee SSN Add to Lookup");
                methodTimer.LogTimeAndDispose("Setup and Startup");
                
                methodTimer.StartTimer("Process all Import Data Rows");
                methodTimer.StartLapTimerPaused("PreProcess Row");
                methodTimer.StartLapTimerPaused("Process Edits and Reviewed");

                List<Employee1095detailsPart2Model> ProcessedUserEdits = new List<Employee1095detailsPart2Model>();

                Dictionary<int, bool?> ProcessedUserReviewedBool = new Dictionary<int, bool?>();

                int? lastEmployeeIdProcessed = null;

                foreach (DataRow row in importData.Rows)
                {

                    if (row.IsRowBlank())
                    {
                        continue;
                    }

                    methodTimer.ResumeLap("PreProcess Row");

                    if (false == this.ValidateRowValues(row))
                    {
                        FailedImportData.Rows.Add(row.ItemArray);

                        continue;
                    }

                    string rowSsn = row["SSN"].ToString().RemoveDashes().ZeroPadSsn();

                    int? EmployeeIdForRow = null;
                    if (SsnToEmployeeIdLookUp.ContainsKey(rowSsn))
                    {
                        EmployeeIdForRow = SsnToEmployeeIdLookUp[rowSsn];
                    }

                    if (null == EmployeeIdForRow)
                    {

                        int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                        Log.Debug(string.Format("Import of 1095 Edits Encountered an Unknown SSN on row: [{0}]", rowNumber));

                        ValidationFailure failure = new Afas.Domain.ValidationFailure
                        {
                            Identifier = string.Format("Line #: [{0}]", rowNumber),
                            ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                            ValidationType = "Unknown SSN Provided",
                            ValidationMessage = string.Format("Data Error: Could not locate Employee by provided SSN on Line #:[{0}].", rowNumber)
                        };
                        this.DataValidationMessages.Add(failure);

                        FailedImportData.Rows.Add(row.ItemArray);

                        continue;
                    }

                    if (lastEmployeeIdProcessed == null
                        || lastEmployeeIdProcessed != EmployeeIdForRow)
                    {
                        lastEmployeeIdProcessed = EmployeeIdForRow;

                        if (false == ProcessedUserReviewedBool.ContainsKey(lastEmployeeIdProcessed.Value))
                        {
                            ProcessedUserReviewedBool.Add(lastEmployeeIdProcessed.Value, null);
                        }

                    }

                    methodTimer.Lap("PreProcess Row");
                    methodTimer.PauseLap("PreProcess Row");

                    methodTimer.ResumeLap("Process Edits and Reviewed");

                    Employee1095detailsPart2Model RowsProcessedEdit = null;
                    bool? RowsProcessedReviewed = null;

                    if (lastEmployeeIdProcessed == null
                        || false == this.ProcessUserEditsRow(row, lastEmployeeIdProcessed.Value, out RowsProcessedEdit)
                        || false == this.ProcessUserReviewedRow(row, lastEmployeeIdProcessed.Value, out RowsProcessedReviewed)
                        || RowsProcessedEdit == null)
                    {
                        FailedImportData.Rows.Add(row.ItemArray);

                        methodTimer.Lap("Process Edits and Reviewed");
                        methodTimer.PauseLap("Process Edits and Reviewed");


                        continue;
                    }
                    else
                    {
                        ProcessedUserEdits.Add(RowsProcessedEdit);

                        if (null != RowsProcessedReviewed
                            && null == ProcessedUserReviewedBool[lastEmployeeIdProcessed.Value])
                        {
                            ProcessedUserReviewedBool[lastEmployeeIdProcessed.Value] = RowsProcessedReviewed;

                        }
                        else if (null != RowsProcessedReviewed
                            && null != ProcessedUserReviewedBool[lastEmployeeIdProcessed.Value]
                            && RowsProcessedReviewed != ProcessedUserReviewedBool[lastEmployeeIdProcessed.Value])
                        {
                            int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                            Log.Debug(string.Format("Import of 1095 Edits Encountered a confilicting Reviewed Value on row: [{0}], line's value: [{1}] Previous loaded value: [{2}]", rowNumber, RowsProcessedReviewed, ProcessedUserReviewedBool[lastEmployeeIdProcessed.Value]));

                            ValidationFailure failure = new Afas.Domain.ValidationFailure
                            {
                                Identifier = string.Format("Line #: [{0}]", rowNumber),
                                ValidationSeverity = Afas.Domain.ValidationSeverity.Value,
                                ValidationType = "Conflicting Reviewed Value",
                                ValidationMessage = string.Format("Data Error: Reviewed Value [{1}] on line #:[{0}] did not match Reviewed Value [{2}] from previous lines for Employee. ", rowNumber, RowsProcessedReviewed, ProcessedUserReviewedBool[lastEmployeeIdProcessed.Value])
                            };
                            this.DataValidationMessages.Add(failure);

                        }

                    }

                    methodTimer.Lap("Process Edits and Reviewed");
                    methodTimer.PauseLap("Process Edits and Reviewed");

                }

                methodTimer.LogTimeAndDispose("Process all Import Data Rows");
                methodTimer.LogAllLapsAndDispose("PreProcess Row");
                methodTimer.LogAllLapsAndDispose("Process Edits and Reviewed");
                
                methodTimer.StartTimer("Saving the Procesed Edits");

                if (ProcessedUserEdits.Count > 0)
                {

                    Log.Info("Getting System Values for User Edits CSV.");

                    Dictionary<int, List<Employee1095detailsPart2Model>> allPart2AsEmployeeIdDictionary = this.GetCurrentSystemMonthValues(UserEditPart2Service);

                    Log.Info("Got System Values for User Edits CSV.");

                    Log.Info(string.Format("Starting Saving Edits To DB. Saving [{0}] Items. ", ProcessedUserEdits.Count));

                    UserEditPart2Service.UpdateWithEdits(
                        ProcessedUserEdits,
                        allPart2AsEmployeeIdDictionary,
                        this.EmployerId,
                        this.TaxYearId,
                        userId);

                    Log.Info(string.Format("Finished Saving Edits To DB. Saved [{0}] Items. ", ProcessedUserEdits.Count));

                }

                methodTimer.LogTimeAndDispose("Saving the Procesed Edits");

                methodTimer.StartTimer("Saving the Procesed Reviewed and UnReviewed");

                Log.Info("Preparing Uploaded Reviews for saving.");

                List<int> ToReview = ProcessedUserReviewedBool.Where(kvp => kvp.Value == true).Select(kvp => kvp.Key).ToList();
                List<int> ToUnReview = ProcessedUserReviewedBool.Where(kvp => kvp.Value == false).Select(kvp => kvp.Key).ToList();
                Log.Info("Finished Preparing Uploaded Reviews for saving.");

                if (ToUnReview.Count > 0)
                {

                    Log.Info(string.Format("Starting Saving UnReviewed To DB. Saving [{0}] Items. ", ToUnReview.Count));

                    UserReviewedService.MarkAsNotReviewed(ToUnReview, this.EmployerId, this.TaxYearId, userId);

                    Log.Info(string.Format("Finished Saving UnReviewed To DB. Saved [{0}] Items. ", ToUnReview.Count));

                }

                if (ToReview.Count > 0)
                {

                    Log.Info(string.Format("Starting Saving Reviewed To DB. Saving [{0}] Items. ", ToReview.Count));

                    UserReviewedService.MarkReviewed(ToReview, this.EmployerId, this.TaxYearId, userId);

                    Log.Info(string.Format("Finished Saving Reviewed To DB. Saved [{0}] Items. ", ToReview.Count));

                }

                methodTimer.LogTimeAndPause("Saving the Procesed Reviewed and UnReviewed");

                if (FailedImportData.Rows.Count > 0)
                {
                    Log.Error("Finished Importing data with failure count: " + FailedImportData.Rows.Count);
                }
                else
                {
                    Log.Warn("Finished Importing data with no failures.");
                }

                if (this.CriticalIssueCount > 0)
                {
                    Log.Error(string.Format("User Edit CSV Impotrt encountered [{0}] Critical Issues.", this.CriticalIssueCount));
                }

                return FailedImportData;

            }

        }

        private bool ValidateRowValues(DataRow row)
        {
            try
            {
                bool SsnValidates = this.ValidateRowSsnValue(row);
                bool TaxYearValidates = this.ValidateRowTaxYearValue(row);
                bool MonthIdValidates = this.ValidateRowMonthIdValue(row);
                bool Line14Validates = this.ValidateRowLine14Value(row);
                bool Line15Validates = this.ValidateRowLine15Value(row);
                bool Line16Validates = this.ValidateRowLine16Value(row);

                return SsnValidates && TaxYearValidates && MonthIdValidates && Line14Validates && Line15Validates && Line15Validates;

            }
            catch (Exception ex)
            {
                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Error("Exception while trying to Validate Row Values", ex);

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = "Line #: [" + (row.Table.Rows.IndexOf(row) + 2).ToString() + "]",
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Unknown Error",
                    ValidationMessage = "Please contact IT: [" + ex.Message + "]."
                };
                this.DataValidationMessages.Add(failure);

                return false;

            }
        }

        private bool ValidateRowSsnValue(DataRow row)
        {
            if (null == row["SSN"]
                || true == row["SSN"].ToString().IsNullOrEmpty()
                || false == row["SSN"].ToString().RemoveDashes().ZeroPadSsn().IsValidSsn())
            {

                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Debug(string.Format("Import of 1095 Edits had Invalid SSN on row: [{0}]", rowNumber));

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Invalid SSN Value",
                    ValidationMessage = string.Format("The SSN value for row [{0}] was not Valid.", rowNumber)
                };

                this.DataValidationMessages.Add(failure);

                return false;

            }

            return true;
        }

        private bool ValidateRowTaxYearValue(DataRow row)

        {
            int TaxYear = 0;
            if (null == row["Tax Year"]
                || true == row["Tax Year"].ToString().IsNullOrEmpty()
                || false == int.TryParse(row["Tax Year"].ToString(), out TaxYear)
                || TaxYear < 2000
                || TaxYear > 2050)
            {

                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Debug(string.Format("Import of 1095 Edits had Invalid Tax Year on row: [{0}]", rowNumber));

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Invalid Tax Year",
                    ValidationMessage = string.Format("The Tax Year value of [{0}] for row [{1}] was not Valid.", row["Tax Year"], rowNumber)
                };

                this.DataValidationMessages.Add(failure);

                return false;

            }

            if (TaxYear != this.TaxYearId)
            {
                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Debug(string.Format("Import of 1095 Edits had Tax Year [{0}] on row [{1}] that did not match the Files year of [{2}]", TaxYear, rowNumber, this.TaxYearId));

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Tax Year Match Failed",
                    ValidationMessage = string.Format("The Tax Year [{0}] for row [{1}] did not match the File's Tax Year [{2}].", TaxYear, rowNumber, this.TaxYearId)
                };

                this.DataValidationMessages.Add(failure);

                return false;
            }

            return true;
        }

        private bool ValidateRowMonthIdValue(DataRow row)
        {

            int MonthId = 0;
            if (row["Month"] == null
                || true == row["Month"].ToString().IsNullOrEmpty()
                || false == int.TryParse(row["Month"].ToString(), out MonthId)
                || MonthId <= 0
                || MonthId > 12)
            {
                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Debug(string.Format("Import of 1095 Edits had Invalid Month Id on row: [{0}]", rowNumber));

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Invalid Month Id Value",
                    ValidationMessage = string.Format("The Month Id value of [{0}] for row [{1}] was not Valid.", row["Month"], rowNumber)
                };
                this.DataValidationMessages.Add(failure);

                return false;

            }

            return true;
        }


        /// <summary>
        /// This is a static list of the valid codes, anything that doesn't match one of these is not valid for a line 14 code
        /// </summary>
        private static readonly string[] ValidLine14Values = { "1A", "1B", "1C", "1D", "1E", "1F", "1G", "1H", "1I", "1J", "1K" };

        private bool ValidateRowLine14Value(DataRow row)
        {
            string line14Text = (row["Line 14"] ?? string.Empty).ToString().ToUpper().Trim();

            if (line14Text.Length != 2 || false == ValidLine14Values.Contains(line14Text))
            {
                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Debug(string.Format("Import of 1095 Edits had Invalid value [{0}] for Line 14 on row [{1}].", line14Text, rowNumber));

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Invalid Line 14 Code",
                    ValidationMessage = string.Format("Import of 1095 Edits had Invalid value [{0}] for Line 14 on row [{1}].", line14Text, rowNumber)
                };

                this.DataValidationMessages.Add(failure);

                return false;
            }

            return true;
        }

        private bool ValidateRowLine15Value(DataRow row)
        {
            string line15Text = (row["Line 15"] ?? string.Empty).ToString().Trim();
            line15Text = line15Text.Replace('$', ' ').Trim();

            if (line15Text.IsNullOrEmpty())
            {
                return true;
            }

            double Line15 = 0.0;

            if (false == double.TryParse(line15Text, out Line15))
            {

                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Debug(string.Format("Import of 1095 Edits had Invalid value [{0}] for Line 15 on row [{1}].", line15Text, rowNumber));

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Invalid Tax Year",
                    ValidationMessage = string.Format("Import of 1095 Edits had Invalid value [{0}] for Line 15 on row [{1}].", line15Text, rowNumber)
                };             

                this.DataValidationMessages.Add(failure);

                return false;

            }

            return true;
        }

        /// <summary>
        /// This is a static list of the valid codes, anything that doesn't match one of these is not valid for a line 14 code
        /// </summary>
        private static readonly string[] ValidLine16Values = { "2A", "2B", "2C", "2D", "2E", "2F", "2G", "2H", "2I" };

        private bool ValidateRowLine16Value(DataRow row)
        {
            string line16Text = (row["Line 16"] ?? string.Empty).ToString().ToUpper().Trim();

            if (line16Text.IsNullOrEmpty())
            {
                return true;
            }

            if (line16Text.Length != 2 || false == ValidLine16Values.Contains(line16Text))
            {
                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Debug(string.Format("Import of 1095 Edits had Invalid value [{0}] for Line 16 on row [{1}].", line16Text, rowNumber));

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Invalid Line 16 Code",
                    ValidationMessage = string.Format("Import of 1095 Edits had Invalid value [{0}] for Line 16 on row [{1}].", line16Text, rowNumber)
                };

                this.DataValidationMessages.Add(failure);

                return false;
            }

            return true;
        }

        /// <summary>
        /// This method processes the user edits from one row of the Imported Data
        /// </summary>
        /// <param name="row">The Data to Process</param>
        /// <param name="EmployeeId">The Employee Id belonging to the Employee</param>
        /// <param name="ProcessedEdit">The Output Processed Data of the User Edit from that Row</param>
        /// <returns>Success as True (or Failure as False)</returns>
        private bool ProcessUserEditsRow(DataRow row, int EmployeeId, out Employee1095detailsPart2Model ProcessedEdit)
        {
            try
            {
                int TaxYear = int.Parse(row["Tax Year"].ToString());
                int MonthId = int.Parse(row["Month"].ToString());

                string line14Text = (row["Line 14"] ?? string.Empty).ToString().ToUpper();
                string line15Text = (row["Line 15"] ?? string.Empty).ToString().ToUpper();
                string line16Text = (row["Line 16"] ?? string.Empty).ToString().ToUpper();
                ProcessedEdit = new Employee1095detailsPart2Model
                {
                    EmployeeId = EmployeeId,
                    MonthId = MonthId,
                    TaxYear = TaxYear,
                    Line14 = line14Text,
                    Line15 = line15Text,
                    Line16 = line16Text
                };

                if (row.Table.Columns.Contains("Measured FT")
                    && null != row["Measured FT"]
                    && false == row["Measured FT"].ToString().IsNullOrEmpty())
                {

                    ProcessedEdit.Receiving1095C = null;

                    string MeasuredFullTimeText = row["Measured FT"].ToString();

                    bool parsed;
                    if (bool.TryParse(MeasuredFullTimeText, out parsed))
                    {
                        ProcessedEdit.Receiving1095C = parsed;
                    }
                    else if (MeasuredFullTimeText.Equals("YES", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ProcessedEdit.Receiving1095C = true;
                    }
                    else if (MeasuredFullTimeText.Equals("NO", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ProcessedEdit.Receiving1095C = false;
                    }

                }

            }
            catch (Exception ex)
            {

                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Error(string.Format("Exception while trying to parse Part 2 edit data on Row #:[{0}].", rowNumber), ex);

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Unknown Error",
                    ValidationMessage = "Please contact IT: [" + ex.Message + "]."
                };
                this.DataValidationMessages.Add(failure);

                ProcessedEdit = null;

                return false;

            }

            return true;
        }

        /// <summary>
        /// This method processes the user Reviews from one row of the Imported Data
        /// </summary>
        /// <param name="row">The Data to Process</param>
        /// <param name="EmployeeId">The Employee Id belonging to the Employee</param>
        /// <param name="ThisEmployeeReview">The Boolean Output Processed Data of the User Reviewed from that Row. True = Reviewed, False = UnReviewed, null = Unspecified</param>
        /// <returns>Success as True (or Failure as False)</returns>
        private bool ProcessUserReviewedRow(DataRow row, int EmployeeId, out bool? ThisEmployeeReview)
        {
            ThisEmployeeReview = null;
            try
            {
                int TaxYear = int.Parse(row["Tax Year"].ToString());
                int MonthId = int.Parse(row["Month"].ToString());

                if (row.Table.Columns.Contains("Reviewed")
                    && null != row["Reviewed"]
                    && false == row["Reviewed"].ToString().IsNullOrEmpty())
                {
                    string ReviewedText = row["Reviewed"].ToString();

                    if (bool.TryParse(ReviewedText, out bool parsed))
                    {            
                        ThisEmployeeReview = parsed;
                    }
                    else if (ReviewedText.Equals("REVIEWED", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ThisEmployeeReview = true;
                    }
                    else if (ReviewedText.Equals("YES", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ThisEmployeeReview = true;
                    }
                    else if (ReviewedText.Equals("UNREVIEWED", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ThisEmployeeReview = false;
                    }
                    else if (ReviewedText.Equals("NO", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ThisEmployeeReview = false;
                    }
                }
            }
            catch (Exception ex)
            {

                int rowNumber = (row.Table.Rows.IndexOf(row) + 2);

                Log.Error(string.Format("Exception while trying to parse Reviewed data on row [{0}]", rowNumber), ex);

                ValidationFailure failure = new Afas.Domain.ValidationFailure
                {
                    Identifier = string.Format("Line #: [{0}]", rowNumber),
                    ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                    ValidationType = "Unknown Error",
                    ValidationMessage = "Please contact IT: [" + ex.Message + "]."
                };
                this.DataValidationMessages.Add(failure);

                ThisEmployeeReview = null;

                return false;

            }

            return true;

        }

        /// <summary>
        /// This is a helper method to get and preprocess the Current System Values for all employees Months
        /// </summary>
        /// <param name="UserEditPart2Service">Current System Values for all employees and Months</param>
        private Dictionary<int, List<Employee1095detailsPart2Model>> GetCurrentSystemMonthValues(IUserEditPart2Service UserEditPart2Service)
        {

            Dictionary<int, List<Employee1095detailsPart2Model>> allPart2AsEmployeeIdDictionary = _1095MonthlyDetails.getEmployeeMonthlyDetailDic(this.EmployerId, this.TaxYearId, UserEditPart2Service);

            foreach (int EmployeeId in allPart2AsEmployeeIdDictionary.Keys)
            {

                IEnumerable<Employee1095detailsPart2Model> MonthZeroAll12 = allPart2AsEmployeeIdDictionary[EmployeeId].Where(item => item.MonthId == 0).AsEnumerable();

                if (MonthZeroAll12 != null && MonthZeroAll12.Count() > 1)
                {          

                    Log.Warn(string.Format("Import of 1095 Edits Encountered Duplicate All 12 months for Employee Id: [{0}]", EmployeeId));

                    this.CriticalIssueCount++;

                    ValidationFailure failure = new Afas.Domain.ValidationFailure
                    {
                        Identifier = EmployeeId.ToString(),
                        ValidationSeverity = Afas.Domain.ValidationSeverity.Item,
                        ValidationType = "Duplicate All12",
                        ValidationMessage = "Data Error: Multiple All12 values for empolyee"
                    };

                    this.DataValidationMessages.Add(failure);

                    continue;

                }

                Employee1095detailsPart2Model part2All12Values = MonthZeroAll12.SingleOrDefault();

                if (null != part2All12Values)
                {
                    if (false == part2All12Values.Line14.IsNullOrEmpty())
                    {
                        foreach (Employee1095detailsPart2Model update in allPart2AsEmployeeIdDictionary[EmployeeId])
                        {
                            update.Line14 = part2All12Values.Line14;
                        }
                    }
                    if (false == part2All12Values.Line15.IsNullOrEmpty())
                    {
                        foreach (Employee1095detailsPart2Model update in allPart2AsEmployeeIdDictionary[EmployeeId])
                        {
                            update.Line15 = part2All12Values.Line15;
                        }
                    }
                    if (false == part2All12Values.Line16.IsNullOrEmpty())
                    {
                        foreach (Employee1095detailsPart2Model update in allPart2AsEmployeeIdDictionary[EmployeeId])
                        {
                            update.Line16 = part2All12Values.Line16;
                        }
                    }
                    if (true == part2All12Values.Receiving1095C)
                    {
                        foreach (Employee1095detailsPart2Model update in allPart2AsEmployeeIdDictionary[EmployeeId])
                        {
                            update.Receiving1095C = part2All12Values.Receiving1095C;
                        }
                    }

                    continue;
                }

            }

            return allPart2AsEmployeeIdDictionary;

        }

        /// <summary>
        /// A simple helper method that Copies the columns from one tabl to another
        /// </summary>
        /// <param name="copyFrom">The Table to copy the columns from</param>
        /// <param name="copyTo">The table to copy the columns to</param>
        private void CopyColumns(DataTable copyFrom, DataTable copyTo)
        {
         

            foreach (DataColumn column in copyFrom.Columns)
            {
                copyTo.Columns.Add(column.ColumnName);
            }
        }

        /// <summary>
        /// A simple helper method that checks if this has the required Columns by name.
        /// </summary>
        /// <param name="importData">The data table to check for the required columns</param>
        /// <returns>True if all the required columns are present or false if one or more is missing.</returns>
        private bool HasRequiredColumnNames(DataTable importData)
        {
            bool hasRequired = true;

            foreach (string required in RequiredColumns)
            {

                bool found = false;
                foreach (DataColumn column in importData.Columns)
                {
                    if (required.Equals(column.ColumnName))
                    {
                        found = true;
                        break;
                    }
                }

                if (false == found)
                {

                    ValidationFailure failure = new Afas.Domain.ValidationFailure
                    {
                        Identifier = required,
                        ValidationSeverity = Afas.Domain.ValidationSeverity.Dataset,
                        ValidationType = "Missing Column",
                        ValidationMessage = "Requires Column named: [" + required + "]"
                    };

                    this.DataValidationMessages.Add(failure);

                    hasRequired = false;

                }

            }

            return hasRequired;
        }

    }

}