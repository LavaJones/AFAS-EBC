using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;

namespace Afas.AfComply.UI
{
    public partial class ForgotUsername : PageBase
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

        protected void BtnRetrieveUsername_Click(object sender, EventArgs e)
        {
            String curremail = null;
            bool usernameRetrieval = false;

            curremail = TxtUNEmail.Text.ToLower();

            string errorMsg = null;

            //Call User Password Reset Function. 
            usernameRetrieval = UserController.usernameRetrieval(curremail, out errorMsg);

            if (errorMsg != null)
            {
                LblUserMessage2.Text = errorMsg;
            }

            if (usernameRetrieval == true)
            {

                SecurityLogger.LogUsernameRecovery(new HttpRequestWrapper(Request), "Missing in current implementation, look by email.", curremail);

                LblUserMessage2.Text = "Your username has been emailed to you.";
                LblUserMessage3.Text = "<a href=\"/Default.aspx\">Click here</a> to go back to login page.";
                BtnRetrieveUsername.Enabled = false;
                BtnRetrieveUsername.BackColor = System.Drawing.Color.Gray;
                BtnRetrieveUsername.BorderColor = System.Drawing.Color.Gray;
            }
        }
    }
}