using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EmployerTaxYearTransmission
{
    public int EmployerTaxYearTransmissionId { get; set; }
    public int EmployerId { get; set; }
    public int TaxYearId { get; set; }
    public Guid ResourceId { get; set; }
    public int EntityStatusId { get; set; }
    public String CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public String ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }

    public EmployerTaxYearTransmission()
    {

    }

 
}
