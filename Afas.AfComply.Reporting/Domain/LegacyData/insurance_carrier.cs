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
    
    public partial class insurance_carrier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public insurance_carrier()
        {
            this.insurance_coverage = new HashSet<insurance_coverage>();
        }
    
        public int carrier_id { get; set; }
        public string name { get; set; }
        public Nullable<bool> import_approved { get; set; }
        public Nullable<bool> hra_flex { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<insurance_coverage> insurance_coverage { get; set; }
    }
}