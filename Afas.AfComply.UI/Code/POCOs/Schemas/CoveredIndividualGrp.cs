using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CoveredIndividualGrp
{
    public int employee_id { get; set; }
    public int covered_individual_id { get; set; }
    public String PersonFirstNm { get; set; }
    public String PersonMiddleNm { get; set; }
    public String PersonLastNm { get; set; }
    public String SuffixNm { get; set; }
    public String PersonNameControlTxt { get; set; }
    public String SSN { get; set; }
    public DateTime DOB { get; set; }
    public String BirthDt { get; set; }

    public String CoveredIndividualAnnualInd { get; set; }
    public CoveredIndividualMonthlyIndGrp CoveredIndividualMonthlyIndGrp { get; set; }

}