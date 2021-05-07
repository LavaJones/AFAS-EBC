using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for hrStatus_Factory
/// </summary>
public class hrStatus_Factory
{
    private ILog Log = LogManager.GetLogger(typeof(hrStatus_Factory));
    
    
    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<hrStatus> manufactureHRStatusList(int _employerID)
    {
        List<hrStatus> tempList = new List<hrStatus>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_employer_hr_status", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object employerID = null;
                object extID = null;
                object desc = null;
                object active = null;

                id = rdr[0] as object ?? default(object);
                employerID = rdr[1] as object ?? default(object);
                extID = rdr[2] as object ?? default(object);
                desc = rdr[3] as object ?? default(object);
                active = rdr[4] as object ?? default(object);

                int _id = id.checkIntNull();
                int _empID = employerID.checkIntNull();
                string _extID = (string)extID.checkStringNull();
                string _desc = (string)desc.checkStringNull();
                bool _active = active.checkBoolNull();

                hrStatus newHR = new hrStatus(_id, _employerID, _extID, _desc);
                tempList.Add(newHR);
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
    public hrStatus manufactureHrStatus(int _employerID, string _extID, string _name, bool _active)
    {
        hrStatus temp = null;
        int _hrStatusID = 0;
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("INSERT_new_hr_status", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@extID", SqlDbType.VarChar).Value = _extID;
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name;
            cmd.Parameters.AddWithValue("@active", SqlDbType.Bit).Value = _active;
            cmd.Parameters.Add("@hrID", SqlDbType.Int);
            cmd.Parameters["@hrID"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            _hrStatusID = (int)cmd.Parameters["@hrID"].Value;

            temp = new hrStatus(_hrStatusID, _employerID, _extID, _name);
            return temp;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return temp;
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
    public bool updateHrStatus(int _hrStatusID, string _name)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("UPDATE_hr_status", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@hrStatusID", SqlDbType.Int).Value = _hrStatusID;
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name;

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
    ~hrStatus_Factory() 
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
}