using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Core.Response;
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
using Afc.Core.Domain;
using Afc.Marketing.Models;
using Afas.AfComply.Reporting.Domain;
using Afas.Application;

namespace Afas.AfComply.Reporting.Api.Controllers
{
    public class BaseCrudWApiController<TEntity, TModel> : BaseWebApiController
        where TEntity : BaseReportingModel
        where TModel : Model
    {
        protected ICrudDomainService<TEntity> Service { get; private set; }

        public BaseCrudWApiController(
            ILogger log,
            ITransactionContext transactionContext,
            ICrudDomainService<TEntity> service)
            : base(log)
        {

            Afc.Core.SharedUtilities.VerifyObjectParameter(transactionContext, "TransactionContext");
            Afc.Core.SharedUtilities.VerifyObjectParameter(service, "Service");

            this.Service = service;
            this.Service.Context = transactionContext;

        }

        [HttpGet]
        public HttpResponseMessage GetById(Guid ResourceId)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            ModelItemResponse<TModel> responseMessage = new ModelItemResponse<TModel>()
            {

                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                this.Log.Info("Getting the Entity");

                TEntity entity = this.Service.GetByResourceId(ResourceId);

                TModel model = Mapper.Map<TEntity, TModel>(entity);
                
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

        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            ModelListResponse<TModel> responseMessage = new ModelListResponse<TModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {
                this.Log.Info("Getting the Entities");

                List<TEntity> entites = this.Service.GetAllEntities().ToList();

                IList<TModel> frames = Mapper.Map<List<TEntity>, List<TModel>>(entites);
                
                responseMessage.Models = frames;

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

        [HttpPost]
        public HttpResponseMessage AddNew(NewItemRequest<TModel> requestMessage)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            ModelItemResponse<TModel> responseMessage = new ModelItemResponse<TModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
            };

            try
            {
                String authorizingUser = requestMessage.Requester;

                TEntity entity = Mapper.Map<TEntity>(requestMessage.model);

                entity = Service.AddNewEntity(
                    entity,
                    authorizingUser);
                
                SharedUtilities.VerifyObjectParameter(entity, "NewEntity");

                TModel model = Mapper.Map<TModel>(entity);
                
                responseMessage.Model = model;

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
        public HttpResponseMessage Update(UpdateItemRequest<TModel> requestMessage)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            ModelItemResponse<TModel> responseMessage = new ModelItemResponse<TModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
            };

            try
            {

                String authorizingUser = requestMessage.Requester;

                TEntity toUpdate = Mapper.Map<TModel, TEntity> (requestMessage.model);

                SharedUtilities.VerifyObjectParameter(toUpdate, "UpdatedEntity");

                TEntity updatedEntity = Service.UpdateEntity(
                   toUpdate,
                   authorizingUser);
          
                TModel updated = Mapper.Map<TEntity, TModel>(updatedEntity);

                responseMessage.Model = updated;

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
        public HttpResponseMessage Delete(DeleteItemRequest<TModel> requestMessage)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            ModelItemResponse<TModel> responseMessage = new ModelItemResponse<TModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
            };

            try
            {
                String authorizingUser = requestMessage.Requester;

                TEntity removedEntity = Service.DeactivateEntity(
                    requestMessage.ResourceId,
                    authorizingUser);

                SharedUtilities.VerifyObjectParameter(removedEntity, "RemovedEntity");
                
                TModel removed = Mapper.Map<TEntity, TModel>(removedEntity);
                
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
    }
}