using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EmployersCurrentTaxYearTransmissionStatus
{
    public TransmissionStatusEnum TransmissionStatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public String name { get; set; }

    public int employer_id { get; set; }

}