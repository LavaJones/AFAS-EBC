using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Globalization;

using log4net;

using Afas.AfComply.Domain.Mapping;
using System.Reflection;
using System.IO;
using System.Collections;
using Afas.Domain;

namespace Afas.AfComply.Domain
{
    
    /// <summary>
    /// Utility methods for working with DataTables within the AFcomply Domain contexts.
    /// </summary>
    public static class DataTableExtensionMethods
    {

        /// <summary>
        /// Handle any truncated 0's from Excel or other programs parsing an SSN as a number.
        /// </summary>
        public static void LeftPadSocialSecurityNumber(this DataTable dataTable, String socialSecurityNumberColumnName)
        {

            foreach (DataRow dataRow in dataTable.Rows)
            {

                String socialSecurityNumber = dataRow[socialSecurityNumberColumnName].ToString();

                if (socialSecurityNumber.Length == 0)
                {
                    continue;
                }

                if (socialSecurityNumber.Length < 9)
                {

                    while (socialSecurityNumber.Trim().Length < 9)
                    {
                        socialSecurityNumber = String.Format("0{0}", socialSecurityNumber);
                    }

                    dataRow[socialSecurityNumberColumnName] = socialSecurityNumber;
                
                }

            }

        }

        /// <summary>
        /// Handle any truncated 0's from Excel or other programs parsing the zip code as a number.
        /// </summary>
        public static void LeftPadZipCode(this DataTable dataTable, String zipCodeColumnName)
        {

            foreach (DataRow dataRow in dataTable.Rows)
            {

                String zipCode = dataRow[zipCodeColumnName].ToString();

                if (zipCode.Length < 5)
                {

                    while (zipCode.Trim().Length < 5)
                    {
                        zipCode = String.Format("0{0}", zipCode);
                    }

                    dataRow[zipCodeColumnName] = zipCode;

                }

            }

        }

        /// <summary>
        /// Generates a suitable HR Status Id and HR Status Description.
        /// Use this method when transforming data that does not come from a payroll/HRIS system.
        /// </summary>
        public static void GenerateHrStatusIdAndDescriptionValues(this DataTable dataTable, String statusIdColumnName, String statusDescriptionColumnName)
        {

            foreach (DataRow dataRow in dataTable.Rows)
            {

                String hrStatusCode = dataRow["HR Status Code"].ToString().TrimDoubleQuotes().TrimCommas();

         
                String hrStatusDescription = dataRow["HR Status Description"].ToString().TrimDoubleQuotes().TrimCommas();

                if (Log.IsDebugEnabled) { Log.Debug(String.Format("Working with HR Status Code: {0}, HR Status Description: {1}", hrStatusCode, hrStatusDescription.Replace("/", String.Empty))); }         

                dataRow["HR Status Code"] = ConverterHelper.BuildHrStatusId(hrStatusCode, hrStatusDescription);
                dataRow["HR Status Description"] = ConverterHelper.BuildHrStatusDescription(hrStatusCode, hrStatusDescription);

            }

        }

        /// <summary>
        /// Remove any column containing the deletion marker set by the RenameConversionColumns method.
        /// </summary>
        public static void PruneConversionColumns(this DataTable dataTable)
        {

            int columnIndexToPrune = 0;
            List<int> columnIndexes = new List<int>();
            IDictionary<String, String> mappingDictionary = LegacyImportsDictionary.Map;

            foreach (DataColumn column in dataTable.Columns)
            {

                if (column.ColumnName.Length >= 3 && column.ColumnName.Substring(0, 3).Equals("DEL"))
                {
                    columnIndexes.Add(columnIndexToPrune);
                }

                columnIndexToPrune++;

            }

            columnIndexToPrune = 0;

            int y = 0;
            int[] a = new int[columnIndexes.Count];
            columnIndexes.CopyTo(a);

            foreach (int x in columnIndexes)
            {

                if (y >= 1) { y = a[columnIndexToPrune] - columnIndexToPrune; }
                
                if (y == 0) { y = a[columnIndexToPrune]; }
                
                dataTable.Columns.RemoveAt(y);
                
                if (y >= 0) { y++; }
                
                columnIndexToPrune++;
            
            }
        
        }

