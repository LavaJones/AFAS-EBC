using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

    /// <summary>
    /// A static helper class to simplify the logging of PII while we work on a more permanent solution
    /// </summary>
    public static class PIILogger
    {

        private static ILog log = LogManager.GetLogger(String.Format("PIILogging.{0}", typeof(PIILogger).FullName));

        /// <summary>
        /// Adds a Log message using the PIILogging.__ logger.
        /// </summary>
        /// <param name="message">THe message to be added to the log</param>
        public static void LogPII(string message) 
        {
            try
            {
                if (log.IsInfoEnabled) 
                {
                    log.Info(message); 
                }
            }
            catch (Exception exception)
            {
                log.Error("Unable to log PII data access.", exception);
            }
        }
    }
