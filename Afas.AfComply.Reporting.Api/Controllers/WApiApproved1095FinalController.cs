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
using Afas.AfComply.Reporting.Domain.MonthlyDetails;
using Afas.AfComply.Reporting.Application.Services.LegacyServices;
using Afas.AfComply.Reporting.Domain.LegacyData;
using System.Diagnostics;
using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.Domain;

namespace Afas.AfComply.Reporting.Api.Controllers
{
    public class WApiApproved1095FinalController : BaseWebApiController
    {

        private IFinalize1095Service Finalize1095Service { get; set; }

        public IPrintBatchService PrintBatchService { get; set; }

        public IPrint1095Service Print1095Service { get; set; }

        public WApiApproved1095FinalController(
            ILogger log,
            ITransactionContext transactionContext,
            IFinalize1095Service finalize1095Service,
            IPrintBatchService printBatchService,
            IPrint1095Service print1095Service

             )
            : base(log)
        {

            if (null == transactionContext)
            {
                throw new ArgumentException("transactionContext");
            }

            if (null == finalize1095Service)
            {
                throw new ArgumentNullException("Finalize1095Service");
            }
            this.Finalize1095Service = finalize1095Service;
            this.Finalize1095Service.Context = transactionContext;

            if (null == printBatchService)
            {
                throw new ArgumentException("PrintBatchService");
            }
            this.PrintBatchService = printBatchService;
            this.PrintBatchService.Context = transactionContext;

            if (null == print1095Service)
            {
                throw new ArgumentException("print1095Service");
            }
            this.Print1095Service = print1095Service;
            this.Print1095Service.Context = transactionContext;

        }

        [HttpPost]
        public HttpResponseMessage UnFinalizeAll1095(EmployerTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
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

                this.Log.Info("UnFinalizing All the Entities");
                IList<Approved1095Final> models = Finalize1095Service.GetApproved1095sForEmployerTaxYear(request.EmployerId, request.TaxYear);

                foreach (var model in models)
                {
                    model.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;

                    foreach (var part2 in model.part2s)
                    {                        
                        part2.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;
                    }
                    foreach (var part3 in model.part3s)
                    {
                        part3.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;
                    }
                }
                
                Finalize1095Service.SaveApproved1095(models, request.EmployerId, request.TaxYear, request.Requester, true);

                responseMessage.UIMessage = "UnFinalized all [" + models.Count + "] 1095 Forms.";

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

        [HttpPost]
        public HttpResponseMessage UnFinalize1095(SingleEmployee1095summaryForTaxYearRequest request)
        {

            Stopwatch stopWatch = new Stopwatch();
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

                Approved1095Final model = Finalize1095Service.GetByResourceId(request.ResourceId);

                model.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;

                foreach (var part2 in model.part2s)
                {
                    part2.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;
                }
                foreach (var part3 in model.part3s)
                {
                    part3.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Inactive;
                }                

                Finalize1095Service.SaveApproved1095(new List<Approved1095Final>(){ model }, request.EmployerId, request.TaxYear, request.Requester, true);

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
        

        [HttpPost]
        public HttpResponseMessage GetForTaxYear(EmployerTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<Approved1095FinalModel> responseMessage = new ModelListResponse<Approved1095FinalModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                List<Approved1095Final> finals = Finalize1095Service.GetApproved1095sForEmployerTaxYear(request.EmployerId, request.TaxYear);

                // The lookup table should speed access by the ID
                Dictionary<long, Approved1095Final> lookup = finals.CreateLookup<long, Approved1095Final>(item => item.ID);

                // There is a printed flag that needs to be set based on the Print Batches

                IList<long> batchIds = this.PrintBatchService.GetIdsForEmployerTaxYear(request.EmployerId, request.TaxYear);

                IList<long> printed1095Ids = this.Print1095Service.GetApprovedIdsForBatchIds(batchIds);

                foreach (long printedId in printed1095Ids)
                {
                    if (lookup.ContainsKey(printedId))
                    {
                        lookup[printedId].Printed = true;
                    }
                }

                // Now mapp the results to the Model before it is returned
                IList<Approved1095FinalModel> models = Mapper.Map<List<Approved1095Final>, List<Approved1095FinalModel>>(finals);

                responseMessage.Models = models;

                stopWatch.Stop();

                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                HttpResponseMessage response =  Request.CreateResponse(HttpStatusCode.OK, responseMessage);

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
        
    }

}