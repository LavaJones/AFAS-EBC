using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Globalization;
using System.Text;

using log4net;

using Afas.Domain;
using Afas.AfComply.Domain;
using Afas.AfComply.Domain.Mapping;

namespace Afas.AfComply.Application
{
    
    public class LegacyConverterService : ILegacyConverterService
    {

        public LegacyConverterService() : this(LogManager.GetLogger(typeof(LegacyConverterService))) { }

        public LegacyConverterService(ILog log) : this(log, new CsvConverterService()) { }

        public LegacyConverterService(ILog log, ICsvConverterService csvConverterService)
        {

           

            this.CsvConverterService = csvConverterService;      
            this.Log = log;
        
        }

        DataTable ILegacyConverterService.ConvertDemographicsVariant1(String[] source, String employerFederalIdentificationNumber)
        {

            DataTable dataTable = this.CsvConverterService.Convert(source);

            dataTable.Columns.RemoveUserDefinedColumns();

            if ((dataTable.Columns.Contains("FEIN") == false) && (dataTable.Columns.Contains("CoFEIN") == false))
            {
                throw new Exception("No FEIN column detected, aborting conversion!");
            }

            dataTable.VerifyContainsOnlyThisFederalIdentificationNumber(employerFederalIdentificationNumber);

            dataTable.RenameConversionColumns(LegacyFormatDemographicsDictionary.Map);
            dataTable.PruneConversionColumns();

            AddAFcomplyDemographicColumns(dataTable);

            MorphFileIntoAFcomplyDemographicStructure(dataTable);

            return dataTable;

        }

        DataTable ILegacyConverterService.ConvertCoverageVariant1(String[] source, String employerFederalIdentificationNumber)
        {

            DataTable dataTable = this.CsvConverterService.Convert(source);

            dataTable.Columns.RemoveUserDefinedColumns();

            if ((dataTable.Columns.Contains("FEIN") == false) && (dataTable.Columns.Contains("CoFEIN") == false))
            {
                throw new Exception("No FEIN column detected, aborting conversion!");
            }

            dataTable.VerifyContainsOnlyThisFederalIdentificationNumber(employerFederalIdentificationNumber);

            dataTable.RenameConversionColumns(LegacyFormatCoverageDictionary.Map);
            dataTable.PruneConversionColumns();

            AddAFcomplyOfferColumns(dataTable);

            AddAFcomplyCarrierColumns(dataTable);

            dataTable.Columns.AddColumnIfMissing("Medical Plan Name");

            MorphFileIntoAFcomplyCoverageStructure(dataTable, Feature.CurrentReportingYear);

            return dataTable;

        }

        DataTable ILegacyConverterService.ConvertExtendedOfferVariant1(String[] source, String employerFederalIdentificationNumber)
        {

            DataTable dataTable = this.CsvConverterService.Convert(source);

            dataTable.Columns.RemoveUserDefinedColumns();

            if ((dataTable.Columns.Contains("FEIN") == false) && (dataTable.Columns.Contains("CoFEIN") == false))
            {
                throw new Exception("No FEIN column detected, aborting conversion!");
            }

            dataTable.VerifyContainsOnlyThisFederalIdentificationNumber(employerFederalIdentificationNumber);

            dataTable.RenameConversionColumns(AfComplyFormatExtendedOfferDictionary.Map);
            dataTable.PruneConversionColumns();

            AddAFcomplyOfferColumns(dataTable);

            MorphFileIntoAFcomplyOfferStructure(dataTable);

            return dataTable;

        }

        DataTable ILegacyConverterService.ConvertPayrollOhioVariant1(String[] source, String employerFederalIdentificationNumber)
        {

            DataTable dataTable = this.CsvConverterService.Convert(source);

            dataTable.Columns.RemoveUserDefinedColumns();

            if ((dataTable.Columns.Contains("FEIN") == false) && (dataTable.Columns.Contains("CoFEIN") == false))
            {
                throw new Exception("No FEIN column detected, aborting conversion!");
            }

            dataTable.VerifyContainsOnlyThisFederalIdentificationNumber(employerFederalIdentificationNumber);

            dataTable.RenameConversionColumns(LegacyFormatOhioAffordPayrollDictionary.Map);
            dataTable.PruneConversionColumns();

            AddAFcomplyPayrollColumns(dataTable);

            MorphFileIntoAFcomplyPayrollStructure(dataTable);

            RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            return dataTable;

        }

        DataTable ILegacyConverterService.ConvertPayrollOhioVariant2(String[] source, String employerFederalIdentificationNumber, int daysInThePast)
        {

            DataTable dataTable = this.CsvConverterService.Convert(source);

            dataTable.Columns.RemoveUserDefinedColumns();

            if ((dataTable.Columns.Contains("FEIN") == false) && (dataTable.Columns.Contains("CoFEIN") == false))
            {
                throw new Exception("No FEIN column detected, aborting conversion!");
            }

            dataTable.VerifyContainsOnlyThisFederalIdentificationNumber(employerFederalIdentificationNumber);

            dataTable.RenameConversionColumns(LegacyFormatOhioAffordPayrollDictionary.Map);
            dataTable.PruneConversionColumns();

            AddAFcomplyPayrollColumns(dataTable);

            ProcessOhioAffordVariantDates(dataTable);

            CalculateStartDateFromEndDate(dataTable, daysInThePast);

            MorphFileIntoAFcomplyPayrollStructure(dataTable);

            RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            return dataTable;

        }

