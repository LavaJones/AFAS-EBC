using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Marketing.Models;
using System.ComponentModel.DataAnnotations;
namespace Afas.AfComply.Reporting.Core.Models
{
    public class Approved1094FinalPart3Model:Model
    {

        [Required]
        public virtual Approved1094FinalPart1Model Approved1094FinalPart1 { get; set; }

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
