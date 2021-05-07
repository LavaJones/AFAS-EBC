using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using log4net;
using Afas.AfComply.Domain;
using System.Configuration;

/// <summary>
/// Summary description for UserFactory
/// </summary>
public class UserFactory
{

    private ILog Log = LogManager.GetLogger(typeof(UserFactory));
    
    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

    public UserFactory()
    {
    }

    public User ManufactureUser(
            String _fname, 
            String _lname, 
            String _email, 
            String _phone, 
            String _username, 
            String _password, 
            int _districtID, 
            Boolean _powerUser, 
            DateTime _lastMod, 
            String _lastModBy, 
            Boolean _active, 
            Boolean _billing, 
            Boolean _irsContact
        )
    {

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        try
        {

            conn.Open();

            cmd.CommandText = "INSERT_new_user";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            string salt = HashPassword.HashedPassword.CreateSalt(_username);
            string hashedPassword = HashPassword.HashedPassword.HashPassword(salt, _password);

            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = _email.checkForDBNull();
            cmd.Parameters.AddWithValue("@phone", SqlDbType.VarChar).Value = _phone.checkForDBNull();
            cmd.Parameters.AddWithValue("@username", SqlDbType.VarChar).Value = _username.checkForDBNull();
            cmd.Parameters.AddWithValue("@password", SqlDbType.VarChar).Value = hashedPassword.checkForDBNull();
            cmd.Parameters.AddWithValue("@employerid", SqlDbType.Int).Value = _districtID.checkIntNull();
            cmd.Parameters.AddWithValue("@active", SqlDbType.Bit).Value = _active.checkBoolNull();
            cmd.Parameters.AddWithValue("@power", SqlDbType.Bit).Value = _powerUser.checkIntNull();
            cmd.Parameters.AddWithValue("@datestamp", SqlDbType.DateTime).Value = _lastMod.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@user", SqlDbType.VarChar).Value = _lastModBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@billing", SqlDbType.Bit).Value = _billing.checkDateDBNull();
            cmd.Parameters.AddWithValue("@irsContact", SqlDbType.Bit).Value = _irsContact.checkBoolNull();

            cmd.Parameters.Add("@userid", SqlDbType.Int);
            cmd.Parameters["@userid"].Direction = ParameterDirection.Output;

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {

                int _id = (int)cmd.Parameters["@userid"].Value;

                User newUser = new User(
                        _id, 
                        _fname, 
                        _lname, 
                        _email, 
                        _phone, 
                        _username, 
                        hashedPassword, 
                        _districtID, 
                        _powerUser, 
                        true, 
                        _lastMod, 
                        _lastModBy, 
                        false, 
                        _billing, 
                        _irsContact, 
                        false
                    );
                
                return newUser;

            }
            else
            {
                return null;
            }

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);
            
            return null;
        
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_fname"></param>
    /// <param name="_lname"></param>
    /// <param name="_email"></param>
    /// <param name="_phone"></param>
    /// <param name="_username"></param>
    /// <param name="_password"></param>
    /// <param name="_districtID"></param>
    /// <returns></returns>
    public bool deleteUser(int _userID)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();

