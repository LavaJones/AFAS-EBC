using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Afc.Core.Presentation.Web;
using Newtonsoft.Json;
using System.IO;
using Afas.AfComply.UI.Areas.ViewModels.Reporting;
using System.Web.Mvc;
using AutoMapper;
using Afc.Core.Logging;
using System.Web.UI;

namespace Afas.AfComply.UI.Areas.Reporting.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ConfirmationPageController : BaseMvcController
    {
        private ILog IRSLog = log4net.LogManager.GetLogger(typeof(ConfirmationPageController));
        public ConfirmationPageController(ILogger logger,
                IEncryptedParameters encryptedParameters
            )
            : base(logger, encryptedParameters)
        { }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public void Confirm()
        {
            string username = CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext);
            int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetObjectByKey(this.HttpContext, "EmployerId"));
            var TaxYearId = Feature.CurrentReportingYear;

            Log.Info(String.Format("Confirming IRS data: UserName:[{0}], EmployerID: [{1}], TaxYearId:[{2}]", username, employerId, TaxYearId ));
            PIILogger.LogPII(String.Format("Confirming IRS data: UserName:[{0}], EmployerID: [{1}], TaxYearId:[{2}]", username, employerId, TaxYearId));

            var currentEmployerTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, TaxYearId);
            if (currentEmployerTransmissionStatus == null)
            {
                initiateEmployerTransaction(username, employerId);
            }

            EmployerTaxYearTransmissionStatus currentEmployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerId, TaxYearId);
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
        [HttpGet]
        public JsonResult GetAllIRSContactUser()
        {
            int EMPLOYER_ID = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            List<User> users = UserController.getDistrictUsers(EMPLOYER_ID);
            List<User> IrsContacts = (from User contact in users where contact.User_IRS_CONTACT == true select contact).ToList();
            return Json(IrsContacts, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public JsonResult GetAllIRSSafeHarborCodes()
        {
            int EMPLOYER_ID = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            employer employer = employerController.getEmployer(EMPLOYER_ID);
            List<classification> classifications = classificationController.ManufactureEmployerClassificationList(EMPLOYER_ID, true);



            IList<ConfirmationClassificationViewModel> classificationsa = Mapper.Map<IList<classification>, IList<ConfirmationClassificationViewModel>>(classifications).Where(c => c.CLASS_AFFORDABILITY_CODE !=null || c.CLASS_AFFORDABILITY_CODE !="").ToList();

            List<ConfirmationClassificationViewModel> TwoG = classificationsa.Where(c => c.CLASS_AFFORDABILITY_CODE!= null && c.CLASS_AFFORDABILITY_CODE.ToLower() == "2g").ToList();

            List<PlanYear> planYears = PlanYear_Controller.getEmployerPlanYear(employer.EMPLOYER_ID);
            foreach (PlanYear year in planYears)
            {
                if (year.PLAN_YEAR_START < new DateTime(Feature.CurrentReportingYear, 12, 31) && year.PLAN_YEAR_END > new DateTime(Feature.CurrentReportingYear, 1, 1))
                {
                    foreach (insurance ins in insuranceController.manufactureInsuranceList(year.PLAN_YEAR_ID))
                    {
                        foreach (insuranceContribution contribution in insuranceController.manufactureInsuranceContributionList(ins.INSURANCE_ID))
                        {
                            foreach (ConfirmationClassificationViewModel classy in TwoG)
                            {
                                if (contribution.INS_CONT_CLASSID == classy.CLASS_ID)
                                {
                                    decimal price = 0;
                                    if (contribution.INS_CONT_CONTRIBUTION_ID.Equals("%"))
                                    {
                                        price = (decimal)(1.0 - (contribution.INS_CONT_AMOUNT / 100.0)) * ins.INSURANCE_COST;
                                    }
                                    else
                                    {
                                        price = ins.INSURANCE_COST - (decimal)contribution.INS_CONT_AMOUNT;
                                    }

                                    if (price > (decimal)95.63)
                                    {
                                        foreach (ConfirmationClassificationViewModel row in classificationsa)
                                        {
                                                if (row.CLASS_ID == classy.CLASS_ID)
                                                {
                                                    row.CLASS_2GInValidPrice = true;
                                                }
                                         
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return Json(classificationsa, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public JsonResult GetEmployerName()
        {

            int EMPLOYER_ID = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            employer employer = employerController.getEmployer(EMPLOYER_ID);
            return Json(employer.EMPLOYER_NAME, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public JsonResult GetAllEmployees()
        {
            int EMPLOYER_ID = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            List<Employee> employees = EmployeeController.manufactureEmployeeList(EMPLOYER_ID);
            DateTime ReportingEnd = new DateTime(Feature.CurrentReportingYear, 12, 31);       
            const int COBRA = 4;
            const int FULLTIME = 5;
            const int RETIRED = 8;

            IList<Employee> nonRetiredNonCobraNonFulltimeEmployees = (
                                                                      from Employee emp in employees
                                                                      where
                                                                          emp.EMPLOYEE_IMP_END >= ReportingEnd
                                                                            &&
                                                                          emp.EMPLOYEE_ACT_STATUS_ID != COBRA
                                                                            &&
                                                                          emp.EMPLOYEE_ACT_STATUS_ID != RETIRED
                                                                            &&
                                                                          emp.EMPLOYEE_ACT_STATUS_ID != FULLTIME
                                                                      select emp
                                                                     ).ToList();
            return Json(nonRetiredNonCobraNonFulltimeEmployees, JsonRequestBehavior.AllowGet);

        }



        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public JsonResult SaveEmployees()
        {
            DateTime ModOn = DateTime.Now;
            string ModBy = CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext);
            string json = new StreamReader(Request.InputStream).ReadToEnd();
            List<ConfirmationIRSEmployees> viewModel = JsonConvert.DeserializeObject<List<ConfirmationIRSEmployees>>(json);
            foreach (ConfirmationIRSEmployees employee in viewModel)
            {
               EmployeeController.UpdateEmployeeClassAcaStatus(employee.EMPLOYEE_EMPLOYER_ID, employee.EMPLOYEE_ID, employee.EMPLOYEE_CLASS_ID, employee.EMPLOYEE_ACT_STATUS_ID, ModBy, ModOn);
            }
            return GetAllEmployees();

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public JsonResult DoAlertExists()
        {
            int EMPLOYER_ID = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
            var alerts = alert_controller.manufactureEmployerAlertList(EMPLOYER_ID);
            if (alerts.Count > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
