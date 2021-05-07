using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;

/// <summary>
/// Summary description for UnitFactory
/// </summary>
public class UnitFactory
{
    private ILog Log = LogManager.GetLogger(typeof(UnitFactory));

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

	public UnitFactory()
	{
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Unit> manufactureUnitList()
    {
        List<Unit> tempList = new List<Unit>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;
        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_equivalency_units";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                int _id = 0;
                if (id != DBNull.Value)
                {
                    _id = (int)id;
                }
                int _empID = 0;

                string _name = null;
                if (name != DBNull.Value)
                {
                    _name = name.ToString();
                }

                Unit newUnit = new Unit(_id, _name);
                tempList.Add(newUnit);
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