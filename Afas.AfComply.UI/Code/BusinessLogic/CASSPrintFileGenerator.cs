using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Text;
using Afas.AfComply.Domain;
using System.IO;
using log4net;
using Afas.Domain;

public static class CASSPrintFileGenerator
{
    public const string NoEmployerTaxTransmissionErrorMessage = "No transmission was found for that employer for the given tax year!";
    public const string NoRecordsFoundErrorMessage = "No records found!";
    public const string SkipCASSErrorMessage = "You can only complete this process when company has approved!";
    public const string PrintCASSErrorMessage = "You can only complete this process when CASS is recieved or company has approved!";

    public static void PopulateFormsDropDownList(DropDownList DdlForm)
    {
        List<KeyValuePair<string, int>> forms = new List<KeyValuePair<string, int>>(){
            new KeyValuePair<string,int>("Select",0),
            new KeyValuePair<string,int>("1094B",1),
            new KeyValuePair<string,int>("1094C",2),
            new KeyValuePair<string,int>("1095B",3),
            new KeyValuePair<string,int>("1095C",4)
        };

        DdlForm.DataSource = forms;
        DdlForm.DataTextField = "Key";
        DdlForm.DataValueField = "Value";
        DdlForm.DataBind();

        DdlForm.SelectedIndex = 0;
    }

    public static void PopulateEmployersDropDownList(DropDownList DdlFilterEmployers, List<employer> employers)
    {
        DdlFilterEmployers.DataSource = employers;
        DdlFilterEmployers.DataTextField = "EMPLOYER_NAME";
        DdlFilterEmployers.DataValueField = "EMPLOYER_ID";
        DdlFilterEmployers.DataBind();

        DdlFilterEmployers.Items.Add("Select");
        DdlFilterEmployers.SelectedIndex = DdlFilterEmployers.Items.Count - 1;
    }

    public static bool ValidateDdlFilterEmployersSelectedItem(DropDownList DdlFilterEmployers, DropDownList Ddl, ModalPopupExtender MpeWebMessage, Literal LitMessage)
    {
        if (DdlFilterEmployers.SelectedItem.Text == "Select")
        {
            Ddl.Items.Clear();
            MpeWebMessage.Show();
            LitMessage.Text = "Please select an EMPLOYER to view.";
            return false;
        }

        return true;
    }

    public static void PopulatePlanYearDropDownList(DropDownList DdlPlanYearCurrent, List<PlanYear> planYears)
    {
        DdlPlanYearCurrent.DataSource = planYears;
        DdlPlanYearCurrent.DataTextField = "PLAN_YEAR_DESCRIPTION";
        DdlPlanYearCurrent.DataValueField = "PLAN_YEAR_ID";
        DdlPlanYearCurrent.DataBind();

        DdlPlanYearCurrent.Items.Add("Select");
        DdlPlanYearCurrent.SelectedIndex = DdlPlanYearCurrent.Items.Count - 1;
    }

    public static void PopulateTaxYearDropDownList(DropDownList DdlTaxYear, List<int> taxYears)
    {
        DdlTaxYear.DataSource = taxYears;
        DdlTaxYear.DataBind();

        DdlTaxYear.Items.Add("Select");
        DdlTaxYear.SelectedIndex = DdlTaxYear.Items.Count - 1;
    }

    public static String FileName(string form, Employee_IRS employee_irs)
    {
        if (employee_irs == null)
        {
            return String.Empty;
        }

        long millis = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;

        return string.Format(@"{0}_{1}_{2}.txt", form, employee_irs.EmployerResourceId.ToString(), millis.ToString());
    }

