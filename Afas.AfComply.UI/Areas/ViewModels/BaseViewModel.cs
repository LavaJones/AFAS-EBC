using Afas.AfComply.UI.Areas.Reporting;
using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;
using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Afas.AfComply.UI.Areas.ViewModels
{

    public abstract class BaseViewModel 
    {

        public BaseViewModel() : base() { }

        public Guid ResourceId { private get; set; }

        /// <summary>
        /// Link to the get method with the encrypted parameters attached to the end
        /// </summary>
        public virtual string GetSingleItemLink
        {
            get
            {
                return this.GetEncyptedLink("GetSingle");
            }
        }

        /// <summary>
        /// Link to the update method with the encrypted parameters attached to the end
        /// </summary>
        public virtual string UpdateItemLink
        {
            get
            {
                return this.GetEncyptedLink("Update");
            }
        }

        /// <summary>
        /// Link to the delete method with the encrypted parameters attached to the end
        /// </summary>
        public virtual string DeleteItemLink
        {
            get
            {
                return this.GetEncyptedLink("Delete");
            }
        }

        /// <summary>
        /// Gets the name of the Item, minus the 'Model' or 'ViewModel' addition
        /// </summary>
        /// <returns>The clean name of this object.</returns>
        public virtual string CleanName()
        {
            string name = this.GetType().Name;

            if (name.Contains("ViewModel")) { name = name.Replace("ViewModel", ""); }
            if (name.Contains("View")) { name = name.Replace("View", ""); }
            if (name.Contains("Model")) { name = name.Replace("Model", ""); }

            return name;
        }

        /// <summary>
        /// Gets the full name of the expecteded controller
        /// </summary>
        /// <returns>The controller name of this object.</returns>
        public virtual string ControllerName()
        {
            return this.CleanName() + "Controller";
        }

        /// <summary>
        /// This is the url parameter for this object
        /// </summary>
        public virtual string ThisUrlParameter
        {
            get
            {
                return GetEncryptedParameters(null);
            }
        }


        /// <summary>
        /// Creates a link with encrypted values apended to the end
        /// </summary>
        /// <param name="Action">The name of the action to be called.</param>
        /// <param name="Params">Any Parameters to be encrypted into the url</param>
        /// <returns>A url to the action that has the encrypted params attached.</returns>
        public virtual string GetEncyptedLink(string Action, Dictionary<string, string> Params = null)
        {

            UrlHelper url = UrlHelperHelper.GetUrlHelper();

            if (Params != null)
            {
                var result =  this.CleanName() + "Api/" + Action + "/" + url.Encode(this.GetEncryptedParameters(Params));
                return result;
            }
            else
            {
                var result =  this.CleanName() + "Api/" + Action + "/" + url.Encode(this.ThisUrlParameter);
                return result;
            }
        }

        /// <summary>
        /// Gets the Encrypted Parma
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public virtual string GetEncryptedParameters(Dictionary<string, string> Params = null)       
        {

            if (null == Params && null != _encryptedParameter)
            {
                return _encryptedParameter;
            }

            if (null != Params)
            {
                if (!Params.ContainsKey("ResourceId"))
                { 
                    Params.Add("ResourceId", this.ResourceId.ToString());
                }
                return EncryptedParametersHelper.AsMvcParameter(Params);
            }
            else
            {
                string asMvc = EncryptedParametersHelper.AsMvcParameter("ResourceId", this.ResourceId.ToString());
                _encryptedParameter = asMvc;
                return asMvc;
            }
            
        }

        /// <summary>
        /// This is a backing variable that stored calculated encrypted parameter for null params
        /// </summary>
        protected string _encryptedParameter = null;
    }
}