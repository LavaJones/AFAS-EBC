using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Afas.AfComply.Domain;

public class Form1095CUpstreamDetail
{
    public int employer_id { get; set; }
    public int employee_id { get; set; }
    public Guid ResourceId { get; set; }
    public String RecordId { get; set; }
    public String TestScenarioId { get; set; }
    public String CorrectedInd { get; set; }
    public String TaxYr { get; set; }
    public String OtherCompletePersonFirstNm { get; set; }
    public String OtherCompletePersonLastNm { get; set; }
    public String PersonNameControlTxt { get; set; }
    public String SSN { get; set; }
    public DateTime DOB { get; set; }
    public String BirthDt { get; set; }
    public String AddressLine1Txt { get; set; }
    public String CityNm { get; set; }
    public String USStateCd { get; set; }

    private String Zip;
    public String USZIPCd { get { return Zip.ZeroPadZip(); } set { Zip = value; } }
    public String USZIPExtensionCd { get; set; }
    public String ALEContactPhoneNum { get; set; }
    public String StartMonthNumberCd { get; set; }
    public String AnnlEmployeeRequiredContriAmt { get; set; }
    public MonthlyEmployeeRequiredContriGrp MonthlyEmployeeRequiredContriGrp { get; set; }
    public String AnnualOfferOfCoverageCd { get; set; }
    public MonthlyOfferCoverageGrp MonthlyOfferCoverageGrp { get; set; }
    public String AnnualSafeHarborCd { get; set; }
    public MonthlySafeHarborGrp MonthlySafeHarborGrp { get; set; }
    public CorrectedRecordInfoGrp CorrectedRecordInfoGrp { get; set; }
    public List<CoveredIndividualGrp> CoveredIndividualGrps { get; set; }
    public String CoveredIndividualInd { get; set; }

}