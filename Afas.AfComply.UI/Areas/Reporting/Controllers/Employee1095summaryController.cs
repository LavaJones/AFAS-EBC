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

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class Employee1095summaryController : BaseReadOnlyController<Employee1095summaryModel, Employee1095summaryViewModel>
    {

        public string UpdateInsuranceCoverageKey { get; set; }

        public string DeleteInsuranceCoverageKey { get; set; }

        public string AddInsuranceCoverageKey { get; set; }

        public string Finalize1095Key { get; set; }

        public string UpdtePart2Key { get; set; }

        public string GetSingleKey { get; set; }

        public string GetPart2ByEmployeeIdKey { get; set; }

        public string GetPart3ByEmployeeIdKey { get; set; }

        public string Review1095Key { get; set; }

        public string UnReview1095Key { get; set; }

        public Employee1095summaryController(ILogger logger,
                IEncryptedParameters encryptedParameters,
                IApiHelper apiHelper
            )
            : base(logger, encryptedParameters, apiHelper)
        {

            GetAllKey = "get-all-1095summary";
            GetManyKey = "get-many-1095summary";
            GetByIdKey = "get-1095summary-by-resource-id";
            GetSingleKey = "get-single-1095summary";
            UpdtePart2Key = "update-many-monthly-details-by-employee";
            GetPart2ByEmployeeIdKey = "get-many-monthly-details-by-employee";
            GetPart3ByEmployeeIdKey = "get-many-covered-individuals-by-employee";
            UpdateInsuranceCoverageKey = "UpdateInsuranceCoverage-1095summary";
            DeleteInsuranceCoverageKey = "DeleteInsuranceCoverage-1095summary";
            AddInsuranceCoverageKey = "AddInsuranceCoverage-1095summary";
            Finalize1095Key = "Finalize1095-1095summary";
            Review1095Key = "review-1095summary";
            UnReview1095Key = "un-review-1095summary";

            this.ApiHelper.TimeoutInSeconds = 300;

        }

        [HttpGet]
        public override ActionResult GetAll()
        {

            IList<Employee1095summaryViewModel> viewModels = new List<Employee1095summaryViewModel>();   


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
        public override ActionResult GetSingle(String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            int TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);
            SingleEmployee1095summaryForTaxYearRequest request = new SingleEmployee1095summaryForTaxYearRequest();
            request.ResourceId = ResourceId;
            request.EmployerId = employerId;
            request.TaxYear = TaxYear;

            request.Requester = requester;

            ModelListResponse<Employee1095summaryModel> message = this.ApiHelper.Send<ModelListResponse<Employee1095summaryModel>, EmployerTaxYearRequest>(GetSingleKey, request);

            Employee1095summaryViewModel viewModels = Mapper.Map<Employee1095summaryModel, Employee1095summaryViewModel>(message.Models.SingleOrDefault());

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
        public ActionResult Finalize1095(String encryptedParameters)
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

            UIMessageResponse message = this.ApiHelper.Send<UIMessageResponse, EmployerTaxYearRequest>(Finalize1095Key, request);

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
            Log.Info(string.Format("Finalize1095 finished in [{0}]ms.", stopwatch.ElapsedMilliseconds));
            
            return Json(message.UIMessage, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpPost]
        public ActionResult Review1095(String encryptedParameters)
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

            string json = new StreamReader(Request.InputStream).ReadToEnd();
            bool value = JsonConvert.DeserializeObject<bool>(json);

            watch.Stop();
            Log.Debug(string.Format("Review1095 decrypted argument and built request in [{0}]ms.", watch.ElapsedMilliseconds));
            watch.Reset();
            watch.Start();
            
            BasicResponse message = null;
            if (true == value)
            {
                message = this.ApiHelper.Send<BasicResponse, SingleEmployee1095summaryForTaxYearRequest>(Review1095Key, request);
            }
            else
            {
                message = this.ApiHelper.Send<BasicResponse, SingleEmployee1095summaryForTaxYearRequest>(UnReview1095Key, request);
            }

            watch.Stop();
            Log.Debug(string.Format("Review1095 made Request and got response in [{0}]ms, claiming it took [{1}]ms.", watch.ElapsedMilliseconds, message.TimeTaken));

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
            Log.Info(string.Format("Review1095 Finished in [{0}]ms.", stopwatch.ElapsedMilliseconds));

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

                ModelListResponse<Employee1095summaryModel> message = this.ApiHelper.Send<ModelListResponse<Employee1095summaryModel>, EmployerTaxYearRequest>(GetManyKey, request);

                watch.Stop();
                Log.Info(string.Format("Get-Many made Request and got response of [{1}] models in [{0}]ms, claiming it took [{2}]ms.", watch.ElapsedMilliseconds, message.Models.Count, message.TimeTaken));
                watch.Reset();
                watch.Start();

                IList<Employee1095summaryViewModel> viewModels = Mapper.Map<IList<Employee1095summaryModel>, IList<Employee1095summaryViewModel>>(message.Models);

                List<classification> classifications = classificationController.ManufactureEmployerClassificationList(employerId, true);
                Dictionary<string, classification> lookupClass = classifications.CreateLookup<string, classification>(item => item.CLASS_ID.ToString());                
                foreach (Employee1095summaryViewModel vm in viewModels)
                {
                    if (lookupClass.ContainsKey(vm.EmployeeClass))
                    {
                        vm.EmployeeClass = lookupClass[vm.EmployeeClass].CLASS_DESC;
                    }
                }
                
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

        /// <summary>
        /// Action Method to Handle the Upload Functionalty
        /// </summary>
        /// <param name="postedFiles">The File uploaded.</param>
        [HttpPost]
        public ActionResult UploadFileEdits(HttpPostedFileBase postedFiles, String encryptedParameters)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (postedFiles == null || postedFiles.ContentLength <= 0)
            {
                if (HttpContext.Request.Files.AllKeys.Any())
                {
                    postedFiles = HttpContext.Request.Files[0];
                }
            }

            if (postedFiles == null || postedFiles.ContentLength <= 0) { return null; }

            if (Path.GetExtension(postedFiles.FileName) != ".csv") { return null; }

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));

            string path = Server.MapPath("..\\..\\..\\ftps\\rawdata\\");

            string saveTo = path + employerId + "_" + Path.GetFileName(postedFiles.FileName);

            postedFiles.SaveAs(saveTo);



            int TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);

            AfComplyFileDataImporter importer = ContainerActivator._container.Resolve<ReportingFileDataImporter>();
            importer.Setup(employerId, saveTo, TaxYear);
            if (importer.ImportData(requester, "Client Upload 1095 Edits", "Edits1095"))
            {

                Log.Info("User edit uploaded by " + requester);

            }
            else
            {

                Log.Info("Failed User edit uploaded by " + requester);

            }

            Log.Info(string.Format("UploadFileExport upload took in [{0}]ms.", stopwatch.ElapsedMilliseconds));

            return null;

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpGet]
        public virtual FileResult GetFileExport(String encryptedParameters)
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
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);

            watch.Stop();
            Log.Debug(string.Format("Get-File decrypted argument and built request in [{0}]ms.", watch.ElapsedMilliseconds));
            watch.Reset();
            watch.Start();

            request.Requester = requester;

            ModelListResponse<Employee1095summaryModel> message = this.ApiHelper.Send<ModelListResponse<Employee1095summaryModel>, EmployerTaxYearRequest>(GetManyKey, request);

            watch.Stop();
            Log.Info(string.Format("Get-File made Request and got response of [{1}] models in [{0}]ms, claiming it took [{2}]ms.", watch.ElapsedMilliseconds, message.Models.Count, message.TimeTaken));
            watch.Reset();
            watch.Start();

            List<classification> classifications = classificationController.ManufactureEmployerClassificationList(employerId, true);


            DataTable export = new DataTable("Part 2 Export");

            export.Columns.Add("FEIN", typeof(string));
            export.Columns.Add("SSN", typeof(string));
            export.Columns.Add("Name", typeof(string));
            export.Columns.Add("Hire Date", typeof(string));
            export.Columns.Add("Term Date", typeof(string));
            export.Columns.Add("Tax Year", typeof(string));
            export.Columns.Add("Month", typeof(string));
            export.Columns.Add("Receiving 1095", typeof(string));
            export.Columns.Add("Measured FT", typeof(string));
            export.Columns.Add("Line 14", typeof(string));
            export.Columns.Add("Line 15", typeof(string));
            export.Columns.Add("Line 16", typeof(string));
            export.Columns.Add("Reviewed", typeof(string));
            export.Columns.Add(" ", typeof(string));
            export.Columns.Add("Insurance Type", typeof(string));
            export.Columns.Add("Status", typeof(string));
            export.Columns.Add("Offered", typeof(string));
            export.Columns.Add("Enrolled", typeof(string));
            export.Columns.Add("Monthly Average Hours", typeof(string));
            export.Columns.Add("Classification", typeof(string));
            
            foreach (Employee1095summaryModel model in message.Models)
            {

                foreach (Employee1095detailsPart2Model part2 in model.EmployeeMonthlyDetails.OrderBy( item => item.MonthId ))
                {

                    if (part2.MonthId == 0)
                    {
                        if (false == part2.Line14.IsNullOrEmpty())
                        {
                            foreach (Employee1095detailsPart2Model update in model.EmployeeMonthlyDetails)
                            {
                                update.Line14 = part2.Line14;
                            }
                        }
                        if (false == part2.Line15.IsNullOrEmpty())
                        {
                            foreach (Employee1095detailsPart2Model update in model.EmployeeMonthlyDetails)
                            {
                                update.Line15 = part2.Line15;
                            }
                        }
                        if (false == part2.Line16.IsNullOrEmpty())
                        {
                            foreach (Employee1095detailsPart2Model update in model.EmployeeMonthlyDetails)
                            {
                                update.Line16 = part2.Line16;
                            }
                        }
                        if (true == part2.Receiving1095C)
                        {
                            foreach (Employee1095detailsPart2Model update in model.EmployeeMonthlyDetails)
                            {
                                update.Receiving1095C = part2.Receiving1095C;
                            }
                        }
                        if (false == part2.InsuranceType.IsNullOrEmpty())
                        {
                            foreach (Employee1095detailsPart2Model update in model.EmployeeMonthlyDetails)
                            {
                                update.InsuranceType = part2.InsuranceType;
                            }
                        }
                        if (true == part2.Offered)
                        {
                            foreach (Employee1095detailsPart2Model update in model.EmployeeMonthlyDetails)
                            {
                                update.Offered = part2.Offered;
                            }
                        }
                        if (true == part2.Enrolled)
                        {
                            foreach (Employee1095detailsPart2Model update in model.EmployeeMonthlyDetails)
                            {
                                update.Enrolled = part2.Enrolled;
                            }
                        }
                        continue;
                    }

                    DataRow row = export.NewRow();

                    row["FEIN"] = Fein;

                    row["SSN"] = model.Ssn.ZeroPadSsn();
                    row["Name"] = model.LastName + ", " + model.FirstName;
                    row["Hire Date"] = model.HireDate.ToShortDateString();
                    if (null != model.TermDate)
                    {
                        row["Term Date"] = model.TermDate.Value.ToShortDateString();
                    }
                    row["Tax Year"] = part2.TaxYear;
                    row["Month"] = part2.MonthId;
                    row["Receiving 1095"] = model.Receiving1095;
                    row["Measured FT"] = part2.Receiving1095C;

                    row["Line 14"] = part2.Line14;
                    row["Line 15"] = part2.Line15;
                    row["Line 16"] = part2.Line16;

                    if (true == model.Reviewed)
                    {
                        row["Reviewed"] = model.Reviewed;
                    }

                    if (false == part2.InsuranceType.IsNullOrEmpty())
                    {
                        row["Insurance Type"] = ((InsuranceTypeEnum)int.Parse(part2.InsuranceType)).ToString();
                    }
                    row["Status"] = ((ACAStatusEnum)part2.AcaStatus).ToString();
                    row["Offered"] = part2.Offered;
                    row["Enrolled"] = part2.Enrolled;
                    row["Monthly Average Hours"] = part2.MonthlyHours;

                    row["Classification"] = model.EmployeeClass;

                    classification classy = (from cl in classifications where cl.CLASS_ID.ToString() == model.EmployeeClass select cl).SingleOrDefault();
                    if (null != classy)
                    {
                        row["Classification"] = classy.CLASS_DESC;
                    }

                    export.Rows.Add(row);

                }

            }

            watch.Stop();
            Log.Debug(string.Format("Get-File buitl file of [{1}] models taking [{0}]ms.", watch.ElapsedMilliseconds, message.Models.Count));

            stopwatch.Stop();
            Log.Info(string.Format("Get-File Returned [{0}] view models in [{1}]ms.", message.Models.Count, stopwatch.ElapsedMilliseconds));

            return File(Encoding.ASCII.GetBytes(export.GetAsCsv()), "application/vnd.ms-excel", employer.EMPLOYER_NAME + "_Part2_" + DateTime.Now.ToShortDateString() + ".csv");

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpPut]
        public ActionResult SavePart1UserEdits(String encryptedParameters)
        {

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));

            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());

            string json = new StreamReader(Request.InputStream).ReadToEnd();
            Employee1095summaryViewModel viewModel = JsonConvert.DeserializeObject<Employee1095summaryViewModel>(json);

            Employee employee = EmployeeController.manufactureEmployeeList(employerId).Single(emp => emp.ResourceId == ResourceId);
            employee.EMPLOYEE_FIRST_NAME = viewModel.FirstName;
            employee.EMPLOYEE_MIDDLE_NAME = viewModel.MiddleName;
            employee.EMPLOYEE_LAST_NAME = viewModel.LastName;
            employee.EMPLOYEE_ADDRESS = viewModel.Address;
            employee.EMPLOYEE_CITY = viewModel.City;
            employee.EMPLOYEE_ZIP = viewModel.Zip.ToString();
            employee.EMPLOYEE_HIRE_DATE = viewModel.HireDate;
            employee.EMPLOYEE_TERM_DATE = viewModel.TermDate;
            if (false == EmployeeController.UpdateEmployee(employee, requester))
            {
                Log.Error("Failed to save udated part 1 employee info for employee id: " + employee.EMPLOYEE_ID);
            }

            SingleEmployee1095summaryForTaxYearRequest request = new SingleEmployee1095summaryForTaxYearRequest();
            request.EmployerId = employerId;
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);
            request.ResourceId = ResourceId;
            request.Requester = requester;

            ModelListResponse<Employee1095summaryModel> message = this.ApiHelper.Send<ModelListResponse<Employee1095summaryModel>, EmployerTaxYearRequest>(GetSingleKey, request);

            Employee1095summaryViewModel viewModels = Mapper.Map<Employee1095summaryModel, Employee1095summaryViewModel>(message.Models.SingleOrDefault());

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
        [HttpPut]
        public ActionResult SavePart2UserEdits(String encryptedParameters)
        {

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));

            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());

            string json = new StreamReader(Request.InputStream).ReadToEnd();
            List<Employee1095detailsPart2ViewModel> viewModels = JsonConvert.DeserializeObject<List<Employee1095detailsPart2ViewModel>>(json);
            List<Employee1095detailsPart2Model> models = Mapper.Map<List<Employee1095detailsPart2ViewModel>, List<Employee1095detailsPart2Model>>(viewModels);
            

            UpdateManyPart2Request request = new UpdateManyPart2Request();
            request.Requester = requester;
            request.EmployerId = employerId;
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);
            request.ResourceId = ResourceId;

            request.models = models;
            
            ModelListResponse<Employee1095detailsPart2Model> message = this.ApiHelper.Send<ModelListResponse<Employee1095detailsPart2Model>, UpdateManyPart2Request>(
                UpdtePart2Key,
                request);

            viewModels = Mapper.Map<List<Employee1095detailsPart2Model>, List<Employee1095detailsPart2ViewModel>>(message.Models.ToList());

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
        [HttpGet]
        public ActionResult GetPart2ForEmployee(String encryptedParameters)
        {

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));

            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());

            SingleEmployee1095summaryForTaxYearRequest request = new SingleEmployee1095summaryForTaxYearRequest();
            request.EmployerId = employerId;
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);

            request.ResourceId = ResourceId;

            request.Requester = requester;

            ModelListResponse<Employee1095detailsPart2Model> message = this.ApiHelper.Send<ModelListResponse<Employee1095detailsPart2Model>, SingleEmployee1095summaryForTaxYearRequest>(GetPart2ByEmployeeIdKey, request);

            IList<Employee1095detailsPart2ViewModel> viewModels = Mapper.Map<IList<Employee1095detailsPart2Model>, IList<Employee1095detailsPart2ViewModel>>(message.Models);

            string parentParameters = encryptedParameters;

            foreach (var viewModel in viewModels)
            {
                viewModel.SummaryEncyptedParameters = parentParameters;
            }

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
        [HttpGet]
        public ActionResult GetPart3ForEmployee(String encryptedParameters)
        {

            base.Load(encryptedParameters);

            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));

            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());

            SingleEmployee1095summaryForTaxYearRequest request = new SingleEmployee1095summaryForTaxYearRequest();
            request.EmployerId = employerId;
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);

            request.ResourceId = ResourceId;

            request.Requester = requester;

            ModelListResponse<Employee1095detailsPart3Model> message = this.ApiHelper.Send<ModelListResponse<Employee1095detailsPart3Model>, SingleEmployee1095summaryForTaxYearRequest>(GetPart3ByEmployeeIdKey, request);

            IList<Employee1095detailsPart3ViewModel> viewModels = Mapper.Map<IList<Employee1095detailsPart3Model>, IList<Employee1095detailsPart3ViewModel>>(message.Models);

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
        [HttpPut]
        public ActionResult UpdatePart3InsuranceCoverage(String encryptedParameters)
        {

            base.Load(encryptedParameters);
            //// Create the Request 
            UpdateItemRequest<Employee1095detailsPart3Model> request = new UpdateItemRequest<Employee1095detailsPart3Model>();

            int TaxYear = int.Parse(this.EncryptedParameters["TaxYear"].ToString());
            int EmployeeID = int.Parse(this.EncryptedParameters["EmployeeID"].ToString());
            int DependantID = int.Parse(this.EncryptedParameters["DependantID"].ToString());
            Guid ResourceID = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            string json = new StreamReader(Request.InputStream).ReadToEnd();
            Employee1095detailsPart3ViewModel viewModel = JsonConvert.DeserializeObject<Employee1095detailsPart3ViewModel>(json);
            Employee1095detailsPart3Model model = Mapper.Map<Employee1095detailsPart3ViewModel, Employee1095detailsPart3Model>(viewModel);
            model.TaxYear = TaxYear;
            model.EmployeeID = EmployeeID;
            model.DependantID = DependantID;
            model.ResourceId = ResourceID;
            request.model = model;

            request.Requester = requester;
            this.Log.Info(String.Format("Part3 Update from UI controller:EmployeeID: {0},ResourceID: {1}, DependantID: {2},TaxYear:{3} ",
                                            EmployeeID,
                                            ResourceID,
                                            DependantID,
                                            TaxYear
                                        )
                                );

            ModelItemResponse<Employee1095detailsPart3Model> message = this.ApiHelper.Send<ModelItemResponse<Employee1095detailsPart3Model>, UpdateItemRequest<Employee1095detailsPart3Model>>(
                UpdateInsuranceCoverageKey,
                request);

            model = message.Model;

            viewModel = Mapper.Map<Employee1095detailsPart3Model, Employee1095detailsPart3ViewModel>(model);

            string jsonOut = "";

            var ser = JsonSerializer.CreateDefault();
            using (var str = new StringWriter())
            {
                ser.Serialize(str, viewModel);
                jsonOut = str.ToString();
            }

            var result = this.Content(jsonOut, "application/json");

            return result;

        }
        
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpDelete]
        public ActionResult DeletePart3InsuranceCoverage(String encryptedParameters)
        {

            base.Load(encryptedParameters);

            Employee1095detailsPart3Model model = new Employee1095detailsPart3Model();
            int EmployeeID = int.Parse(this.EncryptedParameters["EmployeeID"].ToString());
            int DependantID = int.Parse(this.EncryptedParameters["DependantID"].ToString());
            int TaxYear = int.Parse(this.EncryptedParameters["TaxYear"].ToString());
            model.EmployeeID = EmployeeID;
            model.DependantID = DependantID;
            model.TaxYear = TaxYear;

            UpdateItemRequest<Employee1095detailsPart3Model> request = new UpdateItemRequest<Employee1095detailsPart3Model>();
            request.model = model;
            request.Requester = requester;
            this.Log.Info(String.Format("Part3 Delete from UI controller:EmployeeID: {0}, DependantID: {1} ",
                                           EmployeeID,
                                           DependantID
                                       )
                               );
            BasicResponse message = this.ApiHelper.Send<BasicResponse, UpdateItemRequest<Employee1095detailsPart3Model>>(
                DeleteInsuranceCoverageKey,
                request);

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
            
            return null;

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult AddPart3InsuranceCoverage(String encryptedParameters)
        {

            base.Load(encryptedParameters);
            //// Create the Request 
            NewChildItemRequest<Employee1095detailsPart3Model> request = new NewChildItemRequest<Employee1095detailsPart3Model>();

            string json = new StreamReader(Request.InputStream).ReadToEnd();
            Employee1095detailsPart3ViewModel viewModel = JsonConvert.DeserializeObject<Employee1095detailsPart3ViewModel>(json);
            Employee1095detailsPart3Model model = Mapper.Map<Employee1095detailsPart3ViewModel, Employee1095detailsPart3Model>(viewModel);
            model.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);
            request.model = model;
            request.ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());

            request.Requester = requester;
            this.Log.Info(String.Format("Part3 Delete from UI controller:EmployeeID: {0}, DependantID: {1} ",
                                          model.EmployeeID,
                                          model.DependantID
                                      )
                              );
            ModelItemResponse<Employee1095detailsPart3Model> message = this.ApiHelper.Send<ModelItemResponse<Employee1095detailsPart3Model>, NewChildItemRequest<Employee1095detailsPart3Model>>(
                AddInsuranceCoverageKey,
                request);

            model = null;
            if (message.Status == "OK")
            {
                model = message.Model;
            }

            viewModel = Mapper.Map<Employee1095detailsPart3Model, Employee1095detailsPart3ViewModel>(model);

            string jsonOut = "";

            var ser = JsonSerializer.CreateDefault();
            using (var str = new StringWriter())
            {
                ser.Serialize(str, viewModel);
                jsonOut = str.ToString();
            }

            var result = this.Content(jsonOut, "application/json");

            return result;
        }

    }

}
