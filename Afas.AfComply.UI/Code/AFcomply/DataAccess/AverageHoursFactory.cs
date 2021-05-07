using Afas.AfComply.Domain.POCO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Afas.AfComply.Domain;
using System.Data;
using Afc.Core.Domain;

namespace Afas.AfComply.UI.Code.AFcomply.DataAccess
{
    public class AverageHoursFactory
    {
        private static ILog Log = LogManager.GetLogger(typeof(AverageHoursFactory));

        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        private static AverageHours BuildObjectFromReader(SqlDataReader rdr)
        {
            try
            {
                if (rdr.Read())
                {
                    AverageHours results = new AverageHours();
                    int i = 0;
                    results.EmployeeMeasurementAverageHoursId = (rdr[i++] as object ?? default(object)).checkIntNull();
                    results.EmployeeId = (rdr[i++] as object ?? default(object)).checkIntNull();
                    results.MeasurementId = (rdr[i++] as object ?? default(object)).checkIntNull();
                    results.WeeklyAverageHours = (rdr[i++] as object ?? default(object)).checkDoubleNull();
                    results.MonthlyAverageHours = (rdr[i++] as object ?? default(object)).checkDoubleNull();

                    results.ResourceId = (rdr[i++] as object ?? default(object)).checkGuidNull();
                    results.EntityStatus = (EntityStatusEnum) (rdr[i++] as object ?? default(object)).checkIntNull();
                    results.CreatedBy = (rdr[i++] as object ?? default(object)).checkStringNull();
                    i++;      
                    results.ModifiedBy = (rdr[i++] as object ?? default(object)).checkStringNull();
                    results.ModifiedDate = (DateTime)(rdr[i++] as object ?? default(object)).checkDateNull();

                    results.IsNewHire = (rdr[i++] as object ?? default(object)).checkBoolNull();
                    results.TrendingWeeklyAverageHours = (rdr[i++] as object ?? default(object)).checkDoubleNull();
                    results.TrendingMonthlyAverageHours = (rdr[i++] as object ?? default(object)).checkDoubleNull();
                    results.TotalHours = (rdr[i++] as object ?? default(object)).checkDoubleNull();

                    return results;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                Log.Error("Failed to read AverageHoursFactory from EmployeeMeasurementAverageHours table.", exception);
                return null;
            }
        }

        public static DataTable GetNewAverageHoursDataTable()
        {
            DataTable AverageHours = new DataTable();

            AverageHours.Columns.Add("EmployeeMeasurementAverageHoursId", typeof(int));
            AverageHours.Columns.Add("EmployeeId", typeof(int));
            AverageHours.Columns.Add("MeasurementId", typeof(int));
            AverageHours.Columns.Add("WeeklyAverageHours", typeof(double));
            AverageHours.Columns.Add("MonthlyAverageHours", typeof(double));
            AverageHours.Columns.Add("TrendingWeeklyAverageHours", typeof(double));
            AverageHours.Columns.Add("TrendingMonthlyAverageHours", typeof(double));
            AverageHours.Columns.Add("TotalHours", typeof(double));
            AverageHours.Columns.Add("IsNewHire", typeof(bool));

            return AverageHours;
        }

        public static AverageHours GetAverageHoursById(int AverageHoursId)
        {
            if (AverageHoursId <= 0)
            {
                return null;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_EmployeeMeasurementAverageHours]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", AverageHoursId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            AverageHours results = BuildObjectFromReader(rdr);

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select from [EmployeeMeasurementAverageHours] table.", exception);
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static List<AverageHours> GetAllAverageHoursForEmployeeId(int employeeId)
        {
            List<AverageHours> results = new List<AverageHours>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_EmployeeMeasurementAverageHours_ForEmployee]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            AverageHours info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All for Employee from [EmployeeMeasurementAverageHours] table.", exception);
                        return new List<AverageHours>();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }            
        }

