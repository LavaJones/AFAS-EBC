using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Core.Response;
using Afc.Core.Application;
using Afc.Core.Logging;
using Afc.Marketing.Framework.WebApi.Controllers;
using Afc.Marketing.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using AutoMapper;
using System.Diagnostics;
using Afas.AfComply.Reporting.Domain.Approvals.FileCabinet;
using Afas.AfComply.Reporting.Application.FileCabinetServices;
using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afc.Core;

namespace Afas.AfComply.Reporting.Api.Controllers.FileCabinet
{
    public class WApiFileCabinetInfoController : BaseWebApiController
    {
        private IFileCabinetInfoService FileCabinetInfoService { get; set; }
        private IFileCabinetAccessService FileCabinetAccessService { get; set; }
        private IFileCabinetFolderInfoService FileCabinetFolderInfoService { get; set; }
        public WApiFileCabinetInfoController(
            ILogger logger, ITransactionContext transactionContext,
            IFileCabinetInfoService FileCabinetInfoService,
            IFileCabinetAccessService FileCabinetAccessService,
            IFileCabinetFolderInfoService FileCabinetFolderInfoService
            ) : base(logger)
        {
            if (null == FileCabinetInfoService)
            {
                throw new ArgumentNullException("FileCabinetInfoService");
            }
            this.FileCabinetInfoService = FileCabinetInfoService;
            this.FileCabinetInfoService.Context = transactionContext;
            if (null == FileCabinetAccessService)
            {
                throw new ArgumentNullException("FileCabinetAccessService");
            }
            this.FileCabinetAccessService = FileCabinetAccessService;
            this.FileCabinetAccessService.Context = transactionContext;

            if (null == FileCabinetFolderInfoService)
            {
                throw new ArgumentNullException("FileCabinetFolderInfoService");
            }
            this.FileCabinetFolderInfoService = FileCabinetFolderInfoService;
            this.FileCabinetFolderInfoService.Context = transactionContext;
        }




        [HttpPost]
        public HttpResponseMessage GetFilesForParticularFolder(FileCabinetFilesRequests request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<FileCabinetInfoModel> responseMessage = new ModelListResponse<FileCabinetInfoModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                this.Log.Info("Getting the Entity");

                IList<FileCabinetInfoModel> models = new List<FileCabinetInfoModel>();
                FileCabinetFolderInfo folder = FileCabinetAccessService.GetByResourceId(request.ResourceId).FileCabinetFolderInfo;
                //ResourceId is a Guid of a Selected folder from the Folder Styructure
                //OwnerResourceid is Guid of Employer.
                List<FileCabinetInfo> Temp = FileCabinetInfoService.GetFilesForFolderByResourceId(folder.ResourceId, request.OwnerResourceId);
                IList<FileCabinetInfoModel> model = Mapper.Map<IList<FileCabinetInfo>, IList<FileCabinetInfoModel>>(Temp);
                responseMessage.Models = model;
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseMessage);
                return response;
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

        private static int CurrentApplicationId = 1;

        [HttpPost]
        public HttpResponseMessage GetFilesInFolders(FileCabinetFilesRequests request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<FileCabinetAccessModel> responseMessage = new ModelListResponse<FileCabinetAccessModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };
            try
            {
                this.Log.Info("Getting the Entity");

                IList<FileCabinetAccessModel> models = new List<FileCabinetAccessModel>();
                List<FileCabinetAccess> Temp = FileCabinetAccessService.GetByOwnerGuid(request.ResourceId, CurrentApplicationId);
                IList<FileCabinetAccessModel> model = Mapper.Map<IList<FileCabinetAccess>, IList<FileCabinetAccessModel>>(Temp);

                responseMessage.Models = model;
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseMessage);
                return response;
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

        // To do later when we launch in different application
        //[HttpPost]
        // public HttpResponseMessage GetByApplicationId(FileCabinetFilesRequests request)
        //{
        //    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        //    stopWatch.Start();
        //    ModelListResponse<FileCabinetAccessModel> responseMessage = new ModelListResponse<FileCabinetAccessModel>()
        //    {

        //        StatusEnum = ResponseMessageStatus.OK,
        //        Errors = new List<String>()
        //        {
        //        }
        //    };
        //    try
        //    {
        //        this.Log.Info("Getting the Entity");
        //        IList<FileCabinetAccessModel> models = new List<FileCabinetAccessModel>();
        //        var Temp = FileCabinetAccessService.GetByApplicationID(request.ApplicationId);
        //        IList<FileCabinetAccessModel> model = Mapper.Map<IList<FileCabinetAccess>, IList<FileCabinetAccessModel>>(Temp);
        //        responseMessage.Models = model;
        //        stopWatch.Stop();
        //        responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseMessage);
        //        return response;

        //    }


        //    catch (Exception exception)
        //    {

        //        this.Log.Error("Errors during service retrieval.", exception);