        DataTable ILegacyConverterService.ConvertPayrollVariant1(String[] source, String employerFederalIdentificationNumber, int daysInThePast)
        {

            DataTable dataTable = this.CsvConverterService.Convert(source);

            dataTable.Columns.RemoveUserDefinedColumns();

            if ((dataTable.Columns.Contains("FEIN") == false) && (dataTable.Columns.Contains("CoFEIN") == false))
            {
                throw new Exception("No FEIN column detected, aborting conversion!");
            }

            dataTable.VerifyContainsOnlyThisFederalIdentificationNumber(employerFederalIdentificationNumber);

            dataTable.RenameConversionColumns(LegacyFormatPayrollDictionary.Map);
            dataTable.PruneConversionColumns();

            AddAFcomplyPayrollColumns(dataTable);

            CalculateStartDateFromEndDate(dataTable, daysInThePast);

            MorphFileIntoAFcomplyPayrollStructure(dataTable);

            RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            return dataTable;

        }

        public void AddAFcomplyCarrierColumns(DataTable dataTable)
        {

            dataTable.Columns.AddColumnIfMissing("Member SSN");
            dataTable.Columns.AddColumnIfMissing("First Name");
            dataTable.Columns.AddColumnIfMissing("Middle Name");
            dataTable.Columns.AddColumnIfMissing("Last Name");
            dataTable.Columns.AddColumnIfMissing("Suffix");
            dataTable.Columns.AddColumnIfMissing("SUBID");
            dataTable.Columns.AddColumnIfMissing("MEMBER");
            dataTable.Columns.AddColumnIfMissing("SSN");
            dataTable.Columns.AddColumnIfMissing("Last Name");
            dataTable.Columns.AddColumnIfMissing("First Name");

            dataTable.Columns.AddColumnIfMissing("DOB");
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
            dataTable.Columns.AddColumnIfMissing("Total");

        }

        public void AddAFcomplyDemographicColumns(DataTable dataTable)
        {

            dataTable.Columns.AddColumnIfMissing("Zip+4");
            dataTable.Columns.AddColumnIfMissing("Change Date");
            dataTable.Columns.AddColumnIfMissing("DOB");

        }

        public void AddAFcomplyExtendedOfferColumns(DataTable dataTable)
        {

            dataTable.Columns.AddColumnIfMissing("First Name");
            dataTable.Columns.AddColumnIfMissing("Middle Name");
            dataTable.Columns.AddColumnIfMissing("Last Name");
            dataTable.Columns.AddColumnIfMissing("Suffix");
            dataTable.Columns.AddColumnIfMissing("DOB");
            dataTable.Columns.AddColumnIfMissing("Offered");
            dataTable.Columns.AddColumnIfMissing("Offered On");
            dataTable.Columns.AddColumnIfMissing("Accepted");
            dataTable.Columns.AddColumnIfMissing("Accepted/Declined On");
            dataTable.Columns.AddColumnIfMissing("Medical Plan Name");
            dataTable.Columns.AddColumnIfMissing("Coverage Date Start");
            dataTable.Columns.AddColumnIfMissing("Coverage Date End");
            dataTable.Columns.AddColumnIfMissing("Hire Date");
            dataTable.Columns.AddColumnIfMissing("Change Date");
            dataTable.Columns.AddColumnIfMissing("Rehire Date");
            dataTable.Columns.AddColumnIfMissing("Termination Date");
            dataTable.Columns.AddColumnIfMissing("Employee #");

        }

        public void AddAFcomplyOfferColumns(DataTable dataTable)
        {

            dataTable.Columns.AddColumnIfMissing("OFFER_ROW_ID");
            dataTable.Columns.AddColumnIfMissing("OFFER_EMPLOYEE_ID");
            dataTable.Columns.AddColumnIfMissing("OFFER_EMPLOYER_ID");
            dataTable.Columns.AddColumnIfMissing("OFFER_PLANYEAR_ID");
            dataTable.Columns.AddColumnIfMissing("OFFER_EETYPE_ID");
            dataTable.Columns.AddColumnIfMissing("OFFER_NAME");
            dataTable.Columns.AddColumnIfMissing("OFFER_CLASS_ID");
            dataTable.Columns.AddColumnIfMissing("OFFER_AVERAGE_HOURS");
            dataTable.Columns.AddColumnIfMissing("OFFER_OFFERED");
            dataTable.Columns.AddColumnIfMissing("OFFER_OFFERED_ON");
            dataTable.Columns.AddColumnIfMissing("OFFER_ACCEPTED");
            dataTable.Columns.AddColumnIfMissing("OFFER_ACCEPTED_ON");
            dataTable.Columns.AddColumnIfMissing("OFFER_INSURANCE_ID");
            dataTable.Columns.AddColumnIfMissing("OFFER_CONTRIBUTION_ID");
            dataTable.Columns.AddColumnIfMissing("OFFER_EFFECTIVE_DATE");
            dataTable.Columns.AddColumnIfMissing("OFFER_HRA_FLEX");
            dataTable.Columns.AddColumnIfMissing("OFFER_EMPLOYEE_#");

        }

