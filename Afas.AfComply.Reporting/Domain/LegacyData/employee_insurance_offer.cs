//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Afas.AfComply.Reporting.Domain.LegacyData
{
    using System;
    using System.Collections.Generic;
    
    public partial class employee_insurance_offer
    {
        public int rowid { get; set; }
        public int employee_id { get; set; }
        public int plan_year_id { get; set; }
        public Nullable<int> employer_id { get; set; }
        public Nullable<int> insurance_id { get; set; }
        public Nullable<int> ins_cont_id { get; set; }
        public Nullable<decimal> avg_hours_month { get; set; }
        public Nullable<bool> offered { get; set; }
        public Nullable<System.DateTime> offeredOn { get; set; }
        public Nullable<bool> accepted { get; set; }
        public Nullable<System.DateTime> acceptedOn { get; set; }
        public Nullable<System.DateTime> modOn { get; set; }
        public string modBy { get; set; }
        public string notes { get; set; }
        public string history { get; set; }
        public Nullable<System.DateTime> effectiveDate { get; set; }
        public Nullable<decimal> hra_flex_contribution { get; set; }
        public System.Guid ResourceId { get; set; }
    
        public virtual employee employee { get; set; }
        public virtual employee_insurance_offer employee_insurance_offer1 { get; set; }
        public virtual employee_insurance_offer employee_insurance_offer2 { get; set; }
        public virtual employer employer { get; set; }
        public virtual insurance_contribution insurance_contribution { get; set; }
        public virtual plan_year plan_year { get; set; }
        public virtual insurance insurance { get; set; }
    }
}
