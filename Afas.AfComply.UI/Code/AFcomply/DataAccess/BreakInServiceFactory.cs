using Afas.AfComply.Domain.POCO;
using Afas.AfComply.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Afc.Core.Domain;

namespace Afas.AfComply.UI.Code.AFcomply.DataAccess
{
    public class BreakInServiceFactory
    {
        private ILog Log = LogManager.GetLogger(typeof(BreakInServiceFactory));

        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
        
        private SqlConnection conn = null;
        
        private SqlDataReader rdr = null;

        public List<BreakInService> SelectBreaksInService(int measurement_Id) 
        {
            List<BreakInService> retList = new List<BreakInService>();
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT_BreakInService", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@measurementId", SqlDbType.Int).Value = measurement_Id;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    BreakInService item = new BreakInService();

                    item.BreakInServiceId = (rdr[0] as object ?? default(object)).checkIntNull();
                    item.CreatedBy = (rdr[1] as object ?? default(object)).checkStringNull();
                    item.StartDate = (rdr[3] as object ?? default(object)).checkDateNull() ?? default(DateTime);
                    item.EndDate = (rdr[4] as object ?? default(object)).checkDateNull() ?? default(DateTime);
                    item.EntityStatus = (EntityStatusEnum) (rdr[5] as object ?? default(object)).checkIntNull();
                    item.ResourceId = (rdr[6] as object ?? default(object)).checkGuidNull();
                    item.ModifiedBy = (rdr[7] as object ?? default(object)).checkStringNull();
                    item.ModifiedDate = (rdr[8] as object ?? default(object)).checkDateNull() ?? default(DateTime);

                    retList.Add(item);
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                return new List<BreakInService>();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return retList;
        }

        public List<BreakInService> SelectAllBreaksInServiceForEmployer(int employerId)
        {
            List<BreakInService> retList = new List<BreakInService>();
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT_All_BreakInService_ForEmployer", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    BreakInService item = new BreakInService();

                    item.BreakInServiceId = (rdr[0] as object ?? default(object)).checkIntNull();
                    item.CreatedBy = (rdr[1] as object ?? default(object)).checkStringNull();
                    item.StartDate = (rdr[3] as object ?? default(object)).checkDateNull() ?? default(DateTime);
                    item.EndDate = (rdr[4] as object ?? default(object)).checkDateNull() ?? default(DateTime);
                    item.EntityStatus = (EntityStatusEnum) (rdr[5] as object ?? default(object)).checkIntNull();
                    item.ResourceId = (rdr[6] as object ?? default(object)).checkGuidNull();
                    item.ModifiedBy = (rdr[7] as object ?? default(object)).checkStringNull();
                    item.ModifiedDate = (rdr[8] as object ?? default(object)).checkDateNull() ?? default(DateTime);
                    item.MeasurementId = (rdr[9] as object ?? default(object)).checkIntNull();

                    retList.Add(item);
                }
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                return new List<BreakInService>();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return retList;
        }

        /// <summary>
        /// Inserts a new Break In service
        /// </summary>
        /// <returns>If the insert succeded.</returns>
        public bool InsertNewBreakInService(int measurement_Id, BreakInService toSave)
        {
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT_BreakInService", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.NVarChar).Value = toSave.CreatedBy.checkForDBNull();
                cmd.Parameters.AddWithValue("@CreatedDate", SqlDbType.DateTime2).Value = toSave.CreatedDate.checkDateDBNull();
                cmd.Parameters.AddWithValue("@startDate", SqlDbType.DateTime2).Value = toSave.StartDate.checkDateDBNull();
                cmd.Parameters.AddWithValue("@endDate", SqlDbType.DateTime2).Value = toSave.EndDate.checkDateDBNull();
                cmd.Parameters.AddWithValue("@measurementId", SqlDbType.Int).Value = measurement_Id.checkIntDBNull();
                cmd.Parameters.AddWithValue("@week", SqlDbType.Int).Value = toSave.Weeks.checkIntDBNull();
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
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }

        /// <summary>
        /// Update a Break In service
        /// </summary>
        /// <returns>If the Update succeded.</returns>
        public bool UpdateBreakInService(int measurement_Id, BreakInService toSave)
        {
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE_BreakInService", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BreakInServiceId", SqlDbType.Int).Value = toSave.BreakInServiceId.checkForDBNull();
                cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.NVarChar).Value = toSave.CreatedBy.checkForDBNull();
                cmd.Parameters.AddWithValue("@startDate", SqlDbType.DateTime2).Value = toSave.StartDate.checkDateDBNull();
                cmd.Parameters.AddWithValue("@endDate", SqlDbType.DateTime2).Value = toSave.EndDate.checkDateDBNull();
                cmd.Parameters.AddWithValue("@weeks", SqlDbType.Int).Value = toSave.Weeks.checkIntDBNull();

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
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }


        /// <summary>
        /// Deletes a Break In service
        /// </summary>
        /// <returns>If the Delete succeded.</returns>
        public bool DeleteBreakInService(int Break_Id, string ModBy)
        {
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE_BreakInServiceStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EntityStatus", SqlDbType.Int).Value = 2;  
                cmd.Parameters.AddWithValue("@BreakInServiceId", SqlDbType.Int).Value = Break_Id.checkIntDBNull();
                cmd.Parameters.AddWithValue("@modifiedBy", SqlDbType.NVarChar).Value = ModBy.checkForDBNull();

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
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }


        /// <summary>
        /// Distructor to be sure all resources are released
        /// </summary>
        ~BreakInServiceFactory() 
        {
            if (null != rdr && false == rdr.IsClosed) 
            {
                rdr.Close();
            }
            if (null != conn) 
            {
                conn.Dispose();
            }        
        }
    }
}