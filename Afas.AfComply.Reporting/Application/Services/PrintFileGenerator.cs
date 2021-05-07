using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Afas.AfComply.Domain;
using log4net;
using Afas.Domain;
using Afas.AfComply.Reporting.Domain.LegacyData;
using Afas.AfComply.Reporting.Domain.Approvals;
using System.IO;
using Afas.Application.Archiver;
using Afas.Domain.POCO;
using System.Globalization;

namespace Afas.AfComply.Reporting.Application.Services
{
    public static class PrintFileGenerator
    {
        public const string NoEmployerTaxTransmissionErrorMessage = "No transmission was found for that employer for the given tax year!";
        public const string NoRecordsFoundErrorMessage = "No records found!";
        public const string SkipCASSErrorMessage = "You can only complete this process when company has approved!";
        public const string PrintCASSErrorMessage = "You can only complete this process when CASS is recieved or company has approved!";

        private static ILog Log = LogManager.GetLogger(typeof(PrintFileGenerator));

 
        public static String FileName(string form, Approved1094FinalPart1 employee_irs)
        {
            if (employee_irs == null)
            {
                return String.Empty;
            }

            long millis = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;

            //return string.Format(@"{0}_{1}_{2}.csv", form, employee_irs.EmployerResourceId.ToString(), millis.ToString());
            return string.Format(@"{0}_{1}_{2}.txt", form, employee_irs.EmployerResourceId.ToString(), millis.ToString());
        }

