using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using log4net;

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{
    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class AuthenticationController : Controller
    {
        private static ILog Log = LogManager.GetLogger(typeof(CookieTokenAuthCheckAttribute));

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public string GetAntiForgeryToken()
        {
            if (HttpContext == null || HttpContext.Request == null || HttpContext.Request.Cookies["__RequestVerificationToken"] == null)
            {
                Log.Warn("Authentication Controller ran into null, probably the cookie. " + HttpContext.Request.Cookies["__RequestVerificationToken"]);

                return null;
            }

            string cookieValue = HttpContext.Request.Cookies["__RequestVerificationToken"].Value;
            string cookieToken;
            string formToken;

            System.Web.Helpers.AntiForgery.GetTokens(cookieValue, out cookieToken, out formToken);

            HttpContext.Request.Cookies["__RequestVerificationToken"].Value = cookieToken;

            return formToken;

        }
    }
}