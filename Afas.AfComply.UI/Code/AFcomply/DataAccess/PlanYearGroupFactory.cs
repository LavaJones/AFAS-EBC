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
    public class PlanYearGroupFactory
    {
        private static ILog Log = LogManager.GetLogger(typeof(PlanYearGroupFactory));

        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        private static PlanYearGroup BuildObjectFromReader(SqlDataReader rdr)
        {
            try
            {
                if (rdr.Read())
                {
                    PlanYearGroup results = new PlanYearGroup();
                    int i = 0;
                    results.PlanYearGroupId = (rdr[i++] as object ?? default(object)).checkIntNull();

                    results.ResourceId = (rdr[i++] as object ?? default(object)).checkGuidNull();
                    results.EntityStatus = (EntityStatusEnum) (rdr[i++] as object ?? default(object)).checkIntNull();
                    results.CreatedBy = (rdr[i++] as object ?? default(object)).checkStringNull();
                    i++;      
                    results.ModifiedBy = (rdr[i++] as object ?? default(object)).checkStringNull();
                    results.ModifiedDate = (DateTime)(rdr[i++] as object ?? default(object)).checkDateNull();

                    results.GroupName = (rdr[i++] as object ?? default(object)).checkStringNull();
                    results.Employer_id = (rdr[i++] as object ?? default(object)).checkIntNull();

                    return results;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                Log.Error("Failed to read PlanYearGroupFactory from PlanYearGroup table.", exception);
                return null;
            }
        }

        public static int InsertNewPlanYearGroup(int Employer_id, string GroupName, string CreatedBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[INSERT_PlanYearGroup]"))
                {
                    try
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.NVarChar).Value = CreatedBy.checkForDBNull();
                        cmd.Parameters.AddWithValue("@GroupName", SqlDbType.NVarChar).Value = GroupName.checkForDBNull();
                        cmd.Parameters.AddWithValue("@Employer_id", SqlDbType.Int).Value = Employer_id.checkIntDBNull();

                        cmd.Parameters.Add("@insertedID", SqlDbType.Int);
                        cmd.Parameters["@insertedID"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        return (int)cmd.Parameters["@insertedID"].Value;
                    }
                    catch (Exception exception)
                    {
                        Log.Warn("Suppressing errors.", exception);
                        return 0;
                    }
                    finally
                    {
                        if (conn != null)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

        public static PlanYearGroup GetPlanYearGroupById(int PlanYearGroupId)
        {
            if (PlanYearGroupId <= 0)
            {
                return null;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_PlanYearGroup]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@PlanYearGroupId", PlanYearGroupId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            PlanYearGroup results = BuildObjectFromReader(rdr);

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select from [PlanYearGroup] table.", exception);
                        return null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static List<PlanYearGroup> GetAllPlanYearGroupForEmployerId(int employerId)
        {
            List<PlanYearGroup> results = new List<PlanYearGroup>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_PlanYearGroup_ForEmployer]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@EmployerId", employerId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            PlanYearGroup info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All for Employer from [PlanYearGroup] table.", exception);
                        return new List<PlanYearGroup>();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }            
        }

        public static bool UpdatePlanYearGroup(int PlanYearGroupId, string CreatedBy, string GroupName)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_PlanYearGroup]"))
                {
                    try
                    {                       
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@PlanYearGroupId", SqlDbType.Int).Value = PlanYearGroupId.checkIntDBNull();
                        cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.NVarChar).Value = CreatedBy.checkForDBNull();
                        cmd.Parameters.AddWithValue("@GroupName", SqlDbType.NVarChar).Value = GroupName.checkForDBNull();

                        conn.Open();

                        cmd.ExecuteNonQuery();

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
                    }
                }
            }
        }

        public static bool DeletePlanYearGroup(int PlanYearGroupId, string ModBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_PlanYearGroupStatus]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@EntityStatus", SqlDbType.Int).Value = 2;  
                        cmd.Parameters.AddWithValue("@PlanYearGroupId", SqlDbType.Int).Value = PlanYearGroupId.checkIntDBNull();
                        cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.NVarChar).Value = ModBy.checkForDBNull();

                        conn.Open();

                        cmd.ExecuteNonQuery();

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
                    }
                }
            }
        }
    }
}