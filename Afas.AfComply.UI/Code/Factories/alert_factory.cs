using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for alert_factory
/// </summary>
public class alert_factory
{
    private ILog Log = LogManager.GetLogger(typeof(alert_factory));
    

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<alert_type> manufactureAlertTypeList()
    {
        List<alert_type> tempList = new List<alert_type>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_all_alert_types";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;
                object name = null;
                object url = null;
                object table = null;


                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                url = rdr[2] as object ?? default(object);
                table = rdr[3] as object ?? default(object);

                int _id = id.checkIntNull();
                string _name = name.checkStringNull();      
                string _url = url.checkStringNull();         
                string _table = table.checkStringNull();
               
                if (_id > 0)
                {
                    alert_type newAlertType = new alert_type(_id, _name, _url, _table);
                    tempList.Add(newAlertType);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<alert_type>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
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
        }

        return tempList;
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<alert> manufactureEmployerAlertListAll(int _employerID)
    {
        List<alert> tempList = new List<alert>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employer_alerts";
            cmd.Connection =  conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;
                object name = null;
                object typeid = null;
                object employerid = null;
                object tablename = null;
                object count = null;
                object typename = null;
                object imgurl = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                typeid = rdr[2] as object ?? default(object);
                employerid = rdr[3] as object ?? default(object);
                tablename = rdr[4] as object ?? default(object);
                count = rdr[5] as object ?? default(object);
                typename = rdr[6] as object ?? default(object);
                imgurl = rdr[7] as object ?? default(object);

                int _id = 0;
                if (id != DBNull.Value)
                {
                    _id = (int)id;
                }

                string _name = null;
                if (name != DBNull.Value)
                {
                    _name = name.ToString();
                }

                int _typeid = 0;
                if (typeid != DBNull.Value)
                {
                    _typeid = (int)typeid;
                }

                int _employerid2 = 0;
                if (employerid != DBNull.Value)
                {
                    _employerid2 = (int)employerid;
                }

                string _tablename = null;
                if (tablename != DBNull.Value)
                {
                    _tablename = tablename.ToString();
                }

                int _count = 0;
                if (count != DBNull.Value)
                {
                    _count = (int)count;
                   
                }

                string _typename = null;
                if (typename != DBNull.Value)
                {
                    _typename = typename.ToString();
                }

                string _imgurl = null;
                if (imgurl != DBNull.Value)
                {
                    _imgurl = imgurl.ToString();
                }

                    alert newAlert = new alert(_id, _typeid, _name, _employerid2, _tablename, _count, _typename, _imgurl);
                    tempList.Add(newAlert);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<alert>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
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
        }

        return tempList;
    }

    /// <summary>
    /// This deletes an alert from the alert table 
    /// </summary>
    /// <param name="_employerID">The employer to delete the alert from.</param>
    /// <param name="_alertID">The alert to delete</param>
    /// <returns>true if the delete worked, else false</returns>
    public bool deleteAlert(int _employerID, int _alertID)
    {
        bool validTransaction = false;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("DELETE_Alert", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@alertId", SqlDbType.VarChar).Value = _alertID;

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
    public bool deleteEmployerPayrollAlerts(int _employerID, string _modBy, DateTime _modOn)
    {
        bool validTransaction = false;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("DELETE_employer_payroll_alerts", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@empID", SqlDbType.Int).Value = _employerID;
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
    public bool deleteEmployerDemographicAlerts(int _employerID, string _modBy, DateTime _modOn)
    {
        bool validTransaction = false;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("DELETE_employer_demographic_alerts", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@empID", SqlDbType.Int).Value = _employerID;
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
    /// <returns></returns>
    public bool manufactureEmployerAlert(int _employerID, int _alertTypeID)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        bool validTransaction = false;

        try
        {
            conn.Open();

            cmd.CommandText = "INSERT_employer_alert";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@alertTypeID", SqlDbType.Int).Value = _alertTypeID;

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

}