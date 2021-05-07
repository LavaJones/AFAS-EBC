using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for airCoverage
/// </summary>
public class airCoverage
{
    protected int coveredIndividualID = 0;        
    protected int employeeID = 0;                
    protected string fname = null;               
    protected string lname = null;               
    protected string ssn = null;                
    protected DateTime? dob = null;               
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

	public airCoverage(int _coveredIndID, int _employeeID, string _fname, string _lname, string _ssn, DateTime? _dob, bool _jan, bool _feb, bool _mar, bool _apr, bool _may, bool _jun, bool _jul, bool _aug, bool _sep, bool _oct, bool _nov, bool _dec)
	{
        this.coveredIndividualID = _coveredIndID;            
        this.employeeID = _employeeID;
        this.fname = _fname;
        this.lname = _lname;
        this.ssn = _ssn;
        this.dob = _dob;
        this.january = _jan;
        this.february = _feb;
        this.march = _mar;
        this.april = _apr;
        this.may = _may;
        this.june = _jun;
        this.july = _jul;
        this.august = _aug;
        this.september = _sep;
        this.october = _oct;
        this.november = _nov;
        this.december = _dec;
	}

    public int AIC_COVERED_IND_ID
    {
        get { return this.coveredIndividualID; }
        set { this.coveredIndividualID = value; }
    }

    public int AIC_EMPLOYEE_ID
    {
        get { return this.employeeID; }
        set { this.employeeID = value; }
    }

    public string AIC_FIRST_NAME
    {
        get { return this.fname; }
        set { this.fname = value; }
    }

    public string AIC_LAST_NAME
    {
        get { return this.lname; }
        set { this.lname = value; }
    }

    public string AIC_SSN
    {
        get { return this.ssn; }
        set { this.ssn = value; }
    }

    public DateTime? AIC_DOB
    {
        get { return this.dob; }
        set { this.dob = value; }
    }

    public bool AIC_JAN
    {
        get { return this.january; }
        set { this.january = value; }
    }

    public bool AIC_FEB
    {
        get { return this.february; }
        set { this.february = value; }
    }

    public bool AIC_MAR
    {
        get { return this.march; }
        set { this.march = value; }
    }

    public bool AIC_APR
    {
        get { return this.april; }
        set { this.april = value; }
    }

    public bool AIC_MAY
    {
        get { return this.may; }
        set { this.may = value; }
    }

    public bool AIC_JUN
    {
        get { return this.june; }
        set { this.june = value; }
    }

    public bool AIC_JUL
    {
        get { return this.july; }
        set { this.july= value; }
    }

    public bool AIC_AUG
    {
        get { return this.august; }
        set { this.august = value; }
    }

    public bool AIC_SEP
    {
        get { return this.september; }
        set { this.september = value; }
    }

    public bool AIC_OCT
    {
        get { return this.october; }
        set { this.october = value; }
    }

    public bool AIC_NOV
    {
        get { return this.november; }
        set { this.november = value; }
    }

    public bool AIC_DEC
    {
        get { return this.december; }
        set { this.december = value; }
    }
}