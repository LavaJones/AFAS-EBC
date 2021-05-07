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
    /// Standard Request for deleteing a TimeFrame
    /// </summary>
    public class DeleteItemRequest<TModel> : BaseGuidRequest
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public DeleteItemRequest() : base() { }  
              
    }
}