    public static String GeneratePrintCSVContent(Boolean corrected, String form, List<Employee_IRS> tempList, ILog Log)
    {
            string tabDelimiter = "\t";
            string newLine = "\r\n";
            StringBuilder builder = new StringBuilder();

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
                            "DecOfferOfCoverage{0}DecMonthlyPremium{0}DecSafeHarborCode{0}" +
                            "FileName{0}" , tabDelimiter));

            for (int i = 1; i <= 18; i++)
            {
                string dependencyText = string.Format("Dep{0}Fname{1}Dep{0}Mi{1}Dep{0}Lname{1}Dep{0}SSN{1}Dep{0}BirthDt{1}Dep{0}All_Months{1}Dep{0}Jan{1}Dep{0}Feb{1}Dep{0}Mar{1}Dep{0}Apr{1}Dep{0}May{1}Dep{0}Jun{1}Dep{0}Jul{1}Dep{0}Aug{1}Dep{0}Sept{1}Dep{0}Oct{1}Dep{0}Nov{1}Dep{0}Dec{1}", i.ToString(), tabDelimiter);
                builder.Append(dependencyText);
            }

            builder.Append(newLine);

            foreach (Employee_IRS emp_IRS in tempList)
            {
                var employeeText = string.Empty;
                try
                {
                    employeeText = string.Format("{0}{23}{1}{23}{2}{23}{3}{23}{4}{23}{5}{23}{6}{23}{7}{23}{8}{23}{9}{23}{10}{23}{11}{23}{12}{23}{13}{23}{14}{23}{15}{23}{16}{23}{17}{23}{18}{23}{19}{23}{20}{23}{21}{23}{22}{23}{24}{23}",
                       form.GetCsvEscaped(), corrected.ToValueString(), emp_IRS.EmployerResourceId.ToString().GetCsvEscaped(), emp_IRS.EmployeeResourceId.ToString().GetCsvEscaped(), emp_IRS.Fname.GetCsvEscaped(), emp_IRS.Mi.GetCsvEscaped(), emp_IRS.Lname.GetCsvEscaped(), emp_IRS.Suffix.GetCsvEscaped(), emp_IRS.Address.GetCsvEscaped(), emp_IRS.City.GetCsvEscaped(), emp_IRS.State.GetCsvEscaped(), emp_IRS.ZIP.GetCsvEscaped(), FormatDateTime(emp_IRS.PersonBirthDt).GetCsvEscaped(), FormatedSSN(emp_IRS.SSN).GetCsvEscaped(),
                       emp_IRS.BusinessNameLine1.GetCsvEscaped(), emp_IRS.BusinessAddressLine1Txt.GetCsvEscaped(), emp_IRS.BusinessCityNm.GetCsvEscaped(), emp_IRS.BusinessUSStateCd.GetCsvEscaped(), emp_IRS.BusinessUSZipCd.GetCsvEscaped(), FormatedEin(emp_IRS.EIN).GetCsvEscaped(), FormatedPhone(emp_IRS.ContactPhoneNum).GetCsvEscaped(), "B".GetCsvEscaped(), emp_IRS.Is_Self_Insured.ToValueString(), tabDelimiter, emp_IRS.FileName );        
                   
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Employee's Id {0} SSN {1} EmployerResourceId {2}", emp_IRS.employee_id, emp_IRS.SSN, emp_IRS.EmployerResourceId));
                    Log.Error(string.Format("Exception when appending string {0}", ex.Message));
                }

                if (!string.IsNullOrWhiteSpace(employeeText))
                {
                    builder.Append(employeeText);

                    var firstCoverage = emp_IRS.Employee_Coverage.FirstOrDefault();
                    if (firstCoverage == null)
                    {
                        int i = 0;
                        while (i < 36)
                        {
                            builder.Append(string.Format("{0}{1}", "".GetCsvEscaped(), tabDelimiter));
                            i++;
                        }
                    }
                    else
                    {
                        bool has_same_offer_of_coverage_code_for_all_months = emp_IRS.Employee_Coverage.All(c => c.offer_of_coverage_code == firstCoverage.offer_of_coverage_code);
                        string offer_of_coverage_code = (has_same_offer_of_coverage_code_for_all_months && !String.IsNullOrEmpty(firstCoverage.offer_of_coverage_code)) ? firstCoverage.offer_of_coverage_code.ToUpper() : "";

                        string decimalFormat = "{0:0.00}";

                        bool has_same_monthly_hours_for_all_months = emp_IRS.Employee_Coverage.All(c => Decimal.Equals(c.share_lowest_cost_monthly_premium, firstCoverage.share_lowest_cost_monthly_premium));
                        string monthly_hours = (has_same_monthly_hours_for_all_months && (firstCoverage.share_lowest_cost_monthly_premium != null)) ? string.Format(decimalFormat, firstCoverage.share_lowest_cost_monthly_premium.Value) : "";

                        bool has_same_safe_harbor_code_for_all_months = (emp_IRS.Employee_Coverage.All(c => c.safe_harbor_code == firstCoverage.safe_harbor_code));
                        string safe_harbor_code = (has_same_safe_harbor_code_for_all_months && !String.IsNullOrEmpty(firstCoverage.safe_harbor_code)) ? firstCoverage.safe_harbor_code.ToUpper() : "";

                        builder.Append(string.Format("{0}{3}{1}{3}{2}{3}", offer_of_coverage_code.GetCsvEscaped(), monthly_hours.GetCsvEscaped(), safe_harbor_code.GetCsvEscaped(), tabDelimiter));

                        foreach (Coverage employee_coverage in emp_IRS.Employee_Coverage)
                        {
                            offer_of_coverage_code = (has_same_offer_of_coverage_code_for_all_months || String.IsNullOrEmpty(employee_coverage.offer_of_coverage_code)) ? "" : employee_coverage.offer_of_coverage_code.ToUpper();
                            monthly_hours = (has_same_monthly_hours_for_all_months || employee_coverage.share_lowest_cost_monthly_premium == null) ? "" : string.Format(decimalFormat, employee_coverage.share_lowest_cost_monthly_premium.Value);
                            safe_harbor_code = (has_same_safe_harbor_code_for_all_months || String.IsNullOrEmpty(employee_coverage.safe_harbor_code)) ? "" : employee_coverage.safe_harbor_code.ToUpper();

                            builder.Append(string.Format("{0}{3}{1}{3}{2}{3}", offer_of_coverage_code.GetCsvEscaped(), monthly_hours.GetCsvEscaped(), safe_harbor_code.GetCsvEscaped(), tabDelimiter));
                        }
                    }


                    foreach (Dependent_IRS dependent in emp_IRS.Dependents)
                    {
                        var dependentText = string.Empty;
                        try
                        {
                           dependentText = string.Format("{0}{18}{1}{18}{2}{18}{3}{18}{4}{18}{5}{18}{6}{18}{7}{18}{8}{18}{9}{18}{10}{18}{11}{18}{12}{18}{13}{18}{14}{18}{15}{18}{16}{18}{17}{18}", dependent.Fname.GetCsvEscaped(), dependent.Mi.GetCsvEscaped(), dependent.Lname.GetCsvEscaped(), FormatedSSN(dependent.ssn).GetCsvEscaped(), FormatDateTime(dependent.dob).GetCsvEscaped(), dependent.All_Months.ToValueString(),
                               dependent.Jan.ToValueString(), dependent.Feb.ToValueString(), dependent.Mar.ToValueString(), dependent.Apr.ToValueString(), dependent.May.ToValueString(), dependent.Jun.ToValueString(), dependent.Jul.ToValueString(), dependent.Aug.ToValueString(),
                               dependent.Sep.ToValueString(), dependent.Oct.ToValueString(), dependent.Nov.ToValueString(), dependent.Dec.ToValueString(), tabDelimiter);

                        }
                        catch (Exception ex)
                        {
                            Log.Error(string.Format("Dependent's Id {0} SSN {1} EmployeeId {2}", dependent.dependent_id, dependent.ssn, dependent.employee_id));
                            Log.Error(string.Format("Exception when appending string {0}", ex.Message));
                        }

                        if (!string.IsNullOrWhiteSpace(dependentText))
                        {
                            builder.Append(dependentText);
                        }


                    }

                    builder.Append(newLine);
                }

            }

            return builder.ToString();

    }

    public static String FormatedSSN(String s)
    {
        if (String.IsNullOrEmpty(s))
            return "";

        if (s.Length == 0)
            return "";

        string str = AesEncryption.Decrypt(s).Masked_SSN();

        return str.Substring(0, 3) + "-" + str.Substring(3, 2) + "-" + str.Substring(5, 4);

    }

    public static String FormatedPhone(String s)
    {
        if (String.IsNullOrEmpty(s))
            return "";

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

        return s.Substring(0, 2) + "-" + s.Substring(2, 7);
    }

    public static String GenerateCassCSVContent(String form, List<Employee_IRS> tempList)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(string.Format("Form{0}Void_Ind{0}Correct_Ind{0}EmployerResourceId{0}EmployeeResourceId{0}Fname{0}Mi{0}Lname{0}Suffix{0}Address{0}City{0}State{0}ZIP{0}PersonBirthDt{0}SSN{0}ContactPhoneNum{0}", System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator));

        builder.Append("\n");

        foreach (Employee_IRS emp_IRS in tempList)
        {
            builder.Append(string.Format("{0}{16}{1}{16}{2}{16}{3}{16}{4}{16}{5}{16}{6}{16}{7}{16}{8}{16}{9}{16}{10}{16}{11}{16}{12}{16}{13}{16}{14}{16}{15}{16}",
                form.GetCsvEscaped(), "X".GetCsvEscaped(), "X".GetCsvEscaped(), emp_IRS.EmployerResourceId.ToString().GetCsvEscaped(), emp_IRS.EmployeeResourceId.ToString().GetCsvEscaped(), emp_IRS.Fname.GetCsvEscaped(), emp_IRS.Mi.GetCsvEscaped(), emp_IRS.Lname.GetCsvEscaped(), emp_IRS.Suffix.GetCsvEscaped(), emp_IRS.Address.GetCsvEscaped(), emp_IRS.City.GetCsvEscaped(), emp_IRS.State.GetCsvEscaped(), emp_IRS.ZIP.GetCsvEscaped(), FormatDateTime(emp_IRS.PersonBirthDt).GetCsvEscaped(), AesEncryption.Decrypt(emp_IRS.SSN).Masked_SSN().GetCsvEscaped(),
                emp_IRS.ContactPhoneNum.GetCsvEscaped(), System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator));

            builder.Append("\n");

        }

        return builder.ToString();
    }

    public static String WriteCSVContentToFile(string folder_path, string file_name, string csv_content, int EmployerId)
    {
        string tempFolder = HttpContext.Current.Server.MapPath("~/ftps/Scratch/");

        string tempFileName = string.Format(@"{0}/{1}", tempFolder, file_name);

        File.WriteAllText(tempFileName, csv_content);

        var full_file_name = string.Format(@"{0}/{1}", folder_path, file_name);

        File.Copy(tempFileName, full_file_name);

        FileArchiverWrapper archive = new FileArchiverWrapper();

        archive.ArchiveFile(tempFileName, EmployerId, "File Sent to Print");

        return string.Format("Your file: {0} is available at this location {1}", file_name, folder_path);
    }

}