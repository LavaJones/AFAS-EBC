using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Afas.AfComply.Domain;
using System.IO;

public class RecievedFilesApiController : ApiController
{

    private ILog Log = LogManager.GetLogger(typeof(employeeMonthlyAvgCalculator));

    private int TaxYearId
    {
        get
        {
             return DateTime.Now.AddYears(-1).Year;
        }

    }

    [HttpPost]
    public HttpResponseMessage Printed(string username, string password)
    {
        User currUser = UserController.validateLogin(username, password);

        if (currUser.User_District_ID == 1 && currUser.User_Power == true && currUser.USER_ACTIVE == true)
        { 
            var title = "Printed";
            var message = "File is Printed";
            var body = "Test Body 1";

            return ProcessFile(@"~/ftps/Printed/", "File is printed", currUser, TransmissionStatusEnum.Printed,title,message,body);
        }
        else
        {
            this.Log.Warn(String.Format("{0} is requesting the Printed nightly process to run and has invalid credentials or belongs to the wrong employer!", username));
            SecurityLogger.LogSecurityEvent(String.Format("{0} is not authorized for the Printed nightly process.", username));

            return Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
    
    [HttpPost]
    public HttpResponseMessage CASSReceived(string username, string password)
    {
        User currUser = UserController.validateLogin(username, password);

        if (currUser.User_District_ID == 1 && currUser.User_Power == true && currUser.USER_ACTIVE == true)
        {
            var title = "CASS Received";
            var message = "CASS File is Received";
            var body = "Test Body 1";

            return ProcessFile(@"~/ftps/CASSRecieved/", "CASS file is received", currUser, TransmissionStatusEnum.CASSRecieved,title,message,body);
        }
        else
        {
            this.Log.Warn(String.Format("{0} is requesting the CASS Received nightly process to run and has invalid credentials or belongs to the wrong employer!", username));
            SecurityLogger.LogSecurityEvent(String.Format("{0} is not authorized for the CASS Received nightly process.", username));

            return Request.CreateResponse(HttpStatusCode.Forbidden);
        }

    }

    private HttpResponseMessage ProcessFile(string folderPath, string reason, User currUser, TransmissionStatusEnum transmissionStatus, string title, string message, string body)
    {
        var httpRequest = HttpContext.Current.Request;
        if (httpRequest.Files.Count < 1)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        try
        {
            foreach (string file in httpRequest.Files)
            {
                
               var postedFile = httpRequest.Files[file];
               Guid employerResourceId = getEmployerResourceIdFromFileStream(postedFile.InputStream);
                
               var fullFileName = folderPath + postedFile.FileName;

                var filePath = HttpContext.Current.Server.MapPath(fullFileName);
                postedFile.SaveAs(filePath);

                EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerResourceIdAndTaxYearId(employerResourceId, TaxYearId);
                employerController.endEmployerTaxYearTransmissionStatus(currentEmployerTaxYearTransmissionStatus, currUser.User_UserName);

                new FileArchiverWrapper().ArchiveFile(fullFileName, currentEmployerTaxYearTransmissionStatus.EmployerId, reason);

                var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                    currentEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                    transmissionStatus,
                    currUser.User_UserName
                );

                employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
                
                var newEmployerTaxYearTransmissionStatusQueue = new EmployerTaxYearTransmissionStatusQueue(
                    newEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId,
                    title,
                    fullFileName,
                    body,
                    currUser.User_UserName
                );

                employerController.insertUpdateEmployerTaxYearTransmissionStatusQueue(newEmployerTaxYearTransmissionStatusQueue);

                var employer = employerController.getEmployer(currentEmployerTaxYearTransmissionStatus.EmployerId);
                Email email = new Email();
                body = string.Format("Employer \nName: {0}\n EIN: {1}\n Transmission Status: {2}\n ",
                    employer.EMPLOYER_NAME, employer.EMPLOYER_EIN, currentEmployerTaxYearTransmissionStatus.TransmissionStatusId);
                email.SendEmail(SystemSettings.IrsProcessingAddress, Feature.IrsStatusEmailSubject, body, false);

            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception: {0}",ex.Message));
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }

    [HttpPost]
    public HttpResponseMessage ProcessPendingCassFileEmployerTaxYearTransmissionStatusQueue(string username, string password)
    {
        HttpStatusCode statusCode;

        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AIR_Conn"].ConnectionString;
        SqlConnection conn = null;
        conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        conn.Open();
        cmd.CommandText = "etl.spUpdate_employee_from_file";
        cmd.Connection = conn;
        cmd.CommandType = CommandType.StoredProcedure;

        EmployerTaxYearTransmissionStatusQueue employerTaxYearTransmissionStatusQueue = new EmployerTaxYearTransmissionStatusQueue();

        try
        {
            User currUser = UserController.validateLogin(username, password);

            List<EmployerTaxYearTransmissionStatusQueue> employerTaxYearTransmissionStatusQueues = employerController.getEmployerTaxYearTransmissionStatusQueues(10);

            foreach (EmployerTaxYearTransmissionStatusQueue _employerTaxYearTransmissionStatusQueue in employerTaxYearTransmissionStatusQueues)
            {
                employerTaxYearTransmissionStatusQueue = _employerTaxYearTransmissionStatusQueue;

                EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus = employerController.getEmployerTaxYearTransmissionStatusById(employerTaxYearTransmissionStatusQueue.EmployerTaxYearTransmissionStatusId);

                if (employerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.CASSRecieved)
                {
                    employerTaxYearTransmissionStatusQueue.QueueStatusId = QueueStatusEnum.Processing;
                    employerController.insertUpdateEmployerTaxYearTransmissionStatusQueue(employerTaxYearTransmissionStatusQueue);

                    List<Employee_IRS> employees = new List<Employee_IRS>();
                    employees.AddRange(getEmployeeInfoFromCSVFile(employerTaxYearTransmissionStatusQueue.Message));

                    foreach (Employee_IRS employee in employees)
                    {
                        updateEmployeeIRS(cmd, employee);
                    }

                    employerTaxYearTransmissionStatusQueue.QueueStatusId = QueueStatusEnum.Completed;
                    employerController.insertUpdateEmployerTaxYearTransmissionStatusQueue(employerTaxYearTransmissionStatusQueue);

                    employerController.endEmployerTaxYearTransmissionStatus(employerTaxYearTransmissionStatus, currUser.User_UserName);
                }

            }

            statusCode = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            if (employerTaxYearTransmissionStatusQueue.EmployerTaxYearTransmissionStatusQueueId > 0)
            {
                employerTaxYearTransmissionStatusQueue.QueueStatusId = QueueStatusEnum.Pending;
                employerController.insertUpdateEmployerTaxYearTransmissionStatusQueue(employerTaxYearTransmissionStatusQueue);
            }

            this.Log.Error(string.Format("Exception: ", ex.Message));
            
            statusCode = HttpStatusCode.InternalServerError;

        }

        if (cmd != null)
        {
            cmd.Dispose();
        }

        if (conn != null)
        {
            conn.Close();
            conn.Dispose();
        }

        return Request.CreateResponse(statusCode);

    }

    private Guid getEmployerResourceIdFromFileStream(Stream inputStream)
    {
        int row = 0;
        int EMPLOYER_RESOURCE_ID = -1;
        Guid employerResourceId = Guid.Empty;
        using (StreamReader sr = new StreamReader(inputStream))
        {
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (row == 0)
                {
                    string[] header = line.Split(',');
                    EMPLOYER_RESOURCE_ID = Array.FindIndex(header, r => r.Contains("EmployerResourceId"));
                }
                else
                {
                    string[] values = line.Split(',');
                    employerResourceId = Guid.Parse(values[EMPLOYER_RESOURCE_ID]);
                    break;
                }
                row++;
            }
        }

        return employerResourceId;

    }

    private List<Employee_IRS> getEmployeeInfoFromCSVFile(string fullFilePath)
    {
        string[] csvRows = File.ReadAllLines(fullFilePath);

        int row = 0;

        int EMPLOYER_RESOURCE_ID = -1;
        int EMPLOYEE_RESOURCE_ID = -1;
        int F_NAME = -1;
        int MI = -1;
        int L_NAME = -1;
        int ADDRESS = -1;
        int CITY = -1;
        int STATE = -1;
        int ZIP = -1;
        int BIRTHDT = -1;
        int CONTACT_PHONE_NUM = -1;

        List<Employee_IRS> employees = new List<Employee_IRS>();

        foreach (string csvRow in csvRows)
        {
            if (row == 0)
            {
                string[] header = csvRow.Split(',');
                EMPLOYER_RESOURCE_ID = Array.FindIndex(header, r => r.Contains("EmployerResourceId"));
                EMPLOYEE_RESOURCE_ID = Array.FindIndex(header, r => r.Contains("EmployeeResourceId"));
                F_NAME = Array.FindIndex(header, r => r.Contains("Fname"));
                MI = Array.FindIndex(header, r => r.Contains("Mi"));
                L_NAME = Array.FindIndex(header, r => r.Contains("Lname"));
                ADDRESS = Array.FindIndex(header, r => r.Contains("Address"));
                CITY = Array.FindIndex(header, r => r.Contains("City"));
                STATE = Array.FindIndex(header, r => r.Contains("State"));
                ZIP = Array.FindIndex(header, r => r.Contains("ZIP"));
                BIRTHDT = Array.FindIndex(header, r => r.Contains("PersonBirthDt"));
                CONTACT_PHONE_NUM = Array.FindIndex(header, r => r.Contains("ContactPhoneNum"));
            }
            else
            {

                string[] values = csvRow.Split(',');

                employees.Add(
                    new Employee_IRS()
                    {
                        EmployeeResourceId = Guid.Parse(values[EMPLOYEE_RESOURCE_ID]),
                        EmployerResourceId = Guid.Parse(values[EMPLOYER_RESOURCE_ID]),
                        Fname = values[F_NAME],
                        Mi = values[MI],
                        Lname = values[L_NAME],
                        Address = values[ADDRESS],
                        City = values[CITY],
                        State = values[STATE],
                        ZIP = values[ZIP],
                        ContactPhoneNum = values[CONTACT_PHONE_NUM],
                        PersonBirthDt = DateTime.Parse(values[BIRTHDT])
                    }
                );

            }

            row++;
        }

        return employees;
    }

    public void updateEmployeeIRS(SqlCommand cmd, Employee_IRS employee)
    {
        cmd.Parameters.AddWithValue("@first_name", SqlDbType.NVarChar).Value = employee.Fname;
        cmd.Parameters.AddWithValue("@middle_name", SqlDbType.NVarChar).Value = employee.Mi;
        cmd.Parameters.AddWithValue("@last_name", SqlDbType.NVarChar).Value = employee.Lname;
        cmd.Parameters.AddWithValue("@address", SqlDbType.NVarChar).Value = employee.Address;
        cmd.Parameters.AddWithValue("@city", SqlDbType.NVarChar).Value = employee.City;
        cmd.Parameters.AddWithValue("@state_code", SqlDbType.NChar).Value = employee.State;
        cmd.Parameters.AddWithValue("@zipcode", SqlDbType.NChar).Value = employee.ZIP;
        cmd.Parameters.AddWithValue("@telephone", SqlDbType.NVarChar).Value = employee.ContactPhoneNum;
        cmd.Parameters.AddWithValue("@employerResourceId", SqlDbType.UniqueIdentifier).Value = employee.EmployerResourceId;
        cmd.Parameters.AddWithValue("@employeeResourceId", SqlDbType.UniqueIdentifier).Value = employee.EmployeeResourceId;

        cmd.ExecuteNonQuery();

    }
    
}