        public static String GeneratePrintCSVContent(Boolean corrected, String form, List<Approved1095Final> tempList, Approved1094FinalPart1 employer)
        {
            //string tabDelimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            string tabDelimiter = "\t";
            string newLine = "\r\n";
            //string newLine = Environment.NewLine;

            //building the header
            StringBuilder builder = new StringBuilder();
            //Testing last column for medical month hard coding need to take out once done testing. LTV
            string MedMonthly;

            builder.Append(string.Format("Form{0}Correct_Ind{0}EmployerResourceId{0}EmployeeResourceId{0}Fname{0}Mi{0}Lname{0}Suffix{0}Address{0}City{0}State{0}ZIP{0}PersonBirthDt{0}SSN{0}" +
                            "BusinessNameLine1{0}BusinessAddressLine1Txt{0}BusinessCityNm{0}BusinessUSStateCd{0}BusinessUSZIPCd{0}EIN{0}ContactPhoneNum{0}OriginOfHealth{0}Is_Self_Insured{0}" +
                            "AllOfferOfCoverage{0}AllMonthlyPremium{0}AllSafeHarborCode{0}" +
                            "JanOfferOfCoverage{0}JanMonthlyPremium{0}JanSafeHarborCode{0}" +
                            "FebOfferOfCoverage{0}FebMonthlyPremium{0}FebSafeHarborCode{0}" +
                            "MarOfferOfCoverage{0}MarMonthlyPremium{0}MarSafeHarborCode{0}" +
                            "AprOfferOfCoverage{0}AprMonthlyPremium{0}AprSafeHarborCode{0}" +
                            "MayOfferOfCoverage{0}MayMonthlyPremium{0}MaySafeHarborCode{0}" +
                            "JunOfferOfCoverage{0}JunMonthlyPremium{0}JunSafeHarborCode{0}" +
                            "JulOfferOfCoverage{0}JulMonthlyPremium{0}JulSafeHarborCode{0}" +
                            "AugOfferOfCoverage{0}AugMonthlyPremium{0}AugSafeHarborCode{0}" +
                            "SepOfferOfCoverage{0}SepMonthlyPremium{0}SepSafeHarborCode{0}" +
                            "OctOfferOfCoverage{0}OctMonthlyPremium{0}OctSafeHarborCode{0}" +
                            "NovOfferOfCoverage{0}NovMonthlyPremium{0}NovSafeHarborCode{0}" +
                            "DecOfferOfCoverage{0}DecMonthlyPremium{0}DecSafeHarborCode{0}" , tabDelimiter ));

            for (int i = 1; i <= 18; i++)
            {
                string dependencyText = string.Format("Dep{0}Fname{1}Dep{0}Mi{1}Dep{0}Lname{1}Dep{0}SSN{1}Dep{0}BirthDt{1}Dep{0}All_Months{1}Dep{0}Jan{1}Dep{0}Feb{1}Dep{0}Mar{1}Dep{0}Apr{1}Dep{0}May{1}Dep{0}Jun{1}Dep{0}Jul{1}Dep{0}Aug{1}Dep{0}Sept{1}Dep{0}Oct{1}Dep{0}Nov{1}Dep{0}Dec{1}", 
                    i.ToString(), 
                    tabDelimiter);
                builder.Append(dependencyText);
            }
            builder.Append("FileName");
            builder.Append(tabDelimiter + "FirstName");
            builder.Append(tabDelimiter + "MiddleName");
            builder.Append(tabDelimiter + "LastName");
            builder.Append(tabDelimiter + "MedicalMonth");
            builder.Append(newLine);
           

            //building the body
            foreach (Approved1095Final emp_IRS in tempList)
            {
                var employeeText = string.Empty;
                
                try
                {
                    employeeText = string.Format("{0}{23}{1}{23}{2}{23}{3}{23}{4}{23}{5}{23}{6}{23}{7}{23}{8}{23}{9}{23}{10}{23}{11}{23}{12}{23}{13}{23}{14}{23}{15}{23}{16}{23}{17}{23}{18}{23}{19}{23}{20}{23}{21}{23}{22}{23}",
                       form.GetCsvEscaped(), corrected.ToValueString(), employer.EmployerResourceId.ToString().GetCsvEscaped(), emp_IRS.EmployeeResourceId.ToString().GetCsvEscaped(), emp_IRS.FirstName.GetCsvEscaped(), emp_IRS.MiddleName.GetCsvEscaped(), emp_IRS.LastName.GetCsvEscaped(), emp_IRS.Suffix.GetCsvEscaped(), emp_IRS.StreetAddress.GetCsvEscaped(), emp_IRS.City.GetCsvEscaped(), 
                       Enum.Parse(typeof(UsStateAbbreviationEnum), emp_IRS.State).ToString().GetCsvEscaped(), emp_IRS.Zip.ZeroPadZip().GetCsvEscaped(), FormatDateTime(emp_IRS.DOB).GetCsvEscaped(), FormatedSSN(emp_IRS.SSN).GetCsvEscaped(),
                       employer.EmployerName.GetCsvEscaped(), employer.Address.GetCsvEscaped(), employer.City.GetCsvEscaped(), ((UsStateAbbreviationEnum)employer.StateId).ToString().GetCsvEscaped(), employer.ZipCode.ZeroPadZip().GetCsvEscaped(), FormatedEin(employer.EIN.ToString().ZeroPad(9)).GetCsvEscaped(), FormatedPhone(employer.IrsContactPhone).GetCsvEscaped(), "B".GetCsvEscaped(),
                       (emp_IRS.part3s.Count > 0).ToValueString()
                       , tabDelimiter);

                }
                catch (Exception ex)
                {
                    Log.Warn(string.Format("Employee's Id {0} SSN {1} EmployerResourceId {2}", emp_IRS.EmployeeID, emp_IRS.SSN, employer.EmployerResourceId));
                    Log.Error(string.Format("Exception when appending Employee string {0}", ex.Message), ex);
                    //throw;
                }

                if (!string.IsNullOrWhiteSpace(employeeText))
                {
                    builder.Append(employeeText);

                    var firstCoverage = emp_IRS.part2s.FirstOrDefault();
                    if (firstCoverage == null)
                    {
                        // put blanks for coverage columns 
                        int i = 0;
                        while (i < 36)
                        {
                            builder.Append(string.Format("{0}{1}", "".GetCsvEscaped(), tabDelimiter));
                            i++;
                        }
                    }
                    else
                    {
                        //check if all the same; if so just place all months column; if not put each value
                        //bool has_same_offer_of_coverage_code_for_all_months = emp_IRS.Employee_Coverage.All(c => c.offer_of_coverage_code == firstCoverage.offer_of_coverage_code);
                        //string offer_of_coverage_code = (has_same_offer_of_coverage_code_for_all_months && !String.IsNullOrEmpty(firstCoverage.offer_of_coverage_code)) ? firstCoverage.offer_of_coverage_code.ToUpper() : "";

                        //bool has_same_monthly_hours_for_all_months = emp_IRS.Employee_Coverage.All(c => Decimal.Equals(c.share_lowest_cost_monthly_premium, firstCoverage.share_lowest_cost_monthly_premium));
                        //string monthly_hours = (has_same_monthly_hours_for_all_months && (firstCoverage.share_lowest_cost_monthly_premium != null)) ? string.Format(decimalFormat, firstCoverage.share_lowest_cost_monthly_premium.Value) : "";

                        //bool has_same_safe_harbor_code_for_all_months = (emp_IRS.Employee_Coverage.All(c => c.safe_harbor_code == firstCoverage.safe_harbor_code));
                        //string safe_harbor_code = (has_same_safe_harbor_code_for_all_months && !String.IsNullOrEmpty(firstCoverage.safe_harbor_code)) ? firstCoverage.safe_harbor_code.ToUpper() : "";
                        var all12 = emp_IRS.part2s.Where(items => items.MonthId == 0).Single();
                        builder.Append(string.Format("{0}{3}{1}{3}{2}{3}", all12.Line14.GetCsvEscaped(), all12.Line15.GetCsvEscaped(), all12.Line16.GetCsvEscaped(), tabDelimiter));

                        foreach (Approved1095FinalPart2 employee_coverage in emp_IRS.part2s.Where(items => items.MonthId != 0).OrderBy(items => items.MonthId))
                        {
                            //offer_of_coverage_code = (has_same_offer_of_coverage_code_for_all_months || String.IsNullOrEmpty(employee_coverage.offer_of_coverage_code)) ? "" : employee_coverage.offer_of_coverage_code.ToUpper();
                            //monthly_hours = (has_same_monthly_hours_for_all_months || employee_coverage.share_lowest_cost_monthly_premium == null) ? "" : string.Format(decimalFormat, employee_coverage.share_lowest_cost_monthly_premium.Value);
                            //safe_harbor_code = (has_same_safe_harbor_code_for_all_months || String.IsNullOrEmpty(employee_coverage.safe_harbor_code)) ? "" : employee_coverage.safe_harbor_code.ToUpper();

                            builder.Append(string.Format("{0}{3}{1}{3}{2}{3}", employee_coverage.Line14.GetCsvEscaped(), employee_coverage.Line15.GetCsvEscaped(), employee_coverage.Line16.GetCsvEscaped(), tabDelimiter));
                        }
                    }
                    foreach (Approved1095FinalPart3 dependent in emp_IRS.part3s)
                    {
                        if(dependent.SSN == emp_IRS.SSN)
                        {
                            dependent.DependantID = 0;
                        }
                    }
                        // Add all the dependents to the end of the line
                        int count = 0;
                    foreach (Approved1095FinalPart3 dependent in emp_IRS.part3s.OrderByDescending(item => item.DependantID == 0).ThenBy(item => item.FirstName))
                    {
                        // If we max out the dependents on the form then exit the loop
                        if (count >= 18) { break; }
                        var dependentText = string.Empty;
                        try
                        {
                            // dependent info
                            dependentText = string.Format(
                                "{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}{6}",
                                dependent.FirstName.GetCsvEscaped(),
                                dependent.MiddleName.GetCsvEscaped(),
                                dependent.LastName.GetCsvEscaped(),
                                FormatedSSN(AesEncryption.Decrypt(dependent.SSN)).GetCsvEscaped(),
                                FormatDepDob(dependent.Dob, dependent.SSN).GetCsvEscaped(),
                                (dependent.EnrolledAll12.HasValue ? dependent.EnrolledAll12.Value.ToValueString() : ""),
                                tabDelimiter);

                            if (dependent.EnrolledAll12.HasValue && dependent.EnrolledAll12.Value)
                            {
                                // 12 blanks
                                dependentText = dependentText + string.Format(
                                 "{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}",
                                 tabDelimiter);
                            }
                            else
                            {
                                // 12 months worth of values
                                dependentText = dependentText + string.Format(
                                    "{0}{12}{1}{12}{2}{12}{3}{12}{4}{12}{5}{12}{6}{12}{7}{12}{8}{12}{9}{12}{10}{12}{11}{12}",
                                    dependent.EnrolledJan.ToValueString(), dependent.EnrolledFeb.ToValueString(), dependent.EnrolledMar.ToValueString(),
                                    dependent.EnrolledApr.ToValueString(), dependent.EnrolledMay.ToValueString(), dependent.EnrolledJun.ToValueString(),
                                    dependent.EnrolledJul.ToValueString(), dependent.EnrolledAug.ToValueString(), dependent.EnrolledSep.ToValueString(),
                                    dependent.EnrolledOct.ToValueString(), dependent.EnrolledNov.ToValueString(), dependent.EnrolledDec.ToValueString(),
                                    tabDelimiter);
                            }
                            count++;
                        }
                        catch (Exception ex)
                        {
                            Log.Warn(string.Format("Dependent's Id {0} SSN {1} EmployeeId {2}", dependent.DependantID, dependent.SSN, dependent.EmployeeID));
                            Log.Error(string.Format("Exception when appending Dependent string {0}", ex.Message), ex);
                        }

                        // only add it if we built it correctly
                        if (false == string.IsNullOrWhiteSpace(dependentText))
                        {
                            builder.Append(dependentText);
                        }
                    }

                    // fill remaining space with blanks
                    for(int i = count; i < 18; i++)
                    {
                        builder.Append(string.Format("{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}", tabDelimiter));
                    }
                    using (AcaEntities ctx = new AcaEntities())
                    {
                        ctx.Database.CommandTimeout = 180;
                        //ctx.Configuration.ProxyCreationEnabled = false;
                        //ctx.Configuration.LazyLoadingEnabled = false;

                        MedMonthly = getEmployeeMedicalMonth(emp_IRS.EmployeeID, ctx);
                    }
                    // add the file name at the end and hten go to nthe next line
                    builder.Append(emp_IRS.FileName);
                    builder.Append(tabDelimiter + emp_IRS.FirstName);
                    builder.Append(tabDelimiter + emp_IRS.MiddleName);
                    builder.Append(tabDelimiter + emp_IRS.LastName);
                    builder.Append(tabDelimiter + MedMonthly);
                    builder.Append(newLine);
                }

            }

            return builder.ToString();

        }

