using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for PlanYear_Factory
/// </summary>
public class PlanYear_Factory
{
    private ILog Log = LogManager.GetLogger(typeof(PlanYear_Factory));
    

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<PlanYear> manufacturePlanYear(int _employerID)
    {
        List<PlanYear> tempList = new List<PlanYear>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_plan_years", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object employerID = null;
                object desc = null;
                object sdate = null;
                object edate = null;
                object notes = null;
                object history = null;
                object modOn = null;
                object modBy = null;

                id = rdr[0] as object ?? default(object);
                employerID = rdr[1] as object ?? default(object);
                desc = rdr[2] as object ?? default(object);
                sdate = rdr[3] as object ?? default(object);
                edate = rdr[4] as object ?? default(object);
                notes = rdr[5] as object ?? default(object);
                history = rdr[6] as object ?? default(object);
                modOn = rdr[7] as object ?? default(object);
                modBy = rdr[8] as object ?? default(object);

                int _id = id.checkIntNull();
                int _empID = employerID.checkIntNull();
                string _desc = (string)desc.checkStringNull();
                DateTime? _sdate = sdate.checkDateNull();
                DateTime? _edate = edate.checkDateNull();
                string _notes = (string)notes.checkStringNull();
                string _history = (string)history.checkStringNull();
                DateTime? _modOn = System.DateTime.Now.AddYears(-50);
                string _modBy = (string)modBy.checkStringNull();

                if (modOn != DBNull.Value)
                {
                    _modOn = (DateTime)modOn.checkDateNull();
                }
                else
                {
                    _modOn = null;
                }  

                PlanYear newPY = new PlanYear(_id, _empID, _desc, _sdate, _edate, _notes, _history, _modOn, _modBy);
                
                if(rdr.FieldCount > 10)
                {
                    newPY.Default_Meas_Start= (rdr[10] as object ?? default(object)).checkDateNull();
                    newPY.Default_Meas_End = (rdr[11] as object ?? default(object)).checkDateNull();
                    newPY.Default_Admin_Start = (rdr[12] as object ?? default(object)).checkDateNull();
                    newPY.Default_Admin_End = (rdr[13] as object ?? default(object)).checkDateNull();
                    newPY.Default_Open_Start = (rdr[14] as object ?? default(object)).checkDateNull();
                    newPY.Default_Open_End = (rdr[15] as object ?? default(object)).checkDateNull();
                    newPY.Default_Stability_Start = (rdr[16] as object ?? default(object)).checkDateNull();
                    newPY.Default_Stability_End = (rdr[17] as object ?? default(object)).checkDateNull();
                    newPY.PlanYearGroupId = (rdr[18] as object ?? default(object)).checkIntNull();
                }

                tempList.Add(newPY);
            }

            tempList = tempList.OrderBy(plan => plan.PLAN_YEAR_START).ToList();

            return tempList;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempList;
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

    public int manufactureNewPlanYear(int _employerID, string _name, DateTime _startDate, DateTime _endDate, string _notes, string _history, DateTime _modOn, string _modBy,
            DateTime? default_Meas_Start, DateTime? default_Meas_End, DateTime? default_Admin_Start, DateTime? default_Admin_End, 
            DateTime? default_Open_Start, DateTime? default_Open_End, DateTime? default_Stability_Start, DateTime? default_Stability_End, int PlanYearGroupId)
    {
        int _planyearid = 0;
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_plan_year", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@description", SqlDbType.VarChar).Value = _name;
            cmd.Parameters.AddWithValue("@startDate", SqlDbType.DateTime).Value = _startDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@endDate", SqlDbType.DateTime).Value = _endDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = _notes;
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;

            cmd.Parameters.AddWithValue("@default_Meas_Start", SqlDbType.VarChar).Value = default_Meas_Start.checkDateDBNull();
            cmd.Parameters.AddWithValue("@default_Meas_End", SqlDbType.VarChar).Value = default_Meas_End.checkDateDBNull();

