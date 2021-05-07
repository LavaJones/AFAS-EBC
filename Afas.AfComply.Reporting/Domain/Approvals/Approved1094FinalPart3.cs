using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    public class Approved1094FinalPart3 : BaseReportingModel
    {

       

        [Required]
        public virtual Approved1094FinalPart1 Approved1094FinalPart1 { get; set; }

        [Required]
        public virtual int MonthId { set; get; }

        [Required]
        public virtual Boolean MinimumEssentialCoverageOfferIndicator { get; set; }

        [Required]
        public virtual int FullTimeEmployeeCount { get; set; }

        [Required]
        public virtual int TotalEmployeeCount { get; set; }

        [Required]
        public virtual Boolean AggregatedGroupIndicator { get; set; }

    }

}
