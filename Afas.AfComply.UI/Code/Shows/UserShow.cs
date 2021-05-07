using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for UserShow
/// </summary>
public class UserShow
{
    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

    

    private ILog Log = LogManager.GetLogger(typeof(UserShow));


    public UserShow()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_username"></param>
    /// <param name="_password"></param>
    /// <param name="_admin"></param>
    /// <returns></returns>
    public User validateLogin(String _username, String _password)
    {
        this.Log.Info("validateLogin ctor called.");

        _username = _username.ToLower();
        User validUser = null;
        List<User> tempUsers = new List<User>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;
        try
        {
            
            String salt = HashPassword.HashedPassword.CreateSalt(_username);
            String hashpword = HashPassword.HashedPassword.HashPassword(salt, _password);

            conn.Open();

            cmd.CommandText = "VALIDATE_user";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", SqlDbType.VarChar).Value = _username;
            cmd.Parameters.AddWithValue("@password", SqlDbType.VarChar).Value = hashpword;

            rdr = cmd.ExecuteReader();

            tempUsers = readReader(rdr);

            if (tempUsers.Count == 1)
            {
                validUser = tempUsers[0];
            }
            else
            {
                SecurityLogger.LogFailedPasswordAttempt(_username);
            }
           
        }
        catch (Exception exception)
        {
            
            Log.Warn("Suppressing errors.", exception);
            
            validUser = null;
        
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

        return validUser;

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_username"></param>
    /// <param name="_password"></param>
    /// <param name="_admin"></param>
    /// <returns></returns>
    public List<User> showEmployerUsers(int _employerID)
    {
        List<User> tempList = new List<User>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employer_users";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.VarChar).Value = _employerID;

            rdr = cmd.ExecuteReader();

            tempList = readReader(rdr)
                .OrderBy(sortVal => sortVal.User_Last_Name)
                .ToList();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<User>();
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
    /// 
    /// </summary>
    /// <param name="_username"></param>
    /// <param name="_password"></param>
    /// <param name="_admin"></param>
    /// <returns></returns>
    public List<User> showAllUsers()
    {
        List<User> tempList = new List<User>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_all_users";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            tempList = readReader(rdr)
                .OrderBy(sortVal => sortVal.User_Last_Name)
                .ToList();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<User>();
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





    private List<User> readReader(SqlDataReader rdr)
    {
        User newUser = null;
        List<User> tempList = new List<User>();

        while (rdr.Read())
        {
            object id = null;
            object fname = null;
            object lname = null;
            object email = null;
            object phone = null;
            object username = null;
            object password = null;
            object distID = null;
            object active = null;
            object power = null;
            object lastMod = null;
            object lastModBy = null;
            object resetRequired = null;
            object billing = null;
            object irsContact = null;
            object floater = null;
            object resourceId = null;

            id = rdr[0] as object ?? default(object);
            fname = rdr[1] as object ?? default(object);
            lname = rdr[2] as object ?? default(object);
            email = rdr[3] as object ?? default(object);
            phone = rdr[4] as object ?? default(object);
            username = rdr[5] as object ?? default(object);
            password = rdr[6] as object ?? default(object);
            distID = rdr[7] as object ?? default(object);
            active = rdr[8] as object ?? default(object);
            power = rdr[9] as object ?? default(object);
            lastModBy = rdr[10] as object ?? default(object);
            lastMod = rdr[11] as object ?? default(object);
            resetRequired = rdr[12] as object ?? default(object);
            billing = rdr[13] as object ?? default(object);
            irsContact = rdr[14] as object ?? default(object);
            floater = rdr[15] as object ?? default(object);
            resourceId = rdr[16] as object ?? default(object);

            int _id = id.checkIntNull();
            string _fname = fname.checkStringNull().ToString();
            string _lname = lname.checkStringNull().ToString();
            string _email = email.checkStringNull().ToString();
            string _phone = (string)phone.checkStringNull();
            string _un = username.checkStringNull().ToString();
            string _pass = password.checkStringNull().ToString();
            int _distID = distID.checkIntNull();
            bool _power = power.checkBoolNull();
            bool _active = active.checkBoolNull();
            DateTime? _lastMod = lastMod.checkDateNull();
            string _lastModBy = (string)lastModBy.checkStringNull();
            bool _resetRequired = resetRequired.checkBoolNull();
            bool _billing = billing.checkBoolNull();
            bool _irsContact = irsContact.checkBoolNull();
            bool _floater = floater.checkBoolNull();
            Guid _resourceId = resourceId.checkGuidNull();

            newUser = new User(_id, _fname, _lname, _email, _phone, _un, _pass, _distID, _power, _active, _lastMod, _lastModBy, _resetRequired, _billing, _irsContact, _floater, _resourceId);
            tempList.Add(newUser);
        }

        return tempList;
    }



    public List<User> getBillingUsers(int _employerID)
    {
        List<User> billingList = new List<User>();
        List<User> currUsers = new List<User>();

        currUsers = UserController.getDistrictUsers(_employerID);

        foreach (User u in currUsers)
        {
            if (u.User_Billing == true)
            {
                billingList.Add(u);
            }
        }

        return billingList;
            
    }


    public User findUser(int _userID, int _employerID)
    {
        User selectedUser = null;

        try
        {
            List<User> tempList = UserController.getDistrictUsers(_employerID);

            foreach (User u in tempList)
            {
                if (u.User_ID == _userID)
                {
                    selectedUser = u;
                    break;
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            selectedUser = null;
        }

        return selectedUser;
    }

}