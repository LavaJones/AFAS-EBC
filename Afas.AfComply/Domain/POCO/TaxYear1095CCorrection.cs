using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TaxYear1095CCorrection
{

    public TaxYear1095CCorrection()
    {

    }

    public int tax_yearCorrectionId { get; set; }
    public int tax_year { get; set; }
    public int employee_id { get; set; }
    public int employer_id {get; set;}
    public Guid ResourceId { get; set; }
    public bool Corrected { get; set; }
    public String OriginalUniqueSubmissionId { get; set; }
    public String CorrectedUniqueSubmissionId { get; set; }
    public String CorrectedUniqueRecordId { get; set; }
    public bool Transmitted { get; set; }
    public String ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int tax_year_employee_transmissionId { get; set;}
}