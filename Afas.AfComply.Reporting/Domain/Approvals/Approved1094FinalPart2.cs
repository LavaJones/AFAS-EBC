using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Afc.Core;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public class Approved1094FinalPart2 : BaseReportingModel
    {

      

        [Required]
        public virtual Approved1094FinalPart1 Approved1094FinalPart1 { get; set; }

        [Required]
        public virtual int Total1095Forms { get; set; }

        [Required]
        public virtual Boolean IsAggregatedAleGroup { get; set; }
    }

}
