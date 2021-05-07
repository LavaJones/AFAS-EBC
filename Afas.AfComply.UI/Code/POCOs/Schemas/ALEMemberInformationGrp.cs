using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Class to deal with possible null values cleanly
/// </summary>
public static class AleExtension 
{
    /// <summary>
    /// Extension method used to deal with null values
    /// </summary>
    /// <param name="info">The object to check the value of or null</param>
    /// <returns>the value or a blank string</returns>
    public static string GetALEMemberFTECnt(this ALEMemberMonthlyInfo info)
    {
        if (null != info)
            return info.ALEMemberFTECnt;
        return "";
    }

    /// <summary>
    /// Extension method used to deal with null values
    /// </summary>
    /// <param name="info">The object to check the value of or null</param>
    /// <returns>the value or 0</returns>
    public static int GetALEMemberFTECntInt(this ALEMemberMonthlyInfo info)
    {
        int ALEMemberFTECnt = 0;
        int.TryParse(info.GetALEMemberFTECnt(), out ALEMemberFTECnt);
        return ALEMemberFTECnt;
    }

    /// <summary>
    /// Extension method used to deal with null values
    /// </summary>
    /// <param name="info">The object to check the value of or null</param>
    /// <returns>the value or a blank string</returns>
    public static string GetMinEssentialCvrOffrCd(this ALEMemberMonthlyInfo info)
    {
        if (null != info)
            return info.MinEssentialCvrOffrCd;
        return "";
    }
    
    /// <summary>
    /// Extension method used to deal with null values
    /// </summary>
    /// <param name="info">The object to check the value of or null</param>
    /// <returns>the value or a blank string</returns>
    public static string GetTotalEmployeeCnt(this ALEMemberMonthlyInfo info)
    {
        if (null != info)
            return info.TotalEmployeeCnt;
        return "";
    }

    /// <summary>
    /// Extension method used to deal with null values
    /// </summary>
    /// <param name="info">The object to check the value of or null</param>
    /// <returns>the value or 0</returns>
    public static int GetTotalEmployeeCntInt(this ALEMemberMonthlyInfo info)
    {
        int TotalEmployeeCnt = 0;
        int.TryParse(info.GetTotalEmployeeCnt(), out TotalEmployeeCnt);
        return TotalEmployeeCnt;
    }

    /// <summary>
    /// Extension method used to deal with null values
    /// </summary>
    /// <param name="info">The object to check the value of or null</param>
    /// <returns>the value or a blank string</returns>
    public static string GetMecEmployeeCnt(this ALEMemberMonthlyInfo info)
    {
        if (null != info)
            return info.MecEmployeeCnt;
        return "";
    }

    /// <summary>
    /// Extension method used to deal with null values
    /// </summary>
    /// <param name="info">The object to check the value of or null</param>
    /// <returns>the value or 0</returns>
    public static int GetMecEmployeeCntInt(this ALEMemberMonthlyInfo info)
    {
        int MecEmployeeCnt = 0;
        int.TryParse(info.GetMecEmployeeCnt(), out MecEmployeeCnt);
        return MecEmployeeCnt;
    }
}

/// <summary>
/// Storage for data from the database, contains the FTE count and Total Count for an employer time period
/// </summary>
public class ALEMemberMonthlyInfo 
{
    public int employer_id { get; set; }
    public int time_frame_id { get; set; }
    public string MinEssentialCvrOffrCd { get; set; }
    public string ALEMemberFTECnt { get; set; }
    public string TotalEmployeeCnt { get; set; }
    public string MecEmployeeCnt { get; set; }
}

public class ALEMemberInformationGrp
{
    /// <summary>
    /// Standard constructor that sets the important peices of data
    /// </summary>
    /// <param name="employerId">The Employer Id</param>
    /// <param name="monthlyInfo">The list of months of data for this employer</param>
    public ALEMemberInformationGrp(int employerId, List<ALEMemberMonthlyInfo> monthlyInfo) 
    {
        employer_id = employerId;
        MonthlyInfo = monthlyInfo;

    }

    /// <summary>
    /// Default constructor because Obiye's code needs it.
    /// </summary>
    public ALEMemberInformationGrp() { }

    /// <summary>
    /// The Employer Id
    /// </summary>
    public int employer_id{ get; set; }

