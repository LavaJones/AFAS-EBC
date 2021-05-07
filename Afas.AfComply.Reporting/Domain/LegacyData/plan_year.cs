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
    
    public partial class plan_year
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public plan_year()
        {
            this.employees = new HashSet<employee>();
            this.employees1 = new HashSet<employee>();
            this.employees2 = new HashSet<employee>();
            this.employee_insurance_offer = new HashSet<employee_insurance_offer>();
            this.insurances = new HashSet<insurance>();
        }
    
        public int plan_year_id { get; set; }
        public int employer_id { get; set; }
        public string description { get; set; }
        public System.DateTime startDate { get; set; }
        public System.DateTime endDate { get; set; }
        public string notes { get; set; }
        public string history { get; set; }
        public Nullable<System.DateTime> modOn { get; set; }
        public string modBy { get; set; }
        public System.Guid ResourceId { get; set; }
        public Nullable<System.DateTime> default_meas_start { get; set; }
        public Nullable<System.DateTime> default_meas_end { get; set; }
        public Nullable<System.DateTime> default_admin_start { get; set; }
        public Nullable<System.DateTime> default_admin_end { get; set; }
        public Nullable<System.DateTime> default_open_start { get; set; }
        public Nullable<System.DateTime> default_open_end { get; set; }
        public Nullable<System.DateTime> default_stability_start { get; set; }
        public Nullable<System.DateTime> default_stability_end { get; set; }
        public long PlanYearGroupId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employee> employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employee> employees1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employee> employees2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employee_insurance_offer> employee_insurance_offer { get; set; }
        public virtual employer employer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<insurance> insurances { get; set; }
    }
}
