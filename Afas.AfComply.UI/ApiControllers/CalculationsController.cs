using System;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Net.Http;

using System.Web.Http;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

using log4net;

namespace Afas.AfComply.UI.ApiControllers
{

    /// <summary>
    /// REST wrapper around all of the automated calculations
    /// </summary>
    public class CalculationsController : BaseWebApiController
    {

        public CalculationsController() : base(LogManager.GetLogger(typeof(CalculationsController))) { }

        public HttpResponseMessage ProcessQueuedPayrollCalculations(Guid? authorizationKey)
        {

            if (authorizationKey.HasValue == false)
            {

                this.Log.Warn(String.Format("Received unauthorized request. AuthorizationKey is missing."));

                return Request.CreateResponse(HttpStatusCode.NotFound, "Resource not found.");

            }

            IEnumerable<String> headers;
            Boolean located = Request.Headers.TryGetValues("X-AUTH-TOKEN", out headers);

            if (located == false)
            {

                this.Log.Warn(String.Format("Received unauthorized request. AuthorizationToken is missing."));

                return Request.CreateResponse(HttpStatusCode.NotFound, "Resource not found.");

            }

            String authorizationToken = headers.Single();
            IList<User> users = UserController.getDistrictUsers(1);

            if (users.FilterForResourceId(authorizationKey.Value).Count() == 0)
            {

                this.Log.Warn(String.Format("Received unauthorized request. AuthorizationKey does not match a valid user in master employer records."));

                return Request.CreateResponse(HttpStatusCode.NotFound, "Resource not found.");

            }

            User user = users.FilterForResourceId(authorizationKey.Value).Single();

            String _username = user.User_UserName;
            String _password = authorizationToken;

            Boolean processComplete = false;
            int processed = 0;
            int waitingForProcessing = 0;
            DateTime modifiedDate = DateTime.Now;
            try
            {

                this.Log.Info(String.Format("{0} is requesting the nightly process to run.", _username));

                SecurityLogger.LogSecurityEvent(String.Format("{0} is requesting the nightly process.", _username));

                User currUser = UserController.validateLogin(_username, _password);

                if (currUser.User_District_ID == 1 && currUser.User_Power == true && currUser.USER_ACTIVE == true)
                {

                    IList<int> employerData = getNightlyCalculationEmployerId();

                    Email reportEmail2 = new Email();
                    reportEmail2.SendEmail(SystemSettings.EmailNotificationAddress, "Daily Calculation: Started", "Calculation Started", false);

                    this.Log.Info("Daily Calculation: Started");
                    this.Log.Info(String.Format("Processing {0} employers.", employerData.Count));

                    MonthlyAverageCalculator calc = new MonthlyAverageCalculator();

                    foreach (int value in employerData)
                    {
                        
                        calc.CalculateForEmployer(value);

                        processed++;

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

                waitingForProcessing = remainingItemsInQueue();


            }
            catch (Exception exception)
            {

                this.Log.Warn("Suppressing errors.", exception);

                processComplete = false;

            }

            IDictionary<String, String> response = new Dictionary<String, String>();
            response["success"] = processComplete.ToString();
            response["processed"] = processed.ToString();
            response["process_pending"] = waitingForProcessing.ToString();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        private int remainingItemsInQueue()
        {

            IList<int> employerData = new List<int>();

            using (SqlConnection conn = new SqlConnection(connString))
            {

                String query = "SELECT [EmployerId] FROM [dbo].[NightlyCalculation] WHERE ProcessStatus = 0 AND ProcessFail = 0 GROUP BY [EmployerId];";
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employerData.Add(reader.GetInt32(0));
                        }
                    }

                }

            }

            return employerData.Count;

        }

        private IList<int> getNightlyCalculationEmployerId()
        {

            IList<int> employerData = new List<int>();

            using (SqlConnection conn = new SqlConnection(connString))
            {

                String query = "SELECT TOP " + Feature.CalculationBatchLimit + " [EmployerId] FROM [dbo].[NightlyCalculation] WHERE ProcessStatus = 0 AND ProcessFail = 0 GROUP BY [EmployerId];";
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            employerData.Add(reader.GetInt32(0));
                        }

                        return employerData;

                    }

                }

            }

        }

        private String connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

    }

}