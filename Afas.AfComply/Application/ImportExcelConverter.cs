using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using Afas.Domain;
using Afas.AfComply.Domain;
using Afas.AfComply.Domain.Mapping;

namespace Afas.AfComply.Application
{

    public class ImportExcelConverter : IImportExcelConverter
    {

        public ImportExcelConverter() : this(LogManager.GetLogger(typeof(ImportExcelConverter))) { }

        public ImportExcelConverter(ILog log)
        {
            this.Log = log;
        }

        public void CreateCSVTables(DataTable dataTable, String filename, String employerFEIN)
        {

            String payRoll = "Hours";
            String coverage = "Coverage";
            String demoGraphic = "Demographic";

            if ((dataTable.Columns.Contains("CoFEIN") == false) && (dataTable.Columns.Contains("Co_FEIN") == false))
            {
                throw new Exception("No CoFEIN or Co_FEIN column detected, aborting conversion!");
            }

            dataTable.VerifyContainsOnlyThisFederalIdentificationNumber("CoFEIN", employerFEIN);
            dataTable.VerifyContainsOnlyThisFederalIdentificationNumber("Co_FEIN", employerFEIN);

            int type = 0;

            if (filename.Contains(payRoll))
            {
                
                type = 1;
                
                dataTable.RenameConversionColumns(LegacyImportsDictionary.Map);
                dataTable.PruneConversionColumns();
                
                AddingAFComplyPayroll(dataTable);
            
            }

            if (filename.Contains(coverage))
            {

                type = 2;

                dataTable.RenameConversionColumns(LegacyImportsDictionary.Map);
                dataTable.PruneConversionColumns();
                
                AddAFcomplyCoverageColumns(dataTable);
            
            }

            if (filename.Contains(demoGraphic))
            {

                type = 3;

                dataTable.RenameConversionColumns(LegacyImportsDictionary.Map);
                dataTable.PruneConversionColumns();
                
                AddAFcomplyDemographicColumns(dataTable);
            
            }

            dataTable.RenameConversionColumns(LegacyImportsDictionary.Map);
            dataTable.PruneConversionColumns();
            
            switch (type)
            {
                case 1:

                    CreatingAFComplyPayroll(dataTable);

                    RemoveZeroHourEntriesFromPayrollConversion(dataTable);

                    RemoveNegativeHourEntriesFromPayrollConversion(dataTable);

                    break;

                case 2:

                    MorphFileIntoAFcomplyCoverageStructure(dataTable);

                    break;

                case 3:
                    
                    MorphFileIntoAFcomplyDemographicStructure(dataTable);
                    
                    break;
            
            }

        }

        public void AddAFcomplyDemographicColumns(DataTable dataTable)
        {

            dataTable.Columns.AddColumnIfMissing("Employee #");
            dataTable.Columns.AddColumnIfMissing("Middle Name");
            dataTable.Columns.AddColumnIfMissing("Zip+4");
            dataTable.Columns.AddColumnIfMissing("Change Date");
            dataTable.Columns.AddColumnIfMissing("DOB");

        }

