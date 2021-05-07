using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CoveredIndividual
{
    public int covered_individual_id { get; set; }
    public int employee_id { get; set; }
    public int employer_id { get; set; }
    public string first_name { get; set; }
    public string middle_name { get; set; }
    public string last_name { get; set; }
    public string name_suffix { get; set; }
    public string ssn { get; set; }
    public DateTime birth_date { get; set; }
    public bool annual_coverage_indicator { get; set; }
}
