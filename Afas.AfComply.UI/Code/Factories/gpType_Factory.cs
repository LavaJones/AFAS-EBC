using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// This class handles all of the Database transactions for anything to do with the class gpType. 
/// </summary>
public class gpType_Factory
{
    private ILog Log = LogManager.GetLogger(typeof(gpType_Factory));
    

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

    /// <summary>
    /// Gets all of an Employer's Gross Pay Types from the database and returns an Object List of gpType.
    /// </summary>
    /// <returns></returns>
    public List<gpType> manufactureGrossPayType(int _employerID)
    {
        List<gpType> tempList = new List<gpType>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_employer_gross_pay_types", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object employerID = null;
                object extID = null;
                object desc = null;
                object active = null;

                id = rdr[0] as object ?? default(object);
                employerID = rdr[1] as object ?? default(object);
                extID = rdr[2] as object ?? default(object);
                desc = rdr[3] as object ?? default(object);
                active = rdr[4] as object ?? default(object);

                int _id = id.checkIntNull();
                int _empID = employerID.checkIntNull();
                string _extID = (string)extID.checkStringNull();
                string _desc = (string)desc.checkStringNull();
                bool _active = active.checkBoolNull();

                gpType newgpType = new gpType(_id, _empID, _extID, _desc, _active);
                tempList.Add(newgpType);
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<gpType> manufactureGrossPayFilter(int _employerID)
    {
        List<int> gpFilters = new List<int>();
        List<gpType> gpList = new List<gpType>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_employer_gross_pay_filters", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;

                id = rdr[2] as object ?? default(object);

                int _id = id.checkIntNull();

                gpFilters.Add(_id);
            }

            foreach (int i in gpFilters)
            {
                foreach (gpType tp in gpType_Controller.getEmployeeTypes(_employerID))
                {
                    if (i == tp.GROSS_PAY_ID)
                    {
                        gpList.Add(tp);
                        break;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            gpFilters = new List<int>();
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

        return gpList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public gpType manufactureGrossPayType(int _employerID, string _extID, string _name, bool _active)
    {
        List<gpType> tempList = new List<gpType>();
        gpType tempGP = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("INSERT_new_gross_pay", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@extID", SqlDbType.VarChar).Value = _extID;
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name;
            cmd.Parameters.AddWithValue("@active", SqlDbType.Bit).Value = _active;

            cmd.Parameters.Add("@gpID", SqlDbType.Int);
            cmd.Parameters["@gpID"].Direction = ParameterDirection.Output;

            int rows = 0;
           rows = cmd.ExecuteNonQuery();
           if (rows == 1)
           {
               int gpID = (int)cmd.Parameters["@gpID"].Value;
               tempGP = new gpType(gpID, _employerID, _extID, _name, _active);
           }
           else
           {
               tempGP = null;
           }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempGP = null;
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

        return tempGP;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public gpType manufactureGrossPayFilter(int _gpID, int _employerID)
    {
        gpType tempGP = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("INSERT_new_gross_pay_filter", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@gpID", SqlDbType.VarChar).Value = _gpID;

            int rows = 0;
            rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {   
                List<gpType> tempList = gpType_Controller.getEmployeeTypes(_employerID);

                foreach (gpType gt in tempList)
                {
                    if (gt.GROSS_PAY_ID == _gpID)
                    {
                        tempGP = gt;
                        break;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempGP = null;
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

        return tempGP;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool removeGrossPayFilter(int _gpID)
    {
        bool success = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_employer_gross_pay_filter", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@gpID", SqlDbType.Int).Value = _gpID;

            int row = 0;
            row = cmd.ExecuteNonQuery();

            if (row > 0)
            {
                success = true;
            }
           
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            success = false;
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

        return success;
    }


    /// <summary>
    /// This will take two different Gross Pay ID's and merget them into one. 
    /// We needed to add this as there are many instances where the leading zeros were missing in 
    /// reworked files and we needed a way to correct this in the software as it will continue to 
    /// happen when the users are manually manipulating the payroll data. 
    /// </summary>
    /// <returns></returns>
    public bool mergeGrossPayType(int _gpID, int _gpID2)
    {
        bool success = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("MERGE_gross_pay_description", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@gpID", SqlDbType.Int).Value = _gpID;
            cmd.Parameters.AddWithValue("@gpID2", SqlDbType.Int).Value = _gpID2;

            int row = 0;
            row = cmd.ExecuteNonQuery();

            if (row > 0)
            {
                success = true;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            success = false;
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

        return success;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool updateGpDescription(int _gpID, string _name)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("UPDATE_gp_description", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@gpID", SqlDbType.Int).Value = _gpID;
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name;

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
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return validTransaction;
    }
    
    /// <summary>
    /// Distructor to be sure all resources are released
    /// </summary>
    ~gpType_Factory() 
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