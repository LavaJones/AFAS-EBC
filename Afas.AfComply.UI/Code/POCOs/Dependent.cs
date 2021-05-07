using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Dependent
/// </summary>
public class Dependent
{
    int id = 0;
    int employeeID = 0;
    string fname = null;
    string mname = null;
    string lname = null;
    string ssn = null;
    DateTime? dob = null;

	public Dependent(int _dependentID, int _employeeID, string _fname, string _mname, string _lname, string _ssn, DateTime? _dob)
	{
        this.id = _dependentID;
        this.employeeID = _employeeID;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.ssn = _ssn;
        this.dob = _dob;
	}

    public int DEPENDENT_ID
    {
        get { return this.id; }
        set{ this.id = value;}
    }

    public int DEPENDENT_EMPLOYEE_ID
    {
        get { return this.employeeID; }
        set { this.employeeID = value; }
    }

    public string DEPENDENT_FIRST_NAME
    {
        get { return this.fname; }
        set { this.fname = value; }
    }

    public string DEPENDENT_MIDDLE_NAME
    {
        get { return this.mname; }
        set { this.mname = value; }
    }

    public string DEPENDENT_LAST_NAME
    {
        get { return this.lname; }
        set { this.lname = value; }
    }

    public string DEPENDENT_FULL_NAME
    {
        get { return this.lname + ", " + this.fname; }
    }

    public string DEPENDENT_SSN
    {
        get 
        {
            if (ssn != null)
            {
                return ssn;
            }
            else
            {
                return "n/a";
            }
            
        }
        set { this.ssn = value; }
    }

    public string DEPENDENT_SSN_MASKED
    {
        get
        {
            if (ssn != null)
            {
                return "*****" + ssn.Remove(0, 5); 
            }
            else
            {
                return "n/a";
            }

        }
        set { this.ssn = value; }
    }


    public DateTime? DEPENDENT_DOB
    {
        get { return this.dob; }
        set { this.dob = value; }
    }

}