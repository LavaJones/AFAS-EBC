using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using log4net;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class employeeMonthlyAvgCalculator : System.Web.Services.WebService 
{

    [WebMethod]
    public bool process_MonthlyACThours(string _username, string _password)
    {    

        bool processComplete = false;
        DateTime _modOn = DateTime.Now;

        try
        {

      

            Server.ScriptTimeout = 2700;

            this.Log.Info(String.Format("{0} is requesting the nightly process to run.", _username));
            
            SecurityLogger.LogSecurityEvent(String.Format("{0} is requesting the nightly process.", _username));

            User currUser = UserController.validateLogin(_username, _password);

            if (currUser.User_District_ID == 1 && currUser.User_Power == true && currUser.USER_ACTIVE == true)
            {

                List<int> employerData = NightlyCalculationFactory.getNightlyCalculationEmployerId();

                Email reportEmail2 = new Email();
                reportEmail2.SendEmail(SystemSettings.EmailNotificationAddress, "Daily Calculation: Started", "Calculation Started", false);

                this.Log.Info("Daily Calculation: Started");
                this.Log.Info(String.Format("Processing {0} employers.", employerData.Count));

                MonthlyAverageCalculator calc = new MonthlyAverageCalculator();

                foreach (int value in employerData)
                {
                    calc.CalculateForEmployer(value);
                }

                this.Log.Info("All Daily Calculation: Completed");

                reportEmail2.SendEmail(SystemSettings.EmailNotificationAddress, "Daily Calculation: Completed", "Calculation Complete", false);
                
                processComplete = true;

            }
            else
            {

                this.Log.Warn(String.Format("{0} is requesting the nightly process to run and has invalid credentials or belongs to the wrong employer!", _username));

                SecurityLogger.LogSecurityEvent(String.Format("{0} is not authorized for the nightly process.", _username));

                processComplete = false;
            
            }

        }
        catch (Exception exception)
        {
            
            this.Log.Warn("Suppressing errors.", exception);

            processComplete = false;        
        
        }
        
        return processComplete;

    }

    private ILog Log = LogManager.GetLogger(typeof(employeeMonthlyAvgCalculator));

    private string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

}
