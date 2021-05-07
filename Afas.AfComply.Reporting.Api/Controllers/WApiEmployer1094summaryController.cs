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

namespace Afas.AfComply.Reporting.Api.Controllers
{
    public class WApiEmployer1094summaryController : BaseWebApiController
    {
        private IV1094InitialPart1Service Part1Service { get; set; }
        private IFinalize1094Service Finalize1094Service { get; set; }
        private IV1094InitialPart2Service Part2Service { get; set; }
        private IV1094InitialPart3Service Part3Service { get; set; }
        private IV1094InitialPart4Service Part4Service { get; set; }
        public WApiEmployer1094summaryController(
            ILogger logger,
            ITransactionContext transactionContext,
            IV1094InitialPart1Service Part1Service,
            IV1094InitialPart2Service Part2Service,
            IV1094InitialPart3Service Part3Service,
            IFinalize1094Service Finalize1094Service,
            IV1094InitialPart4Service Part4Service
           
            ) : base(logger)
        {
            if (null == Part1Service)
            {
                throw new ArgumentNullException("Part1Service");
            }
            this.Part1Service = Part1Service;
            this.Part1Service.Context = transactionContext;
            if (null == Part2Service)
            {
                throw new ArgumentNullException("Part2Service");
            }
            this.Part2Service = Part2Service;

            if (null == Part3Service)
            {
                throw new ArgumentNullException("Part3Service");
            }
            this.Part3Service = Part3Service;

            if (null == Part4Service)
            {
                throw new ArgumentNullException("Part4Service");
            }
            this.Part4Service = Part4Service;

            if (null == Finalize1094Service)
            {
                throw new ArgumentNullException("Finalize1095Service");
            }
            this.Finalize1094Service = Finalize1094Service;
            this.Finalize1094Service.Context = transactionContext;

        }

        [HttpPost]
        public HttpResponseMessage GetEmployers1094DetailsForTaxYear(EmployerTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<Employer1094SummaryModel> responseMessage = new ModelListResponse<Employer1094SummaryModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                this.Log.Info("Getting the Entity");

                IList<Employer1094SummaryModel> models = new List<Employer1094SummaryModel>();

                Employer1094SummaryModel model = _1094MonthlyDetails.GetEmployee1094summaryModel(request.EmployerId,request.TaxYear);

                models.Add(model);

                responseMessage.Models = models;

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

        [HttpPost]
        public HttpResponseMessage Finalize1094(EmployerTaxYearRequest request)
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

                this.Log.Info("Finalizing the Entities");
                Employer1094SummaryModel models = _1094MonthlyDetails.GetEmployee1094summaryModel(request.EmployerId, request.TaxYear);
                
                var viewModels = Mapper.Map<Employer1094SummaryModel, Approved1094FinalPart1>(models);

                Finalize1094Service.AddNewEntity(viewModels, request.Requester);
                //Finalize1094Service.SaveApproved1094(viewModels, request.EmployerId, request.TaxYear, request.Requester);
                _1094MonthlyDetails.CheckAndChangeEmployerTaxYearTransmissionStatus(request.EmployerId, request.TaxYear);



                responseMessage.UIMessage = "Finalized 1094 Form.";


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
        public HttpResponseMessage GetAllActiveEmployers(EmployerTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<EmployerModel> responseMessage = new ModelListResponse<EmployerModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<String>()
                {
                }
            };

            try
            {

                this.Log.Info("Finalizing the Entities");

                IList<EmployerModel> models = new List<EmployerModel>();
               models = _1094MonthlyDetails.getAllEmployers();
           
               responseMessage.Models = models;


                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseMessage);
                return response;

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during 1094 monthly details GetAllActiveEmployers retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }

        }
    }
}