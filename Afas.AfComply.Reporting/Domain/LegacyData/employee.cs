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
    
    public partial class employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public employee()
        {
            this.employee_dependents = new HashSet<employee_dependents>();
            this.employee_insurance_offer = new HashSet<employee_insurance_offer>();
            this.EmployeeMeasurementAverageHours = new HashSet<EmployeeMeasurementAverageHour>();
            this.insurance_coverage_editable = new HashSet<insurance_coverage_editable>();
            this.insurance_coverage = new HashSet<insurance_coverage>();
        }
    
        public int employee_id { get; set; }
        public int employee_type_id { get; set; }
        public Nullable<int> HR_status_id { get; set; }
        public int employer_id { get; set; }
        public string fName { get; set; }
        public string mName { get; set; }
        public string lName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public int state_id { get; set; }
        public string zip { get; set; }
        public System.DateTime hireDate { get; set; }
        public Nullable<System.DateTime> currDate { get; set; }
        public string ssn { get; set; }
        public string ext_emp_id { get; set; }
        public Nullable<System.DateTime> terminationDate { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public System.DateTime initialMeasurmentEnd { get; set; }
        public Nullable<int> plan_year_id { get; set; }
        public Nullable<int> limbo_plan_year_id { get; set; }
        public int meas_plan_year_id { get; set; }
        public Nullable<System.DateTime> modOn { get; set; }
        public string modBy { get; set; }
        public Nullable<decimal> plan_year_avg_hours { get; set; }
        public Nullable<decimal> limbo_plan_year_avg_hours { get; set; }
        public Nullable<decimal> meas_plan_year_avg_hours { get; set; }
        public Nullable<decimal> imp_plan_year_avg_hours { get; set; }
        public Nullable<int> classification_id { get; set; }
        public Nullable<int> aca_status_id { get; set; }
        public System.Guid ResourceId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employee_dependents> employee_dependents { get; set; }
        public virtual employer employer { get; set; }
        public virtual plan_year plan_year { get; set; }
        public virtual plan_year plan_year1 { get; set; }
        public virtual plan_year plan_year2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employee_insurance_offer> employee_insurance_offer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeMeasurementAverageHour> EmployeeMeasurementAverageHours { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<insurance_coverage_editable> insurance_coverage_editable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<insurance_coverage> insurance_coverage { get; set; }
    }
}
