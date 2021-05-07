using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EmployerTaxYearTransmissionStatus
{

    public int EmployerId { get; set; }
    public int EmployerTaxYearTransmissionStatusId { get; set; }
    public int EmployerTaxYearTransmissionId { get; set; }
    public TransmissionStatusEnum TransmissionStatusId { get; set; }
    public Guid ResourceId { get; set; }
    public int EntityStatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }

    public EmployerTaxYearTransmissionStatus()
    {

    }

    public EmployerTaxYearTransmissionStatus(int employerTaxYearTransmissionId, TransmissionStatusEnum transmissionStatusId, string userName, DateTime? endDate = null)
    {
        EmployerTaxYearTransmissionId = employerTaxYearTransmissionId;
        StartDate = DateTime.Now;
        EndDate = endDate;
        TransmissionStatusId = transmissionStatusId;
        EntityStatusId = 1;
        CreatedBy = userName;
        ModifiedBy = userName;
    }

}