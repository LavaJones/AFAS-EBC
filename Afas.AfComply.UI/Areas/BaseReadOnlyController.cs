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
using Afas.AfComply.Reporting.Core.Request;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using AutoMapper;
using Afas.AfComply.UI.Areas.ViewModels;
using Afc.Marketing.Models;
using System.Web.UI;

namespace Afas.AfComply.UI.Areas.Administration.Controllers
{

    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public abstract class BaseReadOnlyController<TModel,TViewModel> : BaseMvcController 
        where TModel : Model 
        where TViewModel : BaseViewModel
    {

        protected string GetAllKey;
        protected string GetManyKey;
        protected string GetByIdKey;

        protected string requester { get { return CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext); } }
        protected Guid employerResourceId { get { return Guid.Parse(CookieTokenAuthCheckAttribute.GetEmployerResourceId(this.HttpContext)); } }

        public BaseReadOnlyController(ILogger logger,
                IEncryptedParameters encryptedParameters,
                IApiHelper apiHelper
            ) 
            : base(logger, encryptedParameters)
        {

            SharedUtilities.VerifyObjectParameter(apiHelper, "ApiHelper");
            
            this.ApiHelper = apiHelper;

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public virtual ActionResult GetAll()
        {

            ModelListResponse<TModel> message = this.ApiHelper.Retrieve<ModelListResponse<TModel>>(GetAllKey);
            IList<TViewModel> viewModels = Mapper.Map<IList<TModel>, IList <TViewModel>>(message.Models);

            return Json(viewModels, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public virtual ActionResult GetMany(String encryptedParameters)
        {
            
            base.Load(encryptedParameters);

            ModelListResponse<TModel> message = this.ApiHelper.Retrieve<ModelListResponse<TModel>>(GetManyKey);
            IList<TViewModel> viewModels = Mapper.Map<List<TModel>, List<TViewModel>>(message.Models.ToList());

            return Json(viewModels, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public virtual ActionResult GetSingle(String encryptedParameters)
        {

            base.Load(encryptedParameters);
            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());

            ModelItemResponse<TModel> message = this.ApiHelper.Retrieve<ModelItemResponse<TModel>>(GetByIdKey, ResourceId);
            TViewModel viewModel = Mapper.Map<TModel, TViewModel>(message.Model);

            return Json(viewModel, JsonRequestBehavior.AllowGet);

        }

        protected IApiHelper ApiHelper { get; private set; }

    }
}