        public void AddingAFComplyPayroll(DataTable dataTable)
        {

            dataTable.Columns.AddColumnIfMissing("Middle Name");
            dataTable.Columns.AddColumnIfMissing("Pay Description");
            dataTable.Columns.AddColumnIfMissing("Pay Description ID");
            dataTable.Columns.AddColumnIfMissing("Check Date");
            dataTable.Columns.AddColumnIfMissing("Employee #");
            dataTable.Columns.AddColumnIfMissing("StartDate");

            if (dataTable.Columns.CheckColumnForMonthOrYear("January"))
            {

                dataTable.Columns.RemoveColumnIfExists("Address");
                dataTable.Columns.RemoveColumnIfExists("Address_2");
                dataTable.Columns.RemoveColumnIfExists("City");
                dataTable.Columns.RemoveColumnIfExists("State");
                dataTable.Columns.RemoveColumnIfExists("Country");
                dataTable.Columns.RemoveColumnIfExists("Postal");
                dataTable.Columns.RemoveColumnIfExists("Measurement_Group_Name");
                dataTable.Columns.RemoveColumnIfExists("Date_Hired");
                dataTable.Columns.RemoveColumnIfExists("Date_Terminated");
                dataTable.Columns.RemoveColumnIfExists("Employee_Type");
                dataTable.Columns.RemoveColumnIfExists("Hours_Worked_Location");
                dataTable.Columns.RemoveColumnIfExists("Hours_Worked_Paycode");

                String years = dataTable.Columns.DetermineYearFromColumnNames();
                if (dataTable.Columns.Contains("December_2015_Hours_Worked") && years == "2016")
                {
                    dataTable.Columns.RemoveColumnIfExists("December_2015_Hours_Worked");
                }

                if (dataTable.Columns.Contains("Hours_Worked_Paycode"))
                {
                    dataTable.Columns.Remove("Hours_Worked_Paycode");
                }

                dataTable.Columns.AddColumnIfMissing("EndDate");
                dataTable.Columns.AddColumnIfMissing("ACA Hour");
                dataTable.RenameColumn("First_Name", "FirstName");
                dataTable.RenameColumn("Last_Name", "LastName");

                int a = 0;
                int b = 1;
                foreach(DataColumn columns in dataTable.Columns)
                {
                    if (columns.ColumnName.Length > 15)
                    {
                        if (columns.ColumnName.Substring(0, 7).Contains("January"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 8).Contains("February"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 7).Contains("March"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 7).Contains("April"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 7).Contains("May"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 7).Contains("June"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 7).Contains("July"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 7).Contains("August"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 9).Contains("September"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 7).Contains("October"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 8).Contains("November"))
                        { a++; b++; }
                        if (columns.ColumnName.Substring(0, 8).Contains("December"))
                        { a++; b++; }
                    }
                }
                int x = dataTable.Rows.Count;
                int z = 0;
                for (int i = 0; i < x; i++)
                {
                    int y = i * a + z;
                    for (int n = 1; n < b; n++)
                    {
                        AssignDate(dataTable, y, n);
                    }
                    z++;
                }

                z = 0;
                for (int i = 0; i < x; i++)
                {
                    dataTable.Rows.RemoveAt(z);
                    z = z + a;
                }

            }

        }