            cmd.Parameters.AddWithValue("@default_Admin_Start", SqlDbType.VarChar).Value = default_Admin_Start.checkDateDBNull();
            cmd.Parameters.AddWithValue("@default_Admin_End", SqlDbType.VarChar).Value = default_Admin_End.checkDateDBNull();

            cmd.Parameters.AddWithValue("@default_Open_Start", SqlDbType.VarChar).Value = default_Open_Start.checkDateDBNull();
            cmd.Parameters.AddWithValue("@default_Open_End", SqlDbType.VarChar).Value = default_Open_End.checkDateDBNull();

            cmd.Parameters.AddWithValue("@default_Stability_Start", SqlDbType.VarChar).Value = default_Stability_Start.checkDateDBNull();
            cmd.Parameters.AddWithValue("@default_Stability_End", SqlDbType.VarChar).Value = default_Stability_End.checkDateDBNull();

            cmd.Parameters.AddWithValue("@PlanYearGroupId", SqlDbType.VarChar).Value = PlanYearGroupId.checkIntDBNull();

            cmd.Parameters.Add("@planyearid", SqlDbType.Int);
            cmd.Parameters["@planyearid"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            _planyearid = (int)cmd.Parameters["@planyearid"].Value;

            return _planyearid;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return _planyearid;
        }
    }

    public bool updatePlanYear(int _planyearID, string _name, DateTime _startDate, DateTime _endDate, string _notes, string _history, DateTime _modOn, string _modBy,
            DateTime? default_Meas_Start, DateTime? default_Meas_End, DateTime? default_Admin_Start, DateTime? default_Admin_End,
            DateTime? default_Open_Start, DateTime? default_Open_End, DateTime? default_Stability_Start, DateTime? default_Stability_End, int PlanYearGroupId)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_plan_year", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@planyearID", SqlDbType.Int).Value = _planyearID;
            cmd.Parameters.AddWithValue("@description", SqlDbType.VarChar).Value = _name;
            cmd.Parameters.AddWithValue("@sDate", SqlDbType.DateTime).Value = _startDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@eDate", SqlDbType.DateTime).Value = _endDate.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = _notes;
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;

            cmd.Parameters.AddWithValue("@default_Meas_Start", SqlDbType.VarChar).Value = default_Meas_Start.checkDateDBNull();
            cmd.Parameters.AddWithValue("@default_Meas_End", SqlDbType.VarChar).Value = default_Meas_End.checkDateDBNull();

            cmd.Parameters.AddWithValue("@default_Admin_Start", SqlDbType.VarChar).Value = default_Admin_Start.checkDateDBNull();
            cmd.Parameters.AddWithValue("@default_Admin_End", SqlDbType.VarChar).Value = default_Admin_End.checkDateDBNull();

            cmd.Parameters.AddWithValue("@default_Open_Start", SqlDbType.VarChar).Value = default_Open_Start.checkDateDBNull();
            cmd.Parameters.AddWithValue("@default_Open_End", SqlDbType.VarChar).Value = default_Open_End.checkDateDBNull();

            cmd.Parameters.AddWithValue("@default_Stability_Start", SqlDbType.VarChar).Value = default_Stability_Start.checkDateDBNull();
            cmd.Parameters.AddWithValue("@default_Stability_End", SqlDbType.VarChar).Value = default_Stability_End.checkDateDBNull();

            cmd.Parameters.AddWithValue("@PlanYearGroupId", SqlDbType.VarChar).Value = PlanYearGroupId.checkIntDBNull();

            cmd.ExecuteReader();

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
    }

    /// <summary>
    /// Distructor to be sure all resources are released
    /// </summary>
    ~PlanYear_Factory() 
    {
        if (null != rdr && false == rdr.IsClosed) 
        {
            rdr.Close();
        }
        if (null != conn) 
        {
            try
            {
                conn.Dispose();
            }
            catch { }
        }        
    }
}