        private static String getEmployeeMedicalMonth (int ID, AcaEntities ctx)
        {
            DateTime MedicalMonth;
            MedicalMonth = (from emp in ctx.employees
                            join pyear in ctx.plan_year
                            on emp.plan_year_id equals pyear.plan_year_id
                            where emp.employee_id == ID
                            select pyear.startDate).SingleOrDefault();
            return MedicalMonth.Date.Month.ToString();
        }
        private static String FormatDepDob(DateTime? dob, string ssn)
        {
            if (dob.HasValue && (string.IsNullOrEmpty(ssn) || ssn.Trim().Length == 0))
            {
                return dob.Value.ToShortDateString();
            }
            else
            {
                return "";
            }
        }

        public static String FormatedSSN(String s)
        {
            if (String.IsNullOrEmpty(s))
                return "";

            if (s.Trim().Length == 0)
                return "";

            string str = s.Masked_SSN();

            return str.Substring(0, 3) + "-" + str.Substring(3, 2) + "-" + str.Substring(5, 4);

        }

        public static String FormatedPhone(String s)
        {
            if (String.IsNullOrEmpty(s))
            {//blank string if null or empty 
                return "";
            }
            if (s.Contains('-'))
            {// if it is already formatted, let it through
                return s;
            }
            // make sure it's 10 digits long
            s.ZeroPad(10);
            return s.Substring(0, 3) + "-" + s.Substring(3, 3) + "-" + s.Substring(6, 4);
        }

