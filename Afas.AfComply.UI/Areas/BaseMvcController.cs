using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

using Afc.Core;
using Afc.Core.Logging;
using Afc.Core.Presentation.Web;

namespace Afas.AfComply.UI.Areas
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public abstract class BaseMvcController : Controller, ILoggingContext, ILoggingContextAware
    {

        [CookieTokenAuthCheckAttribute]
        public BaseMvcController(ILogger logger, IEncryptedParameters encryptedParameters) : base()
        {

            SharedUtilities.VerifyObjectParameter(logger, "Logger");
            SharedUtilities.VerifyObjectParameter(encryptedParameters, "EncryptedParameters");

            this.EncryptedParameters = encryptedParameters;

            (this.EncryptedParameters as ILoggingContextAware).Log =
                    Afc.Core.Logging.InternalLogManager.GetLogger(typeof(Afc.Framework.Presentation.Web.EncryptedParameters));

            (this as ILoggingContextAware).Log = logger;

        }

        /// <summary>
        /// Ensures all inbound posts have CSRF checking enabled.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                var cookieValue = filterContext.HttpContext.Request.Cookies["__RequestVerificationToken"].Value;
                var headerValue = filterContext.HttpContext.Request.Headers["__RequestVerificationToken"];
                AntiForgery.Validate(cookieValue, headerValue);
            }

            base.OnAuthorization(filterContext);

        }

        /// <summary>
        /// Route exceptions to the log file.
        /// </summary>
        protected override void OnException(ExceptionContext filterContext)
        {

            Exception exception = filterContext.Exception;

            this.Log.Error("Errors during controller action.", exception);

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error"
            };

        }

        /// <summary>
        /// This will take the encryptedPayload from the MVC URL or the QueryString and 
        /// populate the IEncryptedParameters instance. If the IEncryptedParameters:Load fails it will raise
        /// a Server 500 error. The IEncryptedParameters::Load should not fail if the MVC
        /// route is properly defined with the RouteConstraint.
        /// </summary>
        /// <returns></returns>
        protected void Load(String encryptedPayload)
        {

            SharedUtilities.VerifyStringParameter(encryptedPayload, "EncryptedPayload");

            EncryptedParameters.Reset();

            if (EncryptedParameters.Load(encryptedPayload) == false)
            {
                throw new Exception("Load failure of an encrypted payload. Route misconfigured?");
            }

        }

        Boolean ILoggingContext.IsSet
        {

            get
            {
                return (this.Log != null);
            }

        }

        ILogger ILoggingContextAware.Log
        {

            set
            {

                Afc.Core.SharedUtilities.VerifyObjectParameter(value, "Log");

                this.Log = value;

            }

        }

        protected IEncryptedParameters EncryptedParameters { get; private set; }
        protected ILogger Log { get; private set; }

    }
}