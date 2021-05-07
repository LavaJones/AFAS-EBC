using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Afc.Core;
using Afc.Core.Logging;
using Afc.Core.Presentation.Web;
using Afas.AfComply.Reporting.Core.Response;
using Afc.Marketing.Response;
using Afc.Marketing;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using AutoMapper;
using Afas.AfComply.UI.Areas.ViewModels;

namespace Afas.AfComply.UI.Areas.Administration.Controllers
{

    [CookieTokenAdminAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class QlikController : BaseMvcController
    {

        public QlikController(
                ILogger logger,
                IEncryptedParameters encryptedParameters
            ) 
            : base(logger, encryptedParameters)
        {



        }

        public ActionResult Index()
        {
            return View();
        }

    }

}