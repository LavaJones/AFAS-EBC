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
    /// Response message containing 
    /// </summary>
    public class UIMessageResponse : BaseResponseMessage
    {
        /// <summary>
        /// Default Contructor
        /// </summary>
        public UIMessageResponse() : base() {}

        /// <summary>
        /// The message to return to the UI about suceess or failure
        /// </summary>
        public string UIMessage { get; set; }

    }
}
