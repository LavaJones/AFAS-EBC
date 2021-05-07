using System;
using System.ComponentModel.DataAnnotations;

using Afc.Core;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public class Approved1094FinalPart4 : BaseReportingModel
    {

       
        [Required]
        public virtual Approved1094FinalPart1 Approved1094FinalPart1 { get; set; }

        [Required]
        public virtual String EIN { set; get; }

        [Required]
        public virtual String EmployerName { get; set; }

    }

}
