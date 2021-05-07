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
using Afas.AfComply.Reporting.Domain.Printing;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Application.Services;
using Afas.Application.Archiver;
using Afas.AfComply.Reporting.Api.App_Start;

namespace Afas.AfComply.Reporting.Api.Controllers
{
    public class WApiPrintBatchController : BaseWebApiController  
    {

        private IFileArchiver Archiver;

        private IFinalize1095Service FinalService;

        public WApiPrintBatchController(
            ILogger log,
            ITransactionContext transactionContext,
            IPrintBatchService service,
            IFileArchiver archiver,
            IFinalize1095Service finalService
            )
            : base(log)
        {

            if (null == archiver)
            {
                throw new ArgumentException("Archiver");
            }
            this.Archiver = archiver;

            if (null == transactionContext)
            {
                throw new ArgumentException("transactionContext");
            }

            if (null == finalService)
            {
                throw new ArgumentException("FinalService");
            }
            this.FinalService = finalService;
            this.FinalService.Context = transactionContext;


        }

        [HttpPost]
        public HttpResponseMessage Print1095(Employee1095PrintRequests request)       
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            BasicResponse responseMessage = new BasicResponse()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {

                }
            };

            try
            {
                this.Log.Info("Print called");


            }
            catch (Exception exception)
            {
                this.Log.Error("Errors during updating Part3 Insrance Coverage in WApiEmployee1095summaryController.", exception);
                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }
            stopWatch.Stop();
            responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
            return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
        }
    }
    
}