        public void AssignDate(DataTable dataTable, int x, int y)
        {
            
            DataRow dataRow = dataTable.NewRow();

            dataRow["FirstName"] = dataTable.Rows[x]["FirstName"];
            dataRow["Middle Name"] = dataTable.Rows[x]["Middle Name"];
            dataRow["LastName"] = dataTable.Rows[x]["LastName"];
            dataRow["SSN"] = dataTable.Rows[x]["SSN"];
            
            String startDate;
            String endDate;
            String year = dataTable.Columns.DetermineYearFromColumnNames();
            
            switch (y)
            {

                case 1:

                    startDate = year + "0101";
                    endDate = year + "01" + DateTime.DaysInMonth(Convert.ToInt32(year), 01).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["January_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 2:

                    startDate = year + "0201";
                    endDate = year + "02" + DateTime.DaysInMonth(Convert.ToInt32(year), 02).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["February_" + year + "_Hours_Worked"];

                    break;
                
                case 3:

                    startDate = year + "0301";
                    endDate = year + "03" + DateTime.DaysInMonth(Convert.ToInt32(year), 03).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["March_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 4:
                    
                    startDate = year + "0401";
                    endDate = year + "04" +  DateTime.DaysInMonth(Convert.ToInt32(year), 04).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["April_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 5:
                    
                    startDate = year + "0501";
                    endDate = year + "05" +  DateTime.DaysInMonth(Convert.ToInt32(year), 05).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["May_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 6:
                    
                    startDate = year + "0601";
                    endDate = year + "06" +  DateTime.DaysInMonth(Convert.ToInt32(year), 06).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["June_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 7:
                    
                    startDate = year + "0701";
                    endDate = year + "07" +  DateTime.DaysInMonth(Convert.ToInt32(year), 07).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["July_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 8:
                    
                    startDate = year + "0801";
                    endDate = year + "08" +  DateTime.DaysInMonth(Convert.ToInt32(year), 08).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["August_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 9:
                    
                    startDate = year + "0901";
                    endDate = year + "09" +  DateTime.DaysInMonth(Convert.ToInt32(year), 09).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["September_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 10:
                    
                    startDate = year + "1001";
                    endDate = year + "10" + DateTime.DaysInMonth(Convert.ToInt32(year), 10).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["October_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 11:
                    
                    startDate = year + "1101";
                    endDate = year + "11" + DateTime.DaysInMonth(Convert.ToInt32(year), 11).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["November_" + year + "_Hours_Worked"];
                    
                    break;
                
                case 12:
                    
                    startDate = year + "1201";
                    endDate = year + "12" + DateTime.DaysInMonth(Convert.ToInt32(year), 12).ToString();
                    dataRow["StartDate"] = startDate;
                    dataRow["EndDate"] = endDate;

                    dataRow["ACA Hour"] = dataTable.Rows[x]["December_" + year + "_Hours_Worked"];
                    
                    break;
            
            }

            dataTable.Rows.InsertAt(dataRow, x + 1);

        }

        public void MorphFileIntoAFcomplyDemographicStructure(DataTable dataTable)
        {

            try
            {

                dataTable.LeftPadSocialSecurityNumber("SSN");
                dataTable.LeftPadZipCode("Zip Code");

                dataTable.FormatDate("Termination Date");
                dataTable.FormatDate("Hire Date");
                dataTable.FormatDate("DOB");
                dataTable.FormatDate("Hire Date");

                String missingColumns = String.Empty;

                if (dataTable.VerifyContainsColumns(
                        out missingColumns, 
                        "First_Name", 
                        "Middle_Name", 
                        "Last_Name", 
                        "Street Address", 
                        "City",
                        "State Code",
                        "Zip Code",
                        "Zip+4",
                        "Hire Date",
                        "Change Date",
                        "SSN",
                        "Employee #",
                        "Termination Date",
                        "HR Status Code",
                        "HR Status Description",
                        "DOB"
                    ) == false)
                {
                    
                    String detailedErrorMesage = String.Format("Demographics table is missing required columns: {0}", missingColumns);

                    this.Log.Error(detailedErrorMesage);

                    throw new Exception(detailedErrorMesage);

                }

                dataTable.ReorderColumns(
                        "First_Name",
                        "Middle_Name",
                        "Last_Name",
                        "Street Address",
                        "City",
                        "State Code",
                        "Zip Code",
                        "Zip+4",
                        "Hire Date",
                        "Change Date",
                        "SSN",
                        "Employee #",
                        "Termination Date",
                        "HR Status Code",
                        "HR Status Description",
                        "DOB"
                    );

                dataTable.Columns.RemoveColumnIfExists("Middle Name");
                dataTable.Columns.RemoveColumnIfExists("Address_2");

                dataTable.GenerateHrStatusIdAndDescriptionValues("HR Status Code", "HR Status Description");

                }
            catch (Exception exception)
            {
                
                this.Log.Error("Morphing file encountered errors.", exception);

                throw;

            }

        }

        public void CreatingAFComplyPayroll(DataTable dataTable)
        {

            dataTable.LeftPadSocialSecurityNumber("SSN");
            
            if (dataTable.Columns.CheckColumnForMonthOrYear("January"))
            {
                
                String year = dataTable.Columns.DetermineYearFromColumnNames();

                dataTable.Columns.RemoveColumnIfExists("January_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("February_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("March_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("April_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("May_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("June_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("July_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("August_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("September_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("October_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("November_" + year + "_Hours_Worked");
                dataTable.Columns.RemoveColumnIfExists("December_" + year + "_Hours_Worked");
                
            }
            else
            {
                
                dataTable.FormatDate("EndDate");

                String type;
                int i = 0;
                type = "a";
             
                foreach (DataRow dataRow in dataTable.Rows)
                {

                    DateTimeStyles styles = DateTimeStyles.None;
                    CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                    String[] formats = { "yyyy/MM/dd" };                 
                    DateTime initial;
                    DateTime.TryParseExact(dataTable.Rows[i]["End Date"].ToString(), formats, culture, styles, out initial);
                    
                    switch (type)
                    {

                        case "a":

                            int monthNumber = Int32.Parse(initial.ToString("dd"));
                            monthNumber = monthNumber - 1;
                            dataTable.Rows[i]["StartDate"] = initial.AddDays(-monthNumber);
                            
                            break;
                        
                        case "b":

                            dataTable.Rows[i]["StartDate"] = initial.AddDays(-7);
                            
                            break;

                        case "c":

                            dataTable.Rows[i]["StartDate"] = initial.AddDays(-14);
                            
                            break;
                    
                    }
                    
                    i++;
                
                }
                
            }

            dataTable.FormatDate("StartDate");

            dataTable.Columns.RemoveColumnIfExists("Address");
            dataTable.Columns.RemoveColumnIfExists("City");
            dataTable.Columns.RemoveColumnIfExists("Country");
            dataTable.Columns.RemoveColumnIfExists("Postal");
            dataTable.Columns.RemoveColumnIfExists("HR Status Code");
            dataTable.Columns.RemoveColumnIfExists("State Code");
            
            if(dataTable.Columns.Contains("Last_Name"))
            {
                dataTable.RenameColumn("Last_Name", "LastName");
            }

            if (dataTable.Columns.Contains("First_Name"))
            {

                dataTable.RenameColumn("First_Name", "FirstName");

            }

            if (dataTable.Columns.Contains("EndDate"))
            {

                dataTable.RenameColumn("EndDate", "End Date");

            }

            dataTable.Columns.AddColumnIfMissing("Employee #"); 

            String missingColumns = String.Empty;
            if (dataTable.VerifyContainsColumns(
                    out missingColumns,
                    "FirstName",
                    "Middle Name",
                    "LastName",
                    "ACA Hour",
                    "StartDate",
                    "End Date",
                    "SSN",
                    "Pay Description",
                    "Pay Description ID",
                    "Check Date",
                    "Employee #"
                ) == false)
            {

                String detailedErrorMesage = String.Format("Payroll table is missing required columns: {0}", missingColumns);

                this.Log.Error(detailedErrorMesage);

                throw new Exception(detailedErrorMesage);

            }

            dataTable.ReorderColumns(
                    "FirstName",
                    "Middle Name",
                    "LastName",
                    "ACA Hour",
                    "StartDate",
                    "End Date",
                    "SSN",
                    "Pay Description",
                    "Pay Description ID",
                    "Check Date",
                    "Employee #"
                );

        }

        public void ReorderTable(ref DataTable dataTable, params String[] columnNames)
        {

            for (int i = 0; i < columnNames.Length; i++)
            {

                if (this.Log.IsInfoEnabled) { this.Log.Info(String.Format("Moving {0} to position {1}.", columnNames[i], i)); }

                dataTable.Columns[columnNames[i]].SetOrdinal(i);

                if (this.Log.IsInfoEnabled) { this.Log.Info(String.Format("Moved {0}.", columnNames[i])); }
            
            }

        }

        public void AddAFcomplyCoverageColumns(DataTable dataTable)
        {

           
            dataTable.Columns.AddColumnIfMissing("FIRST NAME");
            dataTable.Columns.AddColumnIfMissing("LAST NAME");
            dataTable.Columns.AddColumnIfMissing("SUBID");
            dataTable.Columns.AddColumnIfMissing("MEMBER");
            dataTable.Columns.AddColumnIfMissing("JAN");
            dataTable.Columns.AddColumnIfMissing("FEB");
            dataTable.Columns.AddColumnIfMissing("MAR");
            dataTable.Columns.AddColumnIfMissing("APR");
            dataTable.Columns.AddColumnIfMissing("MAY");
            dataTable.Columns.AddColumnIfMissing("JUN");
            dataTable.Columns.AddColumnIfMissing("JUL");
            dataTable.Columns.AddColumnIfMissing("AUG");
            dataTable.Columns.AddColumnIfMissing("SEP");
            dataTable.Columns.AddColumnIfMissing("OCT");
            dataTable.Columns.AddColumnIfMissing("NOV");
            dataTable.Columns.AddColumnIfMissing("DEC");
            dataTable.Columns.AddColumnIfMissing("TOTAL");

            dataTable.LeftPadSocialSecurityNumber("SSN");
            dataTable.LeftPadSocialSecurityNumber("DepEIN");


            foreach (DataRow dataRow in dataTable.Rows)
            {

                if (String.IsNullOrEmpty(dataRow["DepEIN"].ToString()))
                {

                    dataRow["SUBID"] = dataRow["SSN"].ToString();
                }
                else
                {

                    dataRow["SUBID"] = dataRow["SSN"].ToString();
                    dataRow["MEMBER"] = dataRow["SSN"].ToString();
                    dataRow["SSN"] = dataRow["DepEIN"].ToString();

                }
              
            }

            dataTable.Columns.RemoveColumnIfExists("DepEIN");

        }

        public void MorphFileIntoAFcomplyCoverageStructure(DataTable dataTable)
        {

            dataTable.LeftPadSocialSecurityNumber("SSN");

            dataTable.FormatDate("DOB");
            dataTable.Columns.RemoveColumnIfExists("Middle_Name");
            dataTable.Columns.RemoveColumnIfExists("Suffix");

            String firstName = String.Empty;
            String lastName = String.Empty;

            Boolean detailNamesExist = ((dataTable.Columns.Contains("Last_Name")) && (dataTable.Columns.Contains("First_Name")));

            foreach (DataRow dataRow in dataTable.Rows)
            {
                
                firstName = String.Empty;
                lastName = String.Empty;

                if (detailNamesExist == false)
                {
                this.ParsePersonalName(dataRow["Name"].ToString(), out firstName, out lastName);
                }
                else
                {

                    firstName = dataRow["First_Name"].ToString();
                    lastName = dataRow["Last_Name"].ToString();

                }

                dataRow["FIRST NAME"] = firstName;
                dataRow["LAST NAME"] = lastName;

            }

            dataTable.Columns.RemoveColumnIfExists("First_Name");
            dataTable.Columns.RemoveColumnIfExists("Last_Name");

            String missingColumns = String.Empty;
            if (dataTable.VerifyContainsColumns(
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
                    "TOTAL",
                    "StartDate",
                    "EndDate",
                    "CoverageStatus"
                ) == false)
            {

                String detailedErrorMesage = String.Format("Coverage table is missing required columns: {0}", missingColumns);

                this.Log.Error(detailedErrorMesage);

                throw new Exception(detailedErrorMesage);

            }

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
                    "TOTAL",
                    "StartDate",
                    "EndDate",
                    "CoverageStatus"
                );

            int loggingFileLineNumber = 1;
            int iColCount = dataTable.Columns.Count;
            IList<DataRow> rowsWithWaivedCoverage = new List<DataRow>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                
                if (dataRow["CoverageStatus"].ToString() == "W")
                {
                    
                    rowsWithWaivedCoverage.Add(dataRow);

                    continue;

                }

                DateTime coverageStarted = DateTime.MinValue;
                DateTime coveragedEnded = DateTime.MaxValue;

                if (Determine2015CoverageDates(dataRow, out coverageStarted, out coveragedEnded) == false)
                {

                    if (this.Log.IsInfoEnabled) { this.Log.Info(String.Format("Coverage at line {0} does not have valid 2015 dates.", loggingFileLineNumber)); }

                    loggingFileLineNumber++;

                    continue;

                }
                loggingFileLineNumber++;

                this.FillCoverageColumns(dataRow, coverageStarted, coveragedEnded);
   
                    }

            foreach(DataRow dataRow in rowsWithWaivedCoverage)
                    {
                dataTable.Rows.Remove(dataRow);
                    }

            dataTable.Columns.RemoveColumnIfExists("Name");
            dataTable.Columns.RemoveColumnIfExists("StartDate");
            dataTable.Columns.RemoveColumnIfExists("EndDate");
            dataTable.Columns.RemoveColumnIfExists("Offer_Date");
            dataTable.Columns.RemoveColumnIfExists("CoverageStatus");

        }

        public void ParsePersonalName(String name, out String firstName, out String lastName)
        {
            
            firstName = String.Empty;
            lastName = String.Empty;
            String[] names;

            String cleanedName = name.TrimDoubleQuotes();

            if (name.Contains(',') )
            {
                names = this.SplitPersonalNameOnCommas(cleanedName);
            }
            else
            {
                names = this.SplitPersonalNameOnSpaces(cleanedName);
            }

            firstName = names[0];
            lastName = names[1];

        }

        public String[] SplitPersonalNameOnCommas(String name)
        {

            char[] splitComma = new char[] { ',' };

            String[] splitName = name.Split(splitComma, 2);

            String lastName = splitName[0];
            String firstName = String.Empty;

            String remainder = splitName[1].Trim().TrimCommas();

            String[] splitNameOnSpace = remainder.Split(' ');
            firstName = splitNameOnSpace[0];

            if (splitNameOnSpace.Count() > 1)
            {

                StringBuilder builder = new StringBuilder();

                for (int nameIndex = 1; nameIndex < splitNameOnSpace.Length; nameIndex++)
                {

                    if (splitNameOnSpace[nameIndex].TrimCommas().Trim().Length > 1)
                    {
                        builder.AppendFormat("{0} ", splitNameOnSpace[nameIndex].TrimCommas().Trim());
                    }

                }

                builder.AppendFormat("{0} ", lastName);

                lastName = builder.ToString().Trim();

            }

            return new String[] { firstName, lastName };

        }

        public String[] SplitPersonalNameOnSpaces(String name)
        {

            String[] splitName = name.Split(' ');

            String firstName = splitName[0];
            String lastName = String.Empty;

            if (splitName.Count() == 2)
            {
                lastName = splitName[splitName.Length - 1].TrimCommas();
            }
            else
            {

                StringBuilder builder = new StringBuilder();
                for (int nameIndex = 1; nameIndex < splitName.Length; nameIndex++)
                {

                    if (splitName[nameIndex].TrimCommas().Trim().Length > 1)
                    {
                        builder.AppendFormat("{0} ", splitName[nameIndex].TrimCommas().Trim());
                    }

                }

                lastName = builder.ToString().Trim();

            }

            return new String[] { firstName, lastName };

        }

        public Boolean Determine2015CoverageDates(DataRow dataRow, out DateTime coverageStarted, out DateTime coverageEnded)
        {
            
            Boolean validStartingDate = false;
            Boolean validEndingDate = false;

            coverageStarted = DateTime.MinValue;
            coverageEnded = DateTime.MaxValue;

            DateTime startingDate;
            String rawStartingDate = dataRow["StartDate"].ToString().TrimDoubleQuotes(); ;
            if (this.Log.IsDebugEnabled) { this.Log.Debug(String.Format("StartDate: {0}.", rawStartingDate)); }

            if (DateTime.TryParse(rawStartingDate, out startingDate) == false)
            {

                this.Log.Error(String.Format("Invalid StartDate found: {0}", rawStartingDate));

                throw new Exception(String.Format("StartDate: {0} is invalid.", rawStartingDate));

            }

            DateTime endingDate;
            String rawEndingDate = dataRow["EndDate"].ToString().TrimDoubleQuotes(); ;
            if (this.Log.IsDebugEnabled) { this.Log.Debug(String.Format("EndDate: {0}.", rawEndingDate)); }

            if (DateTime.TryParse(rawEndingDate, out endingDate) == false)
            {

                this.Log.Error(String.Format("Invalid EndDate found: {0}", rawEndingDate));

                throw new Exception(String.Format("EndDate: {0} is invalid.", rawEndingDate));

            }

            if (endingDate < startingDate)
            {

                this.Log.Error(String.Format("EndDate {0} is before StartDate: {1}", rawEndingDate, rawStartingDate));

                throw new Exception(String.Format("EndDate {0} is before StartDate: {1}", rawEndingDate, rawStartingDate));

            }

            if (startingDate > endingDate)
            {

                this.Log.Error(String.Format("StartDate {0} is after EndDate: {1}", rawStartingDate, rawEndingDate));

                throw new Exception(String.Format("StartDate {0} is after EndDate: {1}", rawStartingDate, rawEndingDate));

            }

            if (startingDate <= this.Coverage2015StartDate)
            {
                
                validStartingDate = true;
                coverageStarted = this.Coverage2015StartDate;

            }
            else if (startingDate.Year == 2015)
            {
                validStartingDate = true;
                coverageStarted = startingDate;

            }

            if (endingDate >= this.Coverage2015EndDate)
            {

                validEndingDate = true;
                coverageEnded = this.Coverage2015EndDate;

            }
            else if (endingDate.Year == 2015)
            {
                validEndingDate = true;
                coverageEnded = endingDate;

            }

            return ((validStartingDate == true) && (validEndingDate == true));

        }

        public void RemoveZeroHourEntriesFromPayrollConversion(DataTable dataTable)
        {
            IList<DataRow> dataRowsToRemove = new List<DataRow>();

            foreach(DataRow dataRow in dataTable.Rows)
            {

               if (dataRow["ACA Hour"].ToString().Equals("0"))
               {
                   dataRowsToRemove.Add(dataRow);
               }

            }

            foreach(DataRow dataRow in dataRowsToRemove)
            {
                dataTable.Rows.Remove(dataRow);
            }

        }

        public void FillCoverageColumns(DataRow dataRow, DateTime startDate, DateTime endDate)
        {

            Boolean hasCoverageForJanuary = this.DateRangeCoversMonth("January", startDate, endDate);
            Boolean hasCoverageForFebruary = this.DateRangeCoversMonth("February", startDate, endDate);
            Boolean hasCoverageForMarch = this.DateRangeCoversMonth("March", startDate, endDate);
            Boolean hasCoverageForApril = this.DateRangeCoversMonth("April", startDate, endDate);
            Boolean hasCoverageForMay = this.DateRangeCoversMonth("May", startDate, endDate);
            Boolean hasCoverageForJune = this.DateRangeCoversMonth("June", startDate, endDate);
            Boolean hasCoverageForJuly = this.DateRangeCoversMonth("July", startDate, endDate);
            Boolean hasCoverageForAugust = this.DateRangeCoversMonth("August", startDate, endDate);
            Boolean hasCoverageForSeptember = this.DateRangeCoversMonth("September", startDate, endDate);
            Boolean hasCoverageForOctober = this.DateRangeCoversMonth("October", startDate, endDate);
            Boolean hasCoverageForNovember = this.DateRangeCoversMonth("November", startDate, endDate);
            Boolean hasCoverageForDecember = this.DateRangeCoversMonth("December", startDate, endDate);

            if (hasCoverageForJanuary)
            {
                dataRow["JAN"] = "1";
            }

            if (hasCoverageForFebruary)
            {
                dataRow["FEB"] = "1";
            }

            if (hasCoverageForMarch)
            {
                dataRow["MAR"] = "1";
            }

            if (hasCoverageForApril)
            {
                dataRow["APR"] = "1";
            }

            if (hasCoverageForMay)
            {
                dataRow["MAY"] = "1";
            }

            if (hasCoverageForJune)
            {
                dataRow["JUN"] = "1";
            }

            if (hasCoverageForJuly)
            {
                dataRow["JUL"] = "1";
            }

            if (hasCoverageForJuly)
            {
                dataRow["JUL"] = "1";
            }

            if (hasCoverageForAugust)
            {
                dataRow["AUG"] = "1";
            }

            if (hasCoverageForSeptember)
            {
                dataRow["SEP"] = "1";
            }

            if (hasCoverageForOctober)
            {
                dataRow["OCT"] = "1";
            }

            if (hasCoverageForNovember)
            {
                dataRow["NOV"] = "1";
            }

            if (hasCoverageForDecember)
            {
                dataRow["DEC"] = "1";
            }

        }

        public Boolean DateRangeCoversMonth(String monthName, DateTime startDate, DateTime endTime)
        {

            if (monthName.Equals("JANUARY", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-01-01")) && (DateTime.Parse("2015-01-31") <= endTime));
            }

            if (monthName.Equals("FEBRUARY", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-02-01")) && (DateTime.Parse("2015-02-28") <= endTime));
            }

            if (monthName.Equals("March", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-03-01")) && (DateTime.Parse("2015-03-31") <= endTime));
            }

            if (monthName.Equals("April", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-04-01")) && (DateTime.Parse("2015-04-30") <= endTime));
            }

            if (monthName.Equals("May", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-05-01")) && (DateTime.Parse("2015-05-31") <= endTime));
            }

            if (monthName.Equals("June", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-06-01")) && (DateTime.Parse("2015-06-30") <= endTime));
            }

            if (monthName.Equals("July", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-07-01")) && (DateTime.Parse("2015-07-31") <= endTime));
            }

            if (monthName.Equals("August", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-08-01")) && (DateTime.Parse("2015-08-31") <= endTime));
            }

            if (monthName.Equals("September", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-09-01")) && (DateTime.Parse("2015-09-30") <= endTime));
            }

            if (monthName.Equals("October", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-10-01")) && (DateTime.Parse("2015-10-31") <= endTime));
            }

            if (monthName.Equals("November", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-11-01")) && (DateTime.Parse("2015-11-30") <= endTime));
            }

            if (monthName.Equals("December", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2015-12-01")) && (DateTime.Parse("2015-12-31") <= endTime));
            }

            return false;

        }

        public void RemoveNegativeHourEntriesFromPayrollConversion(DataTable dataTable)
        {

            IList<DataRow> dataRowsToRemove = new List<DataRow>();

            Decimal zeroHours = Decimal.Parse("0.00");
            Decimal acaHours = 0;

            foreach (DataRow dataRow in dataTable.Rows)
            {

                if (Decimal.TryParse(dataRow["ACA Hour"].ToString(), out acaHours) == false)
                {
                    continue;
                }
                
                if (acaHours <= zeroHours)
                {
                    dataRowsToRemove.Add(dataRow);
                }

            }

            foreach (DataRow dataRow in dataRowsToRemove)
            {
                dataTable.Rows.Remove(dataRow);
            }

        }

        protected DateTime Coverage2015StartDate = DateTime.Parse("2015-01-01");
        protected DateTime Coverage2015EndDate = DateTime.Parse("2015-12-31");

        protected ILog Log { get; private set; }

    }

}
