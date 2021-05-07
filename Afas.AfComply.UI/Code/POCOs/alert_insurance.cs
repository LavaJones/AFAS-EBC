using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for alert_insurance
/// </summary>
public class alert_insurance
{
    private int rowID = 0;                            
    private int employeeID = 0;                       
    private int planyearID = 0;                              
    private int employerID = 0;                       
    private double avgHours = 0;                          
    private bool? offered = null;                         
    private DateTime? offeredOn = null;                 
    private bool? accepted = false;                     
    private DateTime? acceptedOn = null;                 
    private int? insuranceID = null;                    
    private DateTime modOn;                           
    private string modBy = null;                      
    private string notes = null;                      
    private string history = null;                    
    private string extEmpID = null;                   
    private string fullName = null;                   
    private DateTime? effectiveDate = null;           
    private int? contributionID = null;               
    private int? classID = null;                       
    private int? hrStatusID = null;                    
    private int? adminPlanYearID = null;               
    private double? adminAvgHours = null;               
    private double? flexHra = null;                      

    public alert_insurance(int _rowID, int _employeeID, int _planyearID, int _employerID,bool? _offered, DateTime? _offeredOn, bool _accepted, DateTime? _acceptedOn, DateTime _modOn, string _modBy, string _notes, string _history, string _extID, string _fname, string _lname, double _avgHours, DateTime? _effectiveDate, int? _contributionID, int? _insuranceID, int? _classID, int? _hrStatusID, int? _adminPlanYearID, double? _flexHra)
	{
        this.rowID = _rowID;
        this.employeeID = _employeeID;
        this.planyearID = _planyearID;
        this.employerID = _employerID;
        this.offered = _offered;
        this.offeredOn = _offeredOn;
        this.accepted = _accepted;
        this.acceptedOn = _acceptedOn;
        this.modOn = _modOn;
        this.modBy = _modBy;
        this.notes = _notes;
        this.history = _history;
        this.extEmpID = _extID;
        this.fullName = _lname + ", " + _fname;
        this.avgHours = _avgHours;
        this.effectiveDate = _effectiveDate;
        this.contributionID = _contributionID;
        this.insuranceID = _insuranceID;
        this.hrStatusID = _hrStatusID;
        this.classID = _classID;
        this.adminPlanYearID = _adminPlanYearID;
        this.flexHra = _flexHra;
	}

    public int ROW_ID
    {
        get
        {
            return this.rowID;
        }
        set
        {
            this.rowID = value;
        }
    }

    public int IALERT_EMPLOYEEID
    {
        get
        { 
            return this.employeeID;
        }
        set
        {
            this.employeeID = value;
        }
    }

    public int IALERT_PLANYEARID
    {
        get
        {
            return this.planyearID;
        }
        set
        {
            this.planyearID = value;
        }
    }

    public int IALERT_EMPLOYERID
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

    public bool? IALERT_OFFERED
    {
        get
        {
            return this.offered;
        }
        set
        {
            this.offered = value;
        }
    }

    public DateTime? IALERT_OFFERED_ON
    {
        get
        {
            return this.offeredOn;
        }
        set
        {
            this.offeredOn = value;
        }
    }

    public bool? IALERT_ACCEPTED
    {
        get
        {
            return this.accepted;
        }
        set
        {
            this.accepted = value;
        }
    }

    public DateTime? IALERT_ACCEPTEDDATE
    {
        get
        {
            return this.acceptedOn;
        }
        set
        {
            this.acceptedOn = value;
        }
    }

    public DateTime IALERT_MODON
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

    public string IALERT_MODBY
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

    public string IALERT_NOTES
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

    public string IALERT_HISTORY
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

    public string EMPLOYEE_EXT_ID
    {
        get
        {
            return this.extEmpID;
        }
        set
        {
            this.extEmpID = value;
        }
    }

    public string EMPLOYEE_FULL_NAME
    {
        get
        {
            return this.fullName;
        }
        set
        {
            this.fullName = value;
        }
    }

    public double EMPLOYEE_AVG_HOURS
    {
        get
        {
            return this.avgHours;
        }
        set
        {
            this.avgHours = value;
        }
    }

    public DateTime? IALERT_EFFECTIVE_DATE
    {
        get
        {
            return this.effectiveDate;
        }
        set
        {
            this.effectiveDate = value;
        }
    }

    public int? IALERT_CONTRIBUTION_ID
    {
        get
        {
            return this.contributionID;
        }
        set
        {
            this.contributionID = value;
        }
    }

    public int? IALERT_INSURANCE_ID
    {
        get
        {
            return this.insuranceID;
        }
        set
        {
            this.insuranceID = value;
        }
    }

    public int? IALERT_HRSTATUS_ID
    {
        get
        {
            return this.hrStatusID;
        }
        set
        {
            this.hrStatusID = value;
        }
    }

    public int? IALERT_CLASS_ID
    {
        get
        {
            return this.classID;
        }
        set
        {
            this.classID = value;
        }
    }

    public int? IALERT_ADMIN_PLAN_YEARID
    {
        get
        {
            return this.adminPlanYearID;
        }
        set
        {
            this.adminPlanYearID = value;
        }
    }

    public double? IALERT_FLEX_HRA
    {
        get
        {
            return this.flexHra;
        }
        set
        {
            this.flexHra = value;
        }
    }

}