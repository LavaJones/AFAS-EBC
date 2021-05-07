using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{
    public class VerificationViewModel : BaseViewModel
    {
       public virtual string Step { get; set; }
       public virtual bool Status { get; set; }
        public virtual string StatusString { get; set; }
        public virtual string Instructions { get; set; }
    }
}