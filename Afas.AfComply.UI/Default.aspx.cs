using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;
using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;
using Afas.Domain;
using System.Web.Configuration;
using System.Configuration;

public partial class _Default : PageBase
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
    
    /// <summary>
    /// 01) Page_Load function. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // nop
    }

    /// <summary>
    /// 02) Login Function. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnLogin_Click(object sender, EventArgs e)
    {
      
        Session.Clear();
        Session["ValidLogon"] = null;
        Session["CurrentUser"] = null;
        Session["CurrentDistrict"] = null;

        String _username = TxtUserName.Text.ToLower();
        String _password = TxtPassword.Text;
        
        User currUser = null;
        employer currDist = null;

        currUser = UserController.validateLogin(_username, _password);
        
        if (currUser != null)
        {

            SecurityLogger.LogLogin(new HttpRequestWrapper(Request), _username);
          

            currDist = employerController.getEmployer(currUser.User_District_ID);

         
            string CookieId = System.Configuration.ConfigurationManager.AppSettings["ReportingAuthCookieId"];
            HttpCookie cookie = new HttpCookie(CookieId);
            cookie.HttpOnly = true;
            cookie.Secure = Request.IsSecureConnection;
            cookie.Shareable = false;

            this.EncryptedParameters[Guid.NewGuid().ToString()] = DateTime.Now.ToUniversalTime().ToString();

            this.EncryptedParameters["UserId"] = currUser.User_UserName;

            this.EncryptedParameters["EmployerId"] = currUser.User_District_ID.ToString();
            this.EncryptedParameters["EmployerResourceId"] = currDist.ResourceId.ToString();
            this.EncryptedParameters["EmployerDbaName"] = currDist.DisplayName;

            this.EncryptedParameters["IsAdmin"] = currUser.User_Power.ToString();
            

            cookie.Value = this.EncryptedParameters.AsMvcUrlParameter;
            cookie.Expires = DateTime.Now.AddHours(4);
            Response.SetCookie(cookie);


       
            if (currUser.User_PWD_RESET == true)
            {
                if (currUser.LAST_MOD.Value.AddMinutes(Feature.PasswordMinute) <= System.DateTime.Now)
                {
                    Response.Redirect("/Forgotpassword.aspx", false);
                    return;
                }
                else
                {
                    Session["CurrentUser"] = currUser;
                    Session["CurrentDistrict"] = currDist;
                    Session["ValidLogon"] = "true";//"false";

                    Response.Redirect("/securepages/passwordreset.aspx", false);
                    return;
                }

            }
            else
            {

                Session["CurrentUser"] = currUser;
                Session["CurrentDistrict"] = currDist;
                Session["ValidLogon"] = "true";

                if (Request.Params["redirect"] != null)
                {

                    IEncryptedParameters FromEncryptedParameters = new EncryptedParameters();
                    FromEncryptedParameters.Load(Request.Params["redirect"]);

                    Response.Redirect(FromEncryptedParameters["LoggedOutFrom"]);
                    return;

                }
                else if (currDist.EMPLOYER_ID == 1)
                {
                    Response.Redirect("admin/Default.aspx", false);
                    return;
                }
                else
                {
                    Response.Redirect("securepages/Default.aspx", false);
                    return;
                }
            }

        }
        else
        {

            this.Log.Info(String.Format("Failed login attempt for user [{0}], from IP [{1}]", _username, Request.UserHostAddress));

            SecurityLogger.LogFailedLogin(new HttpRequestWrapper(Request), _username);

            LblMessage.Text = "Please enter valid credentials.";
            LblMessage.ForeColor = System.Drawing.Color.White;
            LblMessage.BackColor = System.Drawing.Color.Red;

        }

    }

    /// <summary>
    /// 03) Redirect users to the registrations process. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>


}