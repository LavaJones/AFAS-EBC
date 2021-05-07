using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class waitingPeriod
{
    public int id { get; set; }
    public string description { get; set; }
    public int entityStatusID { get; set; }
    public string guid { get; set; }
    public DateTime? createdOn { get; set; }
    public string createdBy { get; set; }
    public DateTime? modifiedOn { get; set; }
    public string modifiedBy { get; set; }

}