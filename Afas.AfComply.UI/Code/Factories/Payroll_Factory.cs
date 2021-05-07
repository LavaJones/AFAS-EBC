using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using log4net;
using Afas.AfComply.Domain;
using System.Globalization;
using Afas.Application.CSV;
using Afas.Domain;

/// <summary>
/// Summary description for Payroll_Factory
/// </summary>
public class Payroll_Factory
{
    private ILog Log = LogManager.GetLogger(typeof(Payroll_Factory));


    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

    /// <summary>
    /// This function will pass all data from a Payroll File Row into the database.
    /// </summary>
    /// <param name="_batchID"></param>
    /// <param name="_employerID"></param>
    /// <param name="_fname"></param>
    /// <param name="_mname"></param>
    /// <param name="_lname"></param>
    /// <param name="_hours"></param>
    /// <param name="_sdate"></param>
    /// <param name="_edate"></param>
    /// <param name="_ssn"></param>
    /// <param name="_gpDesc"></param>
    /// <param name="_gpID"></param>
    /// <param name="_cdate"></param>
    /// <param name="_modBy"></param>
    /// <param name="_modOn"></param>
    /// <returns></returns>
    public bool importPayroll(int _batchID, int _employerID, string _fname, string _mname, string _lname, string _hours, string _sdate, string _edate, string _ssn, string _gpDesc, string _gpID, string _cdate, string _modBy, DateTime _modOn, string _extEmployeeID)
    {
        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("INSERT_import_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = _mname.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@hours", SqlDbType.VarChar).Value = _hours.checkForDBNull();
            cmd.Parameters.AddWithValue("@sdate", SqlDbType.VarChar).Value = _sdate.checkForDBNull();
            cmd.Parameters.AddWithValue("@edate", SqlDbType.VarChar).Value = _edate.checkForDBNull();
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = _ssn.checkForDBNull();
            cmd.Parameters.AddWithValue("@gpDesc", SqlDbType.VarChar).Value = _gpDesc.checkForDBNull();
            cmd.Parameters.AddWithValue("@gpID", SqlDbType.VarChar).Value = _gpID.checkForDBNull();
            cmd.Parameters.AddWithValue("@cdate", SqlDbType.VarChar).Value = _cdate.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkForDBNull();
            cmd.Parameters.AddWithValue("@employeeExtID", SqlDbType.VarChar).Value = _extEmployeeID.checkForDBNull();

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

    public bool BulkImportPayroll(DataTable payroll)
    {
        SqlConnection conn = null;

        SqlCommand cmd = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("BULK_INSERT_import_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@payroll", SqlDbType.Structured).Value = payroll;

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

    public Boolean UpdateImportPayroll(
            int _rowID,
            int _employeeID,
            int _gpdID,
            decimal _hours,
            DateTime? _sdate,
            DateTime? _edate,
            DateTime? _cdate,
            String _modBy,
            DateTime _modOn
        )
    {

        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("UPDATE_import_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@grossPayDescID", SqlDbType.VarChar).Value = _gpdID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@actHours", SqlDbType.Float).Value = _hours.checkDecimalDBNull();
            cmd.Parameters.AddWithValue("@sdate", SqlDbType.VarChar).Value = _sdate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@edate", SqlDbType.VarChar).Value = _edate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@cdate", SqlDbType.VarChar).Value = _cdate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkDateDBNull();

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {

                this.Log.Warn(
                        String.Format(
                                "UpdateImportPayroll did not return the correct number of rows updated, expected 1, received {0} for Id {1}! Returning false.",
                                tsql,
                                _rowID
                            )
                        );

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

    public Boolean BulkUpdateImportPayroll(DataTable payroll)
    {

        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("BULK_UPDATE_import_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@payroll", SqlDbType.Structured).Value = payroll;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                return true;
            }
            else
            {

                this.Log.Warn(
                        String.Format(
                                "BulkUpdateImportPayroll recieved a {0} response, returning false.",
                                tsql)
                        );

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

    public bool updatePayroll(int _rowID, int _employerID, int _employeeID, decimal _hours, DateTime? _sdate, DateTime? _edate, string _modBy, DateTime _modOn, string _history)
    {
        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("UPDATE_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@actHours", SqlDbType.Float).Value = _hours.checkDecimalDBNull();
            cmd.Parameters.AddWithValue("@sdate", SqlDbType.VarChar).Value = _sdate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@edate", SqlDbType.VarChar).Value = _edate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkDateDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkStringNull();

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
    /// This function is used to migrate a payroll record from one employee to another. 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <param name="_employerID"></param>
    /// <param name="_employeeID"></param>
    /// <param name="_modBy"></param>
    /// <param name="_modOn"></param>
    /// <param name="_history"></param>
    /// <returns></returns>
    public bool migratePayrollSingle(int _rowID, int _employerID, int _employeeID, string _modBy, DateTime _modOn, string _history)
    {
        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("MIGRATE_payroll_single", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkDateDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkStringNull();

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

    public Payroll ManufactureSinglePayroll(int _batchID, int _employeeID, int _employerID, int _gpdID, decimal _hours, DateTime? _sdate, DateTime? _edate, DateTime? _cdate, string _modBy, DateTime _modOn, string _history)
    {
        Payroll tempPayroll = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("INSERT_new_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@gpID", SqlDbType.VarChar).Value = _gpdID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@hours", SqlDbType.Float).Value = _hours.checkDecimalDBNull();
            cmd.Parameters.AddWithValue("@sdate", SqlDbType.VarChar).Value = _sdate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@edate", SqlDbType.VarChar).Value = _edate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@cdate", SqlDbType.VarChar).Value = _cdate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkDateDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                tempPayroll = new Payroll(0, _employerID, _batchID, _employeeID, _hours, _sdate, _edate, _gpdID, _cdate, _modBy, _modOn, "", _history);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempPayroll = null;
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

        return tempPayroll;
    }

    public Payroll ManufacturePayrollSummerAverage(int _batchID, int _employeeID, int _employerID, int _gpdID, decimal _hours, DateTime? _sdate, DateTime? _edate, DateTime? _cdate, string _modBy, DateTime _modOn, string _history, int _planYearID)
    {
        Payroll tempPayroll = null;
        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("INSERT_new_payroll_summer_avg", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@gpID", SqlDbType.VarChar).Value = _gpdID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@hours", SqlDbType.Float).Value = _hours.checkDecimalDBNull();
            cmd.Parameters.AddWithValue("@sdate", SqlDbType.VarChar).Value = _sdate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@edate", SqlDbType.VarChar).Value = _edate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@cdate", SqlDbType.VarChar).Value = _cdate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkDateDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                tempPayroll = new Payroll(0, _employerID, _batchID, _employeeID, _hours, _sdate, _edate, _gpdID, _cdate, _modBy, _modOn, "", _history);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempPayroll = null;
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

        return tempPayroll;
    }

    private List<Payroll> BuildPayrollList(SqlDataReader rdr)
    {
        List<Payroll> tempList = new List<Payroll>();
        while (rdr.Read())
        {
            object rowid = null;
            object employerid = null;
            object batchid = null;
            object employeeid = null;
            object gpID = null;
            object hours = null;
            object sdate = null;
            object edate = null;
            object cdate = null;
            object modBy = null;
            object modOn = null;
            object gpDesc = null;
            object history = null;

            rowid = rdr[0] as object ?? default(object);
            employerid = rdr[1] as object ?? default(object);
            batchid = rdr[2] as object ?? default(object);
            employeeid = rdr[3] as object ?? default(object);
            gpID = rdr[4] as object ?? default(object);
            hours = rdr[5] as object ?? default(object);
            sdate = rdr[6] as object ?? default(object);
            edate = rdr[7] as object ?? default(object);
            cdate = rdr[8] as object ?? default(object);
            modBy = rdr[9] as object ?? default(object);
            modOn = rdr[10] as object ?? default(object);
            gpDesc = rdr[11] as object ?? default(object);
            history = rdr[16] as object ?? default(object);

            int _rowID = rowid.checkIntNull();
            int _employerID = employerid.checkIntNull();
            int _batchID = batchid.checkIntNull();
            int _employeeID = employeeid.checkIntNull();
            decimal _hours = hours.checkDecimalNull();
            DateTime? _sdate = sdate.checkDateNull();
            DateTime? _edate = edate.checkDateNull();
            int _gpID = gpID.checkIntNull();
            DateTime? _cdate = cdate.checkDateNull();
            string _modBy = modBy.checkStringNull();
            DateTime _modOn = (DateTime)modOn;
            string _gpDesc = gpDesc.checkStringNull();
            string _history = history.checkStringNull();

            Payroll pay = new Payroll(_rowID, _employerID, _batchID, _employeeID, _hours, _sdate, _edate, _gpID, _cdate, _modBy, _modOn, _gpDesc, _history);
            tempList.Add(pay);
        }

        return tempList;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Payroll_I> manufactureEmployerPayrollImportList(int _employerID, int? rowLimit = null)
    {
        PIILogger.LogPII("Loading Payroll List for employer with Id:" + _employerID);

        List<Payroll_I> tempList = new List<Payroll_I>();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("SELECT_import_employer_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object rowid = null;
                object employerid = null;
                object batchid = null;
                object employeeid = null;
                object fname = null;
                object mname = null;
                object lname = null;
                object ihours = null;
                object hours = null;
                object isdate = null;
                object sdate = null;
                object iedate = null;
                object edate = null;
                object ssn = null;
                object gpDesc = null;
                object gpExtID = null;
                object gpID = null;
                object icdate = null;
                object cdate = null;
                object modBy = null;
                object modOn = null;
                object employeeExtID = null;

                rowid = rdr[0] as object ?? default(object);
                employerid = rdr[1] as object ?? default(object);
                batchid = rdr[2] as object ?? default(object);
                employeeid = rdr[3] as object ?? default(object);
                fname = rdr[4] as object ?? default(object);
                mname = rdr[5] as object ?? default(object);
                lname = rdr[6] as object ?? default(object);
                ihours = rdr[7] as object ?? default(object);
                hours = rdr[8] as object ?? default(object);
                isdate = rdr[9] as object ?? default(object);
                sdate = rdr[10] as object ?? default(object);
                iedate = rdr[11] as object ?? default(object);
                edate = rdr[12] as object ?? default(object);
                ssn = rdr[13] as object ?? default(object);
                gpDesc = rdr[14] as object ?? default(object);
                gpExtID = rdr[15] as object ?? default(object);
                gpID = rdr[16] as object ?? default(object);
                icdate = rdr[17] as object ?? default(object);
                cdate = rdr[18] as object ?? default(object);
                modBy = rdr[19] as object ?? default(object);
                modOn = rdr[20] as object ?? default(object);
                employeeExtID = rdr[21] as object ?? default(object);


                int _rowID = rowid.checkIntNull();
                int _employerID2 = employerid.checkIntNull();
                int _batchID = batchid.checkIntNull();
                int _employeeID = employeeid.checkIntNull();
                string _fname = fname.checkStringNull();
                string _lname = lname.checkStringNull();
                string _mname = mname.checkStringNull();
                string _ihours = ihours.checkStringNull();
                decimal _hours = hours.checkDecimalNull();
                string _isdate = isdate.checkStringNull();
                DateTime? _sdate = sdate.checkDateNull();
                string _iedate = iedate.checkStringNull();
                DateTime? _edate = edate.checkDateNull();
                string _ssn = ssn.checkStringNull();
                string _gpDesc = gpDesc.checkStringNull();
                string _gpExtID = gpExtID.checkStringNull();
                int _gpID = gpID.checkIntNull();
                string _icdate = icdate.checkStringNull();
                DateTime? _cdate = cdate.checkDateNull();
                string _modBy = modBy.checkStringNull();
                DateTime _modOn = (DateTime)(modOn.checkDateNull() ?? DateTime.Now);
                string _extEmployeeID = employeeExtID.checkStringNull();

                _ssn = AesEncryption.Decrypt(_ssn);

                Payroll_I tempPay = new Payroll_I(_rowID, _employerID2, _employeeID, _batchID, _fname, _mname, _lname, _ihours, _hours, _isdate, _sdate, _iedate, _edate, _ssn, _gpDesc, _gpExtID, _gpID, _icdate, _cdate, _modBy, _modOn, _extEmployeeID);
                tempList.Add(tempPay);

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

    public List<string> getEmployerCheckDates(int _employerID, DateTime _msDate)
    {
        List<string> cdates = new List<string>();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("SELECT_employer_check_dates", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("mStart", SqlDbType.DateTime).Value = _msDate;

            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                object cdate = null;

                cdate = rdr[0] as object ?? default(object);

                DateTime _cdate = (DateTime)cdate.checkDateNull();

                cdates.Add(_cdate.ToShortDateString());
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return new List<string>();
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

        return cdates;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<int> getEmployeeIDforPayrollGrossPayID(int _grossPayID)
    {
        List<int> records = new List<int>();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("SELECT_employee_gross_pay_count", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@grossPayID", SqlDbType.Int).Value = _grossPayID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;

                id = rdr[0] as object ?? default(object);

                int _id = id.checkIntNull();

                records.Add(_id);
            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            records = new List<int>(); ;
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

        return records;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_batchID"></param>
    /// <returns></returns>
    public bool deleteImportedPayrollBatch(int _batchID, string _modBy, DateTime _modOn)
    {
        bool validTransaction = false;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("DELETE_payroll_import", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;

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
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
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
    /// 
    /// </summary>
    /// <param name="_batchID"></param>
    /// <returns></returns>
    public bool deletePayroll(int _rowID, string _modBy, DateTime _modOn)
    {
        bool validTransaction = false;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("DELETE_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;

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
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
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
    /// 
    /// </summary>
    /// <param name="_batchID"></param>
    /// <returns></returns>
    public bool deleteSummerAveragePayroll(int _rowID, string _modBy, DateTime _modOn)
    {
        bool validTransaction = false;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("DELETE_payroll_summer_average", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;

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
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }


    public bool deleteImportedPayrollRow(int _rowID)
    {
        bool validTransaction = false;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("DELETE_payroll_import_row", conn);
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
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }

    public Boolean TransferPayroll(
            int _rowID,
            int _employerID,
            int _batchID,
            int _employeeID,
            int _gpID,
            decimal _hours,
            DateTime _sdate,
            DateTime _edate,
            DateTime _cdate,
            String _modBy,
            DateTime _modOn,
            String _history
        )
    {

        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("TRANSFER_import_new_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@gpID", SqlDbType.Int).Value = _gpID;
            cmd.Parameters.AddWithValue("@hours", SqlDbType.Decimal).Value = _hours;
            cmd.Parameters.AddWithValue("@sdate", SqlDbType.DateTime).Value = _sdate;
            cmd.Parameters.AddWithValue("@edate", SqlDbType.DateTime).Value = _edate;
            cmd.Parameters.AddWithValue("@cdate", SqlDbType.DateTime).Value = _cdate;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 2)
            {
                return true;
            }
            else
            {

                this.Log.Warn(
                        String.Format(
                                "TransferPayroll did not return the correct number of rows updated, expected 2, received {0} for Id {1}! Returning false.",
                                tsql,
                                _rowID
                            )
                        );

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

    public Boolean BulkTransferPayroll(DataTable payroll, string history)
    {
        SqlConnection conn = null;
        SqlCommand cmd = null;

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("BULK_TRANSFER_import_new_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@history", SqlDbType.Text).Value = history;
            cmd.Parameters.AddWithValue("@payroll", SqlDbType.Structured).Value = payroll;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                return true;
            }
            else
            {

                this.Log.Warn(
                        String.Format(
                                "TransferPayroll did not return the correct number of rows, received {0}! Returning false.",
                                tsql
                                )
                        );

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
    public List<Payroll> getEmployerPayroll(int _employerId)
    {
        List<Payroll> tempList = new List<Payroll>();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("SELECT_employer_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = _employerId;

            rdr = cmd.ExecuteReader();

            return BuildPayrollList(rdr);
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Payroll> getEmployeePayroll(int _employeeID2, DateTime _mStart, DateTime _mEnd)
    {
        List<Payroll> tempList = new List<Payroll>();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("SELECT_employee_payroll", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID2;
            cmd.Parameters.AddWithValue("@mStart", SqlDbType.DateTime).Value = _mStart;
            cmd.Parameters.AddWithValue("@mEnd", SqlDbType.DateTime).Value = _mEnd;

            rdr = cmd.ExecuteReader();

            return BuildPayrollList(rdr);
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
    /// This will generate a List of payroll data from a specific Batch ID.
    /// </summary>
    /// <returns></returns>
    public List<Payroll_E> getPayrollbyBatchID(int _batchID, int _employerID)
    {
        List<Payroll_E> tempList = new List<Payroll_E>();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("SELECT_payroll_batch", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.DateTime).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object rowid = null;
                object employerid = null;
                object batchid = null;
                object employeeid = null;
                object gpID = null;
                object hours = null;
                object sdate = null;
                object edate = null;
                object cdate = null;
                object modBy = null;
                object modOn = null;
                object gpDesc = null;
                object history = null;
                object fname = null;
                object mname = null;
                object lname = null;
                object gpExtID = null;
                object empExtID = null;

                rowid = rdr[0] as object ?? default(object);
                employerid = rdr[1] as object ?? default(object);
                batchid = rdr[2] as object ?? default(object);
                employeeid = rdr[3] as object ?? default(object);
                gpID = rdr[4] as object ?? default(object);
                hours = rdr[5] as object ?? default(object);
                sdate = rdr[6] as object ?? default(object);
                edate = rdr[7] as object ?? default(object);
                cdate = rdr[8] as object ?? default(object);
                modBy = rdr[9] as object ?? default(object);
                modOn = rdr[10] as object ?? default(object);
                gpDesc = rdr[11] as object ?? default(object);
                gpExtID = rdr[12] as object ?? default(object);
                empExtID = rdr[13] as object ?? default(object);
                fname = rdr[14] as object ?? default(object);
                lname = rdr[15] as object ?? default(object);
                history = rdr[16] as object ?? default(object);


                int _rowID = rowid.checkIntNull();
                int _employerID2 = employerid.checkIntNull();
                int _batchID2 = batchid.checkIntNull();
                int _employeeID = employeeid.checkIntNull();
                decimal _hours = hours.checkDecimalNull();
                DateTime? _sdate = sdate.checkDateNull();
                DateTime? _edate = edate.checkDateNull();
                int _gpID = gpID.checkIntNull();
                DateTime? _cdate = cdate.checkDateNull();
                string _modBy = modBy.checkStringNull();
                DateTime _modOn = (DateTime)modOn;
                string _gpDesc = gpDesc.checkStringNull();
                string _history = history.checkStringNull();
                string _fname = fname.checkStringNull();
                string _mname = mname.checkStringNull();
                string _lname = lname.checkStringNull();
                string _gpExtID = gpExtID.checkStringNull();
                string _empExtID = empExtID.checkStringNull();

                Payroll_E pay = new Payroll_E(_rowID, _employerID2, _batchID2, _employeeID, _hours, _sdate, _edate, _gpID, _cdate, _modBy, _modOn, _gpDesc, _history, _fname, _mname, _lname, _gpExtID, _empExtID);
                tempList.Add(pay);
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Payroll> getEmployeePayrollSummerAverages(int _employeeID2, int _planYearID)
    {
        List<Payroll> tempList = new List<Payroll>();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("SELECT_employee_payroll_summer_avg", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID2;
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object rowid = null;
                object employerid = null;
                object batchid = null;
                object employeeid = null;
                object gpID = null;
                object hours = null;
                object sdate = null;
                object edate = null;
                object cdate = null;
                object modBy = null;
                object modOn = null;
                object gpDesc = null;
                object history = null;

                rowid = rdr[0] as object ?? default(object);
                employerid = rdr[1] as object ?? default(object);
                batchid = rdr[3] as object ?? default(object);
                employeeid = rdr[4] as object ?? default(object);
                gpID = rdr[5] as object ?? default(object);
                hours = rdr[6] as object ?? default(object);
                sdate = rdr[7] as object ?? default(object);
                edate = rdr[8] as object ?? default(object);
                cdate = rdr[9] as object ?? default(object);
                modBy = rdr[10] as object ?? default(object);
                modOn = rdr[11] as object ?? default(object);
                history = rdr[12] as object ?? default(object);
                gpDesc = rdr[13] as object ?? default(object);



                int _rowID = rowid.checkIntNull();
                int _employerID = employerid.checkIntNull();
                int _batchID = batchid.checkIntNull();
                int _employeeID = employeeid.checkIntNull();
                decimal _hours = hours.checkDecimalNull();
                DateTime? _sdate = sdate.checkDateNull();
                DateTime? _edate = edate.checkDateNull();
                int _gpID = gpID.checkIntNull();
                DateTime? _cdate = cdate.checkDateNull();
                string _modBy = modBy.checkStringNull();
                DateTime _modOn = (DateTime)modOn;
                string _gpDesc = gpDesc.checkStringNull();
                string _history = history.checkStringNull();

                Payroll pay = new Payroll(_rowID, _employerID, _batchID, _employeeID, _hours, _sdate, _edate, _gpID, _cdate, _modBy, _modOn, _gpDesc, _history);
                tempList.Add(pay);
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
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Payroll> getEmployerDuplicatePayroll(int _employerID)
    {
        List<Payroll> tempList = new List<Payroll>();
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("SELECT_employer_payroll_duplicates", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object rowid = null;
                object employerid = null;
                object batchid = null;
                object employeeid = null;
                object gpID = null;
                object hours = null;
                object sdate = null;
                object edate = null;
                object cdate = null;
                object gpDesc = null;

                rowid = rdr[0] as object ?? default(object);
                employerid = rdr[1] as object ?? default(object);
                employeeid = rdr[2] as object ?? default(object);
                gpID = rdr[3] as object ?? default(object);
                hours = rdr[4] as object ?? default(object);
                sdate = rdr[5] as object ?? default(object);
                edate = rdr[6] as object ?? default(object);
                cdate = rdr[7] as object ?? default(object);
                gpDesc = rdr[8] as object ?? default(object);


                int _rowID = rowid.checkIntNull();
                int _employerID2 = employerid.checkIntNull();
                int _batchID = batchid.checkIntNull();
                int _employeeID = employeeid.checkIntNull();
                decimal _hours = hours.checkDecimalNull();
                DateTime? _sdate = sdate.checkDateNull();
                DateTime? _edate = edate.checkDateNull();
                int _gpID = gpID.checkIntNull();
                DateTime? _cdate = cdate.checkDateNull();
                string _gpDesc = gpDesc.checkStringNull();

                Payroll pay = new Payroll(_rowID, _employerID2, _batchID, _employeeID, _hours, _sdate, _edate, _gpID, _cdate, "duplicate", System.DateTime.Now, _gpDesc, "");
                tempList.Add(pay);
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
    /// Generate a text file for a Payroll Alert export. 
    /// </summary>
    /// <param name="_tempList"></param>
    /// <param name="_employer"></param>
    /// <returns></returns>
    public string generateTextFile(List<Payroll_I> _tempList, employer _employer)
    {
        string fileName = null;
        string currDate = errorChecking.convertShortDate(DateTime.Now.ToShortDateString());
        int count = _tempList.Count();
        bool success = false;
        string fullFilePath = null;

        int _employerID2 = _employer.EMPLOYER_ID;
        fileName = Branding.ProductName.ToUpper() + "_EXPORT_" + _employer.EMPLOYER_IMPORT_PAYROLL + "_" + count + "_" + currDate + ".csv";
        fullFilePath = HttpContext.Current.Server.MapPath("..\\ftps\\export\\") + fileName;
        string message1 = "*****Format of Hours *****";
        string message2 = "The Hours must be formatted in a specific manner to re-import this file. See the samples.";
        string message3 = "The last two digits are for values on the RIGHT of the decimal.";
        string exampleOne = "Example 1: 80.50 hours = 008050";
        string exampleTwo = "Example 2: 40 hours = 004000";
        string message4 = "*****How to open in EXCEL*****";
        string message5 = "Save the file to your desktop. Open up Excel and Choose File Open --> Downloaded File.";
        string message6 = "Select Delimited File.";
        string message7 = "Format each cell as TEXT or leading zero's will be lost.";
        string message8 = "If you have any questions please call " + Branding.CompanyShortName + ".";
        string message9 = "----- END INSTRUCTIONS -----";
        try
        {
            using (StreamWriter sw = File.CreateText(fullFilePath))
            {
                sw.WriteLine(message1);
                sw.WriteLine(message2);
                sw.WriteLine(message3);
                sw.WriteLine(exampleOne);
                sw.WriteLine(exampleTwo);
                sw.WriteLine(message4);
                sw.WriteLine(message5);
                sw.WriteLine(message6);
                sw.WriteLine(message7);
                sw.WriteLine(message8);
                sw.WriteLine(message9);

                string AllEmployeeIds = string.Empty;  

                foreach (Payroll_I p in _tempList)
                {
                    AllEmployeeIds += p.PAY_EMPLOYEE_ID + ", ";  

                    string line = p.PAY_F_NAME + ",";
                    line += p.PAY_M_NAME + ",";
                    line += p.PAY_L_NAME + ",";
                    line += p.PAY_I_HOURS + ",";
                    line += p.PAY_I_SDATE + ",";
                    line += p.PAY_I_EDATE + ",";
                    line += p.PAY_SSN + ",";
                    line += p.PAY_GP_DESC + ",";
                    line += p.PAY_GP_EXT_ID + ",";
                    line += p.PAY_I_CDATE + ",";
                    line += p.EMPLOYEE_EXT_ID;
                    sw.WriteLine(line);
                }

                PIILogger.LogPII(string.Format("Export File Generated for Employer Id:[{0}] containing Data for employees with Ids:[{1}]", _employer.EMPLOYER_ID, AllEmployeeIds));
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            fileName = null;
        }

        return fileName;
    }

    /// <summary>
    /// Generate a text file for a Payroll Batch export. 
    /// </summary>
    /// <param name="_tempList"></param>
    /// <param name="_employer"></param>
    /// <returns></returns>
    public string generateTextFile(List<Payroll_E> _tempList, employer _employer)
    {
        string fileName = null;
        string currDate = errorChecking.convertShortDate(DateTime.Now.ToShortDateString());
        int count = _tempList.Count();
        bool success = false;
        string fullFilePath = null;

        int _employerID2 = _employer.EMPLOYER_ID;
        fileName = Branding.ProductName.ToUpper() + "_EXPORT_" + _employer.EMPLOYER_IMPORT_PAYROLL + "_" + count + "_" + currDate + ".csv";
        fullFilePath = HttpContext.Current.Server.MapPath("..\\ftps\\export\\") + fileName;
        string message1 = "*****Format of Hours *****";
        string message2 = "The Hours and Dates must be formatted in a specific manner to re-import this file. See the samples below.";
        string message3 = "Only three columns can be updated through this Export-Import process. They are denoted by the Asterisk in the column name";
        string message11 = "The REMOVE RECORD column is used for deleting records. Enter the word 'delete' into the column for the record to be deleted upon importing.";
        string exampleOne = "Payroll format: 80 1/2 hours = 80.50";
        string exampleTwo = "Date format: mm/dd/yyyy";
        string message4 = "*****How to open in EXCEL*****";
        string message5 = "Save the file to your desktop. Open up Excel and Choose File Open --> Downloaded File.";
        string message6 = "Select Delimited File.";
        string message7 = "Format each cell as TEXT or leading zero's will be lost.";
        string message8 = "If you have any questions please call " + Branding.CompanyShortName + ".";
        string message9 = "----- END INSTRUCTIONS -----";
        string message10 = "ROW ID,Employer ID,Employee ID,First Name,Middle Name,Last Name,Payroll ID,*Hours,*Start Date,*End Date,Check Date,Gross Pay Desc, Gross Pay ID, Remove Record";

        try
        {
            using (StreamWriter sw = File.CreateText(fullFilePath))
            {
                sw.WriteLine(message1);
                sw.WriteLine(message2);
                sw.WriteLine(message3);
                sw.WriteLine(message11);
                sw.WriteLine(exampleOne);
                sw.WriteLine(exampleTwo);
                sw.WriteLine(message4);
                sw.WriteLine(message5);
                sw.WriteLine(message6);
                sw.WriteLine(message7);
                sw.WriteLine(message8);
                sw.WriteLine(message9);
                sw.WriteLine(message10);

                foreach (Payroll_E p in _tempList)
                {
                    string line = p.ROW_ID + ",";
                    line += p.PAY_EMPLOYER_ID + ",";
                    line += p.PAY_EMPLOYEE_ID + ",";
                    line += p.PAY_F_NAME.RemoveCommas() + ",";
                    line += p.PAY_M_NAME.RemoveCommas() + ",";
                    line += p.PAY_L_NAME.RemoveCommas() + ",";
                    line += p.PAY_EMPLOYEE_EXT_ID + ",";
                    line += p.PAY_HOURS + ",";
                    line += ((DateTime)p.PAY_SDATE).ToShortDateString() + ",";
                    line += ((DateTime)p.PAY_EDATE).ToShortDateString() + ",";

                    if (p.PAY_CDATE > new DateTime(1920, 1, 1))
                    {
                        line += ((DateTime)p.PAY_CDATE).ToShortDateString() + ",";
                    }
                    else
                    {
                        line += ",";
                    }

                    line += p.PAY_GP_NAME.RemoveCommas() + ",";
                    line += p.PAY_GP_EXT_ID.RemoveCommas();

                    sw.WriteLine(line);
                }

            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            fileName = null;
        }

        return fileName;
    }


    public Boolean process_PAY_I_files(int _employerID, String _modBy, DateTime _modOn, String _filePath, String _fileName)
    {

        Boolean validFile = false;
        int _batchID = 0;
        String fullFilePath = _filePath + _fileName;

        _batchID = EmployeeController.manufactureBatchID(_employerID, _modOn, _modBy);

        System.IO.StreamReader file = null;

        int totalRecords = 0;
        int failedRecords = 0;

        string failedDateRecords = "";

        try
        {

            file = new System.IO.StreamReader(fullFilePath);
            String line = null;

            if (false == file.ReadLine().IsHeaderRow())
            {
                file.Close();
                file = new System.IO.StreamReader(fullFilePath);
            }

            this.Log.Info("Creating a datatable.");

            DataTable payroll = Payroll_Controller.GetNewBulkPayrollDataTable();

            this.Log.Info(String.Format("Looping through {0}.", fullFilePath));

            while ((line = file.ReadLine()) != null)
            {
                if (line.Trim() == null || line.Trim().Equals(String.Empty))
                    continue;
                DataRow row = payroll.NewRow();

                string _sdate = null;
                string _edate = null;
                string _ssn = null;
                string _cdate = null;

                totalRecords += 1;

                String[] gp = CsvParse.SplitRow(line);
                if (DataValidation.StringArrayContainsOnlyBlanks(gp))
                {
                    this.Log.Info(
                                String.Format("Skipping row for payroll processing in file {0}, all colums where blank.", fullFilePath)
                            );
                    continue;
                }
                if (gp.Count() == 11)
                {

                    row["fname"] = gp[0].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["mname"] = gp[1].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["lname"] = gp[2].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["i_hours"] = gp[3].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    _sdate = gp[4].Trim(new char[] { ' ', '"' });
                    _edate = gp[5].Trim(new char[] { ' ', '"' });
                    _ssn = gp[6].Trim(new char[] { ' ', '"' });
                    row["gp_description"] = gp[7].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    row["gp_ext_id"] = gp[8].Trim(new char[] { ' ', '"' }).TruncateLength(50).checkForDBNull();
                    _cdate = gp[9].Trim(new char[] { ' ', '"' });
                    row["ext_employee_id"] = gp[10].Trim(new char[] { ' ', '"' }).TruncateLength(30).checkForDBNull();

                    if (_sdate == "00000000" || _sdate == "0" || _sdate == "")
                    {
                        _sdate = null;
                    }
                    row["i_sdate"] = _sdate.TruncateLength(8).checkForDBNull();

                    if (_edate == "00000000" || _edate == "0" || _edate == "")
                    {
                        _edate = null;
                    }
                    row["i_edate"] = _edate.TruncateLength(8).checkForDBNull();

                    if (_cdate == "00000000" || _cdate == "0" || _cdate == "")
                    {
                        _cdate = null;
                    }
                    row["i_cdate"] = _cdate.TruncateLength(8).checkForDBNull();

                    if (DataValidation.IsInvalidDateRange(_sdate, _edate))
                    {

                        failedRecords = 1;
                        failedDateRecords += row["fname"] + " " + row["lname"] + ", ";

                    }

                    _ssn = _ssn.ZeroPadSsn();
                    row["ssn"] = AesEncryption.Encrypt(_ssn).TruncateLength(50).checkForDBNull();

                    row["batchid"] = _batchID.checkIntDBNull();

                    row["employerid"] = _employerID.checkIntDBNull();
                    
                    payroll.Rows.Add(row);

                }
                else
                {

                    this.Log.Warn(String.Format("Invalid column count at line {0}, expected 11, found {1}.", totalRecords, gp.Count()));

                    failedRecords = 1;

                    break;

                }

            }

            if (failedRecords == 0)
            {

                if (false == Payroll_Controller.BulkImportPayroll(payroll))
                {

                    this.Log.Warn(String.Format("Payroll_Controller.BulkImportPayroll returned false for batch {0}.", _batchID));

                    failedRecords = 1;

                }

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

        Email email = new Email();

        if (failedRecords == 0)
        {

            if (System.IO.File.Exists(fullFilePath))
            {

                validFile = true;
                emailSubject += "Succesful";
                emailBody += "Total Records: " + totalRecords.ToString() + "<br />";

                new FileArchiverWrapper().ArchiveFile(fullFilePath, _employerID, "Processing Pay_I File");

            }
        }
        else
        {
            validFile = false;
            emailSubject += "Failed";
            if (failedDateRecords != string.Empty)
            {
                Log.Warn("Invalid Dates for " + fullFilePath + ": " + failedDateRecords);
                emailBody += "Invalid Dates for : " + failedDateRecords + "<br />";
            }
            else
            {
                emailBody += "Error on line: " + totalRecords.ToString() + "<br />";
            }
            EmployeeController.DeleteFailedBatch(_batchID);

            email.SendEmail(SystemSettings.EmailNotificationAddress, emailSubject, emailBody, false);
        }

        return validFile;

    }

    public bool process_PAY_E_files(int _employerID, string _modBy, DateTime _modOn, string _filePath, string _fileName)
    {
        bool validFile = false;
        int _batchID = 0;
        string fullFilePath = _filePath + _fileName;

        System.IO.StreamReader file = null;

        int totalRecords = 0;
        int failedRecords = 0;

        try
        {
            file = new System.IO.StreamReader(fullFilePath);
            string line = null;

            if (false == file.ReadLine().IsHeaderRow())
            {
                file.Close();
                file = new System.IO.StreamReader(fullFilePath);
            }

            List<Payroll> tempList = Payroll_Controller.getEmployerPayroll(_employerID);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Trim() == null || line.Trim().Equals(string.Empty))
                    continue;
                int _rowID = 0;
                int _employerID2 = 0;
                int _employeeID = 0;
                decimal _hours = 0;
                DateTime? _sdate = null;
                DateTime? _edate = null;
                string _history = null;
                string change = null;
                Payroll tempPayroll = null;

                totalRecords += 1;

                string[] gp = CsvParse.SplitRow(line);
                if (DataValidation.StringArrayContainsOnlyBlanks(gp))
                {
                    this.Log.Info(
                                String.Format("Skipping row for payroll processing in file {0}, all colums where blank.", fullFilePath)
                            );
                    continue;
                }
                if (gp.Count() == 14)
                {
                    bool success = false;
                    bool validData = true;
                    bool deleteRow = false;
                    _rowID = int.Parse(gp[0].Trim(new char[] { ' ', '"' }));
                    _employerID2 = int.Parse(gp[1].Trim(new char[] { ' ', '"' }));
                    _employeeID = int.Parse(gp[2].Trim(new char[] { ' ', '"' }));
                    _hours = decimal.Parse(gp[7].Trim(new char[] { ' ', '"' }));
                    _sdate = DateTime.Parse(gp[8].Trim(new char[] { ' ', '"' }));
                    _edate = DateTime.Parse(gp[9].Trim(new char[] { ' ', '"' }));
                    change = gp[13].Trim(new char[] { ' ', '"' });

                    tempPayroll = tempList.Where(pay => pay.ROW_ID == _rowID).FirstOrDefault();
                    if (tempPayroll == null) 
                    {
                        continue;
                    }

                    _history = tempPayroll.PAY_HISTORY + Environment.NewLine;

                    _history += "Record Altered on " + _modOn.ToString() + " by " + _modBy + Environment.NewLine;
                    _history += "Old Values: " + Environment.NewLine;
                    _history += "Hours: " + tempPayroll.PAY_HOURS.ToString() + Environment.NewLine;
                    _history += "Payroll Start Date: " + tempPayroll.PAY_SDATE.ToString() + Environment.NewLine;
                    _history += "Payroll End Date: " + tempPayroll.PAY_EDATE.ToString() + Environment.NewLine;

                    if (change == "delete")
                    {
                        deleteRow = true;
                    }

                    if (_sdate > _edate)
                    {
                        validData = false;
                    }

                    if (validData == true)
                    {
                        if (deleteRow == true)
                        {
                            success = Payroll_Controller.deletePayroll(_rowID, _modBy, _modOn);
                        }
                        else
                        {
                            success = Payroll_Controller.updatePayroll(_rowID, _employerID, _employeeID, _hours, _sdate, _edate, _modBy, _modOn, _history);
                        }

                        if (success == false)
                        {
                            Log.Info(String.Format("Pay E File Failed on row: {0}", _rowID));

                            failedRecords += 1;
                            break;
                        }
                    }
                    else
                    {
                        Log.Info(String.Format("Pay E File Data did not validate on row: {0}", _rowID));


                        failedRecords += 1;
                        break;
                    }
                }
                else
                {
                    Log.Error(String.Format("Pay E File had wrong number of columns."));
                    failedRecords = 1;
                    break;
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors in Pay E Files.", exception);
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

        string emailSubject = "File Import: ";
        string emailBody = "File: " + _fileName + "<br />";
        emailBody += "Attempted By: " + _modBy + "<br />";
        emailBody += "Attempted On: " + _modOn.ToString() + " <br />";

        Email em = new Email();

        if (failedRecords == 0)
        {
            Log.Info(String.Format("Pay E File uploaded sucessfully."));

            if (System.IO.File.Exists(fullFilePath))
            {
                validFile = true;
                emailSubject += "Succesful";
                emailBody += "Total Records: " + totalRecords.ToString() + "<br />";

                new FileArchiverWrapper().ArchiveFile(fullFilePath, _employerID, "Processing Pay_E File");

            }
        }
        else
        {
            Log.Info(String.Format("Pay E import had FAiled rows: [{0}]", failedRecords));

            validFile = false;
            emailSubject += "Failed";
            emailBody += "Error on line: " + totalRecords.ToString() + "<br />";

            em.SendEmail(SystemSettings.EmailNotificationAddress, emailSubject, emailBody, false);
        }

        return validFile;
    }

    /// <summary>
    /// 3-3)
    /// A) Get the EMPLOYER ID.
    /// B) Get the EMPLOYER Object.
    /// C) Get the EMPLOYER EMPLOYEE Object list.
    /// D) Get the PAYROLL Object list. 
    /// E) Get the Gross Pay Description Object list.
    /// F) Get the Gross Pay Description Filter list.
    /// </summary>
    public void CrossReferenceData(int _employerID, String _modBy, DateTime _modOn)
    {

        DataTable payroll = Payroll_Controller.GetNewBulkPayrollDataTable();

        List<Payroll_I> tempPayIList = new List<Payroll_I>();
        List<Employee> tempEmpList = new List<Employee>();
        List<gpType> tempGpList = new List<gpType>();
        List<gpType> tempGpFilteredList = new List<gpType>();

        employer tempEmployer = null;

        tempEmployer = employerController.getEmployer(_employerID);

        tempEmpList = EmployeeController.manufactureEmployeeList(_employerID);

        tempPayIList = Payroll_Controller.manufactureEmployerPayrollImportList(_employerID, 100000);           

        tempGpList = gpType_Controller.getEmployeeTypes(_employerID);

        tempGpFilteredList = gpType_Controller.manufactureGrossPayFilter(_employerID);

        foreach (Payroll_I pay in tempPayIList)
        {

            Boolean filter = gpType_Controller.ValidateGpTypeFilter(pay.PAY_GP_EXT_ID, tempGpFilteredList);

            if (filter == false)
            {

                Employee tempEmp = EmployeeController.validateExistingEmployee(tempEmpList, pay.PAY_SSN);

                if (tempEmp != null)
                {

                    pay.PAY_EMPLOYEE_ID = tempEmp.EMPLOYEE_ID;

                    tempEmpList.Add(tempEmp);

                }
                else
                {
                    pay.PAY_EMPLOYEE_ID = 0;
                }

                gpType tempGP = gpType_Controller.validateGpType(_employerID, pay.PAY_GP_EXT_ID, pay.PAY_GP_DESC, tempGpList);
                if (tempGP != null)
                {

                    pay.PAY_GP_ID = tempGP.GROSS_PAY_ID;

                    tempGpList.Add(tempGP);

                }
                else
                {
                    pay.PAY_GP_ID = 0;
                }

                if (pay.PAY_I_HOURS != null)
                {
                    pay.PAY_HOURS = errorChecking.convertHours(pay.PAY_I_HOURS);
                }

                if (pay.PAY_I_SDATE != null)
                {
                    pay.PAY_SDATE = errorChecking.convertDateTime(pay.PAY_I_SDATE);
                }

                if (pay.PAY_I_EDATE != null)
                {
                    pay.PAY_EDATE = errorChecking.convertDateTime(pay.PAY_I_EDATE);
                }

                if (pay.PAY_I_CDATE != null)
                {
                    pay.PAY_CDATE = errorChecking.convertDateTime(pay.PAY_I_CDATE);
                }

                AddRowForPayroll(payroll, pay);

                ////N) UPDATE the current import_employee record.
                
            }
            else
            {

                this.Log.Debug(
                        String.Format("GP filter is true, deleting the payroll record {0}!", pay.ROW_ID)
                    );

                Payroll_Controller.deleteImportedPayrollRow(pay.ROW_ID);

            }

        }

        if (false == Payroll_Controller.BulkUpdateImportPayroll(payroll)) 
        {
            this.Log.Warn("Payroll_Controller.BulkUpdateImportPayroll returned false for inserts.");
        }
        
    }

    /// <summary>
    /// Transfer the Imported Payroll Records to the Live Payroll Table. 
    /// </summary>
    public void TransferPayrollRecords(int _employerID, String _modBy, DateTime _modOn)
    {
        DataTable payroll = Payroll_Controller.GetNewBulkPayrollDataTable();

        List<Payroll_I> tempPayIList = new List<Payroll_I>();
        List<equivalency> tempEqList = new List<equivalency>();

        try
        {

            tempPayIList = Payroll_Controller.manufactureEmployerPayrollImportList(_employerID, 100000);         

            tempEqList = equivalencyController.manufactureEquivalencyList(_employerID);

            foreach (Payroll_I pay in tempPayIList)
            {
                if (pay.PAY_EMPLOYEE_ID == null || pay.PAY_EMPLOYEE_ID == 0 || pay.PAY_SDATE == null || pay.PAY_EDATE == null)
                { 
                    continue; 
                }
                DataRow row = payroll.NewRow();

                int _gpID = 0;
                decimal _hours = 0;
                DateTime _cdate;

                try
                {
                    row["employerid"] = _employerID.checkIntDBNull();
                    row["rowid"] = pay.ROW_ID.checkIntDBNull();
                    row["batchid"] = pay.PAY_BATCH_ID.checkIntDBNull();
                    row["employee_id"] = pay.PAY_EMPLOYEE_ID.checkIntDBNull();
                    row["gp_id"] = pay.PAY_GP_ID.checkIntDBNull();
                    _gpID = pay.PAY_GP_ID;
                    _hours = pay.PAY_HOURS;
                    row["sdate"] = pay.PAY_SDATE.checkDateDBNull();
                    row["edate"] = pay.PAY_EDATE.checkDateDBNull();

                    if (Feature.CheckDateDefaultValueEnabled == true)
                    {

                        if (pay.PAY_CDATE.HasValue == false)
                        {
                            _cdate = new DateTime(1920, 01, 01);
                        }
                        else
                        {
                            _cdate = pay.PAY_CDATE.Value;
                        }

                    }
                    else
                    {
                        _cdate = (DateTime) pay.PAY_CDATE;
                    }
                    row["cdate"] = _cdate.checkDateDBNull();

                    equivalency tempEquiv = equivalencyController.getSingleEquivalency(_gpID, tempEqList);

                    if (tempEquiv != null)
                    {

                        if (tempEquiv.EQUIV_UNIT_NAME == "Pay Period")
                        {
                            if (_hours == 0)
                            {
                                _hours = tempEquiv.EQUIV_CREDIT;
                            }
                        }

                        else if (tempEquiv.EQUIV_UNIT_NAME == "Unit")
                        {
                            _hours = (_hours / tempEquiv.EQUIV_EVERY) * tempEquiv.EQUIV_CREDIT;
                        }

                    }
                    row["hours"] = _hours.checkDecimalDBNull();
                    row["modBy"] = _modBy.checkForDBNull();
                    row["modOn"] = _modOn.checkDateDBNull();

                    payroll.Rows.Add(row);
                }
                catch (Exception exception)
                {
                    this.Log.Warn("Suppressing errors.", exception);
                }

            }
            
            string _history = "Import Values: " + _modBy + " " + _modOn.ToString();

            Payroll_Controller.BulkTransferPayroll(payroll, _history);

        }
        catch (Exception exception)
        {
            this.Log.Warn("Suppressing errors.", exception);
        }

    }

    public void AddRowForPayroll(DataTable payroll, Payroll_I pay) 
    {
        DataRow row = payroll.NewRow();

        row["rowid"] = pay.ROW_ID.checkIntDBNull();
        row["employerid"] = pay.PAY_EMPLOYEE_ID.checkIntDBNull();
        row["batchid"] = pay.PAY_BATCH_ID.checkIntDBNull();
        row["employee_id"] = pay.PAY_EMPLOYEE_ID.checkIntDBNull();
        row["fname"] = pay.PAY_F_NAME.TruncateLength(50).checkForDBNull();
        row["mname"] = pay.PAY_M_NAME.TruncateLength(50).checkForDBNull();
        row["lname"] = pay.PAY_L_NAME.TruncateLength(50).checkForDBNull();
        row["i_hours"] = pay.PAY_I_HOURS.TruncateLength(50).checkForDBNull(); 
	    row["hours"] = pay.PAY_HOURS.checkDecimalDBNull2();
        row["i_sdate"] = pay.PAY_I_SDATE.TruncateLength(8).checkForDBNull(); 
	    row["sdate"] = pay.PAY_SDATE.checkDateDBNull();
        row["i_edate"] = pay.PAY_I_EDATE.TruncateLength(8).checkForDBNull();
        row["edate"] = pay.PAY_EDATE.checkDateDBNull();
        row["ssn"] = pay.PAY_SSN.TruncateLength(50).checkForDBNull();
        row["gp_description"] = pay.PAY_GP_DESC.TruncateLength(50).checkForDBNull();
        row["gp_ext_id"] = pay.PAY_GP_EXT_ID.TruncateLength(50).checkForDBNull();
        row["gp_id"] = pay.PAY_GP_ID.checkIntDBNull();
        row["i_cdate"] = pay.PAY_I_CDATE.TruncateLength(8).checkForDBNull();
        row["cdate"] = pay.PAY_CDATE.checkDateDBNull();
        row["modBy"] = pay.PAY_MOD_BY.TruncateLength(50).checkForDBNull();
        row["modOn"] = pay.PAY_MOD_ON.checkDateDBNull();
        row["ext_employee_id"] = pay.EMPLOYEE_EXT_ID.TruncateLength(30).checkForDBNull(); 

        payroll.Rows.Add(row);
    }
}