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
    public abstract class BaseGuidRequest : BaseRequest
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public BaseGuidRequest() : base() { }

        /// <summary>
        /// The Resource Id.
        /// </summary>
        public virtual Guid ResourceId { get; set; }



    }
}
