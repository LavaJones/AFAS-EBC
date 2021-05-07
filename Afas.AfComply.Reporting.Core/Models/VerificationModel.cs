using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Afc.Marketing.Models;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class VerificationModel:Model
    {
        public virtual string Step { get; set; }
        public virtual bool Status { get; set; }
        public virtual string StatusString { get; set; }
        public virtual string Instructions { get; set; }
    }
}