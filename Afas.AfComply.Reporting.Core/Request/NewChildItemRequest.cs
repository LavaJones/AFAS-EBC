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
    /// Standard Request that supplies a guid
    /// </summary>
    public class NewChildItemRequest<TModel> : NewItemRequest<TModel> where TModel : Model
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public NewChildItemRequest() : base() { }

        /// <summary>
        /// The Resource Id.
        /// </summary>
        public virtual Guid ResourceId { get; set; }

    }
}
