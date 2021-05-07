using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;

/// <summary>
/// Summary description for UserController
/// </summary>
public static class UserController
{
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
    public static User orderUser(string _fname, string _lname, string _email, string _phone, string _username, string _password, int _districtID, bool _powerUser, DateTime _lastMod, string _lastModBy, bool _active, bool _billing, bool _irsContact)
    {
        UserFactory ef = new UserFactory();
        return ef.ManufactureUser(_fname, _lname, _email, _phone, _username, _password, _districtID, _powerUser, _lastMod, _lastModBy, _active, _billing, _irsContact);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_userName"></param>
    /// <param name="_password"></param>
    /// <returns></returns>
    public static User validateLogin(string _userName, string _password)
    {
        UserShow us = new UserShow();
        return us.validateLogin(_userName.ToLower(), _password);
    }

    public static bool validateUser(User user)
    {
        if (user != null)
        { 
            return true; 
        }
        else
        {
            return false;
        }
    }

    public static bool validateUser(User _user, Literal _lit, HiddenField _hf)
    {
        if (_user != null)
        {
            _lit.Text = _user.User_Full_Name;
            _hf.Value = _user.User_District_ID.ToString();

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_distID"></param>
    /// <returns></returns>
    public static List<User> getDistrictUsers(int _distID)
    {
        UserShow us = new UserShow();
        return us.showEmployerUsers(_distID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_distID"></param>
    /// <returns></returns>
    public static List<User> getAllUsers()
    {
        UserShow us = new UserShow();
        return us.showAllUsers();
    }


    public static bool deleteUser(int _userID)
    {
        UserFactory uf = new UserFactory();
        return uf.deleteUser(_userID);
    }

    public static bool updateUser(int _userID, string _fname, string _lname, string _email, string _phone, bool _power, string _modBy, DateTime _modOn, bool _billing, bool _irsContact)
    {
        UserFactory uf = new UserFactory();
        return uf.updateUser(_userID, _fname, _lname, _email, _phone, _power, _modBy, _modOn, _billing, _irsContact);
    }

    public static bool userResetPassword(string curremail, string curruname, string _modBy, DateTime _modOn, bool _resetRequired, string _newPwd, out string errorMsg)
    {
        UserFactory uf = new UserFactory();
        return uf.userResetPassword(curremail, curruname, _modBy, _modOn, _resetRequired, _newPwd, out errorMsg);
    }

    public static bool usernameRetrieval(string curremail, out string errorMsg)
    {
        UserFactory uf = new UserFactory();
        return uf.usernameRetrieval(curremail, out errorMsg);
    }

    public static List<User> getBillingUsers(int _employerID)
    {
        UserShow us = new UserShow();
        return us.getBillingUsers(_employerID);
    }

    public static User findUser(int _userID, int _employerID)
    {
        UserShow us = new UserShow();
        return us.findUser(_userID, _employerID);
    }

    public static bool updateUserFloatingFlag(int _employerID, bool _floatFlag)
    {
        UserFactory uf = new UserFactory();
        return uf.updateUserFloatingFlag(_employerID, _floatFlag);
    }

    public static bool updateFloatingUser(int _employerID, int _userID)
    {
        UserFactory uf = new UserFactory();
        return uf.updateFloatingUser(_employerID, _userID);
    }
}