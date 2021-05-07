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
    public class AleMemberFactory
    {
        private static ILog Log = LogManager.GetLogger(typeof(AleMemberFactory));

        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        private static ale BuildObjectFromReader(SqlDataReader rdr)
        {
            try
            {
                if (rdr.Read())
                {
                    ale results = new ale();
                    int i = 0;
                    results.ALE_ID = (rdr[i++] as object ?? default(object)).checkIntNull();

                    results.ALE_EMPLOYER_ID = (rdr[i++] as object ?? default(object)).checkIntNull();
                    results.ALE_EIN = (rdr[i++] as object ?? default(object)).checkStringNull();
                    results.ALE_NAME = (rdr[i++] as object ?? default(object)).checkStringNull();

                    results.ResourceId = (rdr[i++] as object ?? default(object)).checkGuidNull();
                    results.EntityStatus = (EntityStatusEnum) (rdr[i++] as object ?? default(object)).checkIntNull();
                    results.CreatedBy = (rdr[i++] as object ?? default(object)).checkStringNull();
                    i++;      
                    results.ModifiedBy = (rdr[i++] as object ?? default(object)).checkStringNull();
                    results.ModifiedDate = (DateTime)(rdr[i++] as object ?? default(object)).checkDateNull();
                    
                    return results;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                Log.Error("Failed to read AleMemberFactory from ALE table.", exception);
                return null;
            }
        }

        public static int UpsertAleMember(ale AleMember, string CreatedBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UPSERT_Ale_Member]"))
                {
                    try
                    {
                        conn.Open();

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.AddWithValue("@AleMemberId", SqlDbType.Int).Value = AleMember.ALE_ID.checkIntDBNull();
                        cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = AleMember.ALE_EMPLOYER_ID.checkIntDBNull();
                        cmd.Parameters.AddWithValue("@ein", SqlDbType.NVarChar).Value = AleMember.ALE_EIN.checkForDBNull();
                        cmd.Parameters.AddWithValue("@name", SqlDbType.NVarChar).Value = AleMember.ALE_NAME.checkForDBNull();
                        cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.NVarChar).Value = CreatedBy.checkForDBNull();

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

        public static List<ale> GetAllAleMembersForEmployerId(int employerId)
        {
            List<ale> results = new List<ale>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_ALE_Members_ForEmployer]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@EmployerId", employerId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            ale info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All for Employer from [ALE] table.", exception);
                        return new List<ale>();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }            
        }
        
        public static bool DeleteAleMember(int AleMemberId, string ModBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[DELETE_ALE_Member]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@AleMemberId", SqlDbType.Int).Value = AleMemberId.checkIntDBNull();
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