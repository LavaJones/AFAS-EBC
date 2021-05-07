using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

using log4net;

using Afas.AfComply.Domain;

/// <summary>
/// Summary description for employerShow
/// </summary>
public class employerShow
{

    /// <summary>
    /// Build and return a single Employer Object. 
    /// </summary>
    public employer GetEmployer(int employerId)
    {

        try
        {

            sqlConnection = new SqlConnection(acaConnectionString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = employerId;

            sqlDataReader = cmd.ExecuteReader();

            List<employer> tempList = new List<employer>();

            BuildEmployerObjects(sqlDataReader, tempList);

            if (tempList.Count == 1)
            {
                return tempList[0];
            }
            else
            {
                Log.Error("Database did not return the expected one, and only one Employer for ID: ["+ employerId +"] it instead returned #:"+ tempList.Count);

                return null;
            }
        }
        catch (Exception exception)
        {

            this.Log.Error("Error while getting Employer from Database. EmployerId: "+ employerId, exception);

            return null;        
        }    
    }
  

    /// <summary>
    /// Manufacture a List of all Active Employers.
    /// </summary>
    /// <returns></returns>
    public List<employer> GetAllEmployers()
    {

        List<employer> tempList = new List<employer>();

        try
        {

            sqlConnection = new SqlConnection(acaConnectionString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand("SELECT_all_employers", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlDataReader = cmd.ExecuteReader();

            BuildEmployerObjects(sqlDataReader, tempList);

            return tempList;

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            return tempList;
        
        }
    
    }

    public List<employer> GetAll1095FinalizedEmployers()
    {

        List<employer> tempList = new List<employer>();

        try
        {

            sqlConnection = new SqlConnection(acaConnectionString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand("SELECT_all_1095FinalizedEmployers", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlDataReader = cmd.ExecuteReader();

            BuildEmployerObjects(sqlDataReader, tempList);

            return tempList;

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            return tempList;

        }

    }


    public List<employer> GetAllEmployersAutoUpload()
    {

        List<employer> tempList = new List<employer>();

        try
        {

            sqlConnection = new SqlConnection(acaConnectionString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_autoupload", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlDataReader = cmd.ExecuteReader();

            BuildEmployerObjects(sqlDataReader, tempList);

            return tempList;

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            return tempList;
        
        }
    
    }

    /// <summary>
    /// Manufacture a List of all Active Employers.
    /// </summary>
    /// <returns></returns>
    public List<employer> GetAllEmployersBilling()
    {

        List<employer> tempList = new List<employer>();

        try
        {

            sqlConnection = new SqlConnection(acaConnectionString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_billing", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            sqlDataReader = cmd.ExecuteReader();

            BuildEmployerObjects(sqlDataReader, tempList);

            return tempList;

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);
            
            return tempList;
        
        }
    
    }

    /// <summary>
    /// Convert the SQL Server version of employer into the domain version of Employer. 
    /// </summary>
    public List<employer> BuildEmployerObjects(SqlDataReader sqlDataReader, List<employer> tempList)
    {

        while (sqlDataReader.Read())
        {

            int id = sqlDataReader.GetFieldForNamedColumn("employer", "employer_id").checkIntNull();
            String name = sqlDataReader.GetFieldForNamedColumn("employer", "name").checkStringNull(); ;
            String address = sqlDataReader.GetFieldForNamedColumn("employer", "address").checkStringNull();
            String city = sqlDataReader.GetFieldForNamedColumn("employer", "city").checkStringNull();
            int stateId = sqlDataReader.GetFieldForNamedColumn("employer", "state_id").checkIntNull();
            String zip = sqlDataReader.GetFieldForNamedColumn("employer", "zip").checkStringNull();
            String logo = sqlDataReader.GetFieldForNamedColumn("employer", "img_logo").checkStringNull();
            String billAddress = sqlDataReader.GetFieldForNamedColumn("employer", "bill_address").checkStringNull();
            String billCity = sqlDataReader.GetFieldForNamedColumn("employer", "bill_city").checkStringNull();
            int billStateId = sqlDataReader.GetFieldForNamedColumn("employer", "bill_state").checkIntNull();
            String billZip = sqlDataReader.GetFieldForNamedColumn("employer", "bill_zip").checkStringNull();
            int employerTypeId = sqlDataReader.GetFieldForNamedColumn("employer", "employer_type_id").checkIntNull();
            String federalId = sqlDataReader.GetFieldForNamedColumn("employer", "ein").checkStringNull();
            int initialMeasurementId = sqlDataReader.GetFieldForNamedColumn("employer", "initial_measurement_id").checkIntNull();
            String importDemographics = sqlDataReader.GetFieldForNamedColumn("employer", "import_demo").checkStringNull();
            String importPayroll = sqlDataReader.GetFieldForNamedColumn("employer", "import_payroll").checkStringNull();
            String iei = sqlDataReader.GetFieldForNamedColumn("employer", "iei").checkStringNull();
            String iec = sqlDataReader.GetFieldForNamedColumn("employer", "iec").checkStringNull();
            String ftpei = sqlDataReader.GetFieldForNamedColumn("employer", "ftpei").checkStringNull();
            String ftpec = sqlDataReader.GetFieldForNamedColumn("employer", "ftpec").checkStringNull();
            String ipi = sqlDataReader.GetFieldForNamedColumn("employer", "ipi").checkStringNull();
            String ipc = sqlDataReader.GetFieldForNamedColumn("employer", "ipc").checkStringNull();
            String ftppi = sqlDataReader.GetFieldForNamedColumn("employer", "ftppi").checkStringNull();
            String ftppc = sqlDataReader.GetFieldForNamedColumn("employer", "ftppc").checkStringNull();
            String importProcess = sqlDataReader.GetFieldForNamedColumn("employer", "importProcess").checkStringNull();
            int vendorId = sqlDataReader.GetFieldForNamedColumn("employer", "vendor_id").checkIntNull();
            Boolean autoUpload = sqlDataReader.GetFieldForNamedColumn("employer", "autoUpload").checkBoolNull();
            Boolean autoBill = sqlDataReader.GetFieldForNamedColumn("employer", "autoBill").checkBoolNull();
            Boolean suBilled = sqlDataReader.GetFieldForNamedColumn("employer", "suBilled").checkBoolNull();
            String importGP = sqlDataReader.GetFieldForNamedColumn("employer", "import_gp").checkStringNull();
            String importHR = sqlDataReader.GetFieldForNamedColumn("employer", "import_hr").checkStringNull();
            String importEC = sqlDataReader.GetFieldForNamedColumn("employer", "import_ec").checkStringNull();
            String importIO = sqlDataReader.GetFieldForNamedColumn("employer", "import_io").checkStringNull();
            String importIC = sqlDataReader.GetFieldForNamedColumn("employer", "import_ic").checkStringNull();
            String importPayrollModification = sqlDataReader.GetFieldForNamedColumn("employer", "import_pay_mod").checkStringNull();
            Guid resourceId = sqlDataReader.GetFieldForNamedColumn("employer", "ResourceId").checkGuidNull();
            String dbaName = sqlDataReader.GetFieldForNamedColumn("employer", "DBAName").checkStringNull();
            bool IrsEnabled = sqlDataReader.GetFieldForNamedColumn("employer", "IrsEnabled").checkBoolNull();

            employer newEmp = new employer(
                    id, 
                    name, 
                    address, 
                    city, 
                    stateId, 
                    zip, 
                    logo,
                    billAddress,
                    billCity,
                    billStateId,
                    billZip,
                    employerTypeId,
                    federalId,
                    initialMeasurementId,
                    importDemographics,
                    importPayroll, 
                    iei, 
                    iec, 
                    ftpei, 
                    ftpec, 
                    ipi,
                    ipc, 
                    ftppi, 
                    ftppc,
                    importProcess, 
                    vendorId, 
                    autoUpload, 
                    autoBill,
                    suBilled, 
                    importGP, 
                    importHR, 
                    importEC, 
                    importIO, 
                    importIC,
                    importPayrollModification, 
                    resourceId, 
                    dbaName,
                    IrsEnabled
                );

            tempList.Add(newEmp);

        }

        return tempList;

    }

    public DataTable GetEmployeeCountByEmployerAndDateRange(int _employerID, DateTime _sdate, DateTime _edate)
    {

        DataTable dt = new DataTable();

        try
        {

            sqlConnection = new SqlConnection(acaConnectionString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_employee_count", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@sdate", SqlDbType.DateTime).Value = _sdate;
            cmd.Parameters.AddWithValue("@edate", SqlDbType.DateTime).Value = _edate;

            dt.Load(cmd.ExecuteReader());

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);
            
            dt = null;
        
        }

        return dt;

    }


    public List<employer> FilterEmployerByVendor(int _vendorID, List<employer> _empList)
    {

        List<employer> tempList = new List<employer>();

        foreach (employer emp in _empList)
        {
            if (emp.EMPLOYER_VENDOR_ID == _vendorID)
            {
                tempList.Add(emp);
            }
        }

        return tempList;
    
    }

    /// <summary>
    /// Filter down the employer list by search text. 
    /// </summary>
    /// <param name="_searchText"></param>
    /// <param name="_employerList"></param>
    /// <returns></returns>
    public List<employer> FilterEmployerBySearch(string _searchText, List<employer> _employerList)
    {
        List<employer> tempList = new List<employer>();

        foreach (employer emp in _employerList)
        {
            if (emp.EMPLOYER_NAME.ToLower().Contains(_searchText.ToLower()))
            {
                tempList.Add(emp);
            }
        }

        return tempList;
    }

    private static String acaConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

    private ILog Log = LogManager.GetLogger(typeof(employerShow));

    private SqlConnection sqlConnection = null;
    private SqlDataReader sqlDataReader = null;

}