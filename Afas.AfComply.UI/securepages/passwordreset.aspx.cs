using Afas.AfComply.UI.securepages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class securepages_passwordreset : SecurePageBase
{
    protected override void PageLoadLoggedIn(User user, employer employer)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        User currUser = (User) Session["CurrentUser"];
        string _username = currUser.User_UserName.ToLower();
        string _email = currUser.User_Email;
        DateTime _modOn = System.DateTime.Now;
        string _newPwd = null;
        bool validData = true;
        bool pwdUpdated = false;

        validData = errorChecking.validateTextBoxPassword(TxtResetNewPassword, TxtResetNewPassword2, validData);

        if (validData == true)
        {
            string errorMsg = null;
            _newPwd = TxtResetNewPassword.Text;
            pwdUpdated = UserController.userResetPassword(_email, _username, _username, _modOn, false, _newPwd, out errorMsg);

            if (errorMsg != null)
            {
                LblUserMessage.Text = "An error occurred while resetting your password.";
            }

            if (pwdUpdated == true)
            {
                Session["ValidLogon"] = "true";
                Response.Redirect("default.aspx", false);
            }
            else
            {
                LblUserMessage.Text = "An error occurred while, please verify your old and new passwords.";
            }
        }
        else
        {
            LblUserMessage.Text = "An error occurred while, please verify your old and new passwords.";
        }
    }
}