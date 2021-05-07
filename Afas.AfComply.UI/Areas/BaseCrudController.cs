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
    [CookieTokenAuthCheckAttribute]
    public abstract class BaseCrudController<TModel,TViewModel> : BaseReadOnlyController<TModel, TViewModel>
        where TModel : Model 
        where TViewModel : BaseViewModel
    {

        protected string GetAddNewKey;
        protected string GetUpdateKey;
        protected string GetDeleteKey;

        public BaseCrudController(ILogger logger,
                IEncryptedParameters encryptedParameters,
                IApiHelper apiHelper
            ) 
            : base(logger, encryptedParameters, apiHelper)
        {
        }


        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public virtual ActionResult Add()
        {

            string json = new StreamReader(Request.InputStream).ReadToEnd();
            NewItemRequest<TModel> request = new NewItemRequest<TModel>();
            
            TViewModel viewModel = JsonConvert.DeserializeObject<TViewModel>(json);
            TModel model = Mapper.Map<TViewModel, TModel>(viewModel);
            request.model = model;

            request.Requester = requester;

            ModelItemResponse<TModel> message = this.ApiHelper.Send<ModelItemResponse<TModel>, NewItemRequest<TModel>>(
                GetAddNewKey, 
                request);

            model = message.Model;
            viewModel = Mapper.Map<TModel, TViewModel>(model);
            
            return Json(viewModel);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPut]
        public virtual ActionResult Update(String encryptedParameters)
        {

            base.Load(encryptedParameters);
            UpdateItemRequest<TModel> request = new UpdateItemRequest<TModel>();

            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            string json = new StreamReader(Request.InputStream).ReadToEnd();
            TViewModel viewModel = JsonConvert.DeserializeObject<TViewModel>(json);
            TModel model = Mapper.Map<TViewModel, TModel>(viewModel);
            model.ResourceId = ResourceId;
            request.model = model;

            request.Requester = requester; 

            ModelItemResponse<TModel> message = this.ApiHelper.Send<ModelItemResponse<TModel>, UpdateItemRequest<TModel>>(
                GetUpdateKey,
                request);

            model = message.Model;
            viewModel = Mapper.Map<TModel, TViewModel>(model);

            return Json(viewModel);

        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpDelete]
        public virtual ActionResult Delete(String encryptedParameters)
        {

            base.Load(encryptedParameters);
            DeleteItemRequest<TModel> request = new DeleteItemRequest<TModel>();

            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            request.ResourceId = ResourceId;

            request.Requester = requester; 
            
            ModelItemResponse<TModel> message = this.ApiHelper.Send<ModelItemResponse<TModel>, DeleteItemRequest<TModel>>(
                GetDeleteKey,
                request);

            TModel model = message.Model;
            TViewModel viewModel = Mapper.Map<TModel, TViewModel>(model);

            return Json(viewModel);

        }
    }
}