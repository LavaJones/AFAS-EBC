using Afas.AfComply.Reporting.Core.Models;
using Afc.Marketing.Models;
using Afc.Marketing.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Request
{
    /// <summary>
    /// Standard Request for creating a new TimeFrame
    /// </summary>
    public abstract class BaseModelRequest<TModel> : BaseRequest
        where TModel : Model
    {
        
        /// <summary>
        /// default constructor
        /// </summary>
        public BaseModelRequest() : base() { }

        /// <summary>
        /// The model for this request
        /// </summary>
        public virtual TModel model { get; set; }
        
    }
}
