using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class offered_Insurance
    {
        public int employee_id { get; set; }
        public int employer_id { get; set; }
        public string InsuranceDescription { get; set; }
        public string PlanYearDescryption { get; set; }
        public decimal? InsuranceContributionAmount { get; set; }
        public decimal? Monthlycost { get; set; }
        public decimal? avg_hours_month { get; set; }
        public decimal? hra_flex_contribution { get; set; }
        public Boolean offered { get; set; }
        public DateTime? offeredOn { get; set; }
        public Boolean accepted { get; set; }
        public DateTime? acceptedOn { get; set; }
        public string notes { get; set; }
        public string history { get; set; }
        public DateTime? effectiveDate { get; set; }
        public string offeredDatePart
        {
            get { return offeredOn.HasValue ? offeredOn.Value.ToShortDateString():string.Empty; }
        }

        public string acceptedDatePart
        {
            get { return acceptedOn.HasValue ? acceptedOn.Value.ToShortDateString() : string.Empty; }
        }

        public string effectiveDatePart
        {
            get { return effectiveDate.HasValue ? effectiveDate.Value.ToShortDateString() : string.Empty; }
        }

        public Boolean offeredbool
        {
            get { return offered; }
        }
    }
