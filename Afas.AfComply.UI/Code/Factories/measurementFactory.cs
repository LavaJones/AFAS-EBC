using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for measurementFactory
/// </summary>
public class measurementFactory
{
    private ILog Log = LogManager.GetLogger(typeof(measurementFactory));
    
     
    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool manufactureMeasurementTypeList()
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;
        bool transaction = false;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_measurement_types";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object name = null;
  
                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                int _id = 0;
                if (id != DBNull.Value)
                {
                    _id = (int)id;
                }

                string _name = null;
                if (name != DBNull.Value)
                {
                    _name = (string)name;
                }

                measurementType newMT = new measurementType(_id, _name);
                measurementController.addMeasurementType(newMT);
            }

            transaction = true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            transaction = false;
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

        return transaction;
    }

    public int manufactureNewMeasurement(int _employerID, int _planID, int _employeeTypeID, int _measurementTypeID, DateTime _meas_start, DateTime _meas_end, DateTime _admin_start, DateTime _admin_end, DateTime _open_start, DateTime _open_end, DateTime _stab_start, DateTime _stab_end, string _notes, string _modBy, DateTime _modOn, string _history, DateTime? _swStart, DateTime? _swEnd, DateTime? _swStart2, DateTime? _swEnd2)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        int _measurementID = 0;
        try
        {
            conn.Open();
            SqlString nullString = SqlString.Null;
            cmd.CommandText = "INSERT_new_measurement";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planID;
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employeeTypeID;
            cmd.Parameters.AddWithValue("@measurementTypeID", SqlDbType.Int).Value = _measurementTypeID;
            cmd.Parameters.AddWithValue("@meas_start", SqlDbType.DateTime).Value = _meas_start.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@meas_end", SqlDbType.DateTime).Value = _meas_end.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@admin_start", SqlDbType.DateTime).Value = _admin_start.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@admin_end", SqlDbType.DateTime).Value = _admin_end.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@open_start", SqlDbType.DateTime).Value = _open_start.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@open_end", SqlDbType.DateTime).Value = _open_end.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@stab_start", SqlDbType.DateTime).Value = _stab_start.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@stab_end", SqlDbType.DateTime).Value = _stab_end.ToString("yyyy-MM-dd HH:mm:ss");
            if (_notes != null)
            {
                cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = _notes;
            }
            else
            {
                cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = nullString;
            }
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;
            if (_swStart != null)
            {
                DateTime swStart = (DateTime)_swStart;
                cmd.Parameters.AddWithValue("@swStart", SqlDbType.DateTime).Value = swStart.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            { 
                cmd.Parameters.AddWithValue("@swStart", SqlDbType.DateTime).Value = nullString;
            }

            if (_swEnd != null)
            {
                DateTime swEnd = (DateTime)_swEnd;
                cmd.Parameters.AddWithValue("@swEnd", SqlDbType.DateTime).Value = swEnd.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@swEnd", SqlDbType.DateTime).Value = nullString;
            }

            if (_swStart2 != null)
            {
                DateTime swStart2 = (DateTime)_swStart2;
                cmd.Parameters.AddWithValue("@swStart2", SqlDbType.DateTime).Value = swStart2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@swStart2", SqlDbType.DateTime).Value = nullString;
            }

            if (_swEnd2 != null)
            {
                DateTime swEnd2 = (DateTime)_swEnd2;
                cmd.Parameters.AddWithValue("@swEnd2", SqlDbType.DateTime).Value = swEnd2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@swEnd2", SqlDbType.DateTime).Value = nullString;
            }

            cmd.Parameters.Add("@measurementID", SqlDbType.Int);
            cmd.Parameters["@measurementID"].Direction = ParameterDirection.Output;

            cmd.ExecuteReader();

            _measurementID = (int)cmd.Parameters["@measurementID"].Value;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            _measurementID = 0;
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

        return _measurementID;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool manufactureInitialMeasurementList()
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;
        bool transaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd.CommandText = "SELECT_all_initial_measurements";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;
                object name = null;
                object months = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                months = rdr[2] as object ?? default(object);

                int _id = 0;
                if (id != DBNull.Value)
                {
                    _id = (int)id;
                }

                string _name = null;
                if (name != DBNull.Value)
                {
                    _name = (string)name;
                }

                int _months = 0;
                if (months != DBNull.Value)
                {
                    _months = (int)months;
                }

                measurementInitial tempMI = new measurementInitial(_id, _name, _months);
                measurementController.addInitialMeasurement(tempMI);
            }

            transaction = true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            transaction = false;
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

        return transaction;
    }

    public bool updateMeasurement(int _measID, DateTime _meas_start, DateTime _meas_end, DateTime _admin_start, DateTime _admin_end, DateTime _open_start, DateTime _open_end, DateTime _stab_start, DateTime _stab_end, string _notes, string _modBy, DateTime _modOn, string _history, DateTime? _swStart, DateTime? _swEnd, DateTime? _swStart2, DateTime? _swEnd2)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        bool transaction = false;

        try
        {
            SqlString nullString = SqlString.Null;
            conn = new SqlConnection(connString);
            conn.Open();

            cmd.CommandText = "UPDATE_measurement";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@measurementID", SqlDbType.Int).Value = _measID;
            cmd.Parameters.AddWithValue("@measStart", SqlDbType.DateTime).Value = _meas_start.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@measEnd", SqlDbType.DateTime).Value = _meas_end.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@adminStart", SqlDbType.DateTime).Value = _admin_start.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@adminEnd", SqlDbType.DateTime).Value = _admin_end.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@openStart", SqlDbType.DateTime).Value = _open_start.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@openEnd", SqlDbType.DateTime).Value = _open_end.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@stabStart", SqlDbType.DateTime).Value = _stab_start.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@stabEnd", SqlDbType.DateTime).Value = _stab_end.ToString("yyyy-MM-dd HH:mm:ss");
            if (_notes != null)
            {
                cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = _notes;
            }
            else
            {
                cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = nullString;
            }
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;
            if (_swStart != null)
            {
                DateTime swStart = (DateTime)_swStart;
                cmd.Parameters.AddWithValue("@swStart", SqlDbType.DateTime).Value = swStart.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@swStart", SqlDbType.DateTime).Value = nullString;
            }

            if (_swEnd != null)
            {
                DateTime swEnd = (DateTime)_swEnd;
                cmd.Parameters.AddWithValue("@swEnd", SqlDbType.DateTime).Value = swEnd.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@swEnd", SqlDbType.DateTime).Value = nullString;
            }

            if (_swStart2 != null)
            {
                DateTime swStart2 = (DateTime)_swStart2;
                cmd.Parameters.AddWithValue("@swStart2", SqlDbType.DateTime).Value = swStart2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@swStart2", SqlDbType.DateTime).Value = nullString;
            }

            if (_swEnd2 != null)
            {
                DateTime swEnd2 = (DateTime)_swEnd2;
                cmd.Parameters.AddWithValue("@swEnd2", SqlDbType.DateTime).Value = swEnd2.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@swEnd2", SqlDbType.DateTime).Value = nullString;
            }

            int rows = cmd.ExecuteNonQuery();

            if (rows == 1)
            {
                transaction = true;
            }
            else
            {
                transaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            transaction = false;
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

        return transaction;
    }

    public bool updateInitialMeasurement(int _employerID, int _measID)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        bool transaction = false;

        try
        {
            conn.Open();

            cmd.CommandText = "UPDATE_employer_measurement";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@measurementID", SqlDbType.Int).Value = _measID;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.DateTime).Value = _employerID;

            cmd.ExecuteReader();

            transaction = true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            transaction = false;
        }
        finally
        {
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        return transaction;
    }

    /// <summary>
    /// Return all measurement periods for a district. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_planID"></param>
    /// <param name="_employeeTypeID"></param>
    /// <param name="_measTypeID"></param>
    /// <returns></returns>
    public List<Measurement> manufactureMeasurementList(int _employerID)
    {
        List<Measurement> tempList = new List<Measurement>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employer_measurements";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            tempList = createMeasurementObjects(rdr)
                .OrderBy(meas => meas.MEASUREMENT_START)
                .ToList();

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<Measurement>();
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
    /// Return all measurement periods that have expired and have not been completed. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_planID"></param>
    /// <param name="_employeeTypeID"></param>
    /// <param name="_measTypeID"></param>
    /// <returns></returns>
    public List<Measurement> manufactureMeasurementList()
    {
        List<Measurement> tempList = new List<Measurement>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_past_due_measurement_periods";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            tempList = createMeasurementObjects(rdr);

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<Measurement>();
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


    public List<Measurement> manufactureMeasurementList(int _employerID, int _planID, int _employeeTypeID, int _measTypeID)
    {
        List<Measurement> tempList = new List<Measurement>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_specific_measurements";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@planyearID", SqlDbType.Int).Value = _planID;
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employeeTypeID;
            cmd.Parameters.AddWithValue("@measTypeID", SqlDbType.Int).Value = _measTypeID;

            rdr = cmd.ExecuteReader();

            tempList = createMeasurementObjects(rdr);

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<Measurement>();
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
    /// Return all measurement periods for a district. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_planID"></param>
    /// <param name="_employeeTypeID"></param>
    /// <param name="_measTypeID"></param>
    /// <returns></returns>
    public Measurement getMeasurement(int _employerID, int _planyearID, int _employeeTypeID)
    {
        List<Measurement> tempList = new List<Measurement>();
        Measurement tempMeas = null;
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_planyear_measurement";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@planyearID", SqlDbType.Int).Value = _planyearID;
            cmd.Parameters.AddWithValue("@employeeTypeID", SqlDbType.Int).Value = _employeeTypeID;

            rdr = cmd.ExecuteReader();

            tempList = createMeasurementObjects(rdr);
            if (null != tempList && tempList.Count > 0)
            {
                tempMeas = tempList[0];
            }
            else
            { 
                tempMeas = null; 
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempMeas = null;
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

        return tempMeas;
    }



    private List<Measurement> createMeasurementObjects(SqlDataReader rdr)
    {
        List<Measurement> tempList = new List<Measurement>();

        while (rdr.Read())
        {
            int id = 0;
            int employerID = 0;
            int planyearID = 0;
            int employeetypeID = 0;
            int measurementtypeID = 0;
            DateTime meas_start = System.DateTime.Now.AddYears(-50);
            DateTime meas_end = System.DateTime.Now.AddYears(-50);
            DateTime admin_start = System.DateTime.Now.AddYears(-50);
            DateTime admin_end = System.DateTime.Now.AddYears(-50);
            DateTime open_start = System.DateTime.Now.AddYears(-50);
            DateTime open_end = System.DateTime.Now.AddYears(-50);
            DateTime stab_start = System.DateTime.Now.AddYears(-50);
            DateTime stab_end = System.DateTime.Now.AddYears(-50);

            object notes = null;
            object history = null;
            object modOn = null;
            object modBy = null;

            object swStart = null;
            object swEnd = null;
            object swStart2 = null;
            object swEnd2 = null;

            object measComp = null;
            object adminComp = null;
            object measCompBy = null;
            object adminCompBy = null;
            object measCompOn = null;
            object adminCompOn = null;

            id = int.Parse(rdr[0].ToString());
            employerID = int.Parse(rdr[1].ToString());
            planyearID = int.Parse(rdr[2].ToString());
            employeetypeID = int.Parse(rdr[3].ToString());
            measurementtypeID = int.Parse(rdr[4].ToString());
            meas_start = DateTime.Parse(rdr[5].ToString());
            meas_end = DateTime.Parse(rdr[6].ToString());
            admin_start = DateTime.Parse(rdr[7].ToString());
            admin_end = DateTime.Parse(rdr[8].ToString());
            open_start = DateTime.Parse(rdr[9].ToString());
            open_end = DateTime.Parse(rdr[10].ToString());
            stab_start = DateTime.Parse(rdr[11].ToString());
            stab_end = DateTime.Parse(rdr[12].ToString());
            notes = rdr[13] as object ?? default(object);
            history = rdr[14] as object ?? default(object);
            modOn = rdr[15] as object ?? default(object);
            modBy = rdr[16] as object ?? default(object);
            swStart = rdr[17] as object ?? default(object);
            swEnd = rdr[18] as object ?? default(object);
            swStart2 = rdr[19] as object ?? default(object);
            swEnd2 = rdr[20] as object ?? default(object);
            measComp = rdr[21] as object ?? default(object);
            adminComp = rdr[22] as object ?? default(object);
            measCompBy = rdr[23] as object ?? default(object);
            adminCompBy = rdr[24] as object ?? default(object);
            measCompOn = rdr[25] as object ?? default(object);
            adminCompOn = rdr[26] as object ?? default(object);
            
            

            string _notes = notes.checkStringNull();
            string _history = history.checkStringNull();
            DateTime _modOn = (DateTime)modOn.checkDateNull();
            string _modBy = modBy.checkStringNull();
            DateTime? _swStart = swStart.checkDateNull();
            DateTime? _swEnd = swEnd.checkDateNull();
            DateTime? _swStart2 = swStart2.checkDateNull();
            DateTime? _swEnd2 = swEnd2.checkDateNull();
            DateTime? _measCompOn = measCompOn.checkDateNull();
            DateTime? _adminCompOn = adminCompOn.checkDateNull();
            string _measCompBy = measCompBy.checkStringNull();
            string _adminCompBy = adminCompBy.checkStringNull();
            bool _measComp = measComp.checkBoolNull();
            bool _adminComp = adminComp.checkBoolNull();

            Measurement newMeas = new Measurement(id, employerID, planyearID, employeetypeID, measurementtypeID, meas_start, meas_end, admin_start, admin_end, open_start, open_end, stab_start, stab_end, _notes, _history, _modOn, _modBy, _swStart, _swEnd, _swStart2, _swEnd2, _measComp, _adminComp, _measCompBy, _adminCompBy, _measCompOn, _adminCompOn);
            tempList.Add(newMeas);
        }

        return tempList;
    
    }
}