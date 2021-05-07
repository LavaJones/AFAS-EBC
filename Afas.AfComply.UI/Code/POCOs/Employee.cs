using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Afas.AfComply.Domain;

/// <summary>
/// Summary description for Employee
/// </summary>
public class Employee
{
    private ILog Log = LogManager.GetLogger(typeof(Employee));

    /******************************************************************************************************************
    *******************  All Employee Attributes ********************************************************************** 
    ******************************************************************************************************************/
    protected int employee_id = 0;                                
    protected int employee_type_id = 0;                          
    protected int hr_status_id = 0;                               
    protected int employer_id = 0;                              
    protected string fname = null;                             
    protected string mname = null;                             
    protected string lname = null;                             
    protected string address = null;                          
    protected string city = null;                             
    protected int stateID = 0;                                
    protected string zip = null;                              
    protected DateTime? hdate;                                 
    protected DateTime? cdate;                                     
    protected string ssn = null;                                
    protected string employee_ext_id = null;                      
    protected DateTime? tdate = null;                             
    protected DateTime? dob;                                    
    protected DateTime? impEnd;                                      
    protected int planYearID = 0;                                     
    protected int planYearID_limbo = 0;                               
    protected int planYearID_measurement = 0;                      
    
    protected double planYearStabilityAvgHours = 0;                    
    protected double planYearAdminAvgHours = 0;                        
    protected double planYearMeasAvgHours = 0;                         
    protected double planYearInitAvgHours = 0;                          
    protected int classificationID = 0;                              
    protected int actStatusID = 0;                                 

    protected bool recieve1095c = false;                                     

    private int hoursWorked = 0;
    private double avgHoursWorked = 0;
    private double hoursNeeded = 0;
    private double percentMPP = 0;
    private double percentHWP = 0;
    private double percentQT = 0;
    private double monthsLeft = 0;
    private double payPeriodsLeft = 0;
    private double AvgHoursNeeded = 0;
    private Guid resourceId = new Guid();

    public Employee()
    { }

