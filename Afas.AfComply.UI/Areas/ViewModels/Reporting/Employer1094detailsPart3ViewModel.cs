using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{
    public class Employer1094detailsPart3ViewModel : BaseViewModel
    {
        public Employer1094detailsPart3ViewModel() : base() { }       
        public int MonthId { set; get; }
        public int TaxYear { get; set; }

        public double? MEC95 { get; set; }
        public bool MinimumEssentialCoverageOfferIndicator { get; set; }
        public int FullTimeEmployeeCount { get; set; }
        public int TotalEmployeeCount { get; set; }
        public bool AggregatedGroupIndicator { get; set; }
    }
}