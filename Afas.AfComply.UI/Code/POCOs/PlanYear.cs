using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PlanYear
/// </summary>
public class PlanYear
{
    private int id = 0;
    private int employerID = 0;
    private string description = null;
    private DateTime? startDate = null;
    private DateTime? endDate = null;
    private string notes = null;
    private string history = null;
    private DateTime? modOn = null;
    private string modBy = null;

	public PlanYear(int _id, int _employerID, string _desc, DateTime? _sdate, DateTime? _edate, string _notes, string _history, DateTime? _modOn, string _modBy)
	{
        this.id = _id;
        this.employerID = _employerID;
        this.description = _desc;
        this.startDate = _sdate;
        this.endDate = _edate;
        this.notes = _notes;
        this.history = _history;
        this.modOn = _modOn;
        this.modBy = _modBy;
	}

    public int PLAN_YEAR_ID
    {
        get
        {
            return this.id;
        }
        set
        {
            this.id = value;
        }
    }

    public int PLAN_YEAR_EMPLOYER_ID
    {
        get
        {
            return this.employerID;
        }
        set
        {
            this.employerID = value;
        }
    }

    public string PLAN_YEAR_DESCRIPTION
    {
        get
        {
            return this.description;
        }
        set
        {
            this.description = value;
        }
    }

    public DateTime? PLAN_YEAR_START
    {
        get
        {
            return this.startDate;
        }
        set
        {
            this.startDate = value;
        }
    }

    public DateTime? PLAN_YEAR_END
    {
        get
        {
            return this.endDate;
        }
        set
        {
            this.endDate = value;
        }
    }

    public string PLAN_YEAR_NOTES
    {
        get
        {
            return this.notes;
        }
        set
        {
            this.notes = value;
        }
    }

    public string PLAN_YEAR_HISTORY
    {
        get
        {
            return this.history;
        }
        set
        {
            this.history = value;
        }
    }

    public DateTime? PLAN_YEAR_MODON
    {
        get
        {
            return this.modOn;
        }
        set
        {
            this.modOn = value;
        }
    }

    public string PLAN_YEAR_MODBY
    {
        get
        {
            return this.modBy;
        }
        set
        {
            this.modBy = value;
        }
    }

    public DateTime? Default_Meas_Start { get; set; }
    public DateTime? Default_Meas_End { get; set; }

    public DateTime? Default_Admin_Start { get; set; }
    public DateTime? Default_Admin_End { get; set; }

    public DateTime? Default_Open_Start { get; set; }
    public DateTime? Default_Open_End { get; set; }

    public DateTime? Default_Stability_Start { get; set; }
    public DateTime? Default_Stability_End { get; set; }

    public int PlanYearGroupId { get; set; }

}