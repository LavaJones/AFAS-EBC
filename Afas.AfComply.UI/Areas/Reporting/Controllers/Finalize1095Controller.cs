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
using Afc.Core.Application;
using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Domain.Printing;

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class Finalize1095Controller : BaseReadOnlyController<Approved1095FinalModel, Approved1095FinalViewModel>
    {

        public string UnfinalizeAll1095Key { get; set; }

        public string Unfinalize1095Key { get; set; }

        public string UnReview1095Key { get; set; }


        public Finalize1095Controller(ILogger logger,
                IEncryptedParameters encryptedParameters,
                IApiHelper apiHelper
            )
            : base(logger, encryptedParameters, apiHelper)
        {

            GetAllKey = "get-all-1095final";
            GetManyKey = "approved1095final-get-many";
            GetByIdKey = "get-1095final-by-resource-id";
            UnfinalizeAll1095Key = "approved1095final-unfinalize-all-1095";
            Unfinalize1095Key = "approved1095final-unfinalize-1095";

            this.ApiHelper.TimeoutInSeconds = 300;

        }

        [HttpGet]
        public override ActionResult GetAll()
        {

            IList<Approved1095FinalViewModel> viewModels = new List<Approved1095FinalViewModel>();   


            return Json(viewModels, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public ActionResult GetTaxYears()
        {

            var viewModels = new Dictionary<string, string>();

            List<int> years = employerController.getTaxYears();

            foreach (int taxYear in years)
            {
                IEncryptedParameters EncryptedParameters = new EncryptedParameters();

                EncryptedParameters[Guid.NewGuid().ToString()] = DateTime.Now.ToUniversalTime().ToString();
                EncryptedParameters["TaxYear"] = taxYear.ToString();

                string encryptedYearlink = EncryptedParameters.AsMvcUrlParameter;
                viewModels.Add(taxYear.ToString(), encryptedYearlink);
            }
            return Json(viewModels, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpGet]
        public virtual FileResult GetPart2FileExport(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));

            employer employer = employerController.getEmployer(employerId);
            string Fein = employer.EMPLOYER_EIN;

            EmployerTaxYearRequest request = new EmployerTaxYearRequest();
            request.EmployerId = employerId;
            request.TaxYear =  int.Parse(this.EncryptedParameters["TaxYear"]);

            watch.Stop();
            Log.Debug(string.Format("Get-Part2-File decrypted argument and built request in [{0}]ms.", watch.ElapsedMilliseconds));
            watch.Reset();
            watch.Start();

            request.Requester = requester;

            ModelListResponse<Approved1095FinalModel> message = this.ApiHelper.Send<ModelListResponse<Approved1095FinalModel>, EmployerTaxYearRequest>(GetManyKey, request);

            watch.Stop();
            Log.Info(string.Format("Get-Part2-File made Request and got response of [{1}] models in [{0}]ms, claiming it took [{2}]ms.", watch.ElapsedMilliseconds, message.Models.Count, message.TimeTaken));
            watch.Reset();
            watch.Start();

            DataTable export = new DataTable("Export");

            export.Columns.Add("FEIN", typeof(string));
            export.Columns.Add("SSN", typeof(string));
            export.Columns.Add("Name", typeof(string));
            export.Columns.Add("Tax Year", typeof(string));
            export.Columns.Add("Month", typeof(string));
            export.Columns.Add("Receiving 1095", typeof(string));
            export.Columns.Add("Measured FT", typeof(string));
            export.Columns.Add("Line 14", typeof(string));
            export.Columns.Add("Line 15", typeof(string));
            export.Columns.Add("Line 16", typeof(string));
            export.Columns.Add("Offered", typeof(string));

            foreach (Approved1095FinalModel model in message.Models)
            {

                foreach (Approved1095FinalPart2Model part2 in model.part2s.OrderBy(item => item.MonthId))
                {

                    if (part2.MonthId == 0)
                    {
                        if (false == part2.Line14.IsNullOrEmpty())
                        {
                            foreach (Approved1095FinalPart2Model update in model.part2s)
                            {
                                update.Line14 = part2.Line14;
                            }
                        }
                        if (false == part2.Line15.IsNullOrEmpty())
                        {
                            foreach (Approved1095FinalPart2Model update in model.part2s)
                            {
                                update.Line15 = part2.Line15;
                            }
                        }
                        if (false == part2.Line16.IsNullOrEmpty())
                        {
                            foreach (Approved1095FinalPart2Model update in model.part2s)
                            {
                                update.Line16 = part2.Line16;
                            }
                        }
                        if (true == part2.Receiving1095C)
                        {
                            foreach (Approved1095FinalPart2Model update in model.part2s)
                            {
                                update.Receiving1095C = part2.Receiving1095C;
                            }
                        }
                        if (true == part2.Offered)
                        {
                            foreach (Approved1095FinalPart2Model update in model.part2s)
                            {
                                update.Offered = part2.Offered;
                            }
                        }
                        continue;
                    }

                    DataRow row = export.NewRow();

                    row["FEIN"] = Fein;
                    row["SSN"] = model.SSN.ZeroPadSsn();
                    row["Name"] = model.LastName + ", " + model.FirstName;
                    row["Tax Year"] = part2.TaxYear;
                    row["Month"] = part2.MonthId;
                    row["Receiving 1095"] = model.Receiving1095;
                    row["Measured FT"] = part2.Receiving1095C;
                    row["Line 14"] = part2.Line14;
                    row["Line 15"] = part2.Line15;
                    row["Line 16"] = part2.Line16;
                    row["Offered"] = part2.Offered;

                    export.Rows.Add(row);

                }

            }

            watch.Stop();
            Log.Debug(string.Format("Get-Part2-File built file of [{1}] models taking [{0}]ms.", watch.ElapsedMilliseconds, message.Models.Count));

            stopwatch.Stop();
            Log.Info(string.Format("Get-Part2-File Returned [{0}] view models in [{1}]ms.", message.Models.Count, stopwatch.ElapsedMilliseconds));

            return File(Encoding.ASCII.GetBytes(export.GetAsCsv()), "application/vnd.ms-excel", employer.EMPLOYER_NAME + "_Part2_" + DateTime.Now.ToShortDateString() + ".csv");

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpPost]
        public ActionResult UnFinalizeAll1095(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            EmployerTaxYearRequest request = new EmployerTaxYearRequest();
            request.EmployerId = employerId;
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);
            request.Requester = requester;

            watch.Stop();
            Log.Debug(string.Format("Finalize1095 decrypted argument and built request in [{0}]ms.", watch.ElapsedMilliseconds));
            watch.Reset();
            watch.Start();

            UIMessageResponse message = this.ApiHelper.Send<UIMessageResponse, EmployerTaxYearRequest>(UnfinalizeAll1095Key, request);

            watch.Stop();
            Log.Debug(string.Format("Finalize1095 made Request and got response in [{0}]ms, claiming it took [{1}]ms.", watch.ElapsedMilliseconds, message.TimeTaken));

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

            stopwatch.Stop();
            Log.Info(string.Format("UnFinalizeAll1095 finished in [{0}]ms.", stopwatch.ElapsedMilliseconds));

            return null;  

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpPost]
        public ActionResult UnFinalize1095(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Stopwatch watch = new Stopwatch();
            watch.Start();

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            SingleEmployee1095summaryForTaxYearRequest request = new SingleEmployee1095summaryForTaxYearRequest();
            request.EmployerId = employerId;
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);
            request.ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"]);
            request.Requester = requester;
            

            watch.Stop();
            Log.Debug(string.Format("UnFinalize1095 decrypted argument and built request in [{0}]ms.", watch.ElapsedMilliseconds));
            watch.Reset();
            watch.Start();

            BasicResponse message = this.ApiHelper.Send<BasicResponse, SingleEmployee1095summaryForTaxYearRequest>(Unfinalize1095Key, request);


            watch.Stop();
            Log.Debug(string.Format("UnFinalize1095 made Request and got response in [{0}]ms, claiming it took [{1}]ms.", watch.ElapsedMilliseconds, message.TimeTaken));

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

            stopwatch.Stop();
            Log.Info(string.Format("UnFinalize1095 Finished in [{0}]ms.", stopwatch.ElapsedMilliseconds));

            return null;

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
            base.Load(encryptedParameters);

                int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
                EmployerTaxYearRequest request = new EmployerTaxYearRequest();
                request.EmployerId = employerId;
                request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);

                watch.Stop();
                Log.Debug(string.Format("Get-Many decrypted argument and built request in [{0}]ms.", watch.ElapsedMilliseconds));
                watch.Reset();
                watch.Start();

                request.Requester = requester;

                ModelListResponse<Approved1095FinalModel> message = this.ApiHelper.Send<ModelListResponse<Approved1095FinalModel>, EmployerTaxYearRequest>(GetManyKey, request);

                watch.Stop();
                Log.Info(string.Format("Get-Many made Request and got response of [{1}] models in [{0}]ms, claiming it took [{2}]ms.", watch.ElapsedMilliseconds, message.Models.Count, message.TimeTaken));
                watch.Reset();
                watch.Start();

                var models = message.Models;

                IList<Approved1095FinalViewModel> viewModels = Mapper.Map<IList<Approved1095FinalModel>, IList<Approved1095FinalViewModel>>(models);
            
                watch.Stop();
                Log.Debug(string.Format("Get-Many mapping of [{1}] models took [{0}]ms.", watch.ElapsedMilliseconds, viewModels.Count));
                watch.Reset();
                watch.Start();

                string jsonOut = "";

                var ser = JsonSerializer.CreateDefault();
                using (var str = new StringWriter())
                {
                    ser.Serialize(str, viewModels);
                    jsonOut = str.ToString();
                }


                watch.Stop();
                Log.Debug(string.Format("Get-Many json serialize [{1}] models took [{0}]ms.", watch.ElapsedMilliseconds, viewModels.Count));

                var result = this.Content(jsonOut, "application/json");

                stopwatch.Stop();
                Log.Info(string.Format("Get-Many Returned [{0}] view models in [{1}]ms.", viewModels.Count, stopwatch.ElapsedMilliseconds));

                return result;
           

        }        

    }

}