            cmd.CommandText = "DEACTIVATE_user";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userID", SqlDbType.VarChar).Value = _userID;

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }




    public bool updateUser(int _userID, string _fname, string _lname, string _email, string _phone, bool _powerUser, string _modBy, DateTime _modOn, bool _billing, bool _irsContact)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        try
        {

            conn.Open();

            cmd.CommandText = "UPDATE_user";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userID", SqlDbType.Int).Value = _userID.checkForDBNull();
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@email", SqlDbType.Date).Value = _email.checkForDBNull();
            cmd.Parameters.AddWithValue("@phone", SqlDbType.Money).Value = _phone.checkForDBNull();
            cmd.Parameters.AddWithValue("@power", SqlDbType.Int).Value = _powerUser.checkBoolNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkStringNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.Parameters.AddWithValue("@billing", SqlDbType.Bit).Value = _billing.checkForDBNull();
            cmd.Parameters.AddWithValue("@irsContact", SqlDbType.Bit).Value = _irsContact.checkForDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }

    public bool updateUserFloatingFlag(int _userID, bool _floaterFlag)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            int floater = _floaterFlag? 1 : 0;     

            conn.Open();

            cmd.CommandText = "UPDATE_Floater";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@setFloater", SqlDbType.Bit).Value = floater.checkForDBNull();
            cmd.Parameters.AddWithValue("@userId", SqlDbType.Int).Value = _userID.checkForDBNull();
            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }

    public bool updateFloatingUser(int _employerID, int _userID)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        try
        {

            conn.Open();

            cmd.CommandText = "UPDATE_user_floating";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID.checkForDBNull();
            cmd.Parameters.AddWithValue("@userID", SqlDbType.Int).Value = _userID.checkForDBNull();
            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }

    public bool userResetPassword(string curremail, string curruname, string _modBy, DateTime _modOn, bool _resetRequired, string _newPwd, out string errorMsg)
    {
        errorMsg = null;
        bool pwdResetSuccess = false;
        string _tempPwd = null;
        bool _sendEmail = false;
        try
        {
            List<User> tempUsers = UserController.getAllUsers();
            
           
            foreach (User u in tempUsers)
            {
                if (u.User_Email.Equals(curremail) && u.User_UserName.ToLower().Equals(curruname))
                {
                    if (_newPwd == null)
                    {
                        RandomGenerator rsg = new RandomGenerator();
                        _tempPwd = rsg.AutoGeneratePassword();
                        _sendEmail = true;
                    }
                    else 
                    {
                        _tempPwd = _newPwd;
                    }

                    pwdResetSuccess = updatePwd(_tempPwd, curruname, curremail, _modBy, _modOn, _resetRequired);

                    if (pwdResetSuccess == true && _sendEmail == true)
                    {
                        Email em = new Email();
                        bool email_sent = em.SendEmail(curremail, "Password Reset", "This temporary password will expire in " + Feature.PasswordMinute + " minutes. Your Temporary Password is: " + _tempPwd, false);
                        pwdResetSuccess = email_sent;
                    }
                    else
                    {
                        errorMsg = "An error occurred while resetting your password. Please try again and if you need help, give us a call.";
                    }

                    break;
                }
            }

            if (pwdResetSuccess == true)
            {
                return true;
            }
            else
            {
                errorMsg = "An error occurred while resetting the password. Please try again and if you continue getting this message, call "+Branding.CompanyShortName+" at the number in the top right corner of the page. ";

                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            errorMsg = "An error occurred while resetting your password. Please try again and if you need help, give us a call.";

            return false;
        }
    }


    public bool usernameRetrieval(string curremail, out string errorMsg)
    {
        errorMsg = null;
        string _tempUsername = null;
        bool usernameFound = false;

        try
        {
            List<User> tempUsers = UserController.getAllUsers();

            foreach (User u in tempUsers)
            {
                if (u.User_Email.Equals(curremail))
                {
                    _tempUsername = u.User_UserName;

                    Email em = new Email();
                    em.SendEmail(curremail, "Username Retrieval", _tempUsername, false);

                    usernameFound = true;
                    errorMsg = "Your username was emailed to you";
                    break;
                }
            }

            if (usernameFound == false)
            {
                errorMsg = "The system could not find the email address entered. Please try again and if you need help, give us a call.";
            }

            return true;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            errorMsg = "The system could not find the email address entered. Please try again and if you need help, give us a call.";

            return false;
        }
    }

    private bool updatePwd(string _newPwd, string _username, string _email, string _modBy, DateTime _modOn, bool _resetRequired)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();

        try
        {
            conn.Open();

            cmd.CommandText = "UPDATE_reset_pwd";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            string salt = HashPassword.HashedPassword.CreateSalt(_username);
            string hashedPassword = HashPassword.HashedPassword.HashPassword(salt, _newPwd);

            cmd.Parameters.AddWithValue("@pwd", SqlDbType.Int).Value = hashedPassword.checkForDBNull();
            cmd.Parameters.AddWithValue("@username", SqlDbType.VarChar).Value = _username.checkForDBNull();
            cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = _email.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.Date).Value = _modOn.checkDateNull();
            cmd.Parameters.AddWithValue("@pwdReset", SqlDbType.Bit).Value = _resetRequired.checkBoolNull();
            
            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
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
    }
}
