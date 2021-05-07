using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for equivalencyFactory
/// </summary>
public class equivalencyFactory
{
    private ILog Log = LogManager.GetLogger(typeof(equivalencyFactory));
    
     
    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<equivalency> manufactureEquivalencyList(int _employerID)
    {
        List<equivalency> tempList = new List<equivalency>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_employer_equivalencies", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object employerID = null;
                object name = null;
                object gpID = null;
                object every = null;
                object unitID = null;
                object credit = null;
                object sdate = null;
                object edate = null;
                object notes = null;
                object modBy = null;
                object modOn = null;
                object history = null;
                object active = null;
                object typeID = null;
                object typeName = null;
                object unitName = null;
                object positionID = null;
                object activityID = null;
                object detailID = null;

                id = rdr[0] as object ?? default(object);
                employerID = rdr[1] as object ?? default(object);
                name = rdr[2] as object ?? default(object);
                gpID = rdr[3] as object ?? default(object);
                every = rdr[4] as object ?? default(object);
                unitID = rdr[5] as object ?? default(object);
                credit = rdr[6] as object ?? default(object);
                sdate = rdr[7] as object ?? default(object);
                edate = rdr[8] as object ?? default(object);
                notes = rdr[9] as object ?? default(object);
                modBy = rdr[10] as object ?? default(object);
                modOn = rdr[11] as object ?? default(object);
                history = rdr[12] as object ?? default(object);
                active = rdr[13] as object ?? default(object);
                typeID = rdr[14] as object ?? default(object);
                typeName = rdr[15] as object ?? default(object);
                unitName = rdr[16] as object ?? default(object);
                positionID = rdr[17] as object ?? default(object);
                activityID = rdr[18] as object ?? default(object);
                detailID = rdr[19] as object ?? default(object);

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

                int _gpID = 0;
                if (gpID != DBNull.Value)
                {
                    _gpID = (int)gpID;
                }

                decimal _every = 0;
                if (every != DBNull.Value)
                {
                    _every = (decimal)every;
                }

                int _unitID = 0;
                if (unitID != DBNull.Value)
                {
                    _unitID = (int)unitID;
                }

                decimal _credit = 0;
                if (credit != DBNull.Value)
                {
                    _credit = (decimal)credit;
                }

                DateTime? _sdate = null;
                if (sdate != DBNull.Value)
                {
                    _sdate = (DateTime)sdate;
                }

                DateTime? _edate = null;
                if (edate != DBNull.Value)
                {
                    _edate = (DateTime)edate;
                }

                string _notes = null;
                if (notes != DBNull.Value)
                {
                    _notes = (string)notes;
                }

                string _modBy = null;
                if (modBy != DBNull.Value)
                {
                    _modBy = (string)modBy;
                }

                DateTime _modOn = System.DateTime.Now.AddYears(-20);
                if (modOn != DBNull.Value)
                {
                    _modOn = (DateTime)modOn;
                }

                string _history = null;
                if (history != DBNull.Value)
                {
                    _history = (string)history;
                }

                bool _active = false;
                if (active != DBNull.Value)
                {
                    _active = (bool)active;
                }

                int _typeID = 0;
                if (typeID != DBNull.Value)
                {
                    _typeID = (int)typeID;
                }

                string _unitName = (string)unitName;
                string _typeName = (string)typeName;

                int _positionID = 0;
                if (positionID != DBNull.Value)
                {
                    _positionID = (int)positionID;
                }

                int _activityID = 0;
                if (activityID != DBNull.Value)
                {
                    _activityID = (int)activityID;
                }

                int _detailID = 0;
                if (detailID != DBNull.Value)
                {
                    _detailID = (int)detailID;
                }
                
                equivalency newEquiv = new equivalency(_id, _employerID, _name, _gpID, _every, _unitID, _credit, _sdate, _edate, _notes, _modBy, _modOn, _history, _active, _typeID, _typeName, _unitName, _positionID, _activityID, _detailID);
                tempList.Add(newEquiv);
            }

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

