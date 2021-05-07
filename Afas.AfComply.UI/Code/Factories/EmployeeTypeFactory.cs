using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for EmployeeTypeFactory
/// </summary>
public class EmployeeTypeFactory
{
    private ILog Log = LogManager.GetLogger(typeof(EmployeeTypeFactory));
    
    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    
    private SqlConnection conn = null;

    private SqlDataReader rdr = null;

    public bool NewEmployeeType(string _name, int _employerID) 
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_employee_type", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerid", SqlDbType.Int).Value = _employerID.checkForDBNull();
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name.checkForDBNull();

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

    public bool UpdateEmployeeType(string _name, int _typeId)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employeeType", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeId", SqlDbType.Int).Value = _typeId.checkForDBNull();
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name.checkForDBNull();

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

    public bool DeleteEmployeeType(int _typeId)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_employeeType", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeId", SqlDbType.Int).Value = _typeId.checkForDBNull();

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
    public List<EmployeeType> manufactureEmployeeType(int _employerID)
    {
        List<EmployeeType> tempList = new List<EmployeeType>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_all_employee_types", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                int _id = id.checkIntNull();
                string _name = (string)name.checkStringNull();

                EmployeeType et = new EmployeeType(_id, _employerID, _name);
                tempList.Add(et);
            }

            tempList = tempList.OrderBy(sortVal => sortVal.EMPLOYEE_TYPE_NAME).ToList();

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
    /// Distructor to be sure all resources are released
    /// </summary>
    ~EmployeeTypeFactory() 
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