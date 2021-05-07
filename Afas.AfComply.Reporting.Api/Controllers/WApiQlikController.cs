using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Core.Response;
using Afas.AfComply.Reporting.Domain.TimeFrames;
using Afc.Core.Application;
using Afc.Core.Logging;
using Afc.Marketing.Framework.WebApi.Controllers;
using Afc.Marketing.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Afc.Core;
using System.Net;
using AutoMapper;
using System.Web.Configuration;
using System.Security.Cryptography.X509Certificates;
using Afas.Application;
using Afc.Web.Reporting.Application;

namespace Afas.AfComply.Reporting.Api.Controllers
{
    public class WApiQlikController : BaseWebApiController
    {

        public WApiQlikController(ILogger log)
            : base(log)
        {
        }

        [HttpGet]
        public HttpResponseMessage GetKlikUrl()
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            UIMessageResponse responseMessage = new UIMessageResponse()
            {

                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                this.Log.Info("Getting the Iframe URL");




                try
                {

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    Log.Info("This is what StoreLocation it's using" + WebConfigurationManager.AppSettings["Feature.StoreLocation"]);

                    var certLoc = WebConfigurationManager.AppSettings["Feature.StoreLocation"].Equals("LocalMachine", StringComparison.CurrentCultureIgnoreCase) ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
                    Log.Info("This is what cert using" + certLoc.ToString()+ " Thumbprint: "+ WebConfigurationManager.AppSettings["Feature.ThumbPrint"]);
                    var certificateManager = new LocalUserCertificateManager(
                     WebConfigurationManager.AppSettings["Feature.ThumbPrint"], certLoc);
                    Log.Info("This is what cert using" + certLoc.ToString() + WebConfigurationManager.AppSettings["Feature.ThumbPrint"]);
                    // WebConfigurationManager.AppSettings["Feature.StoreLocation"].Equals("LocalMachine", StringComparison.CurrentCultureIgnoreCase) ? StoreLocation.LocalMachine : StoreLocation.CurrentUser);

                    var qlikHelper = new QlikIframeHelper(
                             WebConfigurationManager.AppSettings["Feature.QlikUrl"],
                             WebConfigurationManager.AppSettings["Feature.QlikLocation"],
                             certificateManager,
                             (sender, certificate, chain, error) => { return true; });


                    responseMessage.UIMessage = qlikHelper.GetIframeUrl(WebConfigurationManager.AppSettings["Feature.QlikUser"], WebConfigurationManager.AppSettings["Feature.QlikUserDirectory"]);


                }
                catch (Exception ex)
                {

                    Log.Warn("Caught Exception While conneting to Qlik", ex);

                }


                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());

                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }

        }

    }

}