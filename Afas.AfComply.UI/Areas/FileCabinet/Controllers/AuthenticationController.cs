using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using log4net;

namespace Afas.AfComply.UI.Areas.FileCabinet.Controllers
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
            
            string cookieValue = HttpContext.Request.Cookies["__RequestVerificationToken"].Value;
            string cookieToken;
            string formToken;

            System.Web.Helpers.AntiForgery.GetTokens(cookieValue, out cookieToken, out formToken);

            HttpContext.Request.Cookies["__RequestVerificationToken"].Value = cookieToken;

            return formToken;

        }
    }
}