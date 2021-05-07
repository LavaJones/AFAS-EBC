using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels
{

    public class Approved1094FinalPart3ViewModel : BaseViewModel
   
    {
       
        public int MonthId { set; get; }
        public int TaxYear { get; set; }

        public int? MEC95 { get; set; }
        public bool MinimumEssentialCoverageOfferIndicator { get; set; }
        public int FullTimeEmployeeCount { get; set; }
        public int TotalEmployeeCount { get; set; }
        public bool AggregatedGroupIndicator { get; set; }
    }
}