        public static String FormatDateTime(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToShortDateString();
            }
            else
            {
                return "";
            }
        }

        public static String FormatedEin(String s)
        {
            if (String.IsNullOrEmpty(s))
                return "";

            s = s.Replace("-", "");

            s.ZeroPad(9);

            return s.Substring(0, 2) + "-" + s.Substring(2, 7);
        }

        public static String GenerateCassCSVContent(String form, List<Approved1095Final> tempList, Approved1094FinalPart1 employer)
        {
            //building the header
            StringBuilder builder = new StringBuilder();

            builder.Append(string.Format("Form{0}Void_Ind{0}Correct_Ind{0}EmployerResourceId{0}EmployeeResourceId{0}Fname{0}Mi{0}Lname{0}Suffix{0}Address{0}City{0}State{0}ZIP{0}PersonBirthDt{0}SSN{0}ContactPhoneNum{0}", System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator));

            builder.Append("\n");

            //building the body
            foreach (Approved1095Final emp_IRS in tempList)
            {
                builder.Append(string.Format("{0}{16}{1}{16}{2}{16}{3}{16}{4}{16}{5}{16}{6}{16}{7}{16}{8}{16}{9}{16}{10}{16}{11}{16}{12}{16}{13}{16}{14}{16}{15}{16}",
                    form.GetCsvEscaped(), "X".GetCsvEscaped(), "X".GetCsvEscaped(), employer.EmployerResourceId.ToString().GetCsvEscaped(), emp_IRS.EmployeeResourceId.ToString().GetCsvEscaped(), emp_IRS.FirstName.GetCsvEscaped(), emp_IRS.MiddleName.GetCsvEscaped(), emp_IRS.LastName.GetCsvEscaped(), emp_IRS.Suffix.GetCsvEscaped(), emp_IRS.StreetAddress.GetCsvEscaped(), emp_IRS.City.GetCsvEscaped(), emp_IRS.State.GetCsvEscaped(), emp_IRS.Zip.GetCsvEscaped(), FormatDateTime(emp_IRS.DOB).GetCsvEscaped(), AesEncryption.Decrypt(emp_IRS.SSN).Masked_SSN().GetCsvEscaped(),
                    employer.IrsContactPhone.GetCsvEscaped(), System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator));

                builder.Append("\n");

            }

            return builder.ToString();
        }

