using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class tax_year_1095c_correction_exception
{
    public int TaxYear1095cCorrectionExceptionId { get; set; }
    public int tax_year { get; set; }
    public int employer_id { get; set; }
    public int employee_id { get; set; }
    public String Justification { get; set; }
    public String CreatedBy { get; set; }
    public String ModifiedBy { get; set; }
}