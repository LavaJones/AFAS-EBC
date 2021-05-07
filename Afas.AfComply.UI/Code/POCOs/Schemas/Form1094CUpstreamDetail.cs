using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Afas.AfComply.Domain;

public class Form1094CUpstreamDetail
{
    public int employer_id { get; set; }
    public bool dge { get; set; }
    public String SubmissionId { get; set; }
    public String OriginalUniqueSubmissionId { get; set; }
    public String TestScenarioId { get; set; }
    public String CorrectedInd { get; set; }
    public String TaxYr { get; set; }
    public String BusinessNameLine1Txt { get; set; }
    public String BusinessNameLine2Txt { get; set; }
    public String BusinessNameControlTxt { get; set; }
    public String EmployerEIN { get; set; }
    public String PersonFirstNm { get; set; }
    public String PersonMiddleNm { get; set; }
    public String PersonLastNm { get; set; }
    public String SuffixNm { get; set; }
    public String ContactPhoneNum { get; set; }
    public String AddressLine1Txt { get; set; }
    public String AddressLine2Txt { get; set; }
    public String CityNm { get; set; }
    public String USStateCd { get; set; }

    private String ZipCode;
    public String USZIPCd { get{return ZipCode.ZeroPadZip(); } set{ZipCode = value;} }
    public String USZIPExtensionCd { get; set; }
    public String Form1095CAttachedCnt { get; set; }

    public String dge_ein { get; set; }
    public GovtEntityEmployerInfoGrp GovtEntityEmployerInfoGrp { get; set; }
  
    public String AuthoritativeTransmittalInd { get; set; }
    public String TotalForm1095CALEMemberCnt { get; set; }
    public String AggregatedGroupMemberCd { get; set; }
    public String QualifyingOfferMethodInd { get; set; }
    public String QlfyOfferMethodTrnstReliefInd { get; set; }
    public String Section4980HReliefInd { get; set; }
    public String NinetyEightPctOfferMethodInd { get; set; }
    public String JuratSignaturePIN { get; set; }
    public String PersonTitleTxt { get; set; }
    public String SignatureDt { get; set; }
    public DateTime dtSignature { get; set; }

    public ALEMemberInformationGrp ALEMemberInformationGrp { get; set; }
    public List<OtherALEMembersGrp> OtherALEMembersGrps { get; set; }

    public CorrectedSubmissionInfoGrp CorrectedSubmissionInfoGrp { get; set; }
    public List<Form1095CUpstreamDetail> Form1095CUpstreamDetails { get; set; }

}