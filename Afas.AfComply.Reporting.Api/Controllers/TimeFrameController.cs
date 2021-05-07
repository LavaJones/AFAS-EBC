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
using Afas.AfComply.Reporting.Api.Models;
using Afc.Core;
using System.Net;

namespace Afas.AfComply.Reporting.Api.Controllers
{
    public class TimeFrameController : BaseWebApiController
    {
        protected ITimeFrameService TimeFrameService { get; private set; }

        public TimeFrameController(
            ILogger log,
            ITransactionContext transactionContext,
            ITimeFrameService timeFrameService)
            : base(log)
        {

            Afc.Core.SharedUtilities.VerifyObjectParameter(transactionContext, "TransactionContext");
            Afc.Core.SharedUtilities.VerifyObjectParameter(timeFrameService, "TimeFrameService");

            this.TimeFrameService = timeFrameService;
            this.TimeFrameService.Context = transactionContext;

        }

        [HttpGet]
        public HttpResponseMessage GetTimeFrameById(Guid ResourceId)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            TimeFramesMessage timeframeMessage = new TimeFramesMessage()
            {

                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {
                List<TimeFrameModel> frames = new List<TimeFrameModel>();

                this.Log.Info("Getting the Timeframe");
                TimeFrame timeFrame = this.TimeFrameService.GetByResourceId(ResourceId);
                if (timeFrame != null)
                {
                    TimeFrameModel model = new TimeFrameModel()
                    {
                        Year = timeFrame.Year,
                        Month = timeFrame.Month,
                        ResourceId = timeFrame.ResourceId,
                        ResourceStatus = timeFrame.EntityStatus.ToString()
                    };

                    frames.Add(model);
                }

                timeframeMessage.TimeFrames = frames;

                return Request.CreateResponse(HttpStatusCode.OK, timeframeMessage);

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                timeframeMessage.StatusEnum = ResponseMessageStatus.Error;
                timeframeMessage.Errors.Add(exception.ToString());

                stopWatch.Stop();
                timeframeMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(HttpStatusCode.OK, timeframeMessage);

            }

        }

        [HttpGet]
        public HttpResponseMessage GetAllTimeFrames()
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            TimeFramesMessage timeframeMessage = new TimeFramesMessage()
            {

                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {
                List<TimeFrameModel> frames = new List<TimeFrameModel>();

                this.Log.Info("Getting the Timeframe");
                IList<TimeFrame> timeFrames = this.TimeFrameService.GetAllTimeFrames().ToList();
                if (timeFrames != null)
                {
                    foreach (TimeFrame timeFrame in timeFrames)
                    {
                        TimeFrameModel model = new TimeFrameModel()
                        {
                            Year = timeFrame.Year,
                            Month = timeFrame.Month,
                            ResourceId = timeFrame.ResourceId,
                            ResourceStatus = timeFrame.EntityStatus.ToString()
                        };

                        frames.Add(model);
                    }
                }

                timeframeMessage.TimeFrames = frames;

                return Request.CreateResponse(HttpStatusCode.OK, timeframeMessage);

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                timeframeMessage.StatusEnum = ResponseMessageStatus.Error;
                timeframeMessage.Errors.Add(exception.ToString());

                stopWatch.Stop();
                timeframeMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(HttpStatusCode.OK, timeframeMessage);

            }

        }

        [HttpPost]
        public HttpResponseMessage AddNewTimeFrame(NewTimeFrameRequest newTimeFrameRequest)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            TimeFramesMessage addedTimeFrameMessage = new TimeFramesMessage()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
            };

            try
            {
                String authorizingUser = newTimeFrameRequest.Requester;

                TimeFrame newTimeFrame = TimeFrameService.AddNewTimeFrame(
                    newTimeFrameRequest.Year,
                    newTimeFrameRequest.Month,
                    authorizingUser);

                SharedUtilities.VerifyObjectParameter(newTimeFrame, "NewTimeFrame");

                IList<TimeFrameModel> models = new List<TimeFrameModel>();
                models.Add(newTimeFrame.ToModel());
                addedTimeFrameMessage.TimeFrames = models;

                stopWatch.Stop();
                addedTimeFrameMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        addedTimeFrameMessage
                    );
            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service action.", exception);

                addedTimeFrameMessage.StatusEnum = ResponseMessageStatus.Error;
                addedTimeFrameMessage.Errors.Add(exception.ToString());

                stopWatch.Stop();
                addedTimeFrameMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        addedTimeFrameMessage
                    );

            }
        }


        [HttpPost]
        public HttpResponseMessage UpdateTimeFrame(UpdateTimeFrameRequest updateTimeFrameRequest)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            TimeFramesMessage addedTimeFrameMessage = new TimeFramesMessage()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
            };

            try
            {

                String authorizingUser = updateTimeFrameRequest.Requester;

                TimeFrame updatedTimeFrame = TimeFrameService.GetByResourceId(
                    updateTimeFrameRequest.model.ResourceId);

                SharedUtilities.VerifyObjectParameter(updatedTimeFrame, "UpdatedTimeFrame");

                TimeFrame removedTimeFrame = TimeFrameService.UpdateTimeFrame(
                   updateTimeFrameRequest.model.ToEntity(),
                   authorizingUser);
                
                updatedTimeFrame = TimeFrameService.GetByResourceId(
                    updateTimeFrameRequest.model.ResourceId);

                IList<TimeFrameModel> models = new List<TimeFrameModel>();
                models.Add(updatedTimeFrame.ToModel());
                addedTimeFrameMessage.TimeFrames = models;

                stopWatch.Stop();
                addedTimeFrameMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        addedTimeFrameMessage
                    );

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service action.", exception);

                addedTimeFrameMessage.StatusEnum = ResponseMessageStatus.Error;
                addedTimeFrameMessage.Errors.Add(exception.ToString());

                stopWatch.Stop();
                addedTimeFrameMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        addedTimeFrameMessage
                    );

            }
        }


        [HttpPost]
        public HttpResponseMessage DeleteTimeFrame(DeleteTimeFrameRequest deleteTimeFrameRequest)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            TimeFramesMessage deleteTimeFrameMessage = new TimeFramesMessage()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
            };

            try
            {
                String authorizingUser = deleteTimeFrameRequest.Requester;

                TimeFrame removedTimeFrame = TimeFrameService.DeactivateTimeFrame(
                    deleteTimeFrameRequest.ResourceId,
                    authorizingUser);

                SharedUtilities.VerifyObjectParameter(removedTimeFrame, "RemovedTimeFrame");

                IList<TimeFrameModel> models = new List<TimeFrameModel>();
                models.Add(removedTimeFrame.ToModel());
                deleteTimeFrameMessage.TimeFrames = models;

                stopWatch.Stop();
                deleteTimeFrameMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        deleteTimeFrameMessage
                    );
            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service action.", exception);

                deleteTimeFrameMessage.StatusEnum = ResponseMessageStatus.Error;
                deleteTimeFrameMessage.Errors.Add(exception.ToString());

                stopWatch.Stop();
                deleteTimeFrameMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        deleteTimeFrameMessage
                    );

            }
        }
    }
}