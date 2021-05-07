using Afas.AfComply.UI.Areas.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{

    /// <summary>
    /// Represents a single month of data for  the 1095C Part 2 Data
    /// </summary>
    public class Employee1095detailsPart2ViewModel : BaseViewModel
    {
        public Employee1095detailsPart2ViewModel() : base() { }

        /// <summary>
        /// The Id for the Month (Jan = 1, .... Dec = 12)
        /// </summary>
        public int MonthId { get; set; }

        /// <summary>
        /// The line 14 value
        /// </summary>
        public string Line14 { get; set; }

        /// <summary>
        /// The line 15 value
        /// </summary>
        public string Line15 { get; set; }

        /// <summary>
        /// The line 16 value
        /// </summary>
        public string Line16 { get; set; }

        /// <summary>
        /// If their Offered covereage was Self or Fully
        /// </summary>
        public string InsuranceType { get; set; }

        /// <summary>
        /// The employees status during this month.
        /// </summary>
        public AcaStatus AcaStatus { get; set; }
        
        /// <summary>
        /// If the employee was offered acceptable coverage 
        /// </summary>
        public bool Offered { get; set; }

        /// <summary>
        /// If the employee accepted the offer of coverage.
        /// </summary>
        public bool Enrolled { get; set; }

        /// <summary>
        /// The average monthly hours calculated for this stability period
        /// </summary>
        public double MonthlyHours { get; set; }

        /// <summary>
        /// Has the user edited the data for this employee and month so that the system values are overriden.
        /// </summary>
        public bool UserEdited { get; set; }

        /// <summary>
        /// If this Month is causing this employee to recieve a 1095C form.
        /// </summary>
        public bool? Receiving1095C { get; set; }

        /// <summary>
        /// The Tax year that this data is for.
        /// </summary>
        public int TaxYear { get; set; }

        /// <summary>
        /// This holds the Summary records EncyptedLink
        /// </summary>
        public string SummaryEncyptedParameters { get; set; }

        public override string GetEncryptedParameters(Dictionary<string, string> Params)
        {
            if (null == Params)
            {
                return this.SummaryEncyptedParameters;
            }
            else
            {
                return base.GetEncryptedParameters(Params);
            }
        }
    }
}