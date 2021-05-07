using Afas.AfComply.Domain.POCO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Afas.AfComply.Domain;
using Afc.Core.Domain;

namespace Afas.AfComply.UI.Code.AFcomply.DataAccess
{
    public class UploadFileInfoFactory : Afas.AfComply.UI.Code.AFcomply.DataAccess.IUploadFileInfoFactory
    {
        private static ILog Log = LogManager.GetLogger(typeof(UploadFileInfoFactory));

        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        private static UploadedFileInfo BuildObjectFromReader(SqlDataReader rdr)
        {
            try
            {
                UploadedFileInfo results = new UploadedFileInfo();

                if (rdr.Read())
                {
                    results.UploadedFileInfoId = (rdr[0] as object ?? default(object)).checkIntNull();
                    results.EmployerId = (rdr[1] as object ?? default(object)).checkIntNull();
                    results.UploadedByUser = (rdr[2] as object ?? default(object)).checkStringNull();
                    results.UploadTime = (DateTime)(rdr[3] as object ?? default(object)).checkDateNull();
                    results.UploadSourceDescription = (rdr[4] as object ?? default(object)).checkStringNull();
                    results.UploadTypeDescription = (rdr[5] as object ?? default(object)).checkStringNull();
                    results.FileTypeDescription = (rdr[6] as object ?? default(object)).checkStringNull();
                    results.FileName = (rdr[7] as object ?? default(object)).checkStringNull();
                    results.Processed = (rdr[8] as object ?? default(object)).checkBoolNull();
                    results.ProcessingFailed = (rdr[9] as object ?? default(object)).checkBoolNull();
                    results.ArchiveFileInfoId = (rdr[10] as object).checkNullableInt();

                    results.ResourceId = (rdr[11] as object ?? default(object)).checkGuidNull();
                    results.EntityStatus = (EntityStatusEnum) (rdr[12] as object ?? default(object)).checkIntNull();
                    results.CreatedBy = (rdr[13] as object ?? default(object)).checkStringNull();
                    results.ModifiedBy = (rdr[15] as object ?? default(object)).checkStringNull();
                    results.ModifiedDate = (DateTime)(rdr[16] as object ?? default(object)).checkDateNull();

                    return results;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                Log.Error("Failed to Read File Upload Info From Uploaded File Info table.", exception);
                return null;
            }
        }

        UploadedFileInfo IUploadFileInfoFactory.GetUploadedFileInfoById(int UploadedFileInfoId)
        {
            return UploadFileInfoFactory.GetUploadedFileInfoById(UploadedFileInfoId);
        }

        public static UploadedFileInfo GetUploadedFileInfoById(int UploadedFileInfoId)
        {
            if (UploadedFileInfoId <= 0)
            {
                return null;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_UploadedFileInfo]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UploadedFileInfoId", UploadedFileInfoId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            UploadedFileInfo results = BuildObjectFromReader(rdr);

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

        List<UploadedFileInfo> IUploadFileInfoFactory.GetAllUploadedFilesForEmployerId(int employerId)
        {
            return UploadFileInfoFactory.GetAllUploadedFilesForEmployerId(employerId);
        }

        public static List<UploadedFileInfo> GetAllUploadedFilesForEmployerId(int employerId)
        {
            List<UploadedFileInfo> results = new List<UploadedFileInfo>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_UploadedFileInfo_ForEmployer]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@employerId", employerId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            UploadedFileInfo info = null;
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

        List<UploadedFileInfo> IUploadFileInfoFactory.GetAllUnprocessedUploadedFileInfo()
        {
            return UploadFileInfoFactory.GetAllUnprocessedUploadedFileInfo();
        }

        public static List<UploadedFileInfo> GetAllUnprocessedUploadedFileInfo()
        {
            List<UploadedFileInfo> results = new List<UploadedFileInfo>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_UploadedFileInfo_Unprocessed]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            UploadedFileInfo info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All Unprocessed from Uploaded File Info table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return results;
        }

        List<int> IUploadFileInfoFactory.GetAllEmployerIdsUnprocessedUploadedFileInfo()
        {
            return UploadFileInfoFactory.GetAllEmployerIdsUnprocessedUploadedFileInfo();
        }

        public static List<int> GetAllEmployerIdsUnprocessedUploadedFileInfo()
        {
            List<int> results = new List<int>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_EmployerIds_UploadedFileInfo_Unprocessed]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                results.Add((rdr[0] as object ?? default(object)).checkIntNull());
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All Unprocessed from Uploaded File Info table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return results;
        }

        List<UploadedFileInfo> IUploadFileInfoFactory.GetAllFailedProcessingUploadedFileInfo()
        {
            return UploadFileInfoFactory.GetAllFailedProcessingUploadedFileInfo();
        }

        public static List<UploadedFileInfo> GetAllFailedProcessingUploadedFileInfo()
        {
            List<UploadedFileInfo> results = new List<UploadedFileInfo>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_UploadedFileInfo_Failed]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            UploadedFileInfo info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All Unprocessed from Uploaded File Info table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return results;
        }

