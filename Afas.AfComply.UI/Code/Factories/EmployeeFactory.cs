using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using log4net;
using Afas.AfComply.Domain;
using System.Text;
using Afas.Application.CSV;
using Afas.Domain;

/// <summary>
/// Summary description for EmployeeFactory
/// </summary>
public class EmployeeFactory
{
    private ILog Log = LogManager.GetLogger(typeof(EmployeeFactory));


    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private static string connStringAir = System.Configuration.ConfigurationManager.ConnectionStrings["AIR_Conn"].ConnectionString;

    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

    /// <summary>
    /// Manufacture an entire list of Employees for a specific Employer.
    /// </summary>
    /// <returns></returns>
    public List<Employee> manufactureEmployeeList(int _employerID)
    {

        PIILogger.LogPII("Loading all Employees for employer with Id:" + _employerID);
        List<Employee> tempList = new List<Employee>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_employees", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            tempList = buildEmployeeObjects(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<Employee>();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return tempList;
    }
    public List<Employee> SearchEmployee(int _employerID, string fName, string lName, string mName)
    {

        PIILogger.LogPII("Loading some Employees for employer with Id:" + _employerID);
        List<Employee> tempList = new List<Employee>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SearchEmployee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", SqlDbType.VarChar).Value = '%'+fName+'%';
            cmd.Parameters.AddWithValue("@LastName", SqlDbType.VarChar).Value = '%'+lName+'%';
            cmd.Parameters.AddWithValue("@MiddleName", SqlDbType.VarChar).Value = '%'+mName+'%';
            cmd.Parameters.AddWithValue("@EmployerID", SqlDbType.Int).Value = _employerID;
            rdr = cmd.ExecuteReader();

            tempList = buildEmployeeObjects(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<Employee>();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return tempList;
    }

    public List<Employee> RemainingEmployeesNeeding1095Approval(int _employerID, int _taxYear)
    {
        List<Employee> tempList = new List<Employee>();
        PIILogger.LogPII(string.Format("Loading all Employees for employer with Id:[{0}] and Tax Year:[{1}]", _employerID, _taxYear));

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_employees_Tax_Year_Not_Approved", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            tempList = buildEmployeeObjects(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<Employee>();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return tempList;
    }


    /// <summary>
    /// Manufacture an entire list of Employees for a specific Employer.
    /// </summary>
    /// <returns></returns>
    public List<Employee> EmployeesPending1095Approval(int _employerID, int _taxYear)
    {
        List<Employee> tempList = new List<Employee>();
        PIILogger.LogPII(string.Format("Loading all Employees Pending Approval for employer with Id:[{0}] and Tax Year:[{1}]", _employerID, _taxYear));

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_employees_Tax_Year_Approved", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            tempList = buildEmployeeObjects(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Exception in EmployeesPending1095Approval.", exception);
            tempList = new List<Employee>();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return tempList;
    }