    public equivalency manufactureEquivalency(int _empID, string _name, int _gpID, decimal _every, int _unitID, decimal _credit, DateTime? _sdate, DateTime? _edate, string _notes, string _modBy, DateTime _modOn, string _history, bool _active, int _typeID, string _typeName, string _unitName, int _positionID, int _activityID, int _detailID)
    {
        equivalency tempEquiv = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_equivalency", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _empID.checkForDBNull();
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name.checkForDBNull();
            cmd.Parameters.AddWithValue("@gpID", SqlDbType.Int).Value = _gpID.checkForDBNull();
            cmd.Parameters.AddWithValue("@every", SqlDbType.Decimal).Value = _every.checkForDBNull();
            cmd.Parameters.AddWithValue("@unitID", SqlDbType.Int).Value = _unitID.checkForDBNull();
            cmd.Parameters.AddWithValue("@credit", SqlDbType.Decimal).Value = _credit.checkForDBNull();
            
            if (_sdate != null)
            {
                DateTime sdate = (DateTime)_sdate;
                cmd.Parameters.AddWithValue("@sdate", SqlDbType.DateTime).Value = sdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@sdate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            if (_edate != null)
            {
                DateTime edate = (DateTime)_edate;
                cmd.Parameters.AddWithValue("@edate", SqlDbType.DateTime).Value = edate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@edate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = _notes.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();
            cmd.Parameters.AddWithValue("@active", SqlDbType.Bit).Value = _active.checkBoolNull();
            cmd.Parameters.AddWithValue("@equivTypeID", SqlDbType.Int).Value = _typeID.checkForDBNull();
            cmd.Parameters.AddWithValue("@posID", SqlDbType.Int).Value = _positionID.checkIntNull();
            cmd.Parameters.AddWithValue("@actID", SqlDbType.Int).Value = _activityID.checkIntNull();
            cmd.Parameters.AddWithValue("@detID", SqlDbType.Int).Value = _detailID.checkIntNull();

            cmd.Parameters.Add("@equivalencyID", SqlDbType.Int);
            cmd.Parameters["@equivalencyID"].Direction = ParameterDirection.Output;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                int _id = (int)cmd.Parameters["@equivalencyID"].Value;

                tempEquiv = new equivalency(_id, _empID, _name, _gpID, _every, _unitID, _credit, _sdate, _edate, _notes, _modBy, _modOn, _history, _active, _typeID, _typeName, _unitName, _positionID, _activityID, _detailID);
                return tempEquiv;
            }
            else
            {
                return tempEquiv;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempEquiv;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public equivalency updateEquivalency(int _equivID, int _empID, string _name, string _extID, decimal _every, int _unitID, decimal _credit, DateTime? _sdate, DateTime? _edate, string _notes, string _modBy, DateTime _modOn, string _history, bool _active, int _typeID, string _typeName, string _unitName, int _positionID, int _activityID, int _detailID)
    {
        equivalency tempEquiv = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_equivalency", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@equivalencyID", SqlDbType.Int).Value = _equivID.checkIntNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _empID.checkForDBNull();
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name.checkForDBNull();
            cmd.Parameters.AddWithValue("@gpID", SqlDbType.VarChar).Value = _extID.checkForDBNull();
            cmd.Parameters.AddWithValue("@every", SqlDbType.Decimal).Value = _every.checkForDBNull();
            cmd.Parameters.AddWithValue("@unitID", SqlDbType.Int).Value = _unitID.checkForDBNull();
            cmd.Parameters.AddWithValue("@credit", SqlDbType.Decimal).Value = _credit.checkForDBNull();

            if (_sdate != null)
            {
                DateTime sdate = (DateTime)_sdate;
                cmd.Parameters.AddWithValue("@sdate", SqlDbType.DateTime).Value = sdate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@sdate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            if (_edate != null)
            {
                DateTime edate = (DateTime)_edate;
                cmd.Parameters.AddWithValue("@edate", SqlDbType.DateTime).Value = edate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                cmd.Parameters.AddWithValue("@edate", SqlDbType.DateTime).Value = DBNull.Value;
            }

            cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = _notes.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();
            cmd.Parameters.AddWithValue("@active", SqlDbType.Bit).Value = _active.checkBoolNull();
            cmd.Parameters.AddWithValue("@equivTypeID", SqlDbType.Int).Value = _typeID.checkForDBNull();
            cmd.Parameters.AddWithValue("@posID", SqlDbType.Int).Value = _positionID.checkIntNull();
            cmd.Parameters.AddWithValue("@actID", SqlDbType.Int).Value = _activityID.checkIntNull();
            cmd.Parameters.AddWithValue("@detID", SqlDbType.Int).Value = _detailID.checkIntNull();

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                ////If Database insert returns number other than 0, add the new equivalency object.
                return tempEquiv;
            }
            else
            {
                return tempEquiv;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempEquiv;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }


    public bool deleteEquivalency(int _equivID)
    {
        bool succesfulTransaction = true;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_equivalency", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@equivID", SqlDbType.Int).Value = _equivID.checkIntNull();

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                succesfulTransaction = true;
            }
            else
            {
                succesfulTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            succesfulTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return succesfulTransaction;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool manufacturePositionList()
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_positions", conn);
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

                position newPosition = new position(_id, _name);

                equivalencyController.addPosition(newPosition);
            }

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
    /// 
    /// </summary>
    /// <returns></returns>
    public bool manufactureActivityList()
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_activities", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;
                object name = null;
                object posID = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                posID = rdr[2] as object ?? default(object);

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

                int _posID = 0;
                if (posID != DBNull.Value)
                {
                    _posID = (int)posID;
                }

                activity newActivity = new activity(_id, _name, _posID);

                equivalencyController.addActivity(newActivity);
            }

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
    /// 
    /// </summary>
    /// <returns></returns>
    public bool manufactureDetailList()
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_details", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;
                object name = null;
                object activityID = null;
                object sDate = null;
                object eDate = null;
                object every = null;
                object unitID = null;
                object hours = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                activityID = rdr[2] as object ?? default(object);
                sDate = rdr[3] as object ?? default(object);
                eDate = rdr[4] as object ?? default(object);
                every = rdr[5] as object ?? default(object);
                unitID = rdr[6] as object ?? default(object);
                hours = rdr[7] as object ?? default(object);

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

                int _activityID = 0;
                if (activityID != DBNull.Value)
                {
                    _activityID = (int)activityID;
                }

                DateTime? _sDate = null;
                if (sDate != DBNull.Value)
                {
                    _sDate = (DateTime)sDate;
                }

                DateTime? _eDate = null;
                if (eDate != DBNull.Value)
                {
                    _eDate = (DateTime)eDate;
                }

                decimal _every = 0;
                if (every != DBNull.Value)
                {
                    _every = (decimal)every;
                }

                int _unitID = 0;
                if (unitID != DBNull.Value)
                {
                    _unitID = (int)unitID;
                }

                decimal _hours = 0;
                if (hours != DBNull.Value)
                {
                    _hours = (decimal)hours;
                }

                detail newDetail = new detail(_id, _name, _activityID, _sDate, _eDate, _every, _unitID, _hours);

                equivalencyController.addDetail(newDetail);
            }

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
    ~equivalencyFactory() 
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