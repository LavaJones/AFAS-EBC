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
    public class NewItemRequest<TModel> : BaseModelRequest<TModel>
        where TModel : Model
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public NewItemRequest() : base() { }

    }
}
