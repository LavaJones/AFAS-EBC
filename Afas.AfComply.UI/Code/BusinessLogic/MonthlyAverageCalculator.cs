using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;

    public class MonthlyAverageCalculator
    {

        private ILog Log = LogManager.GetLogger(typeof(MonthlyAverageCalculator));

        private String connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        public void CalculateForEmployer(int _employerID)
        {

            String report = "Plan Year Average Calculation Report: " + DateTime.Now.ToLongDateString() + "<br />";

            Log.Info(String.Format("Daily Calculation started for employer id: {0}", _employerID));

            employer employer = employerController.getEmployer(_employerID);

            try
            {
                bool complete = false;

                AverageHoursCalculator calc = new AverageHoursCalculator();
                complete = calc.CalculateAveragesForEmployer(_employerID);
 
                
                Log.Info(string.Format("Daily Calculation Finished for employer id: {0} with result {1}", _employerID, complete));

                if (complete == true)
                {

                    report += employer.EMPLOYER_NAME + ": Complete <br />";

                    NightlyCalculationFactory.updateNightlyCalculation(_employerID);
                
                }
                else
                {
                
                    report += employer.EMPLOYER_NAME + ": Failed <br />";

                    NightlyCalculationFactory.updateFailNightlyCalculation(_employerID);

                    String emailSubject = "Calculations failed for Employer: [" + employer.EMPLOYER_NAME + "]";
                    String emailMessage = "Calculations failed for Employer: [" + employer.EMPLOYER_NAME + "]";
                    Email mailer = new Email();

                    mailer.SendEmail(SystemSettings.ProcessingFailedAddress, emailSubject, emailMessage, true);
                
                }

                this.Log.Info(report);
            
            }
            catch (Exception exception)
            {
            
                this.Log.Error("Exception During Calculate for employer Id: [" + _employerID + "], Marking Queue item as failed.", exception);

                NightlyCalculationFactory.updateFailNightlyCalculation(_employerID);

                String emailSubject = "Exception during Calculation for Employer: [" + employer.EMPLOYER_NAME + "]";
                String emailMessage = "Exception during Calculation for Employer [" + employer.EMPLOYER_NAME + "]\n\r"+exception.Message+"\n\r"+exception.StackTrace;
                Email mailer = new Email();

                mailer.SendEmail(SystemSettings.ProcessingFailedAddress, emailSubject, emailMessage, false);
            
            }
        
        }
    
    }

