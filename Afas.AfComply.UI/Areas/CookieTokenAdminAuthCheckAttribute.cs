using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using System.Configuration;

using Afc.Core.Logging;
using Afc.Framework.Presentation.Web;
using Afc.Core.Presentation.Web;
using Afas.Domain;

namespace Afas.AfComply.UI.Areas
{

    /// <summary>
    /// This Attribute Handles login to the system admin portion of the site
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class CookieTokenAdminAuthCheckAttribute : CookieTokenAuthCheckAttribute
    {

        /// <summary>
        /// Gets if the User is a System Admin from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>True if the User is a System Admin</returns>
        public static bool GetIsAdmin(HttpContext httpContext)
        {
            string isAdmin = GetObjectByKey(new System.Web.HttpContextWrapper(httpContext), "IsAdmin");
            if (isAdmin.IsNullOrEmpty())
            {
                return false;
            }
            return bool.Parse(isAdmin);
        }

        /// <summary>
        /// Gets if the User is a System Admin from the Cookie
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>True if the User is a System Admin</returns>
        public static bool GetIsAdmin(HttpContextBase httpContext)
        {
            string isAdmin = GetObjectByKey(httpContext, "IsAdmin");
            if (isAdmin.IsNullOrEmpty())
            {
                return false;
            }
            return bool.Parse(isAdmin);
        }

        /// <summary>
        /// Simple core to the authorization that checks if the cookie is valid and the employer is set to ID 1 and the user is Admin
        /// </summary>
        /// <param name="httpContext">The context containing the cookie</param>
        /// <returns>True if the Cookie is Valid and the employer is set to ID 1 and the user is Admin</returns>
        protected override Boolean AuthorizeCore(HttpContextBase httpContext)
        {

            string userId = GetUserId(httpContext);

            if (userId.IsNullOrEmpty())
            {
                return false;
            }

            string EmployerId = GetEmployerId(httpContext);
            if (EmployerId == null || EmployerId != "1")
            {
                return false;
            }

            bool IsAdmin = GetIsAdmin(httpContext);
            if (false == IsAdmin)
            {
                return false;
            }

            return true;           
               
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
