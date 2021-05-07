using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Data;
using System.Globalization;

using log4net;

using Afas.AfComply.Application;
using Afas.Application.Archiver;
using Afas.Domain;
using Afas.Application.FileAccess;
using Afas.Application.CSV;

namespace Afas.AfComply.Domain
{
    
    /// <summary>
    /// Placeholder class to capture the different steps we take to validate the AFcomply formated templates.
    /// </summary>
    public static class DataValidation
    {

        private static void CheckCoFEIN(DataTable dataTable, String key, String cofein)
        {

            if (dataTable.Columns.Contains(key))
            {
                
                foreach (DataRow datarow in dataTable.Rows)
                {
                    
                    if (datarow[key] != null
                        && datarow[key].ToString() != string.Empty
                        && false == datarow[key].ToString().Replace("-", "").Equals(cofein.Replace("-", "")))
                    {
                        throw new Exception("Row FEIN : [" + datarow[key].ToString() + "]  Did Not match Provided FEIN : " + cofein);
                    }

                }

            }

        }

        /// <summary>
        /// This function belongs to a convert service somewhere not in the middle of the data validation/saving library.
        /// </summary>
        public static DataTable ConvertFromExtendedCarrierFormat(DataTable dataTable)
        {

            dataTable.RenameConversionColumns(Afas.AfComply.Domain.Mapping.AfComplyFormatExtendedCarrierDictionary.Map);
            dataTable.PruneConversionColumns();

            dataTable.Columns.AddColumnIfMissing("MEMBER");

            dataTable.LeftPadSocialSecurityNumber("SUBID");
            dataTable.LeftPadSocialSecurityNumber("SSN");

            String[] monthlyColumnNames = "JAN,FEB,MAR,APR,MAY,JUN,JUL,AUG,SEP,OCT,NOV,DEC".Split(',');

            int currentRowPointer = 1;
            foreach (DataRow dataRow in dataTable.Rows)
            {

                dataRow["MEMBER"] = dataRow["SUBID"];

                foreach (String columnName in monthlyColumnNames)
                {

                    if (dataRow[columnName].ToString().Equals("2"))
                    {
                        throw new Exception(String.Format("Extended Carrier File has the 2 code in column {0} at row {1} which is not supported at this time.", columnName, currentRowPointer));
                    }

                }

                currentRowPointer++;

            }

            dataTable.Columns.RemoveColumnIfExists("InsuredMember");

            dataTable.ReorderColumns(
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

            return dataTable;

        }

        /// <summary>
        /// Ensures the passed filename belongs to the employer based upon the FEIN column.
        /// This column is mandatory and must be present. Handles all incoming templates since FEIN is in the first column.
        /// </summary>
        public static void FileIsForEmployer(
                String sourceFileName, 
                String employerEIN,
                IFileArchiver fileArchiver,
                Guid employerResourceId,
                int employerId)
        {

            FileInfo file = new FileInfo(sourceFileName);

            DataTable dataTable = new DataTable("CSVTable");

            dataTable.Clear();

            String[] source = File.ReadAllLines(sourceFileName);

            fileArchiver.ArchiveFile(sourceFileName, employerResourceId, "Automatic DataValidation", employerId);

            String[] headers = CsvParse.SplitRow(source[0]);

            headers = CsvHelper.SanitizeHeaderValues(headers);

            foreach (String header in headers)
            {
                dataTable.Columns.Add(header);
            }

            foreach (String row in source.Skip(1))
            {

                DateTimeStyles styles = DateTimeStyles.None;
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                String[] formats = { "yyyy/MM/dd","MM/dd/yyyy hh:mm","MM/dd/yyyy"};
                String[] rowValues = CsvParse.SplitRow(row);
                DataRow datarow = dataTable.NewRow();

                for (int i = 0; i < rowValues.Length; i++)
                {

                    DateTime time;

                    if (DateTime.TryParseExact(rowValues[i], formats, culture, styles, out time))
                    {
                        datarow[headers[i]] = time;
                    }
                    else
                    {
                        datarow[headers[i]] = rowValues[i];
                    }

                }

                dataTable.Rows.Add(datarow);

            }

            try
            {

                if (dataTable.Columns.Contains("FEIN") == false)
                {
                    throw new Exception("Mandatory column FEIN is not present in the file.");
                }

                CheckCoFEIN(dataTable, "FEIN", employerEIN);
            
            }
            catch (Exception exception)
            {

                Log.Warn("Errors during CoFEIN/FEIN checks.", exception);

                throw;

            }

            dataTable.Columns.RemoveColumnIfExists("FEIN");

            if (dataTable.Columns.Contains("Subscriber SSN"))
            {
                dataTable = ConvertFromExtendedCarrierFormat(dataTable);
            }

            String destinationFileName = String.Format("{0}-nofed.csv", sourceFileName.Replace(".csv", String.Empty));

            dataTable.WriteOutCsv(destinationFileName, false);
            

        }

        /// <summary>
        /// Determines if the (original) Offer file is for the passed employer based on the employerId in the first column.
        /// </summary>
        public static void OfferFileIsForEmployer(
                String sourceFileName, 
                long employerId, 
                Guid employerResourceId,
                IFileArchiver fileArchiver,
                bool includeHeaders = false
            )
        {

            FileInfo file = new FileInfo(sourceFileName);

            String[] source = File.ReadAllLines(sourceFileName);

            fileArchiver.ArchiveFile(sourceFileName, employerResourceId, "Automatic DataValidation", (int)employerId);

            DataTable dataTable = (new CsvConverterService() as ICsvConverterService).Convert(source);

            try
            {

                var distinctEmployerIds = (
                                           from DataRow distinctDataRow in dataTable.Rows
                                           select distinctDataRow["EMPLOYER ID"]
                                          ).Distinct().ToList();

                if (distinctEmployerIds.Count > 1)
                {

                    throw new InvalidOperationException(
                            String.Format("Incoming file had more than one employer's data in it. Expected 1 id, found {0}.", distinctEmployerIds.Count)
                        );

                }

                long employerIdInFile = Int64.Parse(distinctEmployerIds.Single().ToString());

                if (employerId != employerIdInFile)
                {

                    throw new InvalidOperationException(
                            String.Format("Incoming file EMPLOYER ID did not match the employer selected. Expected {0} but found {1}.", employerId, employerIdInFile)
                        );

                }

            }
            catch (Exception exception)
            {

                Log.Error("Errors during employer verification checks.", exception);

                throw;

            }

            String destinationFileName = String.Format("{0}-verified.csv", sourceFileName.Replace(".csv", String.Empty));

            dataTable.WriteOutCsv(destinationFileName, includeHeaders);

        }

        /// <summary>
        /// Returns true if the startDate occurs after the endDate.
        /// Returns false for null Strings.
        /// Throws ArgumentException if the dates can not be parsed.
        /// </summary>
        public static Boolean IsInvalidDateRange(String startDate, String endDate)
        {

            if (startDate == null)
            {
                return false;
            }

            if (endDate == null)
            {
                return false;
            }

            return DataValidation.IsEndDateBeforeStartDate(startDate, endDate);

        }

        /// <summary>
        /// Returns true of the endDate is before the startDate after being parsed into a DateTime object.
        /// </summary>
        public static Boolean IsEndDateBeforeStartDate(String startDate, String endDate)
        {
            return (DataValidation.IsStartBeforeEndDate(startDate, endDate) == false);
        }

        public static bool IsStartBeforeEndDate(string _start, string _end)
        {

            if (String.IsNullOrEmpty(_start))
            {
                throw new ArgumentException("StartDate can not be empty or null.");
            }

            if (String.IsNullOrEmpty(_end))
            {
                throw new ArgumentException("EndDate can not be empty or null.");
            }

            bool result = true;
            DateTime start, end;
            string[] formats = 
            {
                "yyyy/MM/dd",
                "MM/dd/yyyy",
                "M/d/yyyy",
                "yyyyMMdd",
                "yyyy-MM-dd",
                "MM/dd/yyyy hh:mm:ss tt",
                "MM/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy h:mm:ss tt",
                "MM/d/yyyy h:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy hh:mm",
                "MMddyyyy",
            };
            bool startValid = DateTime.TryParseExact(_start, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out start);
            bool endValid = DateTime.TryParseExact(_end, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out end);

            if (startValid == false)
            {
                throw new ArgumentException("StartDate does not appear to be a valid date/datetime format.");
            }

            if (endValid == false)
            {
                throw new ArgumentException("EndDate does not appear to be a valid date/datetime format.");
            }

            if (end < start)
            {
                result = false;
            }

            return result;
        
        }

        /// <summary>
        /// Tests if all elements in a string array are blank
        /// </summary>
        /// <param name="columns"></param>
        /// <returns>true if there are no non-blank elements</returns>
        public static Boolean StringArrayContainsOnlyBlanks(String[] columns)
        {

            return columns.All(x => x.IsNullOrEmpty());
        }

        internal static ILog Log = LogManager.GetLogger(typeof(DataValidation));

    }

}
