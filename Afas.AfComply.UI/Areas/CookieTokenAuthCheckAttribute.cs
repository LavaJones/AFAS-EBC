using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using Afc.Framework.Presentation.Web;
using Afc.Core.Presentation.Web;
using log4net;
using Afas.Domain;

namespace Afas.AfComply.UI.Areas
{
    /// <summary>
    /// This attribute manages our login info that is stored encrypted in a cookie
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class CookieTokenAuthCheckAttribute : AuthorizeAttribute
    {
        private static ILog Log = LogManager.GetLogger(typeof(CookieTokenAuthCheckAttribute));

        /// <summary>
        /// Gets the User Id (name) from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>the User Id (name)</returns>
        public static String GetUserId(HttpContext httpContext)
        {
            return GetObjectByKey(new System.Web.HttpContextWrapper(httpContext), "UserId");
        }

        /// <summary>
        /// Gets the User Id (name) from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>the User Id (name)</returns>
        public static String GetUserId(HttpContextBase httpContext)
        {
            return GetObjectByKey(httpContext, "UserId");
        }

        /// <summary>
        /// Gets the Employers Id from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>the Employers Id</returns>
        public static String GetEmployerId(HttpContext httpContext)
        {
            return GetObjectByKey(new System.Web.HttpContextWrapper(httpContext), "EmployerId");
        }

        /// <summary>
        /// Gets the Employers Id from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>the Employers Id</returns>
        public static String GetEmployerId(HttpContextBase httpContext)
        {
            return GetObjectByKey(httpContext, "EmployerId");
        }

        /// <summary>
        /// Gets the Employers Resource Id from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>the Employers Resource Id</returns>
        public static String GetEmployerResourceId(HttpContext httpContext)
        {
            return GetObjectByKey(new System.Web.HttpContextWrapper(httpContext), "EmployerResourceId");
        }

        /// <summary>
        /// Gets the Employers Resource Id from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>the Employers Resource Id</returns>
        public static String GetEmployerResourceId(HttpContextBase httpContext)
        {
            return GetObjectByKey(httpContext, "EmployerResourceId");
        }

        /// <summary>
        /// Gets the Employers Display Name from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>the Employers Display Name</returns>
        public static String GetEmployerDbaName(HttpContext httpContext)
        {
            return GetObjectByKey(new System.Web.HttpContextWrapper(httpContext), "EmployerDbaName");
        }

        /// <summary>
        /// Gets the Employers Display Name from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>v</returns>
        public static String GetEmployerDbaName(HttpContextBase httpContext)
        {
            return GetObjectByKey(httpContext, "EmployerDbaName");
        }

        /// <summary>
        /// Gets any string value stored in the Cookie by the items Key
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <param name="key">The Key of the item to retrieve</param>
        /// <returns>The item's string value or null</returns>
        public static String GetObjectByKey(HttpContextBase httpContext, String key)
        {

            string CookieId = ConfigurationManager.AppSettings["ReportingAuthCookieId"];
            HttpCookie authCookie = httpContext.Request.Cookies[CookieId];

            if (authCookie == null
                || authCookie.Value == null
                || authCookie.Value == string.Empty)
            {
                return null;

            }
                
            IEncryptedParameters param = new EncryptedParameters();
            
            if (false == param.Load(authCookie.Value))
            {
                Log.Warn("Cookie Auth failed to parse params: " + authCookie.Value);

            }

            if (param.AllParameterNames.Count() <= 1)
            {

                return null;

            }

            String val = param[key.ToLower()];
            if (val == null || val.ToString() == string.Empty || val.ToString().Trim() == string.Empty)
            {

                return null;

            }

            return val;

        }

        /// <summary>
        /// Simple core to the authorization that checks if the cookie is valid and has a valid User Id
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>True if the Cookie is Valid and has a valid User Id</returns>
        protected override Boolean AuthorizeCore(HttpContextBase httpContext)
        {

            string userId = GetObjectByKey(httpContext, "UserId");

            if (userId.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// This is the actions we take if they are no longer Logged in
        /// </summary>
        /// <param name="context">The context</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {

            base.HandleUnauthorizedRequest(context);

            var Request = context.RequestContext.HttpContext.Request;
            SecurityLogger.LogInvalidAccessAdmin(Request);

            var redirect = Request.Path;
            if (Request.QueryString != null && Request.QueryString.Count > 0)
            {
                redirect += "?" + Request.QueryString;
            }
            IEncryptedParameters EncryptedParameters = new EncryptedParameters();

            EncryptedParameters[Guid.NewGuid().ToString()] = DateTime.Now.ToUniversalTime().ToString();
            EncryptedParameters["LoggedOutFrom"] = redirect;

            context.Result = new RedirectResult("~/Logout.aspx?redirect=" + EncryptedParameters.AsMvcUrlParameter);

        }

    }

}