        //        responseMessage.StatusEnum = ResponseMessageStatus.Error;
        //        responseMessage.Errors.Add(exception.ToString());
        //        stopWatch.Stop();
        //        responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

        //        return Request.CreateResponse(HttpStatusCode.OK, responseMessage);

        //    }

        //}

        // To do later when we launch in different application
        // [HttpPost]
        //public HttpResponseMessage GetByOwnerGuid(GetByGuidRequest request)
        //{
        //    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        //    stopWatch.Start();
        //    ModelListResponse<FileCabinetAccessModel> responseMessage = new ModelListResponse<FileCabinetAccessModel>()
        //    {

        //        StatusEnum = ResponseMessageStatus.OK,
        //        Errors = new List<String>()
        //        {
        //        }
        //    };
        //    try
        //    {
        //        this.Log.Info("Getting the Entity");
        //        IList<FileCabinetAccessModel> models = new List<FileCabinetAccessModel>();
        //        var Temp = FileCabinetAccessService.GetByOwnerGuid(request.ResourceId, CurrentApplicationId);
        //        IList<FileCabinetAccessModel> model = Mapper.Map<IList<FileCabinetAccess>, IList<FileCabinetAccessModel>>(Temp);
        //        responseMessage.Models = model;
        //        stopWatch.Stop();
        //        responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseMessage);
        //        return response;

        //    }


        //    catch (Exception exception)
        //    {

        //        this.Log.Error("Errors during service retrieval.", exception);

        //        responseMessage.StatusEnum = ResponseMessageStatus.Error;
        //        responseMessage.Errors.Add(exception.ToString());
        //        stopWatch.Stop();
        //        responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

        //        return Request.CreateResponse(HttpStatusCode.OK, responseMessage);

        //    }
        //}


        [HttpPost]
        public HttpResponseMessage GetByResourceId(GetByGuidRequest request)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            ModelItemResponse<FileCabinetInfoModel> responseMessage = new ModelItemResponse<FileCabinetInfoModel>()
            {

                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                this.Log.Info("Getting the Entity");

                FileCabinetInfo entity = this.FileCabinetInfoService.GetByResourceId(request.ResourceId);

                FileCabinetInfoModel model = Mapper.Map<FileCabinetInfo, FileCabinetInfoModel>(entity);

                responseMessage.Model = model;

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

        [HttpPost]
        public HttpResponseMessage SaveFileCabinetInfo(NewChildItemRequest<FileCabinetInfoModel> request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<FileCabinetInfoModel> responseMessage = new ModelListResponse<FileCabinetInfoModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };
            try
            {

                this.Log.Info("Getting the Entity");


                FileCabinetInfo converted = Mapper.Map<FileCabinetInfoModel, FileCabinetInfo>(request.model);
                FileCabinetFolderInfo folder = FileCabinetAccessService.GetByResourceId(request.ResourceId).FileCabinetFolderInfo;
                converted.FileCabinetFolderInfo = folder;
                FileCabinetInfoService.SaveFileCabinetInfo(converted, request.Requester);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseMessage);

                return response;


            }
            catch (Exception exception)
            {
                this.Log.Error("Errors during saving.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteFileCabinetInfo(DeleteItemRequest<FileCabinetInfoModel> requestMessage)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            ModelItemResponse<FileCabinetInfoModel> responseMessage = new ModelItemResponse<FileCabinetInfoModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
            };

            try
            {
                String authorizingUser = requestMessage.Requester;

                FileCabinetInfo removedEntity = FileCabinetInfoService.DeactivateEntity(
                    requestMessage.ResourceId,
                    authorizingUser);

                SharedUtilities.VerifyObjectParameter(removedEntity, "RemovedEntity");

                FileCabinetInfoModel removed = Mapper.Map<FileCabinetInfo, FileCabinetInfoModel>(removedEntity);

                responseMessage.Model = removed;

                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        responseMessage
                    );
            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service action.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());

                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        responseMessage
                    );

            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteFolderFileCabinetInfo(FileCabinetFilesRequests request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ModelListResponse<FileCabinetInfoModel> responseMessage = new ModelListResponse<FileCabinetInfoModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                this.Log.Info("Deactivating the folder");

                //request.ResourceId is the ResourceId of the FileCabient Folder Access
                FileCabinetFolderInfo folder = FileCabinetAccessService.GetByResourceId(request.ResourceId).FileCabinetFolderInfo;
                
                //folder.ResourceId is a Guid of a Selected folder from the Folder Styructure
                //OwnerResourceid is Guid of Employer.
                //Requester who edited the data
                FileCabinetInfoService.DeactivateFolderInFileCabinetInfo(folder.ResourceId, request.OwnerResourceId, request.Requester);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseMessage);

                return response;

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service action.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());

                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(
                        HttpStatusCode.OK,
                        responseMessage
                    );

            }

        }
    }
}