using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Afas.AfComply.Domain;
using Afc.Core;
using Afc.Core.Logging;
using Afc.Core.Presentation.Web;
using Afc.Marketing;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.UI.Areas.Administration.Controllers;
using Afas.AfComply.UI.Areas.ViewModels.Reporting;
using System.Web.UI;
using Afas.AfComply.Reporting.Application;
using Afc.Core.Application;
using System.Globalization;
using System.Configuration;
using System.Web.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Afas.AfComply.Reporting.Core.Response;
using Afc.Web.Reporting.Application;

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class QlikController : BaseMvcController
    {

        private string GetAllKey;
        IApiHelper ApiHelper;
        public QlikController(ILogger logger,
                IEncryptedParameters encryptedParameters,
                IApiHelper apiHelper
            )
            : base(logger, encryptedParameters)
        {


            this.GetAllKey = "get-all-qlik";

            if (null == apiHelper)
            {
                throw new ArgumentNullException("apiHelper");
            }
            this.ApiHelper = apiHelper;


        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public ActionResult GetAll()
        {

            try { 
                UIMessageResponse message = this.ApiHelper.Retrieve<UIMessageResponse>(GetAllKey);

                Log.Info("WebAPI Returned IFrame url: " + message.UIMessage);
            }
            catch (Exception ex)
            {

                Log.Warn("Caught Exception When Connecting to Qlik from WebAPI for Iframe", ex);

            }

            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var certLoc = WebConfigurationManager.AppSettings["Feature.StoreLocation"].Equals("LocalMachine", StringComparison.CurrentCultureIgnoreCase) ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
                Log.Info("This is what cert using" + certLoc.ToString() + WebConfigurationManager.AppSettings["Feature.ThumbPrint"]);
                var certificateManager = new LocalUserCertificateManager(
                 WebConfigurationManager.AppSettings["Feature.ThumbPrint"], certLoc);
                Log.Info("This is what cert using" + certLoc.ToString() + WebConfigurationManager.AppSettings["Feature.ThumbPrint"]);
                var qlikHelper = new QlikIframeHelper(
                         WebConfigurationManager.AppSettings["Feature.QlikUrl"],
                         WebConfigurationManager.AppSettings["Feature.QlikLocation"],
                         certificateManager,
                         (sender, certificate, chain, error) => { return true; });
                
                var url = qlikHelper.GetIframeUrl(WebConfigurationManager.AppSettings["Feature.QlikUser"], WebConfigurationManager.AppSettings["Feature.QlikUserDirectory"]);
                Log.Info("MVC Returned IFrame url: " + url);

            }
            catch (Exception ex)
            {

                Log.Warn("Caught Exception When COnnecting to Qlik for Iframe", ex);


            }


            return null;
        }
    }
}