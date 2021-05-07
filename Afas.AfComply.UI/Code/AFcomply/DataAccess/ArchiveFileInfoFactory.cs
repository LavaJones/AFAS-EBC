using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using log4net;

using Afas.AfComply.Application;
using Afas.AfComply.Domain;
using Afas.AfComply.Domain.POCO;
using Afas.Domain.POCO;
using Afas.Application.Archiver;
using Afc.Core.Domain;

namespace Afas.AfComply.UI.Code.AFcomply.DataAccess
{
    public class ArchiveFileInfoFactory : IArchiveFileInfoAccess
    {
        private static ILog Log = LogManager.GetLogger(typeof(ArchiveFileInfoFactory));

        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        private static ArchiveFileInfo BuildObjectFromReader(SqlDataReader rdr)
        {
            try
            {
                if (rdr.Read())
                {
                    ArchiveFileInfo results = new ArchiveFileInfo();

                    results.ArchiveFileInfoId = (rdr[0] as object ?? default(object)).checkIntNull();
                    results.EmployerId = (rdr[1] as object ?? default(object)).checkIntNull();
                    results.EmployerGuid = (rdr[2] as object ?? default(object)).checkGuidNull();
                    results.ArchivedTime = (DateTime)(rdr[3] as object ?? default(object)).checkDateNull();
                    results.FileName = (rdr[4] as object ?? default(object)).checkStringNull();
                    results.SourceFilePath = (rdr[5] as object ?? default(object)).checkStringNull();
                    results.ArchiveFilePath = (rdr[6] as object ?? default(object)).checkStringNull();
                    results.ArchiveReason = (rdr[7] as object ?? default(object)).checkStringNull();
                    results.ResourceId = (rdr[8] as object ?? default(object)).checkGuidNull();
                    results.EntityStatus = (EntityStatusEnum) (rdr[9] as object ?? default(object)).checkIntNull();
                    results.CreatedBy = (rdr[10] as object ?? default(object)).checkStringNull();
                    results.ModifiedBy = (rdr[12] as object ?? default(object)).checkStringNull();
                    results.ModifiedDate = (DateTime)(rdr[13] as object ?? default(object)).checkDateNull();

                    return results;
                }
                else 
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                Log.Error("Failed to read File Upload Info from Uploaded File Info table.", exception);
                return null;
            }
        }

        public static ArchiveFileInfo GetArchivedFileInfoById(int ArchiveFileInfoId)
        {
            if (ArchiveFileInfoId <= 0)
            {
                return null;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_ArchiveFileInfo]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ArchiveFileInfoId", ArchiveFileInfoId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            ArchiveFileInfo results = BuildObjectFromReader(rdr);

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select from Uploaded File Info table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return null;
        }

        public static List<ArchiveFileInfo> GetAllArchivedFilesForEmployerId(int employerId)
        {
            List<ArchiveFileInfo> results = new List<ArchiveFileInfo>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_ArchiveFileInfo_ForEmployer]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployerId", employerId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            ArchiveFileInfo info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All for Employer from Uploaded File Info table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return results;
        }
        
        int IArchiveFileInfoAccess.SaveArchiveFileInfo(Guid employerGuid, int employerId, string FileName, string SourceFilePath, string ArchiveFilePath, string userId, string reason)
        {
            return ArchiveFileInfoFactory.InsertArchiveFileInfo(employerGuid, FileName, SourceFilePath, ArchiveFilePath, userId, reason);
        }

        public static int InsertArchiveFileInfo(
            Guid employerGuid, 
            string FileName, 
            string SourceFilePath,
            string ArchiveFilePath,
            string userId, 
            string reason)
        {

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[INSERT_ArchiveFileInfo]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@EmployerGuid", employerGuid);
                        cmd.Parameters.AddWithValue("@FileName", FileName);        
                        cmd.Parameters.AddWithValue("@SourceFilePath", SourceFilePath);
                        cmd.Parameters.AddWithValue("@ArchiveFilePath", ArchiveFilePath);
                        cmd.Parameters.AddWithValue("@ArchiveReason", reason);                          
                        cmd.Parameters.AddWithValue("@CreatedBy", userId);

                        cmd.Parameters.Add("@insertedID", SqlDbType.Int);
                        cmd.Parameters["@insertedID"].Direction = ParameterDirection.Output;

                        conn.Open();

                        cmd.ExecuteNonQuery();
                            
                        return (int)cmd.Parameters["@insertedID"].Value;

                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to Insert File into Import Staging table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return 0;
        }
    }
}