    /// <summary>
    /// The list of months of data for this employer
    /// </summary>
    public List<ALEMemberMonthlyInfo> MonthlyInfo { get; set; }

    /// <summary>
    /// Did the Employer declare that they are qualified for Relief in the Questionaire.
    /// </summary>
    private String TrnstReliefCode { get; set; } 

    /// <summary>
    /// Is the Employer Marked as an Aggregate ALE in the Questionaire
    /// </summary>
    public bool EmployerIsAggrAle { get; set; }

    /// <summary>
    /// internal storage of the Transition Relief elegibility per month. 
    /// </summary>
    private Dictionary<int, String> TrnstRelief { get; set; }

    /// <summary>
    /// Build out the dictionary of months that Transition Reliefe is availible
    /// </summary>
    private void BuildTrnstRelief() 
    {
        TrnstRelief = new Dictionary<int, String>();

        if (null == TrnstReliefCode)
        {
            for(int i=1; i<=12; i++)
            {
                TrnstRelief.Add(i, null);
            }        
            return;
        }
        
        List<PlanYear> pys = PlanYear_Controller.getEmployerPlanYear(employer_id);
        List<PlanYear> filteredPys = (from PlanYear py in pys where py.PLAN_YEAR_START < new DateTime(2016,1,1) 
                                          && py.PLAN_YEAR_END > new DateTime(2016,1,1) select py).ToList();

        for(int i=1; i<=12; i++)
        {
            DateTime firstOfMonth = new DateTime(2016, i, 1);
            bool applies = false;
            foreach(PlanYear py in filteredPys)
            {
                if(py.PLAN_YEAR_END > firstOfMonth)
                {
                    applies = true;
                    break;
                }
            }

            if (applies)
            {
                TrnstRelief.Add(i, TrnstReliefCode); 
            }
            else
            {
                TrnstRelief.Add(i, null);
            }
        }
    }

    /// <summary>
    /// Gets the ALEMemberMonthlyInfo object for a specific month Id
    /// </summary>
    /// <param name="monthId">Month Id indicating which month it is for</param>
    /// <returns>The object for that month or null</returns>
    private ALEMemberMonthlyInfo GetForMonth(int monthId) 
    {
        return (from ALEMemberMonthlyInfo info in MonthlyInfo where info.time_frame_id == monthId select info).FirstOrDefault();
    }

    /// <summary>
    /// Converts the value to either a 1 or a 0 or Null, because upstream code is expecting that.
    /// </summary>
    /// <param name="val">The value to convert to string</param>
    /// <returns>The string representatio of the value.</returns>
    private String BoolToOneOrZero(bool? val) 
    {
        if (null == val)
            return null;
        if (true == val)
            return "1";
        if (false == val)
            return "0";
        return null;
    }

    private bool IsTrnstReliefApplicable(int monthId) 
    {
        if (null == TrnstRelief[monthId]) 
        {
            return false;
        }
        return true;
    }

    private string GetTrnstRelief(int monthId) 
    {
        return TrnstRelief[monthId];
    }

