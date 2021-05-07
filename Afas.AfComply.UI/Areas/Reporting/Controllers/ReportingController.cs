using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Afc.Core.Logging;
using Afc.Core.Presentation.Web;
using System.Web.Mvc;

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ReportingController : BaseMvcController
    {
        public ReportingController(ILogger logger, IEncryptedParameters encryptedParameters) : 
            base(logger, encryptedParameters)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}