        public void AddAFcomplyPayrollColumns(DataTable dataTable)
        {

            dataTable.Columns.AddColumnIfMissing("Middle_Name");
            dataTable.Columns.AddColumnIfMissing("Start Date");
            dataTable.Columns.AddColumnIfMissing("Pay Description");
            dataTable.Columns.AddColumnIfMissing("Pay Description ID");
            dataTable.Columns.AddColumnIfMissing("Check Date");
            dataTable.Columns.AddColumnIfMissing("Employee #");

        }

        public void CalculateStartDateFromEndDate(DataTable dataTable, int daysInThePast)
        {

            foreach (DataRow dataRow in dataTable.Rows)
            {

                DateTime endDate = DateTime.Parse(dataRow["End Date"].ToString());
                DateTime startDate = endDate.AddDays(daysInThePast * -1);
                dataRow["Start Date"] = startDate;

            }

        }

        public DataRow CorrectEffectiveDatesBeforePlanYearStartDates(DataRow dataRow, DateTime effectiveDate, DateTime planYearStart)
        {

            if (effectiveDate < planYearStart)
            {
                dataRow["OFFER_EFFECTIVE_DATE"] = planYearStart.ToShortDateString();
            }
            else
            {
                dataRow["OFFER_EFFECTIVE_DATE"] = effectiveDate.ToShortDateString();
            }

            return dataRow;

        }

        public DataTable ClassifyInsuranceChangeEvents(
                DataTable dataTable,
                IList<String> affectedSocials
            )
        {

            if (dataTable.Rows.Count == 0)
            {
                return dataTable;
            }

            DataTable sortedDataTable = dataTable.AsEnumerable()
                                                 .OrderBy(r => r.Field<long>("EVENT_ROW_ID"))
                                                 .CopyToDataTable();

            IList<DataRow> duplicateEvents = new List<DataRow>();

            foreach (String affectedSocial in affectedSocials)
            {

                DataRow[] dataRows = sortedDataTable.Select(String.Format(" SSN = '{0}'  AND TYPE_OF_EVENT <> 'OFFER-FILE-REJECTION' ", affectedSocial));

                if (dataRows.Count() == 0)
                {
                    continue;
                }

                DataRow lastProcessedDataRow = dataRows.First();

                foreach (DataRow dataRow in dataRows.Skip(1))
                {

                    String lastDataRowDate = lastProcessedDataRow["OFFER_EFFECTIVE_DATE"].ToString();
                    String currentDataRowDate = dataRow["OFFER_EFFECTIVE_DATE"].ToString();

                    if (lastDataRowDate.Equals(currentDataRowDate, StringComparison.CurrentCultureIgnoreCase))
                    {
                        duplicateEvents.Add(dataRow);
                    }
                    else
                    {
                        dataRow["TYPE_OF_EVENT"] = "INSURANCE-CHANGE";
                    }

                    lastProcessedDataRow = dataRow;

                }

            }

            sortedDataTable.Rows.RemoveRows(duplicateEvents);

            return sortedDataTable;

        }

        public DataTable ClassifyInsuranceRejections(
                DataTable dataTable,
                IList<String> affectedSocials
            )
        {

            if (dataTable.Rows.Count == 0)
            {
                return dataTable;
            }

            DataTable sortedDataTable = dataTable.AsEnumerable()
                                                 .OrderBy(r => r.Field<long>("EVENT_ROW_ID"))
                                                 .CopyToDataTable();

            IList<DataRow> duplicateEvents = new List<DataRow>();

            foreach (String affectedSocial in affectedSocials)
            {

                DataRow[] dataRows = sortedDataTable.Select(String.Format(" SSN = '{0}' ", affectedSocial));

                foreach (DataRow dataRow in dataRows)
                {

                    dataRow["TYPE_OF_EVENT"] = OfferChangeEvents.Discrepancy;

                    String reason = dataRow["REJECTION-REASON"].ToString();

                    if (String.IsNullOrEmpty(reason))
                    {
                        dataRow["REJECTION-REASON"] = "OTHER ROWS FOR SSN WHERE REJECTED!";
                    }

                }

            }

            return sortedDataTable;

        }

        public Boolean DateRangeCoversMonthInYear(DateTime startDate, DateTime endTime, int year, int month)
        { 

            if (startDate.Year > year || endTime.Year < year || month < 1 || month > 12)
            {
                return false;
            }
            DateTime monthStart = new DateTime(year, month, 1);

            DateTime monthEnd = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

            return ((startDate <= monthStart) && (monthEnd <= endTime));

        }

