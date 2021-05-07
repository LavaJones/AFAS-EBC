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

namespace Afas.AfComply.Reporting.Api.Controllers
{
    public class WApiTimeFrameController : BaseCrudWApiController<TimeFrame, TimeFrameModel>
    {

        public WApiTimeFrameController(
            ILogger log,
            ITransactionContext transactionContext,
            ITimeFrameService service)
            : base(log, transactionContext, service)
        {
        }

        [HttpGet]
        public HttpResponseMessage GetForYear(TimeFramesForYearRequest request)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            ModelListResponse<TimeFrameModel> responseMessage = new ModelListResponse<TimeFrameModel>()
            {

                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                this.Log.Info("Getting the Entity");

                IList<TimeFrame> entity = ((ITimeFrameService)this.Service)
                    .GetTimeFramesByYear(request.Year).ToList();

                IList<TimeFrameModel> models = Mapper.Map<IList<TimeFrame>, IList<TimeFrameModel>>(entity);

                responseMessage.Models = models;

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