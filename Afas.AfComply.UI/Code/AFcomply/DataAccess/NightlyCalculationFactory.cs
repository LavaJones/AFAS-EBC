using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Code.AFcomply.DataAccess
{
    public class NightlyCalculationFactory
    {
        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        public static DateTime? getLastCalcByEmployerId(int EmployerId)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"
 SELECT TOP 1 [ModifiedDate] FROM [aca].[dbo].[NightlyCalculation] 
 WHERE [ProcessStatus] = 1 AND [EmployerId] = "
                    + EmployerId +
                    " ORDER BY [ModifiedDate] DESC;";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            reader.Read();
                            return reader.GetDateTime(0);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static List<int> getNightlyCalculationEmployerId()
        {
            List<int> employerData = new List<int>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT TOP " + Feature.CalculationBatchLimit
                    + " [EmployerId] FROM [dbo].[NightlyCalculation] WHERE ProcessStatus = 0 AND ProcessFail = 0 GROUP BY [EmployerId];";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            employerData.Add(reader.GetInt32(0));
                        }
                        return employerData;
                    }
                }
            }
        }

        public static void updateNightlyCalculation(int em)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE_NightlyCalculation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = em;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void updateFailNightlyCalculation(int em)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE_FailNightlyCalculation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = em;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}