        public static List<AverageHours> GetAllAverageHoursForMeasurementId(int measurementId)
        {

            List<AverageHours> results = new List<AverageHours>();

            using (SqlConnection conn = new SqlConnection(connString))
            {

                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_EmployeeMeasurementAverageHours_ForMeasurement]"))
                {

                    try
                    {

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@measurementId", measurementId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            
                            AverageHours info = null;
                            
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;

                        }

                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All for Employee from [EmployeeMeasurementAverageHours] table.", exception);
                        return new List<AverageHours>();
                    }
                    finally
                    {
                        conn.Close();
                    }

                }

            }    
        
        }

        public static AverageHours GetAverageHoursForEmployeeMeasurement(int employeeId, int measurementId, bool? newhire = null)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_EmployeeMeasurementAverageHours_ByEmployeeMeasurement]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        cmd.Parameters.AddWithValue("@measurementId", measurementId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            List<AverageHours> results = new List<AverageHours>();
                            AverageHours info = null;

                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                if (newhire != null)
                                {           
                                    if (newhire == info.IsNewHire)
                                    {
                                        results.Add(info);
                                    }
                                }
                                else
                                {
                                    results.Add(info);
                                }
                                
                            }

                            if (results.Count > 1) 
                            {
                                Log.Warn(String.Format(
                                    "Found too many Items for Calculation, found [{0}] averages, Employee Id: [{1}] Measurement Id: [{2}]", 
                                    results.Count, 
                                    employeeId, 
                                    measurementId));
                            }

                            return results.FirstOrDefault();
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All for Employee from [EmployeeMeasurementAverageHours] table.", exception);
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static int UpsertAverageHours(
            int employeeId,
            int measurementId,
            double weeklyAverageHours,
            double monthlyAverageHours,
            double trendingWeeklyAverageHours,
            double trendingMonthlyAverageHours,
            double totalHours,
            bool isNewHire,
            string userId)
        {

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UPSERT_AverageHours]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@employeeId", employeeId);
                        cmd.Parameters.AddWithValue("@measurementId", measurementId);
                        cmd.Parameters.AddWithValue("@weeklyAverageHours", weeklyAverageHours);
                        cmd.Parameters.AddWithValue("@monthlyAverageHours", monthlyAverageHours);
                        cmd.Parameters.AddWithValue("@trendingWeeklyAverageHours", trendingWeeklyAverageHours);
                        cmd.Parameters.AddWithValue("@trendingMonthlyAverageHours", trendingMonthlyAverageHours);
                        cmd.Parameters.AddWithValue("@totalHours", totalHours);
                        cmd.Parameters.AddWithValue("@isNewHire", isNewHire);
                        cmd.Parameters.AddWithValue("@CreatedBy", userId);

                        cmd.Parameters.Add("@insertedID", SqlDbType.Int);
                        cmd.Parameters["@insertedID"].Direction = ParameterDirection.Output;

                        conn.Open();

                        cmd.ExecuteNonQuery();

                        return (int)cmd.Parameters["@insertedID"].Value;
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to Upsert into [EmployeeMeasurementAverageHours] table.", exception);
                        return 0;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static bool BulkUpsertAverageHours(
            DataTable data,
            string userId, 
            int employerId)
        {

            int SplitSize = Feature.BulkBatchSize;

            List<DataTable> splitData = data.AsEnumerable()
               .GroupBy(row => (int)data.Rows.IndexOf(row) / SplitSize)
               .Select(g => g.CopyToDataTable())
               .ToList();

            if(false == employerController.UpdateEmployeeMeasurementAverageHoursEntityStatus(employerId, userId))
            {
                Log.Error("Failed to deactivate hours  for employer Id " + employerId);

                throw new ApplicationException("Failed to Inactivate average hours.");
            }

            int batch  = 0;
            foreach (DataTable split in splitData)
            {
                batch++;
                Log.Info("Upserting average hours data, Batchid: " + batch);

                if (false == BulkUpsertAverageHoursProc(split, userId)) 
                {
                    Log.Error("Upsert average hours failed for: " + batch);

                    throw new ApplicationException("Failed to update batch " + batch + " of " + SplitSize + " rows for average hours.");
                }
            }

            return true;
        }

        public static bool BulkUpsertAverageHoursProc(
            DataTable data,
            string userId)
        {

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[BULK_UPSERT_AverageHours]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@averages", SqlDbType.Structured).Value = data;

                        cmd.Parameters.AddWithValue("@CreatedBy", userId);

                        conn.Open();

                        int tsql = 0;
                        tsql = cmd.ExecuteNonQuery();

                        return true;
                        
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to Bulk Upsert into [EmployeeMeasurementAverageHours] table.", exception);
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}