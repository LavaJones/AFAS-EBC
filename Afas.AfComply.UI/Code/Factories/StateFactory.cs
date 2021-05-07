using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;

/// <summary>
/// Summary description for StateFactory
/// </summary>
public class StateFactory
{
    private ILog Log = LogManager.GetLogger(typeof(StateFactory));

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool manufactureStateList()
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_all_states", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object abbr = null;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                abbr = rdr[2] as object ?? default(object);

                int _id = 0;
                if (id != DBNull.Value)
                {
                    _id = (int)id;
                }
                string _abbr = null;
                if (abbr != DBNull.Value)
                {
                    _abbr = abbr.ToString();
                }
                string _name = null;
                if (name != DBNull.Value)
                {
                    _name = name.ToString();
                }
                State newState = new State(_id, _abbr, _name);
                StateController.addState(newState);
            }

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
            if (rdr != null)
            {
                rdr.Close();
            }
        }
    }

    /// <summary>
    /// Distructor to be sure all resources are released
    /// </summary>
    ~StateFactory() 
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