    public List<Employee> EmployeesPending1095Corrections(int _employerID, int _taxYear)
    {
        List<Employee> tempList = new List<Employee>();
        PIILogger.LogPII(string.Format("Loading all Employees Pending Corrections for employer with Id:[{0}] and Tax Year:[{1}]", _employerID, _taxYear));

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_employees_Tax_Year_Corrections", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            tempList = buildEmployeeObjects(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Exception in EmployeesPending1095Corrections.", exception);
            tempList = new List<Employee>();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return tempList;
    }


    /// <summary>
    /// Pulls a list of Employee ID's that are flagged to get 1095C forms.
    /// </summary>
    public List<int> GetEmployeesInInsuranceCarrierImport(int _employerID, int _taxYear)
    {

        List<int> tempList = new List<int>();

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_employees_in_insurance_carrier_table", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {

                object id = rdr[0] as object ?? default(object);
                int _id = id.checkIntNull();

                tempList.Add(_id);

            }

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            tempList = new List<int>();

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
            }

            if (rdr != null)
            {
                rdr.Close();
            }

        }

        return tempList;

    }


    /// <summary>
    /// Manufacture an entire list of Employees for a specific Employer that have been finalized for the specific tax year.
    /// </summary>
    /// <returns></returns>
    public List<Employee> ManufactureEmployeeList1095Finalized(int _employerID, int _taxYear)
    {

        List<Employee> tempList = new List<Employee>();

        PIILogger.LogPII(String.Format("Loading all Employees for employer with Id:[{0}] and Tax Year:[{1}]", _employerID, _taxYear));

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[dbo].[SELECT_employer_employees_already_finalized]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            tempList = buildEmployeeObjects(rdr);

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            tempList = new List<Employee>();

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
            }

            if (rdr != null)
            {
                rdr.Close();
            }

        }

        return tempList;

    }


    private List<Employee> buildEmployeeObjects(SqlDataReader rdr)
    {

        List<Employee> tempList = new List<Employee>();

        while (rdr.Read())
        {
            object employeeid = 0;
            object employeetypeID = null;
            object hrstatusID = null;
            object employerid = 0;
            object fname = null;
            object mname = null;
            object lname = null;
            object address = null;
            object city = null;
            object stateID = 0;
            object zip = null;
            object hdate = null;
            object cdate = null;
            object ssn = null;
            object extEmployeeID = null;
            object tdate = null;
            object dob = null;
            object impEnd = null;
            object planyearID = 0;
            object planyearID_limbo = 0;
            object planyearID_meas = 0;
            object planYearAvg = null;
            object planYearAvgLimbo = null;
            object planYearAvgMeas = null;
            object planYearAvgInit = null;
            object classID = null;
            object actStatusID = null;
            object resourceId = null;

            employeeid = rdr[0] as object ?? default(object);
            employeetypeID = rdr[1] as object ?? default(object);
            hrstatusID = rdr[2] as object ?? default(object);
            employerid = rdr[3] as object ?? default(object);
            fname = rdr[4] as object ?? default(object);
            mname = rdr[5] as object ?? default(object);
            lname = rdr[6] as object ?? default(object);
            address = rdr[7] as object ?? default(object);
            city = rdr[8] as object ?? default(object);
            stateID = rdr[9] as object ?? default(object);
            zip = rdr[10] as object ?? default(object);
            hdate = rdr[11] as object ?? default(object);
            cdate = rdr[12] as object ?? default(object);
            ssn = rdr[13] as object ?? default(object);
            extEmployeeID = rdr[14] as object ?? default(object);
            tdate = rdr[15] as object ?? default(object);
            dob = rdr[16] as object ?? default(object);
            impEnd = rdr[17] as object ?? default(object);
            planyearID = rdr[18] as object ?? default(object);
            planyearID_limbo = rdr[19] as object ?? default(object);
            planyearID_meas = rdr[20] as object ?? default(object);
            planYearAvg = rdr[23] as object ?? default(object);
            planYearAvgLimbo = rdr[24] as object ?? default(object);
            planYearAvgMeas = rdr[25] as object ?? default(object);
            planYearAvgInit = rdr[26] as object ?? default(object);
            classID = rdr[27] as object ?? default(object);
            actStatusID = rdr[28] as object ?? default(object);
            resourceId = rdr[29] as object ?? default(object);

            int _employeeID = employeeid.checkIntNull();
            int _employeeTypeID = employeetypeID.checkIntNull();
            int _hrStatusID = hrstatusID.checkIntNull();
            int _employerID2 = employerid.checkIntNull();
            string _fname = fname.checkStringNull();
            string _lname = lname.checkStringNull();
            string _mname = mname.checkStringNull();
            string _address = address.checkStringNull();
            string _city = city.checkStringNull();
            int _stateID = stateID.checkIntNull();
            string _zip = zip.checkStringNull().ZeroPadZip();
            DateTime _hdate = (DateTime)hdate.checkDateNull();
            DateTime? _cdate = cdate.checkDateNull();
            string _ssn = ssn.checkStringNull();
            string _extEmployeeID = extEmployeeID.checkStringNull();
            DateTime? _tdate = tdate.checkDateNull();
            DateTime? _dob = dob.checkDateNull();
            DateTime _impEnd = (DateTime)impEnd.checkDateNull();                  
            int _planyearID = planyearID.checkIntNull();
            int _planyearID_limbo = planyearID_limbo.checkIntNull();
            int _planYearID_meas = planyearID_meas.checkIntNull();
            double _planYearAvg = planYearAvg.checkDoubleNull();
            double _planYearAvgLimbo = planYearAvgLimbo.checkDoubleNull();
            double _planYearAvgMeas = planYearAvgMeas.checkDoubleNull();
            double _planYearAvgInit = planYearAvgInit.checkDoubleNull();
            int _classID = classID.checkIntNull();
            int _actStatusID = actStatusID.checkIntNull();
            Guid _resourceId = resourceId.checkGuidNull();

            _ssn = AesEncryption.Decrypt(_ssn);

            Employee tempEmp = new Employee(_employeeID, _employeeTypeID, _hrStatusID, _employerID2, _fname, _mname, _lname, _address, _city, _stateID, _zip, _hdate, _cdate, _ssn, _extEmployeeID, _tdate, _dob, _impEnd, _planyearID, _planyearID_limbo, _planYearID_meas, _planYearAvg, _planYearAvgLimbo, _planYearAvgMeas, _planYearAvgInit, _classID, _actStatusID, _resourceId);
            tempList.Add(tempEmp);
        }

        return tempList;
    }

    /// <summary>
    /// Manufacture an entire list of Employees for a specific Employer.
    /// </summary>
    /// <returns></returns>
    public Employee findSingleEmployee(int _employeeID)
    {
        Employee tempEmp = null;
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_single_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;

            rdr = cmd.ExecuteReader();

            return buildEmployeeObjects(rdr).FirstOrDefault();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempEmp;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Employee_E> manufactureEmployeeExportList(int _employerID)
    {
        List<Employee_E> tempList = new List<Employee_E>();
        PIILogger.LogPII("Loading all 'E' Employees for employer with Id:" + _employerID);

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_employee_export", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object employeeid = 0;
                object employeetypeID = null;
                object hrstatusID = null;
                object employerid = 0;
                object fname = null;
                object mname = null;
                object lname = null;
                object address = null;
                object city = null;
                object stateID = 0;
                object zip = null;
                object hdate = null;
                object cdate = null;
                object ssn = null;
                object extEmployeeID = null;
                object tdate = null;
                object dob = null;
                object impEnd = null;
                object planyearID = 0;
                object planyearID_limbo = 0;
                object planyearID_meas = 0;
                object planYearAvg = null;
                object planYearAvgLimbo = null;
                object planYearAvgMeas = null;
                object planYearAvgInit = null;
                object classID = null;
                object actStatusID = null;
                object hrStatus = null;
                object acaStatus = null;
                object className = null;

                employeeid = rdr[0] as object ?? default(object);
                employeetypeID = rdr[1] as object ?? default(object);
                hrstatusID = rdr[2] as object ?? default(object);
                employerid = rdr[3] as object ?? default(object);
                fname = rdr[4] as object ?? default(object);
                mname = rdr[5] as object ?? default(object);
                lname = rdr[6] as object ?? default(object);
                address = rdr[7] as object ?? default(object);
                city = rdr[8] as object ?? default(object);
                stateID = rdr[9] as object ?? default(object);
                zip = rdr[10] as object ?? default(object);
                hdate = rdr[11] as object ?? default(object);
                cdate = rdr[12] as object ?? default(object);
                ssn = rdr[13] as object ?? default(object);
                extEmployeeID = rdr[14] as object ?? default(object);
                tdate = rdr[15] as object ?? default(object);
                dob = rdr[16] as object ?? default(object);
                impEnd = rdr[17] as object ?? default(object);
                planyearID = rdr[18] as object ?? default(object);
                planyearID_limbo = rdr[19] as object ?? default(object);
                planyearID_meas = rdr[20] as object ?? default(object);
                planYearAvg = rdr[21] as object ?? default(object);
                planYearAvgLimbo = rdr[22] as object ?? default(object);
                planYearAvgMeas = rdr[23] as object ?? default(object);
                planYearAvgInit = rdr[24] as object ?? default(object);
                classID = rdr[27] as object ?? default(object);
                actStatusID = rdr[28] as object ?? default(object);
                hrStatus = rdr[29] as object ?? default(object);
                acaStatus = rdr[30] as object ?? default(object);
                className = rdr[31] as object ?? default(object);


                int _employeeID = employeeid.checkIntNull();
                int _employeeTypeID = employeetypeID.checkIntNull();
                int _hrStatusID = hrstatusID.checkIntNull();
                int _employerID2 = employerid.checkIntNull();
                string _fname = fname.checkStringNull();
                string _lname = lname.checkStringNull();
                string _mname = mname.checkStringNull();
                string _address = address.checkStringNull();
                string _city = city.checkStringNull();
                int _stateID = stateID.checkIntNull();
                string _zip = zip.checkStringNull().ZeroPadZip();
                DateTime _hdate = (DateTime)hdate.checkDateNull();
                DateTime? _cdate = cdate.checkDateNull();
                string _ssn = ssn.checkStringNull();
                string _extEmployeeID = extEmployeeID.checkStringNull();
                DateTime? _tdate = tdate.checkDateNull();
                DateTime? _dob = dob.checkDateNull();
                DateTime _impEnd = (DateTime)impEnd.checkDateNull();                  
                int _planyearID = planyearID.checkIntNull();
                int _planyearID_limbo = planyearID_limbo.checkIntNull();
                int _planYearID_meas = planyearID_meas.checkIntNull();
                double _planYearAvg = planYearAvg.checkDoubleNull();
                double _planYearAvgLimbo = planYearAvgLimbo.checkDoubleNull();
                double _planYearAvgMeas = planYearAvgMeas.checkDoubleNull();
                double _planYearAvgInit = planYearAvgInit.checkDoubleNull();
                int _classID = classID.checkIntNull();
                int _actStatusID = actStatusID.checkIntNull();
                string _hrStatus = hrStatus.checkStringNull();
                string _acaStatus = acaStatus.checkStringNull();
                string _class = className.checkStringNull();

                _ssn = AesEncryption.Decrypt(_ssn);

                Employee_E tempEmp = new Employee_E(_employeeID, _employeeTypeID, _hrStatusID, _employerID2, _fname, _mname, _lname, _address, _city, _stateID, _zip, _hdate, _cdate, _ssn, _extEmployeeID, _tdate, _dob, _impEnd, _planyearID, _planyearID_limbo, _planYearID_meas, _planYearAvg, _planYearAvgLimbo, _planYearAvgMeas, _planYearAvgInit, _classID, _actStatusID, _class, _hrStatus, _acaStatus);
                tempList.Add(tempEmp);
            }

            return tempList;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempList;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }
    }

    public Employee ManufactureEmployee(
            int _employeeTypeID,
            int _hrStatusID,
            int _employerID,
            String _fname,
            String _mname,
            String _lname,
            String _address,
            String _city,
            int _stateid,
            String _zip,
            DateTime _hireDate,
            DateTime? _currDate,
            String _ssn,
            String _extID,
            DateTime? _termDate,
            DateTime? _dob,
            DateTime _impEnd,
            int _planYearID,
            DateTime _modOn,
            String _modBy,
            int _classID,
            int _actStatusID
        )
    {

        Employee tempEmployee = null;

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            string encryptedSSN = AesEncryption.Encrypt(_ssn);

            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employeeTypeID;
            cmd.Parameters.AddWithValue("@hrStatusID", SqlDbType.Int).Value = _hrStatusID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname.checkForDBNull();

            if (_mname != null)
            {
                cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = _mname.checkForDBNull();
            }
            else
            {
                cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = _address.checkForDBNull();
            cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = _city.checkForDBNull();
            cmd.Parameters.AddWithValue("@stateID", SqlDbType.Int).Value = _stateid.checkIntDBNull();
            cmd.Parameters.AddWithValue("@zip", SqlDbType.VarChar).Value = _zip.ZeroPadZip().checkIntDBNull();

            if (_hireDate != null)
            {
                DateTime hdate = (DateTime)_hireDate;
                cmd.Parameters.AddWithValue("@hireDate", SqlDbType.DateTime).Value = hdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@hireDate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            if (_currDate != null)
            {
                DateTime cdate = (DateTime)_currDate;
                cmd.Parameters.AddWithValue("@currDate", SqlDbType.DateTime).Value = cdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@currDate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = encryptedSSN;

            if (String.IsNullOrEmpty(_extID))
            {

                if (Feature.EmployeeDemographicEmployeeNumberRequired == true)
                {
                    throw new ArgumentException(String.Format("Received a null employee external id where EmployeeDemographicEmployeeNumberRequired feature is set to true."));
                }

                cmd.Parameters.AddWithValue("@extID", SqlDbType.VarChar).Value = DBNull.Value;

            }
            else
            {
                cmd.Parameters.AddWithValue("@extID", SqlDbType.VarChar).Value = _extID;
            }

            if (_termDate != null)
            {
                DateTime tdate = (DateTime)_termDate;
                cmd.Parameters.AddWithValue("@termDate", SqlDbType.DateTime).Value = tdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@termDate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            if (_dob != null)
            {
                DateTime dob = (DateTime)_dob;
                cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = dob.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = DBNull.Value;
            }

            if (_impEnd != null)
            {
                cmd.Parameters.AddWithValue("@imEnd", SqlDbType.DateTime).Value = _impEnd.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@imEnd", SqlDbType.DateTime).Value = DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;

            if (_classID != 0)
            {
                cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = _classID;
            }
            else
            {
                cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = DBNull.Value;
            }

            if (_actStatusID != 0)
            {
                cmd.Parameters.AddWithValue("@actStatusID", SqlDbType.Int).Value = _actStatusID;
            }
            else
            {
                cmd.Parameters.AddWithValue("@actStatusID", SqlDbType.Int).Value = DBNull.Value;
            }

            cmd.Parameters.Add("@employeeID", SqlDbType.Int);
            cmd.Parameters["@employeeID"].Direction = ParameterDirection.Output;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {

                int _id = (int)cmd.Parameters["@employeeID"].Value;

                tempEmployee = new Employee(
                        _id,
                        _employeeTypeID,
                        _hrStatusID,
                        _employerID,
                        _fname,
                        _mname,
                        _lname,
                        _address,
                        _city,
                        _stateid,
                        _zip,
                        _hireDate,
                        _currDate,
                        _ssn,
                        _extID,
                        _termDate,
                        _dob,
                        _impEnd,
                        _planYearID,
                        0,
                        0,
                        0,
                        0,
                        0,
                        0,
                        _classID,
                        _actStatusID
                    );

                return tempEmployee;

            }
            else
            {
                return tempEmployee;
            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            return tempEmployee;

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
            }

        }

    }

    public bool importEmployee(int _batchID, string _hrStatusID, string _hrDescription, int _employerID, string _fname, string _mname, string _lname, string _address, string _city, string _stateid, string _zip, string _hireDate, string _currDate, string _ssn, string _extID, string _termDate, string _dob)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_import_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;
            cmd.Parameters.AddWithValue("@hrStatusID", SqlDbType.VarChar).Value = _hrStatusID.checkForDBNull();
            cmd.Parameters.AddWithValue("@hrDescription", SqlDbType.VarChar).Value = _hrDescription.checkForDBNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = _mname.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = _address.checkForDBNull();
            cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = _city.checkForDBNull();
            cmd.Parameters.AddWithValue("@stateID", SqlDbType.Int).Value = _stateid.checkForDBNull();
            cmd.Parameters.AddWithValue("@zip", SqlDbType.VarChar).Value = _zip.ZeroPadZip().checkForDBNull();
            cmd.Parameters.AddWithValue("@hireDate", SqlDbType.VarChar).Value = _hireDate.checkForDBNull();
            cmd.Parameters.AddWithValue("@currDate", SqlDbType.DateTime).Value = _currDate.checkForDBNull();
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = _ssn.checkForDBNull();
            cmd.Parameters.AddWithValue("@extID", SqlDbType.VarChar).Value = _extID.checkForDBNull();
            cmd.Parameters.AddWithValue("@termDate", SqlDbType.DateTime).Value = _termDate.checkForDBNull();
            cmd.Parameters.AddWithValue("@dob", SqlDbType.VarChar).Value = _dob.checkForDBNull();

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public Boolean BulkImportEmployee(int _batchID, DataTable data)
    {

        this.Log.Info(String.Format("Starting BulkImportEmployee for batch {0}.", _batchID));

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("BULK_INSERT_import_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;

            cmd.Parameters.AddWithValue("@employees", SqlDbType.Structured).Value = data;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {

                this.Log.Info(String.Format("BULK_INSERT_import_employee for batch {0} returned {1}.", _batchID, tsql));

                return true;

            }
            else
            {

                this.Log.Warn(String.Format("BULK_INSERT_import_employee for batch {0} returned {1}.", _batchID, tsql));

                return false;

            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Failure to Bulk Import Employees.", exception);

            return false;

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
            }

        }

    }

    public bool BulkImportFullEmployee(int _batchID, DataTable data)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("BULK_INSERT_FULL_import_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;

            cmd.Parameters.AddWithValue("@employees", SqlDbType.Structured).Value = data;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Failure to Bulk Import Emplolyees.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public Boolean BulkInsertTaxYear1095cCorrection(List<TaxYear1095CCorrection> taxYear1095CCorrections)
    {
        try
        {
            DataTable dtCorrections = taxYear1095CCorrections.AsEnumerable().ToDataTable();

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("BULK_INSERT_tax_year_1095c_correction", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@correctionInsert", SqlDbType.Structured).Value = dtCorrections;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            return true;

        }
        catch (Exception exception)
        {
            Log.Warn("Failure to Bulk Insert Tax Year 1095c Corrections.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public Boolean BulkDeleteTaxYear1095cApprovals(List<TaxYear1095CApproval> taxYear1095CApprovals)
    {
        try
        {
            DataTable dtApprovals = taxYear1095CApprovals.AsEnumerable().ToDataTable();

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("BULK_DELETE_tax_year_1095c_approval", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@approvalDelete", SqlDbType.Structured).Value = dtApprovals;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Failure to Bulk Delete Tax Year 1095c Approvals.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public Boolean BulkInactivateTaxYear1095cCorrections(int employerId, String modifiedBy)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("BULK_INACTIVATE_tax_year_1095c_correction", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerId", SqlDbType.Structured).Value = employerId;
            cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.Structured).Value = modifiedBy;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            return true;

        }
        catch (Exception exception)
        {
            Log.Warn("Failure to Bulk Inactivate Tax Year 1095c Corrections.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public Boolean CheckIfTaxYear1095cCorrectionRecord(int employee_Id, int tax_year)
    {
        TaxYear1095CCorrection taxYear1095CCorrection = null;

        SqlConnection conn = null;
        DataTable dtTaxYear1095cCorrection = new DataTable();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("CHECK_if_tax_year_1095c_correction_exists", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employee_Id", SqlDbType.Int).Value = employee_Id;
            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.Int).Value = tax_year;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtTaxYear1095cCorrection);

            taxYear1095CCorrection = dtTaxYear1095cCorrection.DataTableToObject<TaxYear1095CCorrection>();

        }
        catch (Exception exception)
        {
            Log.Warn("CheckIfTaxYear1095cCorrectionRecord exception.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return (taxYear1095CCorrection != null);
    }

    public Boolean InsertTaxYear1095Approval(
            int _taxYear,
            int _employeeID,
            int _employerID,
            String _modBy,
            DateTime _modOn,
            Boolean _1095C
        )
    {
        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_1095_tax_year_approval", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.Int).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.VarChar).Value = _modOn;
            cmd.Parameters.AddWithValue("@1095", SqlDbType.VarChar).Value = _1095C;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            ////If row in the database was succesfully inserted, create the new District object.
            return true;

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            return false;

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
            }

        }

    }

    public tax_year_1095c_correction_exception InsertUpadateTaxYear1095cCorrectionException(tax_year_1095c_correction_exception tax_year_1095c_correction_exception)
    {

        conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        try
        {

            conn.Open();
            cmd.CommandText = "INSERT_UPDATE_tax_year_1095c_correction_exception";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TaxYear1095cCorrectionExceptionId", SqlDbType.Int).Value = tax_year_1095c_correction_exception.TaxYear1095cCorrectionExceptionId;
            cmd.Parameters["@TaxYear1095cCorrectionExceptionId"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.Int).Value = tax_year_1095c_correction_exception.tax_year;
            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = tax_year_1095c_correction_exception.employer_id;
            cmd.Parameters.AddWithValue("@employee_id", SqlDbType.Int).Value = tax_year_1095c_correction_exception.employee_id;
            cmd.Parameters.AddWithValue("@Justification", SqlDbType.VarChar).Value = tax_year_1095c_correction_exception.Justification;
            cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.NVarChar).Value = tax_year_1095c_correction_exception.CreatedBy;
            cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.NVarChar).Value = tax_year_1095c_correction_exception.ModifiedBy;

            cmd.ExecuteNonQuery();
            tax_year_1095c_correction_exception.TaxYear1095cCorrectionExceptionId = (int)cmd.Parameters["@TaxYear1095cCorrectionExceptionId"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Exception in InsertUpadateTaxYear1095cCorrectionException.", exception);
            return null;
        }
        finally
        {
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tax_year_1095c_correction_exception;

    }

    public tax_year_1095c_correction_exception getTaxYear1095cCorrectionException(int tax_year, int employer_id, int employee_id)
    {
        tax_year_1095c_correction_exception tax_year_1095c_correction_exception;
        SqlConnection conn = null;
        DataTable dtTaxYear1095cCorrectionException = new DataTable();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_tax_year_1095c_correction_exception", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.Int).Value = tax_year;
            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = employer_id;
            cmd.Parameters.AddWithValue("@employee_id", SqlDbType.Int).Value = employee_id;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtTaxYear1095cCorrectionException);

            tax_year_1095c_correction_exception = dtTaxYear1095cCorrectionException.DataTableToObject<tax_year_1095c_correction_exception>();
            if (tax_year_1095c_correction_exception == null)
            {
                tax_year_1095c_correction_exception = new tax_year_1095c_correction_exception()
                {
                    tax_year = tax_year,
                    employer_id = employer_id,
                    employee_id = employee_id
                };
            }

        }
        catch (Exception exception)
        {
            Log.Warn("Exception in getTaxYear1095cCorrectionException.", exception);
            return null;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return tax_year_1095c_correction_exception;
    }

    public bool deleteEmployee1095cApproval(int _employeeID, int _employerID, int _taxyear)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_employee_1095c_approval", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxyear", SqlDbType.Int).Value = _taxyear;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }

    public bool DeleteFailedBatch(int _batchID)
    {

        bool validTransaction = false;

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_employee_import", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }

        }

        return validTransaction;

    }

    public bool updateImportEmployee(int _rowID, int _employeeTypeID, int _hrStatusID, string _hrStatusExt, string _hrStatusDesc, int _planyearID, int _stateid, DateTime? _hireDate, string _hireDateI, DateTime? _cDate, DateTime? _termDate, string _termDateI, DateTime? _dob, string _dobI, DateTime? _impEnd, int _acaStatusID, int _classID)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_import_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@rowid", SqlDbType.Int).Value = _rowID.checkIntNull();
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employeeTypeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@hrstatusID", SqlDbType.Int).Value = _hrStatusID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@hrStatusExt", SqlDbType.VarChar).Value = _hrStatusExt.checkForDBNull();
            cmd.Parameters.AddWithValue("@hrStatusDesc", SqlDbType.VarChar).Value = _hrStatusDesc.checkForDBNull();
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planyearID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@stateID", SqlDbType.Int).Value = _stateid.checkIntDBNull();
            cmd.Parameters.AddWithValue("@hdate", SqlDbType.DateTime).Value = _hireDate.checkForDBNull();
            cmd.Parameters.AddWithValue("@hdateI", SqlDbType.VarChar).Value = _hireDateI.checkForDBNull();
            cmd.Parameters.AddWithValue("@cdate", SqlDbType.DateTime).Value = _cDate.checkForDBNull();
            cmd.Parameters.AddWithValue("@tdate", SqlDbType.DateTime).Value = _termDate.checkForDBNull();
            cmd.Parameters.AddWithValue("@tdateI", SqlDbType.VarChar).Value = _termDateI.checkForDBNull();
            cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = _dob.checkForDBNull();
            cmd.Parameters.AddWithValue("@dobI", SqlDbType.VarChar).Value = _dobI.checkForDBNull();
            cmd.Parameters.AddWithValue("@impEnd", SqlDbType.DateTime).Value = _impEnd.checkForDBNull();
            cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = _classID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@acaStatusID", SqlDbType.Int).Value = _acaStatusID.checkIntDBNull();

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public Boolean BulkUpdateImportEmployee(DataTable employees)
    {

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("BULK_UPDATE_import_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employees", SqlDbType.Structured).Value = employees;

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            return false;

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
            }

        }

    }

    public bool updateEmployeeSNN(int _employeeID, DateTime _modOn, string _modBy, string _ssn, int _hrStatusID, int _classID, int _actStatusID)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_ssn", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            _ssn = AesEncryption.Encrypt(_ssn);

            cmd.Parameters.AddWithValue("@employee_ID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = _ssn;
            cmd.Parameters.AddWithValue("@hrStatusID", SqlDbType.Int).Value = _hrStatusID;
            cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = _classID;
            cmd.Parameters.AddWithValue("@acaStatusID", SqlDbType.Int).Value = _actStatusID;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool updateEmployee(Employee employee, String modBy)

    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            var ssn = AesEncryption.Encrypt(employee.Employee_SSN_Visible);

            cmd.Parameters.AddWithValue("@employee_ID", SqlDbType.Int).Value = employee.EMPLOYEE_ID;
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = employee.EMPLOYEE_TYPE_ID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@hrstatusID", SqlDbType.Int).Value = employee.EMPLOYEE_HR_STATUS_ID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = employee.EMPLOYEE_FIRST_NAME.checkForDBNull();
            cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = employee.EMPLOYEE_MIDDLE_NAME.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = employee.EMPLOYEE_LAST_NAME.checkForDBNull();
            cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = employee.EMPLOYEE_ADDRESS.checkForDBNull();
            cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = employee.EMPLOYEE_CITY.checkForDBNull();
            cmd.Parameters.AddWithValue("@stateID", SqlDbType.Int).Value = employee.EMPLOYEE_STATE_ID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@zip", SqlDbType.VarChar).Value = employee.EMPLOYEE_ZIP.checkForDBNull();
            cmd.Parameters.AddWithValue("@hdate", SqlDbType.DateTime).Value = employee.EMPLOYEE_HIRE_DATE.checkDateDBNull();
            cmd.Parameters.AddWithValue("@cdate", SqlDbType.DateTime).Value = employee.EMPLOYEE_C_DATE.checkDateDBNull();
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = ssn.checkForDBNull();
            cmd.Parameters.AddWithValue("@extemployeeid", SqlDbType.VarChar).Value = employee.EMPLOYEE_EXT_ID;
            cmd.Parameters.AddWithValue("@tdate", SqlDbType.DateTime).Value = employee.EMPLOYEE_TERM_DATE.checkDateDBNull();
            cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = employee.EMPLOYEE_DOB.checkDateDBNull();
            cmd.Parameters.AddWithValue("@impEnd", SqlDbType.DateTime).Value = employee.EMPLOYEE_IMP_END.checkDateDBNull();
            cmd.Parameters.AddWithValue("@planyearid", SqlDbType.Int).Value = employee.EMPLOYEE_PLAN_YEAR_ID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@limboplanyearid", SqlDbType.Int).Value = employee.EMPLOYEE_PLAN_YEAR_ID_LIMBO.checkIntDBNull();
            cmd.Parameters.AddWithValue("@measplanyearid", SqlDbType.DateTime).Value = employee.EMPLOYEE_PLAN_YEAR_ID_MEAS.checkIntDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = DateTime.Now.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = employee.EMPLOYEE_CLASS_ID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@actStatusID", SqlDbType.Int).Value = employee.EMPLOYEE_ACT_STATUS_ID.checkIntDBNull();


            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            ////If row in the database was succesfully inserted, create the new District object.
            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Execption in updateEmployee.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool UpdateEmployeeInfo_ACA_AIR(int _employeeID, string FirstName, string MiddleName, string LastName, string StateId, string Address, string City, string Zip, string ssn, int ModBy)
    {
        try
        {
            conn = new SqlConnection(connStringAir);
            conn.Open();

            SqlCommand cmd = new SqlCommand("spUPDATE_employee__ACA_AIR", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            if (ssn != null)
            {
                ssn = AesEncryption.Encrypt(ssn);
            }

            cmd.Parameters.AddWithValue("@employee_ID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = FirstName.checkForDBNull();
            cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = MiddleName.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = LastName.checkForDBNull();
            cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = Address.checkForDBNull();
            cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = City.checkForDBNull();
            cmd.Parameters.AddWithValue("@stateID", SqlDbType.Int).Value = StateId.checkIntDBNull();
            cmd.Parameters.AddWithValue("@zip", SqlDbType.VarChar).Value = Zip.ZeroPadZip().checkForDBNull();
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = ssn.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = ModBy.checkForDBNull();

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool updateEmployeeLineIII_SSN(int _employeeID, DateTime _modOn, string _modBy, string _ssn, string _fname, string _lname)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_LINEIII_SSN", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            _ssn = AesEncryption.Encrypt(_ssn);

            cmd.Parameters.AddWithValue("@employee_ID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = _ssn;
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname;
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Exception in updateEmployeeLineIII_SSN.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool updateEmployeeLineIII_DOB(int _employeeID, DateTime _modOn, string _modBy, DateTime _dob, string _fname, string _lname)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_LINEIII_DOB", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employee_ID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = _dob;
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname;
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Exception in updateEmployeeLineIII_DOB", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool updateEmployeeAvgMonthlyHours(int _employeeID, double _pyAvg, double _lpyAvg, double _mpyAvg, double _impAvg)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_AVG_MONTHLY_HOURS", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;


            cmd.Parameters.AddWithValue("@employee_ID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@pyAvg", SqlDbType.Float).Value = _pyAvg;
            cmd.Parameters.AddWithValue("@lpyAvg", SqlDbType.Float).Value = _lpyAvg;
            cmd.Parameters.AddWithValue("@mpyAvg", SqlDbType.Float).Value = _mpyAvg;
            cmd.Parameters.AddWithValue("@impAvg", SqlDbType.Float).Value = _impAvg;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }


    public static DataTable GetNewEmployeeHoursDataTable()
    {
        DataTable AverageHours = new DataTable();

        AverageHours.Columns.Add("employee_id", typeof(int));
        AverageHours.Columns.Add("pyAvg", typeof(int));
        AverageHours.Columns.Add("lpyAvg", typeof(int));
        AverageHours.Columns.Add("mpyAvg", typeof(double));
        AverageHours.Columns.Add("impAvg", typeof(double));

        return AverageHours;
    }

    public bool BulkUpdateEmployee(DataTable data)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("BULK_UPDATE_employee_AVG_MONTHLY_HOURS", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeHours", SqlDbType.Structured).Value = data;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool updateEmployeePlanYearMeasId(int _employeeID, int _planYearID, DateTime _modOn, string _modBy)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_plan_year_meas_id", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Employee_I> manufactureImportEmployeeList(int _employerID, int? rowLimit = null)
    {
        PIILogger.LogPII("Loading all 'I' Employees for employer with Id:" + _employerID);

        List<Employee_I> tempList = new List<Employee_I>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_Import_employer_employees", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object rowID = null;
                object employeetypeID = null;
                object hrstatusID = null;
                object hrstatusExtID = null;
                object hrstatusName = null;
                object employerid = null;
                object planyearid = null;
                object fname = null;
                object mname = null;
                object lname = null;
                object address = null;
                object city = null;
                object stateID = null;
                object stateAbb = null;
                object zip = null;
                object hdate = null;
                object ihdate = null;
                object cdate = null;
                object icdate = null;
                object ssn = null;
                object extEmployeeID = null;
                object tdate = null;
                object itdate = null;
                object dob = null;
                object idob = null;
                object impEnd = null;
                object classID = null;
                object acaStatusID = null;

                rowID = rdr[0] as object ?? default(object);
                employeetypeID = rdr[1] as object ?? default(object);
                hrstatusID = rdr[2] as object ?? default(object);
                hrstatusExtID = rdr[3] as object ?? default(object);
                hrstatusName = rdr[4] as object ?? default(object);
                employerid = rdr[5] as object ?? default(object);
                planyearid = rdr[6] as object ?? default(object);
                fname = rdr[7] as object ?? default(object);
                mname = rdr[8] as object ?? default(object);
                lname = rdr[9] as object ?? default(object);
                address = rdr[10] as object ?? default(object);
                city = rdr[11] as object ?? default(object);
                stateID = rdr[12] as object ?? default(object);
                stateAbb = rdr[13] as object ?? default(object);
                zip = rdr[14] as object ?? default(object);
                hdate = rdr[15] as object ?? default(object);
                ihdate = rdr[16] as object ?? default(object);
                cdate = rdr[17] as object ?? default(object);
                icdate = rdr[18] as object ?? default(object);
                ssn = rdr[19] as object ?? default(object);
                extEmployeeID = rdr[20] as object ?? default(object);
                tdate = rdr[21] as object ?? default(object);
                itdate = rdr[22] as object ?? default(object);
                dob = rdr[23] as object ?? default(object);
                idob = rdr[24] as object ?? default(object);
                impEnd = rdr[25] as object ?? default(object);
                acaStatusID = rdr[27] as object ?? default(object);
                classID = rdr[28] as object ?? default(object);


                int _rowID = rowID.checkIntNull();
                int _employeeTypeID = employeetypeID.checkIntNull();
                int _hrStatusID = hrstatusID.checkIntNull();
                string _hrStatusExtID = hrstatusExtID.checkStringNull();
                string _hrStatusName = hrstatusName.checkStringNull();
                int _employerID2 = employerid.checkIntNull();
                int _planYearID = planyearid.checkIntNull();
                string _fname = fname.checkStringNull();
                string _lname = lname.checkStringNull();
                string _mname = mname.checkStringNull();
                string _address = address.checkStringNull();
                string _city = city.checkStringNull();
                int _stateID = stateID.checkIntNull();
                string _stateAbb = stateAbb.checkStringNull();
                string _zip = zip.checkStringNull().ZeroPadZip();
                DateTime? _hdate = hdate.checkDateNull();
                string _ihdate = ihdate.checkStringNull();
                DateTime? _cdate = cdate.checkDateNull();
                string _icdate = icdate.checkStringNull();
                string _ssn = ssn.checkStringNull();
                string _extEmployeeID = extEmployeeID.checkStringNull();
                DateTime? _tdate = tdate.checkDateNull();
                string _itdate = itdate.checkStringNull();
                DateTime? _dob = dob.checkDateNull();
                string _idob = idob.checkStringNull();
                DateTime? _impEnd = impEnd.checkDateNull();
                int _classID = classID.checkIntNull();
                int _acaStatusID = acaStatusID.checkIntNull();

                if (_ssn != null)
                {
                    _ssn = AesEncryption.Decrypt(_ssn);
                }

                Employee_I tempEmp = new Employee_I(_rowID, _planYearID, _employerID2, _hrStatusExtID, _hrStatusName, _fname, _mname, _lname, _address, _city, _stateAbb, _zip, _ihdate, _icdate, _ssn, _extEmployeeID, _itdate, _idob);
                tempEmp.EMPLOYEE_TYPE_ID = _employeeTypeID;
                tempEmp.EMPLOYEE_HR_STATUS_ID = _hrStatusID;
                tempEmp.EMPLOYEE_HIRE_DATE = _hdate;
                tempEmp.EMPLOYEE_STATE_ID = _stateID;
                tempEmp.EMPLOYEE_C_DATE = _cdate;
                tempEmp.EMPLOYEE_TERM_DATE = _tdate;
                tempEmp.EMPLOYEE_DOB = _dob;
                tempEmp.EMPLOYEE_IMP_END = _impEnd;
                tempEmp.EMPLOYEE_PLAN_YEAR_ID_MEAS = _planYearID;
                tempEmp.EMPLOYEE_CLASS_ID = _classID;
                tempEmp.EMPLOYEE_ACT_STATUS_ID = _acaStatusID;
                tempList.Add(tempEmp);

                if (rowLimit != null && tempList.Count >= rowLimit)
                {
                    break;
                }
            }

            return tempList;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempList;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
        }
    }


    /// <summary>
    /// This function handles two different stored procedures for the Employee Import process. 
    /// - 1) TRANSFER_import_new_employee
    /// - 2) TRANSFER_import_existing_employee
    /// </summary>
    public Employee TransferImportedEmployee(
            int _employeeID,
            int _rowID,
            int _employeeTypeID,
            int _hrStatusID,
            int _employerID,
            String _fname,
            String _mname,
            String _lname,
            String _address,
            String _city,
            int _stateid,
            String _zip,
            DateTime? _hireDate,
            DateTime? _cDate,
            String _ssn,
            String _extID,
            DateTime? _termDate,
            DateTime? _dob,
            DateTime? _impEnd,
            int _planYearID,
            int _planYearID_limbo,
            int _planYearID_meas,
            DateTime _modOn,
            String _modBy,
            Boolean _offer,
            int _offerPlayYearID,
            int _classID,
            int _actStatusID
        )
    {

        Employee tempEmployee = null;

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        try
        {
            conn.Open();

            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            if (_employeeID == 0)
            {
                NewEmployee(cmd, _offer, _offerPlayYearID, _planYearID_meas);
            }
            else
            {
                ExistingEmployee(cmd, _employeeID, _planYearID_limbo, _planYearID_meas, _planYearID);
            }


            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employeeTypeID;
            cmd.Parameters.AddWithValue("@hrStatusID", SqlDbType.Int).Value = _hrStatusID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = _mname.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = _address.checkForDBNull();
            cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = _city.checkForDBNull();
            cmd.Parameters.AddWithValue("@stateID", SqlDbType.Int).Value = _stateid.checkIntDBNull();
            cmd.Parameters.AddWithValue("@zip", SqlDbType.VarChar).Value = _zip.ZeroPadZip().checkIntDBNull();

            if (_hireDate != null)
            {
                DateTime hdate = (DateTime)_hireDate;
                cmd.Parameters.AddWithValue("@hDate", SqlDbType.DateTime).Value = hdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@hDate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            if (_cDate != null)
            {
                DateTime cdate = (DateTime)_cDate;
                cmd.Parameters.AddWithValue("@cDate", SqlDbType.DateTime).Value = cdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@cDate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            String encryptSSN = AesEncryption.Encrypt(_ssn);
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = encryptSSN;

            if (String.IsNullOrEmpty(_extID))
            {

                if (Feature.EmployeeDemographicEmployeeNumberRequired == true)
                {
                    throw new ArgumentException(String.Format("Received a null employee external id where EmployeeDemographicEmployeeNumberRequired feature is set to true."));
                }

                cmd.Parameters.AddWithValue("@externalEmployeeID", SqlDbType.VarChar).Value = DBNull.Value;

            }
            else
            {
                cmd.Parameters.AddWithValue("@externalEmployeeID", SqlDbType.VarChar).Value = _extID.checkForDBNull();
            }

            if (_termDate != null)
            {
                DateTime tdate = (DateTime)_termDate;
                cmd.Parameters.AddWithValue("@tDate", SqlDbType.DateTime).Value = tdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@tDate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            if (_dob != null)
            {
                DateTime dob = (DateTime)_dob;
                cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = dob.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = DBNull.Value;
            }

            if (_impEnd != null)
            {
                cmd.Parameters.AddWithValue("@impEnd", SqlDbType.DateTime).Value = ((DateTime)_impEnd).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@impEnd", SqlDbType.DateTime).Value = DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = _classID.checkIntDBNull(); ;
            cmd.Parameters.AddWithValue("@actStatusID", SqlDbType.Int).Value = _actStatusID.checkIntDBNull(); ;

            if (_employeeID == 0)
            {
                cmd.Parameters.Add("@employeeID", SqlDbType.Int);
                cmd.Parameters["@employeeID"].Direction = ParameterDirection.Output;
            }

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();
            int _id = 0;

            if (_employeeID == 0)
            {
                if (null == cmd.Parameters["@employeeID"] || null == cmd.Parameters["@employeeID"].Value ||
                    false == int.TryParse(cmd.Parameters["@employeeID"].Value.ToString(), out _id))
                {
                    Log.Error("Stored Procedure Failed to return a valid output. " + cmd.CommandText);

                    return null;
                }
            }
            if (tsql == 3 || tsql == 2)
            {

                tempEmployee = new Employee(
                        _id,
                        _employeeTypeID,
                        _hrStatusID,
                        _employerID,
                        _fname,
                        _mname,
                        _lname,
                        _address,
                        _city,
                        _stateid,
                        _zip,
                        _hireDate,
                        _cDate,
                        _ssn,
                        _extID,
                        _termDate,
                        _dob,
                        _impEnd,
                        0,
                        0,
                        _planYearID_meas,
                        0,
                        0,
                        0,
                        0,
                        _classID,
                        _actStatusID
                    );

            }
            else
            {
                tempEmployee = null;
            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            tempEmployee = null;

        }
        finally
        {

            if (cmd != null)
            {
                cmd.Dispose();
            }

            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }

        }

        return tempEmployee;

    }

    private void ExistingEmployee(SqlCommand cmd, int _employeeID, int _planYearID_limbo, int _planYearID_meas, int _planYearID)
    {
        cmd.CommandText = "TRANSFER_import_existing_employee";
        cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
        cmd.Parameters.AddWithValue("@limboplanyearid", SqlDbType.Int).Value = _planYearID_limbo.checkIntDBNull();
        cmd.Parameters.AddWithValue("@measplanyearid", SqlDbType.Int).Value = _planYearID_meas.checkIntDBNull();
        cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID.checkIntDBNull();
    }
    private void NewEmployee(SqlCommand cmd, bool _offer, int _offerPlayYearID, int _planYearID_meas)
    {
        cmd.CommandText = "TRANSFER_import_new_employee";
        cmd.Parameters.AddWithValue("@offer", SqlDbType.Bit).Value = _offer;
        cmd.Parameters.AddWithValue("@offerPlanYearID", SqlDbType.Int).Value = _offerPlayYearID.checkIntDBNull();
        cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID_meas.checkIntDBNull();                       
    }

    public int manufactureBatchID(int _employerID, DateTime _modOn, string _modBy)
    {
        int newBatchID = 0;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_batch", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.Int).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.Int).Value = _modBy;

            cmd.Parameters.Add("@batchID", SqlDbType.Int);
            cmd.Parameters["@batchID"].Direction = ParameterDirection.Output;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                newBatchID = (int)cmd.Parameters["@batchID"].Value;

            }
            else
            {
                newBatchID = 0;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            newBatchID = 0;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return newBatchID;
    }


    public bool deleteImportedEmployee(int _rowID)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_employee_import_row", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// This function is used for Rolling Over the Measurement Period. 
    /// The Stored Procedure will generate Insurance Offers for all Employees tied to the Current Measurement Plan Year
    /// and then move that period to the Admin Period while updating the Measurement Plan Year with the new Measurement Period. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_employerTypeID"></param>
    /// <param name="_planYearID"></param>
    /// <param name="_newPlayYearID"></param>
    /// <param name="_modOn"></param>
    /// <param name="_modBy"></param>
    /// <param name="_mpStartDate"></param>
    /// <returns></returns>
    public bool updateEmployeePlanYearPeriod_Measurement(int _employerID, int _employerTypeID, int _planYearID, int _newPlayYearID, DateTime _modOn, string _modBy)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_plan_year_meas", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employerTypeID;
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID;
            cmd.Parameters.AddWithValue("@newPlanYearID", SqlDbType.Int).Value = _newPlayYearID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// This function is used for UnRolling the Measurement Period. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_employerTypeID"></param>
    /// <param name="_planYearID"></param>
    /// <param name="_newPlayYearID"></param>
    /// <param name="_modOn"></param>
    /// <param name="_modBy"></param>
    /// <param name="_mpStartDate"></param>
    /// <returns></returns>
    public bool RollBackEmployeePlanYearPeriod_Measurement(int _employerID, int _employerTypeID, int _CurrPlanYearID, int _RollBackToPlayYearID, DateTime _modOn, string _modBy)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[UPDATE_ROLLBACK_employee_plan_year_meas]", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employerTypeID;
            cmd.Parameters.AddWithValue("@CurrPlanYearID", SqlDbType.Int).Value = _CurrPlanYearID;
            cmd.Parameters.AddWithValue("@RollbackToPlanYearID", SqlDbType.Int).Value = _RollBackToPlayYearID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }

    public bool RolloverAdministrativePeriod(int _employerID, int _employerTypeID, int _planYearID, DateTime _modOn, string _modBy)
    {

        bool validTransaction = false;

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_plan_year", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employerTypeID;
            cmd.Parameters.AddWithValue("@adminPlanYearID", SqlDbType.Int).Value = _planYearID;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;
            }
            else
            {

                this.Log.Warn(
                        String.Format(
                                "For Employee Type {0}, Admin Plan Year {1} for Employer {2} UPDATE_employee_plan_year expected greater than zero records, found {3}.",
                                _employerTypeID,
                                _planYearID,
                                _employerID,
                                tsql
                            )
                    );

                validTransaction = false;

            }

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            validTransaction = false;

        }
        finally
        {

            if (conn != null)
            {

                conn.Close();

                conn.Dispose();

            }

        }

        return validTransaction;

    }

    /// <summary>
    /// This will process every DEM file located in the FTP folder for a specific EMPLOYER. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_modBy"></param>
    /// <param name="_modOn"></param>
    /// <param name="_filePath"></param>
    /// <param name="_fileName"></param>
    /// <returns></returns>
    public bool process_DEM_I_files(int _employerID, string _modBy, DateTime _modOn, string _filePath, string _fileName)
    {

        bool validFile = false;
        int _batchID = 0;
        string fullFilePath = _filePath + _fileName;

        _batchID = EmployeeController.manufactureBatchID(_employerID, _modOn, _modBy);

        System.IO.StreamReader file = null;

        int totalRecords = 0;
        int failedRecords = 0;
        try
        {

            this.Log.Info("Creating a datatable.");

            DataTable employees = EmployeeController.GetNewImportEmployeeDataTable();

            file = new System.IO.StreamReader(fullFilePath);
            string line = null;

            if (false == file.ReadLine().IsHeaderRow())
            {
                file.Close();
                file = new System.IO.StreamReader(fullFilePath);
            }

            this.Log.Info(String.Format("Looping through {0}.", fullFilePath));

            while ((line = file.ReadLine()) != null)
            {
                if (line.Trim() == null || line.Trim().Equals(string.Empty))
                    continue;
                DataRow row = employees.NewRow();

                string _ssn = null;
                string _cdate = null;
                string _dob = null;

                totalRecords += 1;

                String[] gp = CsvParse.SplitRow(line);
                if (DataValidation.StringArrayContainsOnlyBlanks(gp))
                {
                    this.Log.Info(
                                String.Format("Skipping row for demographics processing in file {0}, all colums where blank.", fullFilePath)
                            );
                    continue;
                }
                if (gp.Count() == 16)
                {

                    row["fName"] = gp[0].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["mName"] = gp[1].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["lName"] = gp[2].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["address"] = gp[3].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["city"] = gp[4].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["stateAbb"] = gp[5].Trim(new char[] { ' ', '"' }).TruncateLength(2).checkForDBNull();
                    row["zip"] = gp[6].Trim(new char[] { ' ', '"' }).TruncateLength(5).ZeroPadZip().checkForDBNull();
                    row["i_hDate"] = gp[8].Trim(new char[] { ' ' }).TruncateLength(8).checkForDBNull();
                    _cdate = gp[9].Trim(new char[] { ' ' });
                    _ssn = gp[10].Trim(new char[] { ' ' });
                    row["ext_employee_id"] = gp[11].Trim(new char[] { ' ' }).checkForDBNull();
                    row["i_tDate"] = gp[12].Trim(new char[] { ' ' }).TruncateLength(8).checkForDBNull();
                    row["hr_status_ext_id"] = gp[13].Trim(new char[] { ' ' }).TruncateLength(50).checkForDBNull();
                    row["hr_description"] = gp[14].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    _dob = gp[15].Trim(new char[] { ' ' });
                    row["employerID"] = _employerID.checkIntDBNull();

                    if (_cdate == "00000000" || _cdate == "0")
                    {
                        _cdate = null;
                    }
                    row["i_cDate"] = _cdate.TruncateLength(8).checkForDBNull();

                    if (_dob == "00000000" || _dob == "0")
                    {
                        _dob = null;
                    }
                    row["i_dob"] = _dob.TruncateLength(8).checkForDBNull();

                    row["ssn"] = AesEncryption.Encrypt(_ssn).TruncateLength(50).checkForDBNull();

                    employees.Rows.Add(row);

                }
                else
                {

                    this.Log.Warn(String.Format("Invalid column count at line {0}, expected 16, found {1}.", totalRecords, gp.Count()));

                    failedRecords = 1;

                    break;

                }

            }

            if (false == EmployeeController.BulkImportEmployee(_batchID, employees))
            {

                this.Log.Warn(String.Format("EmployeeController.BulkImportEmployee returned false for batch {0}.", _batchID));

                failedRecords = 1;

            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            failedRecords = 1;

        }
        finally
        {
            if (file != null)
            {
                file.Close();
                file.Dispose();
            }
        }

        String emailSubject = "File Import: ";
        String emailBody = "File: " + _fileName + "<br />";
        emailBody += "Attempted By: " + _modBy + "<br />";
        emailBody += "Attempted On: " + _modOn.ToString() + " <br />";

        Email em = new Email();

        if (failedRecords == 0)
        {
            if (System.IO.File.Exists(fullFilePath))
            {
                validFile = true;
                emailSubject += "Succesful";
                emailBody += "Total Records: " + totalRecords.ToString() + "<br />";

                new FileArchiverWrapper().ArchiveFile(fullFilePath, _employerID, "Processing Dem_I File");

            }
        }
        else
        {

            validFile = false;
            emailSubject += "Failed";
            emailBody += "Error on line: " + totalRecords.ToString() + "<br />";

            EmployeeController.DeleteFailedBatch(_batchID);

            em.SendEmail(SystemSettings.EmailNotificationAddress, emailSubject, emailBody, true);

        }

        return validFile;
    }

    /// <summary>
    /// 3-3)
    /// </summary>
    public Boolean CrossReferenceData_DEM_I_data(int _employerID, int _planYearID, int _employeeClassID, int _acaStatusID, int _employeeTypeId)
    {

        List<Employee_I> tempEmpIList = new List<Employee_I>();
        List<hrStatus> tempHRList = new List<hrStatus>();
        List<EmployeeType> tempEmployeeTypeList = new List<EmployeeType>();
        List<PlanYear> tempPlanYearList = new List<PlanYear>();
        bool validProcess = false;

        try
        {

            employer currentEmployer = null;
            int _impMonths = 0;
            int _impID = 0;

            currentEmployer = employerController.getEmployer(_employerID);

            _impID = currentEmployer.EMPLOYER_INITIAL_MEAS_ID;

            _impMonths = measurementController.getInitialMeasurementLength(_impID);

            tempEmpIList = EmployeeController.manufactureImportEmployeeList(_employerID, 100000);             

            tempHRList = hrStatus_Controller.manufactureHRStatusList(_employerID);

            tempEmployeeTypeList = EmployeeTypeController.getEmployeeTypes(_employerID);

            tempPlanYearList = PlanYear_Controller.getEmployerPlanYear(_employerID);

            DataTable employees = EmployeeController.GetNewImportEmployeeDataTable();

            foreach (Employee_I emp in tempEmpIList)
            {

                emp.EMPLOYEE_HR_STATUS_ID = hrStatus_Controller.validateHRStatus(_employerID, emp.EMPLOYEE_HR_EXT_STATUS_ID, emp.EMPLOYEE_HR_EXT_DESCRIPTION, tempHRList);

                emp.EMPLOYEE_STATE_ID = StateController.findStateID(emp.EMPLOYEE_STATE_ABB);

                if (emp.EMPLOYEE_TYPE_ID == 0)
                {
                    emp.EMPLOYEE_TYPE_ID = employerController.validateEmployerEmployeeTypes(tempEmployeeTypeList, _employeeTypeId);
                }

                if (emp.EMPLOYEE_PLAN_YEAR_ID_MEAS == 0)
                {
                    emp.EMPLOYEE_PLAN_YEAR_ID_MEAS = _planYearID;
                }

                if (emp.EMPLOYEE_ACT_STATUS_ID == 0)
                {
                    emp.EMPLOYEE_ACT_STATUS_ID = _acaStatusID;
                }

                if (emp.EMPLOYEE_CLASS_ID == 0)
                {
                    emp.EMPLOYEE_CLASS_ID = _employeeClassID;
                }

                emp.EMPLOYEE_HIRE_DATE = errorChecking.convertDateTime(emp.EMPLOYEE_I_HIRE_DATE);

                emp.EMPLOYEE_DOB = errorChecking.convertDateTime(emp.EMPLOYEE_I_DOB);

                if (emp.EMPLOYEE_I_C_DATE != null)
                {
                    emp.EMPLOYEE_C_DATE = errorChecking.convertDateTime(emp.EMPLOYEE_I_C_DATE);
                }

                if (emp.EMPLOYEE_I_TERM_DATE != null)
                {
                    emp.EMPLOYEE_TERM_DATE = errorChecking.convertDateTime(emp.EMPLOYEE_I_TERM_DATE);
                }

                try
                {
                    emp.EMPLOYEE_IMP_END = EmployeeController.calculateIMPEndDate((DateTime)emp.EMPLOYEE_HIRE_DATE, _impMonths);
                }
                catch (Exception exception)
                {
                    Log.Warn("Suppressing errors.", exception);
                    emp.EMPLOYEE_IMP_END = null;
                }

                EmployeeController.AddEmployeeIUpdateToDataTable(employees, emp);

            }

            validProcess = EmployeeController.BulkUpdateImportEmployee(employees);

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            validProcess = false;

        }

        return validProcess;

    }

    /// <summary>
    /// This function is used to INSERT new employees and UPDATE existing employees. 
    ///     - The logic for whether it's an INSERT or UPDATE is all dictated by the _employeeID. 
    ///     - If the employee exists in the EMPLOYEE table, an employeeID will be found and sent to the 
    ///     factory function. 
    ///     - Else a 0 will be passed to the factory function and a new employee will be created. 
    /// </summary>
    public Boolean TransferDemographicImportTableData(int _employerID, String _modBy, Boolean _initialImport, Boolean _ignoreNewHire)
    {

        Boolean validTransfer = false;
        List<Employee_I> importEmployeeList = new List<Employee_I>();
        List<Employee> employeeList = new List<Employee>();

        List<hrStatus> hrTempList = new List<hrStatus>();
        List<EmployeeType> empTypeTempList = new List<EmployeeType>();
        List<measurementType> measTypeTempList = new List<measurementType>();

        try
        {

            employer _employer = employerController.getEmployer(_employerID);
            importEmployeeList = EmployeeController.manufactureImportEmployeeList(_employerID, 100000);             
            employeeList = EmployeeController.manufactureEmployeeList(_employerID);

            foreach (Employee_I empI in importEmployeeList)
            {

                String empSSN = empI.Employee_SSN_Visible;
                int _employeeID = 0;
                int _rowID = 0;
                int _hrstatusID = 0;
                int _employerID2 = 0;
                String _fname = null;
                String _mname = null;
                String _lname = null;
                String _address = null;
                String _city = null;
                int _stateID = 0;
                String _zip = null;
                DateTime? _hdate = null;
                DateTime? _cdate = null;
                String _ssn = null;
                int _employeeTypeID_curr = 0;
                String _extEmployeeID = null;
                DateTime? _tdate = null;
                DateTime _dob;
                DateTime? _impEnd = null;
                int _planYearID = 0;
                int _planYearID_limbo = 0;
                int _planYearID_meas = 0;
                Boolean validData = true;
                DateTime _modOn = DateTime.Now;
                Boolean _offer = false;
                int _offerPlanYearID = 0;
                int _classID = 0;
                int _acaStatusID = 0;
                Boolean _imp = false;

                Employee currEmployee = EmployeeController.validateExistingEmployee(employeeList, empSSN);

                try
                {

                    if (currEmployee != null)
                    {

                        _employeeID = currEmployee.EMPLOYEE_ID;

                        _planYearID = currEmployee.EMPLOYEE_PLAN_YEAR_ID;
                        if (empI.EMPLOYEE_PLAN_YEAR_ID > 0)
                        {
                            _planYearID = empI.EMPLOYEE_PLAN_YEAR_ID;
                        }

                        _planYearID_limbo = currEmployee.EMPLOYEE_PLAN_YEAR_ID_LIMBO;
                        if (empI.EMPLOYEE_PLAN_YEAR_ID_LIMBO > 0)
                        {
                            _planYearID_limbo = empI.EMPLOYEE_PLAN_YEAR_ID_LIMBO;
                        }


                        _planYearID_meas = currEmployee.EMPLOYEE_PLAN_YEAR_ID_MEAS;
                        if (empI.EMPLOYEE_PLAN_YEAR_ID_MEAS > 0)
                        {
                            _planYearID_meas = empI.EMPLOYEE_PLAN_YEAR_ID_MEAS;
                        }

                        _employeeTypeID_curr = currEmployee.EMPLOYEE_TYPE_ID;
                        if (empI.EMPLOYEE_TYPE_ID > 0)
                        {
                            _employeeTypeID_curr = empI.EMPLOYEE_TYPE_ID;
                        }

                        _classID = currEmployee.EMPLOYEE_CLASS_ID;
                        if (empI.EMPLOYEE_CLASS_ID > 0)
                        {
                            _classID = empI.EMPLOYEE_CLASS_ID;
                        }

                        _acaStatusID = currEmployee.EMPLOYEE_ACT_STATUS_ID;
                        if (empI.EMPLOYEE_ACT_STATUS_ID > 0)
                        {
                            _acaStatusID = empI.EMPLOYEE_ACT_STATUS_ID;
                        }

                        _impEnd = currEmployee.EMPLOYEE_IMP_END;
                        if (empI.EMPLOYEE_IMP_END != null && (DateTime)empI.EMPLOYEE_IMP_END != new DateTime())
                        {
                            _impEnd = (DateTime)empI.EMPLOYEE_IMP_END;
                        }

                        _hdate = currEmployee.EMPLOYEE_HIRE_DATE;
                        if (empI.EMPLOYEE_HIRE_DATE != null && (DateTime)empI.EMPLOYEE_HIRE_DATE != new DateTime())
                        {
                            _hdate = (DateTime)empI.EMPLOYEE_HIRE_DATE;
                        }

                        if (empI.EMPLOYEE_HIRE_DATE != null && (DateTime)empI.EMPLOYEE_HIRE_DATE != new DateTime()
                            && currEmployee.EMPLOYEE_HIRE_DATE != (DateTime)empI.EMPLOYEE_HIRE_DATE)
                        {
                            _imp = EmployeeController.calculateIMP(_employerID, _employeeTypeID_curr, _planYearID_meas, (DateTime)_hdate, 1);
                        }

                    }
                    else
                    {

                        _planYearID_meas = empI.EMPLOYEE_PLAN_YEAR_ID_MEAS;
                        _employeeTypeID_curr = empI.EMPLOYEE_TYPE_ID;

                        if (null != empI.EMPLOYEE_IMP_END)
                        {
                            _impEnd = (DateTime)empI.EMPLOYEE_IMP_END;
                        }

                        if (null != empI.EMPLOYEE_HIRE_DATE)
                        {
                            _hdate = (DateTime)empI.EMPLOYEE_HIRE_DATE;
                            _imp = EmployeeController.calculateIMP(_employerID, _employeeTypeID_curr, _planYearID_meas, (DateTime)_hdate, 1);
                        }

                        _classID = empI.EMPLOYEE_CLASS_ID;
                        _acaStatusID = empI.EMPLOYEE_ACT_STATUS_ID;

                    }

                    _rowID = empI.ROW_ID;
                    _hrstatusID = empI.EMPLOYEE_HR_STATUS_ID;
                    _employerID2 = empI.EMPLOYEE_EMPLOYER_ID;
                    _fname = empI.EMPLOYEE_FIRST_NAME;
                    _mname = empI.EMPLOYEE_MIDDLE_NAME;
                    _lname = empI.EMPLOYEE_LAST_NAME;
                    _address = empI.EMPLOYEE_ADDRESS;
                    _city = empI.EMPLOYEE_CITY;
                    _stateID = empI.EMPLOYEE_STATE_ID;
                    _zip = empI.EMPLOYEE_ZIP;
                    _cdate = empI.EMPLOYEE_C_DATE;
                    _ssn = empI.Employee_SSN_Visible;

                    _extEmployeeID = empI.EMPLOYEE_EXT_ID;

                    if (Feature.EmployeeDemographicGenerateEmployeeNumberEnabled)
                    {

                        if (String.IsNullOrEmpty(_extEmployeeID))
                        {

                            _extEmployeeID = ConverterHelper.BuildDefaultEmployeeNumber(
                                                            Branding.ProductName,
                                                            _fname,
                                                            _lname,
                                                            _ssn
                                                        );

                        }

                    }

                    _tdate = empI.EMPLOYEE_TERM_DATE;

                    if (Feature.DOBDefaultValueEnabled == true)
                    {

                        if (empI.EMPLOYEE_DOB.HasValue == false)
                        {
                            _dob = new DateTime(1920, 1, 1);
                        }
                        else
                        {
                            _dob = empI.EMPLOYEE_DOB.Value;
                        }

                    }
                    else
                    {
                        _dob = (DateTime)empI.EMPLOYEE_DOB;
                    }

                    validData = errorChecking.validateStringNull(_fname, validData);
                    validData = errorChecking.validateStringNull(_lname, validData);
                    validData = errorChecking.validateStringNull(_address, validData);
                    validData = errorChecking.validateStringNull(_city, validData);
                    validData = errorChecking.validateStringZipCode(_zip, validData);
                    validData = errorChecking.validateStringDate(((DateTime)_hdate).ToShortDateString(), validData);
                    validData = _ssn.IsValidSsn() && validData;

                    if (Feature.EmployeeDemographicEmployeeNumberRequired)
                    {
                        validData = errorChecking.validateStringNull(_extEmployeeID, validData);
                    }

                    validData = errorChecking.validateStringDate(_dob.ToShortDateString(), validData);

                    if (_initialImport == true || _ignoreNewHire == true)
                    {

                        if (_imp == true && null == currEmployee)
                        {
                            validData = false;
                        }

                    }

                    if (validData == true && _planYearID_meas != 0 && _employeeTypeID_curr != 0 && _classID != 0 && _acaStatusID != 0)
                    {

                        EmployeeController.TransferImportedEmployee(
                                _employeeID,
                                _rowID,
                                _employeeTypeID_curr,
                                _hrstatusID,
                                _employerID2,
                                _fname,
                                _mname,
                                _lname,
                                _address,
                                _city,
                                _stateID,
                                _zip,
                                _hdate,
                                _cdate,
                                _ssn,
                                _extEmployeeID,
                                _tdate,
                                _dob,
                                _impEnd,
                                _planYearID,
                                _planYearID_limbo,
                                _planYearID_meas,
                                _modOn,
                                _modBy,
                                _offer,
                                _offerPlanYearID,
                                _classID,
                                _acaStatusID
                            );

                    }

                }
                catch (Exception exception)
                {

                    this.Log.Warn("Suppressing errors.", exception);

                }

            }

            validTransfer = true;

        }
        catch (Exception exception)
        {

            this.Log.Warn("Suppressing errors.", exception);

            validTransfer = false;

        }

        return validTransfer;

    }


    public string generateTextFile(List<Employee_E> _tempList, employer _employer)
    {
        string fileName = null;
        string currDate = errorChecking.convertShortDate(DateTime.Now.ToShortDateString());
        int count = _tempList.Count();
        string fullFilePath = null;
        List<classification_aca> acaList = classificationController.manufactureACAstatusList();
        List<classification> classList = classificationController.ManufactureEmployerClassificationList(_employer.EMPLOYER_ID, true);

        int _employerID2 = _employer.EMPLOYER_ID;

        fileName = Branding.ProductName.ToUpper() + "_EXPORT_" + _employer.EMPLOYER_IMPORT_EC + "_" + count + "_" + currDate + ".csv";
        fullFilePath = HttpContext.Current.Server.MapPath("..\\ftps\\export\\") + fileName;

        string message1 = "*****Format of Employee Export and Re-Import *****";
        string message2 = "This file is for syncing an Employee Classification and ACA Status to each Employee.";
        string message3 = "Classification Instructions:";
        string message4 = "Enter the correct code for the Class ID for each employee. See the list below for the codes. If a code doesn't exist, you must create it in the software.";
        string message5 = "ACA Status Instructions:";
        string message6 = "Enter the ACA Status Code for each Employee. Below is a list of your choices.";
        string message7 = "ACA Status Selections:";

        try
        {
            using (StreamWriter sw = File.CreateText(fullFilePath))
            {
                sw.WriteLine(message1);
                sw.WriteLine(message2);
                sw.WriteLine(message3);
                sw.WriteLine(message4);

                foreach (classification cl in classList)
                {
                    string line = cl.CLASS_ID.ToString() + "," + cl.CLASS_DESC;
                    sw.WriteLine(line);
                }

                sw.WriteLine(message5);
                sw.WriteLine(message6);
                sw.WriteLine(message7);

                foreach (classification_aca ca in acaList)
                {
                    string line = ca.ACA_STATUS_ID.ToString() + "," + ca.ACA_STATUS_NAME;
                    sw.WriteLine(line);
                }

                sw.WriteLine("**************** Employee List Start *******************************");
                WriteEmployeeDataToStream(_tempList, sw);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            fileName = null;
        }

        return fileName;
    }

    public void WriteEmployeeDataToStream(List<Employee_E> _tempList, StreamWriter sw)
    {
        sw.WriteLine("Payroll ID, ID, First Name, Last Name, HR Status, Class ID, Class Desc, ACA Status ID, ACA Status Name");

        foreach (Employee_E ee in _tempList)
        {
            StringBuilder line = new StringBuilder();

            line.Append(ee.EMPLOYEE_EXT_ID);
            line.Append(",");
            line.Append(ee.EMPLOYEE_ID.ToString());
            line.Append(",");
            line.Append(ee.EMPLOYEE_FIRST_NAME);
            line.Append(",");
            line.Append(ee.EMPLOYEE_LAST_NAME);
            line.Append(",");
            line.Append(ee.EX_HR_STATUS_NAME);
            line.Append(",");
            line.Append(ee.EMPLOYEE_CLASS_ID);
            line.Append(",");
            line.Append(ee.EX_CLASS_NAME);
            line.Append(",");
            line.Append(ee.EMPLOYEE_ACT_STATUS_ID);
            line.Append(",");
            line.Append(ee.EX_ACA_NAME);

            sw.WriteLine(line.ToString());
        }
    }


    public bool updateEmployeeClassAcaStatus(int _employerID, int _employeeID, int _classID, int _acaID, string _modBy, DateTime _modOn)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_class", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            if (_classID == 0)
            {
                cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = _classID;
            }

            if (_acaID == 0)
            {
                cmd.Parameters.AddWithValue("@acaID", SqlDbType.Int).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.AddWithValue("@acaID", SqlDbType.Int).Value = _acaID;
            }

            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            PIILogger.LogPII(String.Format("saving employee stage 3: tsql:[{0}]", tsql));
            if (tsql > 0)
            {
                validTransaction = true;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// This function is meant to DELETE an actual employee, it will only happen if the Employee does not have any records attached to their ID.
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public bool deleteEmployee(int _employerID, int _employeeID)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }


    /// <summary>
    /// Manufacture an entire list of Employees for a specific Employer.
    /// </summary>
    /// <returns></returns>
    public List<Dependent> manufactureEmployeeDependentList(int _employeeID)
    {
        PIILogger.LogPII("Loading all Dependents for employee with Id:" + _employeeID);

        List<Dependent> tempList = new List<Dependent>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employee_dependents", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object dependentID = 0;
                object employeeID = null;
                object fname = null;
                object mname = null;
                object lname = null;
                object ssn = null;
                object dob = null;

                dependentID = rdr[0] as object ?? default(object);
                employeeID = rdr[1] as object ?? default(object);
                fname = rdr[2] as object ?? default(object);
                mname = rdr[3] as object ?? default(object);
                lname = rdr[4] as object ?? default(object);
                ssn = rdr[5] as object ?? default(object);
                dob = rdr[6] as object ?? default(object);


                int _dependentID = dependentID.checkIntNull();
                int _employeeID2 = employeeID.checkIntNull();
                string _fname = fname.checkStringNull();
                string _lname = lname.checkStringNull();
                string _mname = mname.checkStringNull();
                string _ssn = ssn.checkStringNull();
                DateTime? _dob = dob.checkDateNull();

                if (_ssn != null)
                {
                    _ssn = AesEncryption.Decrypt(_ssn);
                }

                Dependent tempDep = new Dependent(_dependentID, _employeeID, _fname, _mname, _lname, _ssn, _dob);
                tempList.Add(tempDep);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<Dependent>();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return tempList;
    }
    public bool addDependentCoverage(int _taxYear)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_ADD_insuranceCoverage", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            cmd.ExecuteNonQuery();

            return true;

        }
        catch (Exception exception)
        {
            Log.Warn("Exception in addDependentCoverage.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

    }
    /// <summary>
    /// Manufacture an entire list of Employees for a specific Employer.
    /// </summary>
    /// <returns></returns>
    public Dependent updateEmployeeDependent(int _dependentID, int _employeeID, string _fname, string _mname, string _lname, string _ssn, DateTime? _dob, string modBy, int entityStatusID)
    {
        Dependent currDependent = null;
        int dependentID = 0;
        int tsql = 0;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            var cmd = new SqlCommand("INSERT_UPDATE_employee_dependent", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (_ssn != null)
            {
                _ssn = AesEncryption.Encrypt(_ssn);
            }

            cmd.Parameters.AddWithValue("@currDepID", SqlDbType.Int).Value = _dependentID;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = _mname.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = _ssn.checkForDBNull();
            cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = _dob.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@entityStatusID", SqlDbType.Int).Value = entityStatusID.checkIntDBNull();
            cmd.Parameters.Add("@dependentID", SqlDbType.Int);
            cmd.Parameters["@dependentID"].Direction = ParameterDirection.Output;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                dependentID = (int)cmd.Parameters["@dependentID"].Value;
                currDependent = new Dependent(dependentID, _employeeID, _fname, _mname, _lname, _ssn, _dob);
            }


        }
        catch (Exception exception)
        {
            Log.Warn("Exception in updateEmployeeDependent.", exception);
            currDependent = null;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return currDependent;
    }

    /// <summary>
    /// Manufacture an entire list of Employees for a specific Employer.
    /// </summary>
    /// <returns></returns>
    public bool deleteEmployeeDependent(int _dependentID, int _employeeID)
    {
        int tsql = 0;
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            var cmd = new SqlCommand("DELETE_dependent", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@dependentID", SqlDbType.Int).Value = _dependentID;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// Moves a dependent from Employee to Another.
    /// </summary>
    /// <returns></returns>
    public bool migrateEmployeeDependent(int _dependentID, int _employeeIDold, int _employeeIDnew)
    {
        int tsql = 0;
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            var cmd = new SqlCommand("MIGRATE_dependent_single", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@dependentID", SqlDbType.Int).Value = _dependentID;
            cmd.Parameters.AddWithValue("@employeeIDold", SqlDbType.Int).Value = _employeeIDold;
            cmd.Parameters.AddWithValue("@employeeIDnew", SqlDbType.Int).Value = _employeeIDnew;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// Distructor to be sure all resources are released
    /// </summary>
    ~EmployeeFactory()
    {
        if (null != rdr && false == rdr.IsClosed)
        {
            rdr.Close();
        }
        if (null != conn)
        {
            conn.Dispose();
        }
    }

    /// <summary>
    /// This will completely remove an employee from the ACA database. This is used on the employee merge screen. 
    /// </summary>
    /// <param name="employeeID"></param>
    /// <returns></returns>
    public Boolean nukeEmployeeFromACA(int employeeID)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("REMOVE_employee_from_aca", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = employeeID;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;

            }

        }
        catch (Exception exception)
        {
            Log.Warn("Failure to DELETE an employee from ACA.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }
}