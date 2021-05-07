using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Afas.AfComply.Domain;
using Afc.Core;
using Afc.Core.Logging;
using Afc.Core.Presentation.Web;
using Afc.Marketing;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.UI.Areas.Administration.Controllers;
using Afas.AfComply.UI.Areas.ViewModels.Reporting;
using System.Web.UI;
using Afas.AfComply.Reporting.Application;
using Afc.Core.Application;
using System.Globalization;
using System.Configuration;

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class VerificationController : BaseReadOnlyController<VerificationModel, VerificationViewModel>
    {
        int status;
        private IPrintBatchService PrintService;
        public VerificationController(ILogger logger,
                ITransactionContext transactionContext,
                IEncryptedParameters encryptedParameters,
                IPrintBatchService service,
                IApiHelper apiHelper
            )
            : base(logger, encryptedParameters, apiHelper)
        {

            GetAllKey = "get-all-timeframes";
            GetManyKey = "get-all-timeframes";
            GetByIdKey = "get-timeframe-by-resource-id";

            if (null == service)
            {
                throw new ArgumentException("PrintService");
            }
            this.PrintService = service;
            this.PrintService.Context = transactionContext;

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public override ActionResult GetAll()
        {

            bool ConfirmDataIsCorrect = false;
            bool CompleteQuestionnaire = false;
            int EMPLOYER_ID = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            var currentEmployerTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(EMPLOYER_ID, Feature.CurrentReportingYear);       
            Log.Info(String.Format("Verification Get IRS data: EmployerID: [{0}], currentEmployerTransmissionStatus:[{1}]", CompleteQuestionnaire, currentEmployerTransmissionStatus));

            if (currentEmployerTransmissionStatus != null)
            {
                status = (int)currentEmployerTransmissionStatus.TransmissionStatusId;
                ConfirmDataIsCorrect = true;
                if (TransmissionStatusEnum.Initiated != currentEmployerTransmissionStatus.TransmissionStatusId)
                {
                    CompleteQuestionnaire = true;
                }
            }

            IList<VerificationViewModel> viewModels = new List<VerificationViewModel>();
            VerificationViewModel vm = new VerificationViewModel();
            vm.Step = "1. Confirm Data is Correct";
            if (ConfirmDataIsCorrect)
            {
                vm.Status = true;
                vm.StatusString = "Complete";
            }
            else
            {
                vm.Status = false;
                vm.StatusString = "Not Complete";
            }

            vm.Instructions = @"Go to the <a href='Confirmation'>Confirmation Page</a> and follow the instructions to confirm key data is correct and address any open issues.";
            viewModels.Add(vm);

            VerificationViewModel vm2 = new VerificationViewModel();
            vm2.Step = "2. Complete Questionnaire";

            if (CompleteQuestionnaire)
            {
                vm2.Status = true;
                vm2.StatusString = "Complete";
            }
            else
            {
                vm2.Status = false;
                vm2.StatusString = String.Format("Not Complete – must finish by {0} after completing Step 1", Branding.IrsDeadlineSetup);
            }
            vm2.Instructions = @"Complete the <a href='/irs_submission/step1.aspx'> Questionnaire</a> and submit.";
            viewModels.Add(vm2);


            bool Step6StatusString = PrintService.HasPrintedEmployerTaxYear(EMPLOYER_ID, Feature.CurrentReportingYear);


            bool EBC = false;
            if (Feature.QlikEnabled == false) { EBC = true; }
            bool stepThree = false;
            if (currentEmployerTransmissionStatus != null && EBC == true)
            {
                stepThree = true;
            }
            if (currentEmployerTransmissionStatus != null && EBC == false && isView1095Enabled(status))
            {
                stepThree = true;
            }
            VerificationViewModel vm3 = new VerificationViewModel();
            if (EBC)
            {
                vm3.Step = "3 & 4. Certify & Print 1095-C Information ";
            }
            else
            {
                vm3.Step = "3. Certify Information ";
            }
            if (stepThree)
            {
                vm3.Status = true;
                vm3.StatusString = (Step6StatusString) ? "Complete" : "Ongoing";
                var hiddenUnfinalize = ConfigurationManager.AppSettings["Feature.Unfinalize"];

                if (hiddenUnfinalize != null && hiddenUnfinalize == "false")
                {
                    vm3.Instructions = @"Review the information and codes that will be shown on your 1095-C forms and correct as necessary. After you’ve reviewed all forms, you will Finalize the reviewed forms and Print the forms. After you have completed this process, your 1095-C forms will be printed and mailed to your employees.<a href='/Reporting/View1095'> View1095 </a>";
                }
                else
                {
                    vm3.Instructions = @"Review the information and codes that will be shown on your 1095-C forms and correct as necessary. After you’ve reviewed all forms, you will Finalize the reviewed forms and Print the forms. After you have completed this process, your 1095-C forms will be printed and mailed to your employees.<a href='/Reporting/View1095'> View1095 </a> or <a href='/Reporting/Unfinalize1095'> Unfinalized1095 </a> ";
                }

                viewModels.Add(vm3);
            }
            else
            {
                vm3.Status = false;
                vm3.StatusString = "You have not been activated to view and edits the 1095-C forms. Please finish all the other previous step and contact your consultant.";
            }

            if (stepThree)
            {
                VerificationViewModel vm5 = new VerificationViewModel();
                if (EBC)
                {
                    vm5.Step = "5.1095-C PDF";
                }
                else
                {
                    vm5.Step = "4.1095-C PDF";
                }
                vm5.Status = true;
                vm5.StatusString = (Step6StatusString) ? "Complete" : "Ongoing";
                vm5.Instructions = @"<a href='/securepages/1095_PDF_display.aspx'>Download and View 1095 PDFs</a>.";
                viewModels.Add(vm5);
            }


            bool stepFive = false;
            if (currentEmployerTransmissionStatus != null && EBC == true)
            {
                stepFive = true;
            }
            if (currentEmployerTransmissionStatus != null && EBC == false
                && isView1094Enabled(status))
            {
                stepFive = true;
            }
            if (stepFive)
            {
                VerificationViewModel vm6 = new VerificationViewModel();
                if (EBC)
                {
                    vm6.Step = "6. 1094 ";
                }
                else
                {
                    vm6.Step = "5. 1094 ";
                }
                vm6.Status = true;
                vm6.StatusString = "Complete";
                vm6.Instructions = @"<a href='/Reporting/View1094'>View 1094 </a>.";
                viewModels.Add(vm6);
            }

            //////VerificationViewModel vm7 = new VerificationViewModel();
            //////vm7.Step = "7. View 1094";
            //////vm7.Status = true;
            //////vm7.StatusString = "Complete";
            //////vm7.Instructions = @"<a href='/Reporting/View1094'>View 1094 </a>.";
            //////viewModels.Add(vm7);
            ////Send the View Model to the front end as Json

            bool stepTransmit = false;
            if (currentEmployerTransmissionStatus != null && EBC == true)
            {
                stepTransmit = true;
            }
            if (currentEmployerTransmissionStatus != null && EBC == false
                && isTransmitEnabled(status))
            {
                stepTransmit = true;
            }
            if (stepTransmit)
            {
                VerificationViewModel vm8 = new VerificationViewModel();
                if (EBC)
                {
                    vm8.Step = "7. IRS Reporting ";
                }
                else
                {
                    vm8.Step = "6. IRS Reporting ";
                }
                vm8.Status = true;
                vm8.StatusString = "OnGoing";
                vm8.Instructions = @"Once your forms have been submitted to the IRS, additional status information will be <a href='/securepages/10941095_submission_status.aspx'> provided here</a>.";
                viewModels.Add(vm8);
            }

            return Json(viewModels, JsonRequestBehavior.AllowGet);

        }

        public bool isView1095Enabled(int status)
        {
            int[] beforeStepThree = new int[] { 1 ,2, 3, 4, 5, 12, 13 };

            foreach (int step in beforeStepThree)
            {
                if (status == step)
                    return false;
            }

            return true;
        }

        public bool isPDFEnabled(int status)
        {


            int[] beforeViewPdf = new int[] { 8, 9, 22 };

            if (false == isView1095Enabled(status))
            {
                return false;
            }

            foreach (int step in beforeViewPdf)
            {
                if (status == step)
                    return false;
            }

            return true;
        }

        public bool isView1094Enabled(int status)
        {

            int[] stepFive = new int[] {  8, 9, 22 };

            if (false == isPDFEnabled(status))
            {
                return false;
            }

            foreach (int step in stepFive)
            {
                if (status == step)
                    return false;
            }

            return true;
        }


        public bool isTransmitEnabled(int status)
        {

            int[] stepTransmit = new int[] { 6, 7, 11, 15, 16, 17, 18, 19, 20, 21 };


            if (false == isView1094Enabled(status))
            {
                return false;
            }

            foreach (int step in stepTransmit)
            {
                if (status == step)
                    return true;
            }

            return false;
        }
    }



}