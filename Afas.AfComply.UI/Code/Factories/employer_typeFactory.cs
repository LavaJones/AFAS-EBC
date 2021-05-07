using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for employer_typeFactory
/// </summary>
public class employer_typeFactory
{
    private ILog Log = LogManager.GetLogger(typeof(employer_typeFactory));
    

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool manufactureEmployerType()
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_all_employer_types", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                int _id = id.checkIntNull();
                string _name = (string)name.checkStringNull();

                employer_type et = new employer_type(_id, _name);
                employer_typeController.addEmployerType(et);
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
    ~employer_typeFactory() 
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