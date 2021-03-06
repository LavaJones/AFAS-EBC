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
    
    public partial class EmployerTaxYearTransmission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployerTaxYearTransmission()
        {
            this.EmployerTaxYearTransmissionStatus = new HashSet<EmployerTaxYearTransmissionStatu>();
        }
    
        public long EmployerTaxYearTransmissionId { get; set; }
        public int EmployerId { get; set; }
        public int TaxYearId { get; set; }
        public System.Guid ResourceId { get; set; }
        public int EntityStatusId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        public virtual employer employer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployerTaxYearTransmissionStatu> EmployerTaxYearTransmissionStatus { get; set; }
    }
}
