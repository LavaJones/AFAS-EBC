using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;
using Afas.Domain;

/// <summary>
/// Summary description for classificationFactory
/// </summary>
public class classificationFactory
{
    private ILog Log = LogManager.GetLogger(typeof(classificationFactory));
    

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    
    /// <summary>
    /// Creates a new classification with the passed values.
    /// </summary>
    public Boolean ManufactureEmployeeClassification(
            int _employerID, 
            string _desc, 
            string _ashCode, 
            DateTime _modOn, 
            string _modBy, 
            string _history,
            int? _watingPeriodID,
            string _ooc
        )
    {

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        bool validTransaction = false;

        try
        {
            NullHelper nh = new NullHelper();

            conn.Open();

            cmd.CommandText = "INSERT_new_classification";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@desc", SqlDbType.VarChar).Value = _desc;
            cmd.Parameters.AddWithValue("@ashCode", SqlDbType.VarChar).Value = _ashCode.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;
            cmd.Parameters.AddWithValue("@waitingPeriodID", SqlDbType.Int).Value = _watingPeriodID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@ooc", SqlDbType.VarChar).Value = _ooc.checkForDBNull();

            int rows = cmd.ExecuteNonQuery();

            if (rows == 1)
            {
                validTransaction = true;
            }

        }
        catch (Exception exception)
        {
            
            Log.Warn("Suppressing errors.", exception);
            
            validTransaction = false;
        
        }
        finally
        {

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

        return validTransaction;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<classification_aca> manufactureACAstatusList()
    {
        List<classification_aca> tempList = new List<classification_aca>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_all_aca_status";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                object id = null;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                int _id = id.checkIntNull();
                string _name = name.checkStringNull();

                classification_aca tempStatus = new classification_aca(_id, _name);
                tempList.Add(tempStatus);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<classification_aca>();
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

        return tempList;
    }

    /// <summary>
    /// Updates the database record to the passed values.
    /// </summary>
    public Boolean UpdateEmployeeClassification(
            int _classID, 
            string _desc, 
            string _ashCode, 
            DateTime _modOn, 
            string _modBy, 
            string _history,
            int _waitingPeriodID,
            int _entityStatusID,
            string _ooc
        )
    {

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        bool validTransaction = false;

        try
        {

            conn.Open();

            cmd.CommandText = "UPDATE_employee_classification";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = _classID;
            cmd.Parameters.AddWithValue("@description", SqlDbType.VarChar).Value = _desc;
            cmd.Parameters.AddWithValue("@ashCode", SqlDbType.VarChar).Value = _ashCode.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;
            cmd.Parameters.AddWithValue("@waitingPeriodID", SqlDbType.Int).Value = _waitingPeriodID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@entityStatusID", SqlDbType.Int).Value = _entityStatusID;
            cmd.Parameters.AddWithValue("@ooc", SqlDbType.VarChar).Value = _ooc.checkForDBNull();

            int rows = cmd.ExecuteNonQuery();

            if (rows == 1)
            {
                validTransaction = true;
            }

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        
        }
        finally
        {
            
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

        return validTransaction;
    
    }

    /// <summary>
    /// Deletes the classfication referenced by this id.
    /// </summary>
    public bool DeleteEmployeeClassification(int _classID)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        bool validTransaction = false;

        try
        {
            conn.Open();

            cmd.CommandText = "DELETE_classification";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = _classID;

            int rows = cmd.ExecuteNonQuery();

            if (rows == 1)
            {
                validTransaction = true;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
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

        return validTransaction;
    }

    /// <summary>
    /// Returns the current employee classifications from the database for the specific employer.
    /// </summary>
    public List<classification> ManufactureEmployerClassificationList(int _employerID)
    {
        List<classification> tempList = new List<classification>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employer_classifications";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                object id = null;
                object employerID = null;
                object description = null;
                object modOn = null;
                object modBy = null;
                object history = null;
                object affCode = null;
                object waitingID = null;
                object createdBy = null;
                object createdOn = null;
                object entityStatusID = null;
                object ooc = null;

                id = rdr[0] as object ?? default(object);
                employerID = rdr[1] as object ?? default(object);
                description = rdr[2] as object ?? default(object);
                modOn = rdr[3] as object ?? default(object);
                modBy = rdr[4] as object ?? default(object);
                history = rdr[5] as object ?? default(object);
                affCode = rdr[6] as object ?? default(object);
                waitingID = rdr[8] as object ?? default(object);
                createdBy = rdr[9] as object ?? default(object);
                createdOn = rdr[10] as object ?? default(object);
                entityStatusID = rdr[11] as object ?? default(object);
                ooc = rdr[12] as object ?? default(object);

                int _id = id.checkIntNull();
                int _employerID2 = employerID.checkIntNull();
                string _desc = description.checkStringNull();
                DateTime? _modOn = modOn.checkDateNull();
                string _modBy = modBy.checkStringNull();
                string _history = history.checkStringNull();
                string _affCode = affCode.checkStringNull();
                int _waitingPeriodID = waitingID.checkIntNull();
                string _createdBy = createdBy.checkStringNull();
                DateTime? _createdOn = createdOn.checkDateNull();
                int _entityStatusID = entityStatusID.checkIntNull();
                string _ooc = ooc.checkStringNull();

                classification tempClass = new classification(_id, _employerID2, _desc, _affCode, (DateTime)_modOn, _modBy, _history, _waitingPeriodID, (DateTime)_createdOn, _createdBy, _entityStatusID, _ooc);
                tempList.Add(tempClass);
            }

            tempList = tempList.OrderBy(sortVal => sortVal.CLASS_DESC).ToList();
        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            return new List<classification>();
        
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

        return tempList;

    }

    public List<EmployeeClassificationInsurance> getEmployeeClassificationByPlanYearAndEmployer(int employerId, int planYearId)
    {
        DataTable dtEmployeeClassification = new DataTable();

        using (var sqlConnection = new SqlConnection(connString))
        {
            using (var cmd = new SqlCommand("SELECT_employee_classification_by_plan_year_and_employer", sqlConnection))
            {
                cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
                cmd.Parameters.AddWithValue("@planYearId", SqlDbType.Int).Value = planYearId;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtEmployeeClassification);
                }

            }
        }

        return dtEmployeeClassification.DataTableToList<EmployeeClassificationInsurance>();
    }

}