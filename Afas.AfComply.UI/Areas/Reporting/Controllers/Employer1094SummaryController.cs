using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Afc.Core;
using Afc.Core.Logging;
using Afc.Core.Presentation.Web;
using Afas.AfComply.Reporting.Core.Response;
using Afc.Marketing.Response;
using Afc.Marketing;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using AutoMapper;
using Afas.AfComply.UI.Areas.ViewModels;
using Afas.AfComply.UI.Areas.Administration.Controllers;
using Afas.AfComply.UI.Areas.ViewModels.Reporting;
using Afas.AfComply.UI.Areas.ViewModels.Enums;
using Afc.Framework.Presentation.Web;
using System.Web.UI;
using System.Diagnostics;
using System.Data;
using Afas.Domain;
using System.Text;
using Afas.AfComply.UI.App_Start;
using Afas.AfComply.Domain;
using log4net;

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{
    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class Employer1094SummaryController : BaseReadOnlyController<Employer1094SummaryModel, Employee1095summaryViewModel>
    {
        private ILog IRSLog = log4net.LogManager.GetLogger(typeof(ConfirmationPageController));
        public string Finalize1094Key { get; set; }
        public string GetAllEmployersKey { get; set; }
        public Employer1094SummaryController(ILogger logger,
                                             IEncryptedParameters encryptedParameters, 
                                             IApiHelper apiHelper
                                            ) : base(logger, encryptedParameters, apiHelper)
        {
            GetManyKey = "get-many-1094summary";
            Finalize1094Key = "Finalize1094-1094summary"; 
        }


        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public void Confirm(string encryptedParameters)
        {
            base.Load(encryptedParameters);
            string username = CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext);
            int employerId = int.Parse(this.EncryptedParameters["EmployerId"]);
            var TaxYearId = Feature.CurrentReportingYear;

            Log.Info(String.Format("Confirming IRS data: UserName:[{0}], EmployerID: [{1}], TaxYearId:[{2}]", username, employerId, TaxYearId));
            PIILogger.LogPII(String.Format("Confirming IRS data: UserName:[{0}], EmployerID: [{1}], TaxYearId:[{2}]", username, employerId, TaxYearId));

            EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, TaxYearId);
            if (currentEmployerTaxYearTransmissionStatus == null)
            {
                initiateEmployerTransaction(username, employerId);
                currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, TaxYearId);
            }

            
            if (currentEmployerTaxYearTransmissionStatus.TransmissionStatusId == TransmissionStatusEnum.Initiated
                && currentEmployerTaxYearTransmissionStatus.EndDate == null)
            {
                currentEmployerTaxYearTransmissionStatus = employerController.endEmployerTaxYearTransmissionStatus(currentEmployerTaxYearTransmissionStatus, username);
            }
        }

        private void initiateEmployerTransaction(string username, int employerId)
        {

            var TaxYearId = Feature.CurrentReportingYear;
            var newEmployerTaxYearTransmission = new EmployerTaxYearTransmission();

            newEmployerTaxYearTransmission.EmployerId = employerId;
            newEmployerTaxYearTransmission.TaxYearId = Feature.CurrentReportingYear;
            newEmployerTaxYearTransmission.EntityStatusId = 1;
            newEmployerTaxYearTransmission.CreatedBy = username;
            newEmployerTaxYearTransmission.ModifiedBy = username;
            newEmployerTaxYearTransmission = employerController.insertUpdateEmployerTaxYearTransmission(newEmployerTaxYearTransmission);

            if (ValidationHelper.validateNewEmployerTaxYearTransmission(newEmployerTaxYearTransmission, this.IRSLog))
            {

                var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                    newEmployerTaxYearTransmission.EmployerTaxYearTransmissionId,
                    TransmissionStatusEnum.Initiated,
                    username
                );

                newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
                ValidationHelper.validateNewEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus, this.IRSLog);

            }
        }


        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpGet]
        public override ActionResult GetMany(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            EmployerTaxYearRequest request = new EmployerTaxYearRequest();
            request.EmployerId = employerId;
            request.TaxYear = Feature.CurrentReportingYear;
            request.Requester = requester;

            ModelListResponse<Employer1094SummaryModel> message = this.ApiHelper.Send<ModelListResponse<Employer1094SummaryModel>, EmployerTaxYearRequest>(GetManyKey, request);

           
            IList<Employer1094SummaryViewModel> viewModels = Mapper.Map<IList<Employer1094SummaryModel>, IList<Employer1094SummaryViewModel>>(message.Models);
            
            string jsonOut = "";

            var ser = JsonSerializer.CreateDefault();
            using (var str = new StringWriter())
            {
                ser.Serialize(str, viewModels);
                jsonOut = str.ToString();
            }


            var result = this.Content(jsonOut, "application/json");

         
            return result;
          
        }


        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpPost]
        public ActionResult Finalize1094(String encryptedParameters)
        {


            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            EmployerTaxYearRequest request = new EmployerTaxYearRequest();
            request.EmployerId = employerId;
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]); ;
            request.Requester = requester;


            UIMessageResponse message = this.ApiHelper.Send<UIMessageResponse, EmployerTaxYearRequest>(Finalize1094Key, request);

            
            if (message.Status != "OK")
            {
                string allErrors = "";

                foreach (string errorMessage in message.Errors)
                {
                    allErrors += errorMessage + " \n\r ";
                }
                Log.Warn(allErrors);

                throw new Exception(allErrors);
            }

           

            return Json(message.UIMessage, JsonRequestBehavior.AllowGet);

        }


       

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpPost]
        public ActionResult AdminFinalize1094(String encryptedParameters)
        {


            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            EmployerTaxYearRequest request = new EmployerTaxYearRequest();
            request.EmployerId = int.Parse(this.EncryptedParameters["EmployerId"]); 
            request.TaxYear = Feature.CurrentReportingYear; 
            request.Requester = requester;

            Log.Info(string.Format("Received a request to finalize employer :{0}", request.EmployerId));
            UIMessageResponse message = this.ApiHelper.Send<UIMessageResponse, EmployerTaxYearRequest>(Finalize1094Key, request);


            if (message.Status != "OK")
            {
                string allErrors = "";

                foreach (string errorMessage in message.Errors)
                {
                    allErrors += errorMessage + " \n\r ";
                }
                Log.Warn(allErrors);

                throw new Exception(allErrors);
            }



            return Json(message.UIMessage, JsonRequestBehavior.AllowGet);

        }






        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpPost]
        public ActionResult GetAllEmployers()
        {


            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            EmployerTaxYearRequest request = new EmployerTaxYearRequest();
            request.EmployerId = int.Parse(this.EncryptedParameters["EmployerId"]); ; ;
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]); ;
            request.Requester = requester;


            ModelListResponse<EmployerModel> message = this.ApiHelper.Send<ModelListResponse<EmployerModel>, EmployerTaxYearRequest>(GetAllEmployersKey, request);

            IList<EmployerViewModel> viewModels = Mapper.Map<IList<EmployerModel>, IList<EmployerViewModel>>(message.Models);
            return Json(viewModels, JsonRequestBehavior.AllowGet);




        }
    }
}