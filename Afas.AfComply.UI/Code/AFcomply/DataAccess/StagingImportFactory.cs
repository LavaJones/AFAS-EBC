using Afas.AfComply.Domain.POCO;
using Afas.AfComply.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlTypes;
using Afc.Core.Domain;

namespace Afas.AfComply.UI.Code.AFcomply.DataAccess
{
    public class StagingImportFactory : Afas.AfComply.UI.Code.AFcomply.DataAccess.IStagingImportFactory
    {
        private static ILog Log = LogManager.GetLogger(typeof(StagingImportFactory));

        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        private static StagingImport BuildObjectFromReader(SqlDataReader rdr)
        {
            try
            {
                StagingImport results = new StagingImport();

                rdr.Read();

                results.StagingImportId = (rdr[0] as object ?? default(object)).checkIntNull();
                results.Original = new DataTable();
                results.Original.ReadXml(rdr.GetSqlXml(1).CreateReader());
                results.FileInfo = UploadFileInfoFactory.GetUploadedFileInfoById((rdr[2] as object ?? default(object)).checkIntNull());
                results.Modified = new DataTable();
                results.Modified.ReadXml(rdr.GetSqlXml(3).CreateReader());

                results.ResourceId = (rdr[4] as object ?? default(object)).checkGuidNull();
                results.EntityStatus = (EntityStatusEnum) (rdr[5] as object ?? default(object)).checkIntNull();
                results.CreatedBy = (rdr[6] as object ?? default(object)).checkStringNull();
                results.ModifiedBy = (rdr[8] as object ?? default(object)).checkStringNull();
                results.ModifiedDate = (DateTime)(rdr[9] as object ?? default(object)).checkDateNull();

                return results;
            }
            catch (Exception exception)
            {
                Log.Error("Failed to Insert File Upload Info into Uploaded File Info table.", exception);
                return null;
            }
        }

        int IStagingImportFactory.SaveDataTableToStagingImport(DataTable originalDataTable, DataTable modifiedDataTable, string userId, int uploadedFileInfoId)
        {
            return StagingImportFactory.SaveDataTableToStagingImport(originalDataTable, modifiedDataTable, userId, uploadedFileInfoId);
        }

        public static int SaveDataTableToStagingImport(DataTable originalDataTable, DataTable modifiedDataTable, string userId, int uploadedFileInfoId)
        {
            using (Stream stream = new MemoryStream(),
                    stream2 = new MemoryStream())
            {
                if (originalDataTable != null)
                {
                    originalDataTable.WriteXml(stream, XmlWriteMode.WriteSchema);
                }

                if (modifiedDataTable != null)
                {
                    modifiedDataTable.WriteXml(stream2, XmlWriteMode.WriteSchema);
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[INSERT_StagingImport]"))
                    {
                        try
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@xmlOriginal", new SqlXml(stream));
                            cmd.Parameters.AddWithValue("@uploadedFileInfoId", uploadedFileInfoId);
                            cmd.Parameters.AddWithValue("@xmlModify", new SqlXml(stream2));
                            cmd.Parameters.AddWithValue("@CreatedBy", userId);
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

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
                            stream.Dispose();
                            stream2.Dispose();
                        }
                    }
                }
            }

            return 0;
        }

        StagingImport IStagingImportFactory.GetStagingImportById(int StagingImportId) 
        {
            return StagingImportFactory.GetStagingImportById(StagingImportId);
        }

        public static StagingImport GetStagingImportById(int StagingImportId)
        {
            if (StagingImportId <= 0)
            {
                return null;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_StagingImport]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StagingImportId", StagingImportId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            StagingImport results = BuildObjectFromReader(rdr);

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

        List<StagingImport> IStagingImportFactory.GetAllActiveStagingImport()
        {
            return StagingImportFactory.GetAllActiveStagingImport();
        }

        public static List<StagingImport> GetAllActiveStagingImport()
        {
            List<StagingImport> results = new List<StagingImport>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_ActiveStagingImport]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            StagingImport info = null;
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

        List<StagingImport> IStagingImportFactory.GetAllActiveStagingImportForUpload(int UploadId)
        {
            return StagingImportFactory.GetAllActiveStagingImportForUpload(UploadId);
        }

        public static List<StagingImport> GetAllActiveStagingImportForUpload(int UploadId)
        {
            List<StagingImport> results = new List<StagingImport>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_ActiveStagingImport_ForUpload]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@uploadedFileInfoId", UploadId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            StagingImport info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All active for Upload from Uploaded File Info table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return results;
        }

