using System;
using System.Configuration;

using log4net;

// Missing namespace is intentional.

/// <summary>
/// Helper class to handle all of the System Wide Settings deployment variables.
/// </summary>
public static class SystemSettings
{

    /// <summary>
    /// The email address used for/to/cc for billing invoices.
    /// </summary>
    public static String BillingEmailAddress
    {

        get
        {

            if (SystemSettings.Log.IsDebugEnabled)
            {

                SystemSettings.Log.Debug(String.Format("System Setting for {0}: {1}.", "System.BillingEmailAddress",
                    ConfigurationManager.AppSettings["System.BillingEmailAddress"]));

            }

            return ConfigurationManager.AppSettings["System.BillingEmailAddress"].ToString();

        }

    }

    /// <summary>
    /// Email address for all system generated emails.
    /// </summary>
    public static String EmailNotificationAddress
    {

        get
        {

            if (SystemSettings.Log.IsDebugEnabled)
            {

                SystemSettings.Log.Debug(String.Format("System Setting for {0}: {1}.", "System.EmailNotificationAddress",
                    ConfigurationManager.AppSettings["System.EmailNotificationAddress"]));

            }

            return ConfigurationManager.AppSettings["System.EmailNotificationAddress"];
        
        }

    }

    /// <summary>
    /// Email address for issues during processing of data
    /// </summary>
    public static String ProcessingFailedAddress
    {
        get
        {
            return ConfigurationManager.AppSettings["System.ProcessingFailedAddress"];
        }
    }  

    /// <summary>
    /// Email address used instead of the database/code generated email address. Used during non-production deployments
    /// </summary>
    public static String EmailOverrideAddress
    {

        get
        {

            if (SystemSettings.Log.IsDebugEnabled)
            {

                SystemSettings.Log.Debug(String.Format("System Setting for {0}: {1}.", "System.EmailOverrideAddress",
                    ConfigurationManager.AppSettings["System.EmailOverrideAddress"]));

            }

            return ConfigurationManager.AppSettings["System.EmailOverrideAddress"];
        
        }

    }


    public static string IrsProcessingAddress
    {
        get
        {
            return ConfigurationManager.AppSettings["System.IrsProcessingAddress"];
        }
    }


    /// <summary>
    /// The User Id that is used to test
    /// </summary>
    public static int UserDbId
    {

        get
        {
            // this is spamming the logs, and is only used bt status check
            //if (SystemSettings.Log.IsDebugEnabled)
            //{

            //    SystemSettings.Log.Debug(String.Format("System Setting for {0}: {1}.", "System.UserDbId",
            //        ConfigurationManager.AppSettings["System.UserDbId"]));

            //}

            return int.Parse(ConfigurationManager.AppSettings["System.UserDbId"]);

        }

    }
 
    /// <summary>
    /// Defines if Performance data should be writtne into a specific log file, false uses the Main log file
    /// </summary>
    public static Boolean UsePerformanceLog
    {
        get
        {
            if (SystemSettings.Log.IsDebugEnabled)
            {
                SystemSettings.Log.Debug(
                    String.Format(
                        "System Setting for {0}: {1}.",
                        "System.UsePerformanceLog",
                        ConfigurationManager.AppSettings["System.UsePerformanceLog"]));
            }

            return Boolean.Parse(ConfigurationManager.AppSettings["System.UsePerformanceLog"]);
        }
    }

    private static ILog Log = LogManager.GetLogger(typeof(SystemSettings));

}
