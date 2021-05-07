using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;

namespace Afas.AfComply.UI.securepages 
{

    /// <summary>
    /// Common code and functionality that all secure pages share.
    /// </summary>
    public abstract class SecurePageBase : PageBase
    {

        protected virtual void Page_Load(object sender, EventArgs e)
        {

            foreach (var cookey in Request.Cookies.AllKeys)
            {
                var reqCookie = Request.Cookies[cookey];

                if (reqCookie != null)
                {

                    HttpCookie respCookie = new HttpCookie(reqCookie.Name, reqCookie.Value);
                    respCookie.Expires = DateTime.Now.AddMinutes(25);
                    respCookie.Secure = Request.IsSecureConnection;
                    Response.Cookies.Set(respCookie);

                }

            }

            User currUser = (User)Session["CurrentUser"];

            if (currUser != null && UserController.validateUser(currUser) && (string)Session["ValidLogon"] == "true")
            {
                string CookieId = System.Configuration.ConfigurationManager.AppSettings["ReportingAuthCookieId"];
                HttpCookie cookie = new HttpCookie(CookieId);
                cookie.HttpOnly = true;
                cookie.Secure = Request.IsSecureConnection;
                cookie.Shareable = false;

                employer currDist = employerController.getEmployer(currUser.User_District_ID);

                this.EncryptedParameters[Guid.NewGuid().ToString()] = DateTime.Now.ToUniversalTime().ToString();

                this.EncryptedParameters["UserId"] = currUser.User_UserName;

                this.EncryptedParameters["EmployerId"] = currUser.User_District_ID.ToString();
                this.EncryptedParameters["EmployerResourceId"] = currDist.ResourceId.ToString();
                this.EncryptedParameters["EmployerDbaName"] = currDist.DisplayName;

                this.EncryptedParameters["IsAdmin"] = currUser.User_Power.ToString();

                cookie.Value = this.EncryptedParameters.AsMvcUrlParameter;
                cookie.Expires = DateTime.Now.AddHours(4);
                Response.SetCookie(cookie);

            }
            else
            {
                Log.Warn("Current user not set in session or login is not valid.");
                Response.Redirect("~/Logout.aspx", false);
                return;
            }

            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            if (!Page.IsPostBack)
            {
                try
                {
                    
                    String validLogon = (String) Session["ValidLogon"];
                    Boolean validUser = UserController.validateUser(currUser);

                    PageLoadNonPostBack();

                    if (validUser == true && validLogon == "true")
                    {                        
                        
                        employer currDist = (employer) Session["CurrentDistrict"];

                        PageLoadLoggedIn(currUser, currDist);

                    }
                    else
                    {

                        this.Log.Warn(
                                String.Format("A user tried to access a secured page when not logged in. IP:[{0}], DNS Name:[{1}], Raw URL:[{2}]", 
                                        Request.UserHostAddress,
                                        Request.UserHostName,
                                        Request.RawUrl
                                    )
                            );

                        SecurityLogger.LogInvalidAccess(new HttpRequestWrapper(Request));


                        var redirect = Request.Path;
                        if (Request.QueryString != null && Request.QueryString.Count > 0)
                        {
                            redirect += "?" + Request.QueryString;
                        }
                        IEncryptedParameters EncryptedParameters = new EncryptedParameters();

                        EncryptedParameters[Guid.NewGuid().ToString()] = DateTime.Now.ToUniversalTime().ToString();
                        EncryptedParameters["LoggedOutFrom"] = redirect;

                        Response.Redirect("~/Logout.aspx?redirect=" + EncryptedParameters.AsMvcUrlParameter, false);
                        
                        return;
                    
                    }                   
                
                }
                catch (Exception exception)
                {

                    this.Log.Warn("Suppressing errors during page load for authentication/authorization checks.", exception);

                    Response.Redirect("/default.aspx?error=65", false);

                    return;
                
                }
            }            
            else 
            {

                if ((employer)Session["CurrentDistrict"] == null)
                {
                    Log.Warn("Current employer not set in session.");
                    Response.Redirect("~/Logout.aspx", false);
                    return;
                }


                PageLoadPostBack();
            }

        }

        /// <summary>
        /// Page specific functionality for when pageload is called as post back. Override with special behavor. 
        /// </summary>
        protected virtual void PageLoadPostBack() 
        {
        }

        /// <summary>
        ///  Page specific functionality for when pageload is called not as post back but before log in is checked. Override with special behavor. 
        /// </summary>
        protected virtual void PageLoadNonPostBack()
        {
        }

        /// <summary>
        /// Page specific behavior for when a user has been verified. Such as loading data and setting user display.
        /// </summary>
        /// <param name="user">The user that is logged in.</param>
        /// <param name="employer">The Employer of the logged in user.</param>
        protected abstract void PageLoadLoggedIn(User user, employer employer);

        private ILog Log = LogManager.GetLogger(typeof(SecurePageBase));

    }

}