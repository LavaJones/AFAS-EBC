using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using Afas.AfComply.Domain;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Net.Http;
using Afas.Domain;

/// <summary>
/// Summary description for moveGeneratedCassFile
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//[System.ComponentModel.ToolboxItem(false)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class moveGeneratedCassFile : System.Web.Services.WebService
{

    private ILog Log = LogManager.GetLogger(typeof(employeeMonthlyAvgCalculator));

    private int TaxYearId
    {
        get
        {
            return DateTime.Now.AddYears(-1).Year;
        }

    }

    private Regex guidRegex = new Regex("^{[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12}}$");

    [WebMethod]
    public bool processGeneratedCassFile(string filename, string username, string password)
    {

        bool processComplete = false;
        var processName = "GeneratedCassFile";

        try
        {
            if (ValidateParameters(processName, filename, username, password))
            {
                var employersWithCassGenerated = GetEmployersByCurrentTransactionStatus(TransmissionStatusEnum.CASSGenerated);
                var employer = employersWithCassGenerated.FirstOrDefault(e => filename.Contains(e.ResourceId.ToString()));
                if (employer == null)
                {
                    LogEmployerWithResourceIdNotFound("CASSGenerated", filename, processName);
                }
                else
                {
                    EndCurrentEmployerTaxYearTransmissionStatus(employer.employer_id, username, password);
                    processComplete = true;
                }
            }
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }

        return processComplete;

    }

    [WebMethod]
    public bool processPrintFile(string filename, string username, string password)
    {

        bool processComplete = false;
        var processName = "PrintFile";

        try
        {
            if (ValidateParameters(processName, filename, username, password))
            {
                var employersReadyToPrint = GetEmployersByCurrentTransactionStatus(TransmissionStatusEnum.Print);
                var employer = employersReadyToPrint.FirstOrDefault(e => filename.Contains(e.ResourceId.ToString()));
                if (employer == null)
                {
                    LogEmployerWithResourceIdNotFound("Print", filename, processName);
                }
                else
                {
                    EndCurrentEmployerTaxYearTransmissionStatus(employer.employer_id, username, password);
                    processComplete = true;
                }
            }
        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }

        return processComplete;

    }

    private User getCurrentUser(string username, string password)
    {
        return UserController.validateLogin(username, password);
    }

    private bool ValidateParameters(string proccessName, string filename, string username, string password)
    {
        this.Log.Info(String.Format("{0} is requesting the {1} nightly process to run.", username, proccessName));
            
        SecurityLogger.LogSecurityEvent(String.Format("{0} is requesting the {1} nightly process.", username, proccessName));

        //Step 1: Validate Credentials. 
        User currUser = getCurrentUser(username, password);

        //Step 2: Verfiy user has the correct authority to call this function.  
        if (currUser.User_District_ID == 1 && currUser.User_Power == true && currUser.USER_ACTIVE == true)
        {

            String safeFilename = filename.ToLower();
            safeFilename = safeFilename.Replace(".txt", String.Empty);
            safeFilename = safeFilename.Replace(".pdf", String.Empty);
            safeFilename = safeFilename.Replace("1095B_", String.Empty);
            safeFilename = safeFilename.Replace("1095C_", String.Empty);
            safeFilename = safeFilename.Replace("1094B_", String.Empty);
            safeFilename = safeFilename.Replace("1094C_", String.Empty);

            Guid validGuid = Guid.Empty;

            if (Guid.TryParse(safeFilename, out validGuid) == true)
            {
                return true;
            }
            else
            {
                this.Log.Warn(String.Format("{0} is an invalid file for the {1} nightly process!", filename, proccessName));
                SecurityLogger.LogSecurityEvent(String.Format("{0} is not a valid file for the {1} nightly process.", filename, proccessName));
                return false;
            }
        }
        else
        {
            this.Log.Warn(String.Format("{0} is requesting the {1} nightly process to run and has invalid credentials or belongs to the wrong employer!", username, proccessName));
            SecurityLogger.LogSecurityEvent(String.Format("{0} is not authorized for the {1} nightly process.", username, proccessName));
            return false;
        }
    }

    private List<IRSEntity> GetEmployersByCurrentTransactionStatus(TransmissionStatusEnum transmissionStatus)
    {
        DataTable dtEmployersByCurrentTransactionStatus = new DataTable();

        using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
        {
            using (var cmd = new SqlCommand("SELECT_employers_in_transmission_status_for_tax_year", connString))
            {
                int taxYearID = DateTime.Now.AddYears(-1).Year;
                cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearID;

                cmd.Parameters.AddWithValue("@transmissionStatusId", SqlDbType.Int).Value = transmissionStatus;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployersByCurrentTransactionStatus);
                }
            }
        }

        return dtEmployersByCurrentTransactionStatus.DataTableToList<IRSEntity>();
    }

    private void LogEmployerWithResourceIdNotFound(string transmissionStatusName, string filename, string processName)
    {

        Match match = guidRegex.Match(filename);

        var message = "The list of employers with a current status of " + transmissionStatusName + " did not contain an employer with a ResourceId equal to {0} for filename {1}!";
        this.Log.Warn(String.Format(message, match.Value, filename));
        SecurityLogger.LogSecurityEvent(String.Format(message + " for the {2} nightly process.", match.Value, filename, processName));
    }

    private void EndCurrentEmployerTaxYearTransmissionStatus(int employerId, string username, string password)
    {
        EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, TaxYearId);
        User currUser = getCurrentUser(username, password);
        employerController.endEmployerTaxYearTransmissionStatus(employerTaxYearTransmissionStatus, currUser.User_UserName);
    }

}

