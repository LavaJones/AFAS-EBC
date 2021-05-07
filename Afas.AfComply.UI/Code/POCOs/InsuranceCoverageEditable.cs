using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class InsuranceCoverageEditable
{

    public InsuranceCoverageEditable(
            int rowId,
            int employeeId,
            int employerId,
            int? dependentId,
            int taxYearId,
            Boolean january,
            Boolean february,
            Boolean march,
            Boolean april,
            Boolean may,
            Boolean june,
            Boolean july,
            Boolean august,
            Boolean september,
            Boolean october,
            Boolean november,
            Boolean december,
            Guid resourceId
        )
    {

        this.RowId = rowId;
        this.EmployeeId = employeeId;
        this.EmployerId = employerId;
        this.DependentId = dependentId;
        this.TaxYearId = taxYearId;
        this.January = january;
        this.February = february;
        this.March = march;
        this.April = april;
        this.May = may;
        this.June = june;
        this.July = july;
        this.August = august;
        this.September = september;
        this.October = october;
        this.November = november;
        this.December = december;
        this.ResourceId = resourceId;

    }
    
    public int RowId { get; set; }
    public int EmployeeId { get; set; }
    public int EmployerId { get; set; }
    public int? DependentId { get; set; }
    public int TaxYearId { get; set; }
    public Boolean January { get; set; }
    public Boolean February { get; set; }
    public Boolean March { get; set; }
    public Boolean April { get; set; }
    public Boolean May { get; set; }
    public Boolean June { get; set; }
    public Boolean July { get; set; }
    public Boolean August { get; set; }
    public Boolean September { get; set; }
    public Boolean October { get; set; }
    public Boolean November { get; set; }
    public Boolean December { get; set; }
    public Guid ResourceId { get; set; }

}
