using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for monthlyDetail
/// </summary>
public class monthlyDetail
{
    private int employeeID = 0;
    private int timeFrame = 0;
    private int employerID = 0;
    private decimal hours = 0;
    private string ooc = null;
    private bool? mec = null;
    private decimal? lcmp = 0;
    private string ash = null;
    private bool enrolled = false;
    private int monthlyStatusID = 0;
    private int insuranceTypeID = 0;
    private string modBy = null;
    private DateTime? modOn = null;
    private bool finalized = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <param name="_timeFrameID"></param>
    /// <param name="_employerID"></param>
    /// <param name="_hours"></param>
    /// <param name="_ooc"></param>
    /// <param name="_mec"></param>
    /// <param name="_lcmp"></param>
    /// <param name="_ash"></param>
    /// <param name="_enrolled"></param>
    /// <param name="_monthlyStatusID"></param>
    /// <param name="_insuranceTypeID"></param>
	public monthlyDetail(int _employeeID, int _timeFrameID, int _employerID, decimal _hours, string _ooc, bool? _mec, decimal? _lcmp, string _ash, bool _enrolled, int _monthlyStatusID, int _insuranceTypeID, string _modBy, DateTime? _modOn, bool _finalized)
	{
        this.employeeID = _employeeID;
        this.employerID = _employerID;
        this.timeFrame = _timeFrameID;
        this.hours = _hours;
        this.ooc = _ooc;
        this.mec = _mec;
        this.lcmp = _lcmp;
        this.ash = _ash;
        this.enrolled = _enrolled;
        this.monthlyStatusID = _monthlyStatusID;
        this.insuranceTypeID = _insuranceTypeID;
        this.modBy = _modBy;
        this.modOn = _modOn;
        this.finalized = _finalized;
	}

    public int MD_EMPLOYEE_ID
    {
        get { return this.employeeID; }
        set { this.employeeID = value; }
    }

    public int MD_TIME_FRAME_ID
    {
        get { return this.timeFrame; }
        set { this.timeFrame = value; }
    }

    public int MD_EMPLOYER_ID
    {
        get { return this.employerID; }
        set { this.employerID = value; }
    }

    public decimal MD_HOURS
    {
        get { return this.hours; }
        set { this.hours = value; }
    }

    public string MD_OOC
    {
        get { return this.ooc; }
        set { this.ooc = value; }
    }

    public bool? MD_MEC
    {
        get { return this.mec; }
        set { this.mec = value; }
    }

    public decimal? MD_LCMP
    {
        get { return this.lcmp; }
        set { this.lcmp = value; }
    }

    public string MD_ASH
    {
        get { return this.ash; }
        set { this.ash = value; }
    }

    public bool MD_ENROLLED
    {
        get { return this.enrolled; }
        set { this.enrolled = value; }
    }

    public int MD_MONTHLY_STATUS_ID
    {
        get { return this.monthlyStatusID; }
        set { this.monthlyStatusID = value; }
    }

    public int MD_INSURANCE_TYPE_ID
    {
        get { return this.insuranceTypeID; }
        set { this.insuranceTypeID = value; }
    }

    public string MD_MOD_BY
    {
        get { return this.modBy; }
        set { this.modBy = value; }
    }

    public DateTime? MD_MOD_ON
    {
        get { return this.modOn; }
        set { this.modOn = value; }
    }

    public bool MD_FINALIZED
    {
        get { return this.finalized; }
        set { this.finalized = value; }
    }
}