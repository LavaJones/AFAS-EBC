using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for termShow
/// </summary>
public class termShow
{
    private ILog Log = LogManager.GetLogger(typeof(termShow));
    

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool manufactureTermList()
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd.CommandText = "SELECT_all_terms";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object name = null;
                object desc = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                desc = rdr[2] as object ?? default(object);

                int _id = 0;
                string _name = null;
                string _desc = null;

                _id = id.checkIntNull();
                _name = (string)name.checkStringNull();
                _desc = (string)desc.checkStringNull();

                term newTerm = new term(_id, _name, _desc);
                termController.addTerms(newTerm);
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
    }
}