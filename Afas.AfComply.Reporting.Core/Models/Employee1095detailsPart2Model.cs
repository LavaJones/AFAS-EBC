using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.Reporting.Core.Models
{

    /// <summary>
    /// Represents a single month of data for  the 1095C Part 2 Data
    /// </summary>
    public class Employee1095detailsPart2Model : Model
    {
        public Employee1095detailsPart2Model() : base() { }

        /// <summary>
        /// The Id for the Month (Jan = 1, .... Dec = 12)
        /// </summary>
        public int MonthId { get; set; }

        /// <summary>
        /// The 
        /// </summary>
        public int EmployeeId { get; set; }

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
        public int AcaStatus { get; set; }
        
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
    }
}