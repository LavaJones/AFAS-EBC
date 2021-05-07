using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for insurance_coverage
/// </summary>
public class insurance_coverage
{

    private ILog Log = LogManager.GetLogger(typeof(insurance_coverage));

    protected int rowID = 0;                        
    protected int employerID = 0;                  
    protected int employeeID = 0;                  
    protected int taxYear = 0;                     
    protected int batchID = 0;                     
    protected string empDependentLink = null;         
    protected int? dependentID = null;             
    protected string fname = null;                 
    protected string mname = null;                 
    protected string lname = null;                 
    protected string ssn = null;                  
    protected DateTime? dob = null;                 
    protected bool all12 = false;
    protected bool january = false;
    protected bool february = false;
    protected bool march = false;
    protected bool april = false;
    protected bool may = false;
    protected bool june = false;
    protected bool july = false;
    protected bool august = false;
    protected bool september = false;
    protected bool october = false;
    protected bool november = false;
    protected bool december = false;
    protected bool subscriber = false;
    protected int carrier_id = 0;
    protected string histroy = null;

    public insurance_coverage()
    { }

	public insurance_coverage(int _rowID, int _batchID, int _employerID, int _employeeID, int _taxYear, string _empDepLink, int? _dependentID, string _fname, string _mname, string _lname, string _ssn, DateTime? _dob, bool _all12, bool _jan, bool _feb, bool _march, bool _april, bool _may, bool _june, bool _july, bool _august, bool _sep, bool _oct, bool _nov, bool _dec, bool _subscriber, int _carrierID, string _history)
	{
        this.rowID = _rowID;
        this.batchID = _batchID;
        this.employerID = _employerID;
        this.employeeID = _employeeID;
        this.taxYear = _taxYear;
        this.empDependentLink = _empDepLink;
        this.dependentID = _dependentID;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.ssn = _ssn;
        this.dob = _dob;
        this.all12 = _all12;
        this.january = _jan;
        this.february = _feb;
        this.march = _march;
        this.april = _april;
        this.may = _may;
        this.june = _june;
        this.july = _july;
        this.august = _august;
        this.september = _sep;
        this.october = _oct;
        this.november = _nov;
        this.december = _dec;
        this.subscriber = _subscriber;
        this.carrier_id = _carrierID;
        this.histroy = _history;
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

    public string EMPLOYEE_FULL_NAME
    {
        get
        {
            return this.lname + ", " + this.fname;
        }
    }

    public string EMPLOYEE_EXT_ID
    {
        get
        {
            return "n/a";
        }
    }

    public int IC_EMPLOYEE_ID
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

    public int IC_TAX_YEAR
    {
        get
        {
            return this.taxYear;
        }
        set
        {
            this.taxYear = value;
        }
    }

    public int IC_EMPLOYER_ID
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

    public string IC_DEPENDENT_EMPLOYEE_LINK
    {
        get
        {
            return this.empDependentLink;
        }
        set
        {
            this.empDependentLink = value;
        }
    }

    public int? IC_DEPENDENT_ID
    {
        get
        {
            return this.dependentID;
        }
        set
        {
            this.dependentID = value;
        }
    }

    public string IC_FIRST_NAME
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

    public string IC_MIDDLE_NAME
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

    public string IC_LAST_NAME
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

    public string IC_FULL_NAME
    {
        get { return this.fname + " " + this.lname; }
    }

    public string IC_SSN_MASKED
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
        set
        {
            this.ssn = value;
        }
    }

    public string IC_SSN
    {
        get
        {
            return this.ssn;
        }
        set
        {
            this.ssn = value;
        }
    }

    public DateTime? IC_DOB
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

    public bool IC_ALL_12
    {
        get
        {
            return this.all12;
        }
        set
        {
            this.all12 = value;
        }
    }

    public bool IC_JAN
    {
        get
        {
            return this.january;
        }
        set
        {
            this.january = value;
        }
    }

    public bool IC_FEB
    {
        get
        {
            return this.february;
        }
        set
        {
            this.february = value;
        }
    }

    public bool IC_MAR
    {
        get
        {
            return this.march;
        }
        set
        {
            this.march = value;
        }
    }

    public bool IC_APR
    {
        get
        {
            return this.april;
        }
        set
        {
            this.april = value;
        }
    }

    public bool IC_MAY
    {
        get
        {
            return this.may;
        }
        set
        {
            this.may = value;
        }
    }

    public bool IC_JUN
    {
        get
        {
            return this.june;
        }
        set
        {
            this.june = value;
        }
    }

    public bool IC_JUL
    {
        get
        {
            return this.july;
        }
        set
        {
            this.july = value;
        }
    }

    public bool IC_AUG
    {
        get
        {
            return this.august;
        }
        set
        {
            this.august = value;
        }
    }

    public bool IC_SEP
    {
        get
        {
            return this.september;
        }
        set
        {
            this.september = value;
        }
    }

    public bool IC_OCT
    {
        get
        {
            return this.october;
        }
        set
        {
            this.october = value;
        }
    }

    public bool IC_NOV
    {
        get
        {
            return this.november;
        }
        set
        {
            this.november = value;
        }
    }


    public bool IC_DEC
    {
        get
        {
            return this.december;
        }
        set
        {
            this.december = value;
        }
    }

    public bool IC_SUBSCRIBER
    {
        get
        {
            return this.subscriber;
        }
        set
        {
            this.subscriber = value;
        }
    }

    public int IC_CARRIER_ID
    {
        get
        {
            return this.carrier_id;
        }
        set
        {
            this.carrier_id = value;
        }
    }


    public int IC_BatchID
    {
        get { return this.batchID; }
        set { this.batchID = value; }
    }

    public string IC_HISTORY
    {
        get { return this.histroy; }
        set { this.histroy = value; }
    }
}