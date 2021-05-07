using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI.Code.AFcomply.DataUpload
{
    public class OfferDataTableImporter
    {
        private static ILog Log = LogManager.GetLogger(typeof(OfferDataTableImporter));

        private static string[] RequiredColumns = { "FEIN", "SSN", "OFFERED", "ACCEPTED", "EFFECTIVE DATE", "MEDICAL PLAN" };

        private static string[] OptionalColumns = { "FULL NAME", "STABILITY PERIOD", "HRA - FLEX", "CLASSIFICATION", "AVG HOURS", "HIRE DATE", "TERM DATE", "EXT ID", "CONTRIBUTION" };

        public IList<ValidationFailure> DataValidationMessages { get; protected set; }

        public DataTable ImportProcessedData(DataTable importData, int employerId, int planYearId, string userId)
        {
            DataValidationMessages = new List<ValidationFailure>();

            EmployerId = employerId;

            UserId = userId;

            PlanYear = (from PlanYear year in PlanYear_Controller.getEmployerPlanYear(EmployerId) where year.PLAN_YEAR_ID == planYearId select year).Single();

            Insurances = insuranceController.manufactureInsuranceList(PlanYear.PLAN_YEAR_ID);

            if (Insurances == null || Insurances.Count <= 0)
            {
                var failure = new Afas.Domain.ValidationFailure();
                failure.Identifier = "Plan Year ID: " + planYearId;
                failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Dataset;
                failure.ValidationType = "Insurances";
                failure.ValidationMessage = "No Insurances found for Plan Year.";
                DataValidationMessages.Add(failure);
            }

            if (HasRequiredColumnNames(importData))
            {
                DataTable FailedImportData = new DataTable();
                CopyColumns(importData, FailedImportData);

                DataTable ProcessedItems = PlanYear_Controller.GetNewImportOfferDataTable();

                AllEmployees = EmployeeController.manufactureEmployeeList(EmployerId);

                foreach (DataRow row in importData.Rows)
                {
                    if (false == ProcessRow(row, ProcessedItems))
                    {
                        FailedImportData.Rows.Add(row.ItemArray);
                    }
                }

                if (ProcessedItems.Rows.Count > 0)
                {
                    bool success = insuranceController.BulkInsertOfferAndChange(ProcessedItems);
                    if (success == false)
                    {
                        FailedImportData = importData;

                        var failure = new Afas.Domain.ValidationFailure();
                        failure.Identifier = "";
                        failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Dataset;
                        failure.ValidationType = "DataBase Falure";
                        failure.ValidationMessage = "Database Failed to save new values.";
                        DataValidationMessages.Add(failure);

                    }
                }

                return FailedImportData;
            }
            else
            {
                Log.Error("Attempted to process Data Table that was lacking required Columns.");

                var failure = new Afas.Domain.ValidationFailure();
                failure.Identifier = "";
                failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Dataset;
                failure.ValidationType = "Required Columns";
                failure.ValidationMessage = "Import did not have all the required columns.";
                DataValidationMessages.Add(failure);

                return importData;
            }

        }

        private List<Employee> AllEmployees;

        private List<insurance> Insurances;

        private string UserId;

        private int EmployerId;

        private PlanYear PlanYear;

        private bool ProcessRow(DataRow row, DataTable ProcessedEmployees)
        {
            try
            {
                DataRow processedRow = ProcessedEmployees.NewRow();

                if (row.IsRowBlank())
                {
                    return true;       
                }

                string lineNumber = "[" + (row.Table.Rows.IndexOf(row) + 2).ToString() + "] - ";

                Employee Employee;
                try
                {
                    Employee = AllEmployees.Find(emp => emp.Employee_SSN_Visible == row["SSN"].ToString().ZeroPadSsn());
                    if (null == Employee)
                    {
                        Log.Info("Couldn't find employee, probably a SSN mismatch or typo.");

                        var failure = new Afas.Domain.ValidationFailure();
                        failure.Identifier = lineNumber + row["SSN"].ToString().ZeroPadSsn();
                        failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                        failure.ValidationType = "Employee Not Found";
                        failure.ValidationMessage = "Could not find employee by SSN.";
                        DataValidationMessages.Add(failure);

                        return false;
                    }
                }
                catch (Exception e)
                {
                    Log.Warn("Exception while trying to find employee.", e);

                    var failure = new Afas.Domain.ValidationFailure();
                    failure.Identifier = lineNumber + row["SSN"].ToString();
                    failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                    failure.ValidationType = "Employee Not Found";
                    failure.ValidationMessage = "Could not find employee by SSN.";
                    DataValidationMessages.Add(failure);

                    return false;
                }
                processedRow["employee_id"] = Employee.EMPLOYEE_ID.ToString().Trim().checkForDBNull();


                if (row.Table.Columns.Contains("STABILITY PERIOD") &&
                    PlanYear.PLAN_YEAR_ID.ToString() != row["STABILITY PERIOD"].ToString() && PlanYear.PLAN_YEAR_DESCRIPTION != row["STABILITY PERIOD"].ToString())
                {
                    Log.Info("Plan Year's did not match, Selected Plan Year : " + PlanYear.PLAN_YEAR_DESCRIPTION + " plan Year from Data: " + row["STABILITY PERIOD"].ToString());

                    var failure = new Afas.Domain.ValidationFailure();
                    failure.Identifier = lineNumber + row["STABILITY PERIOD"].ToString();
                    failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                    failure.ValidationType = "Wronge Stability";
                    failure.ValidationMessage = "Stability period did not match selected [" + PlanYear.PLAN_YEAR_DESCRIPTION + "].";
                    DataValidationMessages.Add(failure);

                    return false;
                }
                processedRow["plan_year_id"] = PlanYear.PLAN_YEAR_ID.ToString().Trim().checkForDBNull();
                processedRow["employer_id"] = EmployerId.ToString().Trim().checkForDBNull();

                if (row["OFFERED"] == null || row["OFFERED"].convertYesAndNoToBool() == null)
                {
                    var failure = new Afas.Domain.ValidationFailure();
                    failure.Identifier = lineNumber + Employee.EMPLOYEE_FULL_NAME;
                    failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                    failure.ValidationType = "No Offered Value";
                    failure.ValidationMessage = "No Offered value provided for [" + Employee.EMPLOYEE_FULL_NAME + "].";
                    DataValidationMessages.Add(failure);

                    return false;
                }

                if (row["EFFECTIVE DATE"] == null || row["EFFECTIVE DATE"].checkDateDBNull() == DBNull.Value)
                {
                    var failure = new Afas.Domain.ValidationFailure();
                    failure.Identifier = lineNumber + Employee.EMPLOYEE_FULL_NAME;
                    failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                    failure.ValidationType = "No Effective Date";
                    failure.ValidationMessage = "No Effective Date provided for [" + Employee.EMPLOYEE_FULL_NAME + "].";
                    DataValidationMessages.Add(failure);

                    return false;
                }

                if ((DateTime)row["EFFECTIVE DATE"].checkDateDBNull() <= Employee.EMPLOYEE_HIRE_DATE.Value.AddDays(-1))
                {
                    var failure = new Afas.Domain.ValidationFailure();
                    failure.Identifier = lineNumber + Employee.EMPLOYEE_FULL_NAME;
                    failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                    failure.ValidationType = "Effective before Hire Date";
                    failure.ValidationMessage = "Effective Date was before system hire date of [" + Employee.EMPLOYEE_HIRE_DATE + "].";
                    DataValidationMessages.Add(failure);

                    return false;
                }

                if ((true == row["OFFERED"].convertYesAndNoToBool()) && (row["ACCEPTED"] == null || row["ACCEPTED"].convertYesAndNoToBool() == null))
                {
                    var failure = new Afas.Domain.ValidationFailure();
                    failure.Identifier = lineNumber + Employee.EMPLOYEE_FULL_NAME;
                    failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                    failure.ValidationType = "No Accepted Value";
                    failure.ValidationMessage = "No Accepted Value provided for [" + Employee.EMPLOYEE_FULL_NAME + "] who ws offered.";
                    DataValidationMessages.Add(failure);

                    return false;
                }


                if ((false == row["OFFERED"].convertYesAndNoToBool()) && (row["ACCEPTED"] != null && (row["ACCEPTED"].convertYesAndNoToBool() == true)))
                {
                    var failure = new Afas.Domain.ValidationFailure();
                    failure.Identifier = lineNumber + Employee.EMPLOYEE_FULL_NAME;
                    failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                    failure.ValidationType = "Insurance is not offered But it was accepted";
                    failure.ValidationMessage = " Accepted Value provided for [" + Employee.EMPLOYEE_FULL_NAME + "] who was not offered .";
                    DataValidationMessages.Add(failure);

                    return false;
                }



                if (row.Table.Columns.Contains("STABILITY PERIOD") &&
                    ProcessedEmployees.Select(string.Format("plan_year_id = {0} AND employee_id = {1} AND effectiveDate = #{2}#", processedRow["plan_year_id"], Employee.EMPLOYEE_ID.ToString().Trim().checkForDBNull(), row["EFFECTIVE DATE"]))
                    .Count() > 0)
                {
                    Log.Debug("Row already loaded, Skipping.");
                    return true;
                }
                if (ProcessedEmployees.Select(string.Format("employee_id = {0} AND effectiveDate = #{1}#", Employee.EMPLOYEE_ID.ToString().Trim().checkForDBNull(), row["EFFECTIVE DATE"]))
                    .Count() > 0)
                {
                    Log.Debug("Row already loaded, Skipping.");
                    return true;
                }

                processedRow["offered"] = row["OFFERED"].convertYesAndNoToBool().checkForDBNull();
                processedRow["offeredOn"] = row["EFFECTIVE DATE"].checkDateDBNull();
                processedRow["accepted"] = row["ACCEPTED"].convertYesAndNoToBool().checkForDBNull();
                processedRow["acceptedOn"] = row["EFFECTIVE DATE"].checkDateDBNull();
                processedRow["effectiveDate"] = row["EFFECTIVE DATE"].checkDateDBNull();

                if (true == row["OFFERED"].convertYesAndNoToBool())
                {
                    insurance insurance = null;
                    if (Insurances.Count == 1 && (row["MEDICAL PLAN"] == null || row["MEDICAL PLAN"] == DBNull.Value || row["MEDICAL PLAN"].ToString() == string.Empty))
                    {
                        insurance = Insurances.Single();
                    }
                    else
                    {
                        insurance = (from insurance ins in Insurances where ins.INSURANCE_NAME == row["MEDICAL PLAN"].ToString() select ins).SingleOrDefault();
                    }

                    if (null != insurance)
                    {
                        processedRow["insurance_id"] = insurance.INSURANCE_ID;

                        List<insuranceContribution> contributions = insuranceController.manufactureInsuranceContributionList(insurance.INSURANCE_ID);      
                        insuranceContribution contribution = contributions.FilterForEmployeeClassByEmployeeClassId(Employee.EMPLOYEE_CLASS_ID).SingleOrDefault();

                        if (null != contribution)
                        {
                            processedRow["ins_cont_id"] = contribution.INS_CONT_ID;
                        }
                        else
                        {
                            Log.Debug("Couldn't find Ins Contribution for: InsId: " + insurance.INSURANCE_ID + " and EE Class: " + Employee.EMPLOYEE_CLASS_ID);

                            var failure = new Afas.Domain.ValidationFailure();
                            failure.Identifier = lineNumber + insurance.INSURANCE_NAME;
                            failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                            failure.ValidationType = "No Ins Contribution";
                            failure.ValidationMessage = "Did not find insureance Contribution for [" + insurance.INSURANCE_NAME + "] for the employee class.";
                            DataValidationMessages.Add(failure);

                            return false;
                        }
                    }
                    else
                    {
                        Log.Debug("Couldn't find insurance with Name [" + row["MEDICAL PLAN"].ToString() + "] for plan year: " + PlanYear.PLAN_YEAR_DESCRIPTION);

                        var failure = new Afas.Domain.ValidationFailure();
                        failure.Identifier = lineNumber + row["MEDICAL PLAN"].ToString();
                        failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                        failure.ValidationType = "Insurance Not Found";
                        failure.ValidationMessage = "Did not find insureance with name [" + row["MEDICAL PLAN"].ToString() + "].";
                        DataValidationMessages.Add(failure);

                        return false;
                    }
                }


                double flex = 0.0;
                if (row.Table.Columns.Contains("HRA - FLEX") && row["HRA - FLEX"] != null)
                {
                    double.TryParse(row["HRA - FLEX"].ToString().Trim(), out flex);
                }
                processedRow["hra_flex_contribution"] = flex.ToString().checkDecimalDBNull2();

                processedRow["modOn"] = DateTime.Now;
                processedRow["modBy"] = UserId;
                processedRow["notes"] = "Imported On: " + DateTime.Now.ToShortDateString();
                processedRow["history"] = "Imported On: " + DateTime.Now.ToShortDateString();

                ProcessedEmployees.Rows.Add(processedRow);
            }
            catch (Exception ex)
            {
                Log.Warn("Exception while trying to parse Offer data.", ex);

                var failure = new Afas.Domain.ValidationFailure();
                failure.Identifier = "[" + (row.Table.Rows.IndexOf(row) + 2).ToString() + "]";
                failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                failure.ValidationType = "Unknown Error";
                failure.ValidationMessage = "Please contact IT: [" + ex.Message + "].";
                DataValidationMessages.Add(failure);

                return false;
            }

            return true;
        }

        private void CopyColumns(DataTable copyFrom, DataTable copyTo)
        {
            foreach (DataColumn column in copyFrom.Columns)
            {
                copyTo.Columns.Add(column.ColumnName);
            }
        }

        private bool HasRequiredColumnNames(DataTable importData)
        {
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
                    var failure = new Afas.Domain.ValidationFailure();
                    failure.Identifier = required;
                    failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Dataset;
                    failure.ValidationType = "Missing Column";
                    failure.ValidationMessage = "Requires Column named: [" + required + "]";
                    DataValidationMessages.Add(failure);

                    return false;
                }
            }

            return true;
        }
    }
}