        List<StagingImport> IStagingImportFactory.GetAllStagingImportForUpload(int UploadId)
        {
            return StagingImportFactory.GetAllStagingImportForUpload(UploadId);
        }

        public static List<StagingImport> GetAllStagingImportForUpload(int UploadId)
        {
            List<StagingImport> results = new List<StagingImport>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[SELECT_StagingImport_ForUpload]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@uploadedFileInfoId", UploadId);

                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            StagingImport info = null;
                            while ((info = BuildObjectFromReader(rdr)) != null)
                            {
                                results.Add(info);
                            }

                            return results;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed during Select All for Upload from Uploaded File Info table.", exception);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return results;
        }

        bool IStagingImportFactory.UpdateModifiedStagingImport(int stagingId, string userName, DataTable modifiedDataTable)
        {
            return StagingImportFactory.UpdateModifiedStagingImport(stagingId, userName, modifiedDataTable);
        }

        public static bool UpdateModifiedStagingImport(int stagingId, string userName, DataTable modifiedDataTable)
        {
            using (Stream stream = new MemoryStream())
            {
                if (modifiedDataTable != null)
                {
                    modifiedDataTable.WriteXml(stream, XmlWriteMode.WriteSchema);
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_StagingImportModified]"))
                    {
                        try
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@stagingId", stagingId);
                            cmd.Parameters.AddWithValue("@modifiedBy", userName);
                            cmd.Parameters.AddWithValue("@xmlModify", new SqlXml(stream));

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
                            stream.Dispose();
                        }
                    }
                }
            }

            return false;
        }

        int IStagingImportFactory.UpdateStagingImportAfterReprocessing(int stagingId, string userName, DataTable ReprocessedDataTable)
        {
            return StagingImportFactory.UpdateStagingImportAfterReprocessing(stagingId, userName, ReprocessedDataTable);
        }

        public static int UpdateStagingImportAfterReprocessing(int stagingId, string userName, DataTable ReprocessedDataTable)
        {
            using (Stream stream = new MemoryStream())
            {
                if (ReprocessedDataTable != null)
                {
                    ReprocessedDataTable.WriteXml(stream, XmlWriteMode.WriteSchema);
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_StagingImport_Reprocessed]"))
                    {
                        try
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@stagingId", stagingId);
                            cmd.Parameters.AddWithValue("@xmlModify", new SqlXml(stream));
                            cmd.Parameters.AddWithValue("@CreatedBy", userName);

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
                            stream.Dispose();
                        }
                    }
                }
            }

            return 0;
        }

        bool IStagingImportFactory.DeleteStagingImport(int stagingId, string userName)
        {
            return StagingImportFactory.DeleteStagingImport(stagingId, userName);
        }

        public static bool DeleteStagingImport(int stagingId, string userName)
        {
            return UpdateStagingImportEntityStatus(stagingId, userName, 3);
        }

        bool IStagingImportFactory.UpdateStagingImportEntityStatus(int stagingId, string userName, int newStatus)
        {
            return StagingImportFactory.UpdateStagingImportEntityStatus(stagingId, userName, newStatus);
        }

        public static bool UpdateStagingImportEntityStatus(int stagingId, string userName, int newStatus)
        {

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UPDATE_StagingImportEntityStatus]"))
                {
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@stagingId", stagingId);
                        cmd.Parameters.AddWithValue("@EntityStatus", newStatus);
                        cmd.Parameters.AddWithValue("@modifiedBy", userName);

                        conn.Open();

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            return true;
                        }
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