        public Boolean DateRangeCoversMonthIn2016(String monthName, DateTime startDate, DateTime endTime)
        {

            if (monthName.Equals("JANUARY", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-01-01")) && (DateTime.Parse("2016-01-31") <= endTime));
            }

            if (monthName.Equals("FEBRUARY", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-02-01")) && (DateTime.Parse("2016-02-28") <= endTime));
            }

            if (monthName.Equals("March", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-03-01")) && (DateTime.Parse("2016-03-31") <= endTime));
            }

            if (monthName.Equals("April", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-04-01")) && (DateTime.Parse("2016-04-30") <= endTime));
            }

            if (monthName.Equals("May", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-05-01")) && (DateTime.Parse("2016-05-31") <= endTime));
            }

            if (monthName.Equals("June", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-06-01")) && (DateTime.Parse("2016-06-30") <= endTime));
            }

            if (monthName.Equals("July", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-07-01")) && (DateTime.Parse("2016-07-31") <= endTime));
            }

            if (monthName.Equals("August", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-08-01")) && (DateTime.Parse("2016-08-31") <= endTime));
            }

            if (monthName.Equals("September", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-09-01")) && (DateTime.Parse("2016-09-30") <= endTime));
            }

            if (monthName.Equals("October", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-10-01")) && (DateTime.Parse("2016-10-31") <= endTime));
            }

            if (monthName.Equals("November", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-11-01")) && (DateTime.Parse("2016-11-30") <= endTime));
            }

            if (monthName.Equals("December", StringComparison.CurrentCultureIgnoreCase))
            {
                return ((startDate <= DateTime.Parse("2016-12-01")) && (DateTime.Parse("2016-12-31") <= endTime));
            }

            return false;

        }
        
        public Boolean IsCoverageDatesWithinThisPlanYear(DataRow dataRow, DateTime planYearStart, DateTime planYearEnd)
        {

            DateTime coverageStartDate = DateTime.Parse(dataRow["Coverage Date Start"].ToString());
            DateTime coverageEndDate = DateTime.Parse(dataRow["Coverage Date End"].ToString());

            if ((coverageStartDate <= planYearEnd) && (coverageEndDate >= planYearStart))
            {
                return true;
            }

            return false;

        }

        public void FillCoverageColumns(DataRow dataRow, DateTime startDate, DateTime endDate, int year)
        {

            Boolean hasCoverageForJanuary = this.DateRangeCoversMonthInYear(startDate, endDate, year, 1);
            Boolean hasCoverageForFebruary = this.DateRangeCoversMonthInYear(startDate, endDate, year, 2);
            Boolean hasCoverageForMarch = this.DateRangeCoversMonthInYear(startDate, endDate, year, 3);
            Boolean hasCoverageForApril = this.DateRangeCoversMonthInYear(startDate, endDate, year, 4);
            Boolean hasCoverageForMay = this.DateRangeCoversMonthInYear(startDate, endDate, year, 5);
            Boolean hasCoverageForJune = this.DateRangeCoversMonthInYear(startDate, endDate, year, 6);
            Boolean hasCoverageForJuly = this.DateRangeCoversMonthInYear(startDate, endDate, year, 7);
            Boolean hasCoverageForAugust = this.DateRangeCoversMonthInYear(startDate, endDate, year, 8);
            Boolean hasCoverageForSeptember = this.DateRangeCoversMonthInYear(startDate, endDate, year, 9);
            Boolean hasCoverageForOctober = this.DateRangeCoversMonthInYear(startDate, endDate, year, 10);
            Boolean hasCoverageForNovember = this.DateRangeCoversMonthInYear(startDate, endDate, year, 11);
            Boolean hasCoverageForDecember = this.DateRangeCoversMonthInYear(startDate, endDate, year, 12);

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

        public void MorphFileIntoAFcomplyCoverageStructure(DataTable dataTable, int year)
        {

            int loggingRecordNumber = 1;
            String firstName = String.Empty;
            String lastName = String.Empty;

            String[] offerDataColumnNames = "OFFER_ROW_ID,OFFER_EMPLOYER_ID,OFFER_EMPLOYEE_ID,OFFER_PLANYEAR_ID,OFFER_EETYPE_ID,OFFER_CLASS_ID,OFFER_AVERAGE_HOURS,OFFER_INSURANCE_ID,OFFER_CONTRIBUTION_ID".Split(',');

            try
            {

                dataTable.LeftPadSocialSecurityNumber("SSN");

                dataTable.LeftPadSocialSecurityNumber("Member SSN");

                dataTable.LeftPadSocialSecurityNumber("Subscriber SSN");

                dataTable.LeftPadSocialSecurityNumber("Dependent SSN");

                foreach (DataRow dataRow in dataTable.Rows)
                {

                    if (dataRow.IsRowBlank() == true)
                    {
                        continue;
                    }

                    String member = dataRow["InsuredMember"].ToString().Trim().ToUpper();

                    String enrollStatus = dataRow["EnrollStatus"].ToString().Trim().ToUpper();

                    if (dataRow["SSN"].ToString().IsNullOrEmpty())
                    {

                        if (member.Equals("E", StringComparison.CurrentCultureIgnoreCase))
                        {

                            dataRow["SUBID"] = dataRow["Subscriber SSN"].ToString();
                            dataRow["MEMBER"] = dataRow["Subscriber SSN"].ToString();
                            dataRow["SSN"] = dataRow["Subscriber SSN"].ToString();
                        
                        }
                        else if (member.Equals("D", StringComparison.CurrentCultureIgnoreCase))
                        {

                            dataRow["SUBID"] = dataRow["Subscriber SSN"].ToString();
                            dataRow["MEMBER"] = dataRow["Subscriber SSN"].ToString();
                            dataRow["SSN"] = dataRow["Dependent SSN"].ToString();

                        }
                        else
                        {

                            this.Log.Error(String.Format("Unable able to determine correct SSN for InsuredMember value: {0} at line {1}, throwing exception.", member, loggingRecordNumber));

                            throw new Exception(String.Format("Unknown Member {0} at row {1}.", member, loggingRecordNumber));
                        
                        }

                    }

                    if (dataRow["OFFER_ACCEPTED"].ToString().IsNullOrEmpty())
                    {

                        if (enrollStatus.Equals("E", StringComparison.CurrentCultureIgnoreCase))
                        {

                            dataRow["OFFER_ACCEPTED"] = true;
                            dataRow["OFFER_OFFERED"] = true;
                        
                        }
                        else if (enrollStatus.Equals("W", StringComparison.CurrentCultureIgnoreCase))
                        {

                            dataRow["OFFER_ACCEPTED"] = false;
                            dataRow["OFFER_OFFERED"] = true;

                        }
                        else
                        {

                            this.Log.Error(String.Format("Unable able to determine correct accepted state for enrollStatus value: {0} at line {1}, throwing exception.", enrollStatus, loggingRecordNumber));

                            throw new Exception(String.Format("Unknown enrollment status {0} at row {1}.", enrollStatus, loggingRecordNumber));
                        
                        }

                    }

                    DateTime startDate;
                    if (DateTime.TryParse(dataRow["Coverage Date Start"].ToString(), out startDate) == false)
                    {

                        this.Log.Warn(String.Format("Unable to determine start date at line {0} for value {1}.", loggingRecordNumber, dataRow["Coverage Date Start"].ToString()));

                        throw new Exception(String.Format("Unable to determine start date at line {0} for value {1}.", loggingRecordNumber, dataRow["Coverage Date Start"].ToString()));

                    }

                    DateTime endDate;
                    if (DateTime.TryParse(dataRow["Coverage Date End"].ToString(), out endDate) == false)
                    {

                        this.Log.Warn(String.Format("Unable to determine start date at line {0} for value {1}.", loggingRecordNumber, dataRow["Coverage Date End"].ToString()));

                        throw new Exception(String.Format("Unable to determine end date at line {0} for value {1}.", loggingRecordNumber, dataRow["Coverage Date End"].ToString()));
                    
                    }

                    this.FillCoverageColumns(dataRow, startDate, endDate, year);

                    Boolean detailNamesExist = ((dataTable.Columns.Contains("Last_Name")) && (dataTable.Columns.Contains("First_Name")));

                    firstName = String.Empty;
                    lastName = String.Empty;

                    if (detailNamesExist == false)
                    {
                        this.ParsePersonalName(dataRow["CoveredName"].ToString(), out firstName, out lastName);
                    }
                    else
                    {

                        firstName = dataRow["First_Name"].ToString();
                        lastName = dataRow["Last_Name"].ToString();

                    }

                    dataRow["FIRST NAME"] = firstName;
                    dataRow["LAST NAME"] = lastName;

                    dataRow["OFFER_NAME"] = dataRow["CoveredName"].ToString();
                    dataRow["OFFER_OFFERED_ON"] = startDate.ToShortDateString();
                    dataRow["OFFER_ACCEPTED_ON"] = startDate.ToShortDateString();

                    dataRow["OFFER_EFFECTIVE_DATE"] = startDate.ToShortDateString();

                    dataRow["OFFER_HRA_FLEX"] = "0.00";

                    foreach (String columnName in offerDataColumnNames)
                    {
                        dataRow[columnName] = String.Empty;
                    }

                    loggingRecordNumber++;

                }

                dataTable.FormatDate("DOB");

            }
            catch (Exception exception)
            {

                this.Log.Error("Morphing file encountered errors.", exception);

                throw;

            }

        }

        public void MorphFileIntoAFcomplyDemographicStructure(DataTable dataTable)
        {

            try
            {

                dataTable.LeftPadSocialSecurityNumber("SSN");

                dataTable.FormatDate("Hire Date");

                dataTable.FormatDate("Change Date");

                dataTable.FormatDate("Termination Date");

                dataTable.FormatDate("DOB");

                foreach (DataRow dataRow in dataTable.Rows)
                {

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

                    String detailedErrorMesage = String.Format("Legacy Demographics file is missing required columns: {0}.", missingColumns);

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

                dataTable.GenerateHrStatusIdAndDescriptionValues("HR Status Code", "HR Status Description");

            }
            catch (Exception exception)
            {

                this.Log.Error("Morphing file encountered errors.", exception);

                throw;

            }

        }

        public void MorphFileIntoAFcomplyOfferStructure(DataTable dataTable)
        {

            IList<String> convertedSocials = new List<String>();

            int loggingRecordNumber = 1;

            String[] offerDataColumnNames = "OFFER_ROW_ID,OFFER_EMPLOYER_ID,OFFER_EMPLOYEE_ID,OFFER_PLANYEAR_ID,OFFER_EETYPE_ID,OFFER_CLASS_ID,OFFER_AVERAGE_HOURS,OFFER_INSURANCE_ID,OFFER_CONTRIBUTION_ID".Split(',');

            try
            {

                IList<String> existingColumns = new List<String>();

                foreach(DataColumn dataColumn in dataTable.Columns)
                {
                    existingColumns.Add(dataColumn.ColumnName);
                }

                foreach (String existingColumn in existingColumns)
                {
                    
                    String tempColumnName = String.Format("{0}-CAP", existingColumn);
                    String columnNameAllCaps = existingColumn.ToUpper();

                    dataTable.RenameColumn(existingColumn, tempColumnName);
                    dataTable.RenameColumn(tempColumnName, columnNameAllCaps);
                }

                dataTable.LeftPadSocialSecurityNumber("SSN");

                foreach (DataRow dataRow in dataTable.Rows)
                {

                    if (dataRow.IsRowBlank() == true)
                    {
                        continue;
                    }

                    String offered = dataRow["OFFERED"].ToString().Trim().ToUpper();
                    if (offered.Equals("Y"))
                    {
                        offered = Boolean.TrueString;
                    }
                    else if (offered.Equals("N"))
                    {
                        offered = Boolean.FalseString;
                    }

                    Boolean offeredFlag;
                    if (Boolean.TryParse(offered, out offeredFlag) == false)
                    {
                        throw new Exception(String.Format("Unable to determine boolean value for column {0} at row {1}, value is '{2}'!", "OFFERED", loggingRecordNumber, offered));
                    }
                    dataRow["OFFER_OFFERED"] = offeredFlag;

                    String offeredOn = dataRow["OFFERED ON"].ToString();
                    if (offeredOn.Contains("T00:00:00"))
                    {
                        offeredOn = offeredOn.Replace("T00:00:00", String.Empty);
                    }

                    if (offeredOn.IsNullOrEmpty())
                    {

                        if (offeredFlag == true)
                        {
                            throw new Exception(String.Format("Required date is missing for column {0} at row {1}!", "OFFERED ON", loggingRecordNumber));
                        }

                        dataRow["OFFER_OFFERED_ON"] = String.Empty;
                    
                    }
                    else
                    {

                        DateTime offeredOnDate;
                        if (DateTime.TryParse(offeredOn, out offeredOnDate) == false)
                        {
                            throw new Exception(String.Format("Unable to determine date value value for column {0} at row {1}, value is '{2}'!", "OFFERED ON", loggingRecordNumber, offeredOn));
                        }
                        
                        dataRow["OFFER_OFFERED_ON"] = offeredOnDate.ToShortDateString();
                    
                    }

                    Boolean acceptedFlag = false;
                    String accepted = dataRow["ACCEPTED"].ToString().Trim().ToUpper();
                    if (accepted.Equals("Y"))
                    {
                        accepted = Boolean.TrueString;
                    }
                    else if (accepted.Equals("N"))
                    {
                        accepted = Boolean.FalseString;
                    }

                    if (accepted.IsNullOrEmpty())
                    {
                        dataRow["OFFER_ACCEPTED"] = accepted;
                    }
                    else
                    {

                        if (Boolean.TryParse(accepted, out acceptedFlag) == false)
                        {
                            throw new Exception(String.Format("Unable to determine boolean value for column {0} at row {1}, value is '{2}'!", "ACCEPTED", loggingRecordNumber, accepted));
                        }
                        dataRow["OFFER_ACCEPTED"] = acceptedFlag;

                    }

                    String acceptedOn = dataRow["ACCEPTED/DECLINED ON"].ToString();
                    if (acceptedOn.Contains("T00:00:00"))
                    {
                        acceptedOn = offeredOn.Replace("T00:00:00", String.Empty);
                    }

                    if (acceptedOn.IsNullOrEmpty())
                    {

                        if (accepted.IsNullOrEmpty() == false)
                        {
                            throw new Exception(String.Format("Required date is missing for column {0} at row {1}!", "ACCEPTED ON", loggingRecordNumber));
                        }

                        dataRow["OFFER_ACCEPTED_ON"] = String.Empty;

                    }
                    else
                    {

                        DateTime acceptedOnDate;
                        if (DateTime.TryParse(acceptedOn, out acceptedOnDate) == false)
                        {
                            throw new Exception(String.Format("Unable to determine date value value for column {0} at row {1}, value is '{2}'!", "ACCEPTED/DECLINED ON", loggingRecordNumber, acceptedOn));
                        }

                        dataRow["OFFER_ACCEPTED_ON"] = acceptedOnDate.ToShortDateString();

                    }

                    String effectiveDate = dataRow["COVERAGE DATE START"].ToString();
                    if (effectiveDate.Contains("T00:00:00"))
                    {
                        effectiveDate = offeredOn.Replace("T00:00:00", String.Empty);
                    }
                    else if (effectiveDate.Contains(" 12:00:00 AM"))
                    {
                        effectiveDate = offeredOn.Replace(" 12:00:00 AM", String.Empty);
                    }

                    if (effectiveDate.IsNullOrEmpty() == true)
                    {

                        if (offeredFlag == true && offeredOn.IsNullOrEmpty())
                        {
                            throw new Exception(String.Format("Unable to determine date value value for column {0} at row {1}! (offered == {2}) (accepted == {3})", "COVERAGE DATE START", loggingRecordNumber, offeredFlag, acceptedFlag));
                        }
                        else if (offeredFlag == true)
                        {
                            effectiveDate = offeredOn;
                        }

                    }

                    dataRow["OFFER_EFFECTIVE_DATE"] = effectiveDate;

                    dataRow["OFFER_NAME"] = String.Format("{0} {1}", dataRow["FIRST NAME"], dataRow["LAST NAME"]);
                    
                    dataRow["OFFER_HRA_FLEX"] = "0.00";

                    foreach (String columnName in offerDataColumnNames)
                    {
                        dataRow[columnName] = String.Empty;
                    }

                    String dob = dataRow["DOB"].ToString();
                    if (dob.Contains("T00:00:00"))
                    {
                        dataRow["DOB"] = dob.Replace("T00:00:00", String.Empty);
                    }

                    loggingRecordNumber++;

                }

                dataTable.FormatDate("DOB");

                RemoveNoOfferOfferLines(dataTable);

            }
            catch (Exception exception)
            {

                this.Log.Error("Morphing file encountered errors.", exception);

                throw;

            }

        }

        public void MorphFileIntoAFcomplyPayrollStructure(DataTable dataTable)
        {

            try
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

                dataTable.LeftPadSocialSecurityNumber("SSN");

                dataTable.FormatDate("Check Date");

                dataTable.FormatDate("Start Date");

                dataTable.FormatDate("End Date");

                String missingColumns = String.Empty;

                if (dataTable.VerifyContainsColumns(
                        out missingColumns,
                        "First_Name",
                        "Middle_Name",
                        "Last_Name",
                        "ACA Hours",
                        "Start Date",
                        "End Date",
                        "SSN",
                        "Pay Description",
                        "Pay Description ID",
                        "Check Date",
                        "Employee #"
                    ) == false)
                {

                    String detailedErrorMesage = String.Format("Legacy Payroll file is missing required columns: {0}.", missingColumns);

                    this.Log.Error(detailedErrorMesage);

                    throw new Exception(detailedErrorMesage);

                }

                dataTable.PruneToRequiredColumns(
                        "First_Name",
                        "Middle_Name",
                        "Last_Name",
                        "ACA Hours",
                        "Start Date",
                        "End Date",
                        "SSN",
                        "Pay Description",
                        "Pay Description ID",
                        "Check Date",
                        "Employee #"
                    );

                dataTable.ReorderColumns(
                        "First_Name",
                        "Middle_Name",
                        "Last_Name",
                        "ACA Hours",
                        "Start Date",
                        "End Date",
                        "SSN",
                        "Pay Description",
                        "Pay Description ID",
                        "Check Date",
                        "Employee #"
                    );

            }
            catch (Exception exception)
            {

                this.Log.Error("Morphing file encountered errors.", exception);

                throw;

            }

        }

        public Boolean ParseRequiredBoolean(DataTable dataTable, DataRow dataRow, String columnName)
        {

            String parsedColumnName = String.Format("PARSED_{0}", columnName.ToUpper());
            dataTable.Columns.AddColumnIfMissing(parsedColumnName);

            Boolean theBoolean;
            String booleanValue = dataRow[columnName].ToString();
            if (String.IsNullOrEmpty(booleanValue) == false)
            {

                if (Boolean.TryParse(booleanValue, out theBoolean) == false)
                {
                    throw new Exception(String.Format("Required boolean (true/false) for column {0} for {1} has an invalid value of '{2}'.", columnName, dataRow["OFFER_NAME"], booleanValue));
                }
                dataRow[parsedColumnName] = theBoolean;

            }
            else
            {
                throw new Exception(String.Format("Required boolean (true/false) for column {0} does not exist for {1}.", columnName, dataRow["OFFER_NAME"]));
            }

            return theBoolean;

        }

        public DateTime ParseRequiredDate(DataTable dataTable, DataRow dataRow, String columnName)
        {

            String parsedColumnName = String.Format("PARSED_{0}", columnName.ToUpper());
            dataTable.Columns.AddColumnIfMissing(parsedColumnName);

            DateTime theDate;
            String dateValue = dataRow[columnName].ToString();
            if (String.IsNullOrEmpty(dateValue) == false)
            {

                if (DateTime.TryParse(dateValue, out theDate) == false)
                {
                    throw new Exception(String.Format("Required date for column {0} for {1} has an invalid date value of '{2}'.", columnName, dataRow["OFFER_NAME"], dateValue));
                }
                dataRow[parsedColumnName] = theDate.ToShortDateString();

            }
            else
            {
                throw new Exception(String.Format("Required date for column {0} does not exist for {1}.", columnName, dataRow["OFFER_NAME"]));
            }

            return theDate;

        }

        public void ProcessOhioAffordVariantDates(DataTable dataTable)
        {

            DateTimeStyles styles = DateTimeStyles.None;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            String[] formats =
            {
                "MMddyyyy"
            };

            foreach (DataRow dataRow in dataTable.Rows)
            {

                if (dataRow["End Date"].ToString().Contains("/") == false)
                {

                    DateTime time;

                    if (DateTime.TryParseExact(dataRow["End Date"].ToString(), formats, culture, styles, out time))
                    {
                        dataRow["End Date"] = time;
                    }
                    else if (dataRow["End Date"].ToString().Length == 7)
                    {
                        if (DateTime.TryParseExact("0" + dataRow["End Date"].ToString(), formats, culture, styles, out time))
                        {
                            dataRow["End Date"] = time;
                        }

                    }

                }

            }

        }

        public void ParsePersonalName(String name, out String firstName, out String lastName)
        {

            firstName = String.Empty;
            lastName = String.Empty;
            String[] names;

            String cleanedName = name.TrimDoubleQuotes();

            if (name.Contains(','))
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

        public void RemoveNoOfferOfferLines(DataTable dataTable)
        {

            IList<DataRow> dataRowsWithNoOffers = new List<DataRow>();
            foreach (DataRow dataRow in dataTable.Rows)
            {

                Boolean offered = Boolean.Parse(dataRow["OFFER_OFFERED"].ToString());

                if (offered == false)
                {
                    dataRowsWithNoOffers.Add(dataRow);
                }

            }

            dataTable.Rows.RemoveRows(dataRowsWithNoOffers);

        }

        public void RemoveZeroHourEntriesFromPayrollConversion(DataTable dataTable)
        {

            IList<DataRow> dataRowsToRemove = new List<DataRow>();

            foreach (DataRow dataRow in dataTable.Rows)
            {

                String acaStringValue = dataRow["ACA Hours"].ToString();

                Double acaHours = 0;

                if (Double.TryParse(acaStringValue, out acaHours) == false)
                {

                    continue;

                }

                if (String.Format("{0:N2}", acaHours).Equals("0.00"))
                {
                    dataRowsToRemove.Add(dataRow);
                }

            }

            dataTable.Rows.RemoveRows(dataRowsToRemove);

        }

        public void ResetCoverageEndDateBasedUponPlanYearEnd(DataTable dataTable, DateTime planYearEnd)
        {

            dataTable.Columns.AddColumnIfMissing("ORIGINAL_COVERAGE_DATE_END_CLEARING");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                dataRow["ORIGINAL_COVERAGE_DATE_END_CLEARING"] = dataRow["Coverage Date End"];
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {

                String coverageEndDateValue = dataRow["Coverage Date End"].ToString();

                if (String.IsNullOrEmpty(coverageEndDateValue) == false)
                {

                    DateTime coverageEndDate = DateTime.Parse(coverageEndDateValue);

                    if (planYearEnd == coverageEndDate)
                    {
                        dataRow["Coverage Date End"] = String.Empty;
                    }
                    else if (coverageEndDate > planYearEnd)
                    {
                        dataRow["Coverage Date End"] = String.Empty;
                    }

                }

            }

        }

        public void SetBlankCoverageEndDateToPlanYearEnd(DataTable dataTable, DateTime planYearEnd)
        {

            dataTable.Columns.AddColumnIfMissing("ORIGINAL_COVERAGE_DATE_END_FILLED_BLANKS");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                dataRow["ORIGINAL_COVERAGE_DATE_END_FILLED_BLANKS"] = dataRow["Coverage Date End"];
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {

                String coverageEndDateValue = dataRow["Coverage Date End"].ToString();

                if (String.IsNullOrEmpty(coverageEndDateValue) == true)
                {
                    dataRow["Coverage Date End"] = planYearEnd.ToShortDateString();
                }

            }

        }

        public Boolean ShouldOfferBeRejected(DataRow dataRow, IList<String> currentRejectedEmployees)
        {

            Boolean shouldBeRejected = false;

            String socialSecurityNumber = dataRow["SSN"].ToString();

            if (currentRejectedEmployees.Contains(socialSecurityNumber))
            {

                return true;

            }

            DateTime hireDate = DateTime.Parse(dataRow["EMPLOYEE_HIRE_DATE"].ToString());

            Boolean offered = Boolean.Parse(dataRow["OFFER_OFFERED"].ToString());

            DateTime offeredDate = DateTime.MaxValue;
            if (offered)
            {
                offeredDate = DateTime.Parse(dataRow["OFFER_OFFERED_ON"].ToString());
            }

            DateTime acceptedDate = DateTime.MinValue;

            String acceptedValue = dataRow["OFFER_ACCEPTED"].ToString();
            Boolean accepted = false;
            if (String.IsNullOrEmpty(acceptedValue) == false)
            {

                accepted = Boolean.Parse(acceptedValue);

                acceptedDate = DateTime.Parse(dataRow["OFFER_ACCEPTED_ON"].ToString());

                if (acceptedDate < hireDate)
                {
                    shouldBeRejected = true;
                }

            }

            if (offered)
            {

                if (offeredDate < hireDate)
                {
                    shouldBeRejected = true;
                }

                if (acceptedDate < offeredDate)
                {
                    shouldBeRejected = true;
                }

                DateTime effectiveDate = DateTime.Parse(dataRow["OFFER_EFFECTIVE_DATE"].ToString());

                if (effectiveDate < hireDate)
                {
                    shouldBeRejected = true;
                }

            }

            if (shouldBeRejected)
            {

                dataRow["REJECTION-REASON"] = "ISSUES WITH DATES";

            }

            return shouldBeRejected;

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

        protected ICsvConverterService CsvConverterService { get; private set; }

        protected ILog Log { get; private set; }

    }

}
