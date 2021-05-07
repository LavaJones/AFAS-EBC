using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Payroll
/// </summary>
public class Payroll_E : Payroll
{
    private string fname = null;
    private string mname = null;
    private string lname = null;
    private string gp_ext_id = null;
    private string emp_ext_id = null;

    public Payroll_E()
    { }

	public Payroll_E(int _rowID, int _employerID,int _batchID, int _employeeID, decimal _hours, DateTime? _sdate, DateTime? _edate, int _gpID, DateTime? _cdate, string _modBy, DateTime _modOn, string _desc, string _history, string _fname, string _mname, string _lname, string _gpExtid, string _empExtID)
	{
        this.rowID = _rowID;
        this.employerID = _employerID;
        this.employeeID = _employeeID;
        this.batchID = _batchID;
        this.hours = _hours;
        this.sdate = _sdate;
        this.edate = _edate;
        this.gpID = _gpID;
        this.cdate = _cdate;
        this.modBy = _modBy;
        this.modOn = _modOn;
        this.desc = _desc;
        this.history = _history;
        this.fname = _fname;
        this.mname = _mname;
        this.lname = _lname;
        this.gp_ext_id = _gpExtid;
        this.emp_ext_id = _empExtID;
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

    public string PAY_EMPLOYEE_EXT_ID
    {
        get
        {
            return this.emp_ext_id;
        }
        set
        {
            this.emp_ext_id = value;
        }
    }
}