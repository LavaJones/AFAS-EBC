using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Child Class of Payroll.
/// </summary>
public class Payroll_I : Payroll
{
    private ILog Log = LogManager.GetLogger(typeof(Payroll_I));

    private string fname = null;
    private string mname = null;
    private string lname = null;
    private string i_hours = null;
    private string i_sdate = null;
    private string i_edate = null;
    private string ssn = null;
    private string gp_desc = null;
    private string gp_ext_id = null;
    private string i_cdate = null;
    private string employee_ext_id = null;

	public Payroll_I(int _rowID, int _employerID, int _employeeID, int _batchID, string _fname, string _mname, string _lname, string _ihours, decimal _hours, string _isdate, DateTime? _sdate, string _iedate, DateTime? _edate, string _ssn, string _gpDesc, string _gpExtID, int _gpID, string _icdate, DateTime? _cdate, string _modBy, DateTime _modOn, string _empExtID)
	{
        this.rowID = _rowID;
        this.employerID = _employerID;
        this.employeeID = _employeeID;
        this.batchID = _batchID;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.i_hours = _ihours;
        this.hours = _hours;
        this.i_sdate = _isdate;
        this.sdate = _sdate;
        this.i_edate = _iedate;
        this.edate = _edate;
        this.ssn = _ssn;
        this.gp_desc = _gpDesc;
        this.gp_ext_id = _gpExtID;
        this.gpID = _gpID;
        this.i_cdate = _icdate;
        this.cdate = _cdate;
        this.modBy = _modBy;
        this.modOn = _modOn;
        this.employee_ext_id = _empExtID;
	}

    public string PAY_F_NAME
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

    public string PAY_M_NAME
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

    public string PAY_L_NAME
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
            return this.lname + ", " + this.fname; 
        }
    }

    public string PAY_I_HOURS
    {
        get
        {
            return this.i_hours;
        }
        set
        {
            this.i_hours = value;
        }
    }

    public string PAY_I_SDATE
    {
        get
        {
            return this.i_sdate;
        }
        set
        {
            this.i_sdate = value;
        }
    }

    public string PAY_I_EDATE
    {
        get
        {
            return this.i_edate;
        }
        set
        {
            this.i_edate = value;
        }
    }

    public string PAY_SSN
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

    public string PAY_SSN_HIDDEN
    {
        get
        {
            string tempssn;
            try
            {
                tempssn = "*****" + this.ssn.Remove(0, 5);
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                tempssn = "Error";
            }
            return tempssn;
        }
    }

    public string PAY_GP_DESC
    {
        get
        {
            return this.gp_desc;
        }
        set
        {
            this.gp_desc = value;
        }
    }

    public string PAY_GP_EXT_ID
    {
        get
        {
            return this.gp_ext_id;
        }
        set
        {
            this.gp_ext_id = value;
        }
    }

    public string PAY_I_CDATE
    {
        get
        {
            return this.i_cdate;
        }
        set
        {
            this.i_cdate = value;
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
}