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
    public class TimeFrameController : BaseCrudController<TimeFrameModel, TimeFrameViewModel>
    {

        public TimeFrameController(ILogger logger,
                IEncryptedParameters encryptedParameters,
                IApiHelper apiHelper
            ) 
            : base(logger, encryptedParameters, apiHelper)
        {

            GetAllKey = "get-all-timeframes";
            GetManyKey = "get-all-timeframes";
            GetByIdKey = "get-timeframe-by-resource-id";
            GetAddNewKey = "add-new-timeframe";
            GetUpdateKey = "update-timeframe";
            GetDeleteKey = "delete-timeframe";

        }
        
    }
}