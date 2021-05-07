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
using Afas.Application.Archiver;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Application.Services;
using Afas.AfComply.Reporting.Application;
using Afc.Core.Application;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Domain.POCO;
using Afas.AfComply.Reporting.Domain.Printing;

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class PrintController : BaseMvcController
    {
        protected IApiHelper ApiHelper { get; private set; }

        private IFileArchiver Archiver;

        private IPrintBatchService PrintService;

        private IFinalize1095Service FinalService;

        public PrintController(ILogger logger, IEncryptedParameters encryptedParameters, IApiHelper apiHelper,
                ITransactionContext transactionContext,
                IPrintBatchService service,
                IFileArchiver archiver,
                IFinalize1095Service finalService) 
            : base(logger, encryptedParameters)
        {
            if (null == archiver)
            {
                throw new ArgumentNullException("Archiver");
            }
            this.Archiver = archiver;

            if (null == transactionContext)
            {
                throw new ArgumentNullException("transactionContext");
            }

            if (null == service)
            {
                throw new ArgumentNullException("PrintService");
            }
            this.PrintService = service;
            this.PrintService.Context = transactionContext;

            if (null == finalService)
            {
                throw new ArgumentNullException("FinalService");
            }
            this.FinalService = finalService;
            this.FinalService.Context = transactionContext;

            SharedUtilities.VerifyObjectParameter(apiHelper, "ApiHelper");
            
            this.ApiHelper = apiHelper;

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult HasPrinted(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            int taxYear = Feature.CurrentReportingYear;                  

            int.TryParse(EncryptedParameters["TaxYear"], out taxYear);

            Employee1095PrintRequests request = new Employee1095PrintRequests();
            request.EmployerId = employerId;
            request.TaxYear = taxYear;
            request.Correction = false;

            request.Requester = CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext);

            bool hasPrinted = PrintService.HasPrintedEmployerTaxYear(request.EmployerId, request.TaxYear);

            stopwatch.Stop();
            Log.Info(string.Format("HasPrinted finished in [{0}]ms.", stopwatch.ElapsedMilliseconds));

            return Json(hasPrinted);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult Printing1095(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            int taxYear = Feature.CurrentReportingYear;                   

            int.TryParse(EncryptedParameters["TaxYear"], out taxYear);

            Employee1095PrintRequests request = new Employee1095PrintRequests();
            request.EmployerId = employerId;
            request.TaxYear = taxYear;
            request.Correction = false;

            request.Requester = CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext);

            return Print(request, false, false, false);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult AllReprinting1095(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            int taxYear = Feature.CurrentReportingYear;                  

            int.TryParse(EncryptedParameters["TaxYear"], out taxYear);

            Employee1095PrintRequests request = new Employee1095PrintRequests();
            request.EmployerId = employerId;
            request.TaxYear = taxYear;
            request.Correction = false;

            request.Requester = CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext);

            return Print(request, false, false, true);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult PdfPrinting1095(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            int taxYear = Feature.CurrentReportingYear;                  

            int.TryParse(EncryptedParameters["TaxYear"], out taxYear);
            
            Employee1095PrintRequests request = new Employee1095PrintRequests();
            request.EmployerId = employerId;
            request.TaxYear = taxYear;
            request.Correction = false;

            request.Requester = CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext);
            
            return Print(request, true, false, true);

        }

        private ActionResult Print(Employee1095PrintRequests request, bool reprint, bool afasRequested, bool printAll)
        {

            List<Approved1095Final> tempList = FinalService.GetApproved1095sForEmployerTaxYear(request.EmployerId, request.TaxYear);
            
            tempList = tempList.Where(item => item.Receiving1095 == true).ToList();
            
            if (false == printAll)
            {

                var prints = PrintService.GetForEmployerTaxYear(request.EmployerId, request.TaxYear);
                List<Print1095> printed = prints.Select(printbatch => printbatch.AllPrinted1095s.ToList()).SelectMany(item => item).ToList();

                List<Approved1095Final> otherlist = printed.Where(item => tempList.Any(y => y.ID == item.Approved1095.ID)).Select(item => item.Approved1095).ToList();

                tempList.RemoveAll(x => otherlist.Any(y => y.ID == x.ID));

            }
            
            if (tempList.Count <= 0)
            {
                return null;
            }

            employer employ = employerController.getEmployer(request.EmployerId);
            Approved1094FinalPart1 employer = new Approved1094FinalPart1();

            employer.Address = employ.EMPLOYER_ADDRESS;
            employer.City = employ.EMPLOYER_CITY;
            employer.EmployerName = employ.EMPLOYER_NAME;

            employer.StateId = employ.EMPLOYER_STATE_ID;

            List<User> users = UserController.getDistrictUsers(request.EmployerId);
            List<User> IrsContacts = (from User contact in users where contact.User_IRS_CONTACT == true select contact).ToList();
            employer.IrsContactPhone = IrsContacts.First().User_Phone;

            employer.IrsContactName = IrsContacts.First().User_Full_Name;

            employer.ZipCode = employ.EMPLOYER_ZIP;
            employer.CreatedBy = request.Requester;
            employer.EIN = employ.EMPLOYER_EIN.Replace("-", "");
            employer.EmployerId = employ.EMPLOYER_ID;
            employer.EmployerResourceId = employ.ResourceId;
            employer.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            employer.ModifiedBy = request.Requester;
            employer.ModifiedDate = DateTime.Now;
            employer.ResourceId = Guid.NewGuid();

            Guid EmployerGuid = employ.ResourceId;

            string content = PrintFileGenerator.GeneratePrintCSVContent(request.Correction, "1095C", tempList, employer);

            var folder_path = System.Web.HttpContext.Current.Server.MapPath("~/ftps/Print/");
            string tempFolder = System.Web.HttpContext.Current.Server.MapPath("~/ftps/Scratch/");

            long millis = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;
            string file_name = string.Format(@"{0}_{1}_{2}.txt", "1095C", EmployerGuid.ToString(), millis.ToString());

            if (reprint)
            {                  
                file_name = "reprint_" + file_name;
            }

            PrintBatch batch = new PrintBatch();
            batch.CreatedBy = request.Requester;
            batch.ModifiedBy = request.Requester;
            batch.RequestedBy = request.Requester;
            batch.RequestedOn = DateTime.Now;
            batch.ModifiedDate = DateTime.Now;
            batch.EmployerId = employ.EMPLOYER_ID;
            batch.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            batch.PrintFileName = file_name;
            batch.TaxYear = request.TaxYear;

            batch.Reprint = reprint;
            batch.AfasRequested = afasRequested;
            
            int archiveId = PrintFileGenerator.WriteCSVContentToFile(Archiver, EmployerGuid, folder_path, tempFolder, file_name, content, request.EmployerId);
            
            ArchiveFileInfo archive = ArchiveFileInfoFactory.GetArchivedFileInfoById(archiveId);
            batch.ArchivedFile = archive;
            batch.PrintFileArchivePath = archive.ArchiveFilePath;
            batch.SentOn = DateTime.Now;

            foreach (Approved1095Final final in tempList)
            {

                Print1095 print1095 = new Print1095();

                print1095.Approved1095 = final;
                print1095.PrintBatch = batch;
                print1095.ModifiedDate = DateTime.Now;
                print1095.ModifiedBy = request.Requester;
                print1095.CreatedBy = request.Requester;
                print1095.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
                print1095.OutputFilePath = "";           

                batch.AllPrinted1095s.Add(print1095);

            }

            PrintService.SaveBatch(batch, request.Requester);

            return null;
        }

    }

}