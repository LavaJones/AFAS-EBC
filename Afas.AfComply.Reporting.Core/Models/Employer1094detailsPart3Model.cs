using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class Employer1094detailsPart3Model : Model
    {
        public Employer1094detailsPart3Model() : base() { }
        public  int MonthId { set; get; }
        public int TaxYear { get; set; }
        public double? MEC95 { get; set; }
        public bool MinimumEssentialCoverageOfferIndicator { get; set; }
        public int? FullTimeEmployeeCount { get; set; }
        public int? TotalEmployeeCount { get; set; }
        public bool AggregatedGroupIndicator { get; set; }

    }
}
