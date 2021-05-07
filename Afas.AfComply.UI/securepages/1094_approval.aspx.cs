using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Afas.AfComply.Domain;
using log4net;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using System.Data;

public partial class securepages_1094_approval : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(securepages_1094_approval));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        if (null == employer || false == employer.IrsEnabled)
        {
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [securepages_1094_approval] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=54", false);

            return;
        }








                LitUserName.Text = user.User_UserName;
        HfDistrictID.Value = user.User_District_ID.ToString();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void DdlCalendarYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int taxYear = 0;
            string ddlTaxYear = DdlCalendarYear.SelectedValue;
            if (int.TryParse(ddlTaxYear, out taxYear))
            {
                int employerId = 0;
                string hf_employer_id = HfDistrictID.Value;
                if (int.TryParse(hf_employer_id, out employerId))
                {
                    List<Form1094CUpstreamDetail> details = employerController.getForm1094CUpstreamDetails(taxYear, employerId, false, false, false);

                    if (details.Count > 1)
                    {
                        Log.Error("Found more than one upstream details for employer [" + employerId + "] tax year [" + taxYear + "]");
                        Document1094c.Visible = false;
                    }
                    else if (details.Count == 1)
                    {                        
                        Document1094c.Visible = true;

                        Form1094CUpstreamDetail detail = details.FirstOrDefault();

                        Doc1094Field1.Text = detail.BusinessNameLine1Txt;     
                        Doc1094Field2.Text = detail.EmployerEIN;   
                        Doc1094Field3.Text = detail.AddressLine1Txt;      
                        Doc1094Field4.Text = detail.CityNm;  
                        Doc1094Field5.Text = detail.USStateCd;  
                        Doc1094Field6.Text = detail.USZIPCd;      
                        Doc1094Field7.Text = detail.PersonFirstNm + ' ' + detail.PersonLastNm;    
                        Doc1094Field8.Text = detail.ContactPhoneNum;  

                        tax_year_submission taxYearSub = employerController.manufactureTaxYearSubmission(employerId, 2016);

                        bool DGE = taxYearSub.IRS_DGE ?? false;         

                        if (DGE)
                        {
                            Doc1094Field9.Visible = true;
                            Doc1094Field9.Text = taxYearSub.IRS_DGE_NAME;       
                            Doc1094Field10.Visible = true;
                            Doc1094Field10.Text = taxYearSub.IRS_DGE_EIN;   
                            Doc1094Field11.Visible = true;
                            Doc1094Field11.Text = taxYearSub.IRS_DGE_ADDRESS;                                  
                            Doc1094Field12.Visible = true;
                            Doc1094Field12.Text = taxYearSub.IRS_DGE_CITY;  
                            Doc1094Field13.Visible = true;
                            Doc1094Field13.Text = StateController.findState(taxYearSub.IRS_DGE_STATE_ID).State_Name;  
                            Doc1094Field14.Visible = true;
                            Doc1094Field14.Text = taxYearSub.IRS_DGE_ZIP;      
                            Doc1094Field15.Visible = true;
                            Doc1094Field15.Text = taxYearSub.IRS_DGE_CONTACT_FNAME + " " + taxYearSub.IRS_DGE_CONTACT_LNAME;    
                            Doc1094Field16.Visible = true;
                            Doc1094Field16.Text = taxYearSub.IRS_DGE_PHONE;  
                        }
                        else 
                        {
                            Doc1094Field9.Visible = false;
                            Doc1094Field10.Visible = false;
                            Doc1094Field11.Visible = false;
                            Doc1094Field12.Visible = false;
                            Doc1094Field13.Visible = false;
                            Doc1094Field14.Visible = false;
                            Doc1094Field15.Visible = false;
                            Doc1094Field16.Visible = false;
                        }

                        Doc1094Field18.Text = detail.Form1095CAttachedCnt;         
                        Doc1094Field19.Checked = false == String.IsNullOrEmpty(detail.AuthoritativeTransmittalInd);                    
                        Doc1094Field20.Text = detail.TotalForm1095CALEMemberCnt;             
                        Doc1094Field21Yes.Checked = taxYearSub.IRS_ALE ?? false;          
                        Doc1094Field21No.Checked = false == (taxYearSub.IRS_ALE ?? false);
                        Doc1094Field22A.Checked = detail.QualifyingOfferMethodInd == "1";            
                        Doc1094Field22B.Checked = false; 
                        Doc1094Field22B.Enabled = false;
                        Doc1094Field22C.Checked = detail.Section4980HReliefInd == "1";    
                        Doc1094Field22D.Checked = detail.NinetyEightPctOfferMethodInd == "1";   

                        ALEMemberInformationGrp group = detail.ALEMemberInformationGrp;

                        DataTable table = new DataTable();

                        table.Columns.Add("Doc1094MonthlyLineNum");
                        table.Columns.Add("Doc1094MonthlyMonthLabel");
                        table.Columns.Add("Doc1094MonthlyAYes", typeof(bool));
                        table.Columns.Add("Doc1094MonthlyANo", typeof(bool));
                        table.Columns.Add("Doc1094MonthlyB");
                        table.Columns.Add("Doc1094MonthlyC");
                        table.Columns.Add("Doc1094MonthlyD", typeof(bool));
                        table.Columns.Add("Doc1094MonthlyE");

                        DataRow row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 23;
                        row["Doc1094MonthlyMonthLabel"] = "All 12 Months";
                        row["Doc1094MonthlyAYes"] = group.YearlyMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.YearlyMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.YearlyALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.YearlyTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.YearlyAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.YearlyALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);              
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 24;
                        row["Doc1094MonthlyMonthLabel"] = "Jan";
                        row["Doc1094MonthlyAYes"] = group.JanMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.JanMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.JanALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.JanTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.JanAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.JanALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 25;
                        row["Doc1094MonthlyMonthLabel"] = "Feb";
                        row["Doc1094MonthlyAYes"] = group.FebMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.FebMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.FebALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.FebTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.FebAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.FebALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 26;
                        row["Doc1094MonthlyMonthLabel"] = "Mar";
                        row["Doc1094MonthlyAYes"] = group.MarMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.MarMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.MarALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.MarTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.MarAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.MarALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 27;
                        row["Doc1094MonthlyMonthLabel"] = "Apr";
                        row["Doc1094MonthlyAYes"] = group.AprMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.AprMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.AprALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.AprTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.AprAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.AprALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 28;
                        row["Doc1094MonthlyMonthLabel"] = "May";
                        row["Doc1094MonthlyAYes"] = group.MayMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.MayMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.MayALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.MayTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.MayAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.MayALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 29;
                        row["Doc1094MonthlyMonthLabel"] = "June";
                        row["Doc1094MonthlyAYes"] = group.JunMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.JunMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.JunALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.JunTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.JunAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.JunALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 30;
                        row["Doc1094MonthlyMonthLabel"] = "July";
                        row["Doc1094MonthlyAYes"] = group.JulMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.JulMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.JulALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.JulTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.JulAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.JulALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 31;
                        row["Doc1094MonthlyMonthLabel"] = "Aug";
                        row["Doc1094MonthlyAYes"] = group.AugMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.AugMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.AugALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.AugTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.AugAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.AugALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 32;
                        row["Doc1094MonthlyMonthLabel"] = "Sept";
                        row["Doc1094MonthlyAYes"] = group.SeptMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.SeptMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.SeptALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.SeptTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.SeptAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.SeptALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 33;
                        row["Doc1094MonthlyMonthLabel"] = "Oct";
                        row["Doc1094MonthlyAYes"] = group.OctMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.OctMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.OctALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.OctTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.OctAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.OctALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 34;
                        row["Doc1094MonthlyMonthLabel"] = "Nov";
                        row["Doc1094MonthlyAYes"] = group.NovMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.NovMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.NovALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.NovTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.NovAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.NovALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row["Doc1094MonthlyLineNum"] = 35;
                        row["Doc1094MonthlyMonthLabel"] = "Dec";
                        row["Doc1094MonthlyAYes"] = group.DecMinEssentialCvrOffrCd == "1";
                        row["Doc1094MonthlyANo"] = group.DecMinEssentialCvrOffrCd == "0";
                        row["Doc1094MonthlyB"] = group.DecALEMemberFTECnt;
                        row["Doc1094MonthlyC"] = group.DecTotalEmployeeCnt;
                        row["Doc1094MonthlyD"] = group.DecAggregatedGroupInd == "1";
                        row["Doc1094MonthlyE"] = group.DecALESect4980HTrnstReliefCd;
                        table.Rows.Add(row);
                       
                        Gv_ALE_Monthly.DataSource = table;
                        Gv_ALE_Monthly.DataBind();


                        if (taxYearSub.IRS_ALE ?? false) 
                        {
                            List<OtherALEMembersGrp> ales = detail.OtherALEMembersGrps; 
                            DV_OtherAleMembers.DataSource = ales;
                            DV_OtherAleMembers.DataBind();
                        }

                        Gv1095C.DataSource = detail.Form1095CUpstreamDetails.OrderBy(obj => obj.OtherCompletePersonLastNm);
                        Gv1095C.DataBind();
                    }
                    else 
                    {
                        Document1094c.Visible = false;
                    }
                }
                else
                {
                    Log.Error("An employer tried to select a tax year but is not logged in with a valid employer, HfDistrictID: " + hf_employer_id);
                    Document1094c.Visible = false;
                }
            }
            else
            {
                Document1094c.Visible = false;
            }
        }
        catch(Exception ex)
        {
            Log.Error("Exception while trying to change tax year on 1094 page.", ex);
            Document1094c.Visible = false;
        }
    }
}