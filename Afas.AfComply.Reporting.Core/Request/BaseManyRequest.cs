using Afas.AfComply.Reporting.Core.Models;
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
    public abstract class BaseManyRequest<TModel> : BaseRequest
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public BaseManyRequest() : base() { }

        /// <summary>
        /// The Resource Id.
        /// </summary>
        public virtual List<Guid> AllResourceIds { get; set; }
        
    }
}
