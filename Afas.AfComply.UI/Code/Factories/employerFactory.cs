using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;
using System.Configuration;
using Afas.Domain;
using Afas.Application.CSV;

/// <summary>
/// Summary description for districtFactory
/// </summary>
public class employerFactory
{
    private ILog Log = LogManager.GetLogger(typeof(employerFactory));


    private static string connAcaString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private static string connAirString = System.Configuration.ConfigurationManager.ConnectionStrings["AIR_Conn"].ConnectionString;

    private SqlDataReader rdr = null;
    private SqlConnection conn = null;

    public bool insertEmployerCalculation(int _employerID)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();
            cmd.CommandText = "INSERT_NightlyCalculation";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.NVarChar).Value = "SYSTEM";

            cmd.ExecuteReader();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }
    /// <summary>
    /// Update all fields for a specific district. 
    /// </summary>
    /// <param name="_distID"></param>
    /// <param name="_name"></param>
    /// <param name="_address"></param>
    /// <param name="_city"></param>
    /// <param name="_stateID"></param>
    /// <param name="_zip"></param>
    /// <param name="_contact1"></param>
    /// <param name="_email1"></param>
    /// <param name="_phone1"></param>
    /// <param name="_contact2"></param>
    /// <param name="_email2"></param>
    /// <param name="_phone2"></param>
    /// <returns></returns>
    public bool updateEmployer(int _employerID, string _name, string _address, string _city, int _stateID, string _zip, string _imgPath, string _ein, int _empType, string _dbaName)
    {

        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();
        try
        {
            conn.Open();
            cmd.CommandText = "UPDATE_employer";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name;
            cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = _address;
            cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = _city;
            cmd.Parameters.AddWithValue("@stateID", SqlDbType.Int).Value = _stateID;
            cmd.Parameters.AddWithValue("@zip", SqlDbType.VarChar).Value = _zip;
            cmd.Parameters.AddWithValue("@logo", SqlDbType.VarChar).Value = _imgPath;
            cmd.Parameters.AddWithValue("@ein", SqlDbType.VarChar).Value = _ein;
            cmd.Parameters.AddWithValue("@employerTypeId", SqlDbType.Int).Value = _empType;
            cmd.Parameters.AddWithValue("@DBAName", SqlDbType.VarChar).Value = _dbaName.checkForDBNull();

            SqlDateTime nullDate = SqlDateTime.Null;

            cmd.ExecuteReader();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }

    /// <summary>
    /// Stored Proc to update the IRS reporting toggle
    /// </summary>
    /// <param name="_employerID">Employer Id to toggle</param>
    /// <param name="IrsEnabled">Value to set as.</param>
    /// <returns>true is success, false is failure</returns>
    public bool updateEmployer_IrsEnabled(int _employerID, bool IrsEnabled)
    {

        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();
        try
        {
            conn.Open();
            cmd.CommandText = "UPDATE_employer_IRS_Enabled";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@irsEnabled", SqlDbType.Bit).Value = IrsEnabled.checkBoolDBNull();

            cmd.ExecuteReader();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }
    public bool updateEmployer_Step(int _employerID, int taxYearId, int stepId)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();
        try
        {
            conn.Open();
            cmd.CommandText = "UPDATE_employer_Step";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;
            cmd.Parameters.AddWithValue("@stepId", SqlDbType.Int).Value = stepId;

            cmd.ExecuteReader();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }
    public bool UnApprove1095NeedingCorrection(int etytID)
    {

        try
        {

            using (SqlConnection conn = new SqlConnection(connAcaString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UnApproveAllErrant1095ForTransmission", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@transmissionID", SqlDbType.Int).Value = etytID;

                cmd.ExecuteNonQuery();

                return true;
            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return false;

    }

    /// <summary>
    /// Update all fields for a specific district. 
    /// </summary>
    /// <param name="_distID"></param>
    /// <param name="_name"></param>
    /// <param name="_address"></param>
    /// <param name="_city"></param>
    /// <param name="_stateID"></param>
    /// <param name="_zip"></param>
    /// <param name="_contact1"></param>
    /// <param name="_email1"></param>
    /// <param name="_phone1"></param>
    /// <param name="_contact2"></param>
    /// <param name="_email2"></param>
    /// <param name="_phone2"></param>
    /// <returns></returns>
    public bool updateEmployerSUfee(int _employerID, bool _suFee)
    {

        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();
        try
        {
            conn.Open();
            cmd.CommandText = "UPDATE_employer_su_fee";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@sufee", SqlDbType.Bit).Value = _suFee;

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }


    public bool updateEmployerSetup(int _employerID, string _iei, string _iec, string _ftpei, string _ftpec, string _ipi, string _ipc, string _ftppi, string _ftppc, string _ip, bool _billing, bool _fileUpload, string _paySU, string _demSU, string _gpSU, string _hrSU, int _vendorID, string _ecSU, string _ioSU, string _icSU, string _payMod)
    {

        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();
        bool succesful = false;
        try
        {
            conn.Open();
            cmd.CommandText = "UPDATE_employer_setup";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@iei", SqlDbType.VarChar).Value = _iei ?? string.Empty;
            cmd.Parameters.AddWithValue("@iec", SqlDbType.VarChar).Value = _iec ?? string.Empty;
            cmd.Parameters.AddWithValue("@ftpei", SqlDbType.VarChar).Value = _ftpei ?? string.Empty;
            cmd.Parameters.AddWithValue("@ftpec", SqlDbType.Int).Value = (object)_ftpec ?? DBNull.Value;
            cmd.Parameters.AddWithValue("@ipi", SqlDbType.VarChar).Value = _ipi ?? string.Empty;
            cmd.Parameters.AddWithValue("@ipc", SqlDbType.VarChar).Value = _ipc ?? string.Empty;
            cmd.Parameters.AddWithValue("@ftppi", SqlDbType.VarChar).Value = _ftppi ?? string.Empty;
            cmd.Parameters.AddWithValue("@ftppc", SqlDbType.VarChar).Value = _ftppc ?? string.Empty;
            cmd.Parameters.AddWithValue("@ip", SqlDbType.VarChar).Value = _ip ?? string.Empty;
            cmd.Parameters.AddWithValue("@billing", SqlDbType.Bit).Value = _billing;
            cmd.Parameters.AddWithValue("@fileUpload", SqlDbType.Bit).Value = _fileUpload;
            cmd.Parameters.AddWithValue("@demoSU", SqlDbType.VarChar).Value = _demSU ?? string.Empty;
            cmd.Parameters.AddWithValue("@paySU", SqlDbType.VarChar).Value = _paySU ?? string.Empty;
            cmd.Parameters.AddWithValue("@gpSU", SqlDbType.VarChar).Value = _gpSU ?? string.Empty;
            cmd.Parameters.AddWithValue("@hrSU", SqlDbType.VarChar).Value = _hrSU ?? string.Empty;
            cmd.Parameters.AddWithValue("@vendorID", SqlDbType.VarChar).Value = _vendorID;
            cmd.Parameters.AddWithValue("@ecSU", SqlDbType.VarChar).Value = _ecSU ?? string.Empty;
            cmd.Parameters.AddWithValue("@ioSU", SqlDbType.VarChar).Value = _ioSU ?? string.Empty;
            cmd.Parameters.AddWithValue("@icSU", SqlDbType.VarChar).Value = _icSU ?? string.Empty;
            cmd.Parameters.AddWithValue("@payMod", SqlDbType.VarChar).Value = _payMod ?? string.Empty;

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {
                succesful = true;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            succesful = false;
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

        return succesful;
    }

    public int newRegistration(Registration _re)
    {
        int _employerID = 0;

        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();
        try
        {
            conn.Open();

            cmd.CommandText = "INSERT_new_registration";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            DateTime _test1 = System.Convert.ToDateTime(_re.REG_RENEWAL_DATE);

            string salt = HashPassword.HashedPassword.CreateSalt(_re.REG_ADMIN_USER.User_UserName);
            string hashedPassword = HashPassword.HashedPassword.HashPassword(salt, _re.REG_ADMIN_USER.User_Password);

            cmd.Parameters.AddWithValue("@empTypeID", SqlDbType.Int).Value = _re.REG_EMP_ID;
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _re.REG_EMP_NAME;
            cmd.Parameters.AddWithValue("@ein", SqlDbType.VarChar).Value = _re.REG_EIN;
            cmd.Parameters.AddWithValue("@add", SqlDbType.VarChar).Value = _re.REG_ADDRESS;
            cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = _re.REG_CITY;
            cmd.Parameters.AddWithValue("@stateID", SqlDbType.Int).Value = _re.REG_STATE_ID;
            cmd.Parameters.AddWithValue("@zip", SqlDbType.VarChar).Value = _re.REG_ZIP;
            cmd.Parameters.AddWithValue("@userfname", SqlDbType.VarChar).Value = _re.REG_ADMIN_USER.User_First_Name;
            cmd.Parameters.AddWithValue("@userlname", SqlDbType.VarChar).Value = _re.REG_ADMIN_USER.User_Last_Name;
            cmd.Parameters.AddWithValue("@useremail", SqlDbType.VarChar).Value = _re.REG_ADMIN_USER.User_Email;
            cmd.Parameters.AddWithValue("@userphone", SqlDbType.VarChar).Value = _re.REG_ADMIN_USER.User_Phone;
            cmd.Parameters.AddWithValue("@username", SqlDbType.VarChar).Value = _re.REG_ADMIN_USER.User_UserName;
            cmd.Parameters.AddWithValue("@password", SqlDbType.VarChar).Value = hashedPassword;
            cmd.Parameters.AddWithValue("@active", SqlDbType.Bit).Value = _re.REG_ADMIN_USER.USER_ACTIVE;
            cmd.Parameters.AddWithValue("@power", SqlDbType.Bit).Value = _re.REG_ADMIN_USER.User_Power;
            cmd.Parameters.AddWithValue("@billing", SqlDbType.Bit).Value = _re.REG_ADMIN_USER.User_Billing;
            cmd.Parameters.AddWithValue("@b_add", SqlDbType.VarChar).Value = _re.REG_BILL_ADDRESS;
            cmd.Parameters.AddWithValue("@b_city", SqlDbType.VarChar).Value = _re.REG_BILL_CITY;
            cmd.Parameters.AddWithValue("@b_stateID", SqlDbType.Int).Value = _re.REG_BILL_STATE;
            cmd.Parameters.AddWithValue("@b_zip", SqlDbType.VarChar).Value = _re.REG_BILL_ZIP;
            cmd.Parameters.AddWithValue("@p_desc1", SqlDbType.VarChar).Value = _re.REG_PLANNAME1;

            DateTime _rd1 = DateTime.Parse(_re.REG_RENEWAL_DATE.ToString());
            cmd.Parameters.AddWithValue("@p_start1", SqlDbType.DateTime).Value = _rd1.ToString("yyyy-MM-dd HH:mm:ss");
            if (_re.REG_PLANNAME2 == null || _re.REG_RENEWAL_DATE2 == null)
            {
                SqlDateTime nullDate = SqlDateTime.Null;
                SqlString nullString = SqlString.Null;
                cmd.Parameters.AddWithValue("@p_desc2", SqlDbType.VarChar).Value = nullString;
                cmd.Parameters.AddWithValue("@p_start2", SqlDbType.DateTime).Value = nullDate;
            }
            else
            {
                DateTime _rd2 = System.Convert.ToDateTime(_re.REG_RENEWAL_DATE2);
                cmd.Parameters.AddWithValue("@p_desc2", SqlDbType.VarChar).Value = _re.REG_PLANNAME2;
                cmd.Parameters.AddWithValue("@p_start2", SqlDbType.DateTime).Value = _rd2.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (_re.REG_BILL_USER != null)
            {
                string salt2 = HashPassword.HashedPassword.CreateSalt(_re.REG_BILL_USER.User_UserName);
                string hashedPassword2 = HashPassword.HashedPassword.HashPassword(salt2, _re.REG_BILL_USER.User_Password);

                cmd.Parameters.AddWithValue("@b_fname", SqlDbType.VarChar).Value = _re.REG_BILL_USER.User_First_Name;
                cmd.Parameters.AddWithValue("@b_lname", SqlDbType.VarChar).Value = _re.REG_BILL_USER.User_Last_Name;
                cmd.Parameters.AddWithValue("@b_email", SqlDbType.VarChar).Value = _re.REG_BILL_USER.User_Email;
                cmd.Parameters.AddWithValue("@b_phone", SqlDbType.VarChar).Value = _re.REG_BILL_USER.User_Phone;
                cmd.Parameters.AddWithValue("@b_username", SqlDbType.VarChar).Value = _re.REG_BILL_USER.User_UserName;
                cmd.Parameters.AddWithValue("@b_password", SqlDbType.VarChar).Value = hashedPassword2;
                cmd.Parameters.AddWithValue("@b_active", SqlDbType.Bit).Value = _re.REG_BILL_USER.USER_ACTIVE;
                cmd.Parameters.AddWithValue("@b_power", SqlDbType.Bit).Value = _re.REG_BILL_USER.User_Power;
                cmd.Parameters.AddWithValue("@b_billing", SqlDbType.Bit).Value = _re.REG_BILL_USER.User_Billing;
            }
            else
            {
                cmd.Parameters.AddWithValue("@b_fname", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@b_lname", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@b_email", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@b_phone", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@b_username", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@b_password", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@b_active", SqlDbType.Bit).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@b_power", SqlDbType.Bit).Value = DBNull.Value;
                cmd.Parameters.AddWithValue("@b_billing", SqlDbType.Bit).Value = DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@DBAName", SqlDbType.Bit).Value = _re.DbaName.checkForDBNull();


            cmd.Parameters.Add("@employerID", SqlDbType.Int);
            cmd.Parameters["@employerID"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            _employerID = (int)cmd.Parameters["@employerID"].Value;

            return _employerID;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return _employerID;
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vendor manufactureEmployerVendor(int _vendorID)
    {
        Vendor tempVendor = null;
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        try
        {

            conn = new SqlConnection(connAcaString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_vendor", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@vendorID", SqlDbType.Int).Value = _vendorID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;
                object name = null;
                object autoUpload = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                autoUpload = rdr[2] as object ?? default(object);

                int _id = id.checkIntNull();
                string _name = name.checkStringNull();
                bool _autoUpload = autoUpload.checkBoolNull();

                tempVendor = new Vendor(_id, _name, _autoUpload);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempVendor = null;
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

        return tempVendor;
    }

    public List<long> GetTransmittedApprovedIds(int employerId)
    {

        List<long> results = new List<long>();

        try
        {
            using (SqlConnection conn = new SqlConnection(connAcaString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT_Transmitted_EmployeeIds", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployerId", SqlDbType.Int).Value = employerId;

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        object id = rdr[0] as object ?? default(object);
                        int _id = id.checkIntNull();

                        results.Add(_id);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return new List<long>();
        }

        return results;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Vendor> manufactureVendors()
    {
        List<Vendor> tempList = new List<Vendor>();
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        try
        {

            conn = new SqlConnection(connAcaString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_all_vendors", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;
                object name = null;
                object autoUpload = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                autoUpload = rdr[2] as object ?? default(object);

                int _id = id.checkIntNull();
                string _name = name.checkStringNull();
                bool _autoUpload = autoUpload.checkBoolNull();

                Vendor tempVendor = new Vendor(_id, _name, _autoUpload);
                tempList.Add(tempVendor);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<Vendor>();
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<batch> manufactureBatchList(int _employerID)
    {
        List<batch> tempList = new List<batch>();
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        try
        {

            conn = new SqlConnection(connAcaString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_employer_batch_top25", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object employerID = null;
                object modOn = null;
                object modBy = null;
                object delOn = null;
                object delBy = null;

                id = rdr[0] as object ?? default(object);
                employerID = rdr[1] as object ?? default(object);
                modOn = rdr[2] as object ?? default(object);
                modBy = rdr[3] as object ?? default(object);
                delOn = rdr[4] as object ?? default(object);
                delBy = rdr[5] as object ?? default(object);

                int _id = id.checkIntNull();
                int _employerID2 = employerID.checkIntNull();
                DateTime _modOn = (DateTime)modOn;
                string _modBy = modBy.checkStringNull();
                DateTime? _delOn = delOn.checkDateNull();
                string _delBy = delBy.checkStringNull();

                batch bt = new batch(_id, _employerID2, _modOn, _modBy, _delOn, _delBy);
                tempList.Add(bt);
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


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<batch> manufactureBatchListInsuranceCarrierImport(int _employerID)
    {
        List<batch> tempList = new List<batch>();
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        try
        {

            conn = new SqlConnection(connAcaString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_employer_batch_top100_insurance_carrier", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object employerID = null;
                object modOn = null;
                object modBy = null;
                object delOn = null;
                object delBy = null;

                id = rdr[0] as object ?? default(object);
                employerID = rdr[1] as object ?? default(object);
                modOn = rdr[2] as object ?? default(object);
                modBy = rdr[3] as object ?? default(object);
                delOn = rdr[4] as object ?? default(object);
                delBy = rdr[5] as object ?? default(object);

                int _id = id.checkIntNull();
                int _employerID2 = employerID.checkIntNull();
                DateTime _modOn = (DateTime)modOn;
                string _modBy = modBy.checkStringNull();
                DateTime? _delOn = delOn.checkDateNull();
                string _delBy = delBy.checkStringNull();

                batch bt = new batch(_id, _employerID2, _modOn, _modBy, _delOn, _delBy);
                tempList.Add(bt);
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int getSummerHourRecordsCount(int _planYearID)
    {
        int count = 0;
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        try
        {

            conn = new SqlConnection(connAcaString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_employer_payroll_summer_avg", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID;
            cmd.Parameters.Add("@rowCount", SqlDbType.Int);
            cmd.Parameters["@rowCount"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            count = (int)cmd.Parameters["@rowCount"].Value;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            count = 0;
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

        return count;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public tax_year_submission manufactureTaxYearSubmission(int _employerID, int _taxYear)
    {
        tax_year_submission tys = null;
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        try
        {

            conn = new SqlConnection(connAcaString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_employer_irs_submission", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = rdr[0] as object ?? default(object);
                object employerID2 = rdr[1] as object ?? default(object);
                object taxYear2 = rdr[2] as object ?? default(object);
                object dge = rdr[3] as object ?? default(object);
                object dgeName = rdr[4] as object ?? default(object);
                object dgeEIN = rdr[5] as object ?? default(object);
                object dgeAddress = rdr[6] as object ?? default(object);
                object dgeCity = rdr[7] as object ?? default(object);
                object dgeStateID = rdr[8] as object ?? default(object);
                object dgeZip = rdr[9] as object ?? default(object);
                object dgeFname = rdr[10] as object ?? default(object);
                object dgeLname = rdr[11] as object ?? default(object);
                object dgePhone = rdr[12] as object ?? default(object);
                object aale = rdr[13] as object ?? default(object);
                object tr_question1 = rdr[14] as object ?? default(object);
                object tr_question2 = rdr[15] as object ?? default(object);
                object tr_question3 = rdr[16] as object ?? default(object);
                object tr_question4 = rdr[17] as object ?? default(object);
                object tr_question5 = rdr[18] as object ?? default(object);
                object tr = rdr[19] as object ?? default(object);
                object tobacco = rdr[20] as object ?? default(object);
                object unpaidLeave = rdr[21] as object ?? default(object);
                object safeHarbor = rdr[22] as object ?? default(object);
                object completedBy = rdr[23] as object ?? default(object);
                object completedOn = rdr[24] as object ?? default(object);
                object ebcApproved = rdr[25] as object ?? default(object);
                object ebcApprovedBy = rdr[26] as object ?? default(object);
                object ebcApprovedOn = rdr[27] as object ?? default(object);
                object allowEditing = rdr[28] as object ?? default(object);

                int _id = id.checkIntNull();
                int _employerID2 = employerID2.checkIntNull();
                int _taxYear2 = taxYear2.checkIntNull();
                bool? _dge = dge.checkBoolNull2();
                string _dgeName = dgeName.checkStringNull();
                string _dgeEIN = dgeEIN.checkStringNull();
                string _dgeAddress = dgeAddress.checkStringNull();
                string _dgeCity = dgeCity.checkStringNull();
                int _dgeStateID = dgeStateID.checkIntNull();
                string _dgeZip = dgeZip.checkStringNull();
                string _dgeFname = dgeFname.checkStringNull();
                string _dgeLname = dgeLname.checkStringNull();
                string _dgePhone = dgePhone.checkStringNull();
                bool? _aale = aale.checkBoolNull();
                bool? _tr_question1 = tr_question1.checkBoolNull2();
                bool? _tr_question2 = tr_question2.checkBoolNull2();
                bool? _tr_question3 = tr_question3.checkBoolNull2();
                bool? _tr_question4 = tr_question4.checkBoolNull2();
                bool? _tr_question5 = tr_question5.checkBoolNull2();
                bool _tr = tr.checkBoolNull();
                bool? _safeHarbor = safeHarbor.checkBoolNull2();
                bool? _tobacco = tobacco.checkBoolNull2();
                bool? _unpaidLeave = unpaidLeave.checkBoolNull2();
                string _completedBy = completedBy.checkStringNull();
                DateTime? _completedOn = completedOn.checkDateNull();
                bool _ebcApproved = ebcApproved.checkBoolNull();
                string _ebcApprovedBy = ebcApprovedBy.checkStringNull();
                DateTime? _ebcApprovedOn = ebcApprovedOn.checkDateNull();
                bool _allowEditing = allowEditing.checkBoolNull();

                List<ale> _aleMembers = new List<ale>();

                tys = new tax_year_submission(_id, _employerID, _taxYear, _dge, _dgeEIN, _dgeName, _dgeAddress, _dgeCity, _dgeStateID, _dgeZip, _dgeFname, _dgeLname, _dgePhone, _aale, _aleMembers, _tr_question1, _tr_question2, _tr_question3, _tr_question4, _tr_question5, _tr, _safeHarbor, _tobacco, _unpaidLeave, _completedBy, _completedOn, _ebcApproved, _ebcApprovedBy, _ebcApprovedOn, _allowEditing);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tys = null;
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

        return tys;
    }

    public List<Employee_IRS> getEmployeeWithEmployerInfo(int employerId, int taxYearId, bool getCoverageAndDependents)
    {
        DataTable dtEmployersWithEmployerInfo = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_employee_with_employer_info", connString))
            {
                cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = employerId;
                cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployersWithEmployerInfo);
                }
            }
        }

        string[] skipProperties = { "Is_Self_Insured", "PersonBirthDt", "Employee_Coverage", "Dependents" };

        List<Employee_IRS> employees = dtEmployersWithEmployerInfo.DataTableToList<Employee_IRS>(skipProperties);
        foreach (var employee in employees)
        {
            if (!string.IsNullOrEmpty(employee.SSN))
            {
                employee.PersonBirthDt = null;
            }
        }

        if (getCoverageAndDependents)
        {
            foreach (Employee_IRS employee in employees)
            {
                employee.Employee_Coverage.AddRange(getEmployeeCoverageInfo(employee.employee_id, taxYearId));
                employee.Is_Self_Insured = employee.Employee_Coverage.Any(c => c.insurance_type_id == 2);
                employee.Dependents.AddRange(getEmployeeDependents(employee.employee_id, taxYearId));
            }
        }

        return employees;
    }



    protected List<Coverage> getEmployeeCoverageInfo(int employeeID, int taxYearId)
    {
        DataTable dtEmployeeCoverageInfo = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_employee_offer_and_coverage", connString))
            {
                cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = taxYearId;
                cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = employeeID;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployeeCoverageInfo);
                }
            }
        }

        return dtEmployeeCoverageInfo.DataTableToList<Coverage>();

    }

    protected List<Dependent_IRS> getEmployeeDependents(int employeeId, int taxYearId)
    {
        DataTable dtEmployeeDependents = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("[SELECT_employee_tax_year_dependents]", connString))
            {
                cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;
                cmd.Parameters.AddWithValue("@employeeId", SqlDbType.Int).Value = employeeId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployeeDependents);
                }
            }
        }

        var dependents = dtEmployeeDependents.DataTableToList<Dependent_IRS>();

        foreach (var dep in dependents)
        {
            if (!string.IsNullOrEmpty(dep.ssn))
            {
                dep.dob = null;
            }
        }

        return dependents;
    }

    public bool updateInsertIrsSubmissionApproval(tax_year_submission tys)
    {

        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();
        bool successful = false;
        int approvalID = 0;

        try
        {
            conn.Open();
            cmd.CommandText = "INSERT_UPDATE_employer_irs_submission_approval";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@approvalID", SqlDbType.Int).Value = tys.IRS_ROW_ID.checkIntNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = tys.IRS_EMPLOYER_ID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = tys.IRS_TAX_YEAR;
            cmd.Parameters.AddWithValue("@dge", SqlDbType.Bit).Value = tys.IRS_DGE.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@dgeName", SqlDbType.VarChar).Value = tys.IRS_DGE_NAME.checkForDBNull();
            cmd.Parameters.AddWithValue("@dgeEIN", SqlDbType.VarChar).Value = tys.IRS_DGE_EIN.checkForDBNull();
            cmd.Parameters.AddWithValue("@dgeAddress", SqlDbType.VarChar).Value = tys.IRS_DGE_ADDRESS.checkForDBNull();
            cmd.Parameters.AddWithValue("@dgeCity", SqlDbType.VarChar).Value = tys.IRS_DGE_CITY.checkForDBNull();
            cmd.Parameters.AddWithValue("@dgeStateID", SqlDbType.Int).Value = tys.IRS_DGE_STATE_ID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@dgeZip", SqlDbType.VarChar).Value = tys.IRS_DGE_ZIP.checkForDBNull();
            cmd.Parameters.AddWithValue("@dgeFname", SqlDbType.VarChar).Value = tys.IRS_DGE_CONTACT_FNAME.checkForDBNull();
            cmd.Parameters.AddWithValue("@dgeLname", SqlDbType.VarChar).Value = tys.IRS_DGE_CONTACT_LNAME.checkForDBNull();
            cmd.Parameters.AddWithValue("@dgePhone", SqlDbType.VarChar).Value = tys.IRS_DGE_PHONE.checkForDBNull();
            cmd.Parameters.AddWithValue("@ale", SqlDbType.Bit).Value = tys.IRS_ALE.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@tr1", SqlDbType.Bit).Value = tys.IRS_TR_Q1.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@tr2", SqlDbType.Bit).Value = tys.IRS_TR_Q2.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@tr3", SqlDbType.Bit).Value = tys.IRS_TR_Q3.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@tr4", SqlDbType.Bit).Value = tys.IRS_TR_Q4.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@tr5", SqlDbType.Bit).Value = tys.IRS_TR_Q5.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@tr", SqlDbType.Bit).Value = tys.IRS_TR.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@tobacco", SqlDbType.Bit).Value = tys.IRS_TOBACCO.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@unpaidLeave", SqlDbType.Bit).Value = tys.IRS_UNPAID_LEAVE.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@safeHarbor", SqlDbType.Bit).Value = tys.IRS_ASH.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@completedBy", SqlDbType.Bit).Value = tys.IRS_COMPLETED_BY.checkForDBNull();
            cmd.Parameters.AddWithValue("@completedOn", SqlDbType.Bit).Value = tys.IRS_COMPLETED_ON.checkDateDBNull();
            cmd.Parameters.AddWithValue("@ebcApproved", SqlDbType.Bit).Value = tys.IRS_EBC_APPROVED.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@ebcApprovedBy", SqlDbType.Bit).Value = tys.IRS_EBC_APPROVED_BY.checkForDBNull();
            cmd.Parameters.AddWithValue("@ebcApprovedOn", SqlDbType.Bit).Value = tys.IRS_EBC_APPROVED_ON.checkDateDBNull();
            cmd.Parameters.AddWithValue("@allowEditing", SqlDbType.Bit).Value = tys.IRS_EDIT.checkBoolNull();
            cmd.Parameters.Add("@approvalID_Final", SqlDbType.Int);
            cmd.Parameters["@approvalID_Final"].Direction = ParameterDirection.Output;

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {

                successful = true;
                approvalID = (int)cmd.Parameters["@approvalID_Final"].Value;

            }
            else
            {
                this.Log.Warn(String.Format("INSERT_UPDATE_employer_irs_submission_approval did not receive back the proper rows, expected greater than 0, received: {0}", rows));
            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            successful = false;
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

        return successful;

    }

    public EmployerTaxYearTransmission insertUpdateEmployerTaxYearTransmission(EmployerTaxYearTransmission employerTaxYearTransmission)
    {


        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        try
        {

            conn.Open();
            cmd.CommandText = "INSERT_UPDATE_employer_tax_year_transmission";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerTaxYearTransmissionId", SqlDbType.Int).Value = employerTaxYearTransmission.EmployerTaxYearTransmissionId;
            cmd.Parameters["@employerTaxYearTransmissionId"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerTaxYearTransmission.EmployerId;
            cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = employerTaxYearTransmission.TaxYearId;
            cmd.Parameters.AddWithValue("@entityStatusId", SqlDbType.Int).Value = employerTaxYearTransmission.EntityStatusId;
            cmd.Parameters.AddWithValue("@createdBy", SqlDbType.NVarChar).Value = employerTaxYearTransmission.CreatedBy;
            cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.NVarChar).Value = employerTaxYearTransmission.ModifiedBy;

            cmd.ExecuteNonQuery();

            employerTaxYearTransmission.EmployerTaxYearTransmissionId = (int)cmd.Parameters["@employerTaxYearTransmissionId"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
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

        return employerTaxYearTransmission;

    }

    public EmployerTaxYearTransmissionStatus insertUpdateEmployerTaxYearTransmissionStatus(EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();
            cmd.CommandText = "INSERT_UPDATE_employer_tax_year_transmission_status";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerTaxYearTransmissionStatusId", SqlDbType.Int).Value = employerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId;
            cmd.Parameters["@employerTaxYearTransmissionStatusId"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@employerTaxYearTransmissionId", SqlDbType.Int).Value = employerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId;
            cmd.Parameters.AddWithValue("@transmissionStatusId", SqlDbType.Int).Value = employerTaxYearTransmissionStatus.TransmissionStatusId;
            cmd.Parameters.AddWithValue("@entityStatusId", SqlDbType.Int).Value = employerTaxYearTransmissionStatus.EntityStatusId;
            cmd.Parameters.AddWithValue("@startDate", SqlDbType.DateTime2).Value = employerTaxYearTransmissionStatus.StartDate;
            cmd.Parameters.AddWithValue("@endDate", SqlDbType.DateTime2).Value = employerTaxYearTransmissionStatus.EndDate;
            cmd.Parameters.AddWithValue("@createdBy", SqlDbType.NVarChar).Value = employerTaxYearTransmissionStatus.CreatedBy;
            cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.NVarChar).Value = employerTaxYearTransmissionStatus.ModifiedBy;

            cmd.ExecuteNonQuery();
            employerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId = (int)cmd.Parameters["@employerTaxYearTransmissionStatusId"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
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

        return employerTaxYearTransmissionStatus;

    }

    public Boolean insertUpdateTaxYear1095cCorrection(TaxYear1095CCorrection taxYear1095CCorrection)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();
            cmd.CommandText = "INSERT_UPDATE_employer_tax_year_1095c_correction";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@tax_yearCorrectionId", SqlDbType.Int).Value = taxYear1095CCorrection.tax_yearCorrectionId;
            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.Int).Value = taxYear1095CCorrection.tax_year;
            cmd.Parameters.AddWithValue("@employee_id", SqlDbType.Int).Value = taxYear1095CCorrection.employee_id;
            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = taxYear1095CCorrection.employer_id;
            cmd.Parameters.AddWithValue("@Corrected", SqlDbType.Int).Value = taxYear1095CCorrection.Corrected;
            cmd.Parameters.AddWithValue("@OriginalUniqueSubmissionId", SqlDbType.Int).Value = taxYear1095CCorrection.OriginalUniqueSubmissionId;
            cmd.Parameters.AddWithValue("@CorrectedUniqueSubmissionId", SqlDbType.Int).Value = taxYear1095CCorrection.CorrectedUniqueSubmissionId;
            cmd.Parameters.AddWithValue("@CorrectedUniqueRecordId", SqlDbType.Int).Value = taxYear1095CCorrection.CorrectedUniqueRecordId;
            cmd.Parameters.AddWithValue("@Transmitted", SqlDbType.Int).Value = taxYear1095CCorrection.Transmitted;
            cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.Int).Value = taxYear1095CCorrection.ModifiedBy;

            cmd.ExecuteNonQuery();

            return true;

        }
        catch (Exception exception)
        {
            Log.Warn("Exception in insertUpdateTaxYear1095cCorrection.", exception);
            return false;
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

    }

    public Boolean updateTaxYear1095cCorrectionCorrectedBit(TaxYear1095CCorrection taxYear1095CCorrection)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();
            cmd.CommandText = "UPDATE_employer_tax_year_1095c_correction_corrected_bit";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.Int).Value = taxYear1095CCorrection.tax_year;
            cmd.Parameters.AddWithValue("@employee_id", SqlDbType.Int).Value = taxYear1095CCorrection.employee_id;
            cmd.Parameters.AddWithValue("@Corrected", SqlDbType.Int).Value = taxYear1095CCorrection.Corrected;
            cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.Int).Value = taxYear1095CCorrection.ModifiedBy;

            cmd.ExecuteNonQuery();

            return true;

        }
        catch (Exception exception)
        {
            Log.Warn("Exception in updateTaxYear1095cCorrectionCorrectedBit.", exception);
            return false;
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
    }

    public Boolean updateTaxYear1095cCorrectionTransmittedBit(TaxYear1095CCorrection taxYear1095CCorrection)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();
            cmd.CommandText = "UPDATE_employer_tax_year_1095c_correction_transmitted_bit";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.Int).Value = taxYear1095CCorrection.tax_year;
            cmd.Parameters.AddWithValue("@employee_id", SqlDbType.Int).Value = taxYear1095CCorrection.employee_id;
            cmd.Parameters.AddWithValue("@Transmitted", SqlDbType.Int).Value = taxYear1095CCorrection.Transmitted;
            cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.Int).Value = taxYear1095CCorrection.ModifiedBy;

            cmd.ExecuteNonQuery();

            return true;

        }
        catch (Exception exception)
        {
            Log.Warn("Exception in updateTaxYear1095cCorrectionCorrectedBit.", exception);
            return false;
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
    }

    public Boolean stageTaxYear1095cCorrection(int employerId, int taxYearId, String modifiedBy)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();
            cmd.CommandText = "StageForCorrectionRetransmission";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@taxYearID", SqlDbType.Int).Value = taxYearId;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = employerId;
            cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.Int).Value = modifiedBy;

            cmd.ExecuteNonQuery();

            return true;

        }
        catch (Exception exception)
        {
            Log.Warn("Exception in stageTaxYear1095cCorrection.", exception);
            return false;
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
    }

    public EmployerTaxYearTransmission getEmployerTaxYearTransmissionByEmployerTaxYearTransmissionId(int employerTaxYearTransmissionId)
    {
        EmployerTaxYearTransmission employerTaxYearTransmission = null;
        SqlConnection conn = null;
        DataTable dtEmployerTaxYearTransmission = new DataTable();

        try
        {
            conn = new SqlConnection(connAcaString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[SELECT_employer_tax_year_transmission]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerTaxYearTransmissionId", SqlDbType.Int).Value = employerTaxYearTransmissionId;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtEmployerTaxYearTransmission);

            employerTaxYearTransmission = dtEmployerTaxYearTransmission.DataTableToObject<EmployerTaxYearTransmission>();

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return employerTaxYearTransmission;
    }

    public EmployerTaxYearTransmission getEmployerTaxYearTransmissionByEmployerIdAndTaxYearId(int employerId, int taxYearId)
    {
        EmployerTaxYearTransmission employerTaxYearTransmission = null;
        SqlConnection conn = null;
        DataTable dtEmployerTaxYearTransmission = new DataTable();

        try
        {
            conn = new SqlConnection(connAcaString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[SELECT_employer_tax_year_transmission]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
            cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtEmployerTaxYearTransmission);

            employerTaxYearTransmission = dtEmployerTaxYearTransmission.DataTableToObject<EmployerTaxYearTransmission>();

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return employerTaxYearTransmission;
    }

    public EmployerTaxYearTransmissionStatus getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(int employerId, int taxYearId)
    {
        EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus = null;
        SqlConnection conn = null;
        DataTable dtEmployerTaxYearTransmission = new DataTable();

        try
        {
            conn = new SqlConnection(connAcaString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[SELECT_current_employer_tax_year_transmission_status]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
            cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtEmployerTaxYearTransmission);

            employerTaxYearTransmissionStatus = dtEmployerTaxYearTransmission.DataTableToObject<EmployerTaxYearTransmissionStatus>();
            DataRow employerTaxYearTransmissionStatusObj = dtEmployerTaxYearTransmission.AsEnumerable().FirstOrDefault();
            if (employerTaxYearTransmissionStatusObj != null)
            {
                employerTaxYearTransmissionStatus.EndDate = (DateTime?)employerTaxYearTransmissionStatusObj["EndDate"].checkDateNull();
            }


        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return employerTaxYearTransmissionStatus;
    }

    public EmployerTaxYearTransmissionStatus getCurrentEmployerTaxYearTransmissionStatusBeforeHalt(int employerId, int taxYearId)
    {
        EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus = null;
        SqlConnection conn = null;
        DataTable dtEmployerTaxYearTransmission = new DataTable();

        try
        {
            conn = new SqlConnection(connAcaString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_transmission_status_before_halt", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
            cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtEmployerTaxYearTransmission);

            employerTaxYearTransmissionStatus = dtEmployerTaxYearTransmission.DataTableToObject<EmployerTaxYearTransmissionStatus>();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return employerTaxYearTransmissionStatus;
    }

    public List<IRSStatus> getIRSStatus(int employerId, int taxYearId)
    {

        DataTable dtIrsStatuses = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_current_employer_tax_year_irs_status", connString))
            {
                cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;
                cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtIrsStatuses);
                }
            }
        }

        return dtIrsStatuses.DataTableToList<IRSStatus>();

    }

    public List<EmployerTaxYearTransmissionStatus> getEmployerTaxYearTransmissionStatusesByEmployerIdAndTaxYearId(int employerId, int taxYearId)
    {
        DataTable dtEmployerTransmissionStatuses = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_employer_transmission_statuses_by_tax_year", connString))
            {
                cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;
                cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployerTransmissionStatuses);
                }
            }
        }

        return dtEmployerTransmissionStatuses.DataTableToList<EmployerTaxYearTransmissionStatus>();
    }

    public List<EmployersCurrentTaxYearTransmissionStatus> getEmployersCurrentTaxYearTransmissionStatus(int taxYearId)
    {
        DataTable dtEmployersCurrentTaxYearTransmissionStatus = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_current_employers_tax_year_transmission_status", connString))
            {
                cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployersCurrentTaxYearTransmissionStatus);
                }
            }
        }

        return dtEmployersCurrentTaxYearTransmissionStatus.DataTableToList<EmployersCurrentTaxYearTransmissionStatus>();
    }

    public EmployerTaxYearTransmissionStatus getCurrentEmployerTaxYearTransmissionStatusById(int employerTransmissionTaxYearStatusId)
    {
        DataTable dtEmployerTaxYearTransmissionStatus = new DataTable();

        using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
        {
            using (var cmd = new SqlCommand("[SELECT_employer_transmission_tax_year_status]", connString))
            {
                cmd.Parameters.AddWithValue("@employerTransmissionTaxYearStatusId", SqlDbType.Int).Value = employerTransmissionTaxYearStatusId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployerTaxYearTransmissionStatus);
                }
            }
        }

        return dtEmployerTaxYearTransmissionStatus.DataTableToObject<EmployerTaxYearTransmissionStatus>();
    }

    public EmployerTaxYearTransmissionStatus getCurrentEmployerTaxYearTransmissionStatusByEmployerResourceIdAndTaxYearId(Guid employerResourceId, int taxYearId)
    {
        EmployerTaxYearTransmissionStatus employerTaxYearTransmissionStatus = null;
        SqlConnection conn = null;
        DataTable dtEmployerTaxYearTransmission = new DataTable();

        try
        {
            conn = new SqlConnection(connAcaString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[SELECT_current_employer_tax_year_transmission_status]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerResourceId", SqlDbType.UniqueIdentifier).Value = employerResourceId;
            cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtEmployerTaxYearTransmission);

            employerTaxYearTransmissionStatus = dtEmployerTaxYearTransmission.DataTableToObject<EmployerTaxYearTransmissionStatus>();

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return employerTaxYearTransmissionStatus;
    }

    public EmployerTaxYearTransmissionStatusQueue insertUpdateEmployerTaxYearTransmissionStatusQuene(EmployerTaxYearTransmissionStatusQueue employerTaxYearTransmissionStatusQueue)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();
            cmd.CommandText = "INSERT_UPDATE_employer_transmission_status_queue";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerTaxYearTransmissionStatusQueueId", SqlDbType.Int).Value = employerTaxYearTransmissionStatusQueue.EmployerTaxYearTransmissionStatusQueueId;
            cmd.Parameters["@employerTaxYearTransmissionStatusQueueId"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@employerTaxYearTransmissionStatusId", SqlDbType.Int).Value = employerTaxYearTransmissionStatusQueue.EmployerTaxYearTransmissionStatusId;
            cmd.Parameters.AddWithValue("@queueStatusId", SqlDbType.Int).Value = employerTaxYearTransmissionStatusQueue.QueueStatusId;
            cmd.Parameters.AddWithValue("@title", SqlDbType.NVarChar).Value = employerTaxYearTransmissionStatusQueue.Title;
            cmd.Parameters.AddWithValue("@message", SqlDbType.NVarChar).Value = employerTaxYearTransmissionStatusQueue.Message;
            cmd.Parameters.AddWithValue("@body", SqlDbType.NVarChar).Value = employerTaxYearTransmissionStatusQueue.Body;
            cmd.Parameters.AddWithValue("@entityStatusId", SqlDbType.Int).Value = employerTaxYearTransmissionStatusQueue.EntityStatusId;
            cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.NVarChar).Value = employerTaxYearTransmissionStatusQueue.ModifiedBy;
            cmd.Parameters.AddWithValue("@createdBy", SqlDbType.NVarChar).Value = employerTaxYearTransmissionStatusQueue.CreatedBy;

            cmd.ExecuteNonQuery();
            employerTaxYearTransmissionStatusQueue.EmployerTaxYearTransmissionStatusQueueId = (int)cmd.Parameters["@employerTaxYearTransmissionStatusQueueId"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
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

        return employerTaxYearTransmissionStatusQueue;
    }

    public List<EmployerTaxYearTransmissionStatusQueue> getEmployerTaxYearTransmissionStatusQueues(int batchSize)
    {
        DataTable dtEmployerTaxYearTransmissionStatusQueues = new DataTable();

        using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
        {
            using (var cmd = new SqlCommand("[SELECT_employer_tax_year_transmission_status_queue]", connString))
            {
                cmd.Parameters.AddWithValue("@batchSize", SqlDbType.Int).Value = batchSize;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployerTaxYearTransmissionStatusQueues);
                }
            }
        }

        return dtEmployerTaxYearTransmissionStatusQueues.DataTableToList<EmployerTaxYearTransmissionStatusQueue>();
    }

    public List<int> getTaxYears()
    {
        DataTable dtTaxYears = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("[SELECT_tax_years]", connString))
            {
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtTaxYears);
                }
            }
        }

        return dtTaxYears.AsEnumerable().Select(r => r.Field<int>("tax_year")).ToList();
    }

    public List<Form1094CUpstreamDetail> getForm1094CUpstreamDetails(int taxYearId, int? employerId, bool corrected1094, bool corrected1095, bool rejectedInd)
    {
        this.Log.Warn(string.Format("entered getForm1094CUpstreamDetails taxYearId: {0}, employerId: {1}, corrected: {2}", taxYearId, employerId, corrected1094));

        List<Form1094CUpstreamDetail> form1094CUpstreamDetails = new List<Form1094CUpstreamDetail>();
        DataTable dtForm1094CUpstreamDetail = new DataTable();

        try
        {

            using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
            {
                using (var cmd = new SqlCommand("[SELECT_form_1094C_upstream_detail]", connString))
                {
                    cmd.CommandTimeout = Transmitter.Timeout;
                    cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;
                    cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
                    cmd.Parameters.AddWithValue("@correctedInd", SqlDbType.Bit).Value = corrected1094;

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(dtForm1094CUpstreamDetail);
                    }
                }
            }

            foreach (DataRow row in dtForm1094CUpstreamDetail.Rows)
            {

                string[] skipProperties = { "TaxYr", "BusinessNameLine2Txt", "ALEMemberInformationGrp", "GovtEntityEmployerInfoGrp", "OtherALEMembersGrps", "Form1095CUpstreamDetails",
                                        "Form1095CAttachedCnt", "PersonNameControlTxt", "TestScenarioId", "BusinessNameControlTxt", "AddressLine2Txt", "JuratSignaturePIN",
                                        "PersonTitleTxt", "SignatureDt", "dtSignature"};

                var form1094CUpstreamDetail = row.DataRowToObject<Form1094CUpstreamDetail>(skipProperties);
                form1094CUpstreamDetail.TaxYr = taxYearId.ToString();

                if (corrected1094)
                {
                    form1094CUpstreamDetail.CorrectedSubmissionInfoGrp = row.DataRowToObject<CorrectedSubmissionInfoGrp>();
                }

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.BusinessNameLine1Txt) == false)
                    form1094CUpstreamDetail.BusinessNameLine1Txt = form1094CUpstreamDetail.BusinessNameLine1Txt.RemoveIllegalBusinessIRSCharacters().Trim().TruncateLongString(74);

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.BusinessNameLine2Txt) == false)
                    form1094CUpstreamDetail.BusinessNameLine2Txt = form1094CUpstreamDetail.BusinessNameLine2Txt.RemoveIllegalBusinessIRSCharacters().Trim().TruncateLongString(74);

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.AddressLine1Txt) == false)
                {
                    form1094CUpstreamDetail.AddressLine1Txt = form1094CUpstreamDetail.AddressLine1Txt.RemoveIllegalAddressIRSCharacters().RemoveDoubleSpaces().Trim().TruncateLongString(35);
                }

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.PersonFirstNm) == false)
                    form1094CUpstreamDetail.PersonFirstNm = form1094CUpstreamDetail.PersonFirstNm.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19);

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.PersonMiddleNm) == false)
                    form1094CUpstreamDetail.PersonMiddleNm = form1094CUpstreamDetail.PersonMiddleNm.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19);

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.PersonLastNm) == false)
                    form1094CUpstreamDetail.PersonLastNm = form1094CUpstreamDetail.PersonLastNm.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19);

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.ContactPhoneNum) == false)
                {
                    form1094CUpstreamDetail.ContactPhoneNum = form1094CUpstreamDetail.ContactPhoneNum.RemoveDashes().Trim();
                }

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.EmployerEIN) == false)
                {
                    form1094CUpstreamDetail.EmployerEIN = form1094CUpstreamDetail.EmployerEIN.RemoveDashes().Trim();
                }

                if (String.IsNullOrEmpty(form1094CUpstreamDetail.CityNm) == false)
                {
                    form1094CUpstreamDetail.CityNm = form1094CUpstreamDetail.CityNm.RemoveIllegalCityIRSCharacters().RemoveNumbers().Trim().TruncateLongString(22);
                }

                form1094CUpstreamDetail.ALEMemberInformationGrp = getALEMemberInformationGrp(taxYearId, employerId);

                if (form1094CUpstreamDetail.dge)
                {
                    form1094CUpstreamDetail.GovtEntityEmployerInfoGrp = row.DataRowToObject<GovtEntityEmployerInfoGrp>();
                }

                if (form1094CUpstreamDetail.AggregatedGroupMemberCd.Equals("0"))
                {

                    form1094CUpstreamDetail.AggregatedGroupMemberCd = "2";

                }
                else if (form1094CUpstreamDetail.AggregatedGroupMemberCd.Equals("1"))
                {

                    List<OtherALEMembersGrp> aleMembers = getOtherALEMembersGrp(employerId.Value);

                    foreach (OtherALEMembersGrp member in aleMembers)
                    {

                        member.OtherALEBusinessNameLine1Txt = member.OtherALEBusinessNameLine1Txt.RemoveIllegalBusinessIRSCharacters().Trim().TruncateLongString(74);

                        member.OtherALEEIN = member.OtherALEEIN.Replace("-", "");

                        if (member.OtherALEBusinessNameLine2Txt.IsNullOrEmpty() == false)
                        {
                            member.OtherALEBusinessNameLine2Txt = member.OtherALEBusinessNameLine2Txt.RemoveIllegalBusinessIRSCharacters().Trim().TruncateLongString(74);
                        }

                    }

                    form1094CUpstreamDetail.OtherALEMembersGrps = aleMembers;

                }

                if (corrected1094)
                {
                    form1094CUpstreamDetail.Form1095CUpstreamDetails = new List<Form1095CUpstreamDetail>();
                }
                else
                {
                    form1094CUpstreamDetail.Form1095CUpstreamDetails = getForm1095CUpstreamDetails(form1094CUpstreamDetail.employer_id, taxYearId, corrected1095, rejectedInd);
                }

                form1094CUpstreamDetail.Form1095CAttachedCnt = form1094CUpstreamDetail.Form1095CUpstreamDetails.Count.ToString();
                form1094CUpstreamDetails.Add(form1094CUpstreamDetail);
            }
        }
        catch (Exception ex)
        {
            this.Log.Warn(string.Format("Exception in getForm1094CUpstreamDetails taxYearId: {0}, employerId: {1}, corrected: {2}", taxYearId, employerId, corrected1094), ex);
        }

        return form1094CUpstreamDetails;
    }

    /// <summary>
    /// This pulls all the employee 1095c details for the original and submission replacement transmission only.
    /// Corrections and Transmission Replacements come from somewhere else. 
    /// </summary>
    /// <param name="employerId"></param>
    /// <param name="taxYearId"></param>
    /// <param name="corrected"></param>
    /// <returns></returns>
    public List<Form1095CUpstreamDetail> getForm1095CUpstreamDetails(int employerId, int taxYearId, bool corrected, bool rejected)
    {
        this.Log.Warn(string.Format("entered getForm1095CUpstreamDetails taxYearId: {0}, employerId: {1}, corrected: {2}", taxYearId, employerId, corrected));

        DataTable dtForm1095CUpstreamDetail = new DataTable();
        List<Form1095CUpstreamDetail> form1095CUpstreamDetails = new List<Form1095CUpstreamDetail>();

        try
        {
            using (var connString = new SqlConnection(connAcaString))
            {
                using (var cmd = new SqlCommand("SELECT_form_1095C_upstream_detail", connString))
                {
                    cmd.CommandTimeout = Transmitter.Timeout;
                    cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
                    cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;
                    cmd.Parameters.AddWithValue("@correctedInd", SqlDbType.Bit).Value = corrected;
                    cmd.Parameters.AddWithValue("@rejectedInd", SqlDbType.Bit).Value = rejected;

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(dtForm1095CUpstreamDetail);
                    }
                }
            }

            foreach (DataRow row in dtForm1095CUpstreamDetail.Rows)
            {
                string[] skipProperties = { "BirthDt", "MonthlyEmployeeRequiredContriGrp", "MonthlyOfferCoverageGrp", "CorrectedRecordInfoGrp", "MonthlySafeHarborGrp", "CoveredIndividualGrps", "TaxYr", "PersonNameControlTxt", "TestScenarioId" };
                var form1095CUpstreamDetail = row.DataRowToObject<Form1095CUpstreamDetail>(skipProperties);
                form1095CUpstreamDetail.TaxYr = taxYearId.ToString();

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.SSN))
                {
                    form1095CUpstreamDetail.BirthDt = form1095CUpstreamDetail.DOB.ToString("yyyy-MM-dd");
                }
                else
                {
                    form1095CUpstreamDetail.SSN = AesEncryption.Decrypt(form1095CUpstreamDetail.SSN).Replace("l", "").RemoveDashes().Trim();
                    form1095CUpstreamDetail.BirthDt = String.Empty;
                }

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.ALEContactPhoneNum) == false)
                {
                    form1095CUpstreamDetail.ALEContactPhoneNum = form1095CUpstreamDetail.ALEContactPhoneNum.RemoveDashes().Trim();
                }

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.AddressLine1Txt) == false)
                {
                    form1095CUpstreamDetail.AddressLine1Txt = form1095CUpstreamDetail.AddressLine1Txt.RemoveIllegalAddressIRSCharacters().RemoveDoubleSpaces().Trim().TruncateLongString(35);
                }

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.CityNm) == false)
                {
                    form1095CUpstreamDetail.CityNm = form1095CUpstreamDetail.CityNm.RemoveIllegalCityIRSCharacters().RemoveNumbers().Trim().TruncateLongString(22);
                }

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.OtherCompletePersonFirstNm) == false)
                {
                    form1095CUpstreamDetail.OtherCompletePersonFirstNm = form1095CUpstreamDetail.OtherCompletePersonFirstNm.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19);
                }

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.OtherCompletePersonLastNm) == false)
                {
                    form1095CUpstreamDetail.OtherCompletePersonLastNm = form1095CUpstreamDetail.OtherCompletePersonLastNm.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19);
                }

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.AnnlEmployeeRequiredContriAmt))
                    form1095CUpstreamDetail.MonthlyEmployeeRequiredContriGrp = row.DataRowToObject<MonthlyEmployeeRequiredContriGrp>();

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.AnnualOfferOfCoverageCd))
                    form1095CUpstreamDetail.MonthlyOfferCoverageGrp = row.DataRowToObject<MonthlyOfferCoverageGrp>();

                if (String.IsNullOrEmpty(form1095CUpstreamDetail.AnnualSafeHarborCd))
                    form1095CUpstreamDetail.MonthlySafeHarborGrp = row.DataRowToObject<MonthlySafeHarborGrp>();

                if (corrected)
                {
                    form1095CUpstreamDetail.CorrectedRecordInfoGrp = row.DataRowToObject<CorrectedRecordInfoGrp>();
                }

                form1095CUpstreamDetail.CoveredIndividualGrps = getCoveredIndividualGrps(form1095CUpstreamDetail.employee_id, taxYearId);

                form1095CUpstreamDetails.Add(form1095CUpstreamDetail);

            }
        }
        catch (Exception ex)
        {
            this.Log.Warn(string.Format("Exception in getForm1095CUpstreamDetails taxYearId: {0}, employerId: {1}, corrected: {2}", taxYearId, employerId, corrected), ex);
        }

        return form1095CUpstreamDetails;

    }

    public List<CoveredIndividualGrp> getCoveredIndividualGrps(int employeeId, int taxYearId)
    {
        DataTable dtEmployeeCoveredIndividuals = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_employee_covered_individuals", connString))
            {
                cmd.Parameters.AddWithValue("@employeeId", SqlDbType.Int).Value = employeeId;
                cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployeeCoveredIndividuals);
                }
            }
        }

        List<CoveredIndividualGrp> coveredIndividualGrps = new List<CoveredIndividualGrp>();

        foreach (DataRow row in dtEmployeeCoveredIndividuals.Rows)
        {
            string[] skipProperties = { "BirthDt", "CoveredIndividualMonthlyIndGrp", "PersonNameControlTxt" };
            var coveredIndividualGrp = row.DataRowToObject<CoveredIndividualGrp>(skipProperties);

            if (String.IsNullOrEmpty(coveredIndividualGrp.SSN))
            {

                if (coveredIndividualGrp.DOB <= new DateTime(1920, 1, 2))
                {
                    coveredIndividualGrp.BirthDt = String.Empty;
                }
                else
                {
                    coveredIndividualGrp.BirthDt = coveredIndividualGrp.DOB.ToString("yyyy-MM-dd");
                }

            }
            else
            {
                coveredIndividualGrp.SSN = AesEncryption.Decrypt(coveredIndividualGrp.SSN).RemoveDashes().Trim();
                coveredIndividualGrp.BirthDt = String.Empty;
            }

            if (String.IsNullOrEmpty(coveredIndividualGrp.PersonFirstNm) == false)
                coveredIndividualGrp.PersonFirstNm = coveredIndividualGrp.PersonFirstNm.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19);

            if (String.IsNullOrEmpty(coveredIndividualGrp.PersonMiddleNm) == false)
                coveredIndividualGrp.PersonMiddleNm = coveredIndividualGrp.PersonMiddleNm.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19);

            if (String.IsNullOrEmpty(coveredIndividualGrp.PersonLastNm) == false)
                coveredIndividualGrp.PersonLastNm = coveredIndividualGrp.PersonLastNm.RemoveIllegalNameIRSCharacters().RemoveNumbers().Trim().TruncateLongString(19);

            if (coveredIndividualGrp.CoveredIndividualAnnualInd != "1")
                coveredIndividualGrp.CoveredIndividualMonthlyIndGrp = row.DataRowToObject<CoveredIndividualMonthlyIndGrp>();

            coveredIndividualGrps.Add(coveredIndividualGrp);
        }

        return coveredIndividualGrps;

    }

    public ALEMemberInformationGrp getALEMemberInformationGrp(int taxYearId, int? employerId = null)
    {

        DataTable dtALEMemberInformationGrp = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_ale_member_information_monthly", connString))
            {
                cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearId;
                cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
                cmd.CommandType = CommandType.StoredProcedure;

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtALEMemberInformationGrp);
                }
            }
        }

        List<ALEMemberMonthlyInfo> info = dtALEMemberInformationGrp.DataTableToList<ALEMemberMonthlyInfo>();

        ALEMemberInformationGrp result = new ALEMemberInformationGrp(employerId ?? 0, info);

        return result;

    }

    public List<OtherALEMembersGrp> getOtherALEMembersGrp(int employerId)
    {
        DataTable dtOtherALEMembersGrp = new DataTable();

        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_other_ale_group_members", connString))
            {
                cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtOtherALEMembersGrp);
                }
            }
        }

        string[] skipProperties = { "OtherALEBusinessNameLine2Txt", "OtherALEBusinessNameControlTxt" };
        return dtOtherALEMembersGrp.DataTableToList<OtherALEMembersGrp>(skipProperties);

    }

    public header insertUpdateHeader(header header)
    {
        SqlConnection conn = null;

        try
        {
            conn = new SqlConnection(connAirString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[tr].[INSERT_UPDATE_header]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@header_id", SqlDbType.Int).Value = header.header_id;
            cmd.Parameters["@header_id"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@transmitter_control_code", SqlDbType.NChar).Value = header.transmitter_control_code;
            cmd.Parameters.AddWithValue("@unique_transmission_id", SqlDbType.NChar).Value = header.unique_transmission_id;
            cmd.Parameters.AddWithValue("@transmission_timestamp", SqlDbType.DateTime2).Value = header.transmission_timestamp;
            cmd.Parameters.AddWithValue("@1094c_id", SqlDbType.Int).Value = header._1094c_id;
            cmd.Parameters.AddWithValue("@transmitted_base_64", SqlDbType.VarChar).Value = header.transmitted_base_64;

            cmd.ExecuteNonQuery();
            header.header_id = (int)cmd.Parameters["@header_id"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return header;
    }

    public manifest insertUpdateManifest(manifest manifest)
    {
        SqlConnection conn = null;

        try
        {
            conn = new SqlConnection(connAirString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[br].[INSERT_UPDATE_manifest]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@header_id", SqlDbType.Int).Value = manifest.header_id;
            cmd.Parameters["@header_id"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@unique_transmission_id", SqlDbType.NChar).Value = manifest.unique_transmission_id;
            cmd.Parameters.AddWithValue("@payment_year", SqlDbType.NChar).Value = manifest.payment_year;
            cmd.Parameters.AddWithValue("@prior_year_indicator", SqlDbType.Bit).Value = manifest.prior_year_indicator;
            cmd.Parameters.AddWithValue("@ein", SqlDbType.NChar).Value = manifest.ein;
            cmd.Parameters.AddWithValue("@br_type_code", SqlDbType.NChar).Value = manifest.br_type_code;
            cmd.Parameters.AddWithValue("@test_file_indicator", SqlDbType.Char).Value = manifest.test_file_indicator;
            cmd.Parameters.AddWithValue("@original_unique_transmission_id", SqlDbType.NChar).Value = manifest.original_unique_transmission_id;
            cmd.Parameters.AddWithValue("@transmitter_foreign_entity_indicator", SqlDbType.Bit).Value = manifest.transmitter_foreign_entity_indicator;
            cmd.Parameters.AddWithValue("@original_receipt_id", SqlDbType.NChar).Value = manifest.original_receipt_id;
            cmd.Parameters.AddWithValue("@vendor_indicator", SqlDbType.NChar).Value = manifest.vendor_indicator;
            cmd.Parameters.AddWithValue("@vendor_id", SqlDbType.TinyInt).Value = manifest.vendor_id;
            cmd.Parameters.AddWithValue("@payee_count", SqlDbType.SmallInt).Value = manifest.payee_count;
            cmd.Parameters.AddWithValue("@payer_record_count", SqlDbType.Int).Value = manifest.payer_record_count;
            cmd.Parameters.AddWithValue("@software_id", SqlDbType.NVarChar).Value = manifest.software_id;
            cmd.Parameters.AddWithValue("@form_type", SqlDbType.NChar).Value = manifest.form_type;
            cmd.Parameters.AddWithValue("@binary_format", SqlDbType.NChar).Value = manifest.binary_format;
            cmd.Parameters.AddWithValue("@checksum", SqlDbType.NVarChar).Value = manifest.checksum;
            cmd.Parameters.AddWithValue("@attachment_byte_size", SqlDbType.Int).Value = manifest.attachment_byte_size;
            cmd.Parameters.AddWithValue("@document_system_file_name", SqlDbType.NVarChar).Value = manifest.document_system_file_name;
            cmd.Parameters.AddWithValue("@mtom", SqlDbType.VarChar).Value = manifest.mtom;
            cmd.Parameters.AddWithValue("@manifest_file_name", SqlDbType.NVarChar).Value = manifest.manifest_file_name;

            cmd.ExecuteNonQuery();
            manifest.header_id = (int)cmd.Parameters["@header_id"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return manifest;
    }

    public _1094C insertUpdate1094C(_1094C _1094C)
    {
        SqlConnection conn = null;

        try
        {
            conn = new SqlConnection(connAirString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[fdf].[INSERT_UPDATE_1094C]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_1094C_id", SqlDbType.Int).Value = _1094C._1094C_id;
            cmd.Parameters["@_1094C_id"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@header_id", SqlDbType.Int).Value = _1094C.header_id;
            cmd.Parameters.AddWithValue("@unique_transmission_id", SqlDbType.NChar).Value = _1094C.unique_transmission_id;
            cmd.Parameters.AddWithValue("@submission_id", SqlDbType.Int).Value = _1094C.submission_id;
            cmd.Parameters.AddWithValue("@test_scenario_id", SqlDbType.Int).Value = _1094C.test_scenario_id;
            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.SmallInt).Value = _1094C.tax_year;
            cmd.Parameters.AddWithValue("@corrected_indicator", SqlDbType.Bit).Value = _1094C.corrected_indicator;
            cmd.Parameters.AddWithValue("@corrected_usid", SqlDbType.NChar).Value = _1094C.corrected_usid;
            cmd.Parameters.AddWithValue("@corrected_tin", SqlDbType.NChar).Value = _1094C.corrected_tin;
            cmd.Parameters.AddWithValue("@ein", SqlDbType.NChar).Value = _1094C.ein;
            cmd.Parameters.AddWithValue("@authoritative_transmittal_indicator", SqlDbType.Bit).Value = _1094C.authoritative_transmittal_indicator;
            cmd.Parameters.AddWithValue("@_1095C_attached_count", SqlDbType.Int).Value = _1094C._1095C_attached_count;
            cmd.Parameters.AddWithValue("@_1095C_total_count", SqlDbType.SmallInt).Value = _1094C._1095C_total_count;
            cmd.Parameters.AddWithValue("@fulltime_employee_count", SqlDbType.Int).Value = _1094C.fulltime_employee_count;
            cmd.Parameters.AddWithValue("@total_employee_count", SqlDbType.Int).Value = _1094C.total_employee_count;
            cmd.Parameters.AddWithValue("@aag_code", SqlDbType.TinyInt).Value = _1094C.aag_code;
            cmd.Parameters.AddWithValue("@annual_aag_indicator", SqlDbType.Bit).Value = _1094C.annual_aag_indicator;
            cmd.Parameters.AddWithValue("@qom_indicator", SqlDbType.Bit).Value = _1094C.qom_indicator;
            cmd.Parameters.AddWithValue("@qom_transition_relief_indicator", SqlDbType.Bit).Value = _1094C.qom_transition_relief_indicator;
            cmd.Parameters.AddWithValue("@_4980H_transition_relief_indicator", SqlDbType.Bit).Value = _1094C._4980H_transition_relief_indicator;
            cmd.Parameters.AddWithValue("@annual_4980H_transition_relief_code", SqlDbType.Char).Value = _1094C.annual_4980H_transition_relief_code;
            cmd.Parameters.AddWithValue("@_98_percent_offer_method", SqlDbType.Bit).Value = _1094C._98_percent_offer_method;
            cmd.Parameters.AddWithValue("@annual_mec_code", SqlDbType.TinyInt).Value = _1094C.annual_mec_code;
            cmd.Parameters.AddWithValue("@self_insured", SqlDbType.Bit).Value = _1094C.self_insured;
            cmd.Parameters.AddWithValue("@vendor_id", SqlDbType.Int).Value = _1094C.vendor_id;

            cmd.ExecuteNonQuery();
            _1094C._1094C_id = (int)cmd.Parameters["@_1094C_id"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return _1094C;
    }

    public _1095C insertUpdate1095C(_1095C _1095C)
    {
        SqlConnection conn = null;

        try
        {
            conn = new SqlConnection(connAirString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[fdf].[INSERT_UPDATE_1095C]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_1095C_id", SqlDbType.Int).Value = _1095C._1095C_id;
            cmd.Parameters["@_1095C_id"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@_1094C_id", SqlDbType.Int).Value = _1095C._1094C_id;
            cmd.Parameters.AddWithValue("@unique_transmission_id", SqlDbType.NChar).Value = _1095C.unique_transmission_id;
            cmd.Parameters.AddWithValue("@record_id", SqlDbType.Int).Value = _1095C.record_id;
            cmd.Parameters.AddWithValue("@corrected_indicator", SqlDbType.Bit).Value = _1095C.corrected_indicator;
            cmd.Parameters.AddWithValue("@corrected_urid", SqlDbType.NChar).Value = _1095C.corrected_urid;
            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.SmallInt).Value = _1095C.tax_year;
            cmd.Parameters.AddWithValue("@employee_id", SqlDbType.Int).Value = _1095C.employee_id;
            cmd.Parameters.AddWithValue("@annual_offer_of_coverage_code", SqlDbType.NChar).Value = _1095C.annual_offer_of_coverage_code;
            cmd.Parameters.AddWithValue("@annual_share_lowest_cost_monthly_premium", SqlDbType.Money).Value = _1095C.annual_share_lowest_cost_monthly_premium;
            cmd.Parameters.AddWithValue("@annual_safe_harbor_code", SqlDbType.NChar).Value = _1095C.annual_safe_harbor_code;
            cmd.Parameters.AddWithValue("@enrolled", SqlDbType.Bit).Value = _1095C.enrolled;
            cmd.Parameters.AddWithValue("@insurance_type_id", SqlDbType.TinyInt).Value = _1095C.insurance_type_id;
            cmd.Parameters.AddWithValue("@must_supply_ci_info", SqlDbType.TinyInt).Value = _1095C.must_supply_ci_info;
            cmd.Parameters.AddWithValue("@is_1G", SqlDbType.TinyInt).Value = _1095C.is_1G;

            cmd.ExecuteNonQuery();
            _1095C._1095C_id = (int)cmd.Parameters["@_1095C_id"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return _1095C;
    }

    public status_request insertUpdateStatusRequest(status_request status_request)
    {
        SqlConnection conn = null;

        try
        {
            conn = new SqlConnection(connAirString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[sr].[INSERT_UPDATE_status_request]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@status_request_id", SqlDbType.Int).Value = status_request.status_request_id;
            cmd.Parameters["@status_request_id"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@header_id", SqlDbType.Int).Value = status_request.header_id;
            cmd.Parameters.AddWithValue("@receipt_id", SqlDbType.VarChar).Value = status_request.receipt_id.Trim();
            cmd.Parameters.AddWithValue("@status_code_id", SqlDbType.Int).Value = status_request.status_code_id;
            cmd.Parameters.AddWithValue("@sr_base_64", SqlDbType.VarChar).Value = status_request.sr_base_64.Trim();
            cmd.Parameters.AddWithValue("@return_time_utc", SqlDbType.NVarChar).Value = status_request.return_time_utc.Trim();
            cmd.Parameters.AddWithValue("@return_time_local", SqlDbType.DateTime).Value = status_request.return_time_local;
            cmd.Parameters.AddWithValue("@return_time_zone", SqlDbType.NChar).Value = status_request.return_time_zone.Trim();

            cmd.ExecuteNonQuery();
            status_request.status_request_id = (int)cmd.Parameters["@status_request_id"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return status_request;
    }

    public request_error insertUpdateRequestError(request_error request_error)
    {
        SqlConnection conn = null;

        try
        {
            conn = new SqlConnection(connAirString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[sr].[INSERT_UPDATE_request_error]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@error_id", SqlDbType.Int).Value = request_error.error_id;
            cmd.Parameters["@error_id"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters.AddWithValue("@error_type", SqlDbType.Int).Value = request_error.error_type;
            cmd.Parameters.AddWithValue("@status_request_id", SqlDbType.VarChar).Value = request_error.status_request_id;
            cmd.Parameters.AddWithValue("@transmitter_error_id", SqlDbType.Int).Value = request_error.transmitter_error_id;
            cmd.Parameters.AddWithValue("@error_message_code", SqlDbType.NVarChar).Value = request_error.error_message_code;
            cmd.Parameters.AddWithValue("@error_message_text", SqlDbType.NVarChar).Value = request_error.error_message_text;
            cmd.Parameters.AddWithValue("@x_path_content", SqlDbType.NVarChar).Value = request_error.x_path_content;

            cmd.ExecuteNonQuery();
            request_error.error_id = (int)cmd.Parameters["@error_id"].Value;

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return request_error;
    }

    public int generateMissingInsuranceAlerts(int _employerID, int _planYearID, int _generatePlanYearID, DateTime _stabilityEndDate, int _hours, string _modBy, DateTime _modOn, string _history)
    {
        int count = 0;
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connAcaString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("INSERT_PlanYear_Missing_insurance_offers", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@planYearEndDate", SqlDbType.DateTime).Value = _stabilityEndDate;
            cmd.Parameters.AddWithValue("@missingPlanYearID", SqlDbType.Int).Value = _generatePlanYearID;
            cmd.Parameters.AddWithValue("@currPlanYearID", SqlDbType.Int).Value = _planYearID;
            cmd.Parameters.AddWithValue("@hours", SqlDbType.Int).Value = _hours;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.Int).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.Int).Value = _modOn;
            cmd.Parameters.AddWithValue("@history", SqlDbType.Int).Value = _history;

            count = cmd.ExecuteNonQuery();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            count = 0;
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

        return count;
    }

    public bool NukeEmployer(int _employerID)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        cmd.CommandTimeout = 90;                    
        try
        {
            conn.Open();
            cmd.CommandText = "RESET_EMPLOYER";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Nuking Employer Failed: Suppressing errors.", exception);
            return false;
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
    }

    public bool NukeAirEmployer(int _employerID)
    {
        conn = new SqlConnection(connAirString);
        SqlCommand cmd = new SqlCommand();

        cmd.CommandTimeout = 90;                    
        try
        {
            conn.Open();
            cmd.CommandText = "etl.spReset_ale_employer";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Nuking Air Employer Failed: Suppressing errors.", exception);
            return false;
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
    }

    public bool ShortTransmit(int _employerID, int _taxYear)
    {
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        cmd.CommandTimeout = 90;                    
        try
        {
            conn.Open();
            cmd.CommandText = "sp_AIR_ETL_ShortBuild";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@year_id", SqlDbType.Int).Value = _taxYear;

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Transmitting to air Employer Failed: Suppressing errors.", exception);
            return false;
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
    }
    public bool PrepIRS(int _employerID, int _taxYear)
    {
        SqlConnection conn = null;
        conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();

        cmd.CommandTimeout = 600;          
        try
        {
            conn.Open();
            cmd.CommandText = "PrepareAcaForIRSStaging";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@year_id", SqlDbType.Int).Value = _taxYear;

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Preparing ACA for IRS Staging stored procedure execution error: Suppressing errors.", exception);
            return false;
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
    }
    public bool Transmit(int _employerID, int _taxYear)
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AIR_Conn"].ConnectionString;
        SqlConnection conn = null;
        conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        cmd.CommandTimeout = 600;          

        try
        {
            conn.Open();
            cmd.CommandText = "etl.spETL_Build";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@year_id", SqlDbType.Int).Value = _taxYear;

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Transmitting to air Employer Failed: Suppressing errors.", exception);
            return false;
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
    }

    public bool BulkUpdatePrintedStatus(
            DataTable data,
            int taxYear)
    {

        using (SqlConnection conn = new SqlConnection(connAcaString))
        {
            using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_printedStatus]"))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@printedStatus", SqlDbType.Structured).Value = data;

                    cmd.Parameters.AddWithValue("@taxYear", taxYear);

                    conn.Open();

                    int tsql = 0;
                    tsql = cmd.ExecuteNonQuery();

                    return true;

                }
                catch (Exception exception)
                {
                    Log.Error("Failed to Bulk Update into [tax_year_1095c_approval] table.", exception);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }


    public bool UpdateEmployeeMeasurementAverageHoursEntityStatus(int employeeId, String modifiedBy)
    {

        using (SqlConnection conn = new SqlConnection(connAcaString))
        {
            using (SqlCommand cmd = new SqlCommand("[dbo].[Update_EmployeeMeasurementAverageHours_EntityStatusToInactive_ForEmployeesOfEmployer]"))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@employerId", employeeId);

                    cmd.Parameters.AddWithValue("@modifiedBy", modifiedBy);

                    conn.Open();

                    int tsql = 0;
                    tsql = cmd.ExecuteNonQuery();

                    return true;

                }
                catch (Exception exception)
                {
                    Log.Error("Failed to update Employee Measurement Average Hours Entity Status set to inactive.", exception);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }

    public List<object> GetPrintedCountPerEmployer(int taxYear)
    {
        List<object> results = new List<object>();
        using (SqlConnection conn = new SqlConnection(connAcaString))
        {
            using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_PrintedCountPerEmployer]"))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@taxYear", taxYear);

                    conn.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        object employerId = null;
                        object employerName = null;
                        object pdfCount = null;

                        employerId = rdr[0] as object ?? default(object);
                        employerName = rdr[1] as object ?? default(object);
                        pdfCount = rdr[2] as object ?? default(object);

                        int _employerId = employerId.checkIntNull();
                        string _employerName = employerName.checkStringNull();
                        int _pdfCount = pdfCount.checkIntNull();

                        results.Add(new { EmployerId = _employerId, EmployerName = _employerName, PdfCount = _pdfCount });
                    }

                    return results;

                }
                catch (Exception exception)
                {
                    Log.Error("Failed to select count from [tax_year_1095c_approval] table.", exception);
                    return new List<object>();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }

    public List<object> GetNotPrintedCountPerEmployer(int taxYear)
    {
        List<object> results = new List<object>();
        using (SqlConnection conn = new SqlConnection(connAcaString))
        {
            using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_NotPrintedCountPerEmployer]"))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@taxYear", taxYear);

                    conn.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        object employerId = null;
                        object employerName = null;
                        object pdfCount = null;

                        employerId = rdr[0] as object ?? default(object);
                        employerName = rdr[1] as object ?? default(object);
                        pdfCount = rdr[2] as object ?? default(object);

                        int _employerId = employerId.checkIntNull();
                        string _employerName = employerName.checkStringNull();
                        int _pdfCount = pdfCount.checkIntNull();

                        results.Add(new { EmployerId = _employerId, EmployerName = _employerName, PdfCount = _pdfCount });
                    }

                    return results;

                }
                catch (Exception exception)
                {
                    Log.Error("Failed to select count from [tax_year_1095c_approval] table.", exception);
                    return new List<object>();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }

    public List<object> GetNotTransmittedCountPerEmployer(int taxYear)
    {
        List<object> results = new List<object>();
        using (SqlConnection conn = new SqlConnection(connAcaString))
        {
            using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_NotTransmitCountPerEmployer]"))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@taxYear", taxYear);

                    conn.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        object employerId = null;
                        object employerName = null;
                        object transmitCount = null;

                        employerId = rdr[0] as object ?? default(object);
                        employerName = rdr[1] as object ?? default(object);
                        transmitCount = rdr[2] as object ?? default(object);

                        int _employerId = employerId.checkIntNull();
                        string _employerName = employerName.checkStringNull();
                        int _transmitCount = transmitCount.checkIntNull();

                        results.Add(new { EmployerId = _employerId, EmployerName = _employerName, TransmitCount = _transmitCount });
                    }

                    return results;

                }
                catch (Exception exception)
                {
                    Log.Error("Failed to select count from [tax_year_1095c_correction] table.", exception);
                    return new List<object>();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }


    public List<ash> get4980HReliefCodes()
    {
        List<ash> results = new List<ash>();
        using (SqlConnection conn = new SqlConnection(connAcaString))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT_affordability_safe_harbor"))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        object ash_code = null;
                        object description = null;
                        object createdby = null;
                        object createddate = null;
                        object modifiedby = null;
                        object modifieddate = null;
                        object entityStatusID = null;
                        object resourceID = null;

                        ash_code = rdr[0] as object ?? default(object);
                        description = rdr[1] as object ?? default(object);
                        createdby = rdr[2] as object ?? default(object);
                        createddate = rdr[3] as object ?? default(object);
                        modifiedby = rdr[4] as object ?? default(object);
                        modifieddate = rdr[5] as object ?? default(object);
                        entityStatusID = rdr[6] as object ?? default(object);
                        resourceID = rdr[7] as object ?? default(object);

                        string _ashCode = ash_code.checkStringNull().ToLower();
                        string _description = description.checkStringNull();
                        string _createdBy = createdby.checkStringNull();
                        DateTime? _createdDate = createddate.checkDateNull();
                        string _modifiedBy = modifiedby.ToString();
                        DateTime? _modifiedDate = modifieddate.checkDateNull();
                        int _entityStatusID = entityStatusID.checkIntNull();
                        Guid _resourceID = (Guid)resourceID;

                        ash a = new ash(_ashCode, _description, _createdBy, (DateTime)_createdDate, _modifiedBy, (DateTime)_modifiedDate, _entityStatusID, _resourceID);
                        results.Add(a);
                    }
                }
                catch (Exception exception)
                {
                    Log.Error("Failed to select count from [tax_year_1095c_correction] table.", exception);
                    results = new List<ash>();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        return results;
    }

    public List<entityStatus> getEntityStatusCodes()
    {
        List<entityStatus> results = new List<entityStatus>();

        using (SqlConnection conn = new SqlConnection(connAcaString))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT_entity_status"))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        object id = null;
                        object description = null;
                        object createdby = null;
                        object createddate = null;
                        object modifiedby = null;
                        object modifieddate = null;

                        id = rdr[0] as object ?? default(object);
                        description = rdr[1] as object ?? default(object);
                        createdby = rdr[2] as object ?? default(object);
                        createddate = rdr[3] as object ?? default(object);
                        modifiedby = rdr[4] as object ?? default(object);
                        modifieddate = rdr[5] as object ?? default(object);

                        int _id = id.checkIntNull();
                        string _description = description.checkStringNull();
                        string _createdBy = createdby.checkStringNull();
                        DateTime? _createdDate = createddate.checkDateNull();
                        string _modifiedBy = modifiedby.ToString();
                        DateTime? _modifiedDate = modifieddate.checkDateNull();

                        entityStatus es = new entityStatus(_id, _description, _createdBy, (DateTime)_createdDate, _modifiedBy, (DateTime)_modifiedDate);
                        results.Add(es);
                    }
                }
                catch (Exception exception)
                {
                    Log.Error("Failed to select count from [entityStatus] table.", exception);
                    results = new List<entityStatus>();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        return results;
    }

    public DataTable ExportCSV(int employerId, int taxYearId)
    {
        DataTable export = new DataTable();

        using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
        {

            using (var cmd = new SqlCommand("spGetFieldForIRS", connString))
            {

                cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = employerId.checkIntDBNull();
                cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = taxYearId.checkIntDBNull();

                using (var da = new SqlDataAdapter(cmd))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(export);

                }

            }

        }

        return export;
    }

    public List<EmployersMeasurementPeriodDetails> getAllEmployersMeasurementPeriodDetails()
    {
        DataTable EmployersMeasurementPeriodDetails = new DataTable();


        using (var connString = new SqlConnection(connAcaString))
        {
            using (var cmd = new SqlCommand("SELECT_employerPYAdminEndDateForAutomateRoll", connString))
            {
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(EmployersMeasurementPeriodDetails);
                }
            }
        }


        return EmployersMeasurementPeriodDetails.DataTableToList<EmployersMeasurementPeriodDetails>();
    }

    /// <summary>
    /// Bulk Update all of the tax year employer tranmission lines.
    /// </summary>
    /// <param name="tyet"></param>
    /// <returns></returns>
    public bool insertTaxYearEmployerTransmission(List<taxYearEmployerTransmission> tyet)
    {
        SqlConnection conn = null;
        bool validTransaction = false;
        NullHelper nh = new NullHelper();
        try
        {
            DataTable dt = tyet.AsEnumerable().ToDataTable();

            dt.Columns.Remove("FormCount");
            dt.Columns.Remove("Is1094Only");

            conn = new SqlConnection(connAcaString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[aca].[dbo].[BULK_INSERT_tax_year_employer_transmission]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@tyet", SqlDbType.Structured).Value = dt;

            int count = cmd.ExecuteNonQuery();
            if (count > 0) { validTransaction = true; }
        }
        catch (Exception exception)
        {
            validTransaction = false;
            Log.Warn("Suppressing errors.", exception);
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


    public void InsertSingleTaxYearEmployerTransmission(int taxYearId, int employerId, string transmissionTypeCd, string uniqueTransmissionId, string submissionId, int transmissionStatusCodeId, string originalReceiptId, string originalUniqueSubmissionId, string user_UserName, string short10941095FileName, string shortManifestFileName)
    {


        using (SqlConnection conn = new SqlConnection(connAcaString))
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("[aca].[dbo].[INSERT_tax_year_employer_1094_transmission]", conn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = taxYearId.checkIntDBNull();
                cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId.checkIntDBNull();
                cmd.Parameters.AddWithValue("@transmissionType", SqlDbType.VarChar).Value = transmissionTypeCd.checkForDBNull();
                cmd.Parameters.AddWithValue("@UTId", SqlDbType.VarChar).Value = uniqueTransmissionId.checkForDBNull();
                cmd.Parameters.AddWithValue("@USId", SqlDbType.VarChar).Value = submissionId.checkForDBNull();
                cmd.Parameters.AddWithValue("@transmissionStatusCodeId", SqlDbType.Int).Value = transmissionStatusCodeId.checkIntDBNull();
                cmd.Parameters.AddWithValue("@ORId", SqlDbType.VarChar).Value = originalReceiptId.checkForDBNull();
                cmd.Parameters.AddWithValue("@OUSId", SqlDbType.VarChar).Value = originalUniqueSubmissionId.checkForDBNull();
                cmd.Parameters.AddWithValue("@userName", SqlDbType.VarChar).Value = user_UserName.checkForDBNull();
                cmd.Parameters.AddWithValue("@bulkFile", SqlDbType.VarChar).Value = short10941095FileName.checkForDBNull();
                cmd.Parameters.AddWithValue("@manifestFile", SqlDbType.VarChar).Value = shortManifestFileName.checkForDBNull();

                cmd.ExecuteNonQuery();

            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
            }
        }





    }

    /// <summary>
    /// Create 7/11/2017 Travis Wells: Updates the receipt ID in the new table, tax_year_employer_transmission.
    /// </summary>
    /// <param name="employerid"></param>
    /// <param name="taxyear"></param>
    /// <param name="uniqueTransmissionid"></param>
    /// <param name="receiptID"></param>
    /// <param name="modBy"></param>
    /// <param name="modOn"></param>
    /// <returns></returns>
    public bool updateTaxYearEmployerTransmissionReceipt(int employerid, int taxyear, string uniqueTransmissionid, string receiptID, string modBy, DateTime modOn)
    {
        SqlConnection conn = null;
        bool validTransaction = false;
        try
        {
            conn = new SqlConnection(connAcaString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[aca].[dbo].[UPDATE_tax_year_employer_transmission_receipt]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.Int).Value = taxyear;
            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = employerid;
            cmd.Parameters.AddWithValue("@unique_transmission_id", SqlDbType.NChar).Value = uniqueTransmissionid;
            cmd.Parameters.AddWithValue("@receiptID", SqlDbType.VarChar).Value = receiptID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.NVarChar).Value = modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = modOn;

            int count = cmd.ExecuteNonQuery();
            if (count > 0) { validTransaction = true; }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
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



    public bool updateTaxYearEmployerTransmissionStatus(int employerid, int taxyear, string receiptID, string modBy, DateTime modOn, int transmissionStatusID, List<int> errantRecords, string ackFileName)
    {
        SqlConnection conn = null;
        bool validTransaction = false;
        NullHelper nh = new NullHelper();
        try
        {
            DataTable dtIntArray = new DataTable();
            dtIntArray.Columns.Add("recordID");
            foreach (int i in errantRecords)
            {
                dtIntArray.Rows.Add(i);
            }


            conn = new SqlConnection(connAcaString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[aca].[dbo].[UPDATE_tax_year_employer_transmission_status]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@tax_year", SqlDbType.Int).Value = taxyear;
            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = employerid;
            cmd.Parameters.AddWithValue("@receiptID", SqlDbType.VarChar).Value = receiptID;
            cmd.Parameters.AddWithValue("@transmissionStatusCode", SqlDbType.Int).Value = transmissionStatusID;
            cmd.Parameters.AddWithValue("@errantRecordIDs", SqlDbType.Structured).Value = dtIntArray;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.NVarChar).Value = modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = modOn;
            cmd.Parameters.AddWithValue("@ackFileName", SqlDbType.VarChar).Value = nh.checkForDBNull(ackFileName);

            int count = cmd.ExecuteNonQuery();
            if (count > 0) { validTransaction = true; }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<TransmissionType> manufactureTransmissionType()
    {
        List<TransmissionType> tempList = new List<TransmissionType>();

        try
        {
            conn = new SqlConnection(connAcaString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_transmission_types", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                string _id = id.checkStringNull();
                string _name = (string)name.checkStringNull();

                TransmissionType tt = new TransmissionType()
                {
                    transmissionID = _id,
                    name = _name
                };
                tempList.Add(tt);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<TransmissionType>();
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
    /// this method takes employerid as parameter and returns 1094c xml file path from air.br.manifest data table.
    /// </summary>
    /// <param name="employerId"></param>
    /// <returns></returns>
    public DataTable GetEmployer1094CFilePath(int employerId)
    {
        DataTable filePath = new DataTable();
        string xmlFilePath = string.Empty;
        using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
        {
            using (var cmd = new SqlCommand("Pull_1094C_XML_File_Path", connString))
            {
                cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = employerId.checkIntDBNull();
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(filePath);
                }
            }
        }


        return filePath;
    }
    public DataTable GetEmployerTransmitReport(DateTime startDate, DateTime endDate)
    {
        DataTable filePath = new DataTable();
        using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
        {
            using (var cmd = new SqlCommand("SELECT_EmployerTransmissionReport", connString))
            {
                cmd.Parameters.AddWithValue("@startDate", SqlDbType.DateTime).Value = startDate.checkDateDBNull();
                cmd.Parameters.AddWithValue("@endDate", SqlDbType.DateTime).Value = endDate.checkDateDBNull();
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(filePath);
                }
            }
        }

        return filePath;
    }
    /// <summary>
    /// this method takes employerid as parameter and returns employer information.
    /// </summary>
    /// <param name="employerId"></param>
    /// <returns></returns>
    public DataTable GetEmployersInfo()
    {
        DataTable EmployerInfo = new DataTable();
        string xmlFilePath = string.Empty;
        using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
        {
            using (var cmd = new SqlCommand("Employer_Export_Info", connString))
            {

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(EmployerInfo);
                }
            }
        }


        return EmployerInfo;
    }
    public DataTable GetTransmissionStatus()
    {
        DataTable TransmissionStatus = new DataTable();
        string xmlFilePath = string.Empty;
        using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
        {
            using (var cmd = new SqlCommand("TransmissionStatusExportReport", connString))
            {

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(TransmissionStatus);
                }
            }
        }


        return TransmissionStatus;
    }
    public Boolean InsertUpdateEmployerConsultant(
        string _name,
        string _title,
        int _phoneNumber,
        int _employerId,
        string _crtBy)
    {

        SqlConnection conn = new SqlConnection(connAcaString);
        SqlCommand cmd = new SqlCommand();
        bool validTransaction = false;

        try
        {
            NullHelper nh = new NullHelper();

            conn.Open();

            cmd.CommandText = "INSERT_UPDATE_CONSULTANT";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = _name;
            cmd.Parameters.AddWithValue("@Title", SqlDbType.VarChar).Value = _title;
            cmd.Parameters.AddWithValue("@PhoneNumber", SqlDbType.Int).Value = _phoneNumber;
            cmd.Parameters.AddWithValue("@EmployerId", SqlDbType.Int).Value = _employerId;
            cmd.Parameters.AddWithValue("@crtby", SqlDbType.VarChar).Value = _crtBy;


            int rows = cmd.ExecuteNonQuery();

            if (rows == 1)
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

        return validTransaction;

    }





    /// <summary>
    /// Distructor to be sure all resources are released
    /// </summary>
    ~employerFactory()
    {
        if (null != conn)
        {
            conn.Dispose();
        }
    }

}