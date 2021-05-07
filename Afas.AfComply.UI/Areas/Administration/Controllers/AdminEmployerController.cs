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
using System.Web.UI;
using Afc.Framework.Presentation.Web;
using Afas.AfComply.UI.Areas.Reporting;
using Afas.Domain.POCO;

namespace Afas.AfComply.UI.Areas.Administration.Controllers
{
    [CookieTokenAdminAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class AdminEmployerController : BaseMvcController 
    {
        protected IApiHelper ApiHelper { get; private set; }
        public string GetAllEmployersKey { get; set; }
        public AdminEmployerController(ILogger logger,
                IEncryptedParameters encryptedParameters,
                IApiHelper apiHelper
            )
           : base(logger, encryptedParameters)
        {
            GetAllEmployersKey = "get-all-ActiveEmployers";
            SharedUtilities.VerifyObjectParameter(apiHelper, "ApiHelper");

            this.ApiHelper = apiHelper;

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public ActionResult GetEmployerIdList()
        {

            List<EmployerIdSelectViewModel> viewModels = new List<EmployerIdSelectViewModel>();

            List<employer> employers = employerController.getAllEmployers();

            foreach (employer emp  in employers)
            {
                EmployerIdSelectViewModel model = new EmployerIdSelectViewModel();

                Dictionary<string,string> encParams = new Dictionary<string, string>();

                encParams.Add("EmployerResourceId", emp.ResourceId.ToString());
                encParams.Add("EmployerId", emp.EMPLOYER_ID.ToString());

                model.EmployerName = emp.EMPLOYER_NAME;
                model.EncryptedId = EncryptedParametersHelper.AsMvcParameter(encParams);

                viewModels.Add(model);
            }
            return Json(viewModels, JsonRequestBehavior.AllowGet);

        }

 

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public ActionResult GetAll1095FinalizedEmployers()
        {

            List<EmployerIdSelectViewModel> viewModels = new List<EmployerIdSelectViewModel>();

            List<employer> employers = employerController.getAll1095FinalizedEmployers();

            foreach (employer emp in employers)
            {
                EmployerIdSelectViewModel model = new EmployerIdSelectViewModel();

                Dictionary<string, string> encParams = new Dictionary<string, string>();

                encParams.Add("EmployerResourceId", emp.ResourceId.ToString());
                encParams.Add("EmployerId", emp.EMPLOYER_ID.ToString());

                model.EmployerName = emp.EMPLOYER_NAME;
                model.EIN = emp.EMPLOYER_EIN;
                model.Address = emp.EMPLOYER_ADDRESS;
                model.City = emp.EMPLOYER_CITY;
                model.State = ((UsStateAbbreviationEnum)emp.EMPLOYER_STATE_ID).ToString();
                model.Zip = emp.EMPLOYER_ZIP;
                model.EncryptedId = EncryptedParametersHelper.AsMvcParameter(encParams);

                viewModels.Add(model);
            }
            return Json(viewModels, JsonRequestBehavior.AllowGet);

        }

    }

}