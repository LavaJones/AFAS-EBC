using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;
using System.Configuration;

namespace Afas.AfComply.UI
{
    public partial class Forgotpassword : PageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(_Default));

        protected override void OnInit(EventArgs eventArgs)
        {

            base.OnInit(eventArgs);

            if (User.Identity.IsAuthenticated)
            {
                ViewStateUserKey = Session.SessionID;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSaveNewPassword_Click(object sender, EventArgs e)
        {
            String curruname = null;
            String curremail = null;
            String _modBy = "Password Reset";
            DateTime _modOn = System.DateTime.Now;
            bool pwdResetSuccess = false;
            bool _resetRequired = true;

            string errorMsg = null;

            curremail = TxtResetEmail.Text.ToLower();
            curruname = TxtResetUsername.Text.ToLower();

            // Call User Password Reset Function. 
            pwdResetSuccess = UserController.userResetPassword(curremail, curruname, _modBy, _modOn, _resetRequired, null, out errorMsg);

            if (errorMsg != null)
            {
                LblNewUserMessage.Text = errorMsg;
            }

            if (pwdResetSuccess == true)
            {

                SecurityLogger.LogPasswordReset(new HttpRequestWrapper(Request), curruname, curremail);

                LblNewUserMessage.Text = "Your password has been reset. Please check your email for your new temporary password. It will expire in " + Feature.PasswordMinute + " minutes. The next time you login, you will be required to change your password.";
                LblNewUserMessage.ForeColor = System.Drawing.Color.Black;
                LblNewUserMessage.BackColor = System.Drawing.Color.Yellow;
                LblNewUserMessage1.Text = "<a href=\"/Default.aspx\">Click here</a> to go back to login page.";
                BtnSaveNewPassword.Enabled = false;
                BtnSaveNewPassword.BackColor = System.Drawing.Color.Gray;
                BtnSaveNewPassword.BorderColor = System.Drawing.Color.Gray;
            }
            else
            {
                SecurityLogger.LogFailedPasswordReset(new HttpRequestWrapper(Request), curruname, curremail);
            }
        }
    }
}