using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Afas.AfComply.UI.Areas.Administration.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class AuthenticationController : Controller
    {

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