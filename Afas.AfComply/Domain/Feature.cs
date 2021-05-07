using System;
using System.Configuration;

using log4net;

/// <summary>
/// Wrapper class around the different features that can be toggled on and off per site, per deployment and per company.
/// </summary>
public static class Feature
{

    /// <summary>
    /// Controls if the adminstration feature is shown to the adminstation users. Used to limit exposure to VPN or second factor authenticated users.
    /// </summary>
    public static Boolean AdministrationEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.AdministrationEnabled",
                    ConfigurationManager.AppSettings["Feature.AdministrationEnabled"]));

            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.AdministrationEnabled"]) == true;
        }

    }

    /// <summary>
    /// Controls the Auto Logout refresh timer for the App.
    /// </summary>
    public static int AutoLogoutTime
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.AutoLogoutTime",
                    ConfigurationManager.AppSettings["Feature.AutoLogoutTime"]));
            }

            return int.Parse(ConfigurationManager.AppSettings["Feature.AutoLogoutTime"]);

        }

    }

    /// <summary>
    /// Controls the bulk convert link and page on the admin site.
    /// </summary>
    public static Boolean BulkConverterEnabled
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.BulkConvert",
                    ConfigurationManager.AppSettings["Feature.BulkConvert"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.BulkConvert"]) == true;
        }
    }

    /// <summary>
    /// Controls the bulk import hidden pages on the admin site.
    /// </summary>
    public static Boolean BulkImportEnabled
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.BulkImport",
                    ConfigurationManager.AppSettings["Feature.BulkImport"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.BulkImport"]) == true;
        }
    }

    /// <summary>
    /// Controls if a CheckDate that is blank or null is set to the default 1/1/1920 value.
    /// </summary>
    public static Boolean CheckDateDefaultValueEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.CheckDateDefaultValueEnabled",
                    ConfigurationManager.AppSettings["Feature.CheckDateDefaultValueEnabled"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.CheckDateDefaultValueEnabled"]) == true;

        }

    }

    /// <summary>
    /// Controls if the yellow page of death or a reasonable error message is displayed in the last chance exception handler.
    /// </summary>
    public static Boolean DangerousErrorsEnabled
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.DangerousErrorsEnabled",
                    ConfigurationManager.AppSettings["Feature.DangerousErrorsEnabled"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.DangerousErrorsEnabled"]) == true;
        }
    }

    /// <summary>
    /// Controls if a DOB that is blank or null is set to the default 1/1/1920 value.
    /// </summary>
    public static Boolean DOBDefaultValueEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.DOBDefaultValueEnabled",
                    ConfigurationManager.AppSettings["Feature.DOBDefaultValueEnabled"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.DOBDefaultValueEnabled"]) == true;

        }

    }

    /// <summary>
    /// Controls if the invoicing module is presented to billing/adminstration users. Used to limit exposure to VPN or second factor authenticated users.
    /// </summary>
    public static Boolean InvoicingEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.InvoicingEnabled",
                    ConfigurationManager.AppSettings["Feature.InvoicingEnabled"]));

            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.InvoicingEnabled"]) == true;

        }

    }

    /// <summary>
    /// If toggled to true the app will use Qlik for the homepage and reports page, otherwise will use the old pages
    /// </summary>
    public static Boolean QlikEnabled
    {

        get
        {
            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.QlikEnabled"]) == true;
        }

    }

    /// <summary>
    /// If toggled to true forces a null to dbnull conversion check for the employee demographic imports.
    /// </summary>
    public static Boolean EmployeeDemographicEmployeeNumberRequired
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.EmployeeDemographicEmployeeNumberRequired",
                    ConfigurationManager.AppSettings["Feature.EmployeeDemographicEmployeeNumberRequired"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.EmployeeDemographicEmployeeNumberRequired"]) == true;

        }

    }

    /// <summary>
    /// If toggled to true it will create an employee number if it is blank or null in the import table.
    /// </summary>
    public static Boolean EmployeeDemographicGenerateEmployeeNumberEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.EmployeeDemographicGenerateEmployeeNumberEnabled",
                    ConfigurationManager.AppSettings["Feature.EmployeeDemographicGenerateEmployeeNumberEnabled"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.EmployeeDemographicGenerateEmployeeNumberEnabled"]) == true;

        }

    }

    /// <summary>
    /// Controls if Carrier file exports are enabled.
    /// </summary>
    public static Boolean EmployeeExportCarrierFileEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.EmployeeExportCarrierFileEnabled",
                    ConfigurationManager.AppSettings["Feature.EmployeeExportCarrierFileEnabled"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.EmployeeExportCarrierFileEnabled"]) == true;

        }

    }

    /// <summary>
    /// Controls if Offer file exports are enabled.
    /// </summary>
    public static Boolean EmployeeExportOfferFileEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.EmployeeExportOfferFileEnabled",
                    ConfigurationManager.AppSettings["Feature.EmployeeExportOfferFileEnabled"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.EmployeeExportOfferFileEnabled"]) == true;

        }

    }

    /// <summary>
    /// Controls if Exports are done using the legacy export or our new export code.
    /// </summary>
    public static Boolean FullDataExportEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.FullDataExport",
                    ConfigurationManager.AppSettings["Feature.FullDataExport"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.FullDataExport"]) == true;

        }

    }

    /// <summary>
    /// Controls if the System Wide Home Page Message is displayed
    /// </summary>
    public static Boolean HomePageMessageEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.HomePageMessageEnabled",
                    ConfigurationManager.AppSettings["Feature.HomePageMessageEnabled"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.HomePageMessageEnabled"]) == true;

        }

    }

    /// <summary>
    /// Gets the homepage Message to be displayed on login
    /// </summary>
    public static string HomePageMessage
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.HomePageMessage"];
        }
    }

    /// <summary>
    /// Gets the long version of the Home Page Message to be displayed when they click the link
    /// </summary>
    public static string HomePageMessageLong
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.HomePageMessageLong"];
        }
    }

    /// <summary>
    /// Gets the link to the more details for the Home Page Message
    /// </summary>
    public static string HomePageMessageLink
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.HomePageMessageLink"];
        }
    }


    /// <summary>
    /// Gets the long version of the Home Page Message to be displayed when they click the link
    /// </summary>
    public static string HomePageExternalMessage
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.HomePageExternalMessage"];
        }
    }

    /// <summary>
    /// Gets the link to the more details for the Home Page Message
    /// </summary>
    public static string HomePageExternalLink
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.HomePageExternalLink"];
        }
    }
    public static string GuidePageExternalLink
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.GuidePageExternalLink"];
        }
    }
    /// <summary>
    /// Gets the IRS Message to be displayed on the Verification Page
    /// </summary>
    public static string IrsMessage
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.IrsMessage"];
        }
    }

    public static string IrsStatusEmailSubject
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.IrsStatusEmailSubject"];
        }
    }

    /// <summary>
    /// Gets the link to the instructions for the IRS message
    /// </summary>
    public static string IrsInstructionsLink
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.IrsInstructionsLink"];
        }
    }

    /// <summary>
    /// Gets the link to the instructions for the IRS message
    /// </summary>
    public static string IrsInstructionsLink2017
    {
        get
        {
            return ConfigurationManager.AppSettings["Feature.IrsInstructionsLink2017"];
        }
    }

    /// <summary>
    /// Controls the feature that will used the provided email address _or_ will use a configured address instead.
    /// Used in non-production deployments to verify the formatting of emails.
    /// </summary>
    public static Boolean IsRealEmailsEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.UseUsersEmailAddress",
                    ConfigurationManager.AppSettings["Feature.UseUsersEmailAddress"]));

            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.UseUsersEmailAddress"]) == true;

        }

    }

    /// <summary>
    /// Limits the number of Employer Calculations that are run per call
    /// </summary>
    public static int CalculationBatchLimit
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.LimitCalculationBatches",
                    ConfigurationManager.AppSettings["Feature.LimitCalculationBatches"]));

            }

            return int.Parse(ConfigurationManager.AppSettings["Feature.LimitCalculationBatches"]);

        }

    }

    /// <summary>
    /// short timer for status check time out.
    /// </summary>
    public static int ShortTimeStatusCheck
    {
        get
        {
            if(Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.ShortTimeStatusCheck",
                    ConfigurationManager.AppSettings["Feature.ShortTimeStatusCheck"]));

            }

            return int.Parse(ConfigurationManager.AppSettings["Feature.ShortTimeStatusCheck"]);
        }
    }

    /// <summary>
    /// long timer for status check time out.
    /// </summary>
    public static int LongTimeStatusCheck
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.LongTimeStatusCheck",
                    ConfigurationManager.AppSettings["Feature.LongTimeStatusCheck"]));

            }

            return int.Parse(ConfigurationManager.AppSettings["Feature.LongTimeStatusCheck"]);
        }
    }

    /// <summary>
    /// enabling bool ssl for different environment
    /// </summary>
    public static bool EnableSsl
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.EnableSsl",
                    ConfigurationManager.AppSettings["Feature.EnableSsl"]));

            }

            return bool.Parse(ConfigurationManager.AppSettings["Feature.EnableSsl"]);
        }
    }
    
    /// <summary>
    /// Password reset time out in minutes.
    /// </summary>
    public static int PasswordMinute
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.PasswordMinute",
                    ConfigurationManager.AppSettings["Feature.PasswordMinute"]));

            }
            return int.Parse(ConfigurationManager.AppSettings["Feature.PasswordMinute"]);
        }
    }

    /// <summary>
    /// Controls the new Admin Panel in the Admin Site.
    /// </summary>
    public static Boolean NewAdminPanelEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.NewAdminPanelEnabled",
                    ConfigurationManager.AppSettings["Feature.NewAdminPanelEnabled"]));

            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.NewAdminPanelEnabled"]) == true;

        }

    }

    /// <summary>
    /// Controls the ability to edit Measurement Periods from the Client side.
    /// </summary>
    public static Boolean SelfMeasurementPeriodsEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.SelfMeasurementPeriodsEnabled",
                    ConfigurationManager.AppSettings["Feature.SelfMeasurementPeriodsEnabled"]));

            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.SelfMeasurementPeriodsEnabled"]) == true;

        }

    }

    /// <summary>
    /// Controls the registration link and information panel on the default web page for non-authenticated users.
    /// </summary>
    public static Boolean SelfRegistrationEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.SelfRegistrationEnabled",
                    ConfigurationManager.AppSettings["Feature.SelfRegistrationEnabled"]));

            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.SelfRegistrationEnabled"]) == true;

        }

    }

    /// <summary>
    /// Controls the registration link and information panel on the default web page for non-authenticated users.
    /// </summary>
    public static Boolean EtlProcessEnabled
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {

                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.ETLProcess",
                    ConfigurationManager.AppSettings["Feature.ETLProcess"]));

            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.ETLProcess"]) == true;
        }

    }

    /// <summary>
    /// Controls the user managment link and page on the admin site.
    /// </summary>
    public static Boolean ShowDOB
    {

        get
        {

            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.ShowDOB",
                    ConfigurationManager.AppSettings["Feature.ShowDOB"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.ShowDOB"]) == true;

        }

    }

    /// <summary>
    /// Controls the user managment link and page on the admin site.
    /// </summary>
    public static Boolean UserManagementEnabled
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.UserManagement",
                    ConfigurationManager.AppSettings["Feature.UserManagement"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.UserManagement"]) == true;
        }
    }


    /// <summary>
    /// Change Math to speed up Calculations
    /// </summary>
    public static Boolean FastCalculationEnabled
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.FastCalculationEnabled",
                    ConfigurationManager.AppSettings["Feature.FastCalculationEnabled"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["Feature.FastCalculationEnabled"]) == true;
        }
    }

    /// <summary>
    /// Define how many rows to be batch inserted at a time.
    /// </summary>
    public static int BulkBatchSize
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.",
                    "Feature.BulkBatchSize",
                    ConfigurationManager.AppSettings["Feature.BulkBatchSize"]));
            }

            return int.Parse(ConfigurationManager.AppSettings["Feature.BulkBatchSize"]);
        }
    }
    /// <summary>
    /// The Path to the directory where Pdfs are left after they have been printed
    /// </summary>
    public static String PrintPdfDropPath
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.PrintPdfDropPath",
                    ConfigurationManager.AppSettings["Feature.PrintPdfDropPath"]));
            }

            return ConfigurationManager.AppSettings["Feature.PrintPdfDropPath"];
        }
    }
    public static String PrintPdfDropPath1094
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.PrintPdfDropPath1094",
                    ConfigurationManager.AppSettings["Feature.PrintPdfDropPath1094"]));
            }

            return ConfigurationManager.AppSettings["Feature.PrintPdfDropPath1094"];
        }
    }

    public static int CurrentReportingYear
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.CurrentReportingYear",
                    ConfigurationManager.AppSettings["Feature.CurrentReportingYear"]));
            }

            return int.Parse(ConfigurationManager.AppSettings["Feature.CurrentReportingYear"]);
        }
    }

    public static int PreviousReportingYear
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.PreviousReportingYear",
                    ConfigurationManager.AppSettings["Feature.PreviousReportingYear"]));
            }

            return int.Parse(ConfigurationManager.AppSettings["Feature.PreviousReportingYear"]);
        }
    }

    /// <summary>
    /// The Toggle for the Email to be sent on Step 3 being Opened up to the client 
    /// </summary>
    public static bool EnableEmail1095Step3Open
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.EnableEmail1095Step3Open",
                    ConfigurationManager.AppSettings["Feature.EnableEmail1095Step3Open"]));
            }

            return bool.Parse(ConfigurationManager.AppSettings["Feature.EnableEmail1095Step3Open"]);
        }
    }
    
    /// <summary>
    /// The Text for the Email Title to be sent on Step 3 being Opened up to the client 
    /// </summary>
    public static string EmailTitle1095Step3Open
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.EmailTitle1095Step3Open",
                    ConfigurationManager.AppSettings["Feature.EmailTitle1095Step3Open"]));
            }

            return ConfigurationManager.AppSettings["Feature.EmailTitle1095Step3Open"];
        }
    }

    /// <summary>
    /// The Text for the Email Body to be sent on Step 3 being Opened up to the client 
    /// </summary>
    public static string EmailBody1095Step3Open
    {
        get
        {
            if (Feature.Log.IsDebugEnabled)
            {
                Feature.Log.Debug(String.Format("Feature Setting for {0}: {1}.", "Feature.EmailBody1095Step3Open",
                    ConfigurationManager.AppSettings["Feature.EmailBody1095Step3Open"]));
            }

            return ConfigurationManager.AppSettings["Feature.EmailBody1095Step3Open"];
        }
    }
    
    private static ILog Log = LogManager.GetLogger(typeof(Feature));

}
