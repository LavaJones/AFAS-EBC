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
    public class DemographicDataTableImporter
    {
        private static ILog Log = LogManager.GetLogger(typeof(DemographicDataTableImporter));

        private static string[] RequiredColumns = { "First Name", "Middle Name", "Last Name",  "Street Address", "City Name", 
                                           "State Code", "Zip Code", "Hire Date", "SSN", "Employee #" , "HR Status Code" , 
                                           "HR Status Description", "DOB" , "ACA Status", "Employee Class" , "Employee Type" };

        private static string[] OptionalColumns = { "Zip+4", "Suffix", "Country Name", "Change Date", "Rehire Date", 
                                                      "Termination Date", "Custom Pay Rate", "Custom Salary", "Custom Pay Type"};

        public IList<ValidationFailure> DataValidationMessages { get; protected set; }

        public DataTable ImportProcessedData(DataTable importData, int employerId, string userId)
        {
            DataValidationMessages = new List<ValidationFailure>();

            EmployerId = employerId;

            batchId = EmployeeController.manufactureBatchID(EmployerId, DateTime.Now, userId);

            if (HasRequiredColumnNames(importData))
            {
                DataTable FailedImportData = new DataTable();
                CopyColumns(importData, FailedImportData);

                DataTable ProcessedEmployees = EmployeeController.GetNewImportEmployeeDataTable();

                impMonths = GetImpMonths(EmployerId);

                EmployeeTypes = EmployeeTypeController.getEmployeeTypes(EmployerId);
                AcaStatuses = classificationController.getACAstatusList();
                HrStatuses = hrStatus_Controller.manufactureHRStatusList(employerId);
                Classifications = classificationController.ManufactureEmployerClassificationList(employerId, true);

                foreach (DataRow row in importData.Rows)
                {
                    if (false == ProcessRow(row, ProcessedEmployees))
                    {
                        FailedImportData.Rows.Add(row.ItemArray);
                    }
                }

                bool success = EmployeeController.BulkImportFullEmployee(batchId, ProcessedEmployees);
                if (success)
                {

                    success = EmployeeController.TransferDemographicImportTableData(EmployerId, userId, true, true);

                    if (false == success)
                    {
                        Log.Warn("Transfer DEM_I Files failed, they will remain as Alerts.");
                    }

                }
                else
                {
                    FailedImportData = importData;
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

        private List<EmployeeType> EmployeeTypes;

        private List<classification_aca> AcaStatuses;

        private List<classification> Classifications;

        private List<hrStatus> HrStatuses;

        private int impMonths;

        private int batchId;

        private int EmployerId;

        private bool ProcessRow(DataRow row, DataTable ProcessedEmployees)
        {
            try
            {
                DataRow processedRow = ProcessedEmployees.NewRow();

                if (row.IsRowBlank())
                { 
                    return true;       
                }

                processedRow["employerID"] = EmployerId.ToString().Trim().checkForDBNull();

                processedRow["hr_status_ext_id"] = row["HR Status Code"].ToString().Trim().TruncateLength(50).checkForDBNull();
                processedRow["hr_description"] = row["HR Status Description"].ToString().Trim().TruncateLength(50).checkForDBNull();
                processedRow["fName"] = row["First Name"].ToString().Trim().TruncateLength(50).checkForDBNull();
                processedRow["mName"] = row["Middle Name"].ToString().Trim().TruncateLength(50).checkForDBNull();
                processedRow["lName"] = row["Last Name"].ToString().Trim().TruncateLength(50).checkForDBNull();
                processedRow["address"] = row["Street Address"].ToString().Trim().TruncateLength(50).checkForDBNull();
                processedRow["city"] = row["City Name"].ToString().Trim().TruncateLength(50).checkForDBNull();
                processedRow["stateAbb"] = row["State Code"].ToString().Trim().TruncateLength(2).checkForDBNull();
                processedRow["zip"] = row["Zip Code"].ToString().Trim().TruncateLength(5).checkForDBNull();
                                
                processedRow["i_hDate"] = row["Hire Date"].ToString().parseDateToShortStringWithDbNull();
                processedRow["i_cDate"] = row["Change Date"].ToString().parseDateToShortStringWithDbNull();
                processedRow["i_tDate"] = row["Termination Date"].ToString().parseDateToShortStringWithDbNull();
                processedRow["i_dob"] = row["DOB"].ToString().parseDateToShortStringWithDbNull();

                processedRow["ssn"] = AesEncryption.Encrypt(row["SSN"].ToString().Trim()).TruncateLength(50).checkForDBNull();
                processedRow["ext_employee_id"] = row["Employee #"].ToString().Trim().TruncateLength(50).checkForDBNull();
                processedRow["batchid"] = batchId;
                
                processedRow["stateID"] = StateController.findStateID(processedRow["stateAbb"].ToString()).checkForDBNull();
                processedRow["hDate"] = errorChecking.convertDateTime(processedRow["i_hDate"].ToString()).checkForDBNull();
                processedRow["cDate"] = errorChecking.convertDateTime(processedRow["i_cDate"].ToString()).checkForDBNull();
                processedRow["tDate"] = errorChecking.convertDateTime(processedRow["i_tDate"].ToString()).checkForDBNull();
                processedRow["dob"] = errorChecking.convertDateTime(processedRow["i_dob"].ToString()).checkForDBNull();

                string employeeType = row["Employee Type"].ToString().Trim();
                processedRow["employeeTypeID"] = GetEmployeeTypeIdByName(employeeType).checkForDBNull();

                string acaStatus = row["ACA Status"].ToString().Trim();
                processedRow["aca_status_id"] = GetAcaStatusIdByName(acaStatus).checkForDBNull();

                string employeeClass = row["Employee Class"].ToString().Trim();
                processedRow["class_id"] = GetEmployeeClassificationIdByName(employeeClass).checkForDBNull();

                processedRow["planYearID"] = 0;

                string hrStatus = processedRow["hr_status_ext_id"].ToString().Trim();
                processedRow["hr_status_id"] = GetHrStatusIdByName(hrStatus).checkIntDBNull();

                processedRow["impEnd"] = EmployeeController.calculateIMPEndDate((DateTime)processedRow["hDate"], impMonths).checkForDBNull();

                ProcessedEmployees.Rows.Add(processedRow);
            }
            catch (Exception ex)
            {
                Log.Warn("Exception while trying to parse Demographics data.", ex);

                var failure = new Afas.Domain.ValidationFailure();
                failure.Identifier = row.Table.Rows.IndexOf(row).ToString();
                failure.ValidationSeverity = Afas.Domain.ValidationSeverity.Item;
                failure.ValidationType = "Unknown Error";
                failure.ValidationMessage = "Please contact IT: [" + ex.Message + "].";
                DataValidationMessages.Add(failure);

                return false;
            }

            return true;
        }

        private int? GetEmployeeClassificationIdByName(string employeeClass)
        {
            foreach (classification classification in Classifications)
            {
                if (classification.CLASS_DESC.Equals(employeeClass))
                {
                    return classification.CLASS_ID;
                }
            }

            return null;
        }

        private int? GetHrStatusIdByName(string hrStatus)
        {
            foreach (hrStatus status in HrStatuses)
            {
                if (status.HR_STATUS_NAME.Equals(hrStatus))
                {
                    return status.HR_STATUS_ID;
                }
            }

            return null;
        }

        private int? GetAcaStatusIdByName(string acaStatus)
        {
            foreach (classification_aca status in AcaStatuses)
            {
                if (status.ACA_STATUS_NAME.Equals(acaStatus))
                {
                    return status.ACA_STATUS_ID;
                }
            }

            return null;
        }

        private int? GetEmployeeTypeIdByName(string employeeType)
        {
            foreach (EmployeeType type in EmployeeTypes)
            {
                if (type.EMPLOYEE_TYPE_NAME.Equals(employeeType))
                {
                    return type.EMPLOYEE_TYPE_ID;
                }
            }

            return null;
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

        private int GetImpMonths(int employerId)
        {
            employer currentEmployer = employerController.getEmployer(employerId);

            return measurementController.getInitialMeasurementLength(currentEmployer.EMPLOYER_INITIAL_MEAS_ID);
        }
    }
}