        public static int WriteCSVContentToFile(IFileArchiver archive, Guid employerGuid, string folder_path, string tempFolder, string file_name, string csv_content, int EmployerId)
        {
                        // first write the file to a temporary folder 
            string tempFileName = string.Format(@"{0}/{1}", tempFolder, file_name);

            File.WriteAllText(tempFileName, csv_content);

            // Then copy the file to the moveit folder
            var full_file_name = string.Format(@"{0}/{1}", folder_path, file_name);

            File.Copy(tempFileName, full_file_name);

            // Then archive the source file in the temporary folder
            return archive.ArchiveFile(tempFileName, employerGuid, "File Sent to Print", EmployerId);

            // return the success message
            //return string.Format("Your file: {0} is available at this location {1}", file_name, folder_path);

        }
        
        public static String Generate1094PrintCSVContent(Boolean corrected, String form, Approved1094FinalPart1 employer)
        {
            //string tabDelimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            string tabDelimiter = "\t";
            string newLine = "\r\n";
            //string newLine = Environment.NewLine;

            //building the header
            StringBuilder builder = new StringBuilder();

            builder.Append(string.Format(
                "Form{0}Correct_Ind{0}" +
                // Part 1
                "EmployerResourceId{0}DBAname{0}EIN{0}Address{0}City{0}State{0}ZIP{0}PersonToContact{0}ContactPhoneNum{0}" +
                "DGEname{0}DGE_EIN{0}DGEaddressTxt{0}DGEcityNm{0}DGE_USStateCd{0}DGE_USZIPCd{0}DGEpersonToContact{0}DGEcontactPhoneNum{0}" +
                "TotalNumFormsTransmit{0}AuthoritativeTransmittal{0}" +
                // Part 2
                "TotalFormsAleMember{0}IsAggregatedYes{0}IsAggregatedNo{0}CertificationsOfEligibility{0}" +
                // Part 3
                "All12MinCovOfferedYes{0}All12MinCovOfferedNo{0}All12FullTimeCount{0}All12TotalEmpCount{0}All12ALEgroupInd{0}" +
                "JanMinCovOfferedYes{0}JanMinCovOfferedNo{0}JanFullTimeCount{0}JanTotalEmpCount{0}JanALEgroupInd{0}" +
                "FebMinCovOfferedYes{0}FebMinCovOfferedNo{0}FebFullTimeCount{0}FebTotalEmpCount{0}FebALEgroupInd{0}" +
                "MarMinCovOfferedYes{0}MarMinCovOfferedNo{0}MarFullTimeCount{0}MarTotalEmpCount{0}MarALEgroupInd{0}" +
                "AprMinCovOfferedYes{0}AprMinCovOfferedNo{0}AprFullTimeCount{0}AprTotalEmpCount{0}AprALEgroupInd{0}" +
                "MayMinCovOfferedYes{0}MayMinCovOfferedNo{0}MayFullTimeCount{0}MayTotalEmpCount{0}MayALEgroupInd{0}" +
                "JunMinCovOfferedYes{0}JunMinCovOfferedNo{0}JunFullTimeCount{0}JunTotalEmpCount{0}JunALEgroupInd{0}" +
                "JulMinCovOfferedYes{0}JulMinCovOfferedNo{0}JulFullTimeCount{0}JulTotalEmpCount{0}JulALEgroupInd{0}" +
                "AugMinCovOfferedYes{0}AugMinCovOfferedNo{0}AugFullTimeCount{0}AugTotalEmpCount{0}AugALEgroupInd{0}" +
                "SepMinCovOfferedYes{0}SepMinCovOfferedNo{0}SepFullTimeCount{0}SepTotalEmpCount{0}SepALEgroupInd{0}" +
                "OctMinCovOfferedYes{0}OctMinCovOfferedNo{0}OctFullTimeCount{0}OctTotalEmpCount{0}OctALEgroupInd{0}" +
                "NovMinCovOfferedYes{0}NovMinCovOfferedNo{0}NovFullTimeCount{0}NovTotalEmpCount{0}NovALEgroupInd{0}" +
                "DecMinCovOfferedYes{0}DecMinCovOfferedNo{0}DecFullTimeCount{0}DecTotalEmpCount{0}DecALEgroupInd{0}", tabDelimiter));

            for (int i = 1; i <= 30; i++)
            {
                string dependencyText = string.Format("ALE{0}Name{1}ALE{0}EIN{1}", i.ToString(), tabDelimiter);
                builder.Append(dependencyText);
            }

            // headers built
            builder.Append(newLine);

            //building the body
            var Section1And2 = string.Empty;
            // No need for a loop since we only print one 1094 per employer
            try
            {

                Section1And2 = string.Format(
                    "{1}{0}{2}{0}" +
                    // Part 1 
                    "{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}" +
                    "{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}" +
                    "{20}{0}{21}{0}{22}{0}" +
                    // Part 2 
                    "{23}{0}{24}{0}{25}{0}"
                    ,
                    tabDelimiter, // DELIMITER IS 0
                    form.GetCsvEscaped(), corrected.ToValueString().GetCsvEscaped(),
                    // Part 1
                    employer.EmployerResourceId.ToString().GetCsvEscaped(), employer.EmployerName.GetCsvEscaped(),
                    FormatedEin(employer.EIN.ZeroPad(9)).GetCsvEscaped(), employer.Address.GetCsvEscaped(), employer.City.GetCsvEscaped(),
                    ((UsStateAbbreviationEnum) employer.StateId).ToString().GetCsvEscaped(), employer.ZipCode.ZeroPadZip().GetCsvEscaped(), 
                    employer.IrsContactName.GetCsvEscaped(), FormatedPhone(employer.IrsContactPhone).GetCsvEscaped(),
                    
                    employer.DgeName.GetCsvEscaped(), FormatedEin(employer.DgeEIN.ZeroPad(9)).GetCsvEscaped(), employer.DgeAddress.GetCsvEscaped(),
                    employer.DgeCity.GetCsvEscaped(), ((UsStateAbbreviationEnum)employer.DgeState).ToString().GetCsvEscaped(), 
                    employer.DgeZipCode.ZeroPadZip().GetCsvEscaped(), employer.DgeContactName.GetCsvEscaped(), FormatedPhone(employer.DgeContactPhoneNumber).GetCsvEscaped(),

                    employer.TransmissionTotal1095Forms.ToString().GetCsvEscaped(), 
                    (employer.IsAuthoritiveTransmission ? "X" : "").GetCsvEscaped(),

                    // Part 2
                    employer.Total1095Forms.ToString().GetCsvEscaped(),
                    (employer.IsAggregatedAleGroup ? "X" : "").GetCsvEscaped(),
                    (employer.IsAggregatedAleGroup ? "" : "X").GetCsvEscaped(),
                    string.Empty.GetCsvEscaped()
                    );

                builder.Append(Section1And2);

            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Exception when building print file 1094 section 1 & 2 for employer id [{0}], Tax Year [{1}], On Approval [{2}].", employer.EmployerId, employer.TaxYearId, employer.ID), ex);
            }

            try
            {
                // if there isn't an all 12 saved value then add it manually
                if (employer.Approved1094FinalPart3s.Where(item => item.MonthId == 0).Count() <= 0)
                {
                    string MonthlyString = string.Format(
                        "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}",
                        tabDelimiter,
                        string.Empty.GetCsvEscaped(),
                        string.Empty.GetCsvEscaped(),
                        string.Empty.GetCsvEscaped(),
                        string.Empty.GetCsvEscaped(),
                        string.Empty.GetCsvEscaped()
                        );

                    builder.Append(MonthlyString);
                }

                // Part 3
                foreach (var part3 in employer.Approved1094FinalPart3s.OrderBy(item => item.MonthId))
                {

                    string MonthlyString = string.Format(
                        "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}", 
                        tabDelimiter,
                        (part3.MinimumEssentialCoverageOfferIndicator ? "X" : "").GetCsvEscaped(),
                        (part3.MinimumEssentialCoverageOfferIndicator ? "" : "X").GetCsvEscaped(),
                        part3.FullTimeEmployeeCount.ToString().GetCsvEscaped(),
                        part3.TotalEmployeeCount.ToString().GetCsvEscaped(),
                        (part3.AggregatedGroupIndicator ? "X" : "").GetCsvEscaped()
                        );

                    builder.Append(MonthlyString);

                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Exception when building print file 1094 section 3 for employer id [{0}], Tax Year [{1}], On Approval [{2}].", employer.EmployerId, employer.TaxYearId, employer.ID), ex);
            }

            try
            { 
                // Part 4
                foreach (var part4 in employer.Approved1094FinalPart4s.Take(30))
                {

                    string AggregateString = string.Format("{1}{0}{2}{0}", tabDelimiter, part4.EmployerName.GetCsvEscaped(), FormatedEin(part4.EIN.RemoveDashes().ZeroPad(9)).GetCsvEscaped());

                    builder.Append(AggregateString);

                }

            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Exception when building print file 1094 section 4 for employer id [{0}], Tax Year [{1}], On Approval [{2}].", employer.EmployerId, employer.TaxYearId, employer.ID), ex);
            }

            builder.Append(newLine);

            return builder.ToString();

        }

    }

}