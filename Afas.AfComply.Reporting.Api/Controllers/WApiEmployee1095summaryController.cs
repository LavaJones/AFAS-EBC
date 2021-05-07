using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Application.Services.LegacyServices;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Core.Response;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.LegacyData;
using Afas.AfComply.Reporting.Domain.MonthlyDetails;
using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.Domain;
using Afc.Core.Application;
using Afc.Core.Logging;
using Afc.Marketing.Framework.WebApi.Controllers;
using Afc.Marketing.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Afas.AfComply.Reporting.Api.Controllers
{
    public class WApiEmployee1095summaryController : BaseWebApiController
    {

        private ILegacyEmployeeService LegacyEmployeeService { get; set; }

        private ILegacyClassifictaionService LegacyClassifictaionService { get; set; }

        private IUserEditPart2Service UserEditService { get; set; }

        private IFinalize1095Service Finalize1095Service { get; set; }

        private IUserReviewedService UserReviewedService { get; set; }

        public WApiEmployee1095summaryController(
            ILogger log,
            ITransactionContext transactionContext,
            IUserEditPart2Service UserEditService,
            ILegacyEmployeeService LegacyEmployeeService,
            ILegacyClassifictaionService LegacyClassifictaionService,
            IFinalize1095Service Finalize1095Service,
            IUserReviewedService UserReviewedService
             )
            : base(log)
        {

            if (null == UserEditService)
            {
                throw new ArgumentNullException("UserEditService");
            }
            this.UserEditService = UserEditService;
            this.UserEditService.Context = transactionContext;

            if (null == Finalize1095Service)
            {
                throw new ArgumentNullException("Finalize1095Service");
            }
            this.Finalize1095Service = Finalize1095Service;
            this.Finalize1095Service.Context = transactionContext;

            if (null == UserReviewedService)
            {
                throw new ArgumentNullException("UserReviewedService");
            }
            this.UserReviewedService = UserReviewedService;
            this.UserReviewedService.Context = transactionContext;

            if (null == LegacyEmployeeService)
            {
                throw new ArgumentNullException("LegacyEmployeeService");
            }
            this.LegacyEmployeeService = LegacyEmployeeService;

            if (null == LegacyClassifictaionService)
            {
                throw new ArgumentNullException("LegacyClassifictaionService");
            }
            this.LegacyClassifictaionService = LegacyClassifictaionService;

        }

        //save part 3
        [HttpPost]
        public HttpResponseMessage UpdateInsuranceCoverage(UpdateItemRequest<Employee1095detailsPart3Model> request)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            ModelItemResponse<Employee1095summaryModel> responseMessage = new ModelItemResponse<Employee1095summaryModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {

                this.Log.Info("Saving Insurance Coverage");
                this.Log.Info(string.Format("Part3 Update in WAPI:EmployeeID: {0},ResourceID: {1}, DependantID: {2} ",
                                         request.model.EmployeeID,
                                         request.model.ResourceId,
                                         request.model.DependantID
                                      )
                              );


                _1095MonthlyDetails.UpdatePart3InsuranceCoverage(request.model, request.Requester);

                employee emp = this.LegacyEmployeeService.GetEmployeeWithId(request.model.EmployeeID);

                responseMessage.Model = _1095MonthlyDetails.GetSingleEmployee1095summaryModel(emp.ResourceId, emp.employer_id, request.model.TaxYear, this.UserEditService, this.UserReviewedService, this.LegacyClassifictaionService);

            }
            catch (Exception exception)
            {
                this.Log.Error("Errors during updating Part3 Insrance Coverage in WApiEmployee1095summaryController.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }
            stopWatch.Stop();
            responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

            return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);
        }

        [HttpPost]
        public HttpResponseMessage DeleteInsuranceCoverage(UpdateItemRequest<Employee1095detailsPart3Model> request)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            BasicResponse responseMessage = new BasicResponse()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {

                this.Log.Info("Deleting Insurance Coverage");
                _1095MonthlyDetails.DeletePart3InsuranceCoverage(request.model, request.Requester);

            }
            catch (Exception exception)
            {
                this.Log.Error("Errors during Deleting Part3 Insrance Coverage in WApiEmployee1095summaryController.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }

            return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);
        }

        //save part 3
        [HttpPost]
        public HttpResponseMessage AddInsuranceCoverage(NewChildItemRequest<Employee1095detailsPart3Model> request)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            IList<Employee1095detailsPart3Model> models = new List<Employee1095detailsPart3Model>();
            ModelItemResponse<Employee1095detailsPart3Model> responseMessage = new ModelItemResponse<Employee1095detailsPart3Model>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {

                this.Log.Info("Adding New Insurance Coverage");
                responseMessage.Model = _1095MonthlyDetails.AddPart3InsuranceCoverage(request.model, request.ResourceId, request.Requester);

            }
            catch (Exception exception)
            {
                this.Log.Error("Errors during Adding Part3 Insrance Coverage in WApiEmployee1095summaryController.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }

            return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);
        }


        [HttpPost]
        public HttpResponseMessage GetPart2ForEmployee(SingleEmployee1095summaryForTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<Employee1095detailsPart2Model> responseMessage = new ModelListResponse<Employee1095detailsPart2Model>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {
                this.Log.Info("Getting the Entity");

                IList<Employee1095detailsPart2Model> models = new List<Employee1095detailsPart2Model>();


                //get employee by Resource Id
                employee emp = _1095MonthlyDetails.getEmployeeByResourceId(request.ResourceId);
                //get that employees part 2 items
                models = _1095MonthlyDetails.getEmployeeMonthlyDetail(request.EmployerId, request.TaxYear, this.UserEditService, emp);

                responseMessage.Models = models;

                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

                return response;
            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }
        }

        [HttpPost]
        public HttpResponseMessage UpdatePart2ForEmployee(UpdateManyPart2Request request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<Employee1095detailsPart2Model> responseMessage = new ModelListResponse<Employee1095detailsPart2Model>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {

                this.Log.Info("Updating the Entity");

                // get employee by Resource Id
                employee emp = _1095MonthlyDetails.getEmployeeByResourceId(request.ResourceId);
                // get that employees part 2 items

                IList<Employee1095detailsPart2Model> currentSystemModels = _1095MonthlyDetails.getEmployeeMonthlyDetail(request.EmployerId, request.TaxYear, this.UserEditService, emp);

                // deal with all 12 values for comparison
                Employee1095detailsPart2Model all12 = currentSystemModels.Where(x => x.MonthId == 0).FirstOrDefault();

                if (all12 != null)
                {
                    // All12 values will be blank or null unless all values are all 12s, Line 14 cannot be null or empty like 15, or 16
                    if (false == all12.Line14.IsNullOrEmpty())
                    {
                        foreach (Employee1095detailsPart2Model model in currentSystemModels)
                        {

                            model.Line14 = all12.Line14;
                            model.Line15 = all12.Line15;
                            model.Line16 = all12.Line16;
                            model.Receiving1095C = all12.Receiving1095C;

                        }
                    }
                }

                // Ensure that the Employee Id is set correctly on the input data
                foreach (Employee1095detailsPart2Model mod in request.models)
                {
                    mod.EmployeeId = emp.employee_id;
                }

                // Convert the System values to what the mass update expects
                Dictionary<int, List<Employee1095detailsPart2Model>> currentData = new Dictionary<int, List<Employee1095detailsPart2Model>>
                {
                    { emp.employee_id, currentSystemModels.ToList() }
                };
                // Call the update method (This current method is shared with the mass update)
                this.UserEditService.UpdateWithEdits(request.models, currentData, request.EmployerId, request.TaxYear, request.Requester);

                //get that employees part 2 items
                IList<Employee1095detailsPart2Model> models = _1095MonthlyDetails.getEmployeeMonthlyDetail(request.EmployerId, request.TaxYear, this.UserEditService, emp);

                //return the updated values
                responseMessage.Models = models;

                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

                return response;

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }
        }

        [HttpPost]
        public HttpResponseMessage GetPart3ForEmployee(SingleEmployee1095summaryForTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<Employee1095detailsPart3Model> responseMessage = new ModelListResponse<Employee1095detailsPart3Model>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {
                this.Log.Info("Getting the Entity");

                IList<Employee1095detailsPart3Model> models = new List<Employee1095detailsPart3Model>();

                //get employee by Resource Id
                employee emp = _1095MonthlyDetails.getEmployeeByResourceId(request.ResourceId);
                //get that employees part 3 items
                models = _1095MonthlyDetails.getEmployeeInsuranceCoverage(emp.employee_id, request.TaxYear);

                responseMessage.Models = models;

                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

                return response;

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }
        }

        [HttpPost]
        public HttpResponseMessage GetSingleForTaxYear(SingleEmployee1095summaryForTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<Employee1095summaryModel> responseMessage = new ModelListResponse<Employee1095summaryModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {

                this.Log.Info("Getting the Entity");

                List<Employee1095summaryModel> models = new List<Employee1095summaryModel>();

                Employee1095summaryModel model = _1095MonthlyDetails.GetSingleEmployee1095summaryModel(request.ResourceId, request.EmployerId, request.TaxYear, this.UserEditService, this.UserReviewedService, this.LegacyClassifictaionService);

                models.Add(model);

                responseMessage.Models = models;

                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

                return response;

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }
        }

        [HttpPost]
        public HttpResponseMessage Finalize1095(EmployerTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            UIMessageResponse responseMessage = new UIMessageResponse()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {

                this.Log.Info("Finalizing the Entities");
                IList<Employee1095summaryModel> models = _1095MonthlyDetails.GetAllEmployee1095summaryModel(request.EmployerId, request.TaxYear, this.UserEditService, this.LegacyEmployeeService, this.UserReviewedService, this.Finalize1095Service);
                List<Employee1095summaryModel> ReviewedEmployees = models.Where(m => m.Reviewed == true).ToList();
                IList<Approved1095Final> viewModels = Mapper.Map<IList<Employee1095summaryModel>, IList<Approved1095Final>>(ReviewedEmployees);


                this.Finalize1095Service.SaveApproved1095(viewModels, request.EmployerId, request.TaxYear, request.Requester);


                responseMessage.UIMessage = "Finalized [" + viewModels.Count + "] 1095 Forms.";


                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);
                return response;

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }

        }

        [HttpPost]
        public HttpResponseMessage Reviewed1095(SingleEmployee1095summaryForTaxYearRequest request)
        {

            return this.ChangeReviewed1095(request, true);

        }

        [HttpPost]
        public HttpResponseMessage UnReviewed1095(SingleEmployee1095summaryForTaxYearRequest request)
        {

            return this.ChangeReviewed1095(request, false);

        }

        public HttpResponseMessage ChangeReviewed1095(SingleEmployee1095summaryForTaxYearRequest request, bool NewValue)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BasicResponse responseMessage = new BasicResponse()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {

                employee emp = _1095MonthlyDetails.getEmployeeByResourceId(request.ResourceId);

                // first deactivate all active Reviewed
                UserReviewed review = new UserReviewed
                {
                    EmployeeId = emp.employee_id,
                    EmployerId = request.EmployerId,
                    TaxYear = request.TaxYear
                };

                this.UserReviewedService.DeactivateReview(review, request.Requester);

                // then if the new value is ture, create a new reviewed
                if (true == NewValue)
                {
                    // this works as an update-or-insert-ish
                    this.UserReviewedService.UpdateReviewed(review, request.Requester);

                }

                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);
                return response;

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }

        }


        [HttpPost]
        public HttpResponseMessage GetForTaxYear(EmployerTaxYearRequest request)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ModelListResponse<Employee1095summaryModel> responseMessage = new ModelListResponse<Employee1095summaryModel>()
            {
                StatusEnum = ResponseMessageStatus.OK,
                Errors = new List<string>()
                {
                }
            };

            try
            {

                this.Log.Info("Getting the Entities");

                IList<Employee1095summaryModel> models = _1095MonthlyDetails.GetAllEmployee1095summaryModel(request.EmployerId, request.TaxYear, this.UserEditService, this.LegacyEmployeeService, this.UserReviewedService, this.Finalize1095Service);
                responseMessage.Models = models;
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;
                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

                return response;

            }
            catch (Exception exception)
            {

                this.Log.Error("Errors during service retrieval.", exception);

                responseMessage.StatusEnum = ResponseMessageStatus.Error;
                responseMessage.Errors.Add(exception.ToString());
                stopWatch.Stop();
                responseMessage.TimeTaken = stopWatch.ElapsedMilliseconds;

                return this.Request.CreateResponse(HttpStatusCode.OK, responseMessage);

            }

        }

    }

}