    public Employee(int _employeeID, int _employeeTypeID, int _hrStatusID, int _employerID, string _fname, string _mname, string _lname, string _address, string _city, int _stateID, string _zip, DateTime? _hdate, DateTime? _cdate, string _ssn, string _extEmployeeID, DateTime? _tdate, DateTime? _dob, DateTime? _impEnd, int _pyID_curr, int _pyID_limbo, int _pyID_meas, double _pyAvg, double _pyAvgLimbo, double _pyAvgMeas, double _pyAvgInit, int _classID, int _actID, Guid _resourceId = new Guid())
	{
        this.employee_id = _employeeID;
        this.employee_type_id = _employeeTypeID;
        this.hr_status_id = _hrStatusID;
        this.employer_id = _employerID;
        this.hr_status_id = _hrStatusID;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.address = _address;
        this.city = _city;
        this.stateID = _stateID;
        this.zip = _zip;
        this.hdate = _hdate;
        this.cdate = _cdate;
        this.ssn = _ssn;
        this.employee_ext_id = _extEmployeeID;
        this.tdate = _tdate;
        this.dob = _dob;
        this.impEnd = _impEnd;
        this.planYearID = _pyID_curr;
        this.planYearID_limbo = _pyID_limbo;
        this.planYearID_measurement = _pyID_meas;
        this.planYearStabilityAvgHours = _pyAvg;
        this.planYearAdminAvgHours = _pyAvgLimbo;
        this.planYearMeasAvgHours = _pyAvgMeas;
        this.planYearInitAvgHours = _pyAvgInit;
        this.classificationID = _classID;
        this.actStatusID = _actID;
        this.resourceId = _resourceId;
	}


    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYEE_ID
    {
        get
        {
            return this.employee_id;
        }
        set
        {
            this.employee_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYEE_TYPE_ID
    {
        get
        {
            return this.employee_type_id;
        }
        set
        {
            this.employee_type_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYEE_HR_STATUS_ID
    {
        get
        {
            return this.hr_status_id;
        }
        set
        {
            this.hr_status_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYEE_EMPLOYER_ID
    {
        get
        {
            return this.employer_id;
        }
        set
        {
            this.employer_id = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_FIRST_NAME
    {
        get
        {
            return this.fname;
        }
        set
        {
            this.fname = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_MIDDLE_NAME
    {
        get
        {
            return this.mname;
        }
        set
        {
            this.mname = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_LAST_NAME
    {
        get
        {
            return this.lname;
        }
        set
        {
            this.lname = value;
        }
    }

    public string EMPLOYEE_FULL_NAME
    {
        get
        {
            return this.EMPLOYEE_LAST_NAME + ", " + EMPLOYEE_FIRST_NAME;
        }
    }

    public string EMPLOYEE_FULL_NAME_SSN
    {
        get
        {
            return this.EMPLOYEE_LAST_NAME + ", " + EMPLOYEE_FIRST_NAME + " - " + Employee_SSN_Hidden;
        }
    }

    public string EMPLOYEE_FULL_NAME_ExtID
    {
        get
        {
            return this.EMPLOYEE_LAST_NAME + ", " + EMPLOYEE_FIRST_NAME + " - " + EMPLOYEE_EXT_ID;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_ADDRESS
    {
        get
        {
            return this.address;
        }
        set
        {
            this.address = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_CITY
    {
        get
        {
            return this.city;
        }
        set
        {
            this.city = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int EMPLOYEE_STATE_ID
    {
        get
        {
            return this.stateID;
        }
        set
        {
            this.stateID = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_ZIP
    {
        get
        {
            return this.zip.ZeroPadZip();
        }
        set
        {
            this.zip = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? EMPLOYEE_HIRE_DATE
    {
        get
        {
            return this.hdate;
        }
        set
        {
            this.hdate = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? EMPLOYEE_C_DATE
    {
        get
        {
            return this.cdate;
        }
        set
        {
            this.cdate = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string Employee_SSN_Visible
    {
        get
        {
            return this.ssn.ZeroPadSsn();
        }
        set
        {
            this.ssn = value.ZeroPadSsn();
        }
    }

    public string Employee_SSN_Hidden
    {
        get
        {
            string tempssn;
            try
            {
                tempssn = "*****" + ssn.Remove(0, 5);
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                tempssn = "Error";
            }
            return tempssn;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? EMPLOYEE_TERM_DATE
    {
        get
        {
            return this.tdate;
        }
        set
        {
            this.tdate = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? EMPLOYEE_DOB
    {
        get
        {
            return this.dob;
        }
        set
        {
            this.dob = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EMPLOYEE_EXT_ID
    {
        get
        {
            return this.employee_ext_id;
        }
        set
        {
            this.employee_ext_id = value;
        }
    }

    public double EMPLOYEE_PERCENT_MPP
    {
        get
        {
            return this.percentMPP;
        }
        set
        {
            this.percentMPP = value;
        }
    }

    public double EMPLOYEE_PERCENT_HWP
    {
        get
        {
            return this.percentHWP;
        }
        set
        {
            this.percentHWP = value;
        }
    }

    public double EMPLOYEE_PERCENT_QT
    {
        get
        {
            return this.percentQT;
        }
        set
        {
            this.EMPLOYEE_PERCENT_QT = value;
        }
    }

    public DateTime? EMPLOYEE_IMP_END
    {
        get
        {
            return this.impEnd;
        }
        set
        {
            this.impEnd = value;
        }
    }

    public int EMPLOYEE_PLAN_YEAR_ID
    {
        get
        {
            return this.planYearID;
        }
        set
        {
            this.planYearID = value;
        }
    }

    public int EMPLOYEE_PLAN_YEAR_ID_LIMBO
    {
        get
        {
            return this.planYearID_limbo;
        }
        set
        {
            this.planYearID_limbo = value;
        }
    }

    public int EMPLOYEE_PLAN_YEAR_ID_MEAS
    {
        get
        {
            return this.planYearID_measurement;
        }
        set
        {
            this.planYearID_measurement = value;
        }
    }

    public double EMPLOYEE_AVG_HOURS_WORKED
    {
        get
        {
            return this.avgHoursWorked;
        }
        set
        {
            this.avgHoursWorked = value;
        }
    }

    public int EMPLOYEE_HOURS_WORKED
    {
        get
        {
            return this.hoursWorked;
        }
        set
        {
            this.hoursWorked = value;
        }
    }

    public double EMPLOYEE_PY_AVG_STABILITY_HOURS
    {
        get
        {
            return this.planYearStabilityAvgHours;
        }
        set
        {
            this.planYearStabilityAvgHours = value;
        }
    }

    public double EMPLOYEE_PY_AVG_ADMIN_HOURS
    {
        get
        {
            return this.planYearAdminAvgHours;
        }
        set
        {
            this.planYearAdminAvgHours = value;
        }
    }

    public double EMPLOYEE_PY_AVG_MEAS_HOURS
    {
        get
        {
            return this.planYearMeasAvgHours;
        }
        set
        {
            this.planYearMeasAvgHours = value;
        }
    }

    public double EMPLOYEE_PY_AVG_INIT_HOURS
    {
        get
        {
            return this.planYearInitAvgHours;
        }
        set
        {
            this.planYearInitAvgHours = value;
        }
    }

    public int EMPLOYEE_CLASS_ID
    {
        get
        {
            return this.classificationID;
        }
        set
        {
            classificationID = value;
        }
    }

    public int EMPLOYEE_ACT_STATUS_ID
    {
        get
        {
            return this.actStatusID;
        }
        set
        {
            actStatusID = value;
        }
    }

    /// <summary>
    /// Added 2/17/2017 so the state abbreviation displays instead of the database ID.
    /// </summary>
    public string StateAbbreviation
    {
        get { return StateController.findState(this.stateID).State_Abbr; }
    }

    public bool EMPLOYEE_REC_1095c
    {
        get { return this.recieve1095c; }
        set { this.recieve1095c = value; }
    }

    public Guid ResourceId { get { return resourceId; } set { resourceId = value; } }
}