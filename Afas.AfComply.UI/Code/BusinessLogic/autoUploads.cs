using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using log4net;

/// <summary>
/// Summary description for autoUploads
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class autoUploads : System.Web.Services.WebService {
    private ILog Log = LogManager.GetLogger(typeof(autoUploads));

    public autoUploads () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// This function will Import all Demographic files into the Import_Employee table. 
    /// </summary>
    /// <param name="_username"></param>
    /// <param name="_password"></param>
    /// <returns></returns>
    [WebMethod]
    public bool process_DEM_files(string _username, string _password) 
    {
        bool processComplete = false;
        DateTime _modOn = DateTime.Now;

        try
        {
            //Step 1: Validate Credentails. 
            User currUser = UserController.validateLogin(_username, _password);

            //Step 2: Verfiy user has the correct authority to call this function.  
            if (currUser.User_District_ID == 1 && currUser.User_Power == true && currUser.USER_ACTIVE == true)
            {
                //Step 3: Get all Employer objects that are qued for auto file uploads. 
                List<employer> employerList = employerController.getAllEmployersAutoUpload();

                //Step 4: Loop through each Employer.
                foreach (employer emp in employerList)
                { 
                    //Step 5: Get all DEMOGRAPHIC files. 
                    List<FileInfo> demList = FileProcessing.getFtpFiles(emp.EMPLOYER_IMPORT_EMPLOYEE);

                    //Step 6: Loop through each PAY file and try to IMPORT it. 
                    foreach (FileInfo fi in demList)
                    {
                        string filePath = fi.DirectoryName + "\\";
                        bool validFile = EmployeeController.ProcessDemographicImportFiles(emp.EMPLOYER_ID, _username, _modOn, filePath, fi.Name);
                        
                        if (validFile == true)
                        {
                            //Step 7: Cross referance all DEMOGRAPHIC data. 
                            EmployeeController.CrossReferenceDemographicImportTableData(emp.EMPLOYER_ID, 0, 0, 0, 0);

                            //Step 8: Transfer good records to the actual EMPLOYEE table. 
                            EmployeeController.TransferDemographicImportTableData(emp.EMPLOYER_ID, currUser.User_UserName, false, true);
                        }
                    }
                }
                processComplete = true;
            }
            else
            {
                processComplete = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            processComplete = false;
        }

        return processComplete;
    }



     /// <summary>
    /// This function will Import all PAYROLL files into the Import_Payroll table. 
    /// </summary>
    /// <param name="_username"></param>
    /// <param name="_password"></param>
    /// <returns></returns>
    [WebMethod]
    public bool process_PAY_files(string _username, string _password)
    {
        bool processComplete = false;
        DateTime _modOn = DateTime.Now;

        try
        {
            //Step 1: Validate Credentails. 
            User currUser = UserController.validateLogin(_username, _password);

            //Step 2: Verfiy user has the correct authority to call this function.  
            if (currUser.User_District_ID == 1 && currUser.User_Power == true && currUser.USER_ACTIVE == true)
            {
                //Step 3: Get all Employer objects that are qued for auto file uploads. 
                List<employer> employerList = employerController.getAllEmployersAutoUpload();

                //Step 4: Loop through each Employer.
                foreach (employer emp in employerList)
                {
                    //Step 5: Get all DEM files. 
                    List<FileInfo> demList = FileProcessing.getFtpFiles(emp.EMPLOYER_IMPORT_PAYROLL);

                    //Step 6: Loop through each PAY file and try to IMPORT it. 
                    foreach (FileInfo fi in demList)
                    {
                        string filePath = fi.DirectoryName + "\\";

                        //Step 6.1: Import each Payroll File into the Database.
                        bool validFile = Payroll_Controller.process_PAY_I_files(emp.EMPLOYER_ID, _username, _modOn, filePath, fi.Name);

                        if (validFile == true)
                        {
                            //N) Cross referance all records.
                            Payroll_Controller.CrossReferenceData(emp.EMPLOYER_ID, _username, _modOn);

                            //O) Transfer all data from IMPORT table to the live EMPLOYEE table. 
                            Payroll_Controller.TransferPayrollRecords(emp.EMPLOYER_ID, _username, _modOn);

                            // Queue up the calculation
                            employerController.insertEmployerCalculation(emp.EMPLOYER_ID);
                        }
                    }
                }
                processComplete = true;
            }
            else
            {
                processComplete = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            processComplete = false;
        }

        return processComplete;
    }
    
}
