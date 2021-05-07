using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EmployerTaxYearTransmissionStatusQueue
{
    public int EmployerTaxYearTransmissionStatusQueueId {get; set;}
	public int EmployerTaxYearTransmissionStatusId {get; set;}
    public QueueStatusEnum QueueStatusId { get; set; }
    public string Title {get; set;}
	public string Message {get; set;}
    public string Body { get; set; }
    public Guid ResourceId { get; set; }
    public int EntityStatusId { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }

    public EmployerTaxYearTransmissionStatusQueue()
    {

    }

    public EmployerTaxYearTransmissionStatusQueue(int employerTaxYearTransmissionStatusId, string title, string message, string body, string userName, QueueStatusEnum queueStatusId = QueueStatusEnum.Pending)
    {
        EmployerTaxYearTransmissionStatusId = employerTaxYearTransmissionStatusId;
        Title = title;
        Message = message;
        Body = body;
        QueueStatusId = queueStatusId;
        EntityStatusId = 1;
        CreatedBy = userName;
        ModifiedBy = userName;
    }

}