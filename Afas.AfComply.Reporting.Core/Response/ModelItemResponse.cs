using Afas.AfComply.Reporting.Core.Models;
using Afc.Marketing.Models;
using Afc.Marketing.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Response
{
    /// <summary>
    /// Response message containing the requested timeframes
    /// </summary>
    public class ModelItemResponse<TModel> : BaseResponseMessage
        where TModel : Model
    {
        /// <summary>
        /// Default Contructor
        /// </summary>
        public ModelItemResponse() : base() {}

        /// <summary>
        /// The requested items
        /// </summary>
        [Required]
        public virtual TModel Model { get; set; }

    }
}