        /// <summary>
        /// Removes any column that is not in the requiredColumns list.
        /// Ensures that any column not in the list is removed to prevent import errors.
        /// </summary>
        public static void PruneToRequiredColumns(this DataTable dataTable, params String[] requiredColumns)
        {

            IList<DataColumn> columnsToRemove = new List<DataColumn>();

            foreach (DataColumn column in dataTable.Columns)
            {

                if (Log.IsInfoEnabled) { Log.Info(String.Format("Does {0} exist in the required columns?", column.ColumnName)); }

                if (requiredColumns.Contains(column.ColumnName) == false)
                {

                    if (Log.IsInfoEnabled) { Log.Info(String.Format("Nope, adding {0} to the prune list.", column.ColumnName)); }

                    columnsToRemove.Add(column);
                
                }
                {
                    if (Log.IsInfoEnabled) { Log.Info(String.Format("Yes, {0} gets to stay!", column.ColumnName)); }
                }

            }

            if (Log.IsInfoEnabled) { Log.Info(String.Format("Pruning {0} column(s).", columnsToRemove.Count)); }

            foreach (DataColumn column in columnsToRemove)
            {
                dataTable.Columns.Remove(column);
            }

        }

        /// <summary>
        /// Ensures the DataTable contains records associated with the passed in FederalIdentificationNumber.
        /// Prevents common cross over situations where the wrong employer was selected _or_ if the file contains mixed data.
        /// This Method checks many common FEIN column Names 
        /// </summary>
        /// <param name="dataTable">This object, to be checks for FEIN</param>
        /// <param name="federalIdentificationNumber">The FEIN to chek for matches.</param>
        /// <returns>True if it found a standard FEIN Column to check, false if it did not check</returns>
        public static bool VerifyContainsOnlyThisFederalIdentificationNumber(
                this DataTable dataTable,
                String federalIdentificationNumber
            )
        {
            if (true == dataTable.Columns.Contains("CoFEIN"))
            {
                dataTable.VerifyContainsOnlyThisFederalIdentificationNumber("CoFEIN", federalIdentificationNumber);
                return true;
            }            
            else if (true == dataTable.Columns.Contains("FedId"))
            {
                dataTable.VerifyContainsOnlyThisFederalIdentificationNumber("FedId", federalIdentificationNumber);
            } 
            else if (true == dataTable.Columns.Contains("FEIN"))
            { 
                dataTable.VerifyContainsOnlyThisFederalIdentificationNumber("FEIN", federalIdentificationNumber); 
            }
            else if (true == dataTable.Columns.Contains("CO_FEIN"))
            {
                dataTable.VerifyContainsOnlyThisFederalIdentificationNumber("CO_FEIN", federalIdentificationNumber);
            }
            else if (true == dataTable.Columns.Contains("1"))
            {
                try
                {
                    dataTable.VerifyContainsOnlyThisFederalIdentificationNumber("1", federalIdentificationNumber);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensures the DataTable contains records associated with the passed in FederalIdentificationNumber.
        /// Prevents common cross over situations where the wrong employer was selected _or_ if the file contains mixed data.
        /// If the federalIdentificationColumnName does not exist the method returns before processing any data.
        /// </summary>
        public static void VerifyContainsOnlyThisFederalIdentificationNumber(
                this DataTable dataTable, 
                String federalIdentificationColumnName, 
                String federalIdentificationNumber
            )
        {

            String comparableFederalIdentificationNumber = federalIdentificationNumber.Replace("-", String.Empty);

            if (false == dataTable.Columns.Contains(federalIdentificationColumnName))
            {

                Log.Info(String.Format("DataTable does not contain the {0} column, skipping processing.", federalIdentificationColumnName));

                return;

            }

            int errorRowIndex = 1;
            foreach (DataRow datarow in dataTable.Rows)
            {

                if (datarow.IsRowBlank())
                {
                    continue;
                }

                String dataRowFederalIdentificationNumber = datarow[federalIdentificationColumnName].ToString();

                if (String.IsNullOrEmpty(dataRowFederalIdentificationNumber))
                {

                    throw new Exception(
                            String.Format("{0} at row {1} is null or empty.", federalIdentificationColumnName, errorRowIndex)
                        );

                }

                String comparableDataRowFederalIdentificationNumber = dataRowFederalIdentificationNumber.Replace("-", String.Empty);

                if (false == comparableDataRowFederalIdentificationNumber.Equals(comparableFederalIdentificationNumber))
                {

                    throw new Exception(
                            String.Format(
                                    "Federal Id: {0} did not match Employer's Federal Id: {1} at row {2}", 
                                    comparableDataRowFederalIdentificationNumber, 
                                    comparableFederalIdentificationNumber, 
                                    errorRowIndex
                                )
                        );

                }

                errorRowIndex++;

            }

        }

        internal static ILog Log = LogManager.GetLogger(typeof(DataTableExtensionMethods));

    }

}