        List<UploadedFileInfo> IUploadFileInfoFactory.GetAllUnprocessedUploadedFilesForEmployerId(int employerId)
        {
            return UploadFileInfoFactory.GetAllUnprocessedUploadedFilesForEmployerId(employerId);
        }

        public static List<UploadedFileInfo> GetAllUnprocessedUploadedFilesForEmployerId(int employerId)
        {
            List<UploadedFileInfo> results = new List<UploadedFileInfo>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_UploadedFileInfo_UnprocessedForEmployer]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@employerId", employerId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            UploadedFileInfo info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All Unprocessed for Employer from Uploaded File Info table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return results;
        }

        int IUploadFileInfoFactory.SaveUploadedFileInfo(
            string sourceFileName, int employerId, DateTime uploadTime, string uploadSourceDescription,
            string uploadTypeDescription, string fileTypeDescription, string userId)
        {
            return UploadFileInfoFactory.SaveUploadedFileInfo(
                sourceFileName, employerId, uploadTime, uploadSourceDescription,
                uploadTypeDescription, fileTypeDescription, userId);
        }

        public static int SaveUploadedFileInfo(
            string sourceFileName, int employerId, DateTime uploadTime, string uploadSourceDescription,
            string uploadTypeDescription, string fileTypeDescription, string userId)
        {

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[INSERT_UploadedFileInfo]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fileName", sourceFileName);
                        cmd.Parameters.AddWithValue("@employerId", employerId);
                        cmd.Parameters.AddWithValue("@uploadTime", uploadTime);
                        cmd.Parameters.AddWithValue("@uploadSourceDescription", uploadSourceDescription);
                        cmd.Parameters.AddWithValue("@uploadTypeDescription", uploadTypeDescription);
                        cmd.Parameters.AddWithValue("@fileTypeDescription", fileTypeDescription);
                        cmd.Parameters.AddWithValue("@CreatedBy", userId);

                        cmd.Parameters.Add("@insertedID", SqlDbType.Int);
                        cmd.Parameters["@insertedID"].Direction = ParameterDirection.Output;

                        conn.Open();

                        int result = cmd.ExecuteNonQuery();

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

        bool IUploadFileInfoFactory.UpdateUploadedFileInfoProcessedStatus(int UploadedFileInfoId, string userName, bool Processed, bool Failed = false)
        {
            return UploadFileInfoFactory.UpdateUploadedFileInfoProcessedStatus(UploadedFileInfoId, userName, Processed, Failed);
        }

        public static bool UpdateUploadedFileInfoProcessedStatus(int UploadedFileInfoId, string userName, bool Processed, bool Failed = false)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_UploadedFileInfo_Processed]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UploadedFileInfoId", UploadedFileInfoId);
                        cmd.Parameters.AddWithValue("@modifiedBy", userName);
                        cmd.Parameters.AddWithValue("@processed", Processed);
                        cmd.Parameters.AddWithValue("@failed", Failed);

                        conn.Open();

                        int result = cmd.ExecuteNonQuery();

                        return true;
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

            return false;
        }

        bool IUploadFileInfoFactory.UpdateUploadedFileInfoArchived(int UploadedFileInfoId, string userName, int archiveId)
        {
            return UploadFileInfoFactory.UpdateUploadedFileInfoArchived(UploadedFileInfoId, userName, archiveId);
        }

        public static bool UpdateUploadedFileInfoArchived(int UploadedFileInfoId, string userName, int archiveId)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_UploadedFileInfo_Archived]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UploadedFileInfoId", UploadedFileInfoId);
                        cmd.Parameters.AddWithValue("@modifiedBy", userName);
                        cmd.Parameters.AddWithValue("@archiveFileInfoId", archiveId);

                        conn.Open();

                        int result = cmd.ExecuteNonQuery();

                        return true;

                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to Update File Archive in Uploaded File table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return false;
        }

        bool IUploadFileInfoFactory.DeltereUploadedFileInfo(int UploadedFileInfoId, string userName)
        {
            return UploadFileInfoFactory.DeltereUploadedFileInfo(UploadedFileInfoId, userName);
        }

        public static bool DeltereUploadedFileInfo(int UploadedFileInfoId, string userName)
        {
            return UpdateUploadedFileInfoEntityStatus(UploadedFileInfoId, userName, 3);
        }

        bool IUploadFileInfoFactory.UpdateUploadedFileInfoEntityStatus(int UploadedFileInfoId, string userName, int newStatus)
        {
            return UploadFileInfoFactory.UpdateUploadedFileInfoEntityStatus(UploadedFileInfoId, userName, newStatus);
        }

        public static bool UpdateUploadedFileInfoEntityStatus(int UploadedFileInfoId, string userName, int newStatus)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_UploadedFileInfo_EntityStatus]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UploadedFileInfoId", UploadedFileInfoId);
                        cmd.Parameters.AddWithValue("@modifiedBy", userName);
                        cmd.Parameters.AddWithValue("@EntityStatus", newStatus);

                        conn.Open();

                        int result = cmd.ExecuteNonQuery();

                        return true;
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

            return false;
        }
    }
}