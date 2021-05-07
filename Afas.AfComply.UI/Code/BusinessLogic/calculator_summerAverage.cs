using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// This class is used only for calculate the summer hour average for an employee. 
/// Each time this class is called, all SUMMER HOUR AVG records for the current EMPLOYEE within the EMPLOYER's current summer window will be updated.  
/// </summary>
public static class calculator_summerAverage
{
    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private static ILog Log = LogManager.GetLogger(typeof(calculator_summerAverage));

    public static void calculateEmployeeSummerAverages(int batchID, int _planYearID, Employee emp, employer currentEmployer, gpType _gpType, string _modBy, DateTime _modOn)
    {
        try
        {   
            string _history = "Summer Hour created on " + _modOn.ToString() + " by " + _modBy;
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_BreakInServiceCalculation", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = currentEmployer.EMPLOYER_ID;
            cmd.Parameters.AddWithValue("@planYearId", SqlDbType.Int).Value = _planYearID;
            cmd.Parameters.AddWithValue("@batchId", SqlDbType.Int).Value = batchID;
            cmd.Parameters.AddWithValue("@employeeId", SqlDbType.Int).Value = emp.EMPLOYEE_ID;
            cmd.Parameters.AddWithValue("@gpType", SqlDbType.Int).Value = _gpType.GROSS_PAY_ID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@history", SqlDbType.DateTime).Value = _history;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            conn.Close();
            conn.Dispose();
 
        }
        catch (Exception execption)
        {
            Log.Warn("Suppressing errors.", execption);
        }
        
    }
}

   



    