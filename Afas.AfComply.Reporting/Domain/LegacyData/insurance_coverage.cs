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
    
    public partial class insurance_coverage
    {
        public int row_id { get; set; }
        public int tax_year { get; set; }
        public int carrier_id { get; set; }
        public int employee_id { get; set; }
        public Nullable<int> dependent_id { get; set; }
        public bool all12 { get; set; }
        public bool jan { get; set; }
        public bool feb { get; set; }
        public bool mar { get; set; }
        public bool apr { get; set; }
        public bool may { get; set; }
        public bool jun { get; set; }
        public bool jul { get; set; }
        public bool aug { get; set; }
        public bool sep { get; set; }
        public bool oct { get; set; }
        public bool nov { get; set; }
        public bool dec { get; set; }
        public string history { get; set; }
        public System.Guid ResourceId { get; set; }
        public Nullable<int> batch_id { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int EntityStatusID { get; set; }
    
        public virtual employee employee { get; set; }
        public virtual employee_dependents employee_dependents { get; set; }
        public virtual insurance_carrier insurance_carrier { get; set; }
    }
}