    /// <summary>
    /// Decide if the Minimum Esential Coverage was met for this month.
    /// </summary>
    /// <param name="monthId">The id of the month to check</param>
    /// <returns>If the coverage was met or not</returns>
    private bool GetMinEssentialCvrOffrBoolMonth(int monthId)
    {
        int fte = GetForMonth(monthId).GetALEMemberFTECntInt();
        if (fte <= 0) 
        { 
            return true; 
        }

        int mec = GetForMonth(monthId).GetMecEmployeeCntInt();

        double percent = (double) mec / (double) fte;

        if (percent >= 0.95) 
        {
            return true;
        }

        if (percent < 0.70)
        {
            return false;
        }

        if (IsTrnstReliefApplicable(monthId))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the Year value as a string
    /// </summary>
    /// <returns>String value of the year value</returns>
    private String GetMinEssentialCvrOffrYear()
    {
        string jan = GetForMonth(1).GetMinEssentialCvrOffrCd();

        for (int i = 2; i <= 12; i++)
        {
            if (GetForMonth(i).GetMinEssentialCvrOffrCd() != jan)
                return null;
        }

        return jan;
    }

    /// <summary>
    /// returns the month value as a string (returns null if yearly value override) 
    /// </summary>
    /// <param name="month">The month Id to check for</param>
    /// <returns>Null if All 12, else the value for that month as a string</returns>
    private String GetMinEssentialCvrOffrMonth(int month)
    {
        if (null == GetMinEssentialCvrOffrYear())
        {
            return GetForMonth(month).GetMinEssentialCvrOffrCd();
        }
        return null;
    }

    private bool GetAggregatedGroupBoolMonth(int monthId)
    {
        return false;
    }

    private bool? GetAggregatedGroupBoolYear()
    {
        return EmployerIsAggrAle;
    }

    /// <summary>
    /// Returns the Year value as a string
    /// </summary>
    /// <returns>String value of the year value</returns>
    private String GetAggregatedGroupYear()
    {
        return BoolToOneOrZero(GetAggregatedGroupBoolYear());
    }

    /// <summary>
    /// returns the month value as a string (returns null if yearly value override) 
    /// </summary>
    /// <param name="month">The month Id to check for</param>
    /// <returns>Null if All 12, else the value for that month as a string</returns>
    private String GetAggregatedGroupMonth(int month)
    {
        return null;
    }

    /// <summary>
    /// Returns the Year value as a string
    /// </summary>
    /// <returns>String value of the year value</returns>
    private String GetALESect4980HTrnstReliefYear()
    {
        return null;      
    }

    /// <summary>
    /// returns the month value as a string (returns null if yearly value override) 
    /// </summary>
    /// <param name="month">The month Id to check for</param>
    /// <returns>Null if All 12, else the value for that month as a string</returns>
    private String GetALESect4980HTrnstReliefMonth(int month)
    {
        return GetTrnstRelief(month);
    }

    /// <summary>
    /// Gets the count of FTE employees for the whole year if they are constant
    /// </summary>
    /// <returns>Null if not all months the same, or the value for all months</returns>
    private int? GetALEMemberFTECntIntYear()
    {
        int jan = GetForMonth(1).GetALEMemberFTECntInt();

        for (int i = 2; i <= 12; i++)
        {
            if (GetForMonth(i).GetALEMemberFTECntInt() != jan)
                return null;
        }

        return jan;
    }

    /// <summary>
    /// Returns the Year value as a string
    /// </summary>
    /// <returns>String value of the year value</returns>
    private String GetALEMemberFTECntYear()
    {
        if (GetALEMemberFTECntIntYear() == null)
            return null;
        return GetALEMemberFTECntIntYear().ToString();
    }

    /// <summary>
    /// returns the month value as a string (returns null if yearly value override) 
    /// </summary>
    /// <param name="month">The month Id to check for</param>
    /// <returns>Null if All 12, else the value for that month as a string</returns>
    private String GetALEMemberFTEMonth(int month)
    {
        if (null == GetALEMemberFTECntIntYear())
            return GetForMonth(month).GetALEMemberFTECntInt().ToString();
        return null;
    }
    
    /// <summary>
    /// Gets the count of Total Employees for the whole Year if it is constant
    /// </summary>
    /// <returns>Null if not all months the same, or the value for all months</returns>
    private int? GetTotalEmployeeCntIntYear()
    {
        int jan = GetForMonth(1).GetTotalEmployeeCntInt();

        for (int i = 2; i <= 12; i++)
        {
            if (GetForMonth(i).GetTotalEmployeeCntInt() != jan)
                return null;
        }

        return jan;
    }

    /// <summary>
    /// Returns the Year value as a string
    /// </summary>
    /// <returns>String value of the year value</returns>
    private String GetTotalEmployeeCntYear() 
    {
        if (GetTotalEmployeeCntIntYear() == null)
            return null;
        return GetTotalEmployeeCntIntYear().ToString();
    }

    /// <summary>
    /// returns the month value as a string (returns null if yearly value override) 
    /// </summary>
    /// <param name="month">The month Id to check for</param>
    /// <returns>Null if All 12, else the value for that month as a string</returns>
    private String GetTotalEmployeeCntMonth(int month)
    {
        if(null == GetTotalEmployeeCntIntYear())
            return GetForMonth(month).GetTotalEmployeeCntInt().ToString();
        return null;
    }

    public String JanMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(1); } }
    public String JanALEMemberFTECnt { get { return GetALEMemberFTEMonth(1); } }
    public String JanTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(1); } }
    public String JanAggregatedGroupInd { get { return GetAggregatedGroupMonth(1); } }
    public String JanALESect4980HTrnstReliefCd { get { return ""; } }

    public String FebMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(2); } }
    public String FebALEMemberFTECnt { get { return GetALEMemberFTEMonth(2); } }
    public String FebTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(2); } }
    public String FebAggregatedGroupInd { get { return GetAggregatedGroupMonth(2); } }
    public String FebALESect4980HTrnstReliefCd { get { return ""; } }

    public String MarMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(3); } }
    public String MarALEMemberFTECnt { get { return GetALEMemberFTEMonth(3); } }
    public String MarTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(3); } }
    public String MarAggregatedGroupInd { get { return GetAggregatedGroupMonth(3); } }
    public String MarALESect4980HTrnstReliefCd { get { return ""; } }

    public String AprMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(4); } }
    public String AprALEMemberFTECnt { get { return GetALEMemberFTEMonth(4); } }
    public String AprTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(4); } }
    public String AprAggregatedGroupInd { get { return GetAggregatedGroupMonth(4); } }
    public String AprALESect4980HTrnstReliefCd { get { return ""; } }

    public String MayMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(5); } }
    public String MayALEMemberFTECnt { get { return GetALEMemberFTEMonth(5); } }
    public String MayTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(5); } }
    public String MayAggregatedGroupInd { get { return GetAggregatedGroupMonth(5); } }
    public String MayALESect4980HTrnstReliefCd { get { return ""; } }

    public String JunMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(6); } }
    public String JunALEMemberFTECnt { get { return GetALEMemberFTEMonth(6); } }
    public String JunTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(6); } }
    public String JunAggregatedGroupInd { get { return GetAggregatedGroupMonth(6); } }
    public String JunALESect4980HTrnstReliefCd { get { return ""; } }

    public String JulMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(7); } }
    public String JulALEMemberFTECnt { get { return GetALEMemberFTEMonth(7); } }
    public String JulTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(7); } }
	public String JulAggregatedGroupInd { get{ return GetAggregatedGroupMonth(7); } }
    public String JulALESect4980HTrnstReliefCd { get { return ""; } }

    public String AugMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(8); } }
    public String AugALEMemberFTECnt { get { return GetALEMemberFTEMonth(8); } }
    public String AugTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(8); } }
    public String AugAggregatedGroupInd { get { return GetAggregatedGroupMonth(8); } }
    public String AugALESect4980HTrnstReliefCd { get { return ""; } }

    public String SeptMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(9); } }
    public String SeptALEMemberFTECnt { get { return GetALEMemberFTEMonth(9); } }
    public String SeptTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(9); } }
    public String SeptAggregatedGroupInd { get { return GetAggregatedGroupMonth(9); } }
    public String SeptALESect4980HTrnstReliefCd { get { return ""; } }

    public String OctMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(10); } }
    public String OctALEMemberFTECnt { get { return GetALEMemberFTEMonth(10); } }
    public String OctTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(10); } }
    public String OctAggregatedGroupInd { get { return GetAggregatedGroupMonth(10); } }
    public String OctALESect4980HTrnstReliefCd { get { return ""; } }

    public String NovMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(11); } }
    public String NovALEMemberFTECnt { get { return GetALEMemberFTEMonth(11); } }
    public String NovTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(11); } }
    public String NovAggregatedGroupInd { get { return GetAggregatedGroupMonth(11); } }
    public String NovALESect4980HTrnstReliefCd { get { return ""; } }

    public String DecMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrMonth(12); } }
    public String DecALEMemberFTECnt { get { return GetALEMemberFTEMonth(12); } }
    public String DecTotalEmployeeCnt { get { return GetTotalEmployeeCntMonth(12); } }
    public String DecAggregatedGroupInd { get { return GetAggregatedGroupMonth(12); } }
    public String DecALESect4980HTrnstReliefCd { get { return ""; } }

    public String YearlyMinEssentialCvrOffrCd { get { return GetMinEssentialCvrOffrYear(); } }
    public String YearlyALEMemberFTECnt { get { return GetALEMemberFTECntYear(); } }
    public String YearlyTotalEmployeeCnt { get { return GetTotalEmployeeCntYear();  } }
    public String YearlyAggregatedGroupInd { get { return GetAggregatedGroupYear(); } }
    public String YearlyALESect4980HTrnstReliefCd { get { return ""; } }
}