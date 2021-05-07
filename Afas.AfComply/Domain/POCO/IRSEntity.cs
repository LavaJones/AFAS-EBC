using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IRSEntity
{

    public int EmployerTaxYearTransmissionId { get; set; }
    public Guid ResourceId { get; set; }
    public int employer_id { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string zip { get; set; }
    public string bill_address { get; set; }
    public string bill_city { get; set; }
    public string bill_state { get; set; }
    public string bill_zip { get; set; }

}
