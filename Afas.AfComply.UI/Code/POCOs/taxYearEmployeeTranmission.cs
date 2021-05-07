using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class taxYearEmployeeTransmission
{
    public int tax_year_employee_transmissionID { get; set; }
    public int tax_year_employer_transmissionID { get; set; }
    public int tax_year { get; set; }
    public int employee_id { get; set; }
    public int employer_id { get; set; }
    public Guid ResourceId { get; set; }
    public int RecordID { get; set; }
    public int transmission_status_code_id { get; set; }
    public int EntityStatusId { get; set; }
    public string createdBy { get; set; }
    public DateTime createdOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}
