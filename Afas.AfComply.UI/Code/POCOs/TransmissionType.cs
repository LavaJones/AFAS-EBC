using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TransmissionType
{
    public string transmissionID { get; set; }
    public string name { get; set; }
    public Guid ResourceId { get; set; }
    public int EntityStatusId { get; set; }
    public string createdBy { get; set